using Montageplan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan
{
    public class DayMitarbeiterRepository : Repository<DayMitarbeiter>
    {
        public List<DayMitarbeiter> GetAllNameSorted()
        {
            List<DayMitarbeiter> sorted = new List<DayMitarbeiter>(base.GetAll().OrderBy(item => item.Mitarbeiter.GetFullName()));
            return sorted;
        }

        public List<DayMitarbeiter> GetSortedByFuehrer()
        {
            List<DayMitarbeiter> sorted 
                = new List<DayMitarbeiter>(base.GetAll().OrderByDescending(item => item.IstFuehrer).ThenBy(item => item.Mitarbeiter.GetFullName()));
            return sorted;
        }

        public DayMitarbeiter GetDayMitarbeiterFromMitarbeiter(Mitarbeiter mitarbeiter)
        {
            foreach (var item in base.GetAll())
            {
                if (item.Mitarbeiter == mitarbeiter)
                {
                    return item;
                }
            }
            return new DayMitarbeiter();
        }

        public bool ContainsMitarbeiter(Mitarbeiter mitarbeiter)
        {
            bool contains = false;
            foreach (var item in base.GetAll())
            {
                if (item.Mitarbeiter == mitarbeiter)
                {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        /// <summary>
        /// Überprüft die übergebene Auflistung von aktuellen Mitarbeitern, ob neue Mitarbeiter (unbekannte) übergeben wurden und fügt sie
        /// der Repository hinzu. Zusätzlich wird überprüft, ob Mitarbeiter in der Auflistung nicht mehr vorhanden sind,
        /// diese werden aus der Repository entfernt. Ergebnis ist, das die Repository die gleichen Mitarbeiter
        /// beinhaltet, wie die übergebene Auflistung.
        /// </summary>
        /// <param name="mitarbeiter">Aktuelle Mitarbeiter</param>
        public virtual void UniformToDayMitarbeiter(IList<Mitarbeiter> mitarbeiter)
        {
            // Überprüfen, ob Items entfernt wurden
            foreach (var currItem in base.GetAll())
            {
                if (!mitarbeiter.Contains(currItem.Mitarbeiter))
                    base.Remove(currItem);
            }
            // Überprüfen, ob Items neu (noch unbekannt) sind
            foreach (var item in mitarbeiter)
            {
                if (!this.ContainsMitarbeiter(item))
                    base.Add(new DayMitarbeiter(item));
            }
        }

        public void RemoveFuehrer()
        {
            DayMitarbeiter fuehrer = null;
            foreach (var mitar in this.GetAll())
            {
                if (mitar.IstFuehrer)
                {
                    fuehrer = mitar;
                    break;
                }
            }
            if (fuehrer != null)
                this.Remove(fuehrer);
        }

        public void Remove(int mitarbeiterId)
        {
            DayMitarbeiter mitarbeiter = null;
            foreach (var mitar in this.GetAll())
            {
                if (mitar.Mitarbeiter != null && mitar.Mitarbeiter.Id == mitarbeiterId)
                {
                    mitarbeiter = mitar;
                    break;
                }
            }
            if (mitarbeiter != null)
                this.Remove(mitarbeiter);
        }


    }
}
