using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using System.Web.Optimization;
using System.Data.Entity;
using ChoixResto.Models;
using ChoixResto.App_Start;

namespace ChoixResto
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Rajout Bundle
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Rajout pour initialiser la Base de données
            IDatabaseInitializer<BddContext> init = new InitChoixResto();
            Database.SetInitializer(init);
            init.InitializeDatabase(new BddContext());
            // Rajout Filtre
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
