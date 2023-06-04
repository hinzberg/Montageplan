using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model.DAL.Database
{
    public class KolonnenMitarbeiterItem
    {
        public KolonnenMitarbeiterItem()
            : this(0, 0, false)
        {
        }
        public KolonnenMitarbeiterItem(int mitarbeiterId, int kolonneId, bool istFuehrer)
        {
            this.MitarbeiterId = mitarbeiterId;
            this.KolonneId = kolonneId;
            this.IstFuehrer = istFuehrer;
        }

        public int MitarbeiterId { get; set; }
        public int KolonneId { get; set; }
        public bool IstFuehrer { get; set; }

    }
}
