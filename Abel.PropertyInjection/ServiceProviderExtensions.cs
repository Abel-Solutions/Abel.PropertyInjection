using System;

namespace Abel.PropertyInjection
{
    public static class ServiceProviderExtensions
    {
        public static PropertyInjectionServiceProvider WithPropertyInjections(this IServiceProvider serviceProvider) =>
            new(serviceProvider);
    }
}
