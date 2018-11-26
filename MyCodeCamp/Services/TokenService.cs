using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyCodeCamp.Models;


namespace MyCodeCamp.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenSettings _tokenSettings;

        public TokenService(ITokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }

        public string GenerateToken(IEnumerable<Claim> claims, DateTime utcExpiration, string secretKey = null )
        {
            if (string.IsNullOrWhiteSpace(secretKey)) secretKey = _tokenSettings.SecretKey;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = utcExpiration,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
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
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }


        public bool HasTokenExpired(string token)
        {
            
            var securityToken = LoadToken(token);
            var expired = securityToken.ValidTo;
            var principal = GetPrincipalFromToken(token);

            return DateTime.UtcNow > expired;
        }

        public JwtSecurityToken LoadToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadToken(token) as JwtSecurityToken;
        }


        private string GetCertificatePublicKey()
        {
            var certificate = GetCertificate("partners-umbhsasso.umb.com");
            return certificate != null ? certificate.GetPublicKeyString() : null;
        }

        private  X509Certificate2 GetCertificate(string certificateName)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.IncludeArchived);
            try
            {
                X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, false);

                return certificates.Count > 0 ? certificates[0] : null;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return null;
            }
            finally
            {
                if (store != null) store.Close();
            }

        }
    }
}
