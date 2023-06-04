using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class KolonneRepository : Repository<Kolonne>
    {
        public Kolonne GetKolonne(int id)
        {
            Kolonne kolonne = null;
            foreach (var item in base.GetAll())
            {
                if (item.Id == id)
                {
                    kolonne = item;
                    break;
                }
            }
            return kolonne;
        }

        public bool ContainsNummer(string nummer)
        {
            foreach (Kolonne kolonne in this.GetAll())
            {
                if (nummer.CaseInsensitiveMatch(kolonne.Nummer))
                    return true;
            }
            return false;
        }

    }
}
