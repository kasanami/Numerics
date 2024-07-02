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
            Assert.AreEqual<Fraction>(1, Fraction.One);
            Assert.AreEqual<Fraction>(0, Fraction.Zero);
            Assert.AreEqual<Fraction>(2, Fraction.Radix);
            Assert.AreEqual<Fraction>(Fraction.One, Fraction.MultiplicativeIdentity);
            Assert.AreEqual<Fraction>(Fraction.Zero, Fraction.AdditiveIdentity);

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

        [TestMethod()]
        public void AbsTest()
        {
        }

        [TestMethod()]
        public void CompareToTest()
        {
            {
                Fraction a = new Fraction(1, 2);
                Fraction b = new Fraction(2, 4);
                Assert.AreEqual(0, a.CompareTo(b));
                Assert.AreEqual(0, b.CompareTo(a));
            }
            {
                Fraction a = new Fraction(1, -2);
                Fraction b = new Fraction(2, -4);
                Assert.AreEqual(0, a.CompareTo(b));
                Assert.AreEqual(0, b.CompareTo(a));
            }
            {
                Fraction a = new Fraction(1, 2);
                Fraction b = new Fraction(1, 3);
                Assert.AreEqual(+1, a.CompareTo(b));
                Assert.AreEqual(-1, b.CompareTo(a));
            }
            {
                Fraction a = new Fraction(1, -2);
                Fraction b = new Fraction(1, -3);
                Assert.AreEqual(-1, a.CompareTo(b));
                Assert.AreEqual(+1, b.CompareTo(a));
            }
        }

        [TestMethod()]
        public void IsEvenIntegerTest()
        {
        }
        [TestMethod()]
        public void IsFiniteTest()
        {
        }
        [TestMethod()]
        public void IsIntegerTest()
        {
        }
        [TestMethod()]
        public void IsNaNTest()
        {
        }
        [TestMethod()]
        public void IsNegativeTest()
        {
            // 分母が0ならfalse
            Fraction fraction = new Fraction(0, 0);
            Assert.IsFalse(Fraction.IsNegative(fraction));
            fraction = new Fraction(-0, 0);
            Assert.IsFalse(Fraction.IsNegative(fraction));
            fraction = new Fraction(-1, 0);
            Assert.IsFalse(Fraction.IsNegative(fraction));
            fraction = new Fraction(1, 0);
            Assert.IsFalse(Fraction.IsNegative(fraction));

            // 分母が正
            fraction = new Fraction(-1, 1);
            Assert.IsTrue(Fraction.IsNegative(fraction));
            fraction = new Fraction(-0, 1);// マイナスをつけても関係なし
            Assert.IsFalse(Fraction.IsNegative(fraction));
            fraction = new Fraction(0, 1);
            Assert.IsFalse(Fraction.IsNegative(fraction));
            fraction = new Fraction(1, 1);
            Assert.IsFalse(Fraction.IsNegative(fraction));
            // 分母が負
            fraction = new Fraction(1, -1);
            Assert.IsTrue(Fraction.IsNegative(fraction));
            fraction = new Fraction(0, -1);// ゼロは正
            Assert.IsFalse(Fraction.IsNegative(fraction));
            fraction = new Fraction(-1, -1);
            Assert.IsFalse(Fraction.IsNegative(fraction));
        }
        [TestMethod()]
        public void IsOddIntegerTest()
        {
        }
        [TestMethod()]
        public void IsPositiveTest()
        {
            // 分母が0ならfalse
            Fraction fraction = new Fraction(0, 0);
            Assert.IsFalse(Fraction.IsPositive(fraction));
            fraction = new Fraction(-0, 0);
            Assert.IsFalse(Fraction.IsPositive(fraction));
            fraction = new Fraction(-1, 0);
            Assert.IsFalse(Fraction.IsPositive(fraction));
            fraction = new Fraction(1, 0);
            Assert.IsFalse(Fraction.IsPositive(fraction));

            // 分母が正
            fraction = new Fraction(-1, 1);
            Assert.IsFalse(Fraction.IsPositive(fraction));
            fraction = new Fraction(0, 1);// ゼロは正
            Assert.IsTrue(Fraction.IsPositive(fraction));
            fraction = new Fraction(-0, 1);// マイナスをつけても関係なし
            Assert.IsTrue(Fraction.IsPositive(fraction));
            fraction = new Fraction(1, 1);
            Assert.IsTrue(Fraction.IsPositive(fraction));
            // 分母が負
            fraction = new Fraction(1, -1);
            Assert.IsFalse(Fraction.IsPositive(fraction));
            fraction = new Fraction(0, -1);// ゼロは正
            Assert.IsTrue(Fraction.IsPositive(fraction));
            fraction = new Fraction(-1, -1);
            Assert.IsTrue(Fraction.IsPositive(fraction));
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            for (int i = -10; i < 10; i++)
            {
                var a = new Fraction(i, i);
                var b = new Fraction(i, i);
                Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
            }
        }
    }
}
