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

        /// <summary>
        /// マチンの公式
        /// </summary>
        /// <param name="count">計算回数。1未満を設定すると0を返す。</param>
        /// <returns>PI/4(円周率の4分の1)</returns>
        public static T MachinsFormula<T>(int count) where T : INumber<T>, ISignedNumber<T>
        {
            T _1 = T.One;
            T _2 = T.CreateChecked(2);
            T _4 = T.CreateChecked(4);
            T _5 = T.CreateChecked(5);
            T _239 = T.CreateChecked(239);
            T sum = T.Zero;
            for (int k = 1; k <= count; k++)
            {
                var _k = T.CreateChecked(k);
                var odd = 2 * k - 1;
                var _odd = T.CreateChecked(odd);
                sum +=
                    _4 *
                    (Pow<T>(T.NegativeOne, k + 1) / _odd) *
                    Pow<T>(_1 / _5, odd) +
                    (Pow<T>(T.NegativeOne, k) / _odd) *
                    Pow<T>(_1 / _239, odd);
            }
            return sum;
        }
        /// <summary>
        /// べき乗
        /// </summary>
        /// <param name="radix">底</param>
        /// <param name="exponent">指数</param>
        /// <returns></returns>
        public static T Pow<T>(T radix, int exponent) where T : INumber<T>
        {
            T temp = T.One;
            if (exponent < 0)
            {
                exponent = -exponent;
                for (int i = 0; i < exponent; i++)
                {
                    temp /= radix;
                }
            }
            else if (exponent > 0)
            {
                for (int i = 0; i < exponent; i++)
                {
                    temp *= radix;
                }
            }
            return temp;
        }
    }
}
