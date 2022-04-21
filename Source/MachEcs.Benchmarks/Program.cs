using BenchmarkDotNet.Running;

namespace SubC.MachEcs.Benchmarks
{
  public class Program
  {
    public static void Main(string[] args)
    {
      //BenchmarkRunner.Run<BitArrayBenchmarks>();
      BenchmarkRunner.Run<EventWorkerBenchmarks>();
    }
  }
}
