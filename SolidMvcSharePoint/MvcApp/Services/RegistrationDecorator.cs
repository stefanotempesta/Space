using SolidMvcSharePoint.Interfaces;
using SolidMvcSharePoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Services
{
    public class RegistrationDecorator
    {
        protected IIdentity identity;
        protected IRegistrationService registrationService;
        protected INotificationService notificationService;

        public RegistrationDecorator(IIdentity identity, IRegistrationService registrationService, INotificationService notificationService)
        {
            this.identity = identity;
            this.registrationService = registrationService;
            this.notificationService = notificationService;
        }

        public bool Enrol(Registration registration)
        {
            // Security check: Only Instructors can enrol students
            if (!this.identity.IsInRole("Instructor"))
            {
                return false;
            }

            bool enrolled = this.registrationService.Enrol(registration);

            // Notify student
            if (enrolled)
            {
                this.notificationService.Notify(registration);
            }

            return enrolled;
        }
    }
}