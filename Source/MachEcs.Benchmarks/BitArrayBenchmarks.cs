using BenchmarkDotNet.Attributes;
using SubC.MachEcs.Models;

namespace SubC.MachEcs.Benchmarks
{
  [MemoryDiagnoser(false)]
  public class BitArrayBenchmarks
  {
    private readonly SingleLongEcsSignature _singleLong1 = new();
    private readonly SingleLongEcsSignature _singleLong2 = new();

    private readonly DoubleLongEcsSignature _doubleLong1 = new();
    private readonly DoubleLongEcsSignature _doubleLong2 = new();

    private readonly BitArray1KEcsSignature _bitArray1K1 = new();
    private readonly BitArray1KEcsSignature _bitArray1K2 = new();

    public BitArrayBenchmarks()
    {
    }

    [Benchmark]
    public void SingleLong()
    {
      if (_singleLong1.IsMatching(_singleLong2))
      {
        _singleLong1.EnableBit(4);
        _singleLong2.EnableBit(4);
      }
      if (_singleLong1.IsMatching(_singleLong2))
      {
        _singleLong2.DisableBit(4);
      }
    }

    [Benchmark]
    public void DoubleLong()
    {
      if (_doubleLong1.IsMatching(_doubleLong2))
      {
        _doubleLong1.EnableBit(4);
        _doubleLong2.EnableBit(4);
      }
      if (_doubleLong1.IsMatching(_doubleLong2))
      {
        _doubleLong2.DisableBit(4);
      }
    }

    [Benchmark]
    public void BitArray1KLong()
    {
      if (_bitArray1K1.IsMatching(_bitArray1K2))
      {
        _bitArray1K1.EnableBit(4);
        _bitArray1K2.EnableBit(4);
      }
      if (_bitArray1K1.IsMatching(_bitArray1K2))
      {
        _bitArray1K2.DisableBit(4);
      }
    }
  }
}
