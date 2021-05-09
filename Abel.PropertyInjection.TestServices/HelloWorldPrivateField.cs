using System.Threading;
using System.Threading.Tasks;
using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldPrivateField : IHelloWorld, IHostedService
    {
        [Inject] 
        private IConsole _console;

        public void Hello() => _console.WriteLine("Hello World");

        public async Task StartAsync(CancellationToken cancellationToken) =>
            Hello();

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}