using MachEcs.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubC.MachEcs;
using System.Linq;

namespace MachEcs.Tests
{
  [TestClass]
  public class AgentTests
  {
    [TestMethod]
    public void RoundTripTest()
    {
      Agent agent = Agent.CreateInstance(5000, EcsSignatureType.SingleLong);
      var movementSystem = agent.RegisterSystem<MovementSystem>();

      var entity = agent.CreateEntity();
      var position = new PositionComponent { X = 5, Y = 10 };
      var velocity = new VelocityComponent { DX = 2.2f, DY = 3.3f };
      agent.AddComponent(entity, position);
      agent.AddComponent(entity, velocity);

      var systemEntities = movementSystem.GetAllSystemEntities();

      Assert.AreEqual(1, systemEntities.Count());
      Assert.AreEqual(entity, systemEntities.First());
      Assert.AreEqual(position, agent.GetComponent<PositionComponent>(entity));
      Assert.AreEqual(velocity, agent.GetComponent<VelocityComponent>(entity));

      agent.RemoveComponent<PositionComponent>(entity);
      systemEntities = movementSystem.GetAllSystemEntities();

      Assert.AreEqual(0, systemEntities.Count());
      Assert.AreEqual(velocity, agent.GetComponent<VelocityComponent>(entity));

      agent.AddComponent(entity, position);
      systemEntities = movementSystem.GetAllSystemEntities();

      Assert.AreEqual(1, systemEntities.Count());
      Assert.AreEqual(position, agent.GetComponent<PositionComponent>(entity));
      Assert.AreEqual(velocity, agent.GetComponent<VelocityComponent>(entity));

      var entity2 = agent.CreateEntity();
      var position2 = new PositionComponent();
      var velocity2 = new VelocityComponent();
      agent.AddComponent(entity2, position2);
      agent.AddComponent(entity2, velocity2);
      systemEntities = movementSystem.GetAllSystemEntities();

      Assert.AreEqual(2, systemEntities.Count());
      Assert.AreEqual(position2, agent.GetComponent<PositionComponent>(entity2));
      Assert.AreEqual(velocity2, agent.GetComponent<VelocityComponent>(entity2));

      var getSystemEntitiesEvent = new GetSystemEntitiesEvent();
      agent.SendEvent(getSystemEntitiesEvent);

      Assert.AreEqual(systemEntities, getSystemEntitiesEvent.Entities);
    }
  }

}
