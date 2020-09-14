using SharpFast.BinaryMemoryReaderWriter.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;

namespace SharpFast.Helpers
{
    /// <summary>
    /// Represents an universal number which is doing all internal conversations.
    /// </summary>
    public unsafe struct UniversalNumber
    {
        /// <summary>
        /// The kind of the raw data type.
        /// </summary>
        public readonly NumberKind Kind;

        private fixed byte raw[16];

        /// <summary>
        /// Creates a universal number based on an bool resulting in the storage of an unsigned integer.
        /// </summary>
        /// <param name="number">The integer.</param>
        public UniversalNumber(bool number)
        {
            Kind = NumberKind.UnsignedInteger;

            fixed (byte* bpRaw = raw)
                *(long*)bpRaw = number ? 1 : 0;
        }

        /// <summary>
        /// Creates a universal number based on an byte resulting in the storage of an unsigned integer.
        /// </summary>
        /// <param name="number">The byte to store.</param>
        public UniversalNumber(byte number)
        {
            Kind = NumberKind.UnsignedInteger;

            fixed (byte* bpRaw = raw)
                *(ulong*)bpRaw = number;
        }

        /// <summary>
        /// Creates a universal number based on an sbyte resulting in the storage of an signed integer.
        /// </summary>
        /// <param name="number">The sbyte to store.</param>
        public UniversalNumber(sbyte number)
        {
            Kind = NumberKind.SignedInteger;

            fixed (byte* bpRaw = raw)
                *(long*)bpRaw = number;
        }

        /// <summary>
        /// Creates a universal number based on an ushort resulting in the storage of an unsigned integer.
        /// </summary>
        /// <param name="number">The ushort to store.</param>
        public UniversalNumber(ushort number)
        {
            Kind = NumberKind.UnsignedInteger;

            fixed (byte* bpRaw = raw)
                *(ulong*)bpRaw = number;
        }

        /// <summary>
        /// Creates a universal number based on an short resulting in the storage of an signed integer.
        /// </summary>
        /// <param name="number">The short to store.</param>
        public UniversalNumber(short number)
        {
            Kind = NumberKind.SignedInteger;

            fixed (byte* bpRaw = raw)
                *(long*)bpRaw = number;
        }

        /// <summary>
        /// Creates a universal number based on an uint resulting in the storage of an unsigned integer.
        /// </summary>
        /// <param name="number">The uint to store.</param>
        public UniversalNumber(uint number)
        {
            Kind = NumberKind.UnsignedInteger;

            fixed (byte* bpRaw = raw)
                *(ulong*)bpRaw = number;
        }

        /// <summary>
        /// Creates a universal number based on an int resulting in the storage of an signed integer.
        /// </summary>
        /// <param name="number">The int to store.</param>
        public UniversalNumber(int number)
        {
            Kind = NumberKind.SignedInteger;

            fixed (byte* bpRaw = raw)
                *(long*)bpRaw = number;
        }

        /// <summary>
        /// Creates a universal number based on an ulong resulting in the storage of an unsigned integer.
        /// </summary>
        /// <param name="number">The ulong to store.</param>
        public UniversalNumber(ulong number)
        {
            Kind = NumberKind.UnsignedInteger;

            fixed (byte* bpRaw = raw)
                *(ulong*)bpRaw = number;
        }

        /// <summary>
        /// Creates a universal number based on an long resulting in the storage of an signed integer.
        /// </summary>
        /// <param name="number">The long to store.</param>
        public UniversalNumber(long number)
        {
            Kind = NumberKind.SignedInteger;

            fixed (byte* bpRaw = raw)
                *(long*)bpRaw = number;
        }

        /// <summary>
        /// Creates a universal number based on an float resulting in the storage of an float.
        /// </summary>
        /// <param name="number">The float to store.</param>
        public UniversalNumber(float number)
        {
            Kind = NumberKind.Single;

            fixed (byte* bpRaw = raw)
                *(float*)bpRaw = number;
        }

        /// <summary>
        /// Creates a universal number based on an double resulting in the storage of an double.
        /// </summary>
        /// <param name="number">The double to store.</param>
        public UniversalNumber(double number)
        {
            Kind = NumberKind.Double;

            fixed (byte* bpRaw = raw)
                *(double*)bpRaw = number;
        }

