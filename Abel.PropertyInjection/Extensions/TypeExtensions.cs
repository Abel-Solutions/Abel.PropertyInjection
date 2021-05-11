using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Abel.PropertyInjection.Extensions
{
    public static class TypeExtensions
    {
        private static BindingFlags BindingFlags => BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static IEnumerable<MemberInfo> GetAllMembers(this Type type) =>
            type.GetMembers(BindingFlags);

        public static FieldInfo GetAnyField(this Type type, string name) =>
            type.GetField(name, BindingFlags);

        public static IEnumerable<MemberInfo> GetAllMembersInHierarchyByAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute =>
            type.GetAllMembersInHierarchy()
                .Where(m => m.HasAttribute<TAttribute>());

        public static bool HasAnyMemberInHierarchyAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute =>
            type.GetAllMembers().Any(m => m.HasAttribute<TAttribute>()) ||
            type.BaseType != null && HasAnyMemberInHierarchyAttribute<TAttribute>(type.BaseType);

        public static IEnumerable<MemberInfo> GetAllMembersInHierarchy(this Type type, List<MemberInfo> members = null)
        {
            members ??= new List<MemberInfo>();
            members.AddRange(type.GetAllMembers());
            return type.BaseType == null
                ? members
                : GetAllMembersInHierarchy(type.BaseType, members);
        }

        private static bool HasAttribute<TAttribute>(this MemberInfo member) =>
            member.IsDefined(typeof(TAttribute), true);
    }
}
