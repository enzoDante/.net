namespace APIRedeSo.Models
{
    public class PostagemCreateDTO
    {
        public int Usuario_id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data_criacao { get; set; } = DateTime.Now;
        public int Curtidas { get; set; } = 0;
    }
}