        /// <summary>
        /// Creates a universal number based on an decimal resulting in the storage of an decimal.
        /// </summary>
        /// <param name="number">The decimal to store.</param>
        public UniversalNumber(decimal number)
        {
            Kind = NumberKind.Decimal;

            int[] values = decimal.GetBits(number);

            fixed (byte* bpRaw = raw)
            {
                *(int*)(bpRaw) = values[0];
                *(int*)(bpRaw + 4) = values[1];
                *(int*)(bpRaw + 8) = values[2];
                *(int*)(bpRaw + 12) = values[3];
            }
        }

        /// <summary>
        /// Creates a universal number based on what we detect in the object resulting in the storage of the nearest datatype.
        /// </summary>
        /// <param name="number">The number to store.</param>
        public UniversalNumber(object number)
        {
            if (number == null)
                throw new ArgumentNullException("number", "number can't be null.");

            int[] values;

            switch (number)
            {
                case bool _bool:
                    Kind = NumberKind.UnsignedInteger;

                    fixed (byte* bpRaw = raw)
                        *(long*)bpRaw = _bool ? 1 : 0;
                    break;
                case byte _byte:
                    Kind = NumberKind.UnsignedInteger;

                    fixed (byte* bpRaw = raw)
                        *(ulong*)bpRaw = _byte;
                    break;
                case sbyte _sbyte:
                    Kind = NumberKind.SignedInteger;

                    fixed (byte* bpRaw = raw)
                        *(long*)bpRaw = _sbyte;
                    break;
                case ushort _ushort:
                    Kind = NumberKind.UnsignedInteger;

                    fixed (byte* bpRaw = raw)
                        *(ulong*)bpRaw = _ushort;
                    break;
                case short _short:
                    Kind = NumberKind.SignedInteger;

                    fixed (byte* bpRaw = raw)
                        *(long*)bpRaw = _short;
                    break;
                case uint _uint:
                    Kind = NumberKind.UnsignedInteger;

                    fixed (byte* bpRaw = raw)
                        *(ulong*)bpRaw = _uint;
                    break;
                case int _int:
                    Kind = NumberKind.SignedInteger;

                    fixed (byte* bpRaw = raw)
                        *(long*)bpRaw = _int;
                    break;
                case ulong _ulong:
                    Kind = NumberKind.UnsignedInteger;

                    fixed (byte* bpRaw = raw)
                        *(ulong*)bpRaw = _ulong;
                    break;
                case long _long:
                    Kind = NumberKind.SignedInteger;

                    fixed (byte* bpRaw = raw)
                        *(long*)bpRaw = _long;
                    break;
                case float _float:
                    Kind = NumberKind.Single;

                    fixed (byte* bpRaw = raw)
                        *(float*)bpRaw = _float;
                    break;
                case double _double:
                    Kind = NumberKind.Double;

                    fixed (byte* bpRaw = raw)
                        *(double*)bpRaw = _double;
                    break;
                case decimal _decimal:
                    Kind = NumberKind.Decimal;

                    values = decimal.GetBits(_decimal);

                    fixed (byte* bpRaw = raw)
                    {
                        *(int*)(bpRaw) = values[0];
                        *(int*)(bpRaw + 4) = values[1];
                        *(int*)(bpRaw + 8) = values[2];
                        *(int*)(bpRaw + 12) = values[3];
                    }
                    break;
                case string _string:
                    ulong _pULong;
                    long _pLong;
                    decimal _pDecimal;
                    double _pDouble;

                    if (string.IsNullOrWhiteSpace(_string))
                        throw new ArgumentException("number can't be empty.", "number");

                    if (ulong.TryParse(_string, out _pULong))
                    {
                        Kind = NumberKind.UnsignedInteger;

                        fixed (byte* bpRaw = raw)
                            *(ulong*)bpRaw = _pULong;
                    }
                    else if (long.TryParse(_string, out _pLong))
                    {
                        Kind = NumberKind.SignedInteger;

                        fixed (byte* bpRaw = raw)
                            *(long*)bpRaw = _pLong;
                    }
                    else if (decimal.TryParse(_string, out _pDecimal))
                    {
                        Kind = NumberKind.Decimal;

                        values = decimal.GetBits(_pDecimal);

                        fixed (byte* bpRaw = raw)
                        {
                            *(int*)(bpRaw) = values[0];
                            *(int*)(bpRaw + 4) = values[1];
                            *(int*)(bpRaw + 8) = values[2];
                            *(int*)(bpRaw + 12) = values[3];
                        }
                    }
                    else if (double.TryParse(_string, out _pDouble))
                    {
                        Kind = NumberKind.Double;

                        fixed (byte* bpRaw = raw)
                            *(double*)bpRaw = _pDouble;
                    }
                    else
                        throw new ArgumentException("Can't parse your string into a valid number format.", "number");
                    break;
                default:
                    throw new ArgumentException($"Can't parse format {number.GetType()}.", "number");
            }
        }

