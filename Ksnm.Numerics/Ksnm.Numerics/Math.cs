using System.Numerics;

namespace Ksnm.Numerics
{
    internal class Math
    {
        /// <summary>
        /// 最大公約数
        /// </summary>
        public static BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b)
        {
            if (a < b)
            {
                // 引数を入替えて自分を呼び出す
                return GreatestCommonDivisor(b, a);
            }
            while (b != 0)
            {
                var remainder = a % b;
                a = b;
                b = remainder;
            }
            return BigInteger.Abs(a);
        }

    }
}
