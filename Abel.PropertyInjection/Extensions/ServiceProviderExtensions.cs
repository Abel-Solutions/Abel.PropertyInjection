using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Abel.PropertyInjection.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static object GetServiceInHierarchy(this IServiceProvider serviceProvider, Type type, IServiceCollection services) =>
            serviceProvider.GetService(type) ??
            serviceProvider.GetService(GetAssignableService(type, services));

        private static Type GetAssignableService(Type type, IServiceCollection services) => 
            services.FirstOrDefault(s => s.ServiceType.IsAssignableTo(type)).ServiceType;
    }
}
