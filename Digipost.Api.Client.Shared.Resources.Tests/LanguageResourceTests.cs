using Digipost.Api.Client.Shared.Extensions;
using Digipost.Api.Client.Shared.Resources.Certificate;
using Digipost.Api.Client.Shared.Resources.Language;
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

            [Fact]
            public void Get_resource_with_temporary_language()
            {
                LanguageResource.CurrentLanguage = Language.Language.Norwegian;
                var resource = LanguageResource.GetResource(LanguageResourceKey.ToleratedPrefixListError, Language.Language.English);

                Assert.True(resource.Contains("The 'PrefixList' attribute is invalid"));
                Assert.Equal(Language.Language.Norwegian, LanguageResource.CurrentLanguage);
            }
        }
    }
}