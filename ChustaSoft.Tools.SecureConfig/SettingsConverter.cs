using Newtonsoft.Json.Linq;
using System.IO;

namespace ChustaSoft.Tools.SecureConfig
{
    internal class SettingsConverter
    {
        public TSettings Get<TSettings>(string filePath, string sectionName)
            where TSettings : class
        {
            var jsonText = File.ReadAllText(filePath);
            var jsonObj = JObject.Parse(jsonText);
            var settingsNode = jsonObj[sectionName];
            var settingsObj = settingsNode.ToObject<TSettings>();

            return settingsObj;
        }
    }
}
