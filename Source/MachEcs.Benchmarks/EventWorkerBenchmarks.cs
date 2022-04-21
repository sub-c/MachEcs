using BenchmarkDotNet.Attributes;
using SubC.MachEcs.Worker;

namespace SubC.MachEcs.Benchmarks
{
  [MemoryDiagnoser(false)]
  public class EventWorkerBenchmarks
  {
    private readonly Agent _agent = Agent.CreateInstance(5000, EcsSignatureType.SingleLong);
    private readonly EventWorker _eventWorker = new();
    private readonly TestEventData _testEventData = new();
    private int _counter = 0;

    public EventWorkerBenchmarks()
    {
      _agent.RegisterEventHandler<TestEventData>(TestEventHandler);
      _agent.RegisterEventHandler<TestEventData>(TestEventDataHandler);
      _eventWorker.RegisterEventHandler<TestEventData>(TestEventHandler);
      _eventWorker.RegisterEventHandler<TestEventData>(TestEventDataHandler);
    }

    [Benchmark]
    public void AgentEventWithData()
    {
      _agent.SendEvent(_testEventData);
    }

    [Benchmark]
    public void AgentEventWithoutData()
    {
      _agent.SendEvent<TestEventData>();
    }

    [Benchmark]
    public void EventWithoutData()
    {
      _eventWorker.SendEvent<TestEventData>();
    }

    [Benchmark]
    public void EventWithData()
    {
      _eventWorker.SendEvent(_testEventData);
    }

    private class TestEventData : IEcsEvent
    {
      public string Data { get; set; } = string.Empty;
    }

    private void TestEventHandler()
    {
      ++_counter;
      if (_counter == -1)
      {
        --_counter;
      }
    }

    private void TestEventDataHandler(TestEventData testEventData)
    {
      ++_counter;
      if (_counter == -1)
      {
        --_counter;
      }
    }
  }
}
