using System.Threading;
using System.Threading.Tasks;
using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldNoSetter : IHelloWorld, IHostedService
    {
        [Inject]
        public IConsole Console { get; }

        public void Hello() => Console.WriteLine("Hello World");

        public async Task StartAsync(CancellationToken cancellationToken) => 
            Hello();

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}