using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldNoSetter : IHelloWorld
    {
        [Inject]
        public IConsole Console { get; }

        public void Hello() => Console.WriteLine("Hello World");
    }
}