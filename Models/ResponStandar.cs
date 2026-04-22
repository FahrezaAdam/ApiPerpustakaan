namespace ApiPerpustakaan.Models
{
    public class ResponStandar<T>
    {
        public int Status { get; set; }
        public string Pesan { get; set; }
        public T Data { get; set; }
    }
}