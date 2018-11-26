using System;
using System.Xml;
using System.IdentityModel.Tokens;
using System.Xml.XPath;
using System.Diagnostics;

namespace MyCodeCamp.Models
{
    public class XmlHelper
    {
        public XmlHelper()
        {
        }

        public void GetXml(XmlDocument document)
        {
            if (document == null) return;
            try
            {
                

                XPathNavigator navigator = document.CreateNavigator();
                //navigator.MoveToRoot();


                //string query = "//book[@category=\'" + p_Category + "\']";
                //XPathNodeIterator xPathIt = p_xPathNav.Select(query);



                XPathExpression query = navigator.Compile("//*[local-name()='EncryptedAssertion']");
                XPathNodeIterator nodes = navigator.Select(query);
                XPathNavigator nodesNavigator = nodes.Current;
                nodes.MoveNext();

                ////EncryptedAssertion/EncryptedData/KeyInfo/EncryptedKey
                XPathNodeIterator keyInfoIterator = nodesNavigator.Select("//*[local-name()='EncryptedData/KeyInfo']");
                keyInfoIterator.MoveNext();
                var key = keyInfoIterator.Current.Value;

                //if(keyInfoIterator.MoveNext())
                //{
                //    var key = keyInfoIterator.Current.Value;
                //}





                XPathNodeIterator nodesText = nodesNavigator.SelectDescendants(XPathNodeType.Text, false);

                while (nodesText.MoveNext())
                {
                    var test = nodesText.Current.Value;
                    Console.WriteLine(nodesText.Current.Value);
                }

              //  XPathNodeIterator nodes = navigator.Select("//*[local-name()='EncryptedAssertion']");


                //nodes.MoveNext();
                //XPathNavigator nodesNavigator = nodes.Current;
                //XPathNodeIterator nodesText = nodesNavigator.SelectDescendants(XPathNodeType.Text, false);

                //while (nodesText.MoveNext())
                //{
                    
                //}

                ////do
                ////{
                ////    var idpIdentifier = nodeIterator.Current.Value;
                ////} while (nodeIterator.MoveNext());



                //if(nodeIterator.MoveNext())
                //{
                //    var idpIdentifier = nodeIterator.Current.Value;
                //}


            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }


        }







    }


   

}
