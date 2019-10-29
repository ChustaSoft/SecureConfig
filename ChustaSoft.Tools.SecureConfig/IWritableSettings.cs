using Microsoft.Extensions.Options;

namespace ChustaSoft.Tools.SecureConfig
{
    public interface IWritableSettings<out TSettings> : IOptionsSnapshot<TSettings> 
        where TSettings : AppSettingsBase, new()
    {

        bool IsAlreadyEncrypted();

        void ApplyEncryptation(string encryptedValue);
    }
}
