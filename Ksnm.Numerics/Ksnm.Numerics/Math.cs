using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

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
        static decimal Pow(decimal radix, decimal exponent, int terms)
        {
            decimal result = 1;
            decimal x = radix - 1;
            decimal term = 1;

            for (int i = 1; i <= terms; i++)
            {
                term *= exponent * x / i;
                exponent -= 1;
                result += term;
            }
            return result;
        }

        /// <summary>
        /// n番目の素数を計算する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n"></param>
        /// <returns></returns>
        public static BigInteger Prime(int n)
        {
            BigDecimal _n = n;
            BigDecimal _1 = 1;
            BigInteger sum = 0;
            var iMax = System.Math.Pow(2, n);
            for (var i = 1; i <= iMax; i++)
            {
                BigInteger sum2 = 0;
                for (var j = 1; j <= i; j++)
                {
                    BigDecimal fraction = (Factorial(j - 1) + 1) / j;
                    var cos = Math.Cos(fraction * BigDecimal.Pi);
                    sum2 += BigInteger.CreateChecked(BigDecimal.Floor(cos * cos));
                }
                sum += BigInteger.CreateChecked(BigDecimal.Floor(BigDecimal.Pow(_n / sum2, _1 / _n)));
            }
            return 1 + sum;
        }

        #region 階乗
        /// <summary>
        /// 階乗
        /// NOTE:21!以上は BigInteger でないと表現できないので BigInteger のみ用意
        /// </summary>
        /// <param name="value">階乗する整数</param>
        /// <returns>階乗した値</returns>
        public static BigInteger Factorial(BigInteger value)
        {
            BigInteger temp = 1;
            for (BigInteger i = value; i > 0; i--)
            {
                temp *= i;
            }
            return temp;
        }
        #endregion 階乗

        #region 三角関数
        /// <summary>
        /// 指定された角度のサインを返します。
        /// </summary>
        /// <param name="x">ラジアンで表した角度。</param>
        public static T Sin<T>(T x) where T : INumber<T>, IFloatingPointConstants<T>
        {
            // すべての組み合わせをテストしていないが、概ね25回目以降は結果が変わらない
            return Sin(x, 25);
        }
        /// <summary>
        /// 指定された角度のサインを返します。
        /// </summary>
        /// <param name="x">ラジアンで表した角度。</param>
        /// <param name="count">計算回数。</param>
        public static T Sin<T>(T x, int count) where T : INumber<T>, IFloatingPointConstants<T>
        {
            // -2π～2πにする
            x -= (x / (T.Tau)) * T.Tau;
            T sum = x;
            T t = x;
            for (int n = 1; n <= count; n++)
            {
                var d = (n + n + 1) * (n + n);
                t *= -(x * x) / T.CreateChecked(d);
                sum += t;
            }
            return sum;
        }
        /// <summary>
        /// 指定された角度のコサインを返します。
        /// </summary>
        /// <param name="x">ラジアンで表した角度。</param>
        public static T Cos<T>(T x) where T : INumber<T>, IFloatingPointConstants<T>
        {
            return Sin(T.Pi / T.CreateChecked(2) - x);
        }
        /// <summary>
        /// 指定された角度のタンジェントを返します。
        /// </summary>
        /// <param name="x">ラジアンで表した角度。</param>
        public static T Tan<T>(T x) where T : INumber<T>, IFloatingPointConstants<T>
        {
            return Sin(x) / Cos(x);
        }
        /// <summary>
        /// サインが指定数となる角度を返します。
        /// </summary>
        /// <param name="z">サインを表す数で、-1 以上 1 以下である必要があります。</param>
        /// <returns>-π/2 ≤θ≤π/2 の、ラジアンで表した角度 θ。</returns>
        public static T Asin<T>(T z) where T : INumber<T>, IFloatingPointConstants<T>
        {
            // Decimalの場合23を超えるとオーバーフローとなる。
            return Asin(z, 23);
        }
        /// <summary>
        /// サインが指定数となる角度を返します。
        /// </summary>
        /// <param name="x">サインを表す数で、-1 以上 1 以下である必要があります。</param>
        /// <param name="count">計算回数。</param>
        /// <returns>-π/2 ≤θ≤π/2 の、ラジアンで表した角度 θ。</returns>
        public static T Asin<T>(T x, int count) where T : INumber<T>, IFloatingPointConstants<T>
        {
            var _2 = _2<T>();
            if (x < -T.One || x > T.One)
            {
                throw new ArgumentOutOfRangeException($"{nameof(x)}={x} が範囲外の値です。");
            }
            if (x == T.One)
            {
                return T.Pi / _2;
            }
            if (x == -T.One)
            {
                return -T.Pi / _2;
            }
            T sum = x;// ループ一回目は省略
            for (int n = 1; n < count; n++)
            {
                var n2 = (2 * n + 1);
                sum += _Asin_<T>(n) * (Pow<T>(x, n2) / T.CreateChecked(n2));
            }
            return sum;
        }
        /// <summary>
        /// Asin で使用する内部用関数
        /// </summary>
        private static T _Asin_<T>(int count) where T : INumber<T>, IFloatingPointConstants<T>
        {
            T numerator = T.One;
            T denominator = T.One;
            count *= 2;// 2つずつ増やすので2倍
            for (int n = 1; n < count; n += 2)
            {
                numerator *= T.CreateChecked(n);
                denominator *= T.CreateChecked(n + 1);
            }
            return numerator / denominator;
        }
        /// <summary>
        /// コサインが指定数となる角度を返します。
        /// </summary>
        /// <param name="x">コサインを表す数で、-1 以上 1 以下である必要があります。</param>
        /// <returns>0 ≤θ≤π の、ラジアンで表した角度 θ。</returns>
        public static T Acos<T>(T x) where T : INumber<T>, IFloatingPointConstants<T>
        {
            return T.Pi / _2<T>() - Asin(x);
        }
        /// <summary>
        /// タンジェントが指定数となる角度を返します。
        /// </summary>
        /// <param name="x">タンジェントを表す数。</param>
        /// <returns>-π/2 ≤θ≤π/2 の、ラジアンで表した角度 θ。</returns>
        public static T Atan<T>(T x) where T : INumber<T>, IFloatingPointConstants<T>
        {
            // Decimalの場合
            // x=1のときで91回以上は結果がおなじになる。
            // x=1未満のときは90回より少ない回数で十分
            // x=が大きいと10000回でも結果が収束しない
            return Atan(x, 10000);
        }
        /// <summary>
        /// タンジェントが指定数となる角度を返します。
        /// </summary>
        /// <param name="x">タンジェントを表す数。</param>
        /// <param name="count">計算回数。</param>
        /// <returns>-π/2 ≤θ≤π/2 の、ラジアンで表した角度 θ。</returns>
        public static T Atan<T>(T x, int count) where T : INumber<T>, IFloatingPointConstants<T>
        {
#if false
            // TODO:x の値によっては、count=100では足りない。
            //if (x < -1 || x > 1)
            //{
            //    throw new ArgumentOutOfRangeException($"{nameof(x)}={x} が範囲外の値です。");
            //}
            decimal sum = x;// １周目は省略
            try
            {
                for (int n = 1; n < count; n++)
                {
                    var n2 = (2 * n + 1);
                    if (IsEven(n))
                    {
                        sum += Pow(x, n2) / n2;
                    }
                    else
                    {
                        sum -= Pow(x, n2) / n2;
                    }
                }
            }
            catch(OverflowException)
            {
                // オーバーフローが発生したらそこまでの計算結果を返す。
            }
            return sum;
#else
            var _1 = _1<T>();
            var _2 = _2<T>();
            // 過去の積を再利用する
            T product = _1;
            int k = 1;
            /// ｘ の2乗
            var x2 = (x * x);
            T sum = _1;// n=0のときは1なのでループ１回目は省略
            for (int n = 1; n < count; n++)
            {
#if false
                decimal product = 1;
                for (int k = 1; k <= n; k++)
                {
                    decimal multiplier = (2m * k * x2) / ((2m * k + 1) * (1 + x2));
                    product *= multiplier;
                }
#else
                if (k <= n)
                {
                    var _k = T.CreateChecked(k);
                    T multiplier = (_2 * _k * x2) / ((_2 * _k + _1) * (_1 + x2));
                    product *= multiplier;
                    k++;
                }
#endif
                sum += product;
            }
            var temp = x / (_1 + x * x);
            return temp * sum;
#endif
        }
        /// <summary>
        /// タンジェントが 2 つの指定された数の商である角度を返します。
        /// </summary>
        /// <param name="y">点の y 座標。</param>
        /// <param name="x">点の x 座標。</param>
        /// <returns>-π≤θ≤π および tan(θ) = y / x の、ラジアンで示した角度 θ。(x, y) は、デカルト座標の点を示します。
        /// 次の点に注意してください。
        /// クワドラント 1 の (x, y) の場合は、0 < θ < π/2。
        /// クワドラント 2 の (x, y) の場合は、π/2 < θ≤π。
        /// クワドラント 3 の (x, y) の場合は、-π < θ < -π/2。
        /// クワドラント 4 の (x, y) の場合は、-π/2 < θ < 0。
        /// クワドラント間の境界上にある点の場合は、次の戻り値になります。
        /// y が 0 で x が負数でない場合は、θ = 0。
        /// y が 0 で x が負の場合は、θ = π。
        /// y が正で x が 0 の場合は、θ = π/2。
        /// y が負数で x が 0 の場合は、θ = -π/2。
        /// y が 0 かつ x が 0 の場合は、θ = 0。</returns>
        public static T Atan2<T>(T y, T x) where T : INumber<T>, IFloatingPointConstants<T>
        {
            if (x > T.Zero)
            {
                return Atan(y / x);
            }
            else if (x < T.Zero)
            {
                if (y > T.Zero)
                {
                    return Atan(y / x) + T.Pi;
                }
                else
                {
                    return Atan(y / x) - T.Pi;
                }
            }
            else// x==0
            {
                if (y > T.Zero)
                {
                    return +(T.Pi / _2<T>());
                }
                else if (y < T.Zero)
                {
                    return -(T.Pi / _2<T>());
                }
                // y==0
                return T.Zero;
            }
        }
        #endregion 三角関数

        #region CreateCheckedの短縮
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T _1<T>() where T : INumber<T> => T.CreateChecked(1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T _2<T>() where T : INumber<T> => T.CreateChecked(2);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T _3<T>() where T : INumber<T> => T.CreateChecked(3);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T _4<T>() where T : INumber<T> => T.CreateChecked(4);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T _5<T>() where T : INumber<T> => T.CreateChecked(5);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T _6<T>() where T : INumber<T> => T.CreateChecked(6);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T _7<T>() where T : INumber<T> => T.CreateChecked(7);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T _8<T>() where T : INumber<T> => T.CreateChecked(8);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T _9<T>() where T : INumber<T> => T.CreateChecked(9);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T _10<T>() where T : INumber<T> => T.CreateChecked(10);
        #endregion CreateCheckedの短縮
    }
}
