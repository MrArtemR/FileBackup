using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using FileBackup.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO.Compression;
using System.Linq;

namespace FileBackup.BL
{
    public class BackupDirectoryFiles : IFileBackup
    {
        private ILogger _logger;

        public BackupDirectoryFiles(ILogger logger)
        {
            _logger = logger;
        }

        public void Backup(IEnumerable<string> sourcesDirectory, string targetDirectory, bool acrhive = false, IEnumerable<string> exceptions = null)
        {
            if(Directory.Exists(targetDirectory))
            {
                Parallel.ForEach(sourcesDirectory, sourceDirectory =>
                {
                    if(Directory.Exists(sourceDirectory))
                    {
                        var directoryName = Path.GetFileNameWithoutExtension(sourceDirectory);
                        var subDirectory = Path.Combine(targetDirectory, $"{directoryName} {DateTime.Now:MM.dd.yyyy HH:mm:ss}");                      
                        if(acrhive)
                        {
                            ZipFile.CreateFromDirectory(sourceDirectory, $"{subDirectory}.zip");
                            _logger.LogInformation("Archive created");
                        }
                        else
                        {
                            Directory.CreateDirectory(subDirectory);
                            CopyDirectory(sourceDirectory, subDirectory, exceptions);
                            _logger.LogInformation("Folder copied");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Source directory does not exist");
                    }
                });
            }
            else
            {
                _logger.LogWarning("Target directory does not exist");
            }
        }

        private void CopyDirectory(string sourcesDirectory, string targetDirectory, IEnumerable<string> exceptions)
        {
            string[] files = Directory.GetFiles(sourcesDirectory);
            Parallel.ForEach (files,  file =>
            {
                if(exceptions == null || !exceptions.Contains(Path.GetExtension(file)))
                {
                    var fileName = Path.GetFileName(file);
                    var destFile = Path.Combine(targetDirectory, fileName);
                    File.Copy(file, destFile, true);
                    _logger.LogInformation($"File copy: {fileName}");
                }
            });
        }

    }
}
