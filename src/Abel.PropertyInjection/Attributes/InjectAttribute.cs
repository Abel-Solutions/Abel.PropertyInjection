using System;

namespace Abel.PropertyInjection.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
    }
}
