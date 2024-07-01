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
                Int32 origin = 1;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/1", bigFraction.ToString());
                var other = (Int32)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float16 origin = (Float16)1 / (Float16)1;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/1", bigFraction.ToString());
                var other = (Float16)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float32 origin = 1;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/1", bigFraction.ToString());
                var other = (Float32)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float64 origin = 1;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/1", bigFraction.ToString());
                var other = (Float64)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Decimal origin = 1;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/1", bigFraction.ToString());
                var other = (Decimal)bigFraction;
                Assert.AreEqual(origin, other);
            }
            // 1/2
            {
                Float16 origin = (Float16)0.5f;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/2", bigFraction.ToString());
                var other = (Float16)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float32 origin = 0.5f;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/2", bigFraction.ToString());
                var other = (Float32)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float64 origin = 0.5;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/2", bigFraction.ToString());
                var other = (Float64)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Decimal origin = 0.5m;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/2", bigFraction.ToString());
                var other = (Decimal)bigFraction;
                Assert.AreEqual(origin, other);
            }
            // 1/4
            {
                Float16 origin = (Float16)1 / (Float16)4;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/4", bigFraction.ToString());
                var other = (Float16)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float32 origin = 1.0f / 4;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/4", bigFraction.ToString());
                var other = (Float32)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float64 origin = 1.0 / 4;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/4", bigFraction.ToString());
                var other = (Float64)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Decimal origin = 1.0m / 4;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/4", bigFraction.ToString());
                var other = (Decimal)bigFraction;
                Assert.AreEqual(origin, other);
            }
            // 1/10
            {
                Float16 origin = (Float16)1 / (Float16)10;
                BigFraction bigFraction = origin;
                Assert.AreEqual("819/8192", bigFraction.ToString());
                var other = (Float16)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float32 origin = 1.0f / 10;
                BigFraction bigFraction = origin;
                Assert.AreEqual("13421773/134217728", bigFraction.ToString());
                var other = (Float32)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float64 origin = 1.0 / 10;
                BigFraction bigFraction = origin;
                Assert.AreEqual("3602879701896397/36028797018963968", bigFraction.ToString());
                var other = (Float64)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Decimal origin = 1.0m / 10;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/10", bigFraction.ToString());
                var other = (Decimal)bigFraction;
                Assert.AreEqual(origin, other);
            }
            // 1 / 1_000_000_000_000
            {
                Float64 origin = 1.0 / 1_000_000_000_000;
                BigFraction bigFraction = origin;
                Assert.AreEqual("4951760157141521/4951760157141521099596496896", bigFraction.ToString());
                var other = (Float64)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Decimal origin = 1.0m / 1_000_000_000_000;
                BigFraction bigFraction = origin;
                Assert.AreEqual("1/1000000000000", bigFraction.ToString());
                var other = (Decimal)bigFraction;
                Assert.AreEqual(origin, other);
            }
            // 256
            {
                Int32 origin = 256;
                BigFraction bigFraction = origin;
                Assert.AreEqual("256/1", bigFraction.ToString());
                var other = (Int32)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float16 origin = (Float16)256;
                BigFraction bigFraction = origin;
                Assert.AreEqual("256/1", bigFraction.ToString());
                var other = (Float16)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float32 origin = 256;
                BigFraction bigFraction = origin;
                Assert.AreEqual("256/1", bigFraction.ToString());
                var other = (Float32)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Float64 origin = 256;
                BigFraction bigFraction = origin;
                Assert.AreEqual("256/1", bigFraction.ToString());
                var other = (Float64)bigFraction;
                Assert.AreEqual(origin, other);
            }
            {
                Decimal origin = 256m;
                BigFraction bigFraction = origin;
                Assert.AreEqual("256/1", bigFraction.ToString());
                var other = (Decimal)bigFraction;
                Assert.AreEqual(origin, other);
            }
            // 4503599627370496
            {
                Float64 origin = 4503599627370496.0;
                BigFraction bigFraction = origin;
                Assert.AreEqual("4503599627370496/1", bigFraction.ToString());
                var other = (Float64)bigFraction;
                Assert.AreEqual(origin, other);
            }
            // 9007199254740992
            {
                Float64 origin = 9007199254740992.0;
                BigFraction bigFraction = origin;
                Assert.AreEqual("9007199254740992/1", bigFraction.ToString());
                var other = (Float64)bigFraction;
                Assert.AreEqual(origin, other);
            }
            // 18014398509481984
            {
                Float64 origin = 18014398509481984.0;
                BigFraction bigFraction = origin;
                Assert.AreEqual("18014398509481984/1", bigFraction.ToString());
                var other = (Float64)bigFraction;
                Assert.AreEqual(origin, other);
            }
        }
    }
}
