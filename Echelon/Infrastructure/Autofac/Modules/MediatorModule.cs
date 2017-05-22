using System.Linq;
using Autofac;
using Echelon.Mediators;

namespace Echelon.Infrastructure.Autofac.Modules
{
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            foreach (var result in GetType().Assembly.ExportedTypes.Where(type => type.IsAssignableTo<IMediator>()))
            {
                builder.RegisterType(result);
            }
        }
    }
}