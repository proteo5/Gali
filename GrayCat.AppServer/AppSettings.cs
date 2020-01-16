using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrayCat.AppServer
{
    public static class AppSettings
    {
        public static IConfigurationRoot Configuration { get; set; }

        static AppSettings()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }
        public static string HMACKey
        {
            get
            {
                return Configuration["Config:HMACKey"];
            }
        }
    }
}
