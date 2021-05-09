using System;

namespace Abel.PropertyInjection.TestApi.Services
{
    public class CustomConsole : IConsole
    {
        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }
    }
}