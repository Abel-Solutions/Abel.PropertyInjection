using System;
using System.IO;
using System.Text;
using Abel.PropertyInjection.Extensions;
using Abel.PropertyInjection.TestServices;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Abel.PropertyInjection.Tests
{
    public class PropertyInjectionTests
    {
        [Fact]
        public void Lol()
        {
            var services = new ServiceCollection()
                .AddTransient<IHelloWorld, HelloWorld>()
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