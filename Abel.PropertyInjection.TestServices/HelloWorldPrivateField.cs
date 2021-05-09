using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldPrivateField : IHelloWorld
    {
        [Inject] 
        private IConsole _console;

        public void Hello() => _console.WriteLine("Hello World");
    }
}