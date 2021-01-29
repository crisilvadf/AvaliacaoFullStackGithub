using GitHubApp.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GitHubApp.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpPost("~/login")]
        public async Task<IActionResult> Index([FromForm] string provider)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            if (!await HttpContext.IsProviderSupportedAsync(provider))
            {
                return BadRequest();
            }

            return Challenge(new AuthenticationProperties { RedirectUri = "/Home/UsuariosGit" }, provider);
        }
    }
}
