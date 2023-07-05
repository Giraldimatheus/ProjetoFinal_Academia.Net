using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoFinal_Livros.Models;

namespace ProjetoFinal_Livros.Controllers
{
    public class LivroController : Controller
    {
        private readonly string apiKey = "AIzaSyD77VftTjUq3GRum_17beyRmVMbWSg64hs";

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> PesquisarLivros(string titulo)
        {
            string apiUrl = $"https://www.googleapis.com/books/v1/volumes?q={titulo}&key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                    var livros = new List<LivroDto>();

                    foreach (var item in result.items)
                    {

                        if (item.volumeInfo.imageLinks?.thumbnail != null)
                        {

                            var livro = new LivroDto
                            {
                                Id = item.id,
                                Titulo = item.volumeInfo.title,
                                Autor = item.volumeInfo.authors[0],
                                Editora = item.volumeInfo.publisher,
                                Capa = item.volumeInfo.imageLinks?.thumbnail
                            };
                            livros.Add(livro);
                        }
                    }
                    return View("Index", livros);
                }
                else
                {
                    ViewBag.ErroPesquisa = "Nenhum livro encontrado.";
                    return View("Index", null);
                }
            }

        }

        public async Task<IActionResult> DetalhesLivro(string id)
        {
            string apiUrl = $"https://www.googleapis.com/books/v1/volumes/{id}?key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    dynamic livroJson = JsonConvert.DeserializeObject(jsonResponse);

                    string idLivro = livroJson.id;
                    string titulo = livroJson.volumeInfo.title;
                    string autor = livroJson.volumeInfo.authors[0];
                    string editora = livroJson.volumeInfo.publisher;
                    string capa = livroJson.volumeInfo.imageLinks?.small;
                    string dataPublicacao = livroJson.volumeInfo.publishedDate;
                    string descricao = livroJson.volumeInfo.description;
                    string link = livroJson.accessInfo.webReaderLink;
                    string linkCompra = livroJson.saleInfo.buyLink;

                    LivroDto livro = new LivroDto
                    {
                        Id = idLivro,
                        Titulo = titulo,
                        Autor = autor,
                        Capa = capa,
                        Editora = editora,
                        DataPublicacao = dataPublicacao,
                        Descricao = descricao,
                        link = link,
                        linkCompra = linkCompra
                    };

                    using (var context = new Context())
                    {
                        var comentarios = await context
                            .LivroComentarios
                            .Where(l => l.IdLivro == idLivro)
                            .ToListAsync();

                        if (comentarios.Count > 0)
                        {
                            livro.Comentarios.AddRange(comentarios);
                        }


                        livro.Favorito = context
                            .LivrosFavoritos
                            .Select(p => p.IdLivro)
                            .Where(p => p.Contains(id))
                            .Any();
                    }

                    return View(livro);
                }
                else
                {
                    return NotFound("Não encontrado.");
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> AdicionarComentario(string id, string comentario)
        {
            var livro = await FUNCAOLivro.RetornaLivro(id, LivroEnum.Comentario);
            if (livro.Id != "" && !String.IsNullOrEmpty(livro.Id))
            {
                int? usuarioId = HttpContext.Session.GetInt32("SessionKeySessionId");
                if (usuarioId.HasValue)
                {
                    using (var context = new Context())
                    {
                        var usuario = context.Usuarios.FirstOrDefault(u => u.Id == usuarioId.Value);
                        if (usuario != null)
                        {
                            var novoComentario = new LivroComentario
                            {
                                IdLivro = id,
                                Texto = comentario,
                                Usuario = usuario
                            };

                            context.LivroComentarios.Add(novoComentario);
                            context.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("DetalhesLivro", new { id = id });

        }

        [HttpGet]
        public async Task<IActionResult> DeletarComentario(string id, int comentarioId)
        {
            using (var context = new Context())
            {
                var comentario = await context.LivroComentarios.FindAsync(comentarioId);
                if (comentario != null)
                {
                    context.LivroComentarios.Remove(comentario);
                    await context.SaveChangesAsync();
                }
            }

            return RedirectToAction("DetalhesLivro", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> EditarComentario(string id, int comentarioId, string novoTexto)
        {
            using (var context = new Context())
            {
                var comentario = await context.LivroComentarios.FindAsync(comentarioId);
                if (comentario != null)
                {
                    comentario.Texto = novoTexto;
                    await context.SaveChangesAsync();
                }
            }

            return RedirectToAction("DetalhesLivro", new { id = id });
        }

        [HttpDelete]
        public IActionResult RemoverLivroFavorito(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "ID inválido." });
            }

            using (var context = new Context())
            {
                var livroFavorito = context.LivrosFavoritos.FirstOrDefault(l => l.Id.ToString() == id);

                if (livroFavorito == null)
                {
                    return Json(new { success = false, message = "Livro favoritado não encontrado." });
                }

                context.LivrosFavoritos.Remove(livroFavorito);
                context.SaveChanges();
            }

            return Json(new { success = true });
        }

    }
}
