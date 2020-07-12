using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WOTC.LayeredAttribute.Barry;
using WOTC.LayeredAttribute.Barry.Helpers;

namespace WOTC.LayeredAttribute.Tests
{
    [TestClass]
    public class EffectHelperTests
    {
        int a = 12;
        int b = 9;
        [TestMethod]
        public void TestAdd()
        {
            Assert.IsTrue(EffectHelper.ApplyEffect(EffectOperation.Add, a, b) == 21);
        }
        [TestMethod]
        public void TestBitwiseAnd()
        {
            Assert.IsTrue(EffectHelper.ApplyEffect(EffectOperation.BitwiseAnd, a, b) == 8);
        }
        [TestMethod]
        public void TestBitwiseOr()
        {
            Assert.IsTrue(EffectHelper.ApplyEffect(EffectOperation.BitwiseOr, a, b) == 13);
        }
        [TestMethod]
        public void TestBitwiseXor()
        {
            Assert.IsTrue(EffectHelper.ApplyEffect(EffectOperation.BitwiseXor, a, b) == 5);
        }
        [TestMethod]
        public void TestInvalid()
        {
            Assert.IsTrue(EffectHelper.ApplyEffect(EffectOperation.Invalid, a, b) == 0);
        }
        [TestMethod]
        public void TestMultiply()
        {
            Assert.IsTrue(EffectHelper.ApplyEffect(EffectOperation.Multiply, a, b) == 108);

        }

        [TestMethod]
        public void TestSet()
        {
            Assert.IsTrue(EffectHelper.ApplyEffect(EffectOperation.Set, a, b) == b);
        }

        [TestMethod]
        public void TestSubtract()
        {
            Assert.IsTrue(EffectHelper.ApplyEffect(EffectOperation.Subtract, a, b) == 3);
        }
      
    }
}
