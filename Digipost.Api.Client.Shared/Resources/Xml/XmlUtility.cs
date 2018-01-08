using System.Xml;

namespace Digipost.Api.Client.Shared.Resources.Xml
{
    public class XmlUtility
    {
        public static XmlDocument ToXmlDocument(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            return xmlDocument;
        }
    }
}