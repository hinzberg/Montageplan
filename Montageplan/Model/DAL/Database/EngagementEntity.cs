using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class EngagementEntity
    {
        public static EngagementEntity Create(Engagement engagement)
        {
            EngagementEntity entity = new EngagementEntity();
            entity.Id = engagement.Id;
            entity.Date = engagement.Date;
            entity.KolonneId = engagement.Kolonne.Id;
            foreach (var mitar in engagement.Mitarbeiter.GetAll())
            {
                EngagementMitarbeiterEntity engMit = new EngagementMitarbeiterEntity();
                engMit.EngagementId = entity.Id;
                engMit.MitarbeiterId = mitar.Mitarbeiter.Id;
                engMit.IstFuehrer = mitar.IstFuehrer;
                entity.MitarbeiterEntities.Add(engMit);
            }
            foreach (var engProItem in engagement.Projekte.GetAll())
            {
                EngagementProjektEntity engPro = EngagementProjektEntity.Create(engagement.Id, engProItem);
                entity.ProjektEntities.Add(engPro);
            }

            return entity;
        }

        public EngagementEntity()
        {
            this.Id = 0;
            this.KolonneId = 0;
            this.MitarbeiterEntities = new List<EngagementMitarbeiterEntity>();
            this.ProjektEntities = new List<EngagementProjektEntity>();
            this.Date = DateTime.MinValue;
        }

        public int Id { get; set; }
        public int KolonneId { get; set; }
        public List<EngagementMitarbeiterEntity> MitarbeiterEntities { get; set; }
        public List<EngagementProjektEntity> ProjektEntities { get; set; }
        public DateTime Date { get; set; }

    }
}
