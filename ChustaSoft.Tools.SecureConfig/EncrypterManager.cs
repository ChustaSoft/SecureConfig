using ChustaSoft.Common.Helpers;
using Newtonsoft.Json;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class EncrypterManager
    {

        public static string Encrypt<TSettings>(TSettings appSettings, string privateKey)
            where TSettings : new()
        {
            var serializedSettings = JsonConvert.SerializeObject(appSettings);
            var configEncrypted = EncryptationHelper.Encrypt(serializedSettings, privateKey);

            return configEncrypted;
        }

        public static TSettings Decrypt<TSettings>(string encryptedResult, string privateKey)
            where TSettings : new()
        {
            var decryptedJson = EncryptationHelper.Decrypt(encryptedResult, privateKey);
            var deserializedSettings = JsonConvert.DeserializeObject<TSettings>(decryptedJson);

            return deserializedSettings;
        }

    }
}
