using System;
using System.Linq;
using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Abel.PropertyInjection
{
    public class PropertyInjectionServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPropertyInjector _propertyInjector;

        public PropertyInjectionServiceProvider(IServiceCollection services)
        {
            InjectServices(services);
            _serviceProvider = services.BuildServiceProvider();
            _propertyInjector = new PropertyInjector(_serviceProvider);
        }

        private void InjectServices(IServiceCollection services) =>
            services
                .Where(IsInjectable)
                .ToList()
                .ForEach(descriptor => InjectDescriptor(services, descriptor));

        public object GetService(Type serviceType) =>
            _propertyInjector.InjectProperties(_serviceProvider.GetService(serviceType));

        private static bool IsInjectable(ServiceDescriptor service) =>
            service.ImplementationType != null &&
            service.ImplementationType.GetMembers().Any(m => m.IsDefined(typeof(InjectAttribute), false));

        private void InjectDescriptor(IServiceCollection defaultServiceCollection, ServiceDescriptor service) =>
            defaultServiceCollection.Replace(new ServiceDescriptor(service.ServiceType, GetFactory(service), service.Lifetime));

        private Func<IServiceProvider, object> GetFactory(ServiceDescriptor service) =>
            serviceProvider =>
                _propertyInjector.InjectProperties(CreateInstance(service, serviceProvider));

        private static object CreateInstance(ServiceDescriptor descriptor, IServiceProvider serviceProvider) =>
            GetImplementationInstance(descriptor) ??
            CreateImplementationInstance(descriptor, serviceProvider) ??
            CreateImplementationFromFactory(descriptor, serviceProvider);

        private static object GetImplementationInstance(ServiceDescriptor descriptor) =>
            descriptor.ImplementationInstance;

        private static object CreateImplementationInstance(ServiceDescriptor descriptor, IServiceProvider serviceProvider) =>
            descriptor.ImplementationType is var type and not null ?
                ActivatorUtilities.CreateInstance(serviceProvider, type) :
                null;

        private static object CreateImplementationFromFactory(ServiceDescriptor descriptor, IServiceProvider serviceProvider) =>
            descriptor.ImplementationFactory?.Invoke(serviceProvider);
    }
}