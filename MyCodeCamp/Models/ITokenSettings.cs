using System.Security.Cryptography;

namespace MyCodeCamp.Models
{
    public interface ITokenSettings
    {
        string Issuer { get; set; }
        string Audience { get; set; }
        string SecretKey { get; }
        RSA RsaPrivateKey { get; }
        RSA RsaPublicKey { get; }
    }
}