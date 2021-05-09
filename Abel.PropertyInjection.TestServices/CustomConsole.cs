using System;

namespace Abel.PropertyInjection.TestServices
{
    public class CustomConsole : IConsole
    {
        public void WriteLine(string s) => Console.WriteLine(s);
    }
}