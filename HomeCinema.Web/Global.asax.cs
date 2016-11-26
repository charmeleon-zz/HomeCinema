using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using HomeCinema.Web.App_Start;
using System.Web.Optimization;
using System.Web.Routing;

namespace HomeCinema.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            // Register Web API routes
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            // Initialize application container, etc.
            Bootstrapper.Run();
            GlobalConfiguration.Configuration.EnsureInitialized();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
    }
}