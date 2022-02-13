using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class StartupHelper
    {

#if (NET6_0)
        public static TSettings SetUpSecureConfig<TSettings>(this WebApplicationBuilder webApplicationBuilder, string privateKeyParam, string settingsParamName = AppConstants.DEFAULT_SETTINGS_PARAM_NAME)
            where TSettings : class, new()
        {
            return webApplicationBuilder.Services.SetUpSecureConfig<TSettings>(webApplicationBuilder.Configuration, privateKeyParam, settingsParamName);
        }
#endif

        public static TSettings SetUpSecureConfig<TSettings>(this IServiceCollection services, IConfiguration configuration, string privateKeyParam, string settingsParamName = AppConstants.DEFAULT_SETTINGS_PARAM_NAME)
            where TSettings : class, new()
        {
            var appSettings = configuration.GetSettings<TSettings>(privateKeyParam, settingsParamName);

            services.AddSingleton(new EncryptionKey { Key = privateKeyParam });
            services.AddSingleton(appSettings);

            return appSettings;
        }

    }
}
