using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class MitarbeiterRepository : Repository<Mitarbeiter>
    {
        ////public List<Mitarbeiter> GetAllNameSorted()
        ////{
        ////    List<Mitarbeiter> sorted = new List<Mitarbeiter>(base.GetAll().OrderBy(item => item.GetFullName()));
        ////    return sorted;
        ////}

        public List<Mitarbeiter> GetPotentialKolonnenfuehrer()
        {
            List<Mitarbeiter> fuehrer = new List<Mitarbeiter>();
            foreach (var mitar in base.GetAll())
            {
                if (mitar.KannFuehrerSein)
                    fuehrer.Add(mitar);
            }
            return fuehrer;
        }

        public List<Mitarbeiter> GetWithFunktion(int funktionId)
        {
            List<Mitarbeiter> mitarbeiter = new List<Mitarbeiter>();
            foreach (var mitar in base.GetAll())
            {
                if (mitar.Funktion != null && mitar.Funktion.Id == funktionId)
                    mitarbeiter.Add(mitar);
            }
            return mitarbeiter;
        }

        public Mitarbeiter GetMitarbeiter(int id)
        {
            Mitarbeiter mitarbeiter = null;
            foreach (var item in base.GetAll())
            {
                if (item.Id == id)
                {
                    mitarbeiter = item;
                    break;
                }
            }
            return mitarbeiter;
        }

    }
}
