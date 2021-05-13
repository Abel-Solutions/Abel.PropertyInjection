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
            _services = services.AddSingleton<IPropertyInjectionServiceProvider>(this);
            InjectServices(services);
            _originalServiceProvider = services.BuildServiceProvider();
        }

        private void InjectServices(IServiceCollection services) =>
            services
                .Where(IsInjectable)
                .ToList()
                .ForEach(InjectDescriptor);

        public object GetService(Type type) =>
            _propertyInjector.InjectProperties(GetAnyOriginalService(type));

        private object GetAnyOriginalService(Type type) =>
            _originalServiceProvider.GetService(type) ??
            _originalServiceProvider.GetService(GetAssignableService(type));

        private Type GetAssignableService(Type type) =>
            _services.FirstOrDefault(s => s.ServiceType.IsAssignableTo(type))?.ServiceType;

        private static bool IsInjectable(ServiceDescriptor descriptor) =>
            !descriptor.ServiceType.IsGenericTypeDefinition &&
            descriptor.InvokeMethod<Type>("GetImplementationType")
                .HasAnyMemberAttribute<InjectAttribute>();

        private void InjectDescriptor(ServiceDescriptor descriptor) =>
            _services.Replace(new ServiceDescriptor(descriptor.ServiceType, CreateNewFactory(descriptor), descriptor.Lifetime));

        private Func<IServiceProvider, object> CreateNewFactory(ServiceDescriptor descriptor) =>
            _ => _propertyInjector.InjectProperties(CreateInstance(descriptor));

        private object CreateInstance(ServiceDescriptor descriptor) =>
            GetImplementationInstance(descriptor) ??
            CreateImplementationInstance(descriptor) ??
            CreateImplementationFromFactory(descriptor);

        private static object GetImplementationInstance(ServiceDescriptor descriptor) =>
            descriptor.ImplementationInstance;

        private object CreateImplementationInstance(ServiceDescriptor descriptor) =>
            descriptor.ImplementationType != null ?
                ActivatorUtilities.CreateInstance(this, descriptor.ImplementationType) :
                null;

        private object CreateImplementationFromFactory(ServiceDescriptor descriptor) =>
            descriptor.ImplementationFactory?.Invoke(this);
    }
}