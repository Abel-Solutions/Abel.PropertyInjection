using System;
using HarmonyLib;
using Microsoft.Extensions.DependencyInjection;

namespace Abel.PropertyInjection.Infrastructure
{
    public static class PropertyInjectionInstaller
    {
        private static IServiceProvider _serviceProvider;

        public static IServiceCollection AddPropertyInjection(this IServiceCollection services)
        {
            _serviceProvider = services.BuildServiceProvider();
            PatchServiceProvider();
            return services;
        }

        private static void PatchServiceProvider()
        {
            var harmony = new Harmony("yeeee");

            //var serviceProviderType = _serviceProvider.GetService<IServiceProvider>().GetType();
            var serviceProviderType = typeof(ServiceProvider);

            var originalMethod = serviceProviderType.GetMethod(nameof(ServiceProvider.GetService));

            //var originalMethod = typeof(Console).GetMethod(nameof(Console.WriteLine), new []{ typeof(string)});

            var newMethod = typeof(PropertyInjectionInstaller).GetMethod(nameof(Postfix));

            var postfix = new HarmonyMethod(newMethod);

            harmony.Patch(originalMethod, postfix: postfix);
        }

        public static void Postfix(ref object __result) // todo private, flags
        {
            Console.WriteLine("in Postfix!");
            //InjectProps(__result);
        }
    }
}
