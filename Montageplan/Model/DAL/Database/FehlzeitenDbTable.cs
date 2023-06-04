using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    /// <summary>
    /// Diese Klasse verwaltet die beiden Fehlzeitentabellen: Fehlzeiten und Fehlzeitarten.
    /// Die Einträge in der Tabelle Fehlzeitarten sind bis zum aktuellen Stand fest eingetragen (1-3) (Stand: 2013-05-31).
    /// </summary>
    public class FehlzeitenDbTable : SqlCeReadableDatabase<MitarbeiterFehlzeit>
    {
        public FehlzeitenDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<MitarbeiterFehlzeit> RealAll()
        {
            return base.Read("SELECT fehl.Id, fehl.MitarbeiterId, fehl.Startdatum, fehl.Endedatum, art.Bezeichnung " +
                "FROM Fehlzeiten AS fehl INNER JOIN Fehlzeitarten AS art ON fehl.FehlzeitartId = art.Id");
        }

        public void InsertOrUpdate(MitarbeiterFehlzeit fehlzeit, int fehlzeitartId)
        {
            if (fehlzeit.Id == 0)
                this.Insert(fehlzeit, fehlzeitartId);
            else
                this.Update(fehlzeit, fehlzeitartId);
        }

        public void Insert(MitarbeiterFehlzeit fehlzeit, int fehlzeitartId)
        {
            StringBuilder query = new StringBuilder("INSERT INTO Fehlzeiten ");
            query.Append("(MitarbeiterId, Startdatum, Endedatum, FehlzeitartId) VALUES ");
            query.Append("(@MitarbeiterId, @Startdatum, @Endedatum, @FehlzeitartId)");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@MitarbeiterId", fehlzeit.MitarbeiterId));
            paramList.Add(new SqlCeParameter("@Startdatum", fehlzeit.StartDatum));
            paramList.Add(new SqlCeParameter("@Endedatum", fehlzeit.EndeDatum));
            paramList.Add(new SqlCeParameter("@FehlzeitartId", fehlzeitartId));

            fehlzeit.Id = base.ExecuteQueryGetIdentity(query.ToString(), paramList.ToArray());
        }

        public void Update(MitarbeiterFehlzeit fehlzeit, int fehlzeitartId)
        {
            StringBuilder query = new StringBuilder("UPDATE Fehlzeiten SET ");
            query.Append("MitarbeiterId=@MitarbeiterId, Startdatum=@Startdatum, Endedatum=@Endedatum, FehlzeitartId=@FehlzeitartId");
            query.Append("WHERE Id=@Id");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@MitarbeiterId", fehlzeit.MitarbeiterId));
            paramList.Add(new SqlCeParameter("@Startdatum", fehlzeit.StartDatum));
            paramList.Add(new SqlCeParameter("@Endedatum", fehlzeit.EndeDatum));
            paramList.Add(new SqlCeParameter("@FehlzeitartId", fehlzeitartId));

            base.ExecuteQuery(query.ToString(), paramList.ToArray());
        }

        public void Delete(MitarbeiterFehlzeit fehlzeit)
        {
            string query = "DELETE FROM Fehlzeiten WHERE Id=@Id";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", fehlzeit.Id));

            base.ExecuteQueryGetIdentity(query, paramList.ToArray());
        }

        // SqlCeReadableDatabase<MitarbeiterFehlzeit>
        protected override MitarbeiterFehlzeit ReadEntity(System.Data.IDataRecord data)
        {
            MitarbeiterFehlzeit fehlzeit = new MitarbeiterFehlzeit();
            fehlzeit.Id = (int)data["fehl.Id"];
            fehlzeit.MitarbeiterId = (int)data["fehl.MitarbeiterId"];
            fehlzeit.StartDatum = (DateTime)data["fehl.Startdatum"];
            fehlzeit.EndeDatum = (DateTime)data["fehl.Endedatum"];
            fehlzeit.Bezeichnung = (string)data["art.Bezeichnung"];

            return fehlzeit;
        }
    }
}
