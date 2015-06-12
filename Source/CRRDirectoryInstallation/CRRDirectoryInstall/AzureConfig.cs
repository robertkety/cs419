using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CRRDirectoryInstall
{
    public static class XDocumentExtensions
    {
        public static string ToStringWithXmlDeclaration(this XDocument doc)
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            doc.Save(writer);
            writer.Flush();
            return builder.ToString();
        }
    }

    public class AzureConfig
    {
        #region "Variables"
        private const string Version = "2012-10-10";
        private static string uriFormat = "https://management.core.windows.net";
        //private static string thumbPrint = "your thumbprint";
        private const string WebsitePostfix = ".azurewebsites.net";
        #endregion
        #region "Classes"
        public AzureConfig()
        {

        }

        static AzureConfig()
        {

        }
        #endregion
        #region "Create Azure WebSite"


        public static void CreateWebSite(WebSites site, X509Certificate2 Certificate, string SubscriptionID)
        {
            try
            {
                XNamespace xmlns = "http://schemas.microsoft.com/windowsazure";
                XNamespace i = "http://www.w3.org/2001/XMLSchema-instance";
                XNamespace a = "http://schemas.microsoft.com/2003/10/Serialization/Arrays";

                XName iNamespace = XNamespace.Xmlns + "i";
                XName iNil = i + "nil";
                XName aArray = XNamespace.Xmlns + "a";
                XName aString = a + "string";



                var doc = new XDocument(
                          new XDeclaration("1.0", "utf-8", ""),
                          new XElement(xmlns + "Site", new XAttribute(iNamespace, i),
                              new XElement(xmlns + "HostNames", new XAttribute(aArray, a),
                                  new XElement(aString, site.WebSiteName + WebsitePostfix)),
                              new XElement(xmlns + "Name", site.WebSiteName),
                              new XElement(xmlns + "WebSpaceToCreate",
                                  new XElement(xmlns + "GeoRegion", site.GeoRegion),
                                  new XElement(xmlns + "Name", site.WebSpace),
                                  new XElement(xmlns + "Plan", "VirtualDedicatedPlan")
                                  )
                              )
                    );

                string tmp = doc.ToStringWithXmlDeclaration();

                X509Certificate2 cert = Certificate;
                string webUri = uriFormat + "/{0}/" + "services/WebSpaces/{1}/sites";
                Uri uri = new Uri(String.Format(webUri, SubscriptionID, site.WebSpace));

                byte[] byteArray = Encoding.UTF8.GetBytes(doc.ToStringWithXmlDeclaration());

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Method = "POST";
                request.Headers.Add("x-ms-version", Version);
                request.ClientCertificates.Add(cert);
                request.ContentType = "application/xml";
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.

                XDocument responseBody = null;
                HttpStatusCode statusCode;
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                statusCode = response.StatusCode;

                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();



            }
            catch (WebException ex)
            {
                string strErr = ex.ToString();
            }


        }

        #endregion
    }
}
