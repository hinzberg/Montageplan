using System.Collections.Generic;
namespace Montageplan.Model
{
    public class RepositoryContainer
    {
        public RepositoryContainer()
        {
            this.Projekte = new ProjektRepository();
            this.Kolonnen = new KolonneRepository();
            this.Mitarbeiter = new MitarbeiterRepository();
            this.Funktionen = new FunktionRepository();
            this.Days = new CalendarDayRepository();
            this.Users = new UserRepository();
            this.Fehlzeitarten = new Repository<Fehlzeitart>();
            this.Engagements = new EngagementRepository();
        }

        public ProjektRepository Projekte { get; set; }
        public KolonneRepository Kolonnen { get; set; }
        public MitarbeiterRepository Mitarbeiter { get; set; }
        public FunktionRepository Funktionen { get; set; }
        public CalendarDayRepository Days { get; set; }
        public UserRepository Users { get; set; }
        public Repository<Fehlzeitart> Fehlzeitarten { get; set; }
        public EngagementRepository Engagements { get; set; }

        /// <summary>
        /// Ermittelt alle Mitarbeiter, die für die angegebene Kolonne noch zur Verfügung stehen.
        /// Mitarbeiter, die noch keiner Kolonne zugewiesen sind und keine Kolonnenfuehrer, stehen zur Verfügung.
        /// </summary>
        /// <returns></returns>
        public List<Mitarbeiter> GetFreeMitarbeiter(int kolonneId)
        {
            List<Mitarbeiter> freeMitarbeiter = new List<Mitarbeiter>();
            FreeMitarbeiterFilter filter = new FreeMitarbeiterFilter();
            freeMitarbeiter.AddRange(filter.GetFreeMitarbeiter(kolonneId, this.Mitarbeiter.GetAll(), this.Kolonnen.GetAll()));
            return freeMitarbeiter;
        }

    }
}