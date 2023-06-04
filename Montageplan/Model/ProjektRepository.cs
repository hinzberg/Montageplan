using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class ProjektRepository : Repository<Projekt>
    {
        public List<Projekt> GetTemplateUncompletedProjects()
        {
            List<Projekt> filtered = new List<Projekt>();
            foreach (var projItem in base.GetAll())
            {
                if (projItem.IsTemplate && !projItem.IsCompleted)
                    filtered.Add(projItem);
            }
            return filtered;
        }

        public List<Projekt> GetTemplateProjects()
        {
            List<Projekt> filtered = new List<Projekt>();
            foreach (var projItem in base.GetAll())
            {
                if (projItem.IsTemplate)
                    filtered.Add(projItem);
            }
            return filtered;
        }

        //public List<Projekt> GetUncompletedProjects()
        //{
        //    List<Projekt> projects = new List<Projekt>();
        //    foreach (var proj in base.GetAll())
        //        if (!proj.IsCompleted)
        //            projects.Add(proj);
        //    return projects;
        //}

        public override bool Contains(Projekt item)
        {
            bool contains = false;
            foreach (var itemInList in base.GetAll())
            {
                if (item.Id == item.Id)
                {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        public Projekt GetProjekt(int id)
        {
            Projekt projekt = null;
            foreach (var item in base.GetAll())
            {
                if (item.Id == id)
                {
                    projekt = item;
                    break;
                }
            }
            return projekt;
        }

    }
}
