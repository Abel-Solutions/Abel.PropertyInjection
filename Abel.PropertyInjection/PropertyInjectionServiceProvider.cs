using System;
using System.Linq;
using System.Reflection;
using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.Exceptions;
using Abel.PropertyInjection.Extensions;
using Abel.PropertyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Abel.PropertyInjection
{
    public class PropertyInjectionServiceProvider : IPropertyInjectionServiceProvider
    {
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _originalServiceProvider;
        private readonly IPropertyInjector _propertyInjector;

        public PropertyInjectionServiceProvider(IServiceCollection services)
        {
            _services = services.AddSingleton<IPropertyInjectionServiceProvider>(this); // todo
            InjectServices(services);
            _originalServiceProvider = services.BuildServiceProvider();
            _propertyInjector = new PropertyInjector(this);
        }

        private void InjectServices(IServiceCollection services) =>
            services
                .Where(IsInjectable)
                .ToList()
                .ForEach(descriptor => InjectDescriptor(services, descriptor));

        public object GetService(Type serviceType) // todo
        {
            var instance = _originalServiceProvider.GetService(serviceType);

            if (instance == null)
            {
                var assignableTo = _services.Where(s => s.ServiceType.IsAssignableTo(serviceType));
                if (assignableTo.Any())
                {
                    instance = _originalServiceProvider.GetService(assignableTo.First().ServiceType);
                }
            }

            return _propertyInjector.InjectProperties(instance);
        }

        private static bool IsInjectable(ServiceDescriptor descriptor) => // todo use CreateInstance
            descriptor.ImplementationType != null &&
            descriptor.ImplementationType.GetMembersByAttribute<InjectAttribute>().Any();

        private void InjectDescriptor(IServiceCollection defaultServiceCollection, ServiceDescriptor service) =>
            defaultServiceCollection.Replace(new ServiceDescriptor(service.ServiceType, GetFactory(service), service.Lifetime));

        private Func<IServiceProvider, object> GetFactory(ServiceDescriptor service) =>
            _ => _propertyInjector.InjectProperties(CreateInstance(service));

        private object CreateInstance(ServiceDescriptor descriptor) =>
            GetImplementationInstance(descriptor) ??
            CreateImplementationInstance(descriptor) ??
            CreateImplementationFromFactory(descriptor) ??
            throw new PropertyInjectionException($"Could not create instance for descriptor {descriptor.ServiceType.Name}");

        private static object GetImplementationInstance(ServiceDescriptor descriptor) =>
            descriptor.ImplementationInstance;

        private object CreateImplementationInstance(ServiceDescriptor descriptor) =>
            descriptor.ImplementationType is var type and not null ?
                ActivatorUtilities.CreateInstance(this, type) :
                null;

        private object CreateImplementationFromFactory(ServiceDescriptor descriptor) =>
            descriptor.ImplementationFactory?.Invoke(this);
    }
}