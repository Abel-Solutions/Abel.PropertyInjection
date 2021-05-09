using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldPublicSetter : IHelloWorld
    {
        [Inject]
        public IConsole Console { get; set; }

        public void Hello() => Console.WriteLine("Hello World");
    }
}