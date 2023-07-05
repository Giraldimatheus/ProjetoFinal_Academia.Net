namespace ProjetoFinal_Livros.Models
{

    public enum LivroEnum
    {
        Detalhe = 1,
        Listagem = 2,
        Comentario = 3,
    }
    public class LivroDto
    {
        public string Titulo { get; set; }
        public string Capa { get; set; }
        public string Autor { get; set; }
        public string Editora { get; set; }
        public string DataPublicacao { get; set; }
        public string Descricao { get; set; }
        public string link { get; set; }
        public string linkCompra { get; set; }
        public string Id { get; set; }
        public List<LivroComentario> Comentarios { get; set; } = new List<LivroComentario>();

        public bool Favorito { get; set; }
    }
}
