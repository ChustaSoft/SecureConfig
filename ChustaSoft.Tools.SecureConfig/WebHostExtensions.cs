using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class WebHostExtensions
    {

        public static IWebHost EncryptSettings<TSettings>(this IWebHost webHost, bool encrypt)
            where TSettings : AppSettingsBase, new()
        {
            switch (encrypt)
            {
                case true:
                    PerformSettingsEncryptation<TSettings>(webHost);
                    break;

                case false:
                    break;
            }

            return webHost;
        }

        private static void PerformSettingsEncryptation<TSettings>(IWebHost webHost) where TSettings : AppSettingsBase, new()
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var settings = config.GetSettings<TSettings>(AppConstants.DEFAULT_SETTINGS_PARAM_NAME);
                var encryptationKey = config.GetPrivateKey();
                var encryptedConfiguration = EncrypterManager.Encrypt(settings, encryptationKey);
                var hostingEnvironment = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                var optionsMonitor = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<TSettings>>();
                var writableOptions = new WritableSettings<TSettings>(hostingEnvironment, optionsMonitor, AppConstants.DEFAULT_SETTINGS_PARAM_NAME, "appsettings.json");

                writableOptions.Update(s => { s.EncryptedValue = encryptedConfiguration; });

                ///TODO
                ///1. Remove other properties once encrypted by reflection
                ///2. Check environments
            }
        }

    }
}
