using System;
using System.Collections.Generic;

namespace FileBackup.Models
{
    public class AppSettings
    {
        public string TargetDirectory { get; set; }
        public IEnumerable<string> SourceDirectory { get; set; }
        public IEnumerable<string> Exceptions { get; set; }
        public bool Archive { get; set; }
        public string LogLevel { get; set; }
    }
}
