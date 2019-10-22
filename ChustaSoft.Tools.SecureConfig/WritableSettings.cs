using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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


        public void Update(Action<TSettings> applyChanges)
        {
            var fileProvider = _environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(_file);
            var physicalPath = fileInfo.PhysicalPath;
            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
            var sectionObject = jObject.TryGetValue(_section, out JToken section) ?
                JsonConvert.DeserializeObject<TSettings>(section.ToString()) : (Value ?? new TSettings());

            applyChanges(sectionObject);

            jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }

    }
}
