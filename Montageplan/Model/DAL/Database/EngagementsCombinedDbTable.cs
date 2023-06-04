using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class EngagementsCombinedDbTable : SqlCeReadableDatabase<EngagementEntity>
    {
        public EngagementsCombinedDbTable()
            : base(AppConfig.DatabaseConnectionString)
        {
        }

        public List<EngagementEntity> RealAll()
        {
            return base.Read("SELECT Id, KolonneId, Date FROM Engagements");
        }

        public void Insert(EngagementEntity entity)
        {
            int engId = this.InsertEngagement(entity);
            if (engId > 0)
            {
                entity.Id = engId;

                EngagementsMitarbeiterDbTable engMitTable = new EngagementsMitarbeiterDbTable();
                foreach (var mitarEntity in entity.MitarbeiterEntities)
                {
                    mitarEntity.EngagementId = engId;
                    engMitTable.Insert(mitarEntity);
                }
                EngagementsProjekteDbTable engProTable = new EngagementsProjekteDbTable();
                foreach (var proEntity in entity.ProjektEntities)
                {
                    proEntity.EngagementId = engId;
                    int pk = engProTable.Insert(proEntity);
                    proEntity.TempEngagementProjekt.Id = pk;
                }
            }
        }

        //public void UpdateProjekt(int engagementId, int projektId)
        //{
        //    StringBuilder query = new StringBuilder("UPDATE Engagements SET ");
        //    query.Append("ProjektId=@ProjektId ");
        //    query.Append("WHERE Id=@Id");

        //    List<SqlCeParameter> paramList = new List<SqlCeParameter>();
        //    paramList.Add(new SqlCeParameter("@Id", engagementId));
        //    paramList.Add(new SqlCeParameter("@ProjektId", projektId));

        //    base.ExecuteQuery(query.ToString(), paramList.ToArray());
        //}

        ////public void InsertProjektLink(EngagementProjektEntity)
        ////{
        ////}

        ////public void InsertMitarbeiterLink(int engagementId, int mitarbeiterId, bool istFuehrer)
        ////{
        ////    StringBuilder query = new StringBuilder("INSERT INTO EngagementsMitarbeiter ");
        ////    query.Append("(EngagementsId, MitarbeiterId, IstFuehrer) VALUES ");
        ////    query.Append("(@EngagementsId, @MitarbeiterId, @IstFuehrer)");

        ////    List<SqlCeParameter> paramList = new List<SqlCeParameter>();
        ////    paramList.Add(new SqlCeParameter("@EngagementsId", engagementId));
        ////    paramList.Add(new SqlCeParameter("@MitarbeiterId", mitarbeiterId));
        ////    paramList.Add(new SqlCeParameter("@IstFuehrer", istFuehrer));

        ////    base.ExecuteQuery(query.ToString(), paramList.ToArray());
        ////}

        public void UpdateMitarbeiterLink(int engagementId, int mitarbeiterId, bool istFuehrer)
        {
            StringBuilder query = new StringBuilder("UPDATE EngagementsMitarbeiter ");
            query.Append("SET IstFuehrer=@IstFuehrer ");
            query.Append("WHERE EngagementsId=@EngagementsId AND MitarbeiterId=@MitarbeiterId");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@EngagementsId", engagementId));
            paramList.Add(new SqlCeParameter("@MitarbeiterId", mitarbeiterId));
            paramList.Add(new SqlCeParameter("@IstFuehrer", istFuehrer));

            base.ExecuteQuery(query.ToString(), paramList.ToArray());
        }

        public void DeleteMitarbeiterLink(int engagementId, int mitarbeiterId)
        {
            string query = "DELETE FROM EngagementsMitarbeiter WHERE EngagementsId=@EngagementsId AND MitarbeiterId=@MitarbeiterId";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@EngagementsId", engagementId));
            paramList.Add(new SqlCeParameter("@MitarbeiterId", mitarbeiterId));

            base.ExecuteQueryGetIdentity(query, paramList.ToArray());
        }

        public void Delete(EngagementEntity entity)
        {
            // Die Einträge der Tabellen "EngagementsMitarbeiter" und "EngagementsProjekte" werden durch
            // den Fremdschlüssel EngagementId automatisch gelöscht
            this.DeleteEngagement(entity.Id);
        }

        private void DeleteEngagement(int engagementId)
        {
            string query = "DELETE FROM Engagements WHERE Id=@Id";

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@Id", engagementId));

            base.ExecuteQuery(query, paramList.ToArray());
        }







        // SqlCeReadableDatabase<EngagementEntity>
        protected override EngagementEntity ReadEntity(System.Data.IDataRecord data)
        {
            EngagementEntity entity = new EngagementEntity();
            entity.Id = (int)data["Id"];
            entity.KolonneId = (int)data["KolonneId"];
            entity.Date = (DateTime)data["Date"];

            EngagementsMitarbeiterDbTable engMitTable = new EngagementsMitarbeiterDbTable();
            entity.MitarbeiterEntities = engMitTable.ReadByEngagement(entity.Id);

            EngagementsProjekteDbTable engProTable = new EngagementsProjekteDbTable();
            entity.ProjektEntities = engProTable.ReadByEngagement(entity.Id);

            return entity;
        }

        private int InsertEngagement(EngagementEntity entity)
        {
            int id = 0;
            StringBuilder query = new StringBuilder("INSERT INTO Engagements ");
            query.Append("(KolonneId, Date) VALUES ");
            query.Append("(@KolonneId, @Date)");

            List<SqlCeParameter> paramList = new List<SqlCeParameter>();
            paramList.Add(new SqlCeParameter("@KolonneId", entity.KolonneId));
            paramList.Add(new SqlCeParameter("@Date", entity.Date));

            id = base.ExecuteQueryGetIdentity(query.ToString(), paramList.ToArray());
            return id;
        }


    }
}
