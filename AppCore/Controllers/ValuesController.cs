using Microsoft.AspNetCore.Mvc;
using LoginService;
using LoginService.Repository;
using LoginService.Models;

namespace AppCore.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpPost]
        public IActionResult Login([FromBody] User usuario)
        {
            User u = new UserRepository().GetUser(usuario.Username);

            if (u == null)
                return NotFound("The user was not found.");

            bool credentials = u.Password.Equals(usuario.Password);

            if (!credentials)
                return Forbid("The username/password combination was wrong.");

            return Ok(TokenManager.GenerateToken(usuario.Username));
        }

        [HttpGet]
        public IActionResult Validate(string token, string username)
        {
            bool exists = new UserRepository().GetUser(username) != null;
            if (!exists)
                return NotFound("The user was not found.");

            string tokenUsername = TokenManager.ValidateToken(token);

            if (username.Equals(tokenUsername))
                return Ok();

            return BadRequest();
        }
    }
}