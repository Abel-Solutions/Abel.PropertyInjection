using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldPublicSetter : HelloWorld
    {
        [Inject]
        public IConsole Console { get; set; }

        public override void Hello() => Console.WriteLine("Hello World");
    }
}