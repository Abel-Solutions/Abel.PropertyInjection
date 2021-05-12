using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abel.PropertyInjection.Exceptions;

namespace Abel.PropertyInjection.Extensions
{
    public static class TypeExtensions
    {
        private static BindingFlags BindingFlags => BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static FieldInfo GetBackingField(this PropertyInfo prop) =>
            prop.DeclaringType.GetField($"<{prop.Name}>k__BackingField", BindingFlags) ??
            throw new PropertyInjectionException($"Could not find backing field of read-only property {prop.Name}");

        public static bool HasAnyMemberAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute =>
            type.GetMembers(BindingFlags)
                .Any(m => m.HasAttribute<TAttribute>()) ||
            type.BaseType != null && HasAnyMemberAttribute<TAttribute>(type.BaseType);

        public static IEnumerable<MemberInfo> GetAllMembersByAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute =>
            type.GetAllMembers()
                .Where(m => m.HasAttribute<TAttribute>());

        private static IEnumerable<MemberInfo> GetAllMembers(this Type type, List<MemberInfo> members = null)
        {
            members ??= new List<MemberInfo>();
            members.AddRange(type.GetMembers(BindingFlags));
            return type.BaseType == null
                ? members
                : GetAllMembers(type.BaseType, members);
        }

        private static bool HasAttribute<TAttribute>(this MemberInfo member) =>
            member.IsDefined(typeof(TAttribute), true);
    }
}
