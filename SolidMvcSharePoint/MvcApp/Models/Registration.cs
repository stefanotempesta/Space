using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Models
{
    public class Registration
    {
        public Student Student { get; set; }

        public CourseTemplate Course { get; set; }

        public DateTime RegistrationDate { get; set;  }

        public RegistrationStatus Status { get; set; }
    }

    public enum RegistrationStatus
    {
        Confirmed,
        Waitlisted,
        Canceled
    }
}