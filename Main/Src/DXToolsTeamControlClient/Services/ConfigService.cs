using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace DXTools.Azure.TeamControl.Client.Services
{
    public class ConfigService
    {
        public string GetConnectionConfigValue(string key, string fileName)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(fileName);
            if (configFile.ConnectionStrings.ConnectionStrings[key] != null)
            {
                return configFile.ConnectionStrings.ConnectionStrings[key].ConnectionString;
            }
            else
            {
                return "";
            }

        }

        public void UpdateConnectionConfig(string key, string connectionString, string fileName)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(fileName);

            if (configFile.ConnectionStrings.ConnectionStrings[key] != null)
            {
                configFile.ConnectionStrings.ConnectionStrings[key].ConnectionString = connectionString;
            }
            else
            {
                configFile.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings
                {
                    Name = key,
                    ConnectionString = connectionString
                });
            }

            configFile.Save();
        }

        public string GetConfigValue(string key, string fileName)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(fileName);
            if (configFile.AppSettings.Settings.AllKeys.Contains(key))
            {
                return configFile.AppSettings.Settings[key].Value;
            }
            else
            {
                return "";
            }

        }

        public void UpdateConfig(string key, string value, string fileName)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(fileName);

            if (configFile.AppSettings.Settings[key] != null)
            {
                configFile.AppSettings.Settings[key].Value = value;
            }
            else
            {
                configFile.AppSettings.Settings.Add(key, value);
            }

            configFile.Save();
        }
    }
}
