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
        public void Inject_PublicSetter_IsInjected() => TestInjection<HelloWorldPublicSetter>();

        [Fact]
        public void Inject_PrivateSetter_IsInjected() => TestInjection<HelloWorldPrivateSetter>();

        [Fact]
        public void Inject_NoSetter_IsInjected() => TestInjection<HelloWorldNoSetter>();

        [Fact]
        public void Inject_PublicField_IsInjected() => TestInjection<HelloWorldPublicField>();

        [Fact]
        public void Inject_PrivateField_IsInjected() => TestInjection<HelloWorldPrivateField>();

        private static void TestInjection<THelloWorld>()
            where THelloWorld : class, IHelloWorld
        {
            var services = new ServiceCollection()
                .AddTransient<IHelloWorld, THelloWorld>()
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