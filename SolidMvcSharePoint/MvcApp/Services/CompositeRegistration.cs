using SolidMvcSharePoint.Interfaces;
using SolidMvcSharePoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Services
{
    public class CompositeRegistration : IRegistrationService, IWaitlistService
    {
        protected IRegistrationService registrationService;
        protected IWaitlistService waitlistService;

        public CompositeRegistration(IRegistrationService registrationService, IWaitlistService waitlistService)
        {
            this.registrationService = registrationService;
            this.waitlistService = waitlistService;
        }

        // Implement IRegistrationService
        public bool Enrol(Registration registration)
        {
            bool result = this.registrationService.Enrol(registration);

            if (registration.Status == RegistrationStatus.Waitlisted)
            {
                Add(registration);
            }

            return result;
        }
        
        // Implement IWaitlistService
        public void Add(Registration registration)
        {
            this.waitlistService.Add(registration);
        }
    }
}