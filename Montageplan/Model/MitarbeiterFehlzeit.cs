using System;

namespace Montageplan.Model
{
    public class MitarbeiterFehlzeit
    {
        public MitarbeiterFehlzeit()
            : this("Sonstiges")
        {
        }

        public MitarbeiterFehlzeit(string bezeichnung)
        {
            this.Id = 0;
            this.MitarbeiterId = 0;
            this.Bezeichnung = bezeichnung;
            this.StartDatum = DateTime.MinValue;
            this.EndeDatum = DateTime.MinValue;
        }

        public int Id { get; set; }
        public int MitarbeiterId { get; set; }
        public string Bezeichnung { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EndeDatum { get; set; }
    }
}