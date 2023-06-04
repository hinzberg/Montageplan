using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public abstract class DatabaseContext : IDisposable
    {
        protected abstract DbConnection CreateConnection(string connectionString);
        protected abstract DbCommand CreateCommand(string query, DbConnection connection);

        private DbConnection connection;

        public DatabaseContext(string connectionString)
        {
            this.connection = this.CreateConnection(connectionString);
        }

        // IDisposable
        public void Dispose()
        {
            this.connection.Dispose();
        }

        public DbConnection GetConnection()
        {
            return this.connection;
        }

        public virtual void CheckConnection()
        {
            this.OpenConnection();
            this.CloseConnection();
        }

        public virtual void OpenConnection()
        {
            if (this.connection.State != ConnectionState.Open)
                this.connection.Open();
        }

        public virtual void CloseConnection()
        {
            if (this.connection.State != ConnectionState.Closed)
                this.connection.Close();
        }

        public void ExecuteQuery(string query)
        {
            this.ExecuteQuery(query, null);
        }
        public virtual void ExecuteQuery(string query, DbParameter[] paramAry)
        {
            this.OpenConnection();
            using (DbCommand command = this.CreateCommand(query, this.connection))
            {
                if (paramAry != null)
                    command.Parameters.AddRange(paramAry);
                command.ExecuteNonQuery();
            }
        }

        public int ExecuteQueryGetIdentity(string query)
        {
            return this.ExecuteQueryGetIdentity(query, null);
        }
        public virtual int ExecuteQueryGetIdentity(string query, DbParameter[] paramAry)
        {
            this.OpenConnection();
            int id = -1;
            using (DbCommand command = this.CreateCommand(query, this.connection))
            {
                if (paramAry != null)
                    command.Parameters.AddRange(paramAry);
                command.ExecuteNonQuery();
                id = this.SelectIdAfterInsert(command);
            } 
            return id;
        }

        public bool ContainsEntityOnSelect(string query)
        {
            return this.ContainsEntityOnSelect(query, null);
        }
        public bool ContainsEntityOnSelect(string query, DbParameter[] paramAry)
        {
            bool contains = false;
            this.OpenConnection();
            using (DbCommand command = this.CreateCommand(query, this.connection))
            {
                if (paramAry != null)
                    command.Parameters.AddRange(paramAry);
                contains = (command.ExecuteScalar() != null);
            }
            return contains;
        }

        protected virtual int SelectIdAfterInsert(DbCommand command)
        {
            int id = -1;
            command.CommandText = "SELECT @@IDENTITY";
            object idObj = command.ExecuteScalar();

            int tempId;
            if (idObj != null && int.TryParse(idObj.ToString(), out tempId))
                id = tempId;

            return id;
        }

        /// <summary>
        /// Konvertiert den Wert für die Datenbank. Alle Werte werden vollständig übernommen, bis auf NULL-Werte.
        /// Diese Werte werden zum Objekt DBNull-Value konvertiert. Der konvertierte Wert wird zurückgegeben.
        /// </summary>
        /// <param name="value">Wert</param>
        /// <returns></returns>
        protected object ConvertValueForDbParam(object value)
        {
            object newValue = (value == null) ? System.DBNull.Value : value;
            return newValue;
        }

        /// <summary>
        /// Wenn der Wert 'DateTime.MinValue' ist, dann wird 'dbMinValue' als Wert zurückgegeben, ansonsten der übergebene Wert 'value'.
        /// </summary>
        /// <param name="value">Wert, der gespeichert werden soll.</param>
        /// <param name="dbMinValue">Wert, der genommen wird, denn 'value' = 'DateTime.MinValue' ist.</param>
        /// <returns></returns>
        protected DateTime ConvertDateTimeMin(DateTime value, DateTime dbMinValue)
        {
            DateTime newValue;
            if (value == DateTime.MinValue)
                newValue = dbMinValue;
            else
                newValue = value;
            return newValue;
        }

    }
}
