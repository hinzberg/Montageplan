using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class EngagementProjektEntity
    {
        public static EngagementProjektEntity Create(int engagementId, EngagementProjekt engProjekt)
        {
            EngagementProjektEntity entity = new EngagementProjektEntity();
            entity.EngagementId = engagementId;
            entity.ProjektId = engProjekt.Projekt.Id;
            entity.StartTime = engProjekt.StartTime;
            entity.EndTime = engProjekt.EndTime;
            entity.TempEngagementProjekt = engProjekt;
            return entity;
        }


        public EngagementProjektEntity()
        {
            this.Id = 0;
            this.EngagementId = 0;
            this.ProjektId = 0;
            this.StartTime = null;
            this.EndTime = null;
            this.TempEngagementProjekt = null;
        }

        public int Id { get; set; }
        public int EngagementId { get; set; }
        public int ProjektId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Referenz wird verwendet, um beim Inserieren eines Datensatzes den PK in das "Hauptobjekt" zu speichern.
        /// Die Entities werden nur für Datenbankaktionen instanziert.
        /// </summary>
        public EngagementProjekt TempEngagementProjekt { get; set; }

    }
}
