using Autofac;
using Echelon.Core.Interfaces.Data;
using Echelon.Data.RavenDb;

namespace Echelon.Objects.Infrastructure.AutoFac.Modules
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataService>().As<IDataService>().SingleInstance();
        }
    }
}