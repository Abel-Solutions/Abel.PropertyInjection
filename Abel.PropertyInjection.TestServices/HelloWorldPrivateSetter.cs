using System.Threading;
using System.Threading.Tasks;
using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldPrivateSetter : IHelloWorld, IHostedService
    {
        [Inject]
        public IConsole Console { get; private set; }

        public void Hello() => Console.WriteLine("Hello World");

        public async Task StartAsync(CancellationToken cancellationToken) =>
            Hello();

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}