using Ksnm.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

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
        public void OperatorTest()
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
            // -
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
            // *
            {
                BigDecimal a = 123;
                BigDecimal b = 100;
                BigDecimal c = a * b;
                Assert.AreEqual(12300, c);
            }
            {
                BigDecimal a = 123;
                BigDecimal b = -100;
                BigDecimal c = a * b;
                Assert.AreEqual(-12300, c);
            }
            // /
            {
                BigDecimal a = 123;
                BigDecimal b = 100;
                BigDecimal c = a / b;
                Assert.AreEqual(1.23m, c);
            }
            {
                BigDecimal a = 123;
                BigDecimal b = -100;
                BigDecimal c = a / b;
                Assert.AreEqual(-1.23m, c);
            }
            // %
            {
                BigDecimal a = 123;
                BigDecimal b = 100;
                BigDecimal c = a % b;
                Assert.AreEqual(23m, c);
            }
            {
                BigDecimal a = 123;
                BigDecimal b = -100;
                BigDecimal c = a % b;
                Assert.AreEqual(23m, c);
            }
            // 符号
            {
                BigDecimal a = 123;
                BigDecimal b = +a;
                Assert.AreEqual(123, b);
            }
            {
                BigDecimal a = 123;
                BigDecimal b = -a;
                Assert.AreEqual(-123, b);
            }
            // ++
            {
                BigDecimal a = 100;
                BigDecimal b = ++a;
                a = 100;
                BigDecimal c = a++;
                Assert.AreEqual(101, b);
                Assert.AreEqual(100, c);
            }
            // --
            {
                BigDecimal a = 100;
                BigDecimal b = --a;
                a = 100;
                BigDecimal c = a--;
                Assert.AreEqual(99, b);
                Assert.AreEqual(100, c);
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

        [TestMethod()]
        public void ParseTest2()
        {
            for (decimal source = -100; source <= 100; source += 0.5m)
            {
                var str = source.ToString();
                var sample = BigDecimal.Parse(str);
                Assert.AreEqual(source, sample, $"{source}");
            }
            {
                var source = "3.14159265358979323846264338327950288";
                var sample = BigDecimal.Parse(source);
                Assert.AreEqual(source, sample.ToString(), $"{source}");
                Assert.AreEqual(BigInteger.Parse("314159265358979323846264338327950288"), sample.Mantissa, $"{source}");
                Assert.AreEqual(-35, sample.Exponent, $"{source}");
                Assert.AreEqual(-35, sample.MinExponent, $"{source}");
            }
            {
                var source = "0.1234567890123456789012345678901234567890";
                var sample = BigDecimal.Parse(source);
                Assert.AreEqual(source, sample.ToString(), $"{source}");
                Assert.AreEqual(BigInteger.Parse("1234567890123456789012345678901234567890"), sample.Mantissa, $"{source}");
                Assert.AreEqual(-40, sample.Exponent, $"{source}");
                Assert.AreEqual(-40, sample.MinExponent, $"{source}");
            }
            {
                var source = "-0.123456789";
                var sample = BigDecimal.Parse(source);
                Assert.AreEqual(source, sample.ToString(), $"{source}");
                Assert.AreEqual(-123456789, sample.Mantissa, $"{source}");
                Assert.AreEqual(-9, sample.Exponent, $"{source}");
                Assert.AreEqual(-28, sample.MinExponent, $"{source}");
            }
        }

        [TestMethod()]
        public void TryParseTest2()
        {
            for (decimal source = -100; source <= 100; source += 0.5m)
            {
                var str = source.ToString();
                BigDecimal sample;
                var isSuccess = BigDecimal.TryParse(str, out sample);
                Assert.IsTrue(isSuccess, $"{source}");
                Assert.AreEqual(source, sample, $"{source}");
            }
            {
                var source = "3.14159265358979323846264338327950288";
                BigDecimal sample;
                var isSuccess = BigDecimal.TryParse(source, out sample);
                Assert.IsTrue(isSuccess, $"{source}");
                Assert.AreEqual(source, sample.ToString(), $"{source}");
                Assert.AreEqual(BigInteger.Parse("314159265358979323846264338327950288"), sample.Mantissa, $"{source}");
                Assert.AreEqual(-35, sample.Exponent, $"{source}");
                Assert.AreEqual(-35, sample.MinExponent, $"{source}");
            }
            {
                var source = "0.1234567890123456789012345678901234567890";
                BigDecimal sample;
                var isSuccess = BigDecimal.TryParse(source, out sample);
                Assert.IsTrue(isSuccess, $"{source}");
                Assert.AreEqual(source, sample.ToString(), $"{source}");
                Assert.AreEqual(BigInteger.Parse("1234567890123456789012345678901234567890"), sample.Mantissa, $"{source}");
                Assert.AreEqual(-40, sample.Exponent, $"{source}");
                Assert.AreEqual(-40, sample.MinExponent, $"{source}");
            }
            {
                var source = "-0.123456789";
                BigDecimal sample;
                var isSuccess = BigDecimal.TryParse(source, out sample);
                Assert.IsTrue(isSuccess, $"{source}");
                Assert.AreEqual(source, sample.ToString(), $"{source}");
                Assert.AreEqual(-123456789, sample.Mantissa, $"{source}");
                Assert.AreEqual(-9, sample.Exponent, $"{source}");
                Assert.AreEqual(-28, sample.MinExponent, $"{source}");
            }
        }


        [TestMethod()]
        public void CompareToTest()
        {
            {
                var sample1 = new BigDecimal(1, 1);
                var sample2 = new BigDecimal(2, 1);
                Assert.AreEqual(-1, sample1.CompareTo(sample2));
                Assert.AreEqual(+1, sample2.CompareTo(sample1));
            }
            for (decimal d1 = -1; d1 <= 1; d1 += 0.1m)
            {
                var sample1 = new BigDecimal(d1);
                for (decimal d2 = -1; d2 <= 1; d2 += 0.1m)
                {
                    var sample2 = new BigDecimal(d2);
                    Assert.AreEqual(d1.CompareTo(d2), sample1.CompareTo(sample2), $"{d1} {d2}");
                }
            }
            for (decimal d1 = -1; d1 <= 1; d1 += 0.1m)
            {
                var sample1 = new BigDecimal(d1);
                Assert.AreEqual(d1.CompareTo(null), sample1.CompareTo(null), $"{d1}");
            }
        }


        [TestMethod()]
        public void SortTest()
        {
            List<BigDecimal> sample = new List<BigDecimal>();
            for (int i = 0; i < 10; i++)
            {
                var num = Random.Shared.Next(100);
                sample.Add(num);
            }
            // ソート
            sample.Sort();
            // 大小関係チェック
            for (int i = 0; i < sample.Count - 1; i++)
            {
                Assert.IsTrue(sample[i] <= sample[i + 1]);
            }
        }

        [TestMethod()]
        public void SignTest()
        {
            BigDecimal sample = 1;
            Assert.AreEqual(+1, BigDecimal.Sign(sample));
            sample = -1;
            Assert.AreEqual(-1, BigDecimal.Sign(sample));
        }

        [TestMethod()]
        public void MathTest()
        {
            for (decimal i = -10; i <= 10; i += 0.1m)
            {
                var sample = new BigDecimal(i);
                // Abs
                Assert.AreEqual(decimal.Abs(i), BigDecimal.Abs(sample).ToDecimal(), $"Abs({i})");
                // Ceiling
                Assert.AreEqual(decimal.Ceiling(i), BigDecimal.Ceiling(sample).ToDecimal(), $"Ceiling({i})");
                // Floor
                Assert.AreEqual(decimal.Floor(i), BigDecimal.Floor(sample).ToDecimal(), $"Floor({i})");
                // Negate
                Assert.AreEqual(decimal.Negate(i), BigDecimal.Negate(sample).ToDecimal(), $"Negate({i})");
                // Truncate
                Assert.AreEqual(decimal.Truncate(i), BigDecimal.Truncate(sample).ToDecimal(), $"Negate({i})");

                for (decimal j = -10; j <= 10; j += 1m)
                {
                    var sample2 = new BigDecimal(j);
                    // Compare
                    Assert.AreEqual<BigDecimal>(decimal.Compare(i, j), BigDecimal.Compare(sample, sample2));
                    // Max
                    Assert.AreEqual<BigDecimal>(decimal.Max(i, j), BigDecimal.Max(sample, sample2));
                    // Min
                    Assert.AreEqual<BigDecimal>(decimal.Min(i, j), BigDecimal.Min(sample, sample2));
                }
            }

            {
                BigDecimal dividend = 123.456m;
                BigDecimal divisor = 10m;
                BigDecimal remainder;
                BigDecimal quotient = BigDecimal.DivRem(dividend, divisor, out remainder);
                Assert.AreEqual<BigDecimal>(12m, quotient, $"{dividend} / {divisor}");
                Assert.AreEqual<BigDecimal>(3.456m, remainder, $"{dividend} % {divisor}");
            }
            {
                BigDecimal dividend = 1.234m;
                BigDecimal divisor = 10m;
                BigDecimal remainder;
                BigDecimal quotient = BigDecimal.DivRem(dividend, divisor, out remainder);
                Assert.AreEqual<BigDecimal>(0m, quotient, $"{dividend} / {divisor}");
                Assert.AreEqual<BigDecimal>(1.234m, remainder, $"{dividend} % {divisor}");
            }
        }

    }
}