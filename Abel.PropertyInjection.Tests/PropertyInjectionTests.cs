using System;
using System.IO;
using System.Text;
using Abel.PropertyInjection.Extensions;
using Abel.PropertyInjection.TestServices;
using Abel.PropertyInjection.TestServices.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Abel.PropertyInjection.Tests
{
    public class PropertyInjectionTests
    {
        [Fact]
        public void Inject_PublicSet_IsInjected()
        {
            var services = new ServiceCollection()
                .AddTransient<IHelloWorld, HelloWorldPublicSet>()
                .AddTransient<IConsole, CustomConsole>();

            var serviceProvider = services.BuildServiceProvider()
                .WithPropertyInjections();

            var sb = new StringBuilder();

            Console.SetOut(new StringWriter(sb));

            var helloWorld = serviceProvider.GetService<IHelloWorld>();
            helloWorld.Hello();

            sb.ToString().Should().Be("Hello World" + Environment.NewLine);
        }

        [Fact]
        public void Inject_PrivateSet_IsInjected()
        {
            var services = new ServiceCollection()
                .AddTransient<IHelloWorld, HelloWorldPrivateSet>()
                .AddTransient<IConsole, CustomConsole>();

            var serviceProvider = services.BuildServiceProvider()
                .WithPropertyInjections();

            var sb = new StringBuilder();

            Console.SetOut(new StringWriter(sb));

            var helloWorld = serviceProvider.GetService<IHelloWorld>();
            helloWorld.Hello();

            sb.ToString().Should().Be("Hello World" + Environment.NewLine);
        }
    }
}