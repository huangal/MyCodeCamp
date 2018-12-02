using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Models;
using MyCodeCamp.Services;

namespace MyCodeCamp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService userService)
        {
            _authService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _authService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("Token/expired")]
        public IActionResult AuthenticateToken([FromBody]string token)
        {
            var expired = _authService.HasTokenExpired(token);

            return Ok(expired);
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public IActionResult LoadToken([FromBody]string token)
        {
            var securityToken = _authService.LoadToken(token);

            return Ok(securityToken);
        }

        [AllowAnonymous]
        [HttpPost("Token/Refresh")]
        public IActionResult Refresh([FromBody]string token, string refreshToken)
        {
            var secureToken = _authService.Refresh(token, refreshToken);

            return Ok(secureToken);
        }
    }
}
