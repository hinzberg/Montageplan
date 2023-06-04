using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class EngagementsMitarbeiterDbTable : SqlCeReadableDatabase<EngagementMitarbeiterEntity>
    {
        public EngagementsMitarbeiterDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<EngagementMitarbeiterEntity> RealAll()
        {
            return base.Read("SELECT * FROM EngagementsMitarbeiter");
        }

        public List<EngagementMitarbeiterEntity> ReadByEngagement(int engagementId)
        {
            string query = "SELECT * FROM EngagementsMitarbeiter WHERE EngagementsId=@EngagementsId";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@EngagementsId", engagementId));

            return base.Read(query, paramList.ToArray());
        }

        public void Insert(EngagementMitarbeiterEntity entity)
        {
            StringBuilder query = new StringBuilder("INSERT INTO EngagementsMitarbeiter ");
            query.Append("(EngagementsId, MitarbeiterId, IstFuehrer) VALUES ");
            query.Append("(@EngagementsId, @MitarbeiterId, @IstFuehrer)");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@EngagementsId", entity.EngagementId));
            paramList.Add(new SqlCeParameter("@MitarbeiterId", entity.MitarbeiterId));
            paramList.Add(new SqlCeParameter("@IstFuehrer", entity.IstFuehrer));

            base.ExecuteQuery(query.ToString(), paramList.ToArray());
        }

        public void DeleteByEngagement(EngagementMitarbeiterEntity entity)
        {
            string query = "DELETE FROM EngagementsMitarbeiter WHERE EngagementsId=@EngagementsId";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@EngagementsId", entity.EngagementId));

            base.ExecuteQueryGetIdentity(query, paramList.ToArray());
        }

        // SqlCeReadableDatabase<EngagementMitarbeiterEntity>
        protected override EngagementMitarbeiterEntity ReadEntity(System.Data.IDataRecord data)
        {
            EngagementMitarbeiterEntity entity = new EngagementMitarbeiterEntity();
            entity.EngagementId = (int)data["EngagementsId"];
            entity.MitarbeiterId = (int)data["MitarbeiterId"];
            entity.IstFuehrer = (bool)data["IstFuehrer"];

            return entity;
        }
    }
}
