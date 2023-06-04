using System.Windows;
using Montageplan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.ViewModel
{
    public class MitarbeiterViewModel : NotificationModel
    {
        public static List<MitarbeiterViewModel> CreateViewModels(IEnumerable<Mitarbeiter> mitarbeiter)
        {
            List<MitarbeiterViewModel> viewModels = new List<MitarbeiterViewModel>();
            foreach (var mitar in mitarbeiter)
                viewModels.Add(new MitarbeiterViewModel(mitar));

            return viewModels;
        }


        private readonly Mitarbeiter model;

        public MitarbeiterViewModel(Mitarbeiter model)
        {
            this.model = model;
        }

        public string NameToSort
        {
            get { return this.model.GetFullName(); }
        }

        public string Vorname
        {
            get { return this.model.Vorname; }
        }

        public string Name
        {
            get { return this.model.Name; }
        }

        public string Bezeichnung
        {
            get { return this.model.Bezeichnung; }
        }

        public string FullName
        {
            get { return this.GetConfiguredName(); }
        }

        private string GetConfiguredName()
        {
            string name = "Kein Name";

            // Name, Vornamen anzeigen
            if (AppConfig.Settings.MitarbeiterDisplayWithVorname
                && AppConfig.Settings.MitarbeiterDisplayWithName)
            {
                name = "";
                if (!string.IsNullOrEmpty(this.model.Name))
                    name = this.model.Name;

                if (!string.IsNullOrEmpty(this.model.Vorname)
                    && !string.IsNullOrEmpty(this.model.Name))
                    name += ", ";

                if (!string.IsNullOrEmpty(this.model.Vorname))
                    name += this.model.Vorname;
            }
            // Nur Name anzeigen
            else if (AppConfig.Settings.MitarbeiterDisplayWithName)
            {
                name = "";
                if (!string.IsNullOrEmpty(this.model.Name))
                    name = this.model.Name;
            }
            // Nur Vorname
            else if (AppConfig.Settings.MitarbeiterDisplayWithVorname)
            {
                name = "";
                if (!string.IsNullOrEmpty(this.model.Vorname))
                    name = this.model.Vorname;
            }

            // Und Bezeichnung, wenn vorhanden.
            if (AppConfig.Settings.MitarbeiterDisplayWithBezeichnung
                && !string.IsNullOrEmpty(this.model.Bezeichnung))
            {
                name += " (" + this.model.Bezeichnung + ")";
            }

            return name;
        }

        public string Funktion
        {
            get { return (this.model.Funktion == null) ? "-" : PropertyFormatter.FormatString(this.model.Funktion.Name); }
        }

        public string KannFuehrerSein
        {
            get { return PropertyFormatter.FormatBool(this.model.KannFuehrerSein); }
        }

        public Visibility KannFuehrerSeinVisibility
        {
            get
            {
                if (this.model.KannFuehrerSein)
                    return Visibility.Visible;

                return Visibility.Hidden;
            }
        }

        public Mitarbeiter GetModel()
        {
            return this.model;
        }

    }
}
