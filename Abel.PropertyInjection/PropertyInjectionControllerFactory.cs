using Abel.PropertyInjection.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Abel.PropertyInjection
{
    public class PropertyInjectionControllerFactory : IControllerFactory
    {
        private readonly IControllerActivator _controllerActivator;
        private readonly IPropertyInjector _propertyInjector;

        public PropertyInjectionControllerFactory(IControllerActivator controllerActivator, IPropertyInjector propertyInjector)
        {
            _controllerActivator = controllerActivator;
            _propertyInjector = propertyInjector;
        }

        public object CreateController(ControllerContext context)
        {
            var controller = _controllerActivator.Create(context);
            _propertyInjector.InjectProperties(controller, context.HttpContext.RequestServices);
            return controller;
        }

        public void ReleaseController(ControllerContext context, object controller) =>
            _controllerActivator.Release(context, controller);
    }
}
