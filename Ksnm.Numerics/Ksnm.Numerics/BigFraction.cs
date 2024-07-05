using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Ksnm.Numerics
{
    // コードを再利用するためのエイリアスを定義
    using Fraction = BigFraction;
    using Integer = BigInteger;
    using Int8 = sbyte;
    using UInt8 = byte;
    using Float16 = Half;
    using Float32 = float;
    using Float64 = double;
    /// <summary>
    /// 任意精度の分数型
    /// </summary>
    public struct BigFraction :
        INumber<Fraction>,
        ISignedNumber<Fraction>,
        IFloatingPointConstants<Fraction>
    {
        #region フィールド
        #endregion フィールド

        #region プロパティ
        /// <summary>
        /// 分子
        /// </summary>
        public Integer Numerator { get; private set; }
        /// <summary>
        /// 分母
        /// </summary>
        public Integer Denominator { get; private set; }
        #endregion プロパティ

        #region コンストラクタ
        public BigFraction() : this(0, 1) { }
        /// <summary>
        /// 分子と分母を指定して初期化
        /// </summary>
        /// <param name="numerator">分子</param>
        /// <param name="denominator">分母</param>
        public BigFraction(Integer numerator, Integer denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
            Reduce();
        }
        /// <summary>
        /// 分子を指定して初期化
        /// </summary>
        /// <param name="numerator">分子</param>
        public BigFraction(Integer numerator) : this(numerator, 1)
        {
        }
        public BigFraction(Int32 numerator) : this(numerator, 1)
        {
        }
        public BigFraction(UInt32 numerator) : this(numerator, 1)
        {
        }
        public BigFraction(Int64 numerator) : this(numerator, 1)
        {
        }
        public BigFraction(UInt64 numerator) : this(numerator, 1)
        {
        }
        public BigFraction(Float16 number) : this((Float64)number)
        {
        }
        public BigFraction(Float32 number) : this((Float64)number)
        {
        }
        public BigFraction(Float64 number)
        {
            var exponent = number.GetExponent();
            var mantissa = (Integer)number.GetMantissa();
            if (exponent < 0)
            {
                exponent = -exponent;
                Numerator = mantissa;
                Denominator = (Integer)1 << exponent;
            }
            else
            {
                Numerator = mantissa << exponent;
                Denominator = 1;
            }
            Reduce();
        }
        public BigFraction(Decimal number)
        {
            var exponent = number.GetExponent();
            var mantissa = (Integer)number.GetMantissa();
            if (exponent < 0)
            {
                exponent = -exponent;
                Numerator = mantissa;
                Denominator = Integer.Pow(10, exponent);
            }
            else
            {
                Numerator = mantissa << exponent;
                Denominator = 1;
            }
            Reduce();
        }
        public BigFraction(BigDecimal number)
        {
            var exponent = number.Exponent;
            var mantissa = number.Mantissa;
            if (exponent < 0)
            {
                exponent = -exponent;
                Numerator = mantissa;
                Denominator = Integer.Pow(10, exponent);
            }
            else
            {
                Numerator = mantissa << exponent;
                Denominator = 1;
            }
            Reduce();
        }
        #endregion コンストラクタ

        #region 独自関数
        /// <summary>
        /// 約分する。
        /// <para>可約でない場合は何もしません。</para>
        /// </summary>
        public void Reduce()
        {
            var gcd = Math.GreatestCommonDivisor(Numerator, Denominator);
            if (gcd > 1)
            {
                Numerator = (Integer)(Numerator / gcd);
                Denominator = (Integer)(Denominator / gcd);
            }
        }
        /// <summary>
        /// 可約ならtrueを返す。
        /// <para>判定後にReduce()を呼び出すより、Reduce()単体で使用したほうが効率的です。</para>
        /// </summary>
        public bool IsReducible()
        {
            var gcd = Math.GreatestCommonDivisor(Numerator, Denominator);
            return gcd > 1;
        }
        /// <summary>
        /// 逆数を返す。
        /// </summary>
        public Fraction GetReciprocal()
        {
            return new Fraction(Denominator, Numerator);
        }
        #endregion 独自関数

        #region object
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
        #endregion object

        #region 型変換

        #region 他の型→Fraction
        public static implicit operator Fraction(Int8 value) => new Fraction((int)value);
        public static implicit operator Fraction(UInt8 value) => new Fraction((int)value);
        public static implicit operator Fraction(Int16 value) => new Fraction(value);
        public static implicit operator Fraction(UInt16 value) => new Fraction(value);
        public static implicit operator Fraction(Int32 value) => new Fraction(value);
        public static implicit operator Fraction(UInt32 value) => new Fraction(value);
        public static implicit operator Fraction(Int64 value) => new Fraction(value);
        public static implicit operator Fraction(UInt64 value) => new Fraction(value);
        public static implicit operator Fraction(Int128 value) => new Fraction(value);
        public static implicit operator Fraction(UInt128 value) => new Fraction(value);
        public static implicit operator Fraction(Float16 value) => new Fraction(value);
        public static implicit operator Fraction(Float32 value) => new Fraction(value);
        public static implicit operator Fraction(Float64 value) => new Fraction(value);
        public static implicit operator Fraction(Decimal value) => new Fraction(value);
        public static implicit operator Fraction(Integer value) => new Fraction(value);
        public static implicit operator Fraction(BigDecimal value) => new Fraction(value);
        #endregion 他の型→Fraction

        #region Fraction→他の型
        public static explicit operator Int8(Fraction value)
        {
            var integer = (Integer)value;
            return (Int8)integer;
        }
        public static explicit operator UInt8(Fraction value)
        {
            var integer = (Integer)value;
            return (UInt8)integer;
        }
        public static explicit operator Int16(Fraction value)
        {
            var integer = (Integer)value;
            return (Int16)integer;
        }
        public static explicit operator UInt16(Fraction value)
        {
            var integer = (Integer)value;
            return (UInt16)integer;
        }
        public static explicit operator Int32(Fraction value)
        {
            var integer = (Integer)value;
            return (Int32)integer;
        }
        public static explicit operator UInt32(Fraction value)
        {
            var integer = (Integer)value;
            return (UInt32)integer;
        }
        public static explicit operator Int64(Fraction value)
        {
            var integer = (Integer)value;
            return (Int64)integer;
        }
        public static explicit operator UInt64(Fraction value)
        {
            var integer = (Integer)value;
            return (UInt64)integer;
        }
        public static explicit operator Int128(Fraction value)
        {
            var integer = (Integer)value;
            return (Int128)integer;
        }
        public static explicit operator UInt128(Fraction value)
        {
            var integer = (Integer)value;
            return (UInt128)integer;
        }
        public static explicit operator Float16(Fraction value)
        {
            var f = (Float64)value;
            return (Float16)f;
        }
        public static explicit operator Float32(Fraction value)
        {
            var f = (Float64)value;
            return (Float32)f;
        }
        public static explicit operator Float64(Fraction value)
        {
            Float64 numerator = (Float64)value.Numerator;
            Float64 denominator = (Float64)value.Denominator;
            return numerator / denominator;
        }
        public static explicit operator Decimal(Fraction value)
        {
            Decimal numerator = (Decimal)value.Numerator;
            Decimal denominator = (Decimal)value.Denominator;
            return numerator / denominator;
        }
        public static explicit operator Integer(Fraction value)
        {
            return value.Numerator / value.Denominator;
        }
        public static explicit operator BigDecimal(Fraction value)
        {
            BigDecimal numerator = value.Numerator;
            BigDecimal denominator = value.Denominator;
            var exponent = -(int)Integer.Log10(value.Denominator) * 10;
            if (numerator.MinExponent > exponent)
            {
                numerator.MinExponent = exponent;
            }
            return numerator / denominator;
        }
        #endregion Fraction→他の型

        #endregion 型変換

        #region 数学関数
        /// <summary>
        /// 指定された数値の平方根を返します。
        /// </summary>
        /// <param name="value">平方根を求める対象の数値。</param>
        /// <param name="count">計算回数</param>
        /// <returns>戻り値 0 または正 d の正の平方根。</returns>
        public static Fraction Sqrt(Fraction value, int count)
        {
            if (value == 0)
            {
                return 0;
            }
            var temp = value;
            for (int i = 0; i < count; i++)
            {
                temp = (temp * temp + value) / (2 * temp);
            }
            return temp;
        }
        #endregion 数学関数

        #region 
        public static Fraction One => 1;

        public static int Radix => 2;

        public static Fraction Zero => 0;

        public static Fraction AdditiveIdentity => 0;

        public static Fraction MultiplicativeIdentity => 1;

        public static Fraction NegativeOne => -1;

        /// <summary>
        /// ネイピア数・自然対数の底（四捨五入済み小数点以下18桁）
        /// 2.718281828459045235_360
        /// </summary>
        public static Fraction E => new Fraction(2718281828459045235, 1000000000000000000);

        /// <summary>
        /// 円周率（四捨五入済み小数点以下18桁）
        /// 3.141592653589793238_462
        /// </summary>
        public static Fraction Pi => new Fraction(3141592653589793238, 1000000000000000000);

        /// <summary>
        /// 円周率*2（四捨五入済み小数点以下18桁）
        /// 6.283185307179586476_9252
        /// </summary>
        public static Fraction Tau => new Fraction(6283185307179586477, 1000000000000000000);

        public static Fraction Abs(Fraction value)
        {
            return new Fraction(Integer.Abs(value.Numerator), Integer.Abs(value.Denominator));
        }

        public static bool IsCanonical(Fraction value) => true;

        public static bool IsComplexNumber(Fraction value) => false;

        public static bool IsEvenInteger(Fraction value)
        {
            value.Reduce();
            if (value.Denominator == 1 || value.Denominator == -1)
            {
                return Integer.IsEvenInteger(value.Numerator);
            }
            return false;
        }

        public static bool IsFinite(Fraction value)
        {
            return value.Denominator != 0;
        }

        public static bool IsImaginaryNumber(Fraction value) => false;

        public static bool IsInfinity(Fraction value) => false;

        public static bool IsInteger(Fraction value)
        {
            value.Reduce();
            return (value.Denominator == 1 || value.Denominator == -1);
        }

        public static bool IsNaN(Fraction value)
        {
            return value.Denominator == 0;
        }

        public static bool IsNegative(Fraction value)
        {
            // 分母が0なら非数
            if (value.Denominator == 0)
            {
                return false;
            }
            // 分子が0なら正
            if (value.Numerator == 0)
            {
                return false;
            }
            // 両方負なら正
            if (Integer.IsNegative(value.Numerator) && Integer.IsNegative(value.Denominator))
            {
                return false;
            }
            // どちらかが負なら負
            return Integer.IsNegative(value.Numerator) || Integer.IsNegative(value.Denominator);
        }

        public static bool IsNegativeInfinity(Fraction value) => false;

        public static bool IsNormal(Fraction value) => value != 0;

        public static bool IsOddInteger(Fraction value)
        {
            value.Reduce();
            if (value.Denominator == 1 || value.Denominator == -1)
            {
                return Integer.IsOddInteger(value.Numerator);
            }
            return false;
        }

        public static bool IsPositive(Fraction value)
        {
            // 分母が0なら非数
            if (value.Denominator == 0)
            {
                return false;
            }
            // 分子が0なら正
            if (value.Numerator == 0)
            {
                return true;
            }
            // 両方負なら正
            if (Integer.IsNegative(value.Numerator) && Integer.IsNegative(value.Denominator))
            {
                return true;
            }
            // 両方正なら正
            return Integer.IsPositive(value.Numerator) && Integer.IsPositive(value.Denominator);
        }

        public static bool IsPositiveInfinity(Fraction value) => false;

        public static bool IsRealNumber(Fraction value)
        {
            return value.Denominator != 0;
        }

        public static bool IsSubnormal(Fraction value) => false;

        public static bool IsZero(Fraction value)
        {
            if (value.Denominator == 0)
            {
                return false;
            }
            return value.Numerator.IsZero;
        }

        public static Fraction MaxMagnitude(Fraction x, Fraction y)
        {
            Fraction ax = Abs(x);
            Fraction ay = Abs(y);

            if (ax > ay)
            {
                return x;
            }

            if (ax == ay)
            {
                return IsNegative(x) ? y : x;
            }

            return y;
        }

        public static Fraction MaxMagnitudeNumber(Fraction x, Fraction y)
        {
            return MaxMagnitude(x, y);
        }

        public static Fraction MinMagnitude(Fraction x, Fraction y)
        {
            Fraction ax = Abs(x);
            Fraction ay = Abs(y);

            if (ax < ay)
            {
                return x;
            }

            if (ax == ay)
            {
                return IsNegative(x) ? x : y;
            }

            return y;
        }

        public static Fraction MinMagnitudeNumber(Fraction x, Fraction y)
        {
            return MinMagnitude(x, y);
        }

        public static Fraction Parse(string s)
        {
            var temp = Zero;
            string slash = "/";
            var texts = s.Split(slash);
            if (texts.Length == 1)
            {
                temp.Numerator = Integer.Parse(texts[0]);
            }
            else if (texts.Length >= 2)
            {
                temp.Numerator = Integer.Parse(texts[0]);
                temp.Denominator = Integer.Parse(texts[1]);
            }
            return temp;
        }

        public static Fraction Parse(string s, NumberFormatInfo numberFormatInfo)
        {
            return Parse(s);
        }

        public static Fraction Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        {
            return Parse(s.ToString());
        }

        public static Fraction Parse(string s, NumberStyles style, IFormatProvider? provider)
        {
            return Parse(s);
        }

        public static Fraction Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            return Parse(s.ToString());
        }

        public static Fraction Parse(string s, IFormatProvider? provider)
        {
            return Parse(s);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result)
        {
            try
            {
                result = Parse(s, style, provider);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result)
        {
            if (s == null)
            {
                result = Zero;
                return false;
            }
            try
            {
                result = Parse(s, style, provider);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result)
        {
            return TryParse(s, NumberStyles.Number, provider, out result);
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result)
        {
            return TryParse(s, NumberStyles.Number, provider, out result);
        }

        static bool INumberBase<Fraction>.TryConvertFromChecked<TOther>(TOther value, out Fraction result)
        {
            return TryConvertFromChecked(value, out result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromChecked<TOther>(TOther value, out Fraction result)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(byte))
            {
                var actualValue = (byte)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                var actualValue = (char)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                var actualValue = (short)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                var actualValue = (ushort)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                var actualValue = (int)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                var actualValue = (uint)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualValue = (long)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                var actualValue = (ulong)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                var actualValue = (Int128)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                var actualValue = (UInt128)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualValue = (nint)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                var actualValue = (nuint)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(Half))
            {
                var actualValue = (Half)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(Single))
            {
                var actualValue = (Single)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualValue = (decimal)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(BigInteger))
            {
                var actualValue = (BigInteger)(object)value;
                result = actualValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool INumberBase<Fraction>.TryConvertFromSaturating<TOther>(TOther value, out Fraction result)
        {
            return TryConvertFrom(value, out result);
        }

        static bool INumberBase<Fraction>.TryConvertFromTruncating<TOther>(TOther value, out Fraction result)
        {
            return TryConvertFrom(value, out result);
        }

        private static bool TryConvertFrom<TOther>(TOther value, out Fraction result)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(byte))
            {
                byte actualValue = (byte)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                char actualValue = (char)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                ushort actualValue = (ushort)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                uint actualValue = (uint)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                ulong actualValue = (ulong)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                UInt128 actualValue = (UInt128)(object)value;
                result = actualValue;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                nuint actualValue = (nuint)(object)value;
                result = actualValue;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Fraction>.TryConvertToChecked<TOther>(Fraction value, [MaybeNullWhen(false)] out TOther result)
        {
            if (typeof(TOther) == typeof(double))
            {
                double actualResult = checked((double)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Half))
            {
                Half actualResult = checked((Half)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualResult = checked((short)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualResult = checked((int)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualResult = checked((long)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                Int128 actualResult = checked((Int128)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualResult = checked((nint)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualResult = checked((sbyte)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                float actualResult = checked((float)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool INumberBase<Fraction>.TryConvertToSaturating<TOther>(Fraction value, [MaybeNullWhen(false)] out TOther result)
        {
            return TryConvertTo(value, out result);
        }

        static bool INumberBase<Fraction>.TryConvertToTruncating<TOther>(Fraction value, [MaybeNullWhen(false)] out TOther result)
        {
            return TryConvertTo(value, out result);
        }

        private static bool TryConvertTo<TOther>(Fraction value, [MaybeNullWhen(false)] out TOther result)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(double))
            {
                double actualResult = (double)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Half))
            {
                Half actualResult = (Half)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                short actualResult = (value >= short.MaxValue) ? short.MaxValue :
                                     (value <= short.MinValue) ? short.MinValue : (short)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                int actualResult = (value >= int.MaxValue) ? int.MaxValue :
                                   (value <= int.MinValue) ? int.MinValue : (int)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                long actualResult = (value >= long.MaxValue) ? long.MaxValue :
                                    (value <= long.MinValue) ? long.MinValue : (long)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                Int128 actualResult = (Int128)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                nint actualResult = (value >= nint.MaxValue) ? nint.MaxValue :
                                    (value <= nint.MinValue) ? nint.MinValue : (nint)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                sbyte actualResult = (value >= sbyte.MaxValue) ? sbyte.MaxValue :
                                     (value <= sbyte.MinValue) ? sbyte.MinValue : (sbyte)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                float actualResult = (float)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        #endregion

        #region CompareTo

        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }
            if (!(obj is Fraction))
            {
                throw new ArgumentException();
            }
            return CompareTo((Fraction)obj);
        }

        public int CompareTo(Fraction other)
        {
            var numerator = this.Numerator;// インスタンスの値を変更しないようにthisのは複製
            numerator *= other.Denominator;
            other.Numerator *= this.Denominator;
            return numerator.CompareTo(other.Numerator);
        }

        #endregion CompareTo

        public bool Equals(Fraction other)
        {
            var numerator = Numerator;// インスタンスの値を変更しないようにthisのは複製
            numerator *= other.Denominator;
            other.Numerator *= Denominator;
            return numerator.Equals(other.Numerator);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return ToString();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            try
            {
                var str = ToString(format.ToString(), provider);
                charsWritten = int.Min(str.Length, destination.Length);
                str = str.Substring(0, charsWritten);
                str.CopyTo(destination);
                return true;
            }
            catch
            {
                charsWritten = 0;
                return false;
            }
        }
        #region Operations
        public static Fraction operator +(Fraction value)
        {
            return value;
        }

        public static Fraction operator ~(in Fraction value)
        {
            return new Fraction(~value.Numerator, ~value.Denominator);
        }

        public static Fraction operator +(Fraction left, Fraction right)
        {
            var temp = new Fraction();
            if (right.Denominator == left.Denominator)
            {
                temp.Numerator = left.Numerator + right.Numerator;
                temp.Denominator = left.Denominator;
            }
            else
            {
                temp.Numerator = left.Numerator * right.Denominator + right.Numerator * left.Denominator;
                temp.Denominator = left.Denominator * right.Denominator;
            }
            temp.Reduce();
            return temp;
        }

        public static Fraction operator -(Fraction value)
        {
            return new Fraction(-value.Numerator, value.Denominator);
        }

        public static Fraction operator -(Fraction left, Fraction right)
        {
            var temp = new Fraction();
            if (right.Denominator == left.Denominator)
            {
                temp.Numerator = left.Numerator - right.Numerator;
                temp.Denominator = left.Denominator;
            }
            else
            {
                temp.Numerator = left.Numerator * right.Denominator - right.Numerator * left.Denominator;
                temp.Denominator = left.Denominator * right.Denominator;
            }
            temp.Reduce();
            return temp;
        }

        public static Fraction operator ++(Fraction value)
        {
            return new Fraction(value.Numerator + 1, value.Denominator);
        }

        public static Fraction operator --(Fraction value)
        {
            return new Fraction(value.Numerator - 1, value.Denominator);
        }

        public static Fraction operator *(Fraction left, Fraction right)
        {
            var temp = new Fraction();
            temp.Numerator = left.Numerator * right.Numerator;
            temp.Denominator = left.Denominator * right.Denominator;
            temp.Reduce();
            return temp;
        }

        public static Fraction operator /(Fraction left, Fraction right)
        {
            var temp = new Fraction();
            temp.Numerator = left.Numerator * right.Denominator;
            temp.Denominator = left.Denominator * right.Numerator;
            temp.Reduce();
            return temp;
        }

        public static Fraction operator %(Fraction left, Fraction right)
        {
            return 0;
        }

        public static bool operator ==(Fraction left, Fraction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Fraction left, Fraction right)
        {
            // どちらかがNaNならfalse
            if (IsNaN(left) || IsNaN(right))
            {
                return true;
            }
            return !left.Equals(right);
        }

        public static bool operator <(Fraction left, Fraction right)
        {
            return left.Numerator * right.Denominator < right.Numerator * left.Denominator;
        }

        public static bool operator >(Fraction left, Fraction right)
        {
            return left.Numerator * right.Denominator > right.Numerator * left.Denominator;
        }

        public static bool operator <=(Fraction left, Fraction right)
        {
            return left.Numerator * right.Denominator <= right.Numerator * left.Denominator;
        }

        public static bool operator >=(Fraction left, Fraction right)
        {
            return left.Numerator * right.Denominator >= right.Numerator * left.Denominator;
        }
        #endregion Operations

        public static bool Equals(in Fraction objA, in Fraction objB)
        {
            // どちらかがNaNならfalse
            if (IsNaN(objA) || IsNaN(objB))
            {
                return false;
            }
            return objA.Equals(objB);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Fraction))
            {
                throw new ArgumentException();
            }
            return Equals((Fraction)obj);
        }

        public override int GetHashCode()
        {
            return Numerator.GetHashCode() ^ Denominator.GetHashCode();
        }
    }
}
