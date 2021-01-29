using GitHubApp.Extensions;
using GitHubApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using X.PagedList;

namespace GitHubApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("Index", await HttpContext.GetExternalProvidersAsync());
        }
                
        [HttpGet("~/Home/UsuariosGit")]
        public async Task<IActionResult> UsuariosGit([FromServices] IConfiguration config, string state, string login, int? pagina)
        {

            const int itensPorPagina = 10;
            int numPage = (pagina ?? 1);

            List<UsuariosGithub> listaUsuarios = new List<UsuariosGithub>();


            if (string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(login))
            {
                IEnumerable<RepositoriosUsuario> ret  = RepoUsuarioGit(config, login);
                ViewBag.Usuario = login.ToUpper();
                ViewBag.Data = ret;
                return View();
            }


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string baseUrl = config.GetSection("GithubProject.API.Users:BaseURL").Value;

                HttpResponseMessage response = client.GetAsync(baseUrl).Result;
                response.EnsureSuccessStatusCode();

                string conteudo = response.Content.ReadAsStringAsync().Result;

                dynamic resultado = JsonConvert.DeserializeObject(conteudo);

                foreach (var item in resultado)
                {
                    listaUsuarios.Add(new UsuariosGithub()
                    {
                        id = item.id,
                        login = item.login,
                        repos_url = item.repos_url,
                        avatar_url = item.avatar_url
                    });
                }
            }

            return View(await listaUsuarios.ToPagedListAsync(numPage, itensPorPagina));
        }


        public IEnumerable<RepositoriosUsuario> RepoUsuarioGit([FromServices] IConfiguration config, string login)
        {
            List<RepositoriosUsuario> listaRepos = new List<RepositoriosUsuario>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string baseUrl = config.GetSection("GithubProject.API.Repos:BaseURL").Value;
                string url = baseUrl + login;

                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                string conteudo = response.Content.ReadAsStringAsync().Result;

                dynamic resultado = JsonConvert.DeserializeObject(conteudo);

                foreach (var item in resultado)
                {
                    listaRepos.Add(new RepositoriosUsuario()
                    {
                        id = item.id,
                        name = item.name,
                        html_url = item.html_url,
                        repo_private = item.repo_private
                    });
                }
            }

            return listaRepos;
        }
    }
}
