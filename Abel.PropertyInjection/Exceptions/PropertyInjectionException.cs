using System;

namespace Abel.PropertyInjection.Exceptions
{
    public class PropertyInjectionException : Exception
    {
        public PropertyInjectionException(string message) : base(message)
        {
        }
    }
}
