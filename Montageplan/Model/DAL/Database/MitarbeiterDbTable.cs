using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class MitarbeiterDbTable : SqlCeReadableDatabase<Mitarbeiter>
    {
        public MitarbeiterDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<Mitarbeiter> RealAll()
        {
            return base.Read("SELECT * FROM Mitarbeiter");
        }

        public void InsertOrUpdate(Mitarbeiter mitarbeiter)
        {
            if (mitarbeiter.Id == 0)
                this.Insert(mitarbeiter);
            else
                this.Update(mitarbeiter);
        }

        public void Insert(Mitarbeiter mitarbeiter)
        {
            StringBuilder query = new StringBuilder("INSERT INTO Mitarbeiter ");
            query.Append("(IsActive, FunktionId, KannFuehrerSein, Name, Vorname, Description) VALUES ");
            query.Append("(@IsActive, @FunktionId, @KannFuehrerSein, @Name, @Vorname, @Description)");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@IsActive", mitarbeiter.IsActive));
            paramList.Add(new SqlCeParameter("@FunktionId", this.GetFunktionDbValue(mitarbeiter.Funktion)));
            paramList.Add(new SqlCeParameter("@KannFuehrerSein", mitarbeiter.KannFuehrerSein));
            paramList.Add(new SqlCeParameter("@Name", mitarbeiter.Name));
            paramList.Add(new SqlCeParameter("@Vorname", mitarbeiter.Vorname));
            paramList.Add(new SqlCeParameter("@Description", this.GetDescriptionDbValue(mitarbeiter.Bezeichnung)));

            mitarbeiter.Id = base.ExecuteQueryGetIdentity(query.ToString(), paramList.ToArray());
        }

        public void Update(Mitarbeiter mitarbeiter)
        {
            StringBuilder query = new StringBuilder("UPDATE Mitarbeiter SET ");
            query.Append("IsActive=@IsActive, FunktionId=@FunktionId, KannFuehrerSein=@KannFuehrerSein, Name=@Name, Vorname=@Vorname, Description=@Description ");
            query.Append("WHERE Id=@Id");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", mitarbeiter.Id));
            paramList.Add(new SqlCeParameter("@IsActive", mitarbeiter.IsActive));
            paramList.Add(new SqlCeParameter("@FunktionId", this.GetFunktionDbValue(mitarbeiter.Funktion)));
            paramList.Add(new SqlCeParameter("@KannFuehrerSein", mitarbeiter.KannFuehrerSein));
            paramList.Add(new SqlCeParameter("@Name", mitarbeiter.Name));
            paramList.Add(new SqlCeParameter("@Vorname", mitarbeiter.Vorname));
            paramList.Add(new SqlCeParameter("@Description", this.GetDescriptionDbValue(mitarbeiter.Bezeichnung)));

            base.ExecuteQuery(query.ToString(), paramList.ToArray());
        }

        public void Delete(Mitarbeiter mitarbeiter)
        {
            string query = "DELETE FROM Mitarbeiter WHERE Id=@Id";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", mitarbeiter.Id));

            base.ExecuteQueryGetIdentity(query, paramList.ToArray());
        }

        // SqlCeReadableDatabase<Mitarbeiter>
        protected override Mitarbeiter ReadEntity(System.Data.IDataRecord data)
        {
            Mitarbeiter mitarbeiter = new Mitarbeiter();
            mitarbeiter.Id = (int)data["Id"];
            mitarbeiter.IsActive = (bool)data["IsActive"];
            mitarbeiter.KannFuehrerSein = (bool)data["KannFuehrerSein"];
            mitarbeiter.Name = (string)data["Name"];
            mitarbeiter.Vorname = (string)data["Vorname"];
            if (DBNull.Value != data["Description"]) mitarbeiter.Bezeichnung = (string)data["Description"];

            int funktionenId = (DBNull.Value != data["FunktionId"]) ? (int)data["FunktionId"] : 0;
            if (funktionenId > 0)
            {
                mitarbeiter.Funktion = new MitarbeiterFunktion();
                mitarbeiter.Funktion.Id = funktionenId;
            }

            return mitarbeiter;
        }

        private object GetFunktionDbValue(MitarbeiterFunktion funktion)
        {
            int? id = null;
            if (funktion != null)
                id = funktion.Id;

            return base.ConvertValueForDbParam(id);
        }

        private object GetDescriptionDbValue(string description)
        {
            object formatDescrip = null;
            if (!string.IsNullOrEmpty(description))
                formatDescrip = description;

            return base.ConvertValueForDbParam(formatDescrip);
        }

    }
}
