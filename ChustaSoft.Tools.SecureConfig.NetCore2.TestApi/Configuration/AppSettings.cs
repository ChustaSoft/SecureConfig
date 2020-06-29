using System.Collections.Generic;

namespace ChustaSoft.Tools.SecureConfig.TestApi.Configuration
{
    public class AppSettings
    {

        public IDictionary<string, string> ConnectionStrings { get; set; }
        public string this[string key] { get => ConnectionStrings[key]; }
        public int TestInt { get; set; }
        public string TestString { get; set; }

        public AppSettings() { }

    }
}
