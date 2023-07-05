using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoFinal_Livros.Models;
using System;
using System.Linq;
namespace ProjetoFinal_Livros.Controllers
{

    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                using (var context = new Context())
                {
                    // Verificar se o login já está sendo usado
                    var usuarioExistente = context.Usuarios.FirstOrDefault(u => u.Login == usuario.Login);
                    if (usuarioExistente != null)
                    {
                        ModelState.AddModelError(nameof(usuario.Login), "O login informado já está sendo usado.");
                        return View("Cadastro", usuario);
                    }

                    context.Usuarios.Add(usuario);
                    context.SaveChanges();
                }

                return RedirectToAction("Login", "Usuario");
            }

            return View("Cadastro", usuario);
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LivrosFavoritos()
        {
            int? usuarioId = HttpContext.Session.GetInt32("SessionKeySessionId");
            if (usuarioId.HasValue)
            {
                using (var context = new Context())
                {
                    var usuario = context.Usuarios.Where(u => u.Id == usuarioId.Value);

                    var livrosFavoritos = context.LivrosFavoritos.Where(l => l.usuario.Id == usuarioId).ToList();
                    return View("~/Views/Livro/LivrosFavoritos.cshtml", livrosFavoritos);

                }
            }
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> SalvarLivroFavorito(string id)
        {
            string apiUrl = $"https://www.googleapis.com/books/v1/volumes/{id}?key=AIzaSyD77VftTjUq3GRum_17beyRmVMbWSg64hs";

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
                    string capa = livroJson.volumeInfo.imageLinks?.small;

                    //Obter o Id do usuario da sessao
                    int? usuarioId = HttpContext.Session.GetInt32("SessionKeySessionId");

                    if (usuarioId.HasValue)
                    {
                        using (var context = new Context())
                        {
                            var usuario = context.Usuarios.FirstOrDefault(u => u.Id == usuarioId.Value);

                            LivrosFavoritos livro = new LivrosFavoritos
                            {
                                IdLivro = id,
                                Titulo = titulo,
                                Autor = autor,
                                Capa = capa,
                                usuario = usuario,
                            };

                            context.LivrosFavoritos.Add(livro);
                            context.SaveChanges();
                            return Json("success");

                        }
                    }
                    else
                    {
                        return View("Login");
                    }

                }
            }

            return NotFound("Livro não encontrado.");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string senha)
        {
            try
            {
                using (var context = new Context())
                {
                    var usuario = context.Usuarios.FirstOrDefault(u => u.Login == login);
                    if (usuario != null)
                    {
                        if (usuario.Senha == senha)
                        {
                            HttpContext.Session.SetInt32("SessionKeySessionId", usuario.Id);

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ViewBag.ErroLogin = "Credenciais inválidas. Tente novamente.";
                return View("Login");
            }
            catch (Exception ex)
            {
                ViewBag.ErroLogin = "Ocorreu um erro durante o login";
                return View("Login");
            }
        }

    }
}
