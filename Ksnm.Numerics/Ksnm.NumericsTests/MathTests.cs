using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ksnm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksnm.Tests
{
    [TestClass()]
    public class MathTests
    {
        [TestMethod()]
        public void GreatestCommonDivisorTest()
        {
            {
                var actual = Math.GreatestCommonDivisor(18, 24);
                Assert.AreEqual(6, actual);
            }
            {
                var actual = Math.GreatestCommonDivisor(12, 45);
                Assert.AreEqual(3, actual);
            }
        }
        [TestMethod()]
        public void SinTest()
        {
            for (double i = -10; i <= 10; i += 0.125)
            {
                var expected = System.Math.Sin(i);
                var actual = Math.Sin(i, 0.00000_00000_001);
                Assert.AreEqual(expected, actual, 0.00000_00000_001);
            }
            for (double i = -10; i <= 10; i += 0.125)
            {
                var expected = (decimal)System.Math.Sin(i);
                var actual = Math.Sin((decimal)i, 0.00000_00000_001m);
                Assert.AreEqual(expected, actual, 0.00000_00000_001m);
            }
        }
        [TestMethod()]
        public void CosTest()
        {
            for (double i = -10; i <= 10; i += 0.125)
            {
                var expected = System.Math.Cos(i);
                var actual = Math.Cos(i, 0.00000_00000_001);
                Assert.AreEqual(expected, actual, 0.00000_00000_001);
            }
            for (double i = -10; i <= 10; i += 0.125)
            {
                var expected = (decimal)System.Math.Cos(i);
                var actual = Math.Cos((decimal)i, 0.00000_00000_001m);
                Assert.AreEqual(expected, actual, 0.00000_00000_001m);
            }
        }
        [TestMethod()]
        public void TanTest()
        {
            for (double i = -10; i <= 10; i += 0.125)
            {
                var expected = System.Math.Tan(i);
                var actual = Math.Tan(i, 0.00000_00000_001);
                Assert.AreEqual(expected, actual, 0.00000_00000_1);
            }
            for (double i = -10; i <= 10; i += 0.125)
            {
                var expected = (decimal)System.Math.Tan(i);
                var actual = Math.Tan((decimal)i, 0.00000_00000_001m);
                Assert.AreEqual(expected, actual, 0.00000_00000_1m);
            }
        }
        [TestMethod()]
        public void AsinTest()
        {
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = System.Math.Asin(i);
                var actual = Math.Asin(i, 0.00000_00000_001);
                Assert.AreEqual(expected, actual, 0.00000_00000_01);
            }
            // オーバーフローするため計算回数が少ない→精度が悪い
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = (decimal)System.Math.Asin(i);
                var actual = Math.Asin((decimal)i, 0.00000_00000_001m, 22);
                Assert.AreEqual(expected, actual, 0.0001m);
            }
        }
    }
}