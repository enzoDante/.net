using System.ComponentModel.DataAnnotations.Schema;

namespace APIRedeSo.Models
{
    public class Postagem_usuario_relacionado
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("Postagem_id")]
        public int Postagem_id { get; set; }
        public int Usuario_id { get; set; }
        public Postagem Postagem { get; set; }
    }
}
