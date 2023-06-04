using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class DatabaseMetaData
    {
        private const string DATABASE_VERSION_KEY = "DatabaseVersion";

        private readonly MetaDataDbTable dbTable;

        public DatabaseMetaData(string connectionString)
        {
            this.dbTable = new MetaDataDbTable(connectionString);
            this.DatabaseVersion = 0;
        }

        public int DatabaseVersion { get; set; }


        public void ReadMetaData()
        {
            this.dbTable.OpenConnection();
            List<MetaDataItem> items = this.dbTable.ReadAll();
            this.dbTable.CloseConnection();

            foreach (var item in items)
            {
                switch (item.Key)
                {
                    case DATABASE_VERSION_KEY:
                        int? dbVersion = this.GetIntValue(item.Value);
                        if (dbVersion.HasValue)
                            this.DatabaseVersion = dbVersion.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        public void UpdateDatabaseVersion(int version)
        {
            this.SaveValue(DATABASE_VERSION_KEY, version.ToString());
            this.DatabaseVersion = version;
        }

        private void SaveValue(string key, string value)
        {
            string selectQuery = string.Format("SELECT MetaKey FROM MetaData WHERE MetaKey = '{0}'", key);
            string insertQuery = string.Format("INSERT INTO MetaData (MetaKey, Value) VALUES ('{0}', '{1}')", key, value);
            string updateQuery = string.Format("UPDATE MetaData SET Value='{0}' WHERE MetaKey='{1}'", value, key);

            bool containsKey = this.dbTable.ContainsEntityOnSelect(selectQuery);
            if (containsKey)
                this.dbTable.ExecuteQuery(updateQuery);
            else
                this.dbTable.ExecuteQuery(insertQuery);

            this.dbTable.CloseConnection();
        }

        private int? GetIntValue(string value)
        {
            int? iValue = null;
            int tempValue;
            if (int.TryParse(value, out tempValue))
                iValue = tempValue;

            return iValue;
        }


    }
}
