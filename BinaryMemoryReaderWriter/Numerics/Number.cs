using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter.Numerics
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    [DebuggerDisplay("Nothing, hahaha")]
    public struct Number // : IComparable<Number>
    {
        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        private ulong hi;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(8)]
        private ulong lo;

        private Number(ulong hi, ulong lo)
        {
            this.hi = hi;
            this.lo = lo;
        }

        private Number(ulong lo)
        {
            hi = 0;
            this.lo = lo;
        }

        public Number(double number)
        {
            if (double.IsNaN(number))
                throw new ArgumentException("Symphony Number doesn't support NaN.", "number");

            if (double.IsPositiveInfinity(number) || number >= 170141183460469231731.687303715884105728)
            {
                hi = 0x7FFFFFFFFFFFFFFF;
                lo = 0xFFFFFFFFFFFFFFFF;

                return;
            }

            if (double.IsNegativeInfinity(number) || number <= -170141183460469231731.687303715884105728)
            {
                hi = 0x8000000000000000;
                lo = 0;

                return;
            }

            hi = 0;
            lo = 0;

            double helper;

            if (number < 0)
                number = -number;

            for (int bit = 126; bit >= 64; bit--)
            {
                helper = Math.Pow(2, bit) / 1000000000000000000.0;

                if (number >= helper)
                {
                    number -= helper;
                    hi |= 1UL << (bit - 64);
                }
            }

            for (int bit = 63; bit >= 0; bit--)
            {
                helper = Math.Pow(2, bit) / 1000000000000000000.0;

                if (number >= helper)
                {
                    number -= helper;
                    lo |= 1UL << bit;
                }
            }
        }

        public Number(decimal number)
        {
            hi = 0;
            lo = 0;
        }

        public static bool operator ==(Number l, Number r)
        {
            return l.hi == r.hi && l.lo == r.lo;
        }

        public static bool operator !=(Number l, Number r)
        {
            return l.hi != r.hi || l.lo != r.lo;
        }

        public static bool operator >(Number l, Number r)
        {
            return l.hi > r.hi || l.lo > r.lo;
        }

        public static bool operator <(Number l, Number r)
        {
            return l.hi < r.hi || l.lo < r.lo;
        }

        public static Number operator +(Number l, Number r)
        {
            Number result = new Number();

            result.lo = l.lo + r.lo;
            result.hi = l.hi + r.hi;

            if (result.lo < l.lo && result.lo < r.lo)
                ++result.hi;

            return result;
        }

        public static Number operator -(Number l, Number r)
        {
            Number result = new Number();

            result.lo = l.lo - r.lo;
            result.hi = l.hi - r.hi;

            if (l.lo < r.lo)
                --l.hi;

            return result;
        }

        public override string ToString()
        {
            double result = 0.0;

            for (int bit = 0; bit < 64; bit++)
                if ((lo & (1UL << bit)) == (1UL << bit))
                    result += Math.Pow(2, bit) / 1000000000000000000.0;

            for (int bit = 0; bit < 64; bit++)
                if ((hi & (1UL << bit)) == (1UL << bit))
                    result += Math.Pow(2, bit + 64) / 1000000000000000000.0;

            return result.ToString();
        }
    }
}
