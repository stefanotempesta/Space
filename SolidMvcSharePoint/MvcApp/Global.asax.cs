using SolidMvcSharePoint.Data;
using SolidMvcSharePoint.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SolidMvcSharePoint
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, DatabaseConfiguration>());

            DIContainer = new ServiceInjectionContainer();
        }

        protected ServiceInjectionContainer DIContainer;
    }
}
