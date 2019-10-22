using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class ServicesExtensions
    {

        public static void RegisterSecureConfig<TSettings>(this IServiceCollection services, IConfiguration configuration, string privateKeyParam)
            where TSettings : AppSettingsBase, new()
        {
            var appSettings = configuration.GetSection(AppConstants.DEFAULT_SETTINGS_PARAM_NAME);

            services.ConfigureWritable<TSettings>(appSettings);

            var decryptedConfig = EncrypterManager.Decrypt<TSettings>(configuration.GetEncryptedValue<TSettings>(), privateKeyParam);

            services.AddSingleton(decryptedConfig);
        }

    }
}
