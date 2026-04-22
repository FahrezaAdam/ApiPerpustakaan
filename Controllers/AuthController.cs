using ApiPerpustakaan.Helpers;
using ApiPerpustakaan.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiPerpustakaan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SqlDbHelper _dbHelper;
        private readonly IConfiguration _konfigurasi;

        public AuthController(SqlDbHelper dbHelper, IConfiguration konfigurasi)
        {
            _dbHelper = dbHelper;
            _konfigurasi = konfigurasi;
        }

        // ENDPOINT 1: REGISTER
        [HttpPost("register")]
        public IActionResult Register([FromBody] DtoRegister request)
        {
            try
            {
                using IDbConnection koneksi = _dbHelper.BuatKoneksi();

                // Cek apakah username sudah ada
                string cekSql = "SELECT COUNT(1) FROM pengguna WHERE username = @Username";
                int userAda = koneksi.ExecuteScalar<int>(cekSql, new { Username = request.Username });

                if (userAda > 0) return BadRequest(new ResponStandar<object> { Status = 400, Pesan = "Username sudah digunakan", Data = null });

                // Hash password menggunakan BCrypt
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                string insertSql = "INSERT INTO pengguna (username, password_hash) VALUES (@Username, @PasswordHash)";
                koneksi.Execute(insertSql, new { Username = request.Username, PasswordHash = passwordHash });

                return StatusCode(201, new ResponStandar<object> { Status = 201, Pesan = "Registrasi berhasil", Data = null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponStandar<object> { Status = 500, Pesan = ex.Message, Data = null });
            }
        }

        // ENDPOINT 2: LOGIN
        [HttpPost("login")]
        public IActionResult Login([FromBody] DtoLogin request)
        {
            try
            {
                using IDbConnection koneksi = _dbHelper.BuatKoneksi();

                // Cari user berdasarkan username
                string sql = "SELECT id_pengguna AS IdPengguna, username AS Username, password_hash AS PasswordHash, peran AS Peran FROM pengguna WHERE username = @Username";
                var user = koneksi.QueryFirstOrDefault<Pengguna>(sql, new { Username = request.Username });

                // Verifikasi keberadaan user dan kecocokan hash password
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new ResponStandar<object> { Status = 401, Pesan = "Username atau Password salah", Data = null });
                }

                // Jika benar, buat Token JWT
                var jwtSettings = _konfigurasi.GetSection("JwtSettings");
                var secretKey = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.IdPengguna.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Peran)
                    }),
                    Expires = DateTime.UtcNow.AddHours(2), 
                    Issuer = jwtSettings["Issuer"],
                    Audience = jwtSettings["Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                string tokenString = tokenHandler.WriteToken(token);

                return Ok(new ResponStandar<object>
                {
                    Status = 200,
                    Pesan = "Login berhasil",
                    Data = new { Token = tokenString, Peran = user.Peran }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponStandar<object> { Status = 500, Pesan = ex.Message, Data = null });
            }
        }
    }
}