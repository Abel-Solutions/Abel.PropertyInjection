using System;

namespace Abel.PropertyInjection.Exceptions
{
    public class PropertyInjectionException : Exception
    {
        public PropertyInjectionException()
        {
        }

        public PropertyInjectionException(string message) : base(message)
        {
        }

        public PropertyInjectionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
