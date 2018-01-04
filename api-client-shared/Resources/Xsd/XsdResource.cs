using System.IO;
using System.Text;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Api.Client.Shared.Resources.Xsd
{
    public class XsdResource //Todo: Var internal, og bør nok være det videre
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Resources/Xsd/Data");

        private static Stream GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytesNew(path);
            return new MemoryStream(bytes);
        }

        public static Stream Sample()
        {
            return GetResource("Sample.xsd");
        }
    }
}