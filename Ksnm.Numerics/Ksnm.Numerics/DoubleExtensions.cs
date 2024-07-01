using System;
using Float = System.Double;
using UInt = System.UInt64;

namespace Ksnm.Numerics
{
    internal static class DoubleExtensions
    {
        #region 定数
        /// <summary>
        /// 符号部のビット数
        /// </summary>
        public const int SignLength = 1;
        /// <summary>
        /// 指数部のビット数
        /// </summary>
        public const int ExponentLength = 11;
        /// <summary>
        /// 指数部のビットマスク
        /// </summary>
        public const UInt ExponentBitMask = ((UInt)1 << ExponentLength) - 1;
        /// <summary>
        /// 指数部バイアス
        /// </summary>
        public const int ExponentBias = 1023;
        /// <summary>
        /// 無限大を表す指数部
        /// </summary>
        public const int InfinityExponent = 1024;
        /// <summary>
        /// 仮数部のビット数
        /// </summary>
        public const int MantissaLength = 52;
        /// <summary>
        /// 仮数部のビットマスク
        /// </summary>
        public const UInt MantissaBitMask = ((UInt)1 << MantissaLength) - 1;
        /// <summary>
        /// 指数形式ではなく小数形式に変換するためのフォーマット
        /// </summary>
        static readonly string DecimalFormat = "0." + new string('#', 338);
        #endregion 定数

        #region 各部情報の取得
        /// <summary>
        /// 符号なし整数に変換します。
        /// </summary>
        private static UInt _ToBits(this Float value)
        {
            return (UInt)BitConverter.DoubleToInt64Bits(value);
        }
        /// <summary>
        /// 符号ビットを取得
        /// </summary>
        public static byte GetSignBits(this Float value)
        {
            return _GetSignBits(value._ToBits());
        }
        /// <summary>
        /// 符号を取得
        /// </summary>
        public static int GetSign(this Float value)
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
        public static ushort GetExponentBits(this Float value)
        {
            return _GetExponentBits(value._ToBits());
        }
        /// <summary>
        /// 指数を取得
        /// </summary>
        public static int GetExponent(this Float value)
        {
            return _GetExponent(value._ToBits()) - MantissaLength;
        }
        /// <summary>
        /// 仮数部を取得
        /// </summary>
        public static UInt GetMantissaBits(this Float value)
        {
            return _GetMantissaBits(value._ToBits());
        }
        /// <summary>
        /// 仮数を取得
        /// </summary>
        public static UInt GetMantissa(this Float value)
        {
            return _GetMantissa(value._ToBits());
        }
        /// <summary>
        /// 少数部を取得
        /// </summary>
        public static UInt GetFractionalBits(this Float value)
        {
            return _GetFractionalBits(value._ToBits());
        }
        #endregion 各部情報の取得

        #region 各部情報の取得(内部用)
        /// <summary>
        /// 符号ビットを取得
        /// </summary>
        private static byte _GetSignBits(UInt bits)
        {
            return (byte)(bits >> (ExponentLength + MantissaLength));
        }
        /// <summary>
        /// 符号を取得
        /// </summary>
        private static int _GetSign(UInt bits)
        {
            if (_IsNegative(bits))
            {
                return -1;
            }
            return +1;
        }
        /// <summary>
        /// 符号を取得
        /// </summary>
        private static bool _IsNegative(UInt bits)
        {
            return _GetSignBits(bits) == 1;
        }
        /// <summary>
        /// 指数部を取得
        /// </summary>
        private static ushort _GetExponentBits(UInt bits)
        {
            return (ushort)((bits >> MantissaLength) & ExponentBitMask);
        }
        /// <summary>
        /// 指数を取得
        /// </summary>
        private static int _GetExponent(UInt bits)
        {
            return _GetExponentBits(bits) - ExponentBias;
        }
        /// <summary>
        /// 仮数部を取得
        /// </summary>
        private static UInt _GetMantissaBits(UInt bits)
        {
            return bits & MantissaBitMask;
        }
        /// <summary>
        /// 仮数を取得
        /// </summary>
        private static UInt _GetMantissa(UInt bits)
        {
            var mantissaBits = _GetMantissaBits(bits);
            // ((UInt)1 << MantissaLength)は"1."を意味する
            return mantissaBits | ((UInt)1 << MantissaLength);
        }
        /// <summary>
        /// 少数部を取得
        /// </summary>
        private static UInt _GetFractionalBits(UInt bits)
        {
            var shift = _GetExponent(bits);
            if (shift > 0)
            {
                return (_GetMantissaBits(bits) << shift) & MantissaBitMask;
            }
            return _GetMantissaBits(bits);
        }
        #endregion 各部情報の取得(内部用)
    }
}
