using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public abstract class SqlCeReadableDatabase<T> : ReaderDatabaseContext<T>
    {
        public SqlCeReadableDatabase(string connectionString)
            : base(connectionString)
        {
        }

        protected override DbConnection CreateConnection(string connectionString)
        {
            return new SqlCeConnection(connectionString);
        }

        protected override DbCommand CreateCommand(string query, DbConnection connection)
        {
            return new SqlCeCommand(query, (SqlCeConnection)connection);
        }


    }
}
