namespace APIRedeSo.Models
{
    public class Postagem_usuario_relacionado
    {
        public int Id { get; set; }
        public int Postagem_id { get; set; }
        public int Usuario_id { get; set; }
        public Postagem Postagem { get; set; }
    }
}
