using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace SharpFast.BinaryMemoryReaderWriter.Numerics
{
    /// <summary>
    /// Represents a number between -2147483.648 bis 2147483.647.
    /// </summary>
    public struct Medium : IComparable<Medium>
    {
        public static readonly Medium MaxValue = new Medium() { data = 2147483647 };
        public static readonly Medium MinValue = new Medium() { data = -2147483648 };

        private int data;

        public Medium(double number)
        {
            if (double.IsNaN(number))
                throw new ArgumentException("Can't create Tiny from NaN.", "number");

            if (double.IsPositiveInfinity(number) || number >= 2147483.647)
            {
                data = int.MaxValue;
                return;
            }

            if (double.IsNegativeInfinity(number) || number <= -2147483.648)
            {
                data = int.MinValue;
                return;
            }

            data = (int)(number * 1000.0);
        }

        /// <summary>
        /// Specifies the raw value.
        /// </summary>
        /// <param name="raw">raw value to use.</param>
        private Medium(long raw)
        {
            if (raw > 2147483647)
            {
                data = int.MaxValue;
                return;
            }

            if (raw < -2147483648)
            {
                data = int.MinValue;
                return;
            }

            data = (int)raw;
        }

        // == != < > <= >=

        public static Medium operator +(Medium l, Medium r)
        {
            long result = (long)l.data + r.data;

            if (result > int.MaxValue)
                return MaxValue;

            if (result < int.MinValue)
                return MinValue;

            return new Medium(result);
        }

        public static Medium operator -(Medium l, Medium r)
        {
            return new Medium(l.data - r.data);
        }

        public static Medium operator *(Medium l, Medium r)
        {
            return new Medium(l.data * r.data);
        }

        public static Medium operator /(Medium l, Medium r)
        {
            return new Medium(l.data / r.data);
        }

        public static Medium operator %(Medium l, Medium r)
        {
            return new Medium(l.data % r.data);
        }

        public static implicit operator Medium(double src)
        {
            return new Medium(src);
        }

        public static implicit operator Medium(long src)
        {
            return new Medium(src * 1000);
        }

        public static implicit operator double(Medium src)
        {
            return src.data / 1000.0;
        }

        public static explicit operator long(Medium src)
        {
            if (src.data < 0)
                return (src.data - 5) / 1000;
            else
                return (src.data + 5) / 1000;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (data < 0)
                builder.Append(CultureInfo.CurrentCulture.NumberFormat.NegativeSign);

            int cData = data > 0 ? data : -data;

            builder.Append((cData / 1000).ToString());

            if (cData % 1000 != 0)
            {
                builder.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                builder.Append(cData % 1000);
            }

            return builder.ToString();
        }

        public int CompareTo(Medium other)
        {
            return other.data - data;
        }
    }
}
