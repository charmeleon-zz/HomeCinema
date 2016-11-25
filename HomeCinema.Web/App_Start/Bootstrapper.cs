using HomeCinema.Web.Mappings;
using System.Web.Http;

namespace HomeCinema.Web.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            // Configure autofac
            AutofacWebApiConfig.Initialize(GlobalConfiguration.Configuration);
            // Configure automapper
            AutoMapperConfiguration.Configure();
        }
    }
}