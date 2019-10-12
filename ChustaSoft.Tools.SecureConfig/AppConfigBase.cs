using System.Collections.Generic;

namespace ChustaSoft.Tools.SecureConfig
{
    public class AppConfigBase
    {

        public IDictionary<string, string> ConnectionStrings { get; set; }

        public string this[string key] { get => ConnectionStrings[key]; }


        public AppConfigBase() { }


    }
}
