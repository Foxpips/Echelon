using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Echelon.Core.Extensions.Autofac
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterCustomModules(this ContainerBuilder builder, bool includeCoreModules, params Assembly[] assemblies)
        {
            var containerBuilder = builder ?? new ContainerBuilder();

            if (includeCoreModules)
            {
                RegisterModule(typeof(AutofacExtensions).Assembly, containerBuilder);
            }

            foreach (var assembly in assemblies)
            {
                RegisterModule(assembly, containerBuilder);
            }
            return containerBuilder;
        }

        private static void RegisterModule(Assembly assembly, ContainerBuilder containerBuilder)
        {
            var modules = assembly.ExportedTypes.Where(type => type.IsAssignableTo<Module>());

            foreach (var module in modules)
            {
                var instance = (Module)Activator.CreateInstance(module);
                containerBuilder.RegisterModule(instance);
            }
        }
    }
}