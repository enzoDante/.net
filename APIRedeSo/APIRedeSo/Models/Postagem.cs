namespace APIRedeSo.Models
{
    public class Postagem
    {
        public int Id { get; set; }
        public int Usuario_id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data_criacao { get; set; } = DateTime.Now;
        public int Curtidas { get; set; }
        public List<Fotos_postagem> Fotos { get; set; } = new List<Fotos_postagem>();
        public List<Postagem_usuario_relacionado> Colaboradores { get; set; } = new List<Postagem_usuario_relacionado>();

    }
}
