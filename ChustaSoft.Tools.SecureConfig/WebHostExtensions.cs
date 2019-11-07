using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class WebHostExtensions
    {

        #region Constants

        private const string DEVELOPMENT_SETTINGS_FILE = "appsettings.Development.json";
        private const string SETTINGS_FILE_PATTERN = "appsettings*.json";

        #endregion


        #region Public methods

        public static IWebHost EncryptSettings<TSettings>(this IWebHost webHost, bool encrypt, string secretKeyParam = AppConstants.DEFAULT_SECRETKEY_ENVCONFIG_NAME)
            where TSettings : class, new()
        {
            switch (encrypt)
            {
                case true:
                    PerformSettingsEncryption<TSettings>(webHost, secretKeyParam);
                    break;

                case false:
                    PerformSettingsDecryption<TSettings>(webHost, secretKeyParam);
                    break;
            }

            return webHost;
        }

        #endregion


        #region Private methods

        private static void PerformSettingsDecryption<TSettings>(IWebHost webHost, string secretKeyParam)
            where TSettings : class, new()
        {
            using (var scope = webHost.Services.CreateScope())
            {
                foreach (var environmentFile in GetSettingFiles(scope))
                {
                    var writableOptions = GetWritebleSettings<TSettings>(scope, environmentFile);

                    if (writableOptions.IsAlreadyEncrypted())
                    {
                        var decryptedConfiguration = GetDecryptedConfiguration<TSettings>(scope, secretKeyParam);
                        writableOptions.Apply(decryptedConfiguration);
                    }
                }
            }
        }

        private static void PerformSettingsEncryption<TSettings>(IWebHost webHost, string secretKeyParam)
            where TSettings : class, new()
        {
            using (var scope = webHost.Services.CreateScope())
            {
                foreach (var environmentFile in GetSettingFiles(scope))
                {
                    var writableOptions = GetWritebleSettings<TSettings>(scope, environmentFile);

                    if (!writableOptions.IsAlreadyEncrypted())
                    {
                        var encryptedConfiguration = GetEncryptedConfiguration<TSettings>(scope, secretKeyParam);
                        writableOptions.Apply(encryptedConfiguration);
                    }
                }
            }
        }

        private static IWritableSettings<TSettings> GetWritebleSettings<TSettings>(IServiceScope scope, string fileName)
            where TSettings : class, new()
        {
            var hostingEnvironment = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
            var optionsMonitor = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<TSettings>>();
            var writableOptions = new WritableSettings<TSettings>(hostingEnvironment, optionsMonitor, AppConstants.DEFAULT_SETTINGS_PARAM_NAME, fileName);

            return writableOptions;
        }

        private static string GetEncryptedConfiguration<TSettings>(IServiceScope scope, string secretKeyParam)
            where TSettings : class, new()
        {
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var settings = config.GetSettings<TSettings>();
            var encryptationKey = config.GetPrivateKey(secretKeyParam);
            var encryptedConfiguration = EncrypterManager.Encrypt(settings, encryptationKey);

            return encryptedConfiguration;
        }

        private static TSettings GetDecryptedConfiguration<TSettings>(IServiceScope scope, string secretKeyParam)
            where TSettings : class, new()
        {
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var encryptedValue = config.GetEncryptedValue();
            var encryptationKey = config.GetPrivateKey(secretKeyParam);
            var settings = EncrypterManager.Decrypt<TSettings>(encryptedValue, encryptationKey);

            return settings;
        }

        private static IEnumerable<string> GetSettingFiles(IServiceScope scope)
        {
            var assemblyFolder = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>().ContentRootPath; ;
            var files = Directory.GetFiles(assemblyFolder, SETTINGS_FILE_PATTERN).ToList();

            files = files.Select(x => x.Substring(x.LastIndexOf('\\') + 1)).ToList();

            if (!files.Contains(DEVELOPMENT_SETTINGS_FILE))
                files.Add(DEVELOPMENT_SETTINGS_FILE);

            return files;
        }

        #endregion

    }
}
