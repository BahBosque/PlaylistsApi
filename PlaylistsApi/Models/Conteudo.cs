namespace PlaylistsApi.Models
{
    public class Conteudo
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public Criador? Criador { get; set; }
    }

}
