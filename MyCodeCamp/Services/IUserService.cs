using System.Collections.Generic;
using MyCodeCamp.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace MyCodeCamp.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        bool HasTokenExpired(string token);
        JwtSecurityToken LoadToken(string token);
        ObjectResult Refresh(string token, string refreshToken);
    }
}
