using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIRedeSo.Models
{
    public class Fotos_postagem
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("Postagem_id")]
        public int Postagem_id { get; set; }
        public string Url_foto { get; set; }
        //[JsonIgnore] // Ignora para evitar ciclo
        public Postagem Postagem { get; set; }
    }
}
