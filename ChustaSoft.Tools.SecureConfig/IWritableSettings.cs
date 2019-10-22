using Microsoft.Extensions.Options;
using System;

namespace ChustaSoft.Tools.SecureConfig
{
    public interface IWritableSettings<out TSettings> : IOptionsSnapshot<TSettings> 
        where TSettings : AppSettingsBase, new()
    {
        void Update(Action<TSettings> applyChanges);
    }
}
