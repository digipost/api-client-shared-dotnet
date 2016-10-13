using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ApiClientSharedTests
{
    public class CertificateUtilityTests
    {
        private const string StringWithBom = "‎30 5b 7d 02 e6 5e 65 5f de a8 20 65 9c 3a e0 f1 a8 4b 72 2c";
        private const string StringWithoutBom = "30 5b 7d 02 e6 5e 65 5f de a8 20 65 9c 3a e0 f1 a8 4b 72 2c";


        private Mock<BomUtility> BomUtilMock()
        {
            return new Mock<BomUtility>();
        }

        private Mock<KeyStoreUtility> KeyStoreMock()
        {
            var keyStoreUtilityMock = new Mock<KeyStoreUtility>();
            keyStoreUtilityMock
                .Setup(utility => utility.FindCertificate(It.IsAny<string>(), It.IsAny<X509Store>()))
                .Returns(new X509Certificate2());
            return keyStoreUtilityMock;
        }

        [TestClass]
        public class SenderCertificateMethod : CertificateUtilityTests
        {
            [TestMethod]
            public void Calls_remove_bom()
            {
                //Arrange
                var bomUtilityMock = BomUtilMock();
                var keyStoreMock = KeyStoreMock();

                var certificateUtility = new CertificateUtility
                {
                    BomUtility = bomUtilityMock.Object,
                    KeyStoreUtility = keyStoreMock.Object
                };

                //Act
                var certificate = certificateUtility.CreateSenderCertificate(StringWithBom, Language.Norwegian);

                //Assert
                bomUtilityMock.Verify(utility => utility.RemoveBom(It.IsAny<string>()), Times.Once());
            }

            [TestMethod]
            public void Thumbprint_without_bom_should_not_change()
            {
                //Arrange
                var keyStoreMock = KeyStoreMock();
                var certificateUtility = new CertificateUtility {KeyStoreUtility = keyStoreMock.Object};

                //Act
                var certificate = certificateUtility.CreateSenderCertificate(StringWithoutBom, Language.Norwegian);

                //Assert
                Assert.IsNotNull(certificate);
                keyStoreMock.Verify(utility => utility.FindCertificate(StringWithoutBom, It.IsAny<X509Store>()));
            }

            [TestMethod]
            public void Accesses_current_user_store()
            {
                const StoreName storeName = StoreName.My;
                const StoreLocation storeLocation = StoreLocation.CurrentUser;

                var certificate = GetCertificateFromMockedStore(storeName, storeLocation, true);

                Assert.IsNotNull(certificate);
            }

            [TestMethod]
            public void Accesses_local_machine_store()
            {
                const StoreName storeName = StoreName.My;
                const StoreLocation storeLocation = StoreLocation.LocalMachine;

                var certificate = GetCertificateFromMockedStore(storeName, storeLocation, true);

                Assert.IsNotNull(certificate);
            }
        }

        [TestClass]
        public class ReceiverCertificateMethod : CertificateUtilityTests
        {
            [TestMethod]
            public void Calls_remove_bom()
            {
                //Arrange
                var bomUtilityMock = BomUtilMock();
                var keyStoreMock = KeyStoreMock();
                var certificateFactory = new CertificateUtility
                {
                    BomUtility = bomUtilityMock.Object,
                    KeyStoreUtility = keyStoreMock.Object
                };

                //Act
                var certificate = certificateFactory.CreateReceiverCertificate(StringWithBom, Language.Norwegian);

                //Assert
                Assert.IsNotNull(certificate);
                bomUtilityMock.Verify(utility => utility.RemoveBom(It.IsAny<string>()), Times.Once());
            }

            [TestMethod]
            public void Thumbprint_without_bom_should_not_change()
            {
                //Arrange
                var keyStoreMock = KeyStoreMock();
                var certificateFactory = new CertificateUtility {KeyStoreUtility = keyStoreMock.Object};

                //Act
                var certificate = certificateFactory.CreateReceiverCertificate(StringWithoutBom, Language.Norwegian);

                //Assert
                Assert.IsNotNull(certificate);
                keyStoreMock.Verify(utility => utility.FindCertificate(StringWithoutBom, It.IsAny<X509Store>()));
            }

            [TestMethod]
            public void Accesses_current_user_store()
            {
                const StoreName storeName = StoreName.TrustedPeople;
                const StoreLocation storeLocation = StoreLocation.CurrentUser;

                var certificate = GetCertificateFromMockedStore(storeName, storeLocation, false);

                Assert.IsNotNull(certificate);
            }

            [TestMethod]
            public void Accesses_local_machine_store()
            {
                const StoreName storeName = StoreName.TrustedPeople;
                const StoreLocation storeLocation = StoreLocation.LocalMachine;

                var certificate = GetCertificateFromMockedStore(storeName, storeLocation, false);

                Assert.IsNotNull(certificate);
            }
        }

        static X509Certificate2 GetCertificateFromMockedStore(StoreName storeName, StoreLocation storeLocation, bool isSenderCertificate)
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

            var certificateUtility = new CertificateUtility() {KeyStoreUtility = keyStoreUtilityMock.Object};
            X509Certificate2 certificate;

            return isSenderCertificate 
                ? certificateUtility.CreateSenderCertificate(thumbprint, Language.English) 
                : certificateUtility.CreateReceiverCertificate(thumbprint, Language.English);
        }
    }
}