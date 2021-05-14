using System;
using Abel.PropertyInjection.TestServices.Interfaces;

namespace Abel.PropertyInjection.TestServices
{
    public class CustomConsole : IConsole
    {
        public void WriteLine(string s) => Console.WriteLine(s);
    }
}