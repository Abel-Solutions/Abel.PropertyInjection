using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldNoSetter : HelloWorld
    {
        [Inject]
        public IConsole Console { get; }

        public override void Hello() => Console.WriteLine("Hello World");
    }
}