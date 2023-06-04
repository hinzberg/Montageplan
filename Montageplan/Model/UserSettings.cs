using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Montageplan.Model
{
    public enum DaysAlignment
    {
        FixedWidth,
        DynamicWidth
    };

    /// <summary>
    /// Benutzereinstellungen
    /// </summary>
    public class UserSettings
    {
        public DaysAlignment KalenderTageAusrichtung { get; set; }
        public DateTime StartDatumDruck { get; set; }
        public DateTime EndeDatumDruck { get; set; }
        public PrintSettings Print { get; set; }
        public bool MitarbeiterDisplayWithBezeichnung { get; set; }
        public bool MitarbeiterDisplayWithName { get; set; }
        public bool MitarbeiterDisplayWithVorname { get; set; }
        public bool ShowKolonnePrefix { get; set; }
        public SolidColorBrush DayBrush { get; set; } // Farbe für Tage-Datum

        public UserSettings()
        {
            this.Print = new PrintSettings();
            this.KalenderTageAusrichtung = DaysAlignment.DynamicWidth;
            this.EndeDatumDruck = DateTime.Today;
            this.StartDatumDruck = DateTime.Today;
            this.MitarbeiterDisplayWithBezeichnung = true;
            this.MitarbeiterDisplayWithName = true;
            this.MitarbeiterDisplayWithVorname = true;
            this.ShowKolonnePrefix = false;
            this.DayBrush = new SolidColorBrush(Colors.Yellow);
        }

    }
}
