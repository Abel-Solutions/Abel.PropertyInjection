using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldPrivateField : HelloWorld
    {
        [Inject]
        private IConsole _console;

        public override void Hello() => _console.WriteLine("Hello World");
    }
}