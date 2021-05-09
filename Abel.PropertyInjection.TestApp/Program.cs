using Abel.PropertyInjection.Extensions;
using Abel.PropertyInjection.TestServices;
using Abel.PropertyInjection.TestServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Abel.PropertyInjection.TestApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider()
                .WithPropertyInjections();

            var helloWorld = serviceProvider.GetService<IHelloWorld>();
            helloWorld.Hello();
        }

        private static IServiceCollection ConfigureServices() =>
            new ServiceCollection()
                .AddTransient<IHelloWorld, HelloWorldPublicSetter>()
                .AddTransient<IConsole, CustomConsole>();
                //.AddPropertyInjection();
    }
}
