using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Abel.PropertyInjection.Extensions
{
    public static class TypeExtensions
    {
        private const BindingFlags Binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static IEnumerable<MemberInfo> GetMembersWithAttribute<TAttribute>(this Type type, List<MemberInfo> members = null)
            where TAttribute : Attribute
        {
            members ??= new List<MemberInfo>();
            members.AddRange(type.GetMembers(Binding).Where(m => m.IsDefined(typeof(TAttribute), true)));
            return type.BaseType == null
                ? members
                : GetMembersWithAttribute<TAttribute>(type.BaseType, members);
        }
    }
}
