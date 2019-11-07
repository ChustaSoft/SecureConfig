using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class ServicesExtensions
    {

        public static void SetUpSecureConfig<TSettings>(this IServiceCollection services, IConfiguration configuration, string privateKeyParam)
            where TSettings : class, new()
        {
            var appSettings = GetSettings<TSettings>(configuration, privateKeyParam);

            services.AddSingleton(new EncryptionKey { Key = privateKeyParam });
            services.AddSingleton(appSettings);
        }


        private static TSettings GetSettings<TSettings>(IConfiguration configuration, string privateKeyParam) 
            where TSettings : new()
        {
            var encryptedValue = configuration.GetEncryptedValue();

            if (string.IsNullOrEmpty(encryptedValue))
                return configuration.GetSettings<TSettings>();
            else
                return EncrypterManager.Decrypt<TSettings>(encryptedValue, privateKeyParam);
        }

    }
}
