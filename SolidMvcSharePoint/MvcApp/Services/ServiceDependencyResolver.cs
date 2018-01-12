using SolidMvcSharePoint.Interfaces;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Services
{
    internal class ServiceDependencyResolver : NinjectModule
    {
        public override void Load()
        {
            // Services
            Bind<IRegistrationService>().To<InstructorLedRegistrationService>();
            Bind<INotificationService>().To<NotificationService>();
            Bind<IWaitlistService>().To<WaitlistService>();

            // SharePoint Context
            Bind<IRepositoryContext>().To<SharePointContext>()
                .WithConstructorArgument(SharePointContext.SharePointSiteUrl);
        }
    }
}