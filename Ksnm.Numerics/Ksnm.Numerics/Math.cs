using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Ksnm.Numerics;
using Microsoft.VisualBasic;

namespace Ksnm
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
        /// <param name="tolerance">許容値</param>
        /// <param name="terms">計算回数。1未満を設定すると0を返す。</param>
        /// <returns>PI/4(円周率の4分の1)</returns>
        public static T MachinsFormula<T>(T tolerance, int terms = 1000)
            where T : INumber<T>, ISignedNumber<T>
        {
            T _1 = T.One;
            T _2 = T.CreateChecked(2);
            T _4 = T.CreateChecked(4);
            T _5 = T.CreateChecked(5);
            T _239 = T.CreateChecked(239);
            T sum = T.Zero;
            T before = T.Zero;
            for (int k = 1; k <= terms; k++)
            {
                var _k = T.CreateChecked(k);
                var odd = 2 * k - 1;
                var _odd = T.CreateChecked(odd);
                var add =
                    _4 *
                    (Pow<T>(T.NegativeOne, k + 1) / _odd) *
                    Pow<T>(_1 / _5, odd) +
                    (Pow<T>(T.NegativeOne, k) / _odd) *
                    Pow<T>(_1 / _239, odd);
                sum += add;
                if (T.Abs(add) < tolerance)
                {
                    break;
                }
            }
            return sum;
        }
        /// <summary>
        /// べき乗
        /// </summary>
        /// <param name="baseValue">底</param>
        /// <param name="exponent">指数</param>
        /// <returns></returns>
        public static T Pow<T>(T baseValue, int exponent) where T : INumber<T>
        {
            T temp = T.One;
            if (exponent < 0)
            {
                exponent = -exponent;
                for (int i = 0; i < exponent; i++)
                {
                    temp /= baseValue;
                }
            }
            else if (exponent > 0)
            {
                for (int i = 0; i < exponent; i++)
                {
                    temp *= baseValue;
                }
            }
            return temp;
        }
        /// <summary>
        /// べき乗
        /// </summary>
        /// <param name="baseValue">底</param>
        /// <param name="exponent">指数</param>
        /// <param name="tolerance">許容値</param>
        /// <param name="terms">計算回数</param>
        /// <returns></returns>
        public static T Pow<T>(T baseValue, T exponent, T tolerance, int terms = 1000)
            where T : INumber<T>
        {
#if false
            // 誤差が大きいのでボツ
            var log = Log(baseValue);
            var intermediate = exponent * log;
            return Exp(intermediate);
#else
            // 冪級数展開

            // baseValue が2より大きい場合は、1 に近くなるように変換
            var _2 = T.CreateChecked(2);
            if (baseValue > _2)
            {
                T factor = T.Zero;
                while (baseValue > _2)
                {
                    baseValue /= _2;
                    factor++;
                }
                T partialResult = Pow(baseValue, exponent, tolerance, terms);
                return Pow(_2, factor * exponent, tolerance) * partialResult;
            }
            // baseValue を 1 + x に変換
            T x = baseValue - T.One;
            T result = T.One;
            T term = T.One;
            for (int n = 1; n <= terms; n++)
            {
                term *= (exponent - T.CreateChecked(n - 1)) / T.CreateChecked(n) * x;
                result += term;
                if (T.Abs(term) < tolerance)
                {
                    break;
                }
            }
            return result;
#endif
        }

        public static T Exp<T>(T value) where T : INumber<T>
        {
            //  x^0/0! + x^1/1! + x^2/2! + x^3/3! + ... + x^n/n! 
            T result = T.One;// x^0/0!は最初から設定
            T term = T.One;
            // 毎回べき乗と階乗をするのではなく、前回の値にかける。
            for (int i = 1; i < 100; i++)
            {
                term *= value / T.CreateChecked(i);
                result += term;
                if (term <= T.Zero)
                {
                    break;
                }
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
            BigDecimal tolerance = new BigDecimal(1, -30);
            var iMax = System.Math.Pow(2, n);
            for (var i = 1; i <= iMax; i++)
            {
                BigInteger sum2 = 0;
                for (var j = 1; j <= i; j++)
                {
                    BigDecimal fraction = (Factorial(j - 1) + 1) / (BigDecimal)j;
                    var cos = Math.Cos(fraction * BigDecimal.Pi, tolerance);
                    sum2 += BigDecimal.Floor(cos * cos).ToBigInteger();
                }
                sum += BigDecimal.Floor(BigDecimal.Pow(_n / sum2, _1 / _n)).ToBigInteger();
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

        #region 対数
        /// <summary>
        /// 自然対数
        /// </summary>
        /// <param name="value">対数を求める値</param>
        /// <param name="terms">計算回数</param>
        public static T Log<T>(T value, int terms = 1000) where T : INumber<T>
        {
            return TaylorLog(value, terms);
        }
        /// <summary>
        /// テイラー級数展開を使った実装
        /// </summary>
        /// <param name="value">対数を求める値</param>
        /// <param name="terms">計算回数</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T TaylorLog<T>(T value, int terms = 1000) where T : INumber<T>
        {
            if (value <= T.Zero)
            {
                throw new ArgumentOutOfRangeException($"{nameof(value)}は正でなければならない");
            }
            if (value == T.One)
            {
                return T.Zero;
            }
            //  0 < x < 2 の範囲にする
            T _2 = T.CreateChecked(2);
            if (value > _2)
            {
                return TaylorLog(value / _2, terms) + TaylorLog(_2, terms);
            }

            T result = T.Zero;
            T y = value - T.One;
            T power = y;
            for (int n = 1; n <= terms; n++)
            {
                if (n % 2 == 0)
                {
                    result -= power / T.CreateChecked(n);
                }
                else
                {
                    result += power / T.CreateChecked(n);
                }
                power *= y;
            }
            return result;
        }
        /// <summary>
        /// ニュートン・ラフソン法を使った実装
        /// </summary>
        /// <param name="value">対数を求める値</param>
        /// <param name="tolerance">許容値</param>
        /// <param name="terms">計算回数</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
        public static T NewtonRaphsonLog<T>(T value, T tolerance, int terms = 100) where T : INumber<T>
        {
            if (value <= T.Zero)
            {
                throw new ArgumentOutOfRangeException($"{nameof(value)}は正でなければならない");
            }
            if (value == T.One)
            {
                return T.Zero;
            }
            // 最初の推論
            T result = value - T.One > T.One ? value - T.One : value;
            T before = T.Zero;
            for (int i = 1; i < terms; i++)
            {
                T exp = Exp(result);
                result = result - (exp - value) / exp;
                // 差が許容値を下回ったら終了
                if (T.Abs(before - result) < tolerance)
                {
                    return result;
                }
                before = result;
            }
            // 収束しなかった
            throw new Exception("Newton-Raphson method did not converge");
        }
        #endregion 対数

        #region 三角関数
        /// <summary>
        /// 指定された角度のサインを返します。
        /// </summary>
        /// <param name="angle">ラジアンで表した角度</param>
        /// <param name="tolerance">許容値</param>
        /// <param name="terms">計算回数</param>
        public static T Sin<T>(T angle, T tolerance, int terms = 1000)
            where T : INumber<T>, IFloatingPointConstants<T>, IFloatingPoint<T>
        {
            // -2π～2πにする
            if (angle < T.Tau || angle > T.Tau)
            {
                angle -= T.Floor(angle / (T.Tau)) * T.Tau;
            }
            T sum = angle;
            T add = angle;
            for (int n = 1; n <= terms; n++)
            {
                var d = (n + n + 1) * (n + n);
                add *= -(angle * angle) / T.CreateChecked(d);
                sum += add;
                if (T.Abs(add) < tolerance)
                {
                    break;
                }
            }
            return sum;
        }
        /// <summary>
        /// 指定された角度のコサインを返します。
        /// </summary>
        /// <param name="angle">ラジアンで表した角度。</param>
        /// <param name="tolerance">許容値</param>
        /// <param name="terms">計算回数</param>
        public static T Cos<T>(T angle, T tolerance, int terms = 1000)
            where T : INumber<T>, IFloatingPointConstants<T>, IFloatingPoint<T>
        {
            return Sin(T.Pi / T.CreateChecked(2) - angle, tolerance, terms);
        }
        /// <summary>
        /// 指定された角度のタンジェントを返します。
        /// </summary>
        /// <param name="angle">ラジアンで表した角度。</param>
        public static T Tan<T>(T angle, T tolerance, int terms = 1000)
            where T : INumber<T>, IFloatingPointConstants<T>, IFloatingPoint<T>
        {
            return Sin(angle, tolerance, terms) / Cos(angle, tolerance, terms);
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
            var _2 = T.CreateChecked(2);
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
            var _2 = T.CreateChecked(2);
            return T.Pi / _2 - Asin(x);
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
            var _1 = T.One;
            var _2 = T.CreateChecked(2);
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
                var _2 = T.CreateChecked(2);
                if (y > T.Zero)
                {
                    return +(T.Pi / _2);
                }
                else if (y < T.Zero)
                {
                    return -(T.Pi / _2);
                }
                // y==0
                return T.Zero;
            }
        }
        #endregion 三角関数
    }
}
