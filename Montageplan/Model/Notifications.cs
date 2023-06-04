using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public enum Notifications
    {
        /// <summary>
        /// Sendet ein Callback zu der Projekte-Auswahl
        /// </summary>
        RefreshDraggableProjectVM = 1,
        /// <summary>
        /// Aktualisiert die sichtbare Kalenderansicht (7 Tage)
        /// </summary>
        RefreshCompleteCalendar = 2
    }
}
