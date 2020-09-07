using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class ServicesExtensions
    {

        public static TSettings SetUpSecureConfig<TSettings>(this IServiceCollection services, IConfiguration configuration, string privateKeyParam, string settingsParamName = AppConstants.DEFAULT_SETTINGS_PARAM_NAME)
            where TSettings : class, new()
        {
            var appSettings = GetSettings<TSettings>(configuration, privateKeyParam, settingsParamName);

            services.AddSingleton(new EncryptionKey { Key = privateKeyParam });
            services.AddSingleton(appSettings);

            return appSettings;
        }


        private static TSettings GetSettings<TSettings>(IConfiguration configuration, string privateKeyParam, string settingsParamName) 
            where TSettings : new()
        {
            var encryptedValue = configuration.GetEncryptedValue(settingsParamName);

            if (string.IsNullOrEmpty(encryptedValue))
                return configuration.GetSettings<TSettings>(settingsParamName);
            else
                return EncrypterManager.Decrypt<TSettings>(encryptedValue, privateKeyParam);
        }

    }
}
