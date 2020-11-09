using System.Collections.Generic;

namespace FileBackup.Models
{
    public interface IFileBackup
    {
        void Backup(IEnumerable<string> sources, string target, bool acrhive = false, IEnumerable<string> exceptions = null);
    }
}
