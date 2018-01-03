using System;
using System.IO;
using Xunit;

namespace api_client_shared_tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.True(File.Exists("Files/difi-enhetstester.cer"));
        }
    }
}