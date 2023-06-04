using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class EngagementMitarbeiterEntity
    {
        public EngagementMitarbeiterEntity()
            : this(0, 0, false)
        {
        }
        public EngagementMitarbeiterEntity(int engagementId, int mitarbeiterId, bool istFuehrer)
        {
            this.EngagementId = engagementId;
            this.MitarbeiterId = mitarbeiterId;
            this.IstFuehrer = istFuehrer;
        }

        public int EngagementId { get; set; }
        public int MitarbeiterId { get; set; }
        public bool IstFuehrer { get; set; }

    }
}
