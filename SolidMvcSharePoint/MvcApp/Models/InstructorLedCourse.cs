using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidMvcSharePoint.Models
{
    public class InstructorLedCourse : CourseTemplate
    {
        public DateTime StartOn { get; set; }

        public DateTime EndOn { get; set; }

        public int Duration { get; set; }

        public Location Venue { get; set; }

        public int Capacity { get; set; }
    }
}
