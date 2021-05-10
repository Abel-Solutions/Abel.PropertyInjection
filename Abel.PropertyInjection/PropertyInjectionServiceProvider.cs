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
            _propertyInjector = new PropertyInjector(_serviceProvider);

            services
                .Where(IsInjectable)
                .ToList()
                .ForEach(descriptor => InjectDescriptor(services, descriptor));

            _serviceProvider = services.BuildServiceProvider();
        }

        public object GetService(Type serviceType) => 
            _propertyInjector.InjectProperties(_serviceProvider.GetService(serviceType), _serviceProvider);

        private static bool IsInjectable(ServiceDescriptor service) =>
            service.ImplementationType != null &&
            service.ImplementationType.GetMembers().Any(m => m.IsDefined(typeof(InjectAttribute), false));

        private void InjectDescriptor(IServiceCollection defaultServiceCollection, ServiceDescriptor service) =>
            defaultServiceCollection.Replace(new ServiceDescriptor(service.ServiceType, GetFactory(service), service.Lifetime));

        private Func<IServiceProvider, object> GetFactory(ServiceDescriptor service) =>
            serviceProvider =>
                _propertyInjector.InjectProperties(CreateInstance(service, serviceProvider), serviceProvider);

        private static object CreateInstance(ServiceDescriptor descriptor, IServiceProvider serviceProvider)
        {
            var implementationInstance = descriptor.ImplementationInstance;
            if (implementationInstance != null)
            {
                return implementationInstance;
            }

            var implementationType = descriptor.ImplementationType;
            if (implementationType != null)
            {
                return ActivatorUtilities.CreateInstance(serviceProvider, implementationType);
            }

            var implementationFactory = descriptor.ImplementationFactory;
            if (implementationFactory != null)
            {
                return implementationFactory.Invoke(serviceProvider);
            }

            throw new NotSupportedException(); // todo cleanup
        }
    }
}
