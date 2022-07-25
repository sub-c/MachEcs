using BenchmarkDotNet.Attributes;
using System;

namespace SubC.MachEcs.Benchmarks
{
  internal class ConcreteComponent : IEcsComponent
  {
    public string Data = string.Empty;
  }

  [MemoryDiagnoser(false)]
  public class TypeBenchmarks
  {
    private static readonly ConcreteComponent cc = new ConcreteComponent();

    [Benchmark]
    public void UsingBaseClass()
    {
      var a = UseGeneric<ConcreteComponent>();
      if (a == null)
        throw new Exception("no");
    }

    [Benchmark]
    public void UsingGenerics()
    {
      var a = UseBaseClass(cc);
      if (a == null)
        throw new Exception("no");
    }

    [Benchmark]
    public void UsingGenericsWithBaseClass()
    {
      var a = UseBaseClassWithGeneric<ConcreteComponent>(cc);
      if (a == null)
        throw new Exception("no");
    }

    private static IEcsComponent? UseGeneric<T>()
      where T : IEcsComponent
    {
      var type = typeof(T);
      if (type == typeof(ConcreteComponent))
      {
        return cc;
      }
      return default;
    }

    private static IEcsComponent? UseBaseClass(IEcsComponent component)
    {
      var type = component.GetType();
      if (type == typeof(ConcreteComponent))
      {
        return cc;
      }
      return null;
    }

    private static IEcsComponent? UseBaseClassWithGeneric<T>(IEcsComponent component)
      where T : IEcsComponent
    {
      var type = component.GetType();
      if (type == typeof(T))
      {
        return cc;
      }
      return default;
    }
  }
}
