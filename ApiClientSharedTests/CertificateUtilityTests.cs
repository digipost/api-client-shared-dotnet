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
        private Mock<BomUtility> _bomUtilMock;

        private Mock<KeyStoreUtility> _keyStoreUtilityMock;

        [ClassInitialize]
        public void ClassInitialize()
        {
            _keyStoreUtilityMock = new Mock<KeyStoreUtility>();
            _keyStoreUtilityMock
                .Setup(utility => utility.FindCertificate(It.IsAny<string>(), It.IsAny<X509Store>()))
                .Returns(new X509Certificate2());

            _bomUtilMock = new Mock<BomUtility>();
        }

        [TestClass]
        public class SenderCertificateMethod : CertificateUtilityTests
        {
            [TestMethod]
            public void CallsRemoveBom()
            {
                //Arrange
                var certificateUtility = new CertificateUtility
                {
                    BomUtil = _bomUtilMock.Object,
                    KeyStoreUtil = _keyStoreUtilityMock.Object
                };

                //Act
                var certificate = certificateUtility.CreateSenderCertificate(StringWithBom, Language.Norwegian);

                //Assert
                _bomUtilMock.Verify(utility => utility.RemoveBom(It.IsAny<string>()), Times.Once());
            }

            [TestMethod]
            public void ThumbprintWithoutBomShouldNotChange()
            {
                //Arrange
                var certificateFactory = new CertificateUtility {KeyStoreUtil = _keyStoreUtilityMock.Object};

                //Act
                var certificate = certificateFactory.CreateSenderCertificate(StringWithoutBom, Language.Norwegian);

                //Assert
                Assert.IsNotNull(certificate);
                _keyStoreUtilityMock.Verify(utility => utility.FindCertificate(StringWithoutBom, It.IsAny<X509Store>()));
            }
        }

        [TestClass]
        public class ReceiverCertificateMethod : CertificateUtilityTests
        {
            [TestMethod]
            public void CallsRemoveBom()
            {
                //Arrange
                var certificateFactory = new CertificateUtility
                {
                    BomUtil = _bomUtilMock.Object,
                    KeyStoreUtil = _keyStoreUtilityMock.Object
                };

                //Act
                var certificate = certificateFactory.CreateReceiverCertificate(StringWithBom, Language.Norwegian);

                //Assert
                Assert.IsNotNull(certificate);
                _bomUtilMock.Verify(utility => utility.RemoveBom(It.IsAny<string>()), Times.Once());
            }

            [TestMethod]
            public void ThumbprintWithoutBomShouldNotChange()
            {
                //Arrange
                var certificateFactory = new CertificateUtility {KeyStoreUtil = _keyStoreUtilityMock.Object};

                //Act
                var certificate = certificateFactory.CreateReceiverCertificate(StringWithoutBom, Language.Norwegian);

                //Assert
                Assert.IsNotNull(certificate);
                _keyStoreUtilityMock.Verify(utility => utility.FindCertificate(StringWithoutBom, It.IsAny<X509Store>()));
            }
        }
    }
}