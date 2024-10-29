namespace APIRedeSo.Models
{
    public class Fotos_postagem
    {
        public int Id { get; set; }
        public int Postagem_id { get; set; }
        public string Url_foto { get; set; }
        public Postagem Postagem { get; set; }
    }
}
