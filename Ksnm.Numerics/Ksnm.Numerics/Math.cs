using System.Numerics;

namespace Ksnm.Numerics
{
    public class Math
    {
        /// <summary>
        /// 最大公約数
        /// </summary>
        public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
        {
            if (a < b)
            {
                // 引数を入替えて自分を呼び出す
                return GreatestCommonDivisor(b, a);
            }
            while (T.IsZero(b) == false)
            {
                var remainder = a % b;
                a = b;
                b = remainder;
            }
            return T.Abs(a);
        }
        /// <summary>
        /// 指定された数値の平方根を返します。
        /// </summary>
        /// <param name="value">平方根を求める対象の数値。</param>
        /// <param name="count">計算回数</param>
        /// <returns>0 または value の平方根。</returns>
        public static T Sqrt<T>(T value, int count) where T : INumber<T>
        {
            if (T.IsZero(value))
            {
                return T.Zero;
            }
            var temp = value;
            for (int i = 0; i < count; i++)
            {
                temp = (temp * temp + value) / (temp + temp);
            }
            return temp;
        }

    }
}
