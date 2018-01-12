using SolidMvcSharePoint.Interfaces;
using SolidMvcSharePoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Services
{
    public class OnlineCourseRegistrationService : IRegistrationService
    {
        public IList<Registration> Registrations { get; }

        public bool Enrol(Registration registration)
        {
            if (!(registration.Course is OnlineCourse))
            {
                return false;
            }

            if (Registrations.SingleOrDefault(r => r.Course.Id == registration.Course.Id && r.Student.Id == registration.Student.Id) != default(Registration))
            {
                return false;
            }

            Registrations.Add(registration);
            return true;
        }
    }
}