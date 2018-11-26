using System.Security.Cryptography.X509Certificates;

namespace MyCodeCamp.Services
{
    public interface ICertificateService
    {
        X509Certificate2 GetCertificate(string certificateName, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.LocalMachine);
    }
}