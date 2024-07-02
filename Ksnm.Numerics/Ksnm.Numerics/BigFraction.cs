using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    public struct BigFraction : INumber<BigFraction>
    {
        #region フィールド
        #endregion フィールド

        #region プロパティ
        /// <summary>
        /// 分子
        /// </summary>
        public BigInteger Numerator { get; private set; }
        /// <summary>
        /// 分母
        /// </summary>
        public BigInteger Denominator { get; private set; }
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
                Denominator = BigInteger.Pow(10, exponent);
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
        public static implicit operator Fraction(BigInteger value) => new Fraction(value);
        #endregion 他の型→Fraction

        #region Fraction→他の型
        public static explicit operator Int8(Fraction value)
        {
            var integer = (BigInteger)value;
            return (Int8)integer;
        }
        public static explicit operator UInt8(Fraction value)
        {
            var integer = (BigInteger)value;
            return (UInt8)integer;
        }
        public static explicit operator Int16(Fraction value)
        {
            var integer = (BigInteger)value;
            return (Int16)integer;
        }
        public static explicit operator UInt16(Fraction value)
        {
            var integer = (BigInteger)value;
            return (UInt16)integer;
        }
        public static explicit operator Int32(Fraction value)
        {
            var integer = (BigInteger)value;
            return (Int32)integer;
        }
        public static explicit operator UInt32(Fraction value)
        {
            var integer = (BigInteger)value;
            return (UInt32)integer;
        }
        public static explicit operator Int64(Fraction value)
        {
            var integer = (BigInteger)value;
            return (Int64)integer;
        }
        public static explicit operator UInt64(Fraction value)
        {
            var integer = (BigInteger)value;
            return (UInt64)integer;
        }
        public static explicit operator Int128(Fraction value)
        {
            var integer = (BigInteger)value;
            return (Int128)integer;
        }
        public static explicit operator UInt128(Fraction value)
        {
            var integer = (BigInteger)value;
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
        public static explicit operator BigInteger(Fraction value)
        {
            return value.Numerator / value.Denominator;
        }
        #endregion Fraction→他の型

        #endregion 型変換

        public static BigFraction One => 1;

        public static int Radix => 2;

        public static BigFraction Zero => 0;

        public static BigFraction AdditiveIdentity => 0;

        public static BigFraction MultiplicativeIdentity => 1;

        public static BigFraction Abs(BigFraction value)
        {
            return new Fraction(Integer.Abs(value.Numerator), Integer.Abs(value.Denominator));
        }

        public static bool IsCanonical(BigFraction value) => true;

        public static bool IsComplexNumber(BigFraction value) => false;

        public static bool IsEvenInteger(BigFraction value)
        {
            value.Reduce();
            if (value.Denominator == 1 || value.Denominator == -1)
            {
                return Integer.IsEvenInteger(value.Numerator);
            }
            return false;
        }

        public static bool IsFinite(BigFraction value)
        {
            return value.Denominator != 0;
        }

        public static bool IsImaginaryNumber(BigFraction value) => false;

        public static bool IsInfinity(BigFraction value) => false;

        public static bool IsInteger(BigFraction value)
        {
            value.Reduce();
            return (value.Denominator == 1 || value.Denominator == -1);
        }

        public static bool IsNaN(BigFraction value)
        {
            return value.Denominator == 0;
        }

        public static bool IsNegative(BigFraction value)
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

        public static bool IsNegativeInfinity(BigFraction value) => false;

        public static bool IsNormal(BigFraction value) => value != 0;

        public static bool IsOddInteger(BigFraction value)
        {
            value.Reduce();
            if (value.Denominator == 1 || value.Denominator == -1)
            {
                return Integer.IsOddInteger(value.Numerator);
            }
            return false;
        }

        public static bool IsPositive(BigFraction value)
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

        public static bool IsPositiveInfinity(BigFraction value) => false;

        public static bool IsRealNumber(BigFraction value)
        {
            return value.Denominator != 0;
        }

        public static bool IsSubnormal(BigFraction value) => false;

        public static bool IsZero(BigFraction value)
        {
            if (value.Denominator == 0)
            {
                return false;
            }
            return value.Numerator.IsZero;
        }

        public static BigFraction MaxMagnitude(BigFraction x, BigFraction y)
        {
            BigFraction ax = Abs(x);
            BigFraction ay = Abs(y);

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

        public static BigFraction MaxMagnitudeNumber(BigFraction x, BigFraction y)
        {
            return MaxMagnitude(x, y);
        }

        public static BigFraction MinMagnitude(BigFraction x, BigFraction y)
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

        public static BigFraction MinMagnitudeNumber(BigFraction x, BigFraction y)
        {
            return MinMagnitude(x, y);
        }

        public static BigFraction Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public static BigFraction Parse(string s, NumberStyles style, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public static BigFraction Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public static BigFraction Parse(string s, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out BigFraction result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out BigFraction result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out BigFraction result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out BigFraction result)
        {
            throw new NotImplementedException();
        }

        static BigFraction ISpanParsable<BigFraction>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        static BigFraction IParsable<BigFraction>.Parse(string s, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigFraction>.TryConvertFromChecked<TOther>(TOther value, out BigFraction result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigFraction>.TryConvertFromSaturating<TOther>(TOther value, out BigFraction result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigFraction>.TryConvertFromTruncating<TOther>(TOther value, out BigFraction result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigFraction>.TryConvertToChecked<TOther>(BigFraction value, out TOther result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigFraction>.TryConvertToSaturating<TOther>(BigFraction value, out TOther result)
        {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigFraction>.TryConvertToTruncating<TOther>(BigFraction value, out TOther result)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
            {
                return 1;
            }
            if (!(obj is BigFraction))
            {
                throw new ArgumentException();
            }
            return CompareTo((BigFraction)obj);
        }

        public int CompareTo(BigFraction other)
        {
            var numerator = this.Numerator;// インスタンスの値を変更しないようにthisのは複製
            numerator *= other.Denominator;
            other.Numerator *= this.Denominator;
            return numerator.CompareTo(other.Numerator);
        }

        public bool Equals(BigFraction other)
        {
            var numerator = Numerator;// インスタンスの値を変更しないようにthisのは複製
            numerator *= other.Denominator;
            other.Numerator *= Denominator;
            return numerator.Equals(other.Numerator);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public static BigFraction operator +(BigFraction value)
        {
            throw new NotImplementedException();
        }

        public static BigFraction operator +(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public static BigFraction operator -(BigFraction value)
        {
            throw new NotImplementedException();
        }

        public static BigFraction operator -(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public static BigFraction operator ++(BigFraction value)
        {
            throw new NotImplementedException();
        }

        public static BigFraction operator --(BigFraction value)
        {
            throw new NotImplementedException();
        }

        public static BigFraction operator *(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public static BigFraction operator /(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public static BigFraction operator %(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public static bool operator !=(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public static bool operator <(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public static bool operator >(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public static bool operator <=(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public static bool operator >=(BigFraction left, BigFraction right)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is BigFraction))
            {
                throw new ArgumentException();
            }
            return Equals((BigFraction)obj);
        }

        public override int GetHashCode()
        {
            return Numerator.GetHashCode() ^ Denominator.GetHashCode();
        }
    }
}
