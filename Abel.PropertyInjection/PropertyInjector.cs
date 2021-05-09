using System;
using System.Linq;
using System.Reflection;
using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.Interfaces;

namespace Abel.PropertyInjection
{
    public class PropertyInjector : IPropertyInjector
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private readonly IServiceProvider _serviceProvider;

        public PropertyInjector(IServiceProvider serviceProvider) =>
            _serviceProvider = serviceProvider;

        public void InjectProperties(object instance) =>
            instance
                .GetType()
                .GetProperties(Flags)
                .Where(p => p.GetCustomAttribute<InjectAttribute>() != null)
                .ToList()
                .ForEach(p => InjectProperty(instance, p));

        private void InjectProperty(object instance, PropertyInfo p) =>
            p.SetValue(instance, _serviceProvider.GetService(p.PropertyType));
    }
}
