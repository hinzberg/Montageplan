using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL
{
    /// <summary>
    /// Überprüft, ob das übergebene Verzeichnis vorhanden ist. Wenn es nicht vorhanden ist, wird das Verzeichnis erstellt.
    /// </summary>
    public static class DirectoryCreator
    {
        /// <summary>
        /// Überprüft, ob das übergebene Verzeichnis vorhanden ist. Wenn es nicht vorhanden ist, wird das Verzeichnis erstellt.
        /// </summary>
        /// <param name="dir">Verzeichnis</param>
        public static void CreateIfNotExists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch (Exception)
                {
                    // !! Hier darf nicht geloggt werden, wenn diese Methode in der Klasse 'ErrorLogger' aufgerufen wird.
                    // Sonst ensteht eine Endlosschleife.
                }
            }
        }

    }
}
