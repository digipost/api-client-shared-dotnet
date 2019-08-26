using System.IO;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Api.Client.Shared.Resources.Xsd
{
    internal static class XsdResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Resources.Xsd.Data");

        private static Stream GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(path);
            return new MemoryStream(bytes);
        }

        public static Stream Sample()
        {
            return GetResource("Sample.xsd");
        }
    }
}