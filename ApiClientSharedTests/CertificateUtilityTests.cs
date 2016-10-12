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
            var _keyStoreUtilityMock = new Mock<KeyStoreUtility>();
            _keyStoreUtilityMock
                .Setup(utility => utility.FindCertificate(It.IsAny<string>(), It.IsAny<X509Store>()))
                .Returns(new X509Certificate2());
            return _keyStoreUtilityMock;
        }

        [TestClass]
        public class SenderCertificateMethod : CertificateUtilityTests
        {
            [TestMethod]
            public void CallsRemoveBom()
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
            public void ThumbprintWithoutBomShouldNotChange()
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
        }

        [TestClass]
        public class ReceiverCertificateMethod : CertificateUtilityTests
        {
            [TestMethod]
            public void CallsRemoveBom()
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
            public void ThumbprintWithoutBomShouldNotChange()
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
        }

        [TestClass]
        public class TestShit : CertificateUtilityTests
        {
            [TestMethod]
            public void GetsCertificateFromMachineStore()
            {
                var cert = CertificateUtility.SenderCertificate("‎2d 7f 30 dd 05 d3 b7 fc 7a e5 97 3a 73 f8 49 08 3b 20 40 ed", Language.English);
            }
        }
    }
}