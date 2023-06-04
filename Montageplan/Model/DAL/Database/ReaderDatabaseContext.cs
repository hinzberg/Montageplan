using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public abstract class ReaderDatabaseContext<T> : DatabaseContext
    {
        protected abstract T ReadEntity(IDataRecord data);

        public ReaderDatabaseContext(string connectionString)
            : base(connectionString)
        {
        }

        protected List<T> Read(string query)
        {
            return this.Read(query, null);
        }
        protected virtual List<T> Read(string query, DbParameter[] paramAry)
        {
            List<T> entities = new List<T>();
            base.OpenConnection();
            using (DbCommand command = this.CreateCommand(query, base.GetConnection()))
            {
                if (paramAry != null)
                    command.Parameters.AddRange(paramAry);
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T entity = this.ReadEntity((IDataRecord)reader);
                        if (entity != null)
                            entities.Add(entity);
                    }
                }
            }
            return entities;
        }

    }
}
