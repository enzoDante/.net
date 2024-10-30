namespace APIRedeSo.Models
{
    public class PostagemUpdateDTO
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public List<FotosDTO> Fotos { get; set; }
        public List<ColaboradoresDTO> Colaboradores { get; set; }
    }
}
