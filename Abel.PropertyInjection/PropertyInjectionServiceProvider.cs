using System;
using Abel.PropertyInjection.Interfaces;

namespace Abel.PropertyInjection
{
    public class PropertyInjectionServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPropertyInjector _propertyInjector;

        public PropertyInjectionServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _propertyInjector = new PropertyInjector(serviceProvider);
        }

        public object GetService(Type serviceType)
        {
            var service = _serviceProvider.GetService(serviceType);
            _propertyInjector.InjectProperties(service);
            return service;
        }
    }
}
