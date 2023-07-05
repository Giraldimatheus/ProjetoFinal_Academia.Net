using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoFinal_Livros.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public string Genero { get; set; }
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage ="Digite o Login.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Digite a Senha.")]
        public string Senha { get; set; }
        public int Idade { get; set; }

        public virtual ICollection<LivroComentario>? LivrosComentario { get; set; }
        
        public virtual ICollection<LivrosFavoritos>? LivrosFavoritos { get; set; }

        public Usuario()
        {

        }


    }
}
