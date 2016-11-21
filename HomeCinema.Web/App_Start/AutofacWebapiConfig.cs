using Autofac;
using Autofac.Integration.WebApi;
using HomeCinema.Data;
using HomeCinema.Data.Infrastructure;
using HomeCinema.Data.Repositories;
using HomeCinema.Services;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;

namespace HomeCinema.Web.App_Start
{
    public class AutofacWebApiConfig
    {
        public static IContainer Container;
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }
        private static IContainer RegisterServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            containerBuilder.RegisterType<HomeCinemaContext>()
                .As<DbContext>()
                .InstancePerRequest();
            containerBuilder.RegisterType<DbFactory>()
                .As<IDbFactory>()
                .InstancePerRequest();
            containerBuilder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();
            containerBuilder.RegisterGeneric(typeof(EntityBaseRepository<>))
                .As(typeof(IEntityBaseRepository<>))
                .InstancePerRequest();
            // Services
            containerBuilder.RegisterType<EncryptionService>()
                .As<IEncyptionService>()
                .InstancePerRequest();
            Container = containerBuilder.Build();

            return Container;
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}