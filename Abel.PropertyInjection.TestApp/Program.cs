using Abel.PropertyInjection.TestApp.Services;
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
                .AddTransient<IHelloWorld, HelloWorld>()
                .AddTransient<IConsole, CustomConsole>();
                //.AddPropertyInjection();
    }
}
