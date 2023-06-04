using Montageplan.Model.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class EngagementProjekt
    {
        public static EngagementProjekt CreateFromEntity(EngagementProjektEntity engProjektEntity, Projekt projekt)
        {
            EngagementProjekt engProjekt = new EngagementProjekt();
            engProjekt.Id = engProjektEntity.Id;
            engProjekt.StartTime = engProjektEntity.StartTime;
            engProjekt.EndTime = engProjektEntity.EndTime;
            engProjekt.Projekt = projekt;
            return engProjekt;
        }
        public static EngagementProjekt CreateFromProjekt(DateTime day, Projekt projekt)
        {
            EngagementProjekt engProjekt;

            DateTime? start = null;
            DateTime? end = null;
            if (!projekt.IsAllDay())
            {
                DateTime ps = projekt.Startdatum.Value;
                DateTime pe = projekt.Endedatum.Value;
                start = new DateTime(day.Year, day.Month, day.Day, ps.Hour, ps.Minute, ps.Second);
                end = new DateTime(day.Year, day.Month, day.Day, pe.Hour, pe.Minute, pe.Second);
            }

            engProjekt = new EngagementProjekt();
            engProjekt.Projekt = projekt;
            engProjekt.StartTime = start;
            engProjekt.EndTime = end;
            return engProjekt;
        }
        public static EngagementProjekt CreateFromEngagementProjekt(DateTime day, EngagementProjekt projekt)
        {
            EngagementProjekt engProjekt;

            DateTime? start = null;
            DateTime? end = null;
            if (!projekt.IsAllDay())
            {
                DateTime ps = projekt.StartTime.Value;
                DateTime pe = projekt.EndTime.Value;
                start = new DateTime(day.Year, day.Month, day.Day, ps.Hour, ps.Minute, ps.Second);
                end = new DateTime(day.Year, day.Month, day.Day, pe.Hour, pe.Minute, pe.Second);
            }

            engProjekt = new EngagementProjekt();
            engProjekt.Projekt = projekt.Projekt;
            engProjekt.StartTime = start;
            engProjekt.EndTime = end;
            return engProjekt;
        }


        public EngagementProjekt()
        {
            this.Id = 0;
            this.StartTime = null;
            this.EndTime = null;
            this.Projekt = null;
        }

        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Projekt Projekt { get; set; }

        public bool IsAllDay()
        {
            bool isAllDay = (!this.StartTime.HasValue && !this.EndTime.HasValue);
            return isAllDay;
        }

        public bool IntersectTime(DateTime start, DateTime end)
        {
            bool intersect = false;
            if (this.IsAllDay())
                intersect = true;
            else
            {
                DateTime fStart = this.GetOnlyTime(start);
                DateTime fEnd = this.GetOnlyTime(end);
                DateTime fStartLocal = this.GetOnlyTime(this.StartTime.Value);
                DateTime fEndLocal = this.GetOnlyTime(this.EndTime.Value);

                bool notIntersect =
                    (fStart < fStartLocal && fEnd <= fStartLocal) ||
                    (fStart >= fEndLocal && fEnd > fEndLocal);

                intersect = !notIntersect;
            }
            return intersect;
        }

        private DateTime GetOnlyTime(DateTime date)
        {
            DateTime dt;
            DateTime min = DateTime.MinValue;
            dt = new DateTime(min.Year, min.Month, min.Day, date.Hour, date.Minute, date.Second);
            return dt;
        }

    }
}
