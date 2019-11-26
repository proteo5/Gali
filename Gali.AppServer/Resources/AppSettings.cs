using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gali.AppServer.Resources
{

    public static class AppSettings
    {
        public static IConfigurationRoot Configuration { get; set; }

        static AppSettings()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public static string ConnStringMongoDBServer
        {
            get
            {
                return Configuration["ConnString:MongoDB:Server"];
            }
        }
        public static string ConnStringMongoDBDatabase
        {
            get
            {
                return Configuration["ConnString:MongoDB:Database"];
            }
        }

    }
}