        /// <summary>
        /// Returns the number as decimal.
        /// </summary>
        /// <returns>This number as decimal.</returns>
        public decimal AsDecimal()
        {
            fixed (byte* bpRaw = raw)
                switch (Kind)
                {
                    case NumberKind.SignedInteger:
                        return *(long*)bpRaw;
                    case NumberKind.UnsignedInteger:
                        return *(ulong*)bpRaw;
                    case NumberKind.Single:
                        return (decimal)*(float*)bpRaw;
                    case NumberKind.Double:
                        return (decimal)*(double*)bpRaw;
                    case NumberKind.Decimal:
                        return new decimal(new int[] { *(int*)(bpRaw), *(int*)(bpRaw + 4), *(int*)(bpRaw + 8), *(int*)(bpRaw + 12) });
                    default:
                        throw new InvalidDataException("This structure is invalid.");
                }
        }

        public static UniversalNumber operator +(UniversalNumber l, UniversalNumber r)
        {


            return new UniversalNumber();
        }

        public static bool operator ==(UniversalNumber l, float r)
        {
            return l == new UniversalNumber(r);
        }

        public static bool operator !=(UniversalNumber l, float r)
        {
            return l != new UniversalNumber(r);
        }

        public static bool operator ==(UniversalNumber l, double r)
        {
            return l == new UniversalNumber(r);
        }

        public static bool operator !=(UniversalNumber l, double r)
        {
            return l != new UniversalNumber(r);
        }

        public static bool operator ==(UniversalNumber l, decimal r)
        {
            return l == new UniversalNumber(r);
        }

        public static bool operator !=(UniversalNumber l, decimal r)
        {
            return l != new UniversalNumber(r);
        }

        public static bool operator ==(UniversalNumber l, int r)
        {
            return l == new UniversalNumber(r);
        }

        public static bool operator !=(UniversalNumber l, int r)
        {
            return l != new UniversalNumber(r);
        }

        public static bool operator ==(UniversalNumber l, uint r)
        {
            return l == new UniversalNumber(r);
        }

        public static bool operator !=(UniversalNumber l, uint r)
        {
            return l != new UniversalNumber(r);
        }

