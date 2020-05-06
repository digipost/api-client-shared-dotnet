using Xunit;
using static Digipost.Api.Client.Shared.Certificate.CertificateChainUtility;
using static Digipost.Api.Client.Shared.Certificate.CertificateValidationType;
using static Digipost.Api.Client.Shared.Certificate.CertificateValidator;
using static Digipost.Api.Client.Shared.Resources.Certificate.CertificateResource.UnitTests;


namespace Digipost.Api.Client.Shared.Tests.Certificate
{
    public class CertificateValidatorTests
    {
        public class ValidateCertificateAndChainInternalMethod : CertificateValidatorTests
        {
            [Fact]
            public void Returns_fail_if_certificate_error()
            {
                //Arrange
                var funksjoneltTestmiljøSertifikater = FunksjoneltTestmiljøSertifikater();

                //Act
                var result = ValidateCertificateAndChainInternal(GetExpiredSelfSignedTestCertificate(), "988015814", funksjoneltTestmiljøSertifikater);

                //Assert
                Assert.Equal(InvalidCertificate, result.Type);
                Assert.Contains("expired on", result.Message);
            }

            [Fact]
            public void Returns_fail_if_self_signed_certificate()
            {
                //Arrange
                var funksjoneltTestmiljøSertifikater = FunksjoneltTestmiljøSertifikater();

                //Act
                var result = ValidateCertificateAndChainInternal(GetValidSelfSignedTestCertificate(), "988015814", funksjoneltTestmiljøSertifikater);

                //Assert
                Assert.Equal(InvalidChain, result.Type);
                Assert.Contains("is invalid because the chain length is 1", result.Message);
            }

            [Fact]
            public void Returns_ok_if_valid_certificate_and_chain()
            {
                //Arrange
                var funksjoneltTestmiljøSertifikater = FunksjoneltTestmiljøSertifikater();

                //Act
                var result = ValidateCertificateAndChainInternal(GetPostenTestCertificate(), "984661185", funksjoneltTestmiljøSertifikater);

                //Assert
                Assert.Equal(Valid, result.Type);
                Assert.Contains("is a valid certificate", result.Message);
            }
        }

        public class ValidateCertificateMethodWithOrganizationNumber : CertificateValidatorTests
        {
            /// <summary>
            ///     To ensure we are calling the overload doing checking for expiration, activation and not null.
            /// </summary>
            [Fact]
            public void Calls_validate_certificate_overload_with_no_organization_number()
            {
                //Arrange
                const string organizationNumber = "988015814";

                //Act
                var result = ValidateCertificate(GetExpiredSelfSignedTestCertificate(), organizationNumber);

                //Assert
                Assert.Equal(InvalidCertificate, result.Type);
                Assert.Contains("expired on", result.Message);
            }

            [Fact]
            public void Ignores_issued_to_organization_if_no_organization_number()
            {
                //Act
                var result = ValidateCertificate(GetPostenTestCertificate(), string.Empty);

                //Assert
                Assert.Equal(Valid, result.Type);
                Assert.Contains("is a valid certificate", result.Message);
            }

            [Fact]
            public void Returns_fail_if_not_issued_to_organization_number()
            {
                //Arrange
                const string certificateOrganizationNumber = "123456789";

                //Act
                var result = ValidateCertificate(TestIntegrasjonssertifikat(), certificateOrganizationNumber);

                //Assert
                Assert.Equal(InvalidCertificate, result.Type);
                Assert.Contains("is not issued to organization number", result.Message);
            }

            [Fact]
            public void Returns_ok_if_valid()
            {
                //Arrange
                const string certificateOrganizationNumber = "984661185";

                //Act
                var result = ValidateCertificate(GetPostenTestCertificate(), certificateOrganizationNumber);

                //Assert
                Assert.Equal(Valid, result.Type);
                Assert.Contains("is a valid certificate", result.Message);
            }
        }

        public class ValidateCertificateMethodWithNoOrganizationNumber : CertificateValidatorTests
        {
            [Fact]
            public void Returns_fail_if_expired()
            {
                //Arrange

                //Act
                var result = ValidateCertificate(GetExpiredSelfSignedTestCertificate());

                //Assert
                Assert.Equal(InvalidCertificate, result.Type);
                Assert.Contains("expired on", result.Message);
            }

            [Fact]
            public void Returns_fail_if_not_activated()
            {
                //Arrange

                //Act
                var result = ValidateCertificate(NotActivatedSelfSignedTestCertificate());

                //Assert
                Assert.Equal(InvalidCertificate, result.Type);
                Assert.Contains("is not active until", result.Message);
            }

            [Fact]
            public void Returns_fail_with_null_certificate()
            {
                //Arrange

                //Act
                var result = ValidateCertificate(null);

                //Assert
                Assert.Equal(InvalidCertificate, result.Type);
                Assert.Contains("is null", result.Message);
            }

            [Fact]
            public void Returns_ok_if_valid()
            {
                //Arrange

                //Act
                var result = ValidateCertificate(GetPostenTestCertificate());

                //Assert
                Assert.Equal(Valid, result.Type);
                Assert.Contains("is a valid certificate", result.Message);
            }
        }
    }
}
