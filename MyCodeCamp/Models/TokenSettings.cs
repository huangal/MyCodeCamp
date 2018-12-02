using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using MyCodeCamp.Services;

namespace MyCodeCamp.Models
{
    public class TokenSettings : ITokenSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string CertificateName { get; set; }

        public string SecretKey => GetSecretKey();
        public RSA RsaPrivateKey => GetCertificatePrivateKey();
        public RSA RsaPublicKey => GetCertificatePublicKey();
                
        private readonly ICertificateService _certificateService;
        private X509Certificate2 _certificate;

        public TokenSettings(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        private string GetSecretKey()
        {
            if (_certificate == null)
                _certificate = _certificateService.GetCertificate(CertificateName);
            return _certificate?.GetPublicKeyString();
        }

        private RSA GetCertificatePrivateKey()
        {
            if (_certificate == null)
                _certificate = _certificateService.GetCertificate(CertificateName);
            return (_certificate != null && _certificate.HasPrivateKey) ? _certificate.GetRSAPrivateKey() : null;
        }
        private RSA GetCertificatePublicKey()
        {
            if (_certificate == null)
                _certificate = _certificateService.GetCertificate(CertificateName);
            return _certificate != null ? _certificate.GetRSAPublicKey() : null;
        }

    }
}
