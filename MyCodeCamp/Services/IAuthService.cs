using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Models;

namespace MyCodeCamp.Services
{
    public interface IAuthService
    {
        User Authenticate(string username, string password);
        bool HasTokenExpired(string token);
        JwtSecurityToken LoadToken(string token);
        ObjectResult Refresh(string token, string refreshToken);
    }



    //public interface IRsaKeyProvider
    //{
    //    string GenerateToken();
    //    bool ValidateToken(string tokenString);
    //}
}
