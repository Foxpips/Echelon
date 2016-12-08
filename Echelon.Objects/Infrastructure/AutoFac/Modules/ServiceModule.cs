using Autofac;
using Echelon.Objects.Infrastructure.Services.Category;
using Echelon.Objects.Infrastructure.Services.Login;

namespace Echelon.Objects.Infrastructure.AutoFac.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
            builder.RegisterType<CategoryService>().As<ICategoryService>().SingleInstance();
        }
    }
}