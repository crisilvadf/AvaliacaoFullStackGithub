using GithubProject.API.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GithubProject.API.Controllers
{
    /// <summary>
    /// Controller responsável que contém os metódos para conectar com o GitHub
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        /// <summary>
        /// Esse método retorna a lista de usuários do GitHub
        /// </summary>
        [HttpGet]
        [Route("UsuariosGit")]
        public IEnumerable<UsuariosGithub> GetUsuariosGit()
        {
            RestClient client = new RestClient($"http://api.github.com/users");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("accept", "application/vnd.github.v3+json");

            IRestResponse resultado = client.Execute(request);

            List<UsuariosGithub> response = JsonConvert.DeserializeObject<List<UsuariosGithub>>(resultado.Content);

            return response;
        }

        /// <summary>
        /// Método que retorna a lista de repositórios de um determinado usuario do GitHub
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("RepoUsuarioGit")]
        public IEnumerable<RepositoriosUsuario> GetRepoUsuarioGit(string login)
        {
            RestClient client = new RestClient($"https://api.github.com/users/{login}/repos");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("accept", "application/vnd.github.v3+json");

            IRestResponse resultado = client.Execute(request);

            List<RepositoriosUsuario> response = JsonConvert.DeserializeObject<List<RepositoriosUsuario>>(resultado.Content);

            return response;
        }
    }
}
