﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyCodeCamp.Models;


namespace MyCodeCamp.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenSettings _tokenSettings;
        private readonly string _securityAlgorithm;

        public TokenService(ITokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
            _securityAlgorithm = SecurityAlgorithms.HmacSha256Signature;
        }

        public string GenerateToken(IEnumerable<Claim> claims, DateTime utcExpiration, object secretKey = null )
        {
            if (secretKey == null) secretKey = _tokenSettings.SecretKey;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((string)secretKey));
            var signingCredentials = new SigningCredentials(securityKey, _securityAlgorithm);

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
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));
           
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
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(_securityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
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
