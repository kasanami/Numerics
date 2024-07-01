using Ksnm.Numerics;

namespace TestProject
{
    [TestClass()]
    public class BigFractionTests
    {
        [TestMethod()]
        public void ConstantTest()
        {

        }
        [TestMethod()]
        public void ConstructorTest()
        {
            // 0
            {
                BigFraction bigFraction = new BigFraction();
                Assert.AreEqual("0/1", bigFraction.ToString());
            }
            // 1
            {
                BigFraction bigFraction = new BigFraction(1);
                Assert.AreEqual("1/1", bigFraction.ToString());
            }
            // 2
            {
                BigFraction bigFraction = new BigFraction(2);
                Assert.AreEqual("2/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = new BigFraction(2.0f);
                Assert.AreEqual("2/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = new BigFraction(2.0);
                Assert.AreEqual("2/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = new BigFraction(2.0m);
                Assert.AreEqual("2/1", bigFraction.ToString());
            }
            // 1/2
            {
                BigFraction bigFraction = new BigFraction(1, 2);
                Assert.AreEqual("1/2", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = new BigFraction(0.5f);
                Assert.AreEqual("1/2", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = new BigFraction(0.5);
                Assert.AreEqual("1/2", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = new BigFraction(0.5m);
                Assert.AreEqual("1/2", bigFraction.ToString());
            }
            // 非数
            {
                BigFraction bigFraction = new BigFraction(0, 0);
                Assert.AreEqual("0/0", bigFraction.ToString());
            }
        }
        [TestMethod()]
        public void CastTest()
        {
            {
                BigFraction bigFraction = 0.5f;
                Assert.AreEqual("1/2", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 0.5;
                Assert.AreEqual("1/2", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 0.5m;
                Assert.AreEqual("1/2", bigFraction.ToString());
            }

            {
                BigFraction bigFraction = 1.0f / 4;
                Assert.AreEqual("1/4", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 1.0 / 4;
                Assert.AreEqual("1/4", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 1.0m / 4;
                Assert.AreEqual("1/4", bigFraction.ToString());
            }

            {
                BigFraction bigFraction = 1.0f / 10;
                Assert.AreEqual("13421773/134217728", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 1.0 / 10;
                Assert.AreEqual("3602879701896397/36028797018963968", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 1.0m / 10;
                Assert.AreEqual("1/10", bigFraction.ToString());
            }
        }
    }
}
