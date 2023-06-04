using System;
using System.Collections.Generic;

namespace Montageplan.Model
{
    public class Mitarbeiter
    {
        public const int MAX_LENGHT_NAME = 50;
        public const int MAX_LENGHT_VORNAME = 50;
        public const int MAX_LENGHT_BEZEICHNUNG = 50;


        public Mitarbeiter()
            : this("", "")
        {
        }

        public Mitarbeiter(string vorname, string name)
            : this(vorname, name, false)
        {
        }

        public Mitarbeiter(string vorname, string name, bool kannFuehrerSein)
        {
            this.Id = 0;
            this.IsActive = true;
            this.Vorname = vorname;
            this.Name = name;
            this.KannFuehrerSein = kannFuehrerSein;
            this.Funktion = null;
            this.Bezeichnung = "";
            this.Fehlzeiten = new List<MitarbeiterFehlzeit>();
        }

        /// <summary>
        /// Datenbank Id (PK)
        /// </summary>
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Vorname { get; set; }
        public string Name { get; set; }
        public string Bezeichnung { get; set; }
        public bool KannFuehrerSein { get; set; }
        public MitarbeiterFunktion Funktion { get; set; }
        public List<MitarbeiterFehlzeit> Fehlzeiten { get; set; }

        public string GetFullName()
        {
            string name = "Kein Name";

            if (this.Vorname != "" && this.Name != "")
                name = this.Name + ", " + this.Vorname;
            else if (this.Vorname != "" && this.Name == "")
                name = this.Vorname;
            else if (this.Vorname == "" && this.Name != "")
                name = this.Name;

            return name;
        }

        public override string ToString()
        {
            return string.Format("{0}", this.GetFullName());
        }



    }
}
