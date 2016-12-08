using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Echelon.Core.Extensions.Autofac
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterCustomModules(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            var containerBuilder = builder ?? new ContainerBuilder();
            foreach (var assembly in assemblies)
            {
                var modules = assembly.ExportedTypes.Where(type => type.IsAssignableTo<Module>());

                foreach (var module in modules)
                {
                    var instance = (Module) Activator.CreateInstance(module);
                    containerBuilder.RegisterModule(instance);
                }
            }
            return containerBuilder;
        }
    }
}