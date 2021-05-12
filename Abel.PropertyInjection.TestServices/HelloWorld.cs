using System.Threading;
using System.Threading.Tasks;
using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Abel.PropertyInjection.TestServices
{
    public abstract class HelloWorld : IHelloWorld, IHostedService
    {
        [Inject]
        private IHostApplicationLifetime _appLifetime;

        public abstract void Hello();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Hello();
            _appLifetime.StopApplication();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => 
            Task.CompletedTask;
    }
}
