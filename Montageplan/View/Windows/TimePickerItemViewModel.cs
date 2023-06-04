using System;
using System.Collections.Generic;

namespace Montageplan.View.Windows
{
    public class TimePickerItemViewModel
    {
        public static List<TimePickerItemViewModel> CreateList(int startHour, int endHour)
        {
            List<TimePickerItemViewModel> timeVms = new List<TimePickerItemViewModel>();

            timeVms.Add(new TimePickerItemViewModel(null)); // Erster Eintrag - Keine Angabe = null
            for (int i = startHour; i <= endHour; i++)
            {
                DateTime dts = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, i, 0, 0);
                timeVms.Add(new TimePickerItemViewModel(dts));
                DateTime dte = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, i, 30, 0);
                timeVms.Add(new TimePickerItemViewModel(dte));
            }
            return timeVms;
        }


        public TimePickerItemViewModel(DateTime? dt)
        {
            this.Model = dt;
        }

        public DateTime? Model { get; set; }

        public string TimeString
        {
            get
            {
                string timeString = "";
                if (this.Model.HasValue)
                {
                    string h = this.Model.Value.Hour.ToString("N0");
                    string m = this.Model.Value.Minute.ToString("N0");

                    if (h.Length == 1)
                        h = "0" + h;

                    if (m.Length == 1)
                        m = "0" + m;

                    timeString = h + ":" + m;
                }
                return timeString;
            }
        }

        public DateTime? GetModel()
        {
            return this.Model;
        }
    }
}