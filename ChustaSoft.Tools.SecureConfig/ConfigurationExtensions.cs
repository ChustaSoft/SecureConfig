using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ChustaSoft.Tools.SecureConfig
{
    public static class ConfigurationExtensions
    {

        private const string DEFAULT_CONNECTIONS_PARAM_NAME = "ConnectionStrings";
        private const string DEFAULT_SETTINGS_PARAM_NAME = "AppSettings";


        public static IDictionary<string, string> GetConnectionStrings(this IConfiguration configuration, string connectionsParamName = DEFAULT_CONNECTIONS_PARAM_NAME)
        {
            var connections = new Dictionary<string, string>();
            var connectionStrings = configuration.GetSection(connectionsParamName).GetChildren();

            foreach (var connection in connectionStrings)
                connections.Add(connection.Key, connection.Value);

            return connections;
        }

        public static TSettings GetSettings<TSettings>(this IConfiguration configuration, string settingsParamName = DEFAULT_SETTINGS_PARAM_NAME)
            where TSettings : AppSettingsBase, new()
        {
            var settings = configuration.GetSection(settingsParamName).Get<TSettings>();

            return settings;
        }

    }
}
