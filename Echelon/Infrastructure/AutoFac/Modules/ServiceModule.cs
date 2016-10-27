using Autofac;
using Echelon.Core.Data.RavenDb;
using Echelon.Core.Interfaces.Data;
using Echelon.Infrastructure.Services.Login;

namespace Echelon.Infrastructure.AutoFac.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
        }
    }
}