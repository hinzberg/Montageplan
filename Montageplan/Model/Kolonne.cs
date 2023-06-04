using System.Windows.Media;

namespace Montageplan.Model
{
    public class Kolonne
    {
        public Kolonne()
            : this("", "")
        {
        }
        public Kolonne(string nummer, string bezeichnung)
            : this(nummer, bezeichnung, null)
        {
        }
        public Kolonne(string nummer, string bezeichnung, Mitarbeiter fuehrer)
        {
            this.Id = 0;
            this.IsActive = true;
            this.Nummer = nummer;
            this.Bezeichnung = bezeichnung;
            this.Fuehrer = fuehrer;
            this.IsExpanded = true;
            this.AdditionalMitarbeiter = new MitarbeiterRepository();
            Color c = Color.FromArgb(60, Colors.Blue.R, Colors.Blue.G, Colors.Blue.B);
            this.KolonneColorBrush = new SolidColorBrush(c);
        }

        /// <summary>
        /// Datenbank-PK
        /// </summary>
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Nummer { get; set; }
        public string Bezeichnung { get; set; }
        public Mitarbeiter Fuehrer { get; set; }
        public SolidColorBrush KolonneColorBrush { get; set; }

        /// <summary>
        /// Wird die Kolonne auf dem Kalender vollständig angezeigt oder ist sie zusammengeklappt ?
        /// </summary>
        public bool IsExpanded { get; set; }
        /// <summary>
        /// Alle zusätzlichen Mitarbeiter der Kolonne. Der Führer wird hier nicht aufgelistet.
        /// </summary>
        public MitarbeiterRepository AdditionalMitarbeiter { get; set; }

        public override string ToString()
        {
            return string.Format("{0}. {1}", this.Nummer, this.Bezeichnung);
        }

    }
}
