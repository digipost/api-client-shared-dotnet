using System.Xml;
using Digipost.Api.Client.Shared.Resources.Xsd;
using Digipost.Api.Client.Shared.Xml;

namespace Digipost.Api.Client.Shared.Tests.Xml
{
    public class XmlValidatorTestImplementation : XmlValidator
    {
        public XmlValidatorTestImplementation()
        {
            AddXsd("http://tempuri.org/po.xsd", XmlReader.Create(XsdResource.Sample()));
        }
    }
}