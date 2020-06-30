using ChustaSoft.Tools.SecureConfig.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChustaSoft.Tools.SecureConfig.UnitTests
{

    [TestClass]
    public class EncrypterManagerUnitTest
    {

        [TestMethod]
        public void Given_AppSettingsObject_When_Encrypt_Then_EncryptedStringRetrived()
        {
            var appSettings = UnitTestHelper.GenerateTestConfig();

            var result = EncrypterManager.Encrypt(appSettings, UnitTestHelper.PRIVATE_TEST_KEY);

            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void Given_EncryptedString_When_Decrypt_Then_AppSetingsRetrived()
        {
            var appSettings = UnitTestHelper.GenerateTestConfig();
            var encryptedResult = EncrypterManager.Encrypt(appSettings, UnitTestHelper.PRIVATE_TEST_KEY);

            var decryptedResult = EncrypterManager.Decrypt<TestConfig>(encryptedResult, UnitTestHelper.PRIVATE_TEST_KEY);

            Assert.AreEqual(appSettings.ConnectionStrings.Count, decryptedResult.ConnectionStrings.Count);
            Assert.AreEqual(appSettings.TestInt, decryptedResult.TestInt);
            Assert.AreEqual(appSettings.TestString, decryptedResult.TestString);
        }

    }
}
