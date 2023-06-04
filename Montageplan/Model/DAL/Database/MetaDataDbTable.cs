using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class MetaDataDbTable : SqlCeReadableDatabase<MetaDataItem>
    {
        public MetaDataDbTable(string connectionString)
            : base(connectionString)
        {
        }

        public List<MetaDataItem> ReadAll()
        {
            return base.Read("SELECT * FROM MetaData");
        }

        // SqlCeReadableDatabase<MetaDataItem>
        protected override MetaDataItem ReadEntity(System.Data.IDataRecord data)
        {
            MetaDataItem item = new MetaDataItem();
            if (DBNull.Value != data["MetaKey"]) item.Key = (string)data["MetaKey"];
            if (DBNull.Value != data["Value"]) item.Value = (string)data["Value"];
            return item;
        }

    }
}
