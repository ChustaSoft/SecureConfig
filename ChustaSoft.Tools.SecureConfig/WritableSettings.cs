using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ChustaSoft.Tools.SecureConfig
{

    #region Contract

    public interface IWritableSettings<TSettings> : IOptionsSnapshot<TSettings>
        where TSettings : class, new()
    {
        TSettings DecryptedSettings { get; }
        EncryptedConfiguration EncryptedSettings { get; }
        bool IsEncrypted { get; }

        void Apply(string encryptedValue);
        void Apply(TSettings decryptedObj);

    }

    #endregion


    #region Implementation

    public class WritableSettings<TSettings> : IWritableSettings<TSettings>
        where TSettings : class, new()
    {

#if (NETCOREAPP3_1 || NET5_0 || NET6_0)
        private readonly IWebHostEnvironment _environment;
#elif (NETCOREAPP2_1)
        private readonly IHostingEnvironment _environment;
#else
#endif
        private readonly SettingsConverter _converter;
        private readonly IOptionsMonitor<TSettings> _options;
        private readonly string _section;
        private readonly string _file;


        public TSettings Value => _options.CurrentValue;
        public TSettings Get(string name) => _options.Get(name);


#if (NETCOREAPP3_1 || NET5_0 || NET6_0)
        public WritableSettings(IWebHostEnvironment environment, IOptionsMonitor<TSettings> options, string section, string file)
            : this(options, section, file)
        {
            _environment = environment;
        }
#endif

#if (NETCOREAPP2_1)
        public WritableSettings(IHostingEnvironment environment, IOptionsMonitor<TSettings> options, string section, string file)
            : this(options, section, file)
        {
            _environment = environment;
        }
#endif

        public WritableSettings(IOptionsMonitor<TSettings> options, string section, string file)
        {
            _options = options;
            _section = section;
            _file = file;
            _converter = new SettingsConverter();
        }


        public TSettings DecryptedSettings
        {
            get
            {
                var filePath = GetPhysicalPath();

                return _converter.Get<TSettings>(filePath, _section);
            }
        }

        public EncryptedConfiguration EncryptedSettings
        {
            get
            {
                var filePath = GetPhysicalPath();

                return _converter.Get<EncryptedConfiguration>(filePath, _section);
            }
        }

        public bool IsEncrypted 
        { 
            get 
            {
                var physicalPath = GetPhysicalPath();
                var jObject = GetJsonSettingsObject(physicalPath);
                var encryptedValue = GetEncryptedValue(jObject);

                return !string.IsNullOrWhiteSpace(encryptedValue);
            } 
        }
        

        public void Apply(string encryptedValue)
        {
            var physicalPath = GetPhysicalPath();
            var jObject = GetJsonSettingsObject(physicalPath);
            var encryptedObj = JObject.Parse(JsonConvert.SerializeObject(new EncryptedConfiguration { EncryptedValue = encryptedValue }));

            jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(encryptedObj));

            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }

        public void Apply(TSettings decryptedObj)
        {
            var physicalPath = GetPhysicalPath();
            var jObject = GetJsonSettingsObject(physicalPath);
            
            jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(decryptedObj));

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

        private string GetEncryptedValue(JObject jObject)
        {
            return jObject[_section]?[nameof(EncryptedConfiguration.EncryptedValue)]?.Value<string>();
        }

    }

    #endregion

}
