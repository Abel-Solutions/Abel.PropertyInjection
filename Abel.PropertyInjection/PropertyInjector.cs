using System;
using System.Linq;
using System.Reflection;
using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.Exceptions;
using Abel.PropertyInjection.Interfaces;
using Abel.PropertyInjection.Extensions;

namespace Abel.PropertyInjection
{
    public class PropertyInjector : IPropertyInjector
    {
        private const BindingFlags Binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private readonly IPropertyInjectionServiceProvider _serviceProvider;

        public PropertyInjector(IPropertyInjectionServiceProvider serviceProvider) =>
            _serviceProvider = serviceProvider;

        public object InjectProperties(object instance)
        {
            instance.GetType().GetMembersByAttribute<InjectAttribute>()
                .ToList().ForEach(member => InjectMember(instance, member));
            return instance;
        }

        private void InjectMember(object instance, MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Property)
            {
                InjectProperty(instance, (PropertyInfo)member);
                return;
            }

            InjectField(instance, (FieldInfo)member);
        }

        private void InjectProperty(object instance, PropertyInfo prop)
        {
            if (prop.CanWrite)
            {
                prop.SetValue(instance, GetService(prop.PropertyType));
                return;
            }

            InjectField(instance, GetBackingField(prop));
        }

        private void InjectField(object instance, FieldInfo field) =>
            field.SetValue(instance, GetService(field.FieldType));

        private static FieldInfo GetBackingField(PropertyInfo prop) =>
            prop.DeclaringType.GetField($"<{prop.Name}>k__BackingField", Binding) ??
            throw new PropertyInjectionException($"Could not find backing field of read-only property {prop.Name}");

        private object GetService(Type type) =>
            _serviceProvider.GetService(type) ??
                   throw new PropertyInjectionException($"Could not find service for type {type.Name}");
    }
}
