using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class EngagementProjektRepository : Repository<EngagementProjekt>
    {
        public bool Contains(int projektId)
        {
            bool contains = (this.Get(projektId) != null);
            return contains;
        }

        public List<EngagementProjekt> GetAllTimeSorted()
        {
            List<EngagementProjekt> list = new List<EngagementProjekt>();
            if (base.GetCount() > 0)
                list.AddRange(base.GetAll().OrderBy(item => item.StartTime).ThenBy(item => item.Projekt.Id));
            return list;
        }

        public EngagementProjekt Get(int projektId)
        {
            EngagementProjekt engProjekt = null;
            foreach (var engProjektItem in base.GetAll())
            {
                if (engProjektItem.Projekt.Id == projektId)
                {
                    engProjekt = engProjektItem;
                    break;
                }
            }
            return engProjekt;
        }

    }
}
