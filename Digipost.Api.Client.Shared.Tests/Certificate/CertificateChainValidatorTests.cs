using Digipost.Api.Client.Shared.Certificate;
using Digipost.Api.Client.Shared.Resources.Certificate;
using Xunit;

namespace Digipost.Api.Client.Shared.Tests.Certificate
{
    public class CertificateChainValidatorTests
    {
        public class ValidateCertificateChain : CertificateChainValidatorTests
        {
            [Fact]
            public void Fails_with_self_signed_certificate()
            {
                //Arrange
                var selfSignedCertificate = CertificateResource.UnitTests.GetEnhetstesterSelvsignertSertifikat();
                var certificateChainValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                //Act
                var result = certificateChainValidator.Validate(selfSignedCertificate);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidChain, result.Type);
                Assert.Contains("certificate is self signed", result.Message);
            }

            [Fact]
            public void Fails_with_wrong_root_and_intermediate()
            {
                //Arrange
                var productionCertificate = CertificateResource.UnitTests.GetProduksjonsMottakerSertifikatOppslagstjenesten();

                //Act
                var certificateChainValidator = new CertificateChainValidator(CertificateChainUtility.FunksjoneltTestmiljøSertifikater());
                var result = certificateChainValidator.Validate(productionCertificate);

                //Assert
                Assert.Equal(CertificateValidationType.InvalidChain, result.Type);
                Assert.Contains("the certificate is retrieved from Windows Certificate Store", result.Message);
            }

            [Fact]
            public void Valid_with_correct_root_and_intermediate()
            {
                //Arrange
                var productionCertificate = CertificateResource.UnitTests.GetProduksjonsMottakerSertifikatOppslagstjenesten();
                var certificateChainValidator = new CertificateChainValidator(CertificateChainUtility.ProduksjonsSertifikater());

                //Act
                var result = certificateChainValidator.Validate(productionCertificate);

                //Assert
                Assert.Equal(CertificateValidationType.Valid, result.Type);
                Assert.Contains("is a valid certificate", result.Message);
            }
        }
    }
}