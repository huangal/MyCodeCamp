using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
//using DECH.Enterprise.XmlParser;

namespace MyCodeCamp.Models
{
    public static class Tools
    {


        public static string GenerateToken()
        {

            // string token = DECH.

            return "";

        }



        public static List<string> GetAllEntities<T>() where T: class
        {
            if(!typeof(T).IsInterface) return new List<string>();
                return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .Select(x => x.Name).ToList();
        }

        /*
        private IPurge PurgeCache(string entity, string nameSpace)
        {
            var instance = IocContainer.Instance
                .GetCurrentRegistrations().FirstOrDefault(s => s?.ServiceType?.Namespace != null
                && s.ServiceType.Namespace.Contains(nameSpace)
                && s.ServiceType.GenericTypeArguments.Any()
                && String.Equals(s.ServiceType.GenericTypeArguments[0].Name, entity, StringComparison.CurrentCultureIgnoreCase));

            return (IPurge)instance.GetInstance() ?? null;
        }
        */




        /*
       public static Saml2SecurityToken GetSaml2Token()
        {
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor();
            descriptor.TokenType = "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0";
            DateTime issueInstant = DateTime.UtcNow;
            descriptor.Lifetime = new Lifetime(issueInstant, issueInstant + new TimeSpan(8, 0, 0));
            descriptor.AppliesToAddress = "https://bmukes.insurancetechnologies.com/TestSALMAssertionWebService/Service1.svc";


            List<Claim> claims = new List<Claim>() { new Claim("User", "Michele"), new Claim("Permission", "Read"), new Claim("Permission", "Write"), new Claim("Permission", "Update"), new Claim("Permission", "Delete") };
            descriptor.Subject = new ClaimsIdentity(claims);
            descriptor.AddAuthenticationClaims("urn:oasis:names:tc:SAML:2.0:ac:classes:X509");

            //who issued the token
            descriptor.TokenIssuerName = "https://bmukes.insurancetechnologies.com/test/sts/";

            X509Certificate2 signingCert = GetCertificateByThumbprint(StoreName.My, StoreLocation.LocalMachine, "68 94 27 1e 8a 0e 09 a7 fd a1 8b 18 18 ba 2f f8 f4 0c f2 6f");
            SecurityKeyIdentifier ski = new SecurityKeyIdentifier(new SecurityKeyIdentifierClause[] { new X509SecurityToken(signingCert).CreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>() });
            X509SigningCredentials signingCreds = new X509SigningCredentials(signingCert, ski);
            descriptor.SigningCredentials = signingCreds;

            Saml2SecurityTokenHandler tokenHandler = new Saml2SecurityTokenHandler();
            Saml2SecurityToken token = tokenHandler.CreateToken(descriptor) as Saml2SecurityToken;
            return token;

        }
*/

       private static X509Certificate2 GetCertificateByThumbprint(StoreName name, StoreLocation location, string thumbprint)
        {
            String validthumbprint = thumbprint.Replace(" ", null);
            X509Store store = new X509Store(name, location);
            X509Certificate2Collection certificates = null;
            store.Open(OpenFlags.ReadOnly);

            try
            {
                X509Certificate2 result = null;

                //
                // Every time we call store.Certificates property, a new collection will be returned.
                //
                certificates = store.Certificates;

                for (int i = 0; i < certificates.Count; i++)
                {
                    X509Certificate2 cert = certificates[i];

                    if (cert.Thumbprint.ToLower() == validthumbprint.ToLower())
                    {
                        if (result != null)
                        {
                            throw new ApplicationException(string.Format("There are multiple certificates for thumbprint {0}", thumbprint));
                        }

                        result = new X509Certificate2(cert);
                    }
                }

                if (result == null)
                {
                    throw new ApplicationException(string.Format("No certificate was found for thumbprint {0}", thumbprint));
                }

                return result;
            }
            finally
            {
                if (certificates != null)
                {
                    for (int i = 0; i < certificates.Count; i++)
                    {
                        X509Certificate2 cert = certificates[i];
                        cert.Reset();
                    }
                }

                store.Close();
            }
        }
    }
}
