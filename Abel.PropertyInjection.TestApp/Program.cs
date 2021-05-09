using Abel.PropertyInjection.Infrastructure;
using Abel.PropertyInjection.TestServices;
using Abel.PropertyInjection.TestServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Abel.PropertyInjection.TestApp
{
    public static class Program
    {
        public static void Main() =>
            new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .UsePropertyInjection()
                .Build().Run();

        private static void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddHostedService<HelloWorldPublicSetter>()
                .AddTransient<IConsole, CustomConsole>();
    }
}
