using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Echelon.Core.Extensions.Autofac;
using Rhino.ServiceBus.Actions;
using Rhino.ServiceBus.Hosting;
using Rhino.ServiceBus.Internal;

namespace Echelon.TaskRunner
{
    public class TaskRunnerBusBootstrapper : AbstractBootStrapper
    {
        protected IContainer Container { get; private set; }

        public override T GetInstance<T>()
        {
            return Container.Resolve<T>();
        }

        public override void CreateContainer()
        {
            var builder = new ContainerBuilder();
            var assemblies = Assemblies;

            foreach (var assembly in assemblies)
            {
                builder.RegisterAssemblyTypes(assembly).AssignableTo<IDeploymentAction>().SingleInstance();
                builder.RegisterAssemblyTypes(assembly).AssignableTo<IEnvironmentValidationAction>().SingleInstance();
                builder.RegisterCustomModules(assembly);
                RegisterMessageConsumers(assembly, builder);
            }

            Container = builder.Build();
        }

        protected virtual void RegisterMessageConsumers(Assembly assembly, ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(assembly)
                .Where(type => typeof(IMessageConsumer)
                .IsAssignableFrom(type) &&!typeof(IOccasionalMessageConsumer)
                .IsAssignableFrom(type) && IsTypeAcceptableForThisBootStrapper(type))
                .OnRegistered(e => ConfigureConsumer(e.ComponentRegistration))
                .InstancePerDependency();
        }

        public override void ExecuteDeploymentActions(string user)
        {
            foreach (var action in Container.Resolve<IEnumerable<IDeploymentAction>>())
            {
                action.Execute(user);
            }
        }

        public override void ExecuteEnvironmentValidationActions()
        {
            foreach (var action in Container.Resolve<IEnumerable<IEnvironmentValidationAction>>())
            {
                action.Execute();
            }
        }

        protected virtual void ConfigureConsumer(IComponentRegistration registration)
        {
            //Do nothing
        }

        public override void Dispose()
        {
            Container.Dispose();
        }
    }
}