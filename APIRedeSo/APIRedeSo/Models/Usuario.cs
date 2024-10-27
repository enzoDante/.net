namespace APIRedeSo.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int Is_admin { get; set; }
        public DateTime Data_criacao { get; set; }
    }
}
