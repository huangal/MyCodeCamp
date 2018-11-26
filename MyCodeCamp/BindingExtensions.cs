using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MyCodeCamp.Models;
using Microsoft.Extensions.Configuration;
using MyCodeCamp.Services;
using MyCodeCamp.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace MyCodeCamp
{
    public static class BindingExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICertificateService, CertificateService>();

            // configure strongly typed settings objects
            //var appSettingsSection = configuration.GetSection("AppSettings");
            //services.Configure<AppSettings>(appSettingsSection);

            services.AddSingleton<IAppSettings>(cw =>
            {
                var config = new AppSettings();
                configuration.Bind("AppSettings", config);
                return config;
            });

            services.AddSingleton<ITokenSettings>(cw =>
            {
                var certificateService = services.BuildServiceProvider().GetService<ICertificateService>();
                var config = new TokenSettings(certificateService);
                configuration.Bind("TokenSettings", config);
                return config;
            });

            var weblog = configuration.Get<Weblog>();
          
            var section = configuration.Get(typeof(ConfigAdmin));

            services.AddSingleton(cw =>
            {
                var config = new WeblogConfiguration();
                configuration.Bind("Weblog", config);
                return config;
            });

          //  var section = configuration.GetSection("Weblog");



            services.AddSingleton(cw =>
            {
                var config = new EmailConfiguration();
                configuration.Bind("Email", config);
                return config;
            });

           
            services.AddTransient<ITokenService, TokenService>();



            //services.AddSingleton(cw =>
            //{
            //    //var config = new WeblogConfiguration();
            //    configuration.Bind("Weblog", new WeblogConfiguration());
            //   // return config;
            //});
           // services.AddSingleton<WeblogConfiguration>((config) => configuration.Bind("Weblog",   config = new WeblogConfiguration()));


            //

            services.AddTransient<ITopicAreaService, TopicAreaService>();

            //services.AddTransient<ITopicAreaService>(s => new TopicAreaService(5));

            //services.AddTransient<ITopicAreaService, TopicAreaService>(s =>
            // {
            //     return new TopicAreaService(5);
            // });

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ISamlEnrollmentService, SamlService>();

            services.AddTransient<IUserService, UserService>();


            // services.AddSingleton<IDeliveryClient, DeliveryClient>();



            // Add all other services here.
            return services;
        }

        public static void AddTokenAuthentication(this IServiceCollection services)
        {
            var tokenSttings = services.BuildServiceProvider().GetService<ITokenSettings>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSttings.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

    }


    //public static Boolean VerifyXml(XmlDocument Doc, RSA Key)
    //{
    //    // Check arguments.
    //    if (Doc == null)
    //        throw new ArgumentException("Doc");
    //    if (Key == null)
    //        throw new ArgumentException("Key");

    //    // Create a new SignedXml object and pass it
    //    // the XML document class.
    //    SignedXml signedXml = new SignedXml(Doc);

    //    // Find the "Signature" node and create a new
    //    // XmlNodeList object.
    //    XmlNodeList nodeList = Doc.GetElementsByTagName("Signature");

    //    // Throw an exception if no signature was found.
    //    if (nodeList.Count <= 0)
    //    {
    //        throw new CryptographicException("Verification failed: No Signature was found in the document.");
    //    }

    //    // This example only supports one signature for
    //    // the entire XML document.  Throw an exception 
    //    // if more than one signature was found.
    //    if (nodeList.Count >= 2)
    //    {
    //        throw new CryptographicException("Verification failed: More that one signature was found for the document.");
    //    }

    //    // Load the first <signature> node.  
    //    signedXml.LoadXml((XmlElement)nodeList[0]);

    //    // Check the signature and return the result.
    //    return signedXml.CheckSignature(Key);
    //}
    
    //public static bool VerifyXml(XmlDocument Doc) 
    //{
    //    if (Doc == null) throw new ArgumentException("Doc");
    //    SignedXml signedXml = new SignedXml(Doc);
        
    //    var nsManager = new XmlNamespaceManager(Doc.NameTable);
    //    nsManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
    //    var node = Doc.SelectSingleNode("//ds:Signature", nsManager);
    //    // find signature node
    //    var certElement = Doc.SelectSingleNode("//ds:X509Certificate", nsManager);
    //    // find certificate node
    //    var cert = new X509Certificate2(Convert.FromBase64String(certElement.InnerText));            
    //    signedXml.LoadXml((XmlElement)node);
    //    return signedXml.CheckSignature(cert);
    //    //return signedXml.CheckSignature();

    //}
    
    //public static bool VerifyXml(XmlDocument Doc) 
    //{
    //    if (Doc == null)
    //        throw new ArgumentException("Doc");
    //    SignedXml signedXml = new SignedXml(Doc);
    //    var nsManager = new XmlNamespaceManager(Doc.NameTable);
    //    nsManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
    //    var node = Doc.SelectSingleNode("//ds:Signature", nsManager);
    //    // find signature node
    //    var certElement = Doc.SelectSingleNode("//ds:X509Certificate", nsManager);
    //    // find certificate node
    //    var cert = new X509Certificate2(Convert.FromBase64String(certElement.InnerText));            
    //    signedXml.LoadXml((XmlElement)node);

    //    //Find installed certificate from store
    //    X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
    //    store.Open(OpenFlags.ReadOnly);
    //    X509Certificate2 storeCert = store.Certificates.Find(X509FindType.FindBySerialNumber, cert.SerialNumber, true)[0];

    //    return signedXml.CheckSignature(storeCert, true);
    //    //^^^ If certificate is installed in the Root location then 
    //    //this method returns true after validating it as well
    //    //In addition to validating the signature
    //}

}
