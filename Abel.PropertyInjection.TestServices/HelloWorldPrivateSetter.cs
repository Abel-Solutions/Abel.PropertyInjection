using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldPrivateSetter : HelloWorld
    {
        [Inject]
        public IConsole Console { get; private set; }

        public override void Hello() => Console.WriteLine("Hello World");
    }
}