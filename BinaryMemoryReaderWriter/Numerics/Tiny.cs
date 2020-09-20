using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace SharpFast.BinaryMemoryReaderWriter.Numerics
{
    /// <summary>
    /// Represents a number between -12.8 and +12.7.
    /// </summary>
    public struct Tiny
    {
        public static readonly Tiny MaxValue = new Tiny() { data = 127 };
        public static readonly Tiny MinValue = new Tiny() { data = -128 };

        private sbyte data;

        public Tiny(double number)
        {
            if (double.IsNaN(number))
                throw new ArgumentException("Can't create Tiny from NaN.", "number");

            if (double.IsPositiveInfinity(number) || number >= 12.7)
            {
                data = sbyte.MaxValue;
                return;
            }

            if (double.IsNegativeInfinity(number) || number <= -12.8)
            {
                data = sbyte.MinValue;
                return;
            }

            data = (sbyte)(number * 10.0);
        }

        public Tiny(float number)
        {
            if (float.IsNaN(number))
                throw new ArgumentException("Can't create Tiny from NaN.", "number");

            if (float.IsPositiveInfinity(number) || number >= 12.7)
            {
                data = sbyte.MaxValue;
                return;
            }

            if (float.IsNegativeInfinity(number) || number <= -12.8)
            {
                data = sbyte.MinValue;
                return;
            }

            data = (sbyte)(number * 10.0);
        }

        public Tiny(int number)
        {
            if (float.IsNaN(number))
                throw new ArgumentException("Can't create Tiny from NaN.", "number");

            if (float.IsPositiveInfinity(number) || number >= 12.7)
            {
                data = sbyte.MaxValue;
                return;
            }

            if (float.IsNegativeInfinity(number) || number <= -12.8)
            {
                data = sbyte.MinValue;
                return;
            }

            data = (sbyte)(number * 10.0);
        }

        /// <summary>
        /// Specifies the raw value.
        /// </summary>
        /// <param name="raw">raw value to use.</param>
        public Tiny(long raw)
        {
            if (raw > 127)
            {
                data = sbyte.MaxValue;
                return;
            }

            if (raw < -128)
            {
                data = sbyte.MinValue;
                return;
            }

            data = (sbyte)raw;
        }

        public static Tiny operator +(Tiny l, Tiny r)
        {
            return new Tiny(l.data + r.data);
        }

        public static Tiny operator -(Tiny l, Tiny r)
        {
            return new Tiny(l.data - r.data);
        }

        public static Tiny operator *(Tiny l, Tiny r)
        {
            return new Tiny(l.data * r.data);
        }

        public static Tiny operator /(Tiny l, Tiny r)
        {
            return new Tiny(l.data / r.data);
        }

        public static Tiny operator %(Tiny l, Tiny r)
        {
            return new Tiny(l.data % r.data);
        }

        public static implicit operator Tiny(double src)
        {
            return new Tiny(src);
        }

        public static implicit operator Tiny(long src)
        {
            return new Tiny(src * 10);
        }

        public static implicit operator double(Tiny src)
        {
            return src.data / 10.0;
        }

        public static explicit operator long(Tiny src)
        {
            if (src.data < 0)
                return (src.data - 5) / 10;
            else
                return (src.data + 5) / 10;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (data < 0)
                builder.Append(CultureInfo.CurrentCulture.NumberFormat.NegativeSign);

            int cData = data > 0 ? data : -data;

            builder.Append((cData / 10).ToString());

            if (cData % 10 != 0)
            {
                builder.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                builder.Append(cData % 10);
            }

            return builder.ToString();
        }
    }
}
