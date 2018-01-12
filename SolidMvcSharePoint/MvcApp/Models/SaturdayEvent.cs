using SolidMvcSharePoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaturdayInc.Models
{
    public class SaturdayEvent
    {
        public Location City { get; set; }

        public DateTime EventDate { get; set; }

        public IList<Sponsor> Sponsors { get; set; }

        public bool Register(Student attendee)
        {
            if (Attendees.Contains(attendee))
            {
                return false;
            }

            Attendees.Add(attendee);
            return true;
        }

        protected IList<Student> Attendees { get; set; }
    }
}