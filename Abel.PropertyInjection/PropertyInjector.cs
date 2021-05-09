using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.Exceptions;
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

        private void InjectProperty(object instance, PropertyInfo prop)
        {
            var service = GetService(prop.PropertyType);

            if (prop.CanWrite)
            {
                SetValue(prop, instance, service);
                return;
            }

            if (GetBackingField(prop) is var backingField and not null)
            {
                SetValue(backingField, instance, service);
                return;
            }

            throw new NotInjectableException($"Unable to inject {service.GetType().Name} into property {prop.Name}");
        }

        private static void SetValue(PropertyInfo prop, object instance, object value) =>
            prop.SetValue(instance, value);

        private static void SetValue(FieldInfo field, object instance, object service) =>
            field.SetValue(instance, service);

        private static FieldInfo GetBackingField(PropertyInfo prop) =>
            prop.DeclaringType.GetField($"<{prop.Name}>k__BackingField", Flags);

        private object GetService(Type type) =>
            _serviceProvider.GetService(type);
    }
}
