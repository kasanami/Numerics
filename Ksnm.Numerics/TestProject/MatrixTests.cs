using Ksnm.Numerics;
using System.Numerics;

namespace TestProject
{
    // コードを再利用するためのエイリアスを定義
    using Int8 = sbyte;
    using UInt8 = byte;
    using Float16 = Half;
    using Float32 = float;
    using Float64 = double;
    [TestClass()]
    public class MatrixTests
    {
        [TestMethod()]
        public void ConstantTest()
        {
        }
        [TestMethod()]
        public void ConstructorTest()
        {
            Matrix<int> expected = new Matrix<int>(2, 2);
            Matrix<int> actual = new Matrix<int>(expected);
            Assert.AreEqual(expected, actual);

            expected[0, 0] = 1;
            actual[0, 0] = 1;
            Assert.AreEqual(actual, expected);

            expected[1, 1] = 1;
            actual[1, 1] = 1;
            Assert.AreEqual(actual, expected);
        }
        [TestMethod()]
        public void CastTest()
        {
        }

        [TestMethod()]
        public void OperationsTest1()
        {
        }

        [TestMethod()]
        public void OperationsTest2()
        {
            Matrix<int> expected = new Matrix<int>(2, 2);
            Matrix<int> actual = new Matrix<int>(expected);
            Matrix<int> a = new Matrix<int>(expected);
            Matrix<int> b = new Matrix<int>(expected);
            // +
            {
                expected[0, 0] = 34;
                expected[0, 1] = 68;
                expected[1, 0] = 136;
                expected[1, 1] = 272;

                a[0, 0] = 2;
                a[0, 1] = 4;
                a[1, 0] = 8;
                a[1, 1] = 16;

                b[0, 0] = 32;
                b[0, 1] = 64;
                b[1, 0] = 128;
                b[1, 1] = 256;
                actual = a + b;
                Assert.AreEqual(expected, actual);
            }
            // -
            {
                expected[0, 0] = 30;
                expected[0, 1] = 60;
                expected[1, 0] = 120;
                expected[1, 1] = 240;

                a[0, 0] = 2;
                a[0, 1] = 4;
                a[1, 0] = 8;
                a[1, 1] = 16;

                b[0, 0] = 32;
                b[0, 1] = 64;
                b[1, 0] = 128;
                b[1, 1] = 256;
                actual = b - a;
                Assert.AreEqual(expected, actual);
            }
            // *
            {
                expected[0, 0] = 32;
                expected[0, 1] = 51;
                expected[1, 0] = 46;
                expected[1, 1] = 75;

                a[0, 0] = 2;
                a[0, 1] = 5;
                a[1, 0] = 4;
                a[1, 1] = 7;

                b[0, 0] = 1;
                b[0, 1] = 3;
                b[1, 0] = 6;
                b[1, 1] = 9;
                actual = a * b;
                Assert.AreEqual(expected, actual);
            }
            // *
            {
                expected = new Matrix<int>(1, 2);
                actual = new Matrix<int>(expected);
                a = new Matrix<int>(1, 3);
                b = new Matrix<int>(3, 2);

                expected[0, 0] = 62;
                expected[0, 1] = 38;

                a[0, 0] = 2;
                a[0, 1] = 4;
                a[0, 2] = 6;

                b[0, 0] = 7;
                b[0, 1] = 5;
                b[1, 0] = 3;
                b[1, 1] = 4;
                b[2, 0] = 6;
                b[2, 1] = 2;
                actual = a * b;
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            for (int i = -10; i < 10; i++)
            {
                var a = new Matrix<int>(i, i);
                var b = new Matrix<int>(i, i);
                Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
            }
        }
    }
}
