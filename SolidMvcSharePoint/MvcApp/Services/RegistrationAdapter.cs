using SolidMvcSharePoint.Interfaces;
using SolidMvcSharePoint.Models;
using SaturdayInc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Services
{
    public class RegistrationAdapter : IRegistrationService
    {
        protected IRegistrationService registrationService;

        public RegistrationAdapter(IRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        // Implement IRegistrationService
        public bool Enrol(Registration registration)
        {
            return this.registrationService.Enrol(registration);
        }

        // Adapt the Enrol method to the external SaturdayEvent object
        public bool Enrol(Registration registration, SaturdayEvent saturdayEvent)
        {
            return saturdayEvent.Register(attendee: registration.Student);
        }
    }
}