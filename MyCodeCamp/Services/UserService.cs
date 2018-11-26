using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCodeCamp.Models;

namespace MyCodeCamp.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenSettings _tokenSettings;
        private readonly ITokenService _tokenService;

        public UserService( ITokenSettings tokenSettings, ITokenService tokenService)
        {
            _tokenSettings = tokenSettings;
            _tokenService = tokenService;
        }
        public User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null) return null;
            // remove password before returning
            user.Password = null;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, $"{user.Id}{user.Username}{user.FirstName}"),
                new Claim(JwtRegisteredClaimNames.Sub, user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, "henry@huangal.com"),
                new Claim(ClaimTypes.Role, "IsContributor"),
                new Claim( "IsContributor", "true"),
                new Claim("IsReader", "true"),
                new Claim("IsAdmin", "false")
            };

            user.Token = _tokenService.GenerateToken(claims, DateTime.UtcNow.AddMinutes(2), _tokenSettings.SecretKey);    
            return user;
        }


        public bool HasTokenExpired(string token)
        {
            var principal = _tokenService.GetPrincipalFromToken(token);

            return _tokenService.HasTokenExpired(token);
        }

        public IEnumerable<User> GetAll()
        {
            // return users without passwords
            return _users.Select(x => { x.Password = null; return x; });
        }

        public JwtSecurityToken LoadToken(string token)
        {
            return _tokenService.LoadToken(token);
        }


        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Henry", LastName = "Huangal", Username = "test", Password = "test" }
        };




        public ObjectResult Refresh(string token, string refreshToken)
        {
            var principal =   _tokenService.GetPrincipalFromToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = GetRefreshToken(username); //retrieve the refresh token from a data store
            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = _tokenService.GenerateToken(principal.Claims, DateTime.UtcNow.AddMinutes(2), _tokenSettings.SecretKey);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            DeleteRefreshToken(username, refreshToken);
            SaveRefreshToken(username, newRefreshToken);

            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        private void DeleteRefreshToken(string username, string refreshToken)
        {
            //throw new NotImplementedException();
        }

        private void SaveRefreshToken(string username, string newRefreshToken)
        {
            //throw new NotImplementedException();
        }

        private string GetRefreshToken(string username)
        {
            //Get from session
            //throw new NotImplementedException();
            return "abcde";
        }
    }
}
