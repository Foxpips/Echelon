using Autofac;
using Echelon.Core.Data.RavenDb;
using Echelon.Core.Interfaces.Data;

namespace Echelon.Infrastructure.AutoFac.Modules
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataService>().As<IDataService>().SingleInstance();
        }
    }
}