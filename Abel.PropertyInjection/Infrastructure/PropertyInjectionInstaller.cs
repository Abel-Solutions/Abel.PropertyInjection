using Abel.PropertyInjection.Interfaces;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Abel.PropertyInjection.Infrastructure
{
    public static class PropertyInjectionInstaller
    {
        public static IHostBuilder UsePropertyInjection(this IHostBuilder hostBuilder) =>
            hostBuilder
                .UseServiceProviderFactory(new PropertyInjectionServiceProviderFactory())
                .ConfigureServices(ConfigureServices);

        private static void ConfigureServices(IServiceCollection services) =>
            services
                .AddSingleton<IPropertyInjector, PropertyInjector>()
                .AddSingleton<IControllerFactory, PropertyInjectionControllerFactory>();
    }
}
