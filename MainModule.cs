using Autofac;
using Stratus.Extensions;
using Stratus.Services;

namespace Stratus
{
    class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(MainModule).Assembly;
            
            //register site handler extensions
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Namespace == "Stratus.Extensions.SiteExtensions" && t.BaseType == typeof(BaseSiteExtension))
                .As<BaseSiteExtension>().SingleInstance();

            //register view models
            builder.RegisterAssemblyTypes(assembly).Where(t => t.Namespace == "Stratus.ViewModels").AsSelf();

            //register settings service
            builder.RegisterType<SettingsService>().SingleInstance().AsSelf();
        }
    }
}
