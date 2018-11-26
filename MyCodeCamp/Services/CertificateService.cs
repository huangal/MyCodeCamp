using System;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace MyCodeCamp.Services
{
    public class CertificateService : ICertificateService
    {
        public X509Certificate2 GetCertificate(string certificateName, StoreName storeName = StoreName.My, StoreLocation storeLocation= StoreLocation.LocalMachine)
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
               if(store != null) store.Close();
            }
        }
    }
}
