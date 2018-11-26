namespace MyCodeCamp
{
    /*
    public static class IocContainer
    {

        public static IServiceCollection RegisterServices(IServiceCollection services)
        {
            //services.AddTransient<ITopicAreaService, TopicAreaService>();

            services.AddSingleton<ITopicAreaService, TopicAreaService>();
            return services;

        }

    }

*/
    public static class UrlExtensiong
    {
        public static bool IsValidUrl(this string url)
        {
            string urlPattern = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,'\/\\+&%\$#_]*)?$";
            var match = System.Text.RegularExpressions.Regex.Match(url, urlPattern);
            return match.Success;
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
