using Autofac;
using bookworld.core.Interfaces.Services;
using bookworld.Infrastructure.Auth;

namespace bookworld.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance();
        }
    }
}
