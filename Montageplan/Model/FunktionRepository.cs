using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class FunktionRepository : Repository<MitarbeiterFunktion>
    {
        public MitarbeiterFunktion GetFunktion(int id)
        {
            MitarbeiterFunktion funktion = null;
            foreach (var item in base.GetAll())
            {
                if (item.Id == id)
                {
                    funktion = item;
                    break;
                }
            }
            return funktion;
        }

        public override bool Contains(MitarbeiterFunktion item)
        {
            foreach (MitarbeiterFunktion knownItem in base.GetAll())
            {
                if (item.Name.CaseInsensitiveMatch(knownItem.Name))
                    return true;
            }
            return false;
        }

    }
}
