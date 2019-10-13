using ChustaSoft.Tools.SecureConfig.UnitTests.Helpers;
using Xunit;

namespace ChustaSoft.Tools.SecureConfig.UnitTests
{
    public class EncrypterManagerUnitTest
    {

        [Fact]
        public void Given_AppSettingsObject_When_Encrypt_Then_EncryptedStringRetrived()
        {
            var appSettings = UnitTestHelper.GenerateTestConfig();

            var result = EncrypterManager.Encrypt(appSettings, UnitTestHelper.PRIVATE_TEST_KEY);

            Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact]
        public void Given_EncryptedString_When_Decrypt_Then_AppSetingsRetrived()
        {
            var appSettings = UnitTestHelper.GenerateTestConfig();
            var encryptedResult = EncrypterManager.Encrypt(appSettings, UnitTestHelper.PRIVATE_TEST_KEY);

            var decryptedResult = EncrypterManager.Decrypt<TestConfig>(encryptedResult, UnitTestHelper.PRIVATE_TEST_KEY);

            Assert.Equal(appSettings.ConnectionStrings.Count, decryptedResult.ConnectionStrings.Count);
            Assert.Equal(appSettings.TestInt, decryptedResult.TestInt);
            Assert.Equal(appSettings.TestString, decryptedResult.TestString);
        }

    }
}
