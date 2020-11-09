using System;
using FileBackup.BL;
using FileBackup.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            //считываем путь к файлу из командной строки
            var configurationCommandLine = new ConfigurationBuilder()
               .AddCommandLine(args).Build();
            var pathToFile = configurationCommandLine.Get<PathToSettingCommandLine>();

            IConfigurationRoot configuration = null;
            //конфиг в командной строке
            if (pathToFile != null && !string.IsNullOrEmpty(pathToFile.PathToSetting))
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile(pathToFile.PathToSetting, optional: true)
                    .Build();
            }
            else //параметры в командной или опционально в файле по умолчанию
            {
                configuration = new ConfigurationBuilder()
               .AddCommandLine(args)
               .AddJsonFile("appsettings.json", optional: true)
               .Build();
            }
          
            var setting = configuration.Get<AppSettings>();
            //считываем уроовень логирования из настроек
            var loglevel = LogLevel.Information;
            if(!string.IsNullOrEmpty(setting.LogLevel))
            {
                loglevel = (LogLevel)Enum.Parse(typeof(LogLevel), setting.LogLevel); ;
            }

            //логирование 
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(loglevel);
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();

            logger.LogInformation("Logger created");

            var backup = new BackupDirectoryFiles(logger);

            backup.Backup(setting.SourceDirectory, setting.TargetDirectory, setting.Archive, setting.Exceptions);

            logger.LogInformation("Success");
        }
    }
}
