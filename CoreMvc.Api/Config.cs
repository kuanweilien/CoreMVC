using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMvc.Api
{
    public class Config
    {
        private readonly IConfiguration config;

        public Config(IConfiguration configruration)
        {
            this.config = configruration;
        }

        public string GetAllowedHosts()
        {
            return config["AllowedHosts"];
        }
        public string GetApiKey(string keyName)
        {
            return config.GetValue<string>($"AppSeettings:ApiKey:{keyName}");
        }
    }
}
