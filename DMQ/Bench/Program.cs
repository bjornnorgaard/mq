using BenchmarkDotNet.Running;

namespace Bench
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set configuration to Release before running without debugger.
            BenchmarkRunner.Run<CacheBenchmarks>();
        }
    }
}