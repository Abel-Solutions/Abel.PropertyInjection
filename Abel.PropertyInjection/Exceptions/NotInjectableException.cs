using System;

namespace Abel.PropertyInjection.Exceptions
{
    public class NotInjectableException : Exception
    {
        public NotInjectableException(string message) : base(message)
        {
        }
    }
}
