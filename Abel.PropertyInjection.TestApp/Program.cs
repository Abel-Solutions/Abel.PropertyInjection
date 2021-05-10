using System.Threading.Tasks;
using Abel.PropertyInjection.Infrastructure;
using Abel.PropertyInjection.TestServices;
using Abel.PropertyInjection.TestServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Abel.PropertyInjection.TestApp
{
    public static class Program
    {
        public static async Task Main()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .UsePropertyInjection();
            await builder.RunConsoleAsync();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddHostedService<HelloWorldPublicSetter>()
                .AddTransient<IConsole, CustomConsole>();
    }
}
