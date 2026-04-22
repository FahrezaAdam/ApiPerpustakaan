namespace ApiPerpustakaan.Models
{
    public class Buku
    {
        public int IdBuku { get; set; }
        public string Judul { get; set; }
        public string Penulis { get; set; }
        public int TahunTerbit { get; set; }
    }
}