using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Abel.PropertyInjection.Infrastructure;
using Abel.PropertyInjection.TestServices;
using Abel.PropertyInjection.TestServices.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Abel.PropertyInjection.Tests
{
    public class PropertyInjectionTests
    {
        [Fact]
        public async Task Inject_PublicSetter_IsInjected() =>
          await TestInstanceInjection<HelloWorldPublicSetter>();

        [Fact]
        public async Task Inject_PrivateSetter_IsInjected() =>
            await TestInstanceInjection<HelloWorldPrivateSetter>();

        [Fact]
        public async Task Inject_NoSetter_IsInjected() =>
            await TestInstanceInjection<HelloWorldNoSetter>();

        [Fact]
        public async Task Inject_PublicField_IsInjected() =>
            await TestInstanceInjection<HelloWorldPublicField>();

        [Fact]
        public async Task Inject_PrivateField_IsInjected() =>
            await TestInstanceInjection<HelloWorldPrivateField>();

        [Fact]
        public async Task Inject_ReadonlyField_IsInjected() =>
            await TestInstanceInjection<HelloWorldReadonlyField>();

        [Fact]
        public async Task Inject_ImplementationFactory_IsInjected() =>
            await TestInjection(s =>
                s.AddHostedService(_ => new HelloWorldPublicSetter()));

        private static async Task TestInstanceInjection<TService>()
            where TService : class, IHostedService =>
            await TestInjection(s =>
                s.AddHostedService<TService>());

        private static async Task TestInjection(Func<IServiceCollection, IServiceCollection> servicesFunc)
        {
            var sb = new StringBuilder();
            await using var stringWriter = new StringWriter(sb);
            Console.SetOut(stringWriter);

            await Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                    servicesFunc(services)
                        .AddTransient<IConsole, CustomConsole>())
                .UsePropertyInjection()
                .RunConsoleAsync();

            sb.ToString().Should().StartWith("Hello World");
        }
    }
}