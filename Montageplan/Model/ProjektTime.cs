using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class ProjektTime
    {
        public ProjektTime()
            : this(0, 0)
        {
        }
        public ProjektTime(int hours, int minutes)
        {
            this.Hours = hours;
            this.Minutes = minutes;     
        }

        public int Hours { get; set; }
        public int Minutes { get; set; }

        public bool HasTime()
        {
            bool hasTime = (this.Hours > 0 || this.Minutes > 0);
            return hasTime;
        }

    }
}
