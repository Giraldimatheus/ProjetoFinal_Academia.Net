using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;

namespace ProjetoFinal_Livros.Models
{
    public class FUNCAOLivro
    {

        public static async Task<LivroDto> RetornaLivro(string id, LivroEnum tipoLivro)
        {
            string apiUrl = $"https://www.googleapis.com/books/v1/volumes/{id}?key=AIzaSyD77VftTjUq3GRum_17beyRmVMbWSg64hs";

            var livro = new LivroDto();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic livroJson = JsonConvert.DeserializeObject(jsonResponse);

                PreencheLivro(tipoLivro, livro, livroJson);
            }

            return livro;
        }

        private static void PreencheLivro(LivroEnum tipoLivro, LivroDto livro, dynamic livroJson)
        {
            if (tipoLivro == LivroEnum.Comentario)
            {
                livro.Id = livroJson.id;
            }
            else if (tipoLivro == LivroEnum.Listagem)
            {
                livro.Id = livroJson.id;
                livro.Titulo = livroJson.volumeInfo.title;
                livro.Autor = livroJson.volumeInfo.authors[0];
                livro.Editora = livroJson.volumeInfo.publisher;
                livro.Capa = livroJson.imageLinks?.thumbnail;
            }
            else if (tipoLivro == LivroEnum.Detalhe)
            {
                livro.Id = livroJson.id;
                livro.Titulo = livroJson.volumeInfo.title;
                livro.Autor = livroJson.volumeInfo.authors[0];
                livro.Editora = livroJson.volumeInfo.publisher;
                livro.Capa = livroJson.imageLinks?.thumbnail;
                livro.DataPublicacao = livroJson.volumeInfo.publishedDate;
                livro.Descricao = livroJson.volumeInfo.description;
            }
        }
    }
}
