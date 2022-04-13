using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using AspNetHosting = Microsoft.AspNetCore.Hosting;

namespace ChustaSoft.Tools.SecureConfig
{
    internal static class IServiceProviderExtensions
    {
        
        private const string SETTINGS_FILE_PATTERN = "appsettings*.json";


        internal static void PerformSettingsDecryption<TSettings>(this IServiceProvider services, string settingsParamName)
            where TSettings : class, new()
        {
            using (var scope = services.CreateScope())
            {
                foreach (var environmentFile in GetSettingFiles(scope))
                {
                    var writableOptions = GetWritebleSettings<TSettings>(scope, environmentFile, settingsParamName);

                    if (writableOptions.IsAlreadyEncrypted)
                    {
                        var decryptedConfiguration = GetDecryptedConfiguration<TSettings>(scope, writableOptions);
                        writableOptions.Apply(decryptedConfiguration);
                    }
                }
            }
        }

        internal static void PerformSettingsEncryption<TSettings>(this IServiceProvider services, string settingsParamName)
            where TSettings : class, new()
        {
            using (var scope = services.CreateScope())
            {
                foreach (var environmentFile in GetSettingFiles(scope))
                {
                    var writableOptions = GetWritebleSettings<TSettings>(scope, environmentFile, settingsParamName);

                    if (!writableOptions.IsAlreadyEncrypted)
                    {
                        var encryptedConfiguration = GetEncryptedConfiguration(scope, writableOptions);
                        writableOptions.Apply(encryptedConfiguration);
                    }
                }
            }
        }

        private static IWritableSettings<TSettings> GetWritebleSettings<TSettings>(IServiceScope scope, string fileName, string settingsParamName)
            where TSettings : class, new()
        {
#if (NETCOREAPP3_1 || NET5_0 || NET6_0)
            var hostingEnvironment = scope.ServiceProvider.GetRequiredService<AspNetHosting.IWebHostEnvironment>();
#elif (NETCOREAPP2_1)
            var hostingEnvironment = scope.ServiceProvider.GetRequiredService<AspNetHosting.IHostingEnvironment>();
#else
#endif
            var optionsMonitor = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<TSettings>>();
            var writableOptions = new WritableSettings<TSettings>(hostingEnvironment, optionsMonitor, settingsParamName, fileName);

            return writableOptions;
        }

        private static string GetEncryptedConfiguration<TSettings>(IServiceScope scope, IWritableSettings<TSettings> writableSettings)
            where TSettings : class, new()
        {
            var encryptationKey = scope.ServiceProvider.GetRequiredService<EncryptionKey>().Key;
            var encryptedConfiguration = EncrypterManager.Encrypt(writableSettings.DecryptedSettings, encryptationKey);

            return encryptedConfiguration;
        }

        private static TSettings GetDecryptedConfiguration<TSettings>(IServiceScope scope, IWritableSettings<TSettings> writableSettings)
            where TSettings : class, new()
        {
            var encryptationKey = scope.ServiceProvider.GetRequiredService<EncryptionKey>().Key;
            var settings = EncrypterManager.Decrypt<TSettings>(writableSettings.EncryptedSettings.EncryptedValue, encryptationKey);

            return settings;
        }

        private static IEnumerable<string> GetSettingFiles(IServiceScope scope)
        {
#if (NETCOREAPP3_1 || NET5_0 || NET6_0)
            var assemblyFolder = scope.ServiceProvider.GetRequiredService<AspNetHosting.IWebHostEnvironment>().ContentRootPath;
#elif (NETCOREAPP2_1)
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return '\\';
            else
                return '/';
        }

    }
}
