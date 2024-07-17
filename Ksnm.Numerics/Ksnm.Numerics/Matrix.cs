using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ksnm.Numerics
{
    public class Matrix<TValue> :
        IAdditionOperators<Matrix<TValue>, Matrix<TValue>, Matrix<TValue>>,
        ISubtractionOperators<Matrix<TValue>, Matrix<TValue>, Matrix<TValue>>,
        IMultiplyOperators<Matrix<TValue>, Matrix<TValue>, Matrix<TValue>>
        where TValue : INumber<TValue>
    {
        #region 定数

        #endregion 定数
        /// <summary>
        /// _arrayに
        /// </summary>
        public enum Status
        {
            None = 0,
            AdditiveIdentity,
        }

        #region フィールド
        /// <summary>
        /// 1次元配列
        /// </summary>
        private TValue[] _array = new TValue[0];
        #endregion フィールド

        #region プロパティ
        Status status;
        public int RowLength = 0;
        public int ColumnLength = 0;
        public int ArrayLength => RowLength * ColumnLength;
        public TValue this[int row, int column]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var index = GetIndex(row, column);
                return _array[index];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                var index = GetIndex(row, column);
                _array[index] = value;
            }
        }
        #endregion プロパティ

        #region コンストラクタ
        public Matrix() { }

        public Matrix(int rowLength, int columnLength)
        {
            RowLength = rowLength;
            ColumnLength = columnLength;
            _array = new TValue[rowLength * columnLength];
        }

        public Matrix(Matrix<TValue> other) : this(other.RowLength, other.ColumnLength)
        {
            Array.Copy(other._array, _array, ArrayLength);
        }

        public Matrix(Matrix4x4 other)
        {
            _array = new TValue[4 * 4];
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    var index = GetIndex(r, c);
                    _array[index] = TValue.CreateChecked(other[r, c]);
                }
            }
        }
        #endregion コンストラクタ

        #region Get
        public int GetIndex(int row, int column)
        {
            return row * ColumnLength + column;
        }
        public IEnumerable<TValue> GetColumnItems(int column)
        {
            for (int r = 0; r < RowLength; r++)
            {
                var index = GetIndex(r, column);
                yield return _array[index];
            }
        }
        public IEnumerable<TValue> GetRowItems(int row)
        {
            for (int c = 0; c < ColumnLength; c++)
            {
                var index = GetIndex(row, c);
                yield return _array[index];
            }
        }
        #endregion Get

        #region operators
        public static Matrix<TValue> operator +(Matrix<TValue> left, Matrix<TValue> right)
        {
            var rowLength = System.Math.Min(left.RowLength, right.RowLength);
            var columnLength = System.Math.Min(left.ColumnLength, right.ColumnLength);
            var temp = new Matrix<TValue>(rowLength, columnLength);

            for (int r = 0; r < rowLength; r++)
            {
                for (int c = 0; c < columnLength; c++)
                {
                    var index = temp.GetIndex(r, c);
                    temp._array[index] = left[r, c] + right[r, c];
                }
            }
            return temp;
        }

        public static Matrix<TValue> operator -(Matrix<TValue> left, Matrix<TValue> right)
        {
            var rowLength = System.Math.Min(left.RowLength, right.RowLength);
            var columnLength = System.Math.Min(left.ColumnLength, right.ColumnLength);
            var temp = new Matrix<TValue>(rowLength, columnLength);

            for (int r = 0; r < rowLength; r++)
            {
                for (int c = 0; c < columnLength; c++)
                {
                    var index = temp.GetIndex(r, c);
                    temp._array[index] = left[r, c] - right[r, c];
                }
            }
            return temp;
        }

        public static Matrix<TValue> operator *(Matrix<TValue> left, Matrix<TValue> right)
        {
            var rowLength = left.RowLength;
            var columnLength = right.ColumnLength;
            var temp = new Matrix<TValue>(rowLength, columnLength);

            for (int r = 0; r < rowLength; r++)
            {
                var leftRowItems = left.GetRowItems(r);
                for (int c = 0; c < columnLength; c++)
                {
                    var rightColumnItems = right.GetColumnItems(c);
                    var count = Min(leftRowItems.Count(), rightColumnItems.Count());
                    TValue tempValue = TValue.Zero;
                    for (int i = 0; i < count; i++)
                    {
                        tempValue += leftRowItems.ElementAt(i) * rightColumnItems.ElementAt(i);
                    }
                    var index = temp.GetIndex(r, c);
                    temp._array[index] = tempValue;
                }
            }
            return temp;
        }
        #endregion operators

        #region object
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            for (int r = 0; ;)
            {
                stringBuilder.Append("{");
                for (int c = 0; ;)
                {
                    var index = GetIndex(r, c);
                    stringBuilder.Append(_array[index].ToString());
                    // 次へ
                    c++;
                    if (c < ColumnLength)
                    {
                        stringBuilder.Append(",");
                    }
                    else
                    {
                        break;
                    }
                }
                stringBuilder.Append("}");
                r++;
                if (r < RowLength)
                {
                    stringBuilder.Append(",");
                }
                else
                {
                    break;
                }
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Equals((Matrix<TValue>)obj);
        }
        public bool Equals(Matrix<TValue> other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (_array.Length != other._array.Length)
            {
                return false;
            }
            return _array.SequenceEqual(other._array);
        }
        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (var item in _array)
            {
                hashCode = hashCode ^ item.GetHashCode();
            }
            return hashCode;
        }
        #endregion object

        #region private
        private static int Min(int a, int b)
        {
            return System.Math.Min(a, b);
        }
        #endregion private

    }
}
