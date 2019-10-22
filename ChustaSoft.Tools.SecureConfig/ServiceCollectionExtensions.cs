using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWritable<TSettings>(this IServiceCollection services, IConfigurationSection section, string file = "appsettings.json") 
            where TSettings : AppSettingsBase, new()
        {
            services.Configure<TSettings>(section);
            services.AddTransient<IWritableSettings<TSettings>>(provider =>
            {
                var environment = provider.GetService<IHostingEnvironment>();
                var options = provider.GetService<IOptionsMonitor<TSettings>>();

                return new WritableSettings<TSettings>(environment, options, section.Key, file);
            });
        }
    }
}