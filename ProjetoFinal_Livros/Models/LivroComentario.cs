using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal_Livros.Models
{
    public class LivroComentario
    {
        [Key]
        public int Id { get; set; }
        public string IdLivro { get; set; }
        public string Texto { get; set; }
        [DefaultValue(0)]
        public int ComentarioPaiId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
