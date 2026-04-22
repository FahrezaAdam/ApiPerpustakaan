namespace ApiPerpustakaan.Models
{
    public class DtoRegister
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DtoLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Pengguna
    {
        public int IdPengguna { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Peran { get; set; }
    }
}