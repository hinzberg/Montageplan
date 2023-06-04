using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class FunktionenDbTable : SqlCeReadableDatabase<MitarbeiterFunktion>
    {
        public FunktionenDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<MitarbeiterFunktion> RealAll()
        {
            return base.Read("SELECT * FROM Funktionen");
        }

        public void InsertOrUpdate(MitarbeiterFunktion funktion)
        {
            if (funktion.Id == 0)
                this.Insert(funktion);
            else
                this.Update(funktion);
        }

        public void Insert(MitarbeiterFunktion funktion)
        {
            StringBuilder query = new StringBuilder("INSERT INTO Funktionen ");
            query.Append("(Name) VALUES ");
            query.Append("(@Name)");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Name", funktion.Name));

            funktion.Id = base.ExecuteQueryGetIdentity(query.ToString(), paramList.ToArray());
        }

        public void Update(MitarbeiterFunktion funktion)
        {
            StringBuilder query = new StringBuilder("UPDATE Funktionen SET ");
            query.Append("Name=@Name ");
            query.Append("WHERE Id=@Id");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", funktion.Id));
            paramList.Add(new SqlCeParameter("@Name", funktion.Name));

            base.ExecuteQuery(query.ToString(), paramList.ToArray());
        }

        public void Delete(MitarbeiterFunktion funktion)
        {
            string query = "DELETE FROM Funktionen WHERE Id=@Id";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", funktion.Id));

            base.ExecuteQueryGetIdentity(query, paramList.ToArray());
        }

        // SqlCeReadableDatabase<MitarbeiterFunktion>
        protected override MitarbeiterFunktion ReadEntity(System.Data.IDataRecord data)
        {
            MitarbeiterFunktion funktion = new MitarbeiterFunktion();
            funktion.Id = (int)data["Id"];
            funktion.Name = (string)data["Name"];

            return funktion;
        }

    }
}
