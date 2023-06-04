using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class KolonnenMitarbeiterDbTable : SqlCeReadableDatabase<KolonnenMitarbeiterItem>
    {
        public KolonnenMitarbeiterDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<KolonnenMitarbeiterItem> RealAll()
        {
            return base.Read("SELECT * FROM KolonnenMitarbeiter");
        }

        public void Insert(KolonnenMitarbeiterItem item)
        {
            StringBuilder query = new StringBuilder("INSERT INTO KolonnenMitarbeiter ");
            query.Append("(MitarbeiterId, KolonneId, IstFuehrer) VALUES ");
            query.Append("(@MitarbeiterId, @KolonneId, @IstFuehrer)");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@MitarbeiterId", item.MitarbeiterId));
            paramList.Add(new SqlCeParameter("@KolonneId", item.KolonneId));
            paramList.Add(new SqlCeParameter("@IstFuehrer", item.IstFuehrer));

            base.ExecuteQuery(query.ToString(), paramList.ToArray());
        }

        public void Delete(int mitarbeiterId, int kolonneId)
        {
            string query = "DELETE FROM KolonnenMitarbeiter WHERE MitarbeiterId=@MitarbeiterId AND KolonneId=@KolonneId";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@MitarbeiterId", mitarbeiterId));
            paramList.Add(new SqlCeParameter("@KolonneId", kolonneId));

            base.ExecuteQueryGetIdentity(query, paramList.ToArray());
        }

        // SqlCeReadableDatabase<KolonnenMitarbeiterItem>
        protected override KolonnenMitarbeiterItem ReadEntity(System.Data.IDataRecord data)
        {
            KolonnenMitarbeiterItem item = new KolonnenMitarbeiterItem();
            item.MitarbeiterId = (int)data["MitarbeiterId"];
            item.KolonneId = (int)data["KolonneId"];
            item.IstFuehrer = (bool)data["IstFuehrer"];
            return item;
        }

    }
}
