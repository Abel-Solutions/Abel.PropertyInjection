using System;
using System.Collections.Generic;
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
            GetInjectableProperties(instance)
                .ToList().ForEach(prop => InjectProperty(instance, prop));

        private static IEnumerable<PropertyInfo> GetInjectableProperties(object instance) =>
            instance
                .GetType()
                .GetProperties(Flags)
                .Where(IsInjectable);

        private static bool IsInjectable(PropertyInfo prop) =>
            prop.GetCustomAttribute<InjectAttribute>() != null;

        private void InjectProperty(object instance, PropertyInfo prop) =>
            prop.SetValue(instance, GetService(prop));

        private object GetService(PropertyInfo prop) =>
            _serviceProvider.GetService(prop.PropertyType);
    }
}
