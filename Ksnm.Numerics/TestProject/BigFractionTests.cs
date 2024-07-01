using Ksnm.Numerics;
using System.Numerics;

namespace TestProject
{
    // コードを再利用するためのエイリアスを定義
    using Fraction = BigFraction;
    using Integer = BigInteger;
    using Int8 = sbyte;
    using UInt8 = byte;
    using Float16 = Half;
    using Float32 = float;
    using Float64 = double;
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
            // 1
            {
                BigFraction bigFraction = 1;
                Assert.AreEqual("1/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = (Float16)1 / (Float16)1;
                Assert.AreEqual("1/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 1.0f;
                Assert.AreEqual("1/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 1.0;
                Assert.AreEqual("1/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 1.0m;
                Assert.AreEqual("1/1", bigFraction.ToString());
            }
            // 1/2
            {
                BigFraction bigFraction = (Float16)1 / (Float16)2;
                Assert.AreEqual("1/2", bigFraction.ToString());
            }
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
            // 1/4
            {
                BigFraction bigFraction = (Float16)1 / (Float16)4;
                Assert.AreEqual("1/4", bigFraction.ToString());
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
            // 1/10
            {
                BigFraction bigFraction = (Float16)1 / (Float16)10;
                Assert.AreEqual("819/8192", bigFraction.ToString());
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
            // 256
            {
                BigFraction bigFraction = 256;
                Assert.AreEqual("256/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = (Float16)256;
                Assert.AreEqual("256/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 256.0f;
                Assert.AreEqual("256/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 256.0;
                Assert.AreEqual("256/1", bigFraction.ToString());
            }
            {
                BigFraction bigFraction = 256m;
                Assert.AreEqual("256/1", bigFraction.ToString());
            }
            // 4503599627370496
            {
                BigFraction bigFraction = 4503599627370496.0;
                Assert.AreEqual("4503599627370496/1", bigFraction.ToString());
            }
            // 9007199254740992
            {
                BigFraction bigFraction = 9007199254740992.0;
                Assert.AreEqual("9007199254740992/1", bigFraction.ToString());
            }
            // 18014398509481984
            {
                BigFraction bigFraction = 18014398509481984.0;
                Assert.AreEqual("18014398509481984/1", bigFraction.ToString());
            }
        }
    }
}
