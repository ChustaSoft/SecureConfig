using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class WebHostExtensions
    {

        #region Public methods

        public static IWebHost EncryptSettings<TSettings>(this IWebHost webHost, bool encrypt)
            where TSettings : AppSettingsBase, new()
        {
            switch (encrypt)
            {
                case true:
                    PerformSettingsEncryption<TSettings>(webHost);
                    break;

                case false:
                    PerformSettingsDecryption<TSettings>(webHost);
                    break;
            }

            return webHost;
        }

        #endregion


        #region Private methods

        private static void PerformSettingsDecryption<TSettings>(IWebHost webHost) where TSettings : AppSettingsBase, new()
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var writableOptions = GetWritebleSettings<TSettings>(scope);

                if (writableOptions.IsAlreadyEncrypted())
                {
                    var decryptedConfiguration = GetDecryptedConfiguration<TSettings>(scope);
                    writableOptions.Apply(decryptedConfiguration);
                }

                ///TODO
                ///2. Check environments
            }
        }

        private static void PerformSettingsEncryption<TSettings>(IWebHost webHost) where TSettings : AppSettingsBase, new()
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var writableOptions = GetWritebleSettings<TSettings>(scope);

                if (!writableOptions.IsAlreadyEncrypted())
                {
                    var encryptedConfiguration = GetEncryptedConfiguration<TSettings>(scope);
                    writableOptions.Apply(encryptedConfiguration);
                }

                ///TODO
                ///2. Check environments
            }
        }

        private static IWritableSettings<TSettings> GetWritebleSettings<TSettings>(IServiceScope scope)
            where TSettings : AppSettingsBase, new()
        {
            var hostingEnvironment = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
            var optionsMonitor = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<TSettings>>();
            var writableOptions = new WritableSettings<TSettings>(hostingEnvironment, optionsMonitor, AppConstants.DEFAULT_SETTINGS_PARAM_NAME, "appsettings.json");

            return writableOptions;
        }

        private static string GetEncryptedConfiguration<TSettings>(IServiceScope scope)
            where TSettings : AppSettingsBase, new()
        {
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var settings = config.GetSettings<TSettings>();
            var encryptationKey = config.GetPrivateKey();
            var encryptedConfiguration = EncrypterManager.Encrypt(settings, encryptationKey);

            return encryptedConfiguration;
        }

        private static TSettings GetDecryptedConfiguration<TSettings>(IServiceScope scope)
            where TSettings : AppSettingsBase, new()
        {
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var encryptedValue = config.GetEncryptedValue();
            var encryptationKey = config.GetPrivateKey();
            var settings = EncrypterManager.Decrypt<TSettings>(encryptedValue, encryptationKey);

            return settings;
        }

        #endregion

    }
}
