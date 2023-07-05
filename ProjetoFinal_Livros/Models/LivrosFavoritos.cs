using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProjetoFinal_Livros.Models
{
    public class LivrosFavoritos
    {
        [Key]
        public int Id { get; set; }
        public string IdLivro { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Capa { get; set; }

        public virtual Usuario usuario { get; set; }

    }
}
