using Autofac;
using Echelon.Core.Logging.Loggers;

namespace Echelon.Core.Infrastructure.AutoFac.Modules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClientLogger>().As<IClientLogger>().SingleInstance();
        }
    }
}