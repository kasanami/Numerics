using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksnm.Numerics
{
    internal static class DecimalExtensions
    {
        #region Get*
        /// <summary>
        /// 符号ビットを取得
        /// </summary>
        /// <returns>正なら 0 を返す。負なら 1 を返す。</returns>
        public static byte GetSignBits(this decimal value)
        {
            uint bits = (uint)decimal.GetBits(value)[3];
            return (byte)(bits >> 31);
        }
        /// <summary>
        /// 符号を取得
        /// </summary>
        /// <returns>正なら +1 を返す。負なら -1 を返す。</returns>
        public static int GetSign(this decimal value)
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
        public static byte GetExponentBits(this decimal value)
        {
            int[] bits = decimal.GetBits(value);
            return (byte)((bits[3] >> 16) & 0x7F);
        }
        /// <summary>
        /// 指数を取得
        /// </summary>
        /// <returns>10の基数に累乗する際の指数</returns>
        public static int GetExponent(this decimal value)
        {
            return -value.GetExponentBits();
        }
        /// <summary>
        /// 仮数部を取得
        /// </summary>
        /// <returns>仮数部のビット</returns>
        public static int[] GetMantissaBits(this decimal value)
        {
            int[] bits = decimal.GetBits(value);
            return bits.Take(3).ToArray();
        }
        /// <summary>
        /// 仮数を取得
        /// </summary>
        /// <returns>仮数のみの値</returns>
        public static decimal GetMantissa(this decimal value)
        {
            int[] bits = decimal.GetBits(value);
            return new decimal(bits[0], bits[1], bits[2], false, 0);
        }
        /// <summary>
        /// 小数部を取得
        /// </summary>
        public static decimal GetFractional(this decimal value)
        {
            return decimal.Remainder(value, 1);
        }
        #endregion Get*

    }
}
