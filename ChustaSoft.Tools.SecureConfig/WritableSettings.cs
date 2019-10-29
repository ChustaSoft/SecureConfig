using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ChustaSoft.Tools.SecureConfig
{
    public class WritableSettings<TSettings> : IWritableSettings<TSettings> 
        where TSettings : AppSettingsBase, new()
    {

        private readonly IHostingEnvironment _environment;
        private readonly IOptionsMonitor<TSettings> _options;
        private readonly string _section;
        private readonly string _file;


        public TSettings Value => _options.CurrentValue;
        public TSettings Get(string name) => _options.Get(name);


        public WritableSettings(IHostingEnvironment environment, IOptionsMonitor<TSettings> options, string section, string file)
        {
            _environment = environment;
            _options = options;
            _section = section;
            _file = file;
        }

        public bool IsAlreadyEncrypted()
        {
            var physicalPath = GetPhysicalPath();
            var jObject = GetJsonSettingsObject(physicalPath);
            var encryptedValue = jObject[_section][nameof(EncryptedConfiguration.EncryptedValue)]?.Value<string>();

            return !string.IsNullOrWhiteSpace(encryptedValue);
        }

        public void ApplyEncryptation(string encryptedValue)
        {
            var physicalPath = GetPhysicalPath();
            var jObject = GetJsonSettingsObject(physicalPath);
            
            var encryptedObj = JObject.Parse(JsonConvert.SerializeObject(new { EncryptedValue = encryptedValue }));

            jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(encryptedObj));

            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }


        private JObject GetJsonSettingsObject(string physicalPath)
        {
            return JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
        }

        private string GetPhysicalPath()
        {
            var fileProvider = _environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(_file);
            var physicalPath = fileInfo.PhysicalPath;

            return physicalPath;
        }

    }
}