        /// <summary>
        /// Compares two numbers.
        /// </summary>
        /// <param name="l">The left part.</param>
        /// <param name="r">The right part.</param>
        /// <returns>true, if l and r are equal.</returns>
        public static bool operator ==(UniversalNumber l, UniversalNumber r)
        {
            if (l.Kind == r.Kind)
                switch (l.Kind)
                {
                    case NumberKind.SignedInteger:
                    case NumberKind.UnsignedInteger:
                        return *(long*)l.raw == *(long*)r.raw;
                    case NumberKind.Single:
                        return *(float*)l.raw == *(float*)r.raw;
                    case NumberKind.Double:
                        return *(double*)l.raw == *(double*)r.raw;
                    case NumberKind.Decimal:
                        return new decimal(new int[] { *(int*)l.raw, *(int*)(l.raw + 4), *(int*)(l.raw + 8), *(int*)(l.raw + 12) }) == new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                    default:
                        throw new InvalidDataException("This structure is invalid.");
                }

            switch (l.Kind)
            {
                case NumberKind.SignedInteger:
                    switch (r.Kind)
                    {
                        case NumberKind.UnsignedInteger:
                            if (*(long*)l.raw < 0)
                                return false;

                            return *(long*)l.raw == *(long*)r.raw;
                        case NumberKind.SignedInteger:
                            return *(long*)l.raw == *(long*)r.raw;
                        case NumberKind.Single:
                            return *(long*)l.raw == *(float*)r.raw;
                        case NumberKind.Decimal:
                            return *(long*)l.raw == new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                        default:
                            throw new InvalidDataException("This structure is invalid.");
                    }
                case NumberKind.UnsignedInteger:
                    switch (r.Kind)
                    {
                        case NumberKind.UnsignedInteger:
                            return *(long*)l.raw == *(long*)r.raw;
                        case NumberKind.SignedInteger:
                            if (*(long*)r.raw < 0)
                                return false;

                            return *(long*)l.raw == *(long*)r.raw;
                        case NumberKind.Single:
                            if (*(long*)r.raw < 0)
                                return false;

                            return *(long*)l.raw == *(float*)r.raw;
                        case NumberKind.Decimal:
                            if (*(long*)r.raw < 0)
                                return false;

                            return *(long*)l.raw == new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                        default:
                            throw new InvalidDataException("This structure is invalid.");
                    }
                case NumberKind.Single:
                    switch (r.Kind)
                    {
                        case NumberKind.UnsignedInteger:
                            return *(float*)l.raw == *(long*)r.raw;
                        case NumberKind.SignedInteger:
                            return *(float*)l.raw == *(long*)r.raw;
                        case NumberKind.Single:
                            return *(float*)l.raw == *(float*)r.raw;
                        case NumberKind.Decimal:
                            return (decimal)*(float*)l.raw == new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                        default:
                            throw new InvalidDataException("This structure is invalid.");
                    }
                case NumberKind.Double:
                    switch (r.Kind)
                    {
                        case NumberKind.UnsignedInteger:
                            return *(double*)l.raw == *(long*)r.raw;
                        case NumberKind.SignedInteger:
                            return *(double*)l.raw == *(long*)r.raw;
                        case NumberKind.Single:
                            return *(double*)l.raw == *(float*)r.raw;
                        case NumberKind.Decimal:
                            return (decimal)*(double*)l.raw == new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                        default:
                            throw new InvalidDataException("This structure is invalid.");
                    }
                case NumberKind.Decimal:
                    switch (r.Kind)
                    {
                        case NumberKind.UnsignedInteger:
                            return *(decimal*)l.raw == *(long*)r.raw;
                        case NumberKind.SignedInteger:
                            return *(decimal*)l.raw == *(long*)r.raw;
                        case NumberKind.Single:
                            return *(decimal*)l.raw == (decimal)*(float*)r.raw;
                        case NumberKind.Decimal:
                            return *(decimal*)l.raw == new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                        default:
                            throw new InvalidDataException("This structure is invalid.");
                    }

            }

            return false;
        }

