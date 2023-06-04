using Montageplan.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Montageplan.Model
{
    public class ListBoxDropHelper : TargetDropHelper
    {
        private readonly Engagement engagement;

        public ListBoxDropHelper(UIElement target, Engagement engagement)
            : base(target)
        {
            this.engagement = engagement;
        }

        protected override bool IsDropAllowed(object draggedData)
        {
            bool isAllowed = false;
            if (draggedData is DayMitarbeiterViewModel)
                isAllowed = this.IsMitarbeiterDropAllowed((DayMitarbeiterViewModel)draggedData);
            else
                isAllowed = true;

            return isAllowed;
        }

        private bool IsMitarbeiterDropAllowed(DayMitarbeiterViewModel dayMitViewModel)
        {
            bool isAllowed = false;
            // Ist der Mitarbeiter noch verfügbar ?
            isAllowed = (dayMitViewModel.GetModel().Status == MitarbeiterDayStatus.Available);
            if (isAllowed)
            {
                // Datum überürpfen
                isAllowed = (dayMitViewModel.Date.Date == this.engagement.Date.Date);
                if (isAllowed)
                {
                    // Ist das Projekt schon vorhanden ?
                    isAllowed = (this.engagement.HasProjekte);
                }
            }

            return isAllowed;
        }

    }
}
