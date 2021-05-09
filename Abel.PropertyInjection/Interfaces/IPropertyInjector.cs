using System;

namespace Abel.PropertyInjection.Interfaces
{
    public interface IPropertyInjector
    {
        void InjectProperties(object instance, IServiceProvider serviceProvider);
    }
}
