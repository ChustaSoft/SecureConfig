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
                var writableOptions = GetWritebleSettings<TSettings>(scope);

                if (!writableOptions.IsAlreadyEncrypted())
                {
                    var encryptedConfiguration = GetEncryptedConfiguration<TSettings>(scope);
                    writableOptions.ApplyEncryptation(encryptedConfiguration);
                }

                ///TODO
                ///2. Check environments
            }
        }

        private static IWritableSettings<TSettings> GetWritebleSettings<TSettings>(IServiceScope scope) where TSettings : AppSettingsBase, new()
        {
            var hostingEnvironment = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
            var optionsMonitor = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<TSettings>>();
            var writableOptions = new WritableSettings<TSettings>(hostingEnvironment, optionsMonitor, AppConstants.DEFAULT_SETTINGS_PARAM_NAME, "appsettings.json");

            return writableOptions;
        }

        private static string GetEncryptedConfiguration<TSettings>(IServiceScope scope) where TSettings : AppSettingsBase, new()
        {
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var settings = config.GetSettings<TSettings>();
            var encryptationKey = config.GetPrivateKey();
            var encryptedConfiguration = EncrypterManager.Encrypt(settings, encryptationKey);

            return encryptedConfiguration;
        }
    }
}
