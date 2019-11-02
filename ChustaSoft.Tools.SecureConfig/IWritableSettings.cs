using Microsoft.Extensions.Options;

namespace ChustaSoft.Tools.SecureConfig
{
    public interface IWritableSettings<TSettings> : IOptionsSnapshot<TSettings> 
        where TSettings : class, new()
    {

        bool IsAlreadyEncrypted();

        void Apply(string encryptedValue);

        void Apply(TSettings decryptedObj);

    }
}
