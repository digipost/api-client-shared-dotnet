using System.Reflection;
using Digipost.Api.Client.Shared.Certificate;
using Digipost.Api.Client.Shared.Resources.Resource;
using Xunit;

namespace Digipost.Api.Client.Shared.Tests.Resources.Resource
{
    public class ResourceUtilityTests
    {
        [Fact]
        public static void Can_load_file_from_external_assembly_with_preloaded_base_path()
        {
            var currentAssembly = typeof(ResourceUtilityTests).GetTypeInfo().Assembly;
            var utility = new ResourceUtility(currentAssembly, "Digipost.Api.Client.Shared.Tests.Resources.Resource");

            var bytes = utility.ReadAllBytes("ResourceFile.txt");

            Assert.NotNull(bytes);
        }
        
        [Fact]
        public static void Can_load_file_from_internal_assembly_with_preloaded_base_path()
        {
            /*
             * This will be tested implicitly by all the other tests in the project.
             */
        }
        
        [Fact]
        public static void Resources_added_correctly_to_project()
        {
            var assembly = typeof(CertificateChainUtility).GetTypeInfo().Assembly;

            var manifestResourceNames = assembly.GetManifestResourceNames();

            Assert.True(manifestResourceNames.Length > 25, "It should be more than 20 files refrenced as resources. If not so, the embedded resources in .csproj has been tampered with.");
        }

        [Fact]
        public static void Does_not_add_project_name_to_base_path_if_in_input()
        {
            var currentAssembly = typeof(ResourceUtilityTests).GetTypeInfo().Assembly;
            var resourcePath = "Digipost.Api.Client.Shared.Tests.Resources.Resource";
            var utility = new ResourceUtility(currentAssembly, resourcePath);

            Assert.Equal(resourcePath.Length, utility.BasePath.Length);
        }
        
        
        [Fact]
        public static void Does_add_project_name_to_base_path_if_not_in_input()
        {
            var currentAssembly = typeof(ResourceUtilityTests).GetTypeInfo().Assembly;
            var inputResourcePath = "Resources.Resource";
            var expectedResourcePath = "Digipost.Api.Client.Shared.Tests.Resources.Resource";
            var utility = new ResourceUtility(currentAssembly, inputResourcePath);

            Assert.Equal(expectedResourcePath.Length, utility.BasePath.Length);
        }
    }
}