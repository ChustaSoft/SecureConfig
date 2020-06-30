using ChustaSoft.Tools.SecureConfig.UnitTests.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ChustaSoft.Tools.SecureConfig.UnitTests
{

    [TestClass]
    public class ConfigurationExtensionsUnitTest
    {
        [TestMethod]
        public void Given_Configuration_When_GetConnectionStrings_Then_DictionaryArrayRetrived()
        {
            var expectedConnections = 5;
            var configuration = UnitTestHelper.MockedConfiguration(5);

            var result = configuration.GetConnectionStrings();

            Assert.AreEqual(expectedConnections, result.Count);
        }

        [TestMethod]
        public void Given_ConfigurationWithEmptyConnections_When_GetConnectionStrings_Then_DictionaryArrayRetrived()
        {
            var configuration = UnitTestHelper.MockedConfiguration();

            var result = configuration.GetConnectionStrings();

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void Given_NullConfiguration_When_GetConnectionStrings_Then_NullReferenceExceptionThrown()
        {
            IConfiguration configuration = null;

            Assert.ThrowsException<NullReferenceException>(() => configuration.GetConnectionStrings());
        }

    }
}
