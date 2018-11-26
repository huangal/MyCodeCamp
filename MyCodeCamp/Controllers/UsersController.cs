using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Models;
using MyCodeCamp.Services;

namespace MyCodeCamp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }




        [AllowAnonymous]
        [HttpPost("Token/{token}/expired")]
        public IActionResult AuthenticateToken(string token)
        {
            var expired = _userService.HasTokenExpired(token);

            return Ok(expired);
        }

        [AllowAnonymous]
        [HttpPost("Token")]
        public IActionResult LoadToken([FromBody]string token)
        {
            var securityToken = _userService.LoadToken(token);

            return Ok(securityToken);
        }



        [AllowAnonymous]
        [HttpPost]
        public IActionResult Refresh(string token, string refreshToken)
        {
            var secureToken = _userService.Refresh(token, refreshToken);

            return Ok(secureToken);
        }
    }
}
