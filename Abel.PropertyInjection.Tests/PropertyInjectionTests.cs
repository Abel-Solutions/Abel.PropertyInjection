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
          await TestInjection<HelloWorldPublicSetter>();

        [Fact]
        public async Task Inject_PrivateSetter_IsInjected() =>
            await TestInjection<HelloWorldPrivateSetter>();

        [Fact]
        public async Task Inject_NoSetter_IsInjected() =>
            await TestInjection<HelloWorldNoSetter>();

        [Fact]
        public async Task Inject_PublicField_IsInjected() =>
            await TestInjection<HelloWorldPublicField>();

        [Fact]
        public async Task Inject_PrivateField_IsInjected() =>
            await TestInjection<HelloWorldPrivateField>();

        private static async Task TestInjection<TService>()
            where TService : class, IHostedService
        {
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            await Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices<TService>)
                .UsePropertyInjection()
                .RunConsoleAsync();

            sb.ToString().Should().StartWith("Hello World");
        }

        private static void ConfigureServices<TService>(IServiceCollection services)
            where TService : class, IHostedService =>
            services
                .AddHostedService<TService>()
                .AddTransient<IConsole, CustomConsole>();
    }
}