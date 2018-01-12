using SolidMvcSharePoint.Interfaces;
using SolidMvcSharePoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Services
{
    public class InstructorLedRegistrationService : IRegistrationService
    {
        public IList<Registration> Registrations { get; }

        public bool Enrol(Registration registration)
        {
            var course = registration.Course as InstructorLedCourse;
            if (course == null)
            {
                return false;
            }

            if (Registrations.Count(r => r.Course.Id == registration.Course.Id) >= course.Capacity)
            {
                Waitlist(registration);
                return false;
            }

            if (Registrations.SingleOrDefault(r => r.Course.Id == registration.Course.Id && r.Student.Id == registration.Student.Id) != default(Registration))
            {
                return false;
            }

            registration.Status = RegistrationStatus.Confirmed;
            Registrations.Add(registration);
            return true;
        }

        protected void Waitlist(Registration registration)
        {
            registration.Status = RegistrationStatus.Waitlisted;

            // Without DI
            WaitlistService waitlist = new WaitlistService();
            waitlist.Add(registration);

            // With property-setter DI
            InjectedWaitlist.Add(registration);
        }

        public IWaitlistService InjectedWaitlist { get; set; }
    }
}