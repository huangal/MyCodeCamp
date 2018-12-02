using System.Security.Cryptography;

namespace MyCodeCamp.Models
{
    public interface ITokenSettings
    {
        string SecretKey { get;  }
        string Issuer { get; set; }
        string Audience { get; set; }
        RSA PrivateKey { get; }
        RSA PublicKey { get; }
        string CertificateName { get; set; }
    }
}