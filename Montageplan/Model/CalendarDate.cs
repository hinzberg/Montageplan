using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class CalendarDate
    {
        public CalendarDate()
            : this(DateTime.MinValue)
        {
        }
        public CalendarDate(DateTime day)
        {
            this.Id = Id;
            this.Day = day;
            this.TimeFrom = null;
            this.TimeTo = null;
        }

        public int Id { get; set; }
        public DateTime Day { get; set; }
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }


    }
}
