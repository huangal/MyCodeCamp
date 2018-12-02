using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using MyCodeCamp.Models;


namespace MyCodeCamp.Services
{
    public class RsaTokenService : ITokenService
    {
        private readonly ITokenSettings _tokenSettings;

        public RsaTokenService(ITokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }

        public string GenerateToken(IEnumerable<Claim> claims, DateTime utcExpiration, object privateKey = null)
        {
            if (privateKey == null) privateKey = _tokenSettings.RsaPrivateKey;

            var securityKey = new RsaSecurityKey((RSA)privateKey);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            return GenerateToken(claims, utcExpiration, signingCredentials);
        }

        private string GenerateToken(IEnumerable<Claim> claims, DateTime utcExpiration, SigningCredentials signingCredentials)
        {
            var claimsIdentity = new ClaimsIdentity(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Subject = claimsIdentity,
                NotBefore = DateTime.UtcNow,
                Expires = utcExpiration,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {

            var issuerSigningKey = new RsaSecurityKey(_tokenSettings.RsaPublicKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = issuerSigningKey,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }


        public bool HasTokenExpired(string token)
        {
            var securityToken = LoadToken(token);
            return DateTime.UtcNow > securityToken.ValidTo;
        }

        public JwtSecurityToken LoadToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadToken(token) as JwtSecurityToken;
        }


    }
}
