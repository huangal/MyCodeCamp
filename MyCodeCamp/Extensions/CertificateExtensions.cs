using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace MyCodeCamp.Extensions
{

    public static class CertificateExtensions
    {
        public static RSA RSAPrivateKey(this X509Certificate2 certificate )
        {
            return (certificate != null && certificate.HasPrivateKey) ? certificate.GetRSAPrivateKey() : null;
        }

        public static RSA RSAPublicKey(this X509Certificate2 certificate)
        {
            return certificate?.GetRSAPublicKey();
        }
    }


}
