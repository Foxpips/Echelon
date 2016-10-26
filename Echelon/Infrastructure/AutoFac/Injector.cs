using System;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Module = Autofac.Module;

namespace Echelon.Infrastructure.AutoFac
{
    public static class Injector
    {
        public static IContainer RegisterModulesOnInit(ContainerBuilder builder = null)
        {
            var containerBuilder = builder ?? new ContainerBuilder();
            var modules = typeof(Injector).GetTypeInfo()
                .Assembly.ExportedTypes.Where(type => type.IsAssignableTo<Module>());

            foreach (var module in modules)
            {
                var instance = (Module) Activator.CreateInstance(module);
                containerBuilder.RegisterModule(instance);
            }

            return containerBuilder.Build();
        }

        public static void RegisterProfilesOnInit()
        {
            var profiles = typeof(Injector).GetTypeInfo()
                .Assembly.ExportedTypes.Where(type => type.IsAssignableTo<Profile>());

            Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
        }
    }
}