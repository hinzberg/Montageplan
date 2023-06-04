using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    /// <summary>
    /// Ermittelt Mitarbeiter, die einer Kolonne zugewiesen werden können.
    /// </summary>
    public class FreeMitarbeiterFilter
    {
        /// <summary>
        /// Ermittelt alle Mitarbeiter, die für die angegebene Kolonne noch zur Verfügung stehen.
        /// Mitarbeiter, die noch keiner Kolonne zugewiesen sind und keine Kolonnenfuehrer, stehen zur Verfügung.
        /// </summary>
        /// <returns></returns>
        public List<Mitarbeiter> GetFreeMitarbeiter(int kolonneId, List<Mitarbeiter> mitarbeiter, List<Kolonne> kolonnen)
        {
            List<Mitarbeiter> freeMitarbeiter = new List<Mitarbeiter>();
            foreach (var mitItem in mitarbeiter)
            {
                bool isFree = true;
                foreach (var kolItem in kolonnen)
                {
                    // Ein Kolonnenführer kann nie ein freier "Mitarbeiter" sein
                    if (kolItem.Fuehrer != null && kolItem.Fuehrer.Id == mitItem.Id)
                    {
                        isFree = false;
                        break;
                    }
                    else
                    {
                        // Die Mitarbeiter der angegeben Kolonne, sollen auch als freie Mitarbeiter gelten
                        if (kolItem.Fuehrer == null || kolItem.Fuehrer.Id != mitItem.Id)
                        {
                            foreach (var kolMitItem in kolItem.AdditionalMitarbeiter.GetAll())
                            {
                                if (kolMitItem.Id == mitItem.Id && kolItem.Id != kolonneId)
                                {
                                    isFree = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (isFree)
                    freeMitarbeiter.Add(mitItem);
            }
            return freeMitarbeiter;
        }

    }
}
