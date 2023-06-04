using System;
using System.Collections.Generic;
using System.ComponentModel;
using Montageplan.Model;

namespace Montageplan.ViewModel
{
    public class DayMitarbeiterViewModel : NotificationModel
    {
        private readonly DayMitarbeiter model;

        public DayMitarbeiterViewModel(DayMitarbeiter model, DateTime date)
        {
            this.model = model;
            this.Date = date;
        }

        public DateTime Date { get; set; }

        public MitarbeiterViewModel Mitarbeiter
        {
            get { return new MitarbeiterViewModel(this.model.Mitarbeiter); }
        }

        public string StatusColor
        {
            get
            {
                string color = "Black";
                switch (this.model.Status)
                {
                    case MitarbeiterDayStatus.Available:
                        color = "Green";
                        break;
                    case MitarbeiterDayStatus.PartialBusy:
                        color = "Orange";
                        break;
                    case MitarbeiterDayStatus.Busy:
                        color = "Red";
                        break;
                    case MitarbeiterDayStatus.Sick:
                        color = "Gray";
                        break;
                    case MitarbeiterDayStatus.School:
                        color = "Gray";
                        break;
                    case MitarbeiterDayStatus.Holiday:
                        color = "Gray";
                        break;
                }
                return color;
            }
        }

        public string StatusTooltip
        {
            get
            {
                string status = "Black";
                switch (this.model.Status)
                {
                    case MitarbeiterDayStatus.Available:
                        status = "Mitarbeiter noch nicht zugeteilt";
                        break;
                    case MitarbeiterDayStatus.PartialBusy:
                        status = "Mitarbeiter teilweise zugeteilt";
                        break;
                    case MitarbeiterDayStatus.Busy:
                        status = "Mitarbeiter bereits zugeteilt";
                        break;
                    case MitarbeiterDayStatus.Sick:
                        status = "Krankheit";
                        break;
                    case MitarbeiterDayStatus.School:
                        status = "Schule";
                        break;
                    case MitarbeiterDayStatus.Holiday:
                        status = "Urlaub";
                        break;
                }
                return status;
            }
        }

        public bool IstFuehrer
        {
            get { return this.model.IstFuehrer; }
        }

        public string Funktion
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Mitarbeiter.Funktion) && this.Mitarbeiter.Funktion != "-")
                {
                    return this.Mitarbeiter.Funktion;
                }
                return null;
            }
        }

        public static List<DayMitarbeiterViewModel> CreateViewModels(IEnumerable<DayMitarbeiter> links, DateTime date)
        {
            List<DayMitarbeiterViewModel> viewModels = new List<DayMitarbeiterViewModel>();
            foreach (var link in links)
                viewModels.Add(new DayMitarbeiterViewModel(link, date));

            return viewModels;
        }

        public DayMitarbeiter GetModel()
        {
            return this.model;
        }

        public void NotifyStatusColor()
        {
            base.NotifyPropertyChanged("StatusColor");
        }
    }
}