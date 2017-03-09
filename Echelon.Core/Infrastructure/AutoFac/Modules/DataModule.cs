using Autofac;
using Echelon.Data;
using Echelon.Data.RavenDb;

namespace Echelon.Core.Infrastructure.AutoFac.Modules
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RavenDataService>().As<IDataService>().SingleInstance();
        }
    }
}