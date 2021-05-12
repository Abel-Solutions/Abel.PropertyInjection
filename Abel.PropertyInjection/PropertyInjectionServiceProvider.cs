using System;
using System.Linq;
using Abel.PropertyInjection.Attributes;
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
            _propertyInjector = new PropertyInjector(this);
            _services = services.AddSingleton<IPropertyInjectionServiceProvider>(this); // todo
            _originalServiceProvider = services.BuildServiceProvider(); // todo
            InjectServices(services);
            _originalServiceProvider = services.BuildServiceProvider();
        }

        private void InjectServices(IServiceCollection services) =>
            services
                .ToList()
                .ForEach(InjectDescriptor);

        public object GetService(Type serviceType) =>
            GetAnyOriginalService(serviceType) is var service and not null ?
                _propertyInjector.InjectProperties(service) :
                null;

        private object GetAnyOriginalService(Type type) =>
            GetOriginalService(type) ??
            GetOriginalService(GetAssignableService(type));

        private object GetOriginalService(Type type) =>
            type == null ? null : _originalServiceProvider.GetService(type);

        private Type GetAssignableService(Type type) =>
            _services.FirstOrDefault(s => s.ServiceType.IsAssignableTo(type))?.ServiceType;

        private static bool IsInjectable(object service) =>
            service != null && service.GetType().GetAllMembersByAttribute<InjectAttribute>().Any();

        private void InjectDescriptor(ServiceDescriptor descriptor)
        {
            if (CreateInstance(descriptor) is var service && IsInjectable(service))
            {
                ReplaceDescriptor(descriptor, service);
            }
        }

        private void ReplaceDescriptor(ServiceDescriptor descriptor, object service) =>
            _services.Replace(new ServiceDescriptor(descriptor.ServiceType, GetFactory(service), descriptor.Lifetime));

        private Func<IServiceProvider, object> GetFactory(object instance) =>
            _ => _propertyInjector.InjectProperties(instance);

        private object CreateInstance(ServiceDescriptor descriptor) =>
            GetImplementationInstance(descriptor) ??
            CreateImplementationInstance(descriptor) ??
            CreateImplementationFromFactory(descriptor);

        private static object GetImplementationInstance(ServiceDescriptor descriptor) =>
            descriptor.ImplementationInstance;

        private object CreateImplementationInstance(ServiceDescriptor descriptor) =>
            descriptor.ImplementationType is { IsGenericTypeDefinition: false } type ? ActivateInstance(type) : null;

        private object ActivateInstance(Type type)
        {
            try { return ActivatorUtilities.CreateInstance(this, type); }
            catch { return null; }
        }

        private object CreateImplementationFromFactory(ServiceDescriptor descriptor) =>
            descriptor.ImplementationFactory?.Invoke(this);
    }
}