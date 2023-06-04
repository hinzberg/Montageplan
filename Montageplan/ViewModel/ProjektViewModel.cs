using System.Windows.Media;
using Montageplan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.ViewModel
{
    public class ProjektViewModel : NotificationModel
    {
        public static List<ProjektViewModel> CreateViewModels(IEnumerable<Projekt> projects)
        {
            List<ProjektViewModel> viewModels = new List<ProjektViewModel>();
            foreach (var proj in projects)
                viewModels.Add(new ProjektViewModel(proj));

            return viewModels;
        }


        private readonly Projekt model;

        public ProjektViewModel(Projekt model)
        {
            this.model = model;
            NotificationCenter.GetInstance().Add(
                new NotificationCenter.ObjectNotification((int)Notifications.RefreshDraggableProjectVM, model, this.NotifyStatus));
        }

        ~ProjektViewModel()
        {
            NotificationCenter.GetInstance().Remove((int)Notifications.RefreshDraggableProjectVM, model);
        }

        public DateTime? DateToSort
        {
            get { return this.model.Startdatum; }
        }

        public string Bezeichnung
        {
            get { return this.model.Name; }
        }

        public string Startdatum
        {
            get
            {
                string startdatum;
                if (this.model.IsAllDay())
                    startdatum = PropertyFormatter.FormatDate(this.model.Startdatum);
                else
                    startdatum = PropertyFormatter.FormatDateTime(this.model.Startdatum);
                return startdatum;
            }
        }

        public string Endedatum
        {
            get
            {
                string endeatum;
                if (this.model.IsAllDay())
                    endeatum = PropertyFormatter.FormatDate(this.model.Endedatum);
                else
                    endeatum = PropertyFormatter.FormatDateTime(this.model.Endedatum);
                return endeatum;
            }
        }

        public Projekt GetModel()
        {
            return this.model;
        }

        public string Completed
        {
            get
            {
                if (this.model.IsCompleted)
                    return "Abgeschlossen";
                return "Laufend";
            }
        }

        public string DateSpan
        {
            get
            {
                if (!this.model.Startdatum.HasValue && !this.model.Endedatum.HasValue)
                    return "(Keine Zeitangabe)";

                string start = "";
                string ende = "";

                if (this.model.Startdatum.HasValue)
                    start = PropertyFormatter.FormatDate(this.model.Startdatum);

                if (this.model.Endedatum.HasValue)
                    ende = PropertyFormatter.FormatDate(this.model.Endedatum);

                return start + " bis " + ende;
            }
        }

        public string TimeSpan
        {
            get
            {
                string span = "";
                if (this.model.HasTime())
                {
                    span = string.Format("{0} bis {1} Uhr",
                        PropertyFormatter.FormatTime(this.model.GetStartTime().Value),
                        PropertyFormatter.FormatTime(this.model.GetEndTime().Value));
                }
                return span;
            }
        }

        public ProjektStatus Status
        {
            get { return this.model.GetStatus(); }
        }

        public SolidColorBrush ProjectColorBrush
        {
            get { return this.model.ProjectColorBrush; }
        }

        public void NotifyStatus()
        {
            base.NotifyPropertyChanged("Status");
        }

    }
}
