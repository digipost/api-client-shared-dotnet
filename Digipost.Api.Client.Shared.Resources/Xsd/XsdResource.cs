using System.IO;
using System.Text;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Api.Client.Shared.Resources.Xsd
{
    internal class XsdResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Digipost.Api.Client.Shared.Resources.Xsd.Data");

        private static Stream GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return new MemoryStream(bytes);
        }

        public static Stream Sample()
        {
            return GetResource("Sample.xsd");
        }
    }
}