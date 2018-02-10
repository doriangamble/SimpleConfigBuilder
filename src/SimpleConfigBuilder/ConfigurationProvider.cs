using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.SimpleConfiguration
{
    public class AppSettingsConfigurationProvider : IConfigurationProvider
    {
        public string GetValue(string key) => ConfigurationManager.AppSettings.Get(key);
    }
}
