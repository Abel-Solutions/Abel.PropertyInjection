using Abel.PropertyInjection.Attributes;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorld : IHelloWorld
    {
        [Inject]
        public IConsole Console { get; set; } // todo field, private prop

        public void Hello() => Console.WriteLine("Hello World");
    }
}