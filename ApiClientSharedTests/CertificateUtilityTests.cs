using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ApiClientSharedTests
{
    public class CertificateUtilityTests
    {
        const string StringWithBom = "‎30 5b 7d 02 e6 5e 65 5f de a8 20 65 9c 3a e0 f1 a8 4b 72 2c";
        const string StringWithoutBom = "30 5b 7d 02 e6 5e 65 5f de a8 20 65 9c 3a e0 f1 a8 4b 72 2c";
        
        [TestClass]
        public class SenderCertificateMethod : CertificateUtilityTests
        {
            [TestMethod]
            public void CallsRemoveBom()
            {
                //Arrange
                var keyStoreUtilityMock =  new Mock<KeyStoreUtility>();
                keyStoreUtilityMock.Setup(
                    utility => utility.FindCertificate(It.IsAny<string>(), It.IsAny<X509Store>()))
                    .Returns(new X509Certificate2());
                
                var bomUtilMock = new Mock<BomUtility>();
                var certificateFactory = new CertificateFactory(keyStoreUtilityMock.Object,bomUtilMock.Object);
                
                //Act
                var certificate  = certificateFactory.SenderCertificate(StringWithBom, Language.Norwegian);
                
                //Assert
                Assert.IsNotNull(certificate);
                bomUtilMock.Verify(utility => utility.RemoveBom(It.IsAny<string>()),Times.Once());
            }

            [TestMethod]
            public void ThumbprintWithoutBomShouldNotChange()
            {
                //Arrange
                var keyStoreUtilityMock = new Mock<KeyStoreUtility>();
                keyStoreUtilityMock.Setup(
                    utility => utility.FindCertificate(It.IsAny<string>(), It.IsAny<X509Store>()))
                    .Returns(new X509Certificate2());

                var certificateFactory = new CertificateFactory(keyStoreUtilityMock.Object);

                //Act
                var certificate = certificateFactory.SenderCertificate(StringWithoutBom, Language.Norwegian);

                //Assert
                Assert.IsNotNull(certificate);
                keyStoreUtilityMock.Verify(utility => utility.FindCertificate(StringWithoutBom, It.IsAny<X509Store>()));

            }
        }

        [TestClass]
        public class ReceiverCertificateMethod : CertificateUtilityTests
        {
            [TestMethod]
            public void CallsRemoveBom()
            {
                //Arrange
                var keyStoreUtilityMock = new Mock<KeyStoreUtility>();
                keyStoreUtilityMock.Setup(
                    utility => utility.FindCertificate(It.IsAny<string>(), It.IsAny<X509Store>()))
                    .Returns(new X509Certificate2());

                var bomUtilMock = new Mock<BomUtility>();
                var certificateFactory = new CertificateFactory(keyStoreUtilityMock.Object, bomUtilMock.Object);

                //Act
                var certificate = certificateFactory.ReceiverCertificate(StringWithBom, Language.Norwegian);

                //Assert
                Assert.IsNotNull(certificate);
                bomUtilMock.Verify(utility => utility.RemoveBom(It.IsAny<string>()), Times.Once());
            }

            [TestMethod]
            public void ThumbprintWithoutBomShouldNotChange()
            {
                //Arrange
                var keyStoreUtilityMock = new Mock<KeyStoreUtility>();
                keyStoreUtilityMock.Setup(
                    utility => utility.FindCertificate(It.IsAny<string>(), It.IsAny<X509Store>()))
                    .Returns(new X509Certificate2());
                
                var certificateFactory = new CertificateFactory(keyStoreUtilityMock.Object);

                //Act
                var certificate = certificateFactory.ReceiverCertificate(StringWithoutBom, Language.Norwegian);

                //Assert
                Assert.IsNotNull(certificate);
                keyStoreUtilityMock.Verify(utility => utility.FindCertificate(StringWithoutBom,It.IsAny<X509Store>()));
                
            }
        }
    }
}