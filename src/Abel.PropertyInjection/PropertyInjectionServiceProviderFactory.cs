using System;
using Microsoft.Extensions.DependencyInjection;

namespace Abel.PropertyInjection
{
    public class PropertyInjectionServiceProviderFactory : IServiceProviderFactory<PropertyInjectionServiceProvider>
    {
        public PropertyInjectionServiceProvider CreateBuilder(IServiceCollection services) => 
            new(services);

        public IServiceProvider CreateServiceProvider(PropertyInjectionServiceProvider containerBuilder) => 
            containerBuilder;
    }
}