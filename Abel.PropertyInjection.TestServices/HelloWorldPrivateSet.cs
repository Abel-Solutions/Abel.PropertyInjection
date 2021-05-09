﻿using Abel.PropertyInjection.Attributes;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class HelloWorldPrivateSet : IHelloWorld
    {
        [Inject]
        public IConsole Console { get; private set; }

        public void Hello() => Console.WriteLine("Hello World");
    }
}