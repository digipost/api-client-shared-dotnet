using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;
using Moq;
using Xunit;

namespace Digipost.Api.Client.Shared.Tests.Certificate
{
    public class CertificateUtilityTests
    {
        private const string StringWithBom = "‎30 5b 7d 02 e6 5e 65 5f de a8 20 65 9c 3a e0 f1 a8 4b 72 2c";
        private const string StringWithoutBom = "30 5b 7d 02 e6 5e 65 5f de a8 20 65 9c 3a e0 f1 a8 4b 72 2c";

        private static Mock<BomUtility> BomUtilMock => new Mock<BomUtility>();

        private static Mock<KeyStoreUtility> KeyStoreMock
        {
            get
            {
                var keyStoreUtilityMock = new Mock<KeyStoreUtility>();
                keyStoreUtilityMock
                    .Setup(utility => utility.FindCertificate(It.IsAny<string>(), It.IsAny<X509Store>()))
                    .Returns(new X509Certificate2());
                return keyStoreUtilityMock;
            }
        }

        private static X509Certificate2 GetCertificateFromMockedStore(StoreName storeName, StoreLocation storeLocation,
            bool isSenderCertificate)
        {
            const string thumbprint = "someUniqueThumbprint";

            var keyStoreUtilityMock = new Mock<KeyStoreUtility>();
            keyStoreUtilityMock
                .Setup(utility => utility
                    .FindCertificate(
                        It.Is<string>(p => p == thumbprint),
                        It.Is<X509Store>(p => p.Location == storeLocation && p.Name == storeName.ToString()))
                )
                .Returns(new X509Certificate2());

            var certificateUtility = new CertificateUtility {KeyStoreUtility = keyStoreUtilityMock.Object};

            return isSenderCertificate
                ? certificateUtility.CreateSenderCertificate(thumbprint)
                : certificateUtility.CreateReceiverCertificate(thumbprint);
        }

        public class SenderCertificateMethod : CertificateUtilityTests
        {
            [Fact]
            public void Accesses_current_user_store()
            {
                const StoreName storeName = StoreName.My;
                const StoreLocation storeLocation = StoreLocation.CurrentUser;

                var certificate = GetCertificateFromMockedStore(storeName, storeLocation, true);

                Assert.NotNull(certificate);
            }

            [Fact]
            public void Accesses_local_machine_store()
            {
                const StoreName storeName = StoreName.My;
                const StoreLocation storeLocation = StoreLocation.LocalMachine;

                var certificate = GetCertificateFromMockedStore(storeName, storeLocation, true);

                Assert.NotNull(certificate);
            }

            [Fact]
            public void Calls_remove_bom()
            {
                //Arrange
                var bomUtilityMock = BomUtilMock;
                var keyStoreMock = KeyStoreMock;

                var certificateUtility = new CertificateUtility
                {
                    BomUtility = bomUtilityMock.Object,
                    KeyStoreUtility = keyStoreMock.Object
                };

                //Act
                var certificate = certificateUtility.CreateSenderCertificate(StringWithBom);

                //Assert
                bomUtilityMock.Verify(utility => utility.RemoveBom(It.IsAny<string>()), Times.Once());
            }

            [Fact]
            public void Thumbprint_without_bom_should_not_change()
            {
                //Arrange
                var keyStoreMock = KeyStoreMock;
                var certificateUtility = new CertificateUtility {KeyStoreUtility = keyStoreMock.Object};

                //Act
                var certificate = certificateUtility.CreateSenderCertificate(StringWithoutBom);

                //Assert
                Assert.NotNull(certificate);
                keyStoreMock.Verify(utility => utility.FindCertificate(StringWithoutBom, It.IsAny<X509Store>()));
            }
        }

        public class ReceiverCertificateMethod : CertificateUtilityTests
        {
            [Fact]
            public void Accesses_current_user_store()
            {
                const StoreName storeName = StoreName.TrustedPeople;
                const StoreLocation storeLocation = StoreLocation.CurrentUser;

                var certificate = GetCertificateFromMockedStore(storeName, storeLocation, false);

                Assert.NotNull(certificate);
            }

            [Fact]
            public void Accesses_local_machine_store()
            {
                const StoreName storeName = StoreName.TrustedPeople;
                const StoreLocation storeLocation = StoreLocation.LocalMachine;

                var certificate = GetCertificateFromMockedStore(storeName, storeLocation, false);

                Assert.NotNull(certificate);
            }

            [Fact]
            public void Calls_remove_bom()
            {
                //Arrange
                var bomUtilityMock = BomUtilMock;
                var keyStoreMock = KeyStoreMock;
                var certificateFactory = new CertificateUtility
                {
                    BomUtility = bomUtilityMock.Object,
                    KeyStoreUtility = keyStoreMock.Object
                };

                //Act
                var certificate = certificateFactory.CreateReceiverCertificate(StringWithBom);

                //Assert
                Assert.NotNull(certificate);
                bomUtilityMock.Verify(utility => utility.RemoveBom(It.IsAny<string>()), Times.Once());
            }

            [Fact]
            public void Thumbprint_without_bom_should_not_change()
            {
                //Arrange
                var keyStoreMock = KeyStoreMock;
                var certificateFactory = new CertificateUtility {KeyStoreUtility = keyStoreMock.Object};

                //Act
                var certificate = certificateFactory.CreateReceiverCertificate(StringWithoutBom);

                //Assert
                Assert.NotNull(certificate);
                keyStoreMock.Verify(utility => utility.FindCertificate(StringWithoutBom, It.IsAny<X509Store>()));
            }
        }
    }
}