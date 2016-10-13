using System.Linq;
using ApiClientShared;
using Xunit;

namespace ApiClientSharedTests
{
    
    public class BomUtilityTests
    {
        private const string StringWithBom = "‎30 5b 7d 02 e6 5e 65 5f de a8 20 65 9c 3a e0 f1 a8 4b 72 2c";
        private const string StringWithoutBom = "30 5b 7d 02 e6 5e 65 5f de a8 20 65 9c 3a e0 f1 a8 4b 72 2c";

        
        public class RemoveBomMethod : BomUtilityTests
        {
            [Fact]
            public void Should_remove_bom()
            {
                //Arrange
                var bomUtility = new BomUtility();
                //Act
                var trimmed = bomUtility.RemoveBom(StringWithBom);

                var firstElement = trimmed.ElementAt(0);

                //Assert
                Assert.True(firstElement == '3');
                Assert.NotEqual(trimmed, StringWithBom);
            }

            [Fact]
            public void Should_not_remove_bom()
            {
                //Arrange
                var bomUtility = new BomUtility();
                //Act
                var trimmed = bomUtility.RemoveBom(StringWithoutBom);

                var firstElement = trimmed.ElementAt(0);

                //Assert
                Assert.True(firstElement == '3');
                Assert.Equal(trimmed, StringWithoutBom);
            }
        }
    }
}