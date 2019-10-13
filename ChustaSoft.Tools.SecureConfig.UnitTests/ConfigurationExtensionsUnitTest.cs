using ChustaSoft.Tools.SecureConfig.UnitTests.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Xunit;

namespace ChustaSoft.Tools.SecureConfig.UnitTests
{
    public class ConfigurationExtensionsUnitTest
    {
        [Fact]
        public void Given_Configuration_When_GetConnectionStrings_Then_DictionaryArrayRetrived()
        {
            var expectedConnections = 5;
            var configuration = UnitTestHelper.MockedConfiguration(5);

            var result = configuration.GetConnectionStrings();

            Assert.Equal(expectedConnections, result.Count);
        }

        [Fact]
        public void Given_ConfigurationWithEmptyConnections_When_GetConnectionStrings_Then_DictionaryArrayRetrived()
        {
            var configuration = UnitTestHelper.MockedConfiguration();

            var result = configuration.GetConnectionStrings();

            Assert.False(result.Any());
        }

        [Fact]
        public void Given_NullConfiguration_When_GetConnectionStrings_Then_NullReferenceExceptionThrown()
        {
            IConfiguration configuration = null;

            Assert.Throws<NullReferenceException>(() => configuration.GetConnectionStrings());
        }

        //Invalid Test, not possible to mock Get extension method from Microsoft.Extensions.Configuration.Binder
        //[Fact]
        //public void Given_ConfigurationAndType_When_GetSettings_Then_ObjectRetrived()
        //{
        //    var configuration = ConfigurationManagerUnitTestHelper.MockedConfiguration();

        //    var result = configuration.GetSettings<TestConfig>();

        //    Assert.Equal(result.TestInt, TestConstants.TEST_CONFIG_INT);
        //    Assert.Equal(result.TestString, TestConstants.TEST_CONFIG_STRING);
        //}

    }
}
