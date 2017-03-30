using Difi.Felles.Utility.Resources.Language;
using Digipost.Api.Client.Shared.Extensions;
using Digipost.Api.Client.Shared.Resources.Certificate;
using Xunit;

namespace Digipost.Api.Client.Shared.Resources.Tests
{
    public class LanguageResourceTests
    {
        public class GetResourceMethod
        {
            [Fact]
            public void Get_resource_with_placeholders()
            {
                LanguageResource.CurrentLanguage = Language.Language.English;
                var certificate = CertificateResource.UnitTests.GetPostenCertificate();
                
                var certDescr = certificate.ToShortString("Extrainfo");

                Assert.True(certDescr.Contains(certificate.Subject));
            }
        }
    }
}