using System;
using System.Threading.Tasks;
using Api.Interfaces;

namespace Api.Services
{
    public class SomeImplementation : ISomeService
    {
        public async Task DoSomethingAsync()
        {
            Console.WriteLine($"{nameof(SomeImplementation)} started");
            await Task.Delay(5000);
            Console.WriteLine($"{nameof(SomeImplementation)} finished");
        }
    }
}