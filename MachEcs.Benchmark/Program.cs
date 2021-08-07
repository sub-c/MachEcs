using System;
using BenchmarkDotNet.Running;

namespace MachEcs.Benchmark
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BitArrayBenchmarks>();
        }
    }
}
