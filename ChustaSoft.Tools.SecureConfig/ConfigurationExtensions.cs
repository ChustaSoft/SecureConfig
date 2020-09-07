using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class ConfigurationExtensions
    {

        public static IDictionary<string, string> GetConnectionStrings(this IConfiguration configuration, string connectionsParamName = AppConstants.DEFAULT_CONNECTIONS_PARAM_NAME)
        {
            var connections = new Dictionary<string, string>();
            var connectionStrings = configuration.GetSection(connectionsParamName).GetChildren();

            foreach (var connection in connectionStrings)
                connections.Add(connection.Key, connection.Value);

            return connections;
        }

        public static TSettings GetSettings<TSettings>(this IConfiguration configuration, string settingsParamName)
            where TSettings : new()
        {
            var settings = configuration.GetSection(settingsParamName).Get<TSettings>();

            return settings;
        }

        public static string GetEncryptedValue(this IConfiguration configuration, string settingsParamName)
        {
            return configuration.GetSettings<EncryptedConfiguration>(settingsParamName)?.EncryptedValue;
        }

    }
}
