using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyCodeCamp.Services
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        string GenerateToken(IEnumerable<Claim> claims, DateTime utcExpiration,object secretKey = null);
        ClaimsPrincipal GetPrincipalFromToken(string token);
        bool HasTokenExpired(string token);
        JwtSecurityToken LoadToken(string token);
    }
}