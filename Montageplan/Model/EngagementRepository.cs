using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class EngagementRepository : Repository<Engagement>
    {
        public bool ContainsKolonne(Kolonne kolonne)
        {
            bool contains = false;
            foreach (var item in base.GetAll())
            {
                if (item.Kolonne == kolonne)
                {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        /// <summary>
        /// Überprüft die übergebene Auflistung von aktuellen Kolonnen, ob neue Kolonnen (unbekannte) übergeben wurden und fügt sie
        /// der Repository hinzu. Zusätzlich wird überprüft, ob Kolonnen in der Auflistung nicht mehr vorhanden sind,
        /// diese werden aus der Repository entfernt. Ergebnis ist, das die Repository die gleichen Kolonnen
        /// beinhaltet, wie die übergebene Auflistung.
        /// </summary>
        /// <param name="kolonnen">Aktuelle Kolonnen</param>
        public virtual void UniformToDayKolonnen(IList<Kolonne> kolonnen, DateTime day)
        {
            // Überprüfen, ob Items entfernt wurden
            foreach (var currItem in base.GetAll())
            {
                if (!kolonnen.Contains(currItem.Kolonne))
                    base.Remove(currItem);
            }
            // Überprüfen, ob Items neu (noch unbekannt) sind
            foreach (var item in kolonnen)
            {
                if (!this.ContainsKolonne(item))
                {
                    Engagement engagement = new Engagement(item, day);
                    engagement.RefreshMitarbeiter();
                    base.Add(engagement);
                }
            }
        }

        /// <summary>
        /// Wenn bei einer Kolonne der Führer oder Mitarbeiter verändert wurden, soll dies in den Terminen aktualisiert werden,
        /// die noch nicht als gültig gelten (Nur Vorlagen sind und noch keine tatsächlichen Aufträge)
        /// </summary>
        public void RefreshKolonnenMitarbeiterOnUnvalidEngagements()
        {
            foreach (var engagement in this.GetAll())
            {
                if (!engagement.HasProjekte)
                    engagement.RefreshMitarbeiter();
            }
        }

        /// <summary>
        /// Überprüft, ob das Projekt mindestens zu einem Termin zugeordnet wurde.
        /// </summary>
        /// <param name="projectId">Projekt-Id</param>
        /// <returns></returns>
        public bool IsProjectLinkedToEngagement(int projectId)
        {
            bool isLinked = false;
            foreach (var engagement in base.GetAll())
            {
                if (engagement.HasProjekte)
                {
                    bool contains = engagement.Projekte.Contains(projectId);
                    if (contains)
                    {
                        isLinked = true;
                        break;
                    }
                }
            }
            return isLinked;
        }

    }
}
