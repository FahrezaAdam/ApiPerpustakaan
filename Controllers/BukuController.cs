using ApiPerpustakaan.Models;
using ApiPerpustakaan.Helpers; 
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ApiPerpustakaan.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class BukuController : ControllerBase
    {
        private readonly SqlDbHelper _dbHelper;
        public BukuController(SqlDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // 1. GET: api/buku
        [HttpGet]
        public IActionResult AmbilSemuaBuku()
        {
            try
            {
                using IDbConnection koneksi = _dbHelper.BuatKoneksi(); 
                string sql = "SELECT id_buku AS IdBuku, judul AS Judul, penulis AS Penulis, tahun_terbit AS TahunTerbit FROM buku WHERE dihapus_pada IS NULL";
                var daftarBuku = koneksi.Query<Buku>(sql);

                return Ok(new ResponStandar<IEnumerable<Buku>>
                {
                    Status = 200,
                    Pesan = "Berhasil mengambil data buku",
                    Data = daftarBuku
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponStandar<object> { Status = 500, Pesan = $"Error server: {ex.Message}", Data = null });
            }
        }

        // 2. GET: api/buku/{id}
        [HttpGet("{id}")]
        public IActionResult AmbilBukuBerdasarkanId(int id)
        {
            try
            {
                using IDbConnection koneksi = _dbHelper.BuatKoneksi(); 
                string sql = "SELECT id_buku AS IdBuku, judul AS Judul, penulis AS Penulis, tahun_terbit AS TahunTerbit FROM buku WHERE id_buku = @Id AND dihapus_pada IS NULL";
                var buku = koneksi.QueryFirstOrDefault<Buku>(sql, new { Id = id });

                if (buku == null) return NotFound(new ResponStandar<object> { Status = 404, Pesan = "Buku tidak ditemukan", Data = null });

                return Ok(new ResponStandar<Buku> { Status = 200, Pesan = "Buku ditemukan", Data = buku });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponStandar<object> { Status = 500, Pesan = ex.Message, Data = null });
            }
        }

        // 3. POST: api/buku
        [HttpPost]
        public IActionResult TambahBuku([FromBody] Buku bukuBaru)
        {
            try
            {
                using IDbConnection koneksi = _dbHelper.BuatKoneksi(); 
                string sql = "INSERT INTO buku (judul, penulis, tahun_terbit) VALUES (@Judul, @Penulis, @TahunTerbit)";

                int barisTerpengaruh = koneksi.Execute(sql, bukuBaru);

                if (barisTerpengaruh > 0)
                    return StatusCode(201, new ResponStandar<object> { Status = 201, Pesan = "Buku berhasil ditambahkan", Data = null });

                return BadRequest(new ResponStandar<object> { Status = 400, Pesan = "Gagal menambahkan buku", Data = null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponStandar<object> { Status = 500, Pesan = ex.Message, Data = null });
            }
        }

        // 4. PUT: api/buku/{id}
        [HttpPut("{id}")]
        public IActionResult PerbaruiBuku(int id, [FromBody] Buku dataPerbarui)
        {
            try
            {
                using IDbConnection koneksi = _dbHelper.BuatKoneksi();
                string sql = "UPDATE buku SET judul = @Judul, penulis = @Penulis, tahun_terbit = @TahunTerbit, diperbarui_pada = CURRENT_TIMESTAMP WHERE id_buku = @IdBuku AND dihapus_pada IS NULL";

                dataPerbarui.IdBuku = id;
                int barisTerpengaruh = koneksi.Execute(sql, dataPerbarui);

                if (barisTerpengaruh == 0) return NotFound(new ResponStandar<object> { Status = 404, Pesan = "Buku tidak ditemukan atau sudah dihapus", Data = null });

                return Ok(new ResponStandar<object> { Status = 200, Pesan = "Data buku berhasil diperbarui", Data = null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponStandar<object> { Status = 500, Pesan = ex.Message, Data = null });
            }
        }

        // 5. DELETE: api/buku/{id}
        [HttpDelete("{id}")]
        public IActionResult HapusBuku(int id)
        {
            try
            {
                using IDbConnection koneksi = _dbHelper.BuatKoneksi(); 
                string sql = "UPDATE buku SET dihapus_pada = CURRENT_TIMESTAMP WHERE id_buku = @Id AND dihapus_pada IS NULL";

                int barisTerpengaruh = koneksi.Execute(sql, new { Id = id });

                if (barisTerpengaruh == 0) return NotFound(new ResponStandar<object> { Status = 404, Pesan = "Buku tidak ditemukan", Data = null });

                return Ok(new ResponStandar<object> { Status = 200, Pesan = "Buku berhasil dihapus", Data = null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponStandar<object> { Status = 500, Pesan = ex.Message, Data = null });
            }
        }
    }
}