        /// <summary>
        /// Compares two numbers.
        /// </summary>
        /// <param name="l">The left part.</param>
        /// <param name="r">The right part.</param>
        /// <returns>true, if l and r are unequal.</returns>
        public static bool operator !=(UniversalNumber l, UniversalNumber r)
        {
            if (l.Kind == r.Kind)
                switch (l.Kind)
                {
                    case NumberKind.SignedInteger:
                    case NumberKind.UnsignedInteger:
                        return *(long*)l.raw == *(long*)r.raw;
                    case NumberKind.Single:
                        return *(float*)l.raw == *(float*)r.raw;
                    case NumberKind.Double:
                        return *(double*)l.raw == *(double*)r.raw;
                    case NumberKind.Decimal:
                        return new decimal(new int[] { *(int*)l.raw, *(int*)(l.raw + 4), *(int*)(l.raw + 8), *(int*)(l.raw + 12) }) == new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                    default:
                        throw new InvalidDataException("This structure is invalid.");
                }

            switch (l.Kind)
            {
                case NumberKind.SignedInteger:
                    switch (r.Kind)
                    {
                        case NumberKind.UnsignedInteger:
                            if (*(long*)l.raw < 0)
                                return false;

                            return *(long*)l.raw != *(long*)r.raw;
                        case NumberKind.SignedInteger:
                            return *(long*)l.raw != *(long*)r.raw;
                        case NumberKind.Single:
                            return *(long*)l.raw != *(float*)r.raw;
                        case NumberKind.Decimal:
                            return *(long*)l.raw != new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                        default:
                            throw new InvalidDataException("This structure is invalid.");
                    }
                case NumberKind.UnsignedInteger:
                    switch (r.Kind)
                    {
                        case NumberKind.UnsignedInteger:
                            return *(long*)l.raw != *(long*)r.raw;
                        case NumberKind.SignedInteger:
                            if (*(long*)r.raw < 0)
                                return false;

                            return *(long*)l.raw != *(long*)r.raw;
                        case NumberKind.Single:
                            if (*(long*)r.raw < 0)
                                return false;

                            return *(long*)l.raw != *(float*)r.raw;
                        case NumberKind.Decimal:
                            if (*(long*)r.raw < 0)
                                return false;

                            return *(long*)l.raw != new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                        default:
                            throw new InvalidDataException("This structure is invalid.");
                    }
                case NumberKind.Single:
                    switch (r.Kind)
                    {
                        case NumberKind.UnsignedInteger:
                            return *(float*)l.raw != *(long*)r.raw;
                        case NumberKind.SignedInteger:
                            return *(float*)l.raw != *(long*)r.raw;
                        case NumberKind.Single:
                            return *(float*)l.raw != *(float*)r.raw;
                        case NumberKind.Decimal:
                            return (decimal)*(float*)l.raw != new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                        default:
                            throw new InvalidDataException("This structure is invalid.");
                    }
                case NumberKind.Double:
                    switch (r.Kind)
                    {
                        case NumberKind.UnsignedInteger:
                            return *(double*)l.raw != *(long*)r.raw;
                        case NumberKind.SignedInteger:
                            return *(double*)l.raw != *(long*)r.raw;
                        case NumberKind.Single:
                            return *(double*)l.raw != *(float*)r.raw;
                        case NumberKind.Decimal:
                            return (decimal)*(double*)l.raw != new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                        default:
                            throw new InvalidDataException("This structure is invalid.");
                    }
                case NumberKind.Decimal:
                    switch (r.Kind)
                    {
                        case NumberKind.UnsignedInteger:
                            return *(decimal*)l.raw != *(long*)r.raw;
                        case NumberKind.SignedInteger:
                            return *(decimal*)l.raw != *(long*)r.raw;
                        case NumberKind.Single:
                            return *(decimal*)l.raw != (decimal)*(float*)r.raw;
                        case NumberKind.Decimal:
                            return *(decimal*)l.raw != new decimal(new int[] { *(int*)r.raw, *(int*)(r.raw + 4), *(int*)(r.raw + 8), *(int*)(r.raw + 12) });
                        default:
                            throw new InvalidDataException("This structure is invalid.");
                    }
            }

            return false;
        }

        /// <summary>
        /// Returns the string representation.
        /// </summary>
        /// <returns>The number as string.</returns>
        public override string ToString()
        {
            switch (Kind)
            {
                case NumberKind.SignedInteger:
                    fixed (byte* bpRaw = raw)
                        return (*(long*)bpRaw).ToString();
                case NumberKind.UnsignedInteger:
                    fixed (byte* bpRaw = raw)
                        return (*(ulong*)bpRaw).ToString();
                case NumberKind.Single:
                    fixed (byte* bpRaw = raw)
                        return (*(float*)bpRaw).ToString();
                case NumberKind.Double:
                    fixed (byte* bpRaw = raw)
                        return (*(double*)bpRaw).ToString();
                case NumberKind.Decimal:
                    fixed (byte* bpRaw = raw)
                        return new decimal(new int[] { *(int*)(bpRaw), *(int*)(bpRaw + 4), *(int*)(bpRaw + 8), *(int*)(bpRaw + 12) }).ToString();
                default:
                    return "<INVALID>";
            }
        }
    }
}
