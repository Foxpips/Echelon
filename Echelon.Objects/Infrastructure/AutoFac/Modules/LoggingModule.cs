using Autofac;
using Echelon.Core.Logging.Interfaces;
using Echelon.Core.Logging.Loggers;

namespace Echelon.Objects.Infrastructure.AutoFac.Modules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClientLogger>().As<IClientLogger>().SingleInstance();
        }
    }
}