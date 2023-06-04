using Montageplan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Montageplan.ViewModel
{
    public class EngagementProjektViewModel : NotificationModel
    {
        public static List<EngagementProjektViewModel> CreateViewModels(IEnumerable<EngagementProjekt> models)
        {
            List<EngagementProjektViewModel> viewModels = new List<EngagementProjektViewModel>();
            foreach (var model in models)
                viewModels.Add(new EngagementProjektViewModel(model));

            return viewModels;
        }


        private readonly EngagementProjekt model;

        public EngagementProjektViewModel(EngagementProjekt model)
        {
            this.model = model;
        }

        public string ProjektName
        {
            get { return this.model.Projekt.Name; }
        }

        public string ProjektColor
        {
            get { return this.model.Projekt.ProjectColorBrush.ToString(); }
        }

        public string ProjektBorderColor
        {
            get 
            {
                string color;
                SolidColorBrush brush = this.model.Projekt.ProjectColorBrush;
                SolidColorBrush newbrush = new SolidColorBrush(Color.FromArgb(220, brush.Color.R, brush.Color.G, brush.Color.B));
                color = newbrush.ToString();
                return color;
            }
        }

        public string TimeInfo
        {
            get
            {
                string timeInfo;
                if (this.model.IsAllDay())
                    timeInfo = "Ganztägig";
                else
                    timeInfo = (string.Format("{0:HH:mm} - {1:HH:mm} Uhr", this.model.StartTime.Value, this.model.EndTime.Value));
                return timeInfo;
            }
        }

        public EngagementProjekt GetModel()
        {
            return this.model;
        }

    }
}
