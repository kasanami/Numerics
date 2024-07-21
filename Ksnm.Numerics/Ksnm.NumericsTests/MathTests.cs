using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ksnm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksnm.Numerics;

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
            BigDecimal.DefaultMinExponent = -100;
            var tolerance = (BigDecimal)0.00000_00000_001m;
            for (double i = -10; i <= 10; i += 0.125)
            {
                var expected = (BigDecimal)System.Math.Sin(i);
                var actual = Math.Sin((BigDecimal)i, tolerance);
                Assert.IsTrue(BigDecimal.Abs(expected - actual) < tolerance);
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
            BigDecimal.DefaultMinExponent = -100;
            var tolerance = (BigDecimal)0.00000_00000_001m;
            for (double i = -10; i <= 10; i += 0.125)
            {
                var expected = (BigDecimal)System.Math.Cos(i);
                var actual = Math.Cos((BigDecimal)i, tolerance);
                Assert.IsTrue(BigDecimal.Abs(expected - actual) < tolerance);
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
            BigDecimal.DefaultMinExponent = -100;
            for (double i = -10; i <= 10; i += 0.125)
            {
                var expected = (BigDecimal)System.Math.Tan(i);
                var actual = Math.Tan((BigDecimal)i, 0.00000_00000_001m);
                Assert.IsTrue(BigDecimal.Abs(expected - actual) < 0.00000_00000_1m, $"{nameof(expected)}={expected} {nameof(actual)}={actual}");
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
            BigDecimal.DefaultMinExponent = -100;
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = (BigDecimal)System.Math.Asin(i);
                var actual = Math.Asin((BigDecimal)i, 0.00000_00000_001m);
                Assert.IsTrue(BigDecimal.Abs(expected - actual) < 0.00000_00000_01m, $"{nameof(expected)}={expected} {nameof(actual)}={actual}");
            }
        }
        [TestMethod()]
        public void AcosTest()
        {
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = System.Math.Acos(i);
                var actual = Math.Acos(i, 0.00000_00000_001);
                Assert.AreEqual(expected, actual, 0.00000_00000_01);
            }
            // オーバーフローするため計算回数が少ない→精度が悪い
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = (decimal)System.Math.Acos(i);
                var actual = Math.Acos((decimal)i, 0.00000_00000_001m, 22);
                Assert.AreEqual(expected, actual, 0.0001m);
            }
            BigDecimal.DefaultMinExponent = -100;
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = (BigDecimal)System.Math.Acos(i);
                var actual = Math.Acos((BigDecimal)i, 0.00000_00000_001m);
                Assert.IsTrue(BigDecimal.Abs(expected - actual) < 0.00000_00000_01m, $"{nameof(expected)}={expected} {nameof(actual)}={actual}");
            }
        }
        [TestMethod()]
        public void AtanTest()
        {
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = System.Math.Atan(i);
                var actual = Math.Atan(i, 0.00000_00000_001);
                Assert.AreEqual(expected, actual, 0.00000_00000_01);
            }
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = (decimal)System.Math.Atan(i);
                var actual = Math.Atan((decimal)i, 0.00000_00000_001m);
                Assert.AreEqual(expected, actual, 0.0001m);
            }
            BigDecimal.DefaultMinExponent = -100;
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = (BigDecimal)System.Math.Atan(i);
                var actual = Math.Atan((BigDecimal)i, 0.00000_00000_001m);
                Assert.IsTrue(BigDecimal.Abs(expected - actual) < 0.00000_00000_01m, $"{nameof(expected)}={expected} {nameof(actual)}={actual}");
            }
        }
        [TestMethod()]
        public void Atan2Test()
        {
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = System.Math.Atan2(i, i);
                var actual = Math.Atan2(i, i, 0.00000_00000_001);
                Assert.AreEqual(expected, actual, 0.00000_00000_01);
            }
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = (decimal)System.Math.Atan2(i, i);
                var actual = Math.Atan2((decimal)i, (decimal)i, 0.00000_00000_001m);
                Assert.AreEqual(expected, actual, 0.0001m);
            }
            BigDecimal.DefaultMinExponent = -100;
            var tolerance = (BigDecimal)0.00000_00000_001m;
            for (double i = -1; i <= 1; i += 0.125)
            {
                var expected = (BigDecimal)System.Math.Atan2(i, i);
                var actual = Math.Atan2((BigDecimal)i, (BigDecimal)i, tolerance);
                Assert.IsTrue(BigDecimal.Abs(expected - actual) < tolerance, $"{nameof(expected)}={expected} {nameof(actual)}={actual}");
            }
        }
    }
}