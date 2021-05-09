using System;
using Abel.PropertyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Abel.PropertyInjection
{
    public class PropertyInjectionServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPropertyInjector _propertyInjector;

        public PropertyInjectionServiceProvider(IServiceCollection services)
        {
            _serviceProvider = services.BuildServiceProvider();
            _propertyInjector = new PropertyInjector(_serviceProvider);
        }

        public object GetService(Type serviceType)
        {
            var service = _serviceProvider.GetService(serviceType);
            _propertyInjector.InjectProperties(service, _serviceProvider);
            return service;
        }
    }
}
