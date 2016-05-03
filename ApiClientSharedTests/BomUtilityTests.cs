using System.Linq;
using ApiClientShared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiClientSharedTests
{
    [TestClass]
    class BomUtilityTests
    {
        const string StringWithBom = "‎30 5b 7d 02 e6 5e 65 5f de a8 20 65 9c 3a e0 f1 a8 4b 72 2c";
        const string StringWithoutBom = "30 5b 7d 02 e6 5e 65 5f de a8 20 65 9c 3a e0 f1 a8 4b 72 2c";
        [TestClass]
        public class RemoveBomMethod : BomUtilityTests
        {
            [TestMethod]
            public void ShouldRemoveBom()
            {
                //Arrange
                BomUtility bomUtility = new BomUtility();
                //Act
                var trimmed = bomUtility.RemoveBom(StringWithBom);

                var firstElement = trimmed.ElementAt(0);

                //Assert
                Assert.IsTrue(firstElement == '3');
                Assert.AreNotEqual(trimmed, StringWithBom);
            }

            [TestMethod]
            public void ShouldNotRemoveBom()
            {
                //Arrange
                BomUtility bomUtility = new BomUtility();
                //Act
                var trimmed = bomUtility.RemoveBom(StringWithoutBom);

                var firstElement = trimmed.ElementAt(0);

                //Assert
                Assert.IsTrue(firstElement == '3');
                Assert.AreEqual(trimmed, StringWithoutBom);
            }
        }
    }
}
