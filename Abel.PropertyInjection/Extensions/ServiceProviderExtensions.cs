using System;

namespace Abel.PropertyInjection.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static PropertyInjectionServiceProvider WithPropertyInjections(this IServiceProvider serviceProvider) =>
            new(serviceProvider);
    }
}
