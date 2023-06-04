using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class SqlCeDatabaseCreator
    {
        private readonly string file;

        public SqlCeDatabaseCreator(string file)
            : this(file, "")
        {
        }
        public SqlCeDatabaseCreator(string file, string password)
        {
            this.file = file;
            this.Password = password;
        }

        public string Password { get; set; }

        public void CreateIfNotExists()
        {
            if (!File.Exists(this.file))
                this.Create();
        }
        public void Create()
        {
            string connectionString = this.BuildConnectionString();
            SqlCeEngine engine = new SqlCeEngine(connectionString);
            engine.CreateDatabase();

            if (File.Exists(this.file))
                this.CreateTableMetaData();
        }

        private string BuildConnectionString()
        {
            string connString;
            connString = string.Format("DataSource=\"{0}\";", this.file);
            if (this.Password != "")
                connString += string.Format("Password=\"{0}\";", this.Password);

            return connString;
        }

        /// <summary>
        /// MetaData Tabelle wird erstellt. Diese Aktion muss direkt nach Erzeugung der Datenbank ausgeführt werden.
        /// In einem folgenden Schritt wird diese Tabelle ausgelesen, um festzustellen, welche Version die Datenbank hat und
        /// welche Updates folgen müssen. Diese Version steht in der MetaData Tabelle.
        /// </summary>
        private void CreateTableMetaData()
        {
            string queryCreate = "CREATE TABLE MetaData (" +
                "MetaKey nvarchar(50) NOT NULL, " +
                "Value nvarchar(400) NOT NULL, " +
                "PRIMARY KEY (MetaKey))";

            string queryInsert = "INSERT INTO MetaData (MetaKey, Value) VALUES ('DatabaseVersion', '0')";

            SqlCeDatabase db = new SqlCeDatabase(AppConfig.DatabaseConnectionString);
            db.ExecuteQuery(queryCreate);
            db.ExecuteQuery(queryInsert);
            db.CloseConnection();
        }

    }
}
