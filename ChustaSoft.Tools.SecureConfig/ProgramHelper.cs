using Microsoft.AspNetCore.Builder;
using AspNetHosting = Microsoft.AspNetCore.Hosting;
using CommonHosting = Microsoft.Extensions.Hosting;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class WebHostExtensions
    {

#if (NET6_0)

        public static WebApplication EncryptSettings<TSettings>(this WebApplication webApplication, bool encrypt = true, string settingsParamName = AppConstants.DEFAULT_SETTINGS_PARAM_NAME)
            where TSettings : class, new()
        {
            switch (encrypt)
            {
                case true:
                    webApplication.Services.PerformSettingsEncryption<TSettings>(settingsParamName);
                    break;

                case false:
                    webApplication.Services.PerformSettingsDecryption<TSettings>(settingsParamName);
                    break;
            }

            return webApplication;
        }

#elif (NETCOREAPP3_1 || NET5_0)

        public static CommonHosting.IHost EncryptSettings<TSettings>(this CommonHosting.IHost host, bool encrypt, string settingsParamName = AppConstants.DEFAULT_SETTINGS_PARAM_NAME)
            where TSettings : class, new()
        {
            switch (encrypt)
            {
                case true:
                    host.Services.PerformSettingsEncryption<TSettings>(settingsParamName);
                    break;

                case false:
                    host.Services.PerformSettingsDecryption<TSettings>(settingsParamName);
                    break;
            }

            return host;
        }

#elif (NETCOREAPP2_1)

        public static AspNetHosting.IWebHost EncryptSettings<TSettings>(this AspNetHosting.IWebHost webHost, bool encrypt, string settingsParamName = AppConstants.DEFAULT_SETTINGS_PARAM_NAME)
            where TSettings : class, new()
        {
            switch (encrypt)
            {
                case true:
                    webHost.Services.PerformSettingsEncryption<TSettings>(settingsParamName);
                    break;

                case false:
                    webHost.Services.PerformSettingsDecryption<TSettings>(settingsParamName);
                    break;
            }

            return webHost;
        }

#else
#endif

    }
}