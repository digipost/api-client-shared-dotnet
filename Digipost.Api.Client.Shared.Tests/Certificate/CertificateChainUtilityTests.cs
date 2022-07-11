using System.Diagnostics;
using Digipost.Api.Client.Shared.Certificate;
using Xunit;

namespace Digipost.Api.Client.Shared.Tests.Certificate
{
    public class CertificateChainUtilityTests
    {
        public class TestsertifikaterMethod : CertificateChainUtilityTests
        {
            [Fact]
            public void Returns_ten_certificates_with_thumbprint()
            {
                //Arrange
                var certificates = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act

                //Assert
                Assert.Equal(10, certificates.Count);
                foreach (var certificate in certificates)
                {
                    Assert.NotNull(certificate.Thumbprint);
                }
            }
        }

        public class ProduksjonssertifikaterMethod : CertificateChainUtilityTests
        {
            [Fact]
            public void Returns_ten_certificates_with_thumbprint()
            {
                //Arrange
                var certificates = CertificateChainUtility.ProduksjonsSertifikater();

                //Act

                //Assert
                Assert.Equal(10, certificates.Count);
                foreach (var certificate in certificates)
                {
                    Assert.NotNull(certificate.Thumbprint);
                }
            }
        }

        public class CertificateChainInfoTests : CertificateChainUtilityTests
        {
            [Fact]
            public void DebugMesages()
            {
                var i = 0;
                foreach (var certificate in CertificateChainUtility.FunksjoneltTestmiljøSertifikater())
                {
                    Trace.WriteLine($"{i++}: Issuer `{certificate.Issuer}`, thumbprint `{certificate.Thumbprint}`");
                }
            }
        }
    }
}