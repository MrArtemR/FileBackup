using System;
using FileBackup.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .AddCommandLine(args)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();
            var setting = configuration.Get<AppSettings>();

            var loglevel = LogLevel.Information;
            if(!string.IsNullOrEmpty(setting.LogLevel))
            {
                loglevel = (LogLevel)Enum.Parse(typeof(LogLevel), setting.LogLevel); ;
            }

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(loglevel);
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();


            Console.WriteLine("Hello World!");
        }
    }
}
