﻿using Autofac;
using Stratus.Extensions;

namespace Stratus
{
    class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(MainModule).Assembly;
            
            //register site handler extensions
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Namespace == "Stratus.Extensions.SiteHandlers" && t.BaseType == typeof(BaseSiteHandler))
                .As<BaseSiteHandler>();

            //register view models
            builder.RegisterAssemblyTypes(assembly).Where(t => t.Namespace == "Stratus.ViewModels").AsSelf();
        }
    }
}
