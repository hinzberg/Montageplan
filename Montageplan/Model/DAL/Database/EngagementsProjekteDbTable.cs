using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class EngagementsProjekteDbTable : SqlCeReadableDatabase<EngagementProjektEntity>
    {
        public EngagementsProjekteDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<EngagementProjektEntity> RealAll()
        {
            return base.Read("SELECT * FROM EngagementsProjekte");
        }

        public List<EngagementProjektEntity> Delete(int id)
        {
            string query = "DELETE FROM EngagementsProjekte WHERE Id=@Id";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", id));

            return base.Read(query, paramList.ToArray());
        }

        public List<EngagementProjektEntity> ReadByEngagement(int engagementId)
        {
            string query = "SELECT * FROM EngagementsProjekte WHERE EngagementId=@EngagementId";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@EngagementId", engagementId));

            return base.Read(query, paramList.ToArray());
        }

        public int Insert(EngagementProjektEntity entity)
        {
            int pk;
            StringBuilder query = new StringBuilder("INSERT INTO EngagementsProjekte ");
            query.Append("(EngagementId, ProjektId, StartTime, EndTime) VALUES ");
            query.Append("(@EngagementId, @ProjektId, @StartTime, @EndTime)");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@EngagementId", entity.EngagementId));
            paramList.Add(new SqlCeParameter("@ProjektId", entity.ProjektId));
            paramList.Add(new SqlCeParameter("@StartTime", base.ConvertValueForDbParam(entity.StartTime)));
            paramList.Add(new SqlCeParameter("@EndTime", base.ConvertValueForDbParam(entity.EndTime)));

            pk = base.ExecuteQueryGetIdentity(query.ToString(), paramList.ToArray());
            entity.Id = pk;
            return pk;
        }

        public void DeleteByEngagement(EngagementMitarbeiterEntity entity)
        {
            string query = "DELETE FROM EngagementsProjekte WHERE EngagementId=@EngagementId";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@EngagementId", entity.EngagementId));

            base.ExecuteQueryGetIdentity(query, paramList.ToArray());
        }

        // SqlCeReadableDatabase<EngagementProjektEntity>
        protected override EngagementProjektEntity ReadEntity(System.Data.IDataRecord data)
        {
            EngagementProjektEntity entity = new EngagementProjektEntity();
            entity.Id = (int)data["Id"];
            entity.EngagementId = (int)data["EngagementId"];
            entity.ProjektId = (int)data["ProjektId"];
            if (data["StartTime"] != DBNull.Value) entity.StartTime = (DateTime)data["StartTime"];
            if (data["EndTime"] != DBNull.Value) entity.EndTime = (DateTime)data["EndTime"];

            return entity;
        }
    }
}
