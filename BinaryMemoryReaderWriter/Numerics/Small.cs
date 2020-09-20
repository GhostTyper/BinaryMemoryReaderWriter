using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace SharpFast.BinaryMemoryReaderWriter.Numerics
{
    /// <summary>
    /// Represents a number between -327.68 bis 327.67.
    /// </summary>
    public struct Small
    {
        public static readonly Small MaxValue = new Small() { data = 32767 };
        public static readonly Small MinValue = new Small() { data = -32768 };

        private short data;

        public Small(double number)
        {
            if (double.IsNaN(number))
                throw new ArgumentException("Can't create Small from NaN.", "number");

            if (double.IsPositiveInfinity(number) || number >= 327.67)
            {
                data = short.MaxValue;
                return;
            }

            if (double.IsNegativeInfinity(number) || number <= -327.68)
            {
                data = short.MinValue;
                return;
            }

            data = (short)(number * 100.0);
        }

        /// <summary>
        /// Specifies the raw value.
        /// </summary>
        /// <param name="raw">raw value to use.</param>
        public Small(long raw)
        {
            if (raw > 327.67)
            {
                data = short.MaxValue;
                return;
            }

            if (raw < -327.68)
            {
                data = short.MinValue;
                return;
            }

            data = (short)raw;
        }

        public static Small operator +(Small l, Small r)
        {
            return new Small(l.data + r.data);
        }

        public static Small operator -(Small l, Small r)
        {
            return new Small(l.data - r.data);
        }

        public static Small operator *(Small l, Small r)
        {
            return new Small(l.data * r.data);
        }

        public static Small operator /(Small l, Small r)
        {
            return new Small(l.data / r.data);
        }

        public static Small operator %(Small l, Small r)
        {
            return new Small(l.data % r.data);
        }

        public static implicit operator Small(double src)
        {
            return new Small(src);
        }

        public static implicit operator Small(long src)
        {
            return new Small(src * 10);
        }

        public static implicit operator double(Small src)
        {
            return src.data / 10.0;
        }

        public static explicit operator long(Small src)
        {
            if (src.data < 0)
                return (src.data - 5) / 100;
            else
                return (src.data + 5) / 100;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (data < 0)
                builder.Append(CultureInfo.CurrentCulture.NumberFormat.NegativeSign);

            int cData = data > 0 ? data : -data;

            builder.Append((cData / 100).ToString());

            if (cData % 100 != 0)
            {
                builder.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                builder.Append(cData % 100);
            }

            return builder.ToString();
        }
    }
}
