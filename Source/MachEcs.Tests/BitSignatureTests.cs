using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubC.MachEcs.Models;
using System.Linq;

namespace MachEcs.Tests
{
  [TestClass]
  public class BitSignatureTests
  {
    [TestMethod]
    public void BitArray1KEcsSignature_WhenDisableBit_DisablesBit()
    {
      // Arrange
      var signature1 = new BitArray1KEcsSignature();
      var signature2 = new BitArray1KEcsSignature();
      signature1.EnableBit(900);
      signature2.EnableBit(900);
      Assert.IsTrue(signature1.IsMatching(signature2));

      // Act
      signature1.DisableBit(900);

      // Assert
      Assert.IsFalse(signature1.IsMatching(signature2));
    }

    [TestMethod]
    public void BitArray1KEcsSignature_WhenEnableBit_CreatesUniqueSignature()
    {
      // Arrange
      var signatures = new BitArray1KEcsSignature[1000];

      // Act
      for (var index = 0; index < signatures.Length; ++index)
      {
        var signature = new BitArray1KEcsSignature();
        signature.EnableBit(index);
        signatures[index] = signature;
      }

      // Assert
      int i = 0;
      var matchingSignatures = signatures.Where(x =>
      {
        ++i;
        return i < signatures.Length && x.IsMatching(signatures[i]);
      });
      Assert.AreEqual(0, matchingSignatures.Count());
    }

    [TestMethod]
    public void BitArray1KEcsSignature_WhenEnableBit_SetsBit()
    {
      // Arrange
      var signature1 = new BitArray1KEcsSignature();
      var signature2 = new BitArray1KEcsSignature();
      var signature3 = new BitArray1KEcsSignature();

      // Act
      signature1.EnableBit(999);
      signature2.EnableBit(500);
      signature3.EnableBit(999);

      // Assert
      Assert.IsTrue(signature1.IsMatching(signature3));
      Assert.IsFalse(signature1.IsMatching(signature2));
    }

    [TestMethod]
    public void DoubleLongEcsSignature_WhenDisableBit_DisablesBit()
    {
      // Arrange
      var signature1 = new DoubleLongEcsSignature();
      var signature2 = new DoubleLongEcsSignature();
      signature1.EnableBit(100);
      signature2.EnableBit(100);
      Assert.IsTrue(signature1.IsMatching(signature2));

      // Act
      signature1.DisableBit(100);

      // Assert
      Assert.IsFalse(signature1.IsMatching(signature2));
    }

    [TestMethod]
    public void DoubleLongEcsSignature_WhenEnableBit_CreatesUniqueSignature()
    {
      // Arrange
      var signatures = new DoubleLongEcsSignature[128];

      // Act
      for (var index = 0; index < signatures.Length; ++index)
      {
        var signature = new DoubleLongEcsSignature();
        signature.EnableBit(index);
        signatures[index] = signature;
      }

      // Assert
      int i = 0;
      var matchingSignatures = signatures.Where(x =>
      {
        ++i;
        return i < signatures.Length && x.IsMatching(signatures[i]);
      });
      Assert.AreEqual(0, matchingSignatures.Count());
    }

    [TestMethod]
    public void DoubleLongEcsSignature_WhenEnableBit_SetsBit()
    {
      // Arrange
      var signature1 = new DoubleLongEcsSignature();
      var signature2 = new DoubleLongEcsSignature();
      var signature3 = new DoubleLongEcsSignature();

      // Act
      signature1.EnableBit(127);
      signature2.EnableBit(63);
      signature3.EnableBit(127);

      // Assert
      Assert.IsTrue(signature1.IsMatching(signature3));
      Assert.IsFalse(signature1.IsMatching(signature2));
    }

    [TestMethod]
    public void SingleLongEcsSignature_WhenDisableBit_DisablesBit()
    {
      // Arrange
      var signature1 = new SingleLongEcsSignature();
      var signature2 = new SingleLongEcsSignature();
      signature1.EnableBit(60);
      signature2.EnableBit(60);
      Assert.IsTrue(signature1.IsMatching(signature2));

      // Act
      signature1.DisableBit(60);

      // Assert
      Assert.IsFalse(signature1.IsMatching(signature2));
    }

    [TestMethod]
    public void SingleLongEcsSignature_WhenEnableBit_CreatesUniqueSignature()
    {
      // Arrange
      var signatures = new SingleLongEcsSignature[64];

      // Act
      for (var index = 0; index < signatures.Length; ++index)
      {
        var signature = new SingleLongEcsSignature();
        signature.EnableBit(index);
        signatures[index] = signature;
      }

      // Assert
      int i = 0;
      var matchingSignatures = signatures.Where(x =>
      {
        ++i;
        return i < signatures.Length && x.IsMatching(signatures[i]);
      });
      Assert.AreEqual(0, matchingSignatures.Count());
    }

    [TestMethod]
    public void SingleLongEcsSignature_WhenEnableBit_SetsBit()
    {
      // Arrange
      var signature1 = new SingleLongEcsSignature();
      var signature2 = new SingleLongEcsSignature();
      var signature3 = new SingleLongEcsSignature();

      // Act
      signature1.EnableBit(63);
      signature2.EnableBit(0);
      signature3.EnableBit(63);

      // Assert
      Assert.IsTrue(signature1.IsMatching(signature3));
      Assert.IsFalse(signature1.IsMatching(signature2));
    }
  }
}
