namespace Abel.PropertyInjection.TestApi.Services
{
    public class HelloWorld : IHelloWorld
    {
        [Inject]
        public IConsole Console { get; set; } // todo field, private prop

        public HelloWorld(IConsole console)
        {
            Console = console;
        }

        public void Hello()
        {
            Console.WriteLine("Hello World");
        }
    }
}