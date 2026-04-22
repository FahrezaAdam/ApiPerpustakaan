using System.Data;
using Npgsql; 

namespace ApiPerpustakaan.Helpers
{
    public class SqlDbHelper
    {
        private readonly string _stringKoneksi;

        public SqlDbHelper(IConfiguration konfigurasi)
        {
            _stringKoneksi = konfigurasi.GetConnectionString("KoneksiPerpustakaan");
        }

        public IDbConnection BuatKoneksi()
        {
            return new NpgsqlConnection(_stringKoneksi);
        }
    }
}