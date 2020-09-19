using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using AspNetHosting = Microsoft.AspNetCore.Hosting;
using CommonHosting = Microsoft.Extensions.Hosting;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class WebHostExtensions
    {

        #region Constants

        private const string SETTINGS_FILE_PATTERN = "appsettings*.json";

        #endregion


        #region Public methods


        #if (NETCOREAPP3_0 || NETCOREAPP3_1)
        public static CommonHosting.IHost EncryptSettings<TSettings>(this CommonHosting.IHost host, bool encrypt, string settingsParamName = AppConstants.DEFAULT_SETTINGS_PARAM_NAME)
            where TSettings : class, new()
        {
            switch (encrypt)
            {
                case true:
                    PerformSettingsEncryption<TSettings>(host.Services, settingsParamName);
                    break;

                case false:
                    PerformSettingsDecryption<TSettings>(host.Services, settingsParamName);
                    break;
            }

            return host;
        }

        #elif (NETCOREAPP2_1 || NETCOREAPP2_2)
        public static AspNetHosting.IWebHost EncryptSettings<TSettings>(this AspNetHosting.IWebHost webHost, bool encrypt, string settingsParamName = AppConstants.DEFAULT_SETTINGS_PARAM_NAME)
            where TSettings : class, new()
        {
            switch (encrypt)
            {
                case true:
                    PerformSettingsEncryption<TSettings>(webHost.Services, settingsParamName);
                    break;

                case false:
                    PerformSettingsDecryption<TSettings>(webHost.Services, settingsParamName);
                    break;
            }

            return webHost;
        }
        #else
        #endif

        #endregion


        #region Private methods

        private static void PerformSettingsDecryption<TSettings>(IServiceProvider services, string settingsParamName)
            where TSettings : class, new()
        {
            using (var scope = services.CreateScope())
            {
                foreach (var environmentFile in GetSettingFiles(scope))
                {
                    var writableOptions = GetWritebleSettings<TSettings>(scope, environmentFile, settingsParamName);

                    if (writableOptions.IsAlreadyEncrypted())
                    {
                        var decryptedConfiguration = GetDecryptedConfiguration<TSettings>(scope, settingsParamName);
                        writableOptions.Apply(decryptedConfiguration);
                    }
                }
            }
        }

        private static void PerformSettingsEncryption<TSettings>(IServiceProvider services, string settingsParamName)
            where TSettings : class, new()
        {
            using (var scope = services.CreateScope())
            {
                foreach (var environmentFile in GetSettingFiles(scope))
                {
                    var writableOptions = GetWritebleSettings<TSettings>(scope, environmentFile, settingsParamName);

                    if (!writableOptions.IsAlreadyEncrypted())
                    {
                        var encryptedConfiguration = GetEncryptedConfiguration<TSettings>(scope, settingsParamName);
                        writableOptions.Apply(encryptedConfiguration);
                    }
                }
            }
        }

        private static IWritableSettings<TSettings> GetWritebleSettings<TSettings>(IServiceScope scope, string fileName, string settingsParamName)
            where TSettings : class, new()
        {
#if (NETCOREAPP3_0 || NETCOREAPP3_1)
            var hostingEnvironment = scope.ServiceProvider.GetRequiredService<AspNetHosting.IWebHostEnvironment>();
#elif (NETCOREAPP2_1 || NETCOREAPP2_2)
            var hostingEnvironment = scope.ServiceProvider.GetRequiredService<AspNetHosting.IHostingEnvironment>();
#else
#endif      
            var optionsMonitor = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<TSettings>>();
            var writableOptions = new WritableSettings<TSettings>(hostingEnvironment, optionsMonitor, settingsParamName, fileName);

            return writableOptions;
        }

        private static string GetEncryptedConfiguration<TSettings>(IServiceScope scope, string settingsParamName)
            where TSettings : class, new()
        {
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var settings = config.GetSettings<TSettings>(settingsParamName);
            var encryptationKey = scope.ServiceProvider.GetRequiredService<EncryptionKey>().Key;
            var encryptedConfiguration = EncrypterManager.Encrypt(settings, encryptationKey);

            return encryptedConfiguration;
        }

        private static TSettings GetDecryptedConfiguration<TSettings>(IServiceScope scope, string settingsParamName)
            where TSettings : class, new()
        {
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var encryptedValue = config.GetEncryptedValue(settingsParamName);
            var encryptationKey = scope.ServiceProvider.GetRequiredService<EncryptionKey>().Key;
            var settings = EncrypterManager.Decrypt<TSettings>(encryptedValue, encryptationKey);

            return settings;
        }

        private static IEnumerable<string> GetSettingFiles(IServiceScope scope)
        {
#if (NETCOREAPP3_0 || NETCOREAPP3_1)
            var assemblyFolder = scope.ServiceProvider.GetRequiredService<AspNetHosting.IWebHostEnvironment>().ContentRootPath;
#elif (NETCOREAPP2_1 || NETCOREAPP2_2)
            var assemblyFolder = scope.ServiceProvider.GetRequiredService<AspNetHosting.IHostingEnvironment>().ContentRootPath;
#else
#endif         
            var files = Directory.GetFiles(assemblyFolder, SETTINGS_FILE_PATTERN)
                .Select(x => x.Substring(x.LastIndexOf(GetProperSeparator()) + 1))
                .ToList();

            return files;
        }

        private static char GetProperSeparator()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return '\\';
            else
                return '/';
        }

        #endregion

    }
}
