using Autofac;
using Echelon.Core.Infrastructure.Services.Email.Components;

namespace Echelon.Core.Infrastructure.AutoFac.Modules
{
    public class HelperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailTokenHelper>().As<ITokenHelper>();
        }
    }
}