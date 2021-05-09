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
            GetInjectableMembers(instance)
                .ToList().ForEach(member => InjectMember(instance, member));

        private static IEnumerable<MemberInfo> GetInjectableMembers(object instance) =>
            instance
                .GetType()
                .GetMembers(Flags)
                .Where(IsInjectable);

        private static bool IsInjectable(MemberInfo member) =>
            member.GetCustomAttribute<InjectAttribute>() != null;

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
            prop.DeclaringType.GetField($"<{prop.Name}>k__BackingField", Flags);

        private object GetService(Type type) =>
            _serviceProvider.GetService(type);
    }
}
