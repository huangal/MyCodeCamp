using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCodeCamp.Models;

namespace MyCodeCamp.Services
{
    public class AuthService: IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthService( ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }
        public User Authenticate(string username, string password)
        {
            var user = _userService.GetUser(username, password);

            // return null if user not found
            if (user == null) return null;
            //context.Result = new UnauthorizedResult();
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

            user.Token = _tokenService.GenerateToken(claims, DateTime.UtcNow.AddMinutes(2));
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            return user;
        }


        public bool HasTokenExpired(string token)
        {
            return _tokenService.HasTokenExpired(token);
        }

     
        public JwtSecurityToken LoadToken(string token)
        {
            return _tokenService.LoadToken(token);
        }

        public ObjectResult Refresh(string token, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = GetRefreshToken(username); //retrieve the refresh token from a data store
            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = _tokenService.GenerateToken(principal.Claims, DateTime.UtcNow.AddMinutes(2));
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
