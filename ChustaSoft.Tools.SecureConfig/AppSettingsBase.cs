using System.Collections.Generic;

namespace ChustaSoft.Tools.SecureConfig
{
    public class AppSettingsBase
    {

        public IDictionary<string, string> ConnectionStrings { get; set; }

        public string this[string key] { get => ConnectionStrings[key]; }

        public string EncryptedValue { get; internal set; }


        public AppSettingsBase() { }

    }
}
