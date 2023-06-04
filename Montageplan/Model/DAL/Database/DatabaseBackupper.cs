using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class DatabaseBackupper
    {
        private readonly string directory;

        public DatabaseBackupper(string directory)
        {
            this.directory = directory;
        }

        public void Create(string databaseFile)
        {
            DirectoryCreator.CreateIfNotExists(this.directory);
            string name = this.GetBackupName();
            string destination = string.Format(@"{0}\{1}", this.directory, name);
            this.CopyBackupFile(databaseFile, destination);
        }

        private void CopyBackupFile(string source, string destination)
        {
            try
            {
                File.Copy(source, destination);
            }
            catch (Exception exp)
            {
                AppConfig.Logger.Write("Datenbank-Backup fehlgeschlagen", exp.Message);
            }
        }

        private string GetBackupName()
        {
            string name = string.Format("Montageplan {0:dd.MM.yyyy HH.mm.ss} [{1}].sdf",
                DateTime.Now, AppConfig.Version.ProductVersion);

            return name;
        }


    }
}
