using Digipost.Api.Client.Shared.Xml;
using Xunit;

namespace Digipost.Api.Client.Shared.Tests.Xml
{
    public class XmlValidationRunnerTests
    {
        public class ConstructorMethod : XmlValidationRunnerTests
        {
            [Fact]
            public void SimpleInitialization()
            {
                //Arrange
                var xmlSchemaSet = TestGenerator.XmlSchemaSet();

                //Act
                var validationRunner = new XmlValidationRunner(xmlSchemaSet);

                //Assert
                Assert.Equal(xmlSchemaSet, validationRunner.XmlSchemaSet);
            }
        }

        public class ValidateMethod : ValidationMessagesTests
        {
            [Fact]
            public void AddsValidationMessage()
            {
                //Arrange
                var validationRunner = new XmlValidationRunner(TestGenerator.XmlSchemaSet());
                var invalidTestCouple = new TestGenerator.InvalidContentTestCouple();

                //Act
                validationRunner.Validate(invalidTestCouple.Input());

                //Assert
                Assert.Equal(1, validationRunner.ValidationMessages.Count);
            }
        }
    }
}