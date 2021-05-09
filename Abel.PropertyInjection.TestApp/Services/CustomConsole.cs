using System;

namespace Abel.PropertyInjection.TestApp.Services
{
    public class CustomConsole : IConsole
    {
        public void WriteLine(string s) => Console.WriteLine(s);
    }
}