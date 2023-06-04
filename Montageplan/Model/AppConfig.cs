using Montageplan.Model.DAL;
using Montageplan.Model.DAL.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Montageplan.Model
{
    /// <summary>
    /// Programmhinweise wie Versionsnummer und Erstelldatum werden hier abgerufen.
    /// Zusätzlich werden die Benutzereinstellungen gespeichert und geladen.
    /// </summary>
    public static class AppConfig
    {
        public const double KOLONNE_DEFAULT_HEIGHT = 493;
        public const double KOLONNE_COLLAPSED_HEIGHT = 46;
        public const double MIN_DAY_ITEMS_WIDTH = 150;
        public const double FIXED_DAY_ITEMS_WIDTH = 250;

        public const int REQUIRED_DB_VERSION = 34;

        public const string APP_NAME = "Montageplan";
        public const string RELEASE_STATE = "Beta";
        public const string DATABASE_NAME = "Montageplan.sdf";
        public const string DATABASE_PWD = "montageplan";
        public const string BACKUPS_DIR_NAME = "backups";

        private static FileVersionInfo version;
        private static string rootDirectory;
        private static string appTitle;
        private static DatabaseMetaData metaData;
        private static string databaseConnectionString;
        private static DateTime assemblyDate;
        private static string databaseFile;
        private static string backupsDirectory;

        public static FileVersionInfo Version
        {
            get { return version; }
        }
        public static string AppTitle
        {
            get { return appTitle; }
        }
        public static string RootDirectory
        {
            get { return rootDirectory; }
        }
        public static DatabaseMetaData MetaData
        {
            get { return metaData; }
        }
        public static string DatabaseConnectionString
        {
            get { return databaseConnectionString; }
        }
        public static DateTime AssemblyDate
        {
            get { return assemblyDate; }
        }
        public static string DatabaseFile
        {
            get { return databaseFile; }
        }
        public static string BackupsDirectory
        {
            get { return backupsDirectory; }
        }

        public static bool UserCanModify { get; set; }
        public static UserSettings Settings { get; set; }
        public static ErrorLogger Logger { get; set; }
        public static bool IsMitarbeiterListVisible { get; set; }

        /// <summary>
        /// Initialisiert die Klasse und holt sich einmalig Informationen zum Programm.
        /// </summary>
        public static void Init()
        {
            try
            {
                version = GetVersion();
                UserCanModify = true;
                rootDirectory = GetRootDirectory(version.FileName);
                assemblyDate = GetAssemblyCompileDate(version.FileName);
                appTitle = BuildAppTitle(version.FileName, assemblyDate);
                Logger = new ErrorLogger(version.ProductVersion, rootDirectory);
                Settings = new UserSettings();
                databaseFile = string.Format(@"{0}\{1}", rootDirectory, DATABASE_NAME);
                databaseConnectionString = BuildConnectionString(databaseFile);
                backupsDirectory = string.Format(@"{0}\{1}", rootDirectory, BACKUPS_DIR_NAME);
                metaData = new DatabaseMetaData(databaseConnectionString);
                IsMitarbeiterListVisible = true;
            }
            catch (Exception)
            {
                // TODO (EventLog?)
            }
        }

        /// <summary>
        /// Speichert die Benutzereinstellungen als xml-Datei ab.
        /// </summary>
        public static void Save()
        {
            ConfigsXml xml = new ConfigsXml(rootDirectory);
            xml.Save(Settings);
        }

        /// <summary>
        /// Läd die Benutzereinstellungen von der xml-Datei.
        /// </summary>
        public static void Load()
        {
            ConfigsXml xml = new ConfigsXml(rootDirectory);
            Settings = xml.Load();
        }

        /// <summary>
        /// Erstellt einen Text mit wichtigen Programminformationen, die z.B. in dem Hauptfenster 
        /// angezeigt werden und gibt den Text zurück.
        /// </summary>
        /// <param name="fileName">exe-Datei</param>
        /// <returns></returns>
        private static string BuildAppTitle(string fileName, DateTime assemblyDate)
        {
            string title;

            string releaseState = "";
            if (RELEASE_STATE != "")
                releaseState = "  " + RELEASE_STATE;

            title = string.Format("{0}  [{1}]  [{2}]{3}", APP_NAME, assemblyDate.ToShortDateString(), version.ProductVersion, releaseState);
            return title;
        }

        /// <summary>
        /// Ermittelt das Kompilierdatum des Programms und gibt es zurück.
        /// </summary>
        /// <param name="fileName">exe-Datei</param>
        /// <returns></returns>
        private static DateTime GetAssemblyCompileDate(string fileName)
        {
            DateTime date;
            FileInfo info = new FileInfo(fileName);
            date = info.LastWriteTime;
            return date;
        }

        /// <summary>
        /// Ermittelt die Version des Programms und gibt das sie zurück.
        /// </summary>
        /// <returns></returns>
        private static FileVersionInfo GetVersion()
        {
            FileVersionInfo version;
            Assembly current = Assembly.GetExecutingAssembly();
            string file = current.GetModules()[0].FullyQualifiedName;
            version = FileVersionInfo.GetVersionInfo(file);
            return version;
        }

        /// <summary>
        /// Ermittelt das Stammverzeichnis und gibt es zurück.
        /// </summary>
        /// <param name="fileName">exe-Datei</param>
        /// <returns></returns>
        private static string GetRootDirectory(string fileName)
        {
            string rootDir;
            FileInfo inf = new FileInfo(fileName);
            rootDir = inf.DirectoryName;
            return rootDir;
        }

        private static string BuildConnectionString(string databaseFile)
        {
            string connString;
            connString = string.Format("Data Source={0};password={1};Max Database Size=2048;", databaseFile, DATABASE_PWD);
            return connString;
        }

    }
}
