using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class CalendarDayRepository : Repository<CalendarDay>
    {
        public CalendarDay Get(DateTime date)
        {
            CalendarDay day = null;
            foreach (var item in base.GetAll())
            {
                if (item.Date.Date == date.Date)
                {
                    day = item;
                    break;
                }
            }
            return day;
        }

        public bool Contains(DateTime date)
        {
            bool contains = false;
            contains = (this.Get(date) != null);
            return contains;
        }

    }
}
