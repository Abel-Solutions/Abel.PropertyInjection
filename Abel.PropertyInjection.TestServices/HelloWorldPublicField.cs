using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldPublicField : IHelloWorld
    {
        [Inject] 
        public IConsole Console;

        public void Hello() => Console.WriteLine("Hello World");
    }
}