using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldPublicField : HelloWorld
    {
        [Inject]
        public IConsole Console;

        public override void Hello() => Console.WriteLine("Hello World");
    }
}