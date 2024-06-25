using Ksnm.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass()]
    public class BigDecimalTests
    {
        [TestMethod()]
        public void ConstantTest()
        {
            Assert.AreEqual(10, BigDecimal.Radix);
            Assert.AreEqual(0, BigDecimal.Zero);
            Assert.AreEqual(1, BigDecimal.One);
            Assert.AreEqual(-1, BigDecimal.MinusOne);
            Assert.AreEqual(0.5m, (decimal)BigDecimal.ZeroPointFive);
        }
        [TestMethod()]
        public void AdditionTest()
        {
            {
                BigDecimal a = 123;
                BigDecimal b = 111;
                BigDecimal c = a + b;
                Assert.AreEqual(234, c);
            }
            {
                BigDecimal a = decimal.MaxValue;
                BigDecimal b = decimal.MaxValue;
                BigDecimal c = a + b;
                BigDecimal d = BigDecimal.Parse("158456325028528675187087900670");
                Assert.AreEqual(d, c);
            }
            {
                BigDecimal a = decimal.MaxValue;
                BigDecimal b = decimal.MinValue;
                BigDecimal c = a + b;
                Assert.AreEqual(0, c);
            }
        }
        [TestMethod()]
        public void SubtractionTest()
        {
            {
                BigDecimal a = 123;
                BigDecimal b = 111;
                BigDecimal c = a - b;
                Assert.AreEqual(12, c);
            }
            {
                BigDecimal a = decimal.MaxValue;
                BigDecimal b = decimal.MaxValue;
                BigDecimal c = a - b;
                Assert.AreEqual(0, c);
            }
        }
        [TestMethod()]
        public void ParseTest()
        {
            Assert.AreEqual(1, BigDecimal.Parse("1"));
            Assert.AreEqual(-1, BigDecimal.Parse("-1"));
            Assert.AreEqual(10000000000, BigDecimal.Parse("10000000000"));
            Assert.AreEqual(0.0000000001m, BigDecimal.Parse("0.0000000001"));
        }
    }
}