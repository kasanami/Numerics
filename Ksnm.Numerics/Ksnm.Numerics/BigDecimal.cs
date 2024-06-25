using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;

namespace Ksnm.Numerics
{
    /// <summary>
    /// 任意精度の符号付き10進数
    /// </summary>
    public struct BigDecimal : INumber<BigDecimal>
    {
        #region 定数

        /// <summary>
        /// 十進数の底（てい）
        /// </summary>
        public static int Radix { get; } = 10;

        /// <summary>
        /// 数値 0 を表します。
        /// </summary>
        public static BigDecimal Zero => 0;
        /// <summary>
        /// 数値 0.5 を表します。
        /// </summary>
        public static BigDecimal ZeroPointFive => 0.5m;
        /// <summary>
        /// 数値 1 を表します。
        /// </summary>
        public static BigDecimal One => 1;
        /// <summary>
        /// 負の 1 (-1) を表します。
        /// </summary>
        public static BigDecimal MinusOne => -1;

        /// <summary>
        /// MinExponentの初期値
        /// </summary>
        public const int DefaultMinExponent = DecimalMinExponent;
        /// <summary>
        /// System.Decimal の指数の最小値
        /// ※System.Decimal 内では正数で保持しているが、この値は指数のため負の値とする。
        /// </summary>
        private const int DecimalMinExponent = -28;
        /// <summary>
        /// 任意の計算結果が Decimal で同様の計算をした結果と一致しないので、より高精度にするための追加指数。
        /// </summary>
        private const int AddExponent = -3;
        /// <summary>
        /// デフォルトの丸め処理方法
        /// </summary>
        public const MidpointRounding DefaultMidpointRounding = MidpointRounding.ToEven;
        #endregion 定数

        #region フィールド
        #endregion フィールド

        #region プロパティ
        /// <summary>
        /// 指数部
        /// </summary>
        public int Exponent { get; private set; }
        /// <summary>
        /// 仮数部
        /// </summary>
        public BigInteger Mantissa { get; private set; }
        /// <summary>
        /// 指数部の最小値
        /// <para>無限小数の場合にこの桁数で丸める</para>
        /// <para>精度とも言える</para>
        /// </summary>
        public int MinExponent { get; private set; }
        #endregion プロパティ

        #region コンストラクタ
        /// <summary>
        /// 指定した値で初期化
        /// </summary>
        /// <param name="other">コピー元</param>
        public BigDecimal(BigDecimal other)
        {
            Exponent = other.Exponent;
            Mantissa = other.Mantissa;
            MinExponent = other.MinExponent;
        }
        /// <summary>
        /// 指定した値で初期化
        /// </summary>
        /// <param name="integer">整数</param>
        public BigDecimal(BigInteger integer) : this(integer, 0, DefaultMinExponent)
        {
        }
        /// <summary>
        /// 指定した値で初期化
        /// * exponent と DefaultMinExponent どちらか小さい方を、MinExponent に設定します。
        /// </summary>
        /// <param name="mantissa">仮数部</param>
        /// <param name="exponent">指数部</param>
        public BigDecimal(BigInteger mantissa, int exponent) : this(
            mantissa, exponent, System.Math.Min(exponent, DefaultMinExponent))
        {
        }
        /// <summary>
        /// 指定した値で初期化
        /// ※exponent が minExponent より小さい場合は、exponent を最小値とします。
        /// </summary>
        /// <param name="mantissa">仮数部</param>
        /// <param name="exponent">指数部</param>
        /// <param name="minExponent">指数部の最小値</param>
        public BigDecimal(BigInteger mantissa, int exponent, int minExponent)
        {
            Mantissa = mantissa;
            Exponent = exponent;
            MinExponent = System.Math.Min(exponent, minExponent);
        }
        /// <summary>
        /// 指定した値で初期化
        /// </summary>
        public BigDecimal(int value)
        {
            Exponent = 0;
            Mantissa = value;
            MinExponent = DefaultMinExponent;
        }
        /// <summary>
        /// 指定した値で初期化
        /// </summary>
        public BigDecimal(uint value)
        {
            Exponent = 0;
            Mantissa = value;
            MinExponent = DefaultMinExponent;
        }
        /// <summary>
        /// 指定した値で初期化
        /// </summary>
        public BigDecimal(long value)
        {
            Exponent = 0;
            Mantissa = value;
            MinExponent = DefaultMinExponent;
        }
        /// <summary>
        /// 指定した値で初期化
        /// </summary>
        public BigDecimal(ulong value)
        {
            Exponent = 0;
            Mantissa = value;
            MinExponent = DefaultMinExponent;
        }
        /// <summary>
        /// 指定した値で初期化
        /// </summary>
        public BigDecimal(decimal value)
        {
            Exponent = GetExponent(value);
            Mantissa = (BigInteger)GetMantissa(value);
            Mantissa *= GetSign(value);
            MinExponent = DefaultMinExponent;
        }
        /// <summary>
        /// 指定した値で初期化
        /// </summary>
        public BigDecimal(double value)
        {
            var temp = value.ToString(DecimalFormat);
            this = Parse(temp);
        }
        #endregion コンストラクタ

        #region 独自メソッド
        /// <summary>
        /// Exponent が最小になるように変換します。
        /// Mantissa は大きくなります。
        /// </summary>
        public void MinimizeExponent()
        {
            _MinimizeExponent(MinExponent);
        }
        /// <summary>
        /// 指定した Exponent を設定する。
        /// * minExponent が現在の値より大きい場合は何もしません
        /// * この関数では MinExponent より小さい値を設定してもエラーにしない
        /// </summary>
        /// <param name="newExponent">設定する Exponent</param>
        private void _MinimizeExponent(int newExponent)
        {
            if (Exponent > newExponent)
            {
                var diff = Exponent - newExponent;
                Exponent = newExponent;
                Mantissa *= Pow10(diff);
            }
        }
        /// <summary>
        /// Mantissa が最小になるように変換します。
        /// * Exponent が大きくなります。
        /// * Mantissa が 0 の場合は、Exponent は 0 になります。
        /// </summary>
        public void MinimizeMantissa()
        {
            if (Mantissa == 0)
            {
                Exponent = 0;
                return;
            }
            // 10^eで割り切れる値の最大
            int maxExponent = MaxExponent(Mantissa);
            // プロパティを更新
            if (maxExponent > 0)
            {
                Exponent += maxExponent;
                Mantissa /= Pow10(maxExponent);
                Assert(Exponent > MinExponent);
            }
        }
        /// <summary>
        /// 底を 10 とする value の割り切れる最大の指数
        /// * 100 の場合は 2 を返す。
        /// * 120 の場合は 1 を返す。
        /// * 123 の場合は 0 を返す。
        /// * 0 の場合は 0 を返す。
        /// </summary>
        /// <param name="value">調査する値</param>
        /// <returns>底を 10 とする指数</returns>
        public static int MaxExponent(BigInteger value)
        {
            value = BigInteger.Abs(value);
            // 10^eで割り切れる値の最大
            int maxExponent = 0;
            for (int e = 1; e < int.MaxValue; e++)
            {
                var divisor = Pow10(e);
                // divisor のほうが大きいなら終了
                if (value < divisor)
                {
                    break;
                }
                // 10^eで割り切れるか
                if (value % divisor == 0)
                {
                    // 更新して次へ
                    maxExponent = e;
                }
                else
                {
                    // 終了
                    break;
                }
            }
            return maxExponent;
        }
        /// <summary>
        /// 2 つの値を比較します。
        /// </summary>
        /// <param name="d1">比較する最初の値です。</param>
        /// <param name="d2">比較する 2 番目の値です。</param>
        /// <returns>
        /// -1：d1 は d2 より小さい。
        /// 0 ：d1 と d2 が等しい。 
        /// +1：d1 が d2 より大きい。</returns>
        public static int Compare(BigDecimal d1, BigDecimal d2)
        {
            _ConvertSameExponent(ref d1, ref d2);
            if (d1.Mantissa > d2.Mantissa)
            {
                return 1;
            }
            else if (d1.Mantissa < d2.Mantissa)
            {
                return -1;
            }
            return 0;
        }
        /// <summary>
        /// 指定された 2 つのインスタンスが同じ値を表しているかどうかを示す値を返します。
        /// </summary>
        /// <param name="d1">比較する最初の値です。</param>
        /// <param name="d2">比較する 2 番目の値です。</param>
        /// <returns>d1 と d2 が等しい場合は true。それ以外の場合は false。</returns>
        public static bool Equals(BigDecimal d1, BigDecimal d2)
        {
            UniformExponent(ref d1, ref d2);
            return d1.Mantissa == d2.Mantissa;
        }
        /// <summary>
        /// 数値の文字列形式を、それと等価の BigDecimal に変換します。
        /// </summary>
        /// <param name="value">変換する数値の文字列形式。</param>
        /// <returns>指定されている数値と等価の値。</returns>
        public static BigDecimal Parse(string value)
        {
            var temp = Zero;
            var pointIindex = value.IndexOf('.');
            if (pointIindex < 0)
            {
                temp.Mantissa = BigInteger.Parse(value);
            }
            else
            {
                value = value.Remove(pointIindex, 1);
                temp.Mantissa = BigInteger.Parse(value);
                temp.Exponent = -(value.Length - pointIindex);
                if (temp.MinExponent > temp.Exponent)
                {
                    temp.MinExponent = temp.Exponent;
                }
            }
            return temp;
        }
        /// <summary>
        /// 2つの BigDecimal の Exponent を揃える
        /// </summary>
        public static void UniformExponent(ref BigDecimal d1, ref BigDecimal d2)
        {
            if (d1.Exponent > d2.Exponent)
            {
                var diff = d1.Exponent - d2.Exponent;
                d1.Mantissa *= Pow10(diff);
                d1.Exponent -= diff;
            }
            else if (d1.Exponent < d2.Exponent)
            {
                var diff = d2.Exponent - d1.Exponent;
                d2.Mantissa *= Pow10(diff);
                d2.Exponent -= diff;
            }
            Assert(d1.Exponent == d2.Exponent);
        }
        #region Get*
        /// <summary>
        /// 小数部を取得
        /// </summary>
        public BigDecimal GetFractional()
        {
            return this % 1;
        }
        #endregion Get*
        #region　Is*
        public bool IsNegative()
        {
            return Mantissa < 0;
        }
        public bool IsPositive()
        {
            return Mantissa > 0;
        }
        public bool IsZero()
        {
            return Mantissa == 0;
        }
        #endregion　Is*
        #endregion 独自メソッド

        #region 型変換
        #region 他の型→BigDecimal
        /// <summary>
        /// byte から BigDecimal への暗黙的な変換を定義します。
        /// </summary>
        public static implicit operator BigDecimal(byte value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// sbyte から BigDecimal への暗黙的な変換を定義します。
        /// </summary>
        public static implicit operator BigDecimal(sbyte value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// short から BigDecimal への暗黙的な変換を定義します。
        /// </summary>
        public static implicit operator BigDecimal(short value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// ushort から BigDecimal への暗黙的な変換を定義します。
        /// </summary>
        public static implicit operator BigDecimal(ushort value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// int から BigDecimal への暗黙的な変換を定義します。
        /// </summary>
        public static implicit operator BigDecimal(int value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// uint から BigDecimal への暗黙的な変換を定義します。
        /// </summary>
        public static implicit operator BigDecimal(uint value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// long から BigDecimal への暗黙的な変換を定義します。
        /// </summary>
        public static implicit operator BigDecimal(long value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// ulong から BigDecimal への暗黙的な変換を定義します。
        /// </summary>
        public static implicit operator BigDecimal(ulong value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// float から BigDecimal への明示的な変換を定義します。
        /// </summary>
        public static explicit operator BigDecimal(float value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// double から BigDecimal への明示的な変換を定義します。
        /// </summary>
        public static explicit operator BigDecimal(double value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// decimal から BigDecimal への暗黙的な変換を定義します。
        /// </summary>
        public static implicit operator BigDecimal(decimal value)
        {
            return new BigDecimal(value);
        }
        /// <summary>
        /// BigInteger から BigDecimal への暗黙的な変換を定義します。
        /// </summary>
        public static implicit operator BigDecimal(BigInteger value)
        {
            return new BigDecimal(value);
        }
        #endregion 他の型→BigDecimal
        #region BigDecimal→他の型
        /// <summary>
        /// BigDecimal から byte への明示的な変換を定義します。
        /// </summary>
        public static explicit operator byte(BigDecimal value)
        {
            return (byte)value.ToBigInteger();
        }
        /// <summary>
        /// BigDecimal から sbyte への明示的な変換を定義します。
        /// </summary>
        public static explicit operator sbyte(BigDecimal value)
        {
            return (sbyte)value.ToBigInteger();
        }
        /// <summary>
        /// BigDecimal から short への明示的な変換を定義します。
        /// </summary>
        public static explicit operator short(BigDecimal value)
        {
            return (short)value.ToBigInteger();
        }
        /// <summary>
        /// BigDecimal から ushort への明示的な変換を定義します。
        /// </summary>
        public static explicit operator ushort(BigDecimal value)
        {
            return (ushort)value.ToBigInteger();
        }
        /// <summary>
        /// BigDecimal から int への明示的な変換を定義します。
        /// </summary>
        public static explicit operator int(BigDecimal value)
        {
            return value.ToInt32();
        }
        /// <summary>
        /// BigDecimal から uint への明示的な変換を定義します。
        /// </summary>
        public static explicit operator uint(BigDecimal value)
        {
            return value.ToUInt32();
        }
        /// <summary>
        /// BigDecimal から long への明示的な変換を定義します。
        /// </summary>
        public static explicit operator long(BigDecimal value)
        {
            return value.ToInt64();
        }
        /// <summary>
        /// BigDecimal から ulong への明示的な変換を定義します。
        /// </summary>
        public static explicit operator ulong(BigDecimal value)
        {
            return value.ToUInt64();
        }
        /// <summary>
        /// BigDecimal から float への明示的な変換を定義します。
        /// </summary>
        public static explicit operator float(BigDecimal value)
        {
            return (float)value.ToDecimal();
        }
        /// <summary>
        /// BigDecimal から double への明示的な変換を定義します。
        /// </summary>
        public static explicit operator double(BigDecimal value)
        {
            return (double)value.ToDecimal();
        }
        /// <summary>
        /// BigDecimal から decimal への明示的な変換を定義します。
        /// </summary>
        public static explicit operator decimal(BigDecimal value)
        {
            return value.ToDecimal();
        }
        /// <summary>
        /// BigDecimal から BigInteger への明示的な変換を定義します。
        /// </summary>
        public static explicit operator BigInteger(BigDecimal value)
        {
            return value.ToBigInteger();
        }
        #endregion BigDecimal→他の型

        #region To*
        /// <summary>
        /// int へ変換します。
        /// * 小数点以下は切り捨て
        /// </summary>
        public int ToInt32()
        {
            return (int)ToBigInteger();
        }
        /// <summary>
        /// uint へ変換します。
        /// * 小数点以下は切り捨て
        /// </summary>
        public uint ToUInt32()
        {
            return (uint)ToBigInteger();
        }
        /// <summary>
        /// long へ変換します。
        /// * 小数点以下は切り捨て
        /// </summary>
        public long ToInt64()
        {
            return (long)ToBigInteger();
        }
        /// <summary>
        /// ulong へ変換します。
        /// * 小数点以下は切り捨て
        /// </summary>
        public ulong ToUInt64()
        {
            return (ulong)ToBigInteger();
        }
        /// <summary>
        /// decimal へ変換します。
        /// </summary>
        public decimal ToDecimal()
        {
            // mantissa は正の数にする
            var mantissa = BigInteger.Abs(Mantissa);
            byte scale = 0;
            // decimal は正の Exponent に対応していないので、mantissa を変換
            if (Exponent > 0)
            {
                mantissa *= Pow10(Exponent);
            }
            else if (Exponent < 0)
            {
                var exponent = Exponent;
                // Decimal より精度が高い場合、丸める
                if (exponent < DecimalMinExponent)
                {
                    var diff = DecimalMinExponent - exponent;
                    mantissa = Round(mantissa, diff, MidpointRounding.ToEven);
                    exponent = DecimalMinExponent;
                }
                scale = (byte)(-exponent);
            }
            // [0]=最上位
            var bytes = mantissa.ToByteArray().ToList();
            if (bytes.Count > (4 * 3))
            {
                //return decimal.MaxValue;
                throw new OverflowException($"{nameof(Mantissa)}={Mantissa}");
                //throw new InvalidCastException($"{nameof(Mantissa)}({Mantissa})が decimal の最大値より大きい");
            }
            while (bytes.Count < (4 * 3))
            {
                bytes.Add(0);
            }
            var bytes2 = bytes.ToArray();
            int lo = BitConverter.ToInt32(bytes2, 0);
            int mid = BitConverter.ToInt32(bytes2, 4);
            int hi = BitConverter.ToInt32(bytes2, 8);
            bool isNegative = Mantissa < 0;
            return new decimal(lo, mid, hi, isNegative, scale);
        }
        /// <summary>
        /// BigInteger へ変換します。
        /// * 小数点以下は切り捨て
        /// </summary>
        public BigInteger ToBigInteger()
        {
            var mantissa = Mantissa;
            if (Exponent > 0)
            {
                mantissa *= Pow10(Exponent);
            }
            else if (Exponent < 0)
            {
                mantissa /= Pow10(-Exponent);
            }
            return mantissa;
        }
        #endregion To*
        #endregion 型変換

        #region 数学関数
        /// <summary>
        /// 除算し、その結果を返します。剰余は出力パラメーターとして返されます。
        /// </summary>
        /// <param name="dividend">被除数。</param>
        /// <param name="divisor">除数。</param>
        /// <param name="remainder">このメソッドから制御が戻るときに、除算の剰余を表す System.Numerics.BigInteger 値が格納されます。 このパラメーターは初期化せずに渡されます。</param>
        /// <returns>除算の商。</returns>
        /// <exception cref="System.DivideByZeroException">divisor が 0 (ゼロ) です。</exception>
        public static BigDecimal DivRem(BigDecimal dividend, BigDecimal divisor, out BigDecimal remainder)
        {
            UniformExponent(ref dividend, ref divisor);
            BigInteger remainderInt;
            var quotient = BigInteger.DivRem(dividend.Mantissa, divisor.Mantissa, out remainderInt);
            remainder = new BigDecimal(remainderInt, dividend.Exponent, System.Math.Min(dividend.MinExponent, divisor.MinExponent));
            return new BigDecimal(quotient);
        }
        /// <summary>
        /// 指定された値を指数として System.Numerics.BigInteger 値を累乗します。
        /// </summary>
        /// <param name="value">累乗する数値</param>
        /// <param name="exponent">指数</param>
        /// <returns>value を exponent で累乗した結果。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">exponent が負の値です。</exception>
        public static BigDecimal Pow(BigDecimal value, int exponent)
        {
            if (exponent == 0)
            {
                return 1;
            }
            if (value == 0)
            {
                return 0;
            }
            if (exponent < 0)
            {
                exponent = -exponent;
                var temp = Pow(value, exponent);
                return 1 / temp;
            }
            else
            {
                var temp = One;
                for (int i = 0; i < exponent; i++)
                {
                    temp *= value;
                }
                //temp.Mantissa = BigInteger.Pow(temp.Mantissa, exponent);
                return temp;
            }
        }
        /// <summary>
        /// 指定された値を指数として 10 を累乗します。
        /// </summary>
        /// <param name="exponent">指数</param>
        /// <returns>10 を exponent で累乗した結果。</returns>
        public static BigInteger Pow10(int exponent)
        {
            return BigInteger.Pow(Radix, exponent);
        }
        /// <summary>
        /// 10 進値を最も近い整数に丸めます。
        /// </summary>
        public static BigDecimal Round(BigDecimal value, MidpointRounding mode)
        {
            var fractional = value.GetFractional();
            if (mode == MidpointRounding.AwayFromZero)
            {
                if (fractional >= ZeroPointFive)
                {
                    // 切り上げ
                    return (value - fractional) + 1;
                }
                else if (fractional <= -ZeroPointFive)
                {
                    // マイナス方向へ切り上げ
                    return (value - fractional) - 1;
                }
                else if (fractional < ZeroPointFive && fractional > -ZeroPointFive)
                {
                    // 切り捨て
                    return value - fractional;
                }
            }
            else if (mode == MidpointRounding.ToEven)
            {
                if (fractional > ZeroPointFive)
                {
                    // 切り上げ
                    return (value - fractional) + 1;
                }
                else if (fractional < -ZeroPointFive)
                {
                    // マイナス方向へ切り上げ
                    return (value - fractional) - 1;
                }
                else if (fractional < ZeroPointFive && fractional > -ZeroPointFive)
                {
                    // 切り捨て
                    return value - fractional;
                }
                // 中間の場合は奇数なら加算
                if (value.ToBigInteger().IsEven == false)
                {
                    if (fractional > 0)
                    {
                        // 切り上げ
                        return (value - fractional) + 1;
                    }
                    else
                    {
                        // マイナス方向へ切り上げ
                        return (value - fractional) - 1;
                    }
                }
                // 偶数なら切り捨て
                return value - fractional;
            }
            throw new ArgumentException($"{nameof(mode)}={mode} 値が不正");
        }
        /// <summary>
        /// 10 進値を指定した精度に丸めます。 パラメーターは、値が他の 2 つの数値の中間にある場合にその値を丸める方法を指定します。
        /// </summary>
        /// <param name="value">丸め対象の 10 進数。</param>
        /// <param name="precision">戻り値の精度（小数点以下の桁数）</param>
        /// <param name="mode">d が他の 2 つの数値の中間にある場合に丸める方法を指定する値。</param>
        public static BigDecimal Round(BigDecimal value, int precision, MidpointRounding mode)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(precision)}={precision} 範囲を超えています。");
            }
            var scale = Pow10(precision);
            value *= scale;
            return Round(value, mode) / scale;
        }
        /// <summary>
        /// 今の値が MinExponent 以下の値の場合、MinExponent に収まるように丸めます。
        /// 丸められた桁は 0 になります。
        /// * 丸めの種類は DefaultMidpointRounding
        /// </summary>
        public void RoundByMinExponent()
        {
            if (Exponent < MinExponent)
            {
                var digits = -(Exponent - MinExponent);
                RoundMantissa(digits);
            }
        }
        /// <summary>
        /// MinExponentを変更します。
        /// * 今の値が MinExponent 以下の値の場合、MinExponent に収まるように丸めます。
        /// * 丸められた桁は 0 になります。
        /// * 丸めの種類は DefaultMidpointRounding
        /// </summary>
        /// <param name="newMinExponent">新しい MinExponent</param>
        public void SetMinExponentAndRound(int newMinExponent)
        {
            MinExponent = newMinExponent;
            if (Exponent < MinExponent)
            {
                var digits = -(Exponent - MinExponent);
                RoundMantissa(digits);
            }
        }
        /// <summary>
        /// 仮数部を指定した桁の最も近い10の累乗に丸めます。
        /// * 丸めの種類は DefaultMidpointRounding
        /// * Round と違い小数部分だけでなく仮数部自体に対する丸め処理
        /// </summary>
        /// <param name="digits">丸める桁数</param>
        public void RoundMantissa(int digits)
        {
            if (digits > 0)
            {
                Mantissa = Round(Mantissa, digits, DefaultMidpointRounding);
                Exponent += digits;
            }
        }
        /// <summary>
        /// 整数の桁を返します。小数の桁は破棄されます。
        /// </summary>
        /// <param name="value">切り捨てる 10 進数。</param>
        /// <returns>0 方向の近似整数に丸めた結果。</returns>
        public static BigDecimal Truncate(BigDecimal value)
        {
            if (value.Exponent < 0)
            {
                value.Mantissa /= Pow10(-value.Exponent);
                value.Exponent = 0;
            }
            return value;
        }
        /// <summary>
        /// 正の無限大方向の近似整数に丸めます。
        /// 小数部がない場合は、未変更のまま返されます。
        /// </summary>
        public static BigDecimal Ceiling(BigDecimal value)
        {
            if (value.Exponent < 0)
            {
                var scale = Pow10(-value.Exponent);
                var remainder = value.Mantissa % scale;
                value.Mantissa /= scale;
                value.Exponent = 0;
                if (remainder > 0)
                {
                    value.Mantissa++;
                }
            }
            return value;
        }
        /// <summary>
        /// 負の無限大方向の近似整数に丸めます。
        /// 小数部がない場合は、未変更のまま返されます。
        /// </summary>
        public static BigDecimal Floor(BigDecimal value)
        {
            if (value.Exponent < 0)
            {
                var scale = Pow10(-value.Exponent);
                var remainder = value.Mantissa % scale;
                value.Mantissa /= scale;
                value.Exponent = 0;
                if (remainder < 0)
                {
                    value.Mantissa--;
                }
            }
            return value;
        }
        /// <summary>
        /// 指定された数値の平方根を返します。
        /// * 精度は現在の value.MinExponent の値となる
        /// </summary>
        /// <param name="value">平方根を求める対象の数値。</param>
        /// <returns>戻り値 0 または正 d の正の平方根。</returns>
        public static BigDecimal Sqrt(BigDecimal value)
        {
            int decimals = 0;
            if (value.MinExponent < 0)
            {
                decimals = -value.MinExponent;
            }
            return Sqrt(value, decimals);
        }
        /// <summary>
        /// 指定された数値の平方根を返します。
        /// </summary>
        /// <param name="value">平方根を求める対象の数値。</param>
        /// <param name="precision">(精度（小数点以下の桁数）</param>
        /// <returns>戻り値 0 または正 d の正の平方根。</returns>
        public static BigDecimal Sqrt(BigDecimal value, int precision)
        {
            // 計算回数は仮
            return Sqrt(value, precision, precision + 10);
        }
        /// <summary>
        /// 指定された数値の平方根を返します。
        /// </summary>
        /// <param name="value">平方根を求める対象の数値。</param>
        /// <param name="precision">精度（小数点以下の桁数）</param>
        /// <param name="count">計算回数</param>
        /// <returns>戻り値 0 または正 d の正の平方根。</returns>
        public static BigDecimal Sqrt(BigDecimal value, int precision, int count)
        {
            if (value == 0)
            {
                return 0;
            }
            // 精度を設定
            if (value.MinExponent > -precision)
            {
                value.MinExponent = -precision;
            }
            var temp = value;
            var prev = value;
            for (int i = 0; i < count; i++)
            {
                temp = (temp * temp + value) / (2 * temp);
                // 精度を制限
                temp = Round(temp, precision, DefaultMidpointRounding);
                // 前回から値が変わっていないなら終了
                if (prev == temp)
                {
                    return temp;
                }
                prev = temp;
            }
            return temp;
        }
        #endregion 数学関数

        #region INumber<BigDecimal>

        public static BigDecimal AdditiveIdentity => Zero;

        public static BigDecimal MultiplicativeIdentity => One;

        public static BigDecimal Abs(BigDecimal value)
        {
            if (value.IsNegative())
            {
                return -value;
            }
            return value;
        }

        public static BigDecimal Clamp(BigDecimal value, BigDecimal min, BigDecimal max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            return value;
        }

        public static BigDecimal Create<TOther>(TOther value) where TOther : INumber<TOther>
        {
            throw new NotImplementedException();
        }

        public static BigDecimal CreateSaturating<TOther>(TOther value) where TOther : INumber<TOther>
        {
            throw new NotImplementedException();
        }

        public static BigDecimal CreateTruncating<TOther>(TOther value) where TOther : INumber<TOther>
        {
            throw new NotImplementedException();
        }

        public static (BigDecimal Quotient, BigDecimal Remainder) DivRem(BigDecimal left, BigDecimal right)
        {
            throw new NotImplementedException();
        }

        public static BigDecimal Max(BigDecimal x, BigDecimal y)
        {
            if (x > y)
            {
                return x;
            }
            return y;
        }

        public static BigDecimal Min(BigDecimal x, BigDecimal y)
        {
            if (x < y)
            {
                return x;
            }
            return y;
        }

        public static BigDecimal Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static BigDecimal Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static BigDecimal Parse(ReadOnlySpan<char> s, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static BigDecimal Parse(string s, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static BigDecimal Sign(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool TryCreate<TOther>(TOther value, out BigDecimal result) where TOther : INumber<TOther>
        {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string s, NumberStyles style, IFormatProvider provider, out BigDecimal result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out BigDecimal result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out BigDecimal result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out BigDecimal result)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(BigDecimal? other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(BigDecimal? other)
        {
            throw new NotImplementedException();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public static BigDecimal operator +(BigDecimal value)
        {
            return value;
        }

        public static BigDecimal operator +(BigDecimal left, BigDecimal right)
        {
            UniformExponent(ref left, ref right);
            return new BigDecimal(
                left.Mantissa + right.Mantissa,
                left.Exponent,
                System.Math.Min(left.MinExponent, right.MinExponent));
        }

        public static BigDecimal operator -(BigDecimal value)
        {
            return new BigDecimal(-value.Mantissa, value.Exponent, value.MinExponent);
        }

        public static BigDecimal operator -(BigDecimal left, BigDecimal right)
        {
            UniformExponent(ref left, ref right);
            return new BigDecimal(
                left.Mantissa - right.Mantissa,
                left.Exponent,
                System.Math.Min(left.MinExponent, right.MinExponent));
        }

        public static BigDecimal operator ++(BigDecimal value)
        {
            return value + 1;
        }

        public static BigDecimal operator --(BigDecimal value)
        {
            return value - 1;
        }

        public static BigDecimal operator *(BigDecimal left, BigDecimal right)
        {
            var temp = new BigDecimal(
                left.Mantissa * right.Mantissa,
                left.Exponent + right.Exponent);
            temp.MinExponent = System.Math.Min(left.MinExponent, right.MinExponent);
            temp.RoundByMinExponent();
            return temp;
        }

        public static BigDecimal operator /(BigDecimal left, BigDecimal right)
        {
            var temp = new BigDecimal(left);
            // 指数が小さい方に合わせる
            temp.MinExponent = System.Math.Min(left.MinExponent, right.MinExponent);
            // 割られる数の Exponent を最小にする。
            // さらに、丸め処理のため桁増やす
            var addExponent = System.Math.Min(right.Exponent + AddExponent, 0);
            temp._MinimizeExponent(temp.MinExponent + addExponent);
            // 除算
            temp.Mantissa /= right.Mantissa;
            temp.Exponent -= right.Exponent;
            // 丸め処理(桁増やした分はここで減る)
            temp.RoundByMinExponent();
            Assert(temp.Exponent >= temp.MinExponent);
            // 最適化
            temp.MinimizeMantissa();
            return temp;
        }

        public static BigDecimal operator %(BigDecimal left, BigDecimal right)
        {
            UniformExponent(ref left, ref right);
            return new BigDecimal(
                left.Mantissa % right.Mantissa,
                left.Exponent,
                System.Math.Min(left.MinExponent, right.MinExponent));
        }

        public static bool operator ==(BigDecimal left, BigDecimal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BigDecimal left, BigDecimal right)
        {
            return left.Equals(right) != true;
        }

        public static bool operator <(BigDecimal left, BigDecimal right)
        {
            _ConvertSameExponent(ref left, ref right);
            return left.Mantissa < right.Mantissa;
        }

        public static bool operator >(BigDecimal left, BigDecimal right)
        {
            _ConvertSameExponent(ref left, ref right);
            return left.Mantissa > right.Mantissa;
        }

        public static bool operator <=(BigDecimal left, BigDecimal right)
        {
            _ConvertSameExponent(ref left, ref right);
            return left.Mantissa <= right.Mantissa;
        }

        public static bool operator >=(BigDecimal left, BigDecimal right)
        {
            _ConvertSameExponent(ref left, ref right);
            return left.Mantissa >= right.Mantissa;
        }

        static BigDecimal IModulusOperators<BigDecimal, BigDecimal, BigDecimal>.operator %(BigDecimal left, BigDecimal right)
        {
            throw new NotImplementedException();
        }

        static BigDecimal IAdditionOperators<BigDecimal, BigDecimal, BigDecimal>.operator +(BigDecimal left, BigDecimal right)
        {
            throw new NotImplementedException();
        }

        static BigDecimal IDecrementOperators<BigDecimal>.operator --(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static BigDecimal IDivisionOperators<BigDecimal, BigDecimal, BigDecimal>.operator /(BigDecimal left, BigDecimal right)
        {
            throw new NotImplementedException();
        }

        static BigDecimal IIncrementOperators<BigDecimal>.operator ++(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static BigDecimal IMultiplyOperators<BigDecimal, BigDecimal, BigDecimal>.operator *(BigDecimal left, BigDecimal right)
        {
            throw new NotImplementedException();
        }

        static BigDecimal ISubtractionOperators<BigDecimal, BigDecimal, BigDecimal>.operator -(BigDecimal left, BigDecimal right)
        {
            throw new NotImplementedException();
        }

        static BigDecimal IUnaryNegationOperators<BigDecimal, BigDecimal>.operator -(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static BigDecimal IUnaryPlusOperators<BigDecimal, BigDecimal>.operator +(BigDecimal value)
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS8765 // パラメーターの型の NULL 値の許容が、オーバーライドされたメンバーと一致しません。おそらく、NULL 値の許容の属性が原因です。
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is BigDecimal)
            {
                return Equals((BigDecimal)obj);
            }
            return false;
        }
#pragma warning restore CS8765 // パラメーターの型の NULL 値の許容が、オーバーライドされたメンバーと一致しません。おそらく、NULL 値の許容の属性が原因です。

        public int CompareTo(BigDecimal other)
        {
            return Compare(this, other);
        }

        public bool Equals(BigDecimal other)
        {
            return Equals(this, other);
        }

        public override int GetHashCode()
        {
            MinimizeMantissa();
            return Mantissa.GetHashCode() ^ Exponent.GetHashCode();
        }

        #endregion INumber<BigDecimal>


        #region 補助
        /// <summary>
        /// 指数形式ではなく小数形式に変換するためのフォーマット
        /// </summary>
        static readonly string DecimalFormat = "0." + new string('#', 338);
        /// <summary>
        /// 符号ビットを取得
        /// </summary>
        /// <returns>正なら 0 を返す。負なら 1 を返す。</returns>
        public static byte GetSignBits(decimal value)
        {
            uint bits = (uint)decimal.GetBits(value)[3];
            return (byte)(bits >> 31);
        }
        /// <summary>
        /// 符号を取得
        /// </summary>
        /// <returns>正なら +1 を返す。負なら -1 を返す。</returns>
        public static int GetSign(decimal value)
        {
            if (value < 0)
            {
                return -1;
            }
            return +1;
        }
        /// <summary>
        /// 指数部を取得
        /// </summary>
        /// <returns>指数部のビット</returns>
        public static byte GetExponentBits(decimal value)
        {
            int[] bits = decimal.GetBits(value);
            return (byte)((bits[3] >> 16) & 0x7F);
        }
        /// <summary>
        /// 指数を取得
        /// </summary>
        /// <returns>10の基数に累乗する際の指数</returns>
        public static int GetExponent(decimal value)
        {
            return -GetExponentBits(value);
        }
        /// <summary>
        /// 仮数部を取得
        /// </summary>
        /// <returns>仮数部のビット</returns>
        public static int[] GetMantissaBits(decimal value)
        {
            int[] bits = decimal.GetBits(value);
            return bits.Take(3).ToArray();
        }
        /// <summary>
        /// 仮数を取得
        /// </summary>
        /// <returns>仮数のみの値</returns>
        public static decimal GetMantissa(decimal value)
        {
            int[] bits = decimal.GetBits(value);
            return new decimal(bits[0], bits[1], bits[2], false, 0);
        }
        /// <summary>
        /// 小数部を取得
        /// </summary>
        public static decimal GetFractional(decimal value)
        {
            return decimal.Remainder(value, 1);
        }
        /// <summary>
        /// 丸め処理
        /// </summary>
        /// <param name="value">丸める値</param>
        /// <param name="digits">桁数</param>
        /// <param name="midpointRounding">丸め処理の方法</param>
        /// <returns>丸めた後の値</returns>
        public static BigInteger Round(BigInteger value, int digits, MidpointRounding midpointRounding)
        {
            if (digits <= 0)
            {
                return value;
            }
            var divisor = BigInteger.Pow(Radix, digits);
            var half = divisor / 2;
            var remainder = value % divisor;
            // 中間を超えている時
            if (remainder > half)
            {
                value += divisor;
            }
            else if (remainder < -half)
            {
                value -= divisor;
            }
            // 桁を減らす
            value /= divisor;
            // 中間の時
            if (remainder == half || remainder == -half)
            {
                if (midpointRounding == MidpointRounding.ToEven)
                {
                    // 偶数に変更する。
                    remainder = (int)(value % 10);// 新1桁目
                                                  // 奇数なら変更する
                    if (remainder.IsEven == false)
                    {
                        if (remainder > 0)
                        {
                            value++;
                        }
                        else
                        {
                            value--;
                        }
                    }
                }
                else if (midpointRounding == MidpointRounding.AwayFromZero)
                {
                    // 普通の四捨五入
                    if (remainder > 0)
                    {
                        value++;
                    }
                    else
                    {
                        value--;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(midpointRounding), $"{midpointRounding}");
                }
            }
            return value;
        }
        /// <summary>
        /// 指定した双方の値を、同じ Exponent になるように変換する
        /// ※Mantissa は調整するが、Exponent は調整しないので、元の値から大きさが変わる。
        /// </summary>
        private static void _ConvertSameExponent(ref BigDecimal valueL, ref BigDecimal valueR)
        {
            if (valueL.Exponent > valueR.Exponent)
            {
                var exponentDiff = valueL.Exponent - valueR.Exponent;
                valueL.Mantissa *= Pow10(exponentDiff);
            }
            else if (valueR.Exponent > valueL.Exponent)
            {
                var exponentDiff = valueR.Exponent - valueL.Exponent;
                valueR.Mantissa *= Pow10(exponentDiff);
            }
        }

        public static bool IsCanonical(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsComplexNumber(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsEvenInteger(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsFinite(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsImaginaryNumber(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsInfinity(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsInteger(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsNaN(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsNegative(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsNegativeInfinity(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsNormal(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsOddInteger(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsPositive(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsPositiveInfinity(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsRealNumber(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsSubnormal(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static bool IsZero(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        public static BigDecimal MaxMagnitude(BigDecimal x, BigDecimal y)
        {
            throw new NotImplementedException();
        }

        public static BigDecimal MaxMagnitudeNumber(BigDecimal x, BigDecimal y)
        {
            throw new NotImplementedException();
        }

        public static BigDecimal MinMagnitude(BigDecimal x, BigDecimal y)
        {
            throw new NotImplementedException();
        }

        public static BigDecimal MinMagnitudeNumber(BigDecimal x, BigDecimal y)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumber<BigDecimal>.Clamp(BigDecimal value, BigDecimal min, BigDecimal max)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumber<BigDecimal>.Max(BigDecimal x, BigDecimal y)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumber<BigDecimal>.Min(BigDecimal x, BigDecimal y)
        {
            throw new NotImplementedException();
        }

        int IComparable.CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        int IComparable<BigDecimal>.CompareTo(BigDecimal other)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumberBase<BigDecimal>.Abs(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumberBase<BigDecimal>.CreateSaturating<TOther>(TOther value)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumberBase<BigDecimal>.CreateTruncating<TOther>(TOther value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsCanonical(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsComplexNumber(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsEvenInteger(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsFinite(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsImaginaryNumber(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsInfinity(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsInteger(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsNaN(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsNegative(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsNegativeInfinity(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsNormal(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsOddInteger(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsPositive(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsPositiveInfinity(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsRealNumber(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsSubnormal(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.IsZero(BigDecimal value)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumberBase<BigDecimal>.MaxMagnitude(BigDecimal x, BigDecimal y)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumberBase<BigDecimal>.MaxMagnitudeNumber(BigDecimal x, BigDecimal y)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumberBase<BigDecimal>.MinMagnitude(BigDecimal x, BigDecimal y)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumberBase<BigDecimal>.MinMagnitudeNumber(BigDecimal x, BigDecimal y)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumberBase<BigDecimal>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        static BigDecimal INumberBase<BigDecimal>.Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertFromChecked<TOther>(TOther value, out BigDecimal result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertFromSaturating<TOther>(TOther value, out BigDecimal result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertFromTruncating<TOther>(TOther value, out BigDecimal result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertToChecked<TOther>(BigDecimal value, out TOther result) where TOther : default
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertToSaturating<TOther>(BigDecimal value, out TOther result) where TOther : default
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertToTruncating<TOther>(BigDecimal value, out TOther result) where TOther : default
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out BigDecimal result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryParse(string s, NumberStyles style, IFormatProvider provider, out BigDecimal result)
        {
            throw new NotImplementedException();
        }

        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        static BigDecimal ISpanParsable<BigDecimal>.Parse(ReadOnlySpan<char> s, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        static bool ISpanParsable<BigDecimal>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out BigDecimal result)
        {
            throw new NotImplementedException();
        }

        static BigDecimal IParsable<BigDecimal>.Parse(string s, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        static bool IParsable<BigDecimal>.TryParse(string s, IFormatProvider provider, out BigDecimal result)
        {
            throw new NotImplementedException();
        }
        #endregion 補助
    }
}