﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter
{
    /// <summary>
    /// An UNSAFE binary memory reader. This class can be used to read binary data from a pointer.
    /// </summary>
    /// <remarks>Use this class only if you are sure that you won't read over the memory border.</remarks>
    public unsafe struct UnsafeBinaryMemoryReader : IReader
    {
        private byte* position;

        /// <summary>
        /// Initializes an UNSAFE binary memory reader.
        /// </summary>
        /// <param name="position">The position you want to start reading from.</param>
        /// <remarks>Use this class only if you are sure that you won't read over the memory border.</remarks>
        public UnsafeBinaryMemoryReader(byte* position)
        {
            this.position = position;
        }

        /// <summary>
        /// Reads a string encoded in UTF-8 with 7 bit encoded length prefix.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns null if the string is empty.</remarks>
        public string ReadString()
        {
            int length = 0;
            int shift = 0;
            string result;

            while ((*position & 0x80) == 0x80 && shift <= 28)
            {
                length |= (*position++ & 0x7F) << shift;
                shift += 7;
            }

            if (shift > 28)
                throw new System.IO.InvalidDataException("Ambiguous length information.");

            length |= *position++ << shift;

            if (length == 0)
                return null;

            result = Encoding.UTF8.GetString(position, length);

            position += length;

            return result;
        }

        /// <summary>
        /// Reads a string encoded in UTF-8 with 7 bit encoded length prefix.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns an empty string if the string is empty and not null.</remarks>
        public string ReadStringNonNull()
        {
            int length = 0;
            int shift = 0;
            string result;

            while ((*position & 0x80) == 0x80 && shift <= 28)
            {
                length |= (*position++ & 0x7F) << shift;
                shift += 7;
            }

            if (shift > 28)
                throw new System.IO.InvalidDataException("Ambiguous length information.");

            length |= *position++ << shift;

            if (length == 0)
                return "";

            result = Encoding.UTF8.GetString(position, length);

            position += length;

            return result;
        }

        /// <summary>
        /// Reads a string encoded in UTF-8 without leading length and without NUL-termination.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns null if the string is empty.</remarks>
        public string ReadVanillaString(int bytes)
        {
            if (bytes <= 0)
                return null;

            position += bytes;

            return Encoding.UTF8.GetString(position - bytes, bytes);
        }

        /// <summary>
        /// Reads a string encoded in UTF-8 without leading length and without NUL-termination.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns an empty string if the string is empty and not null.</remarks>
        public string ReadVanillaStringNonNull(int bytes)
        {
            return ReadVanillaString(bytes) ?? "";
        }

        /// <summary>
        /// Cuts a sub reader at the current position with the specified length.
        /// </summary>
        /// <param name="size">The size of the cut.</param>
        /// <returns>The new reader.</returns>
        public UnsafeBinaryMemoryReader Cut(int size)
        {
            position += size;

            return new UnsafeBinaryMemoryReader(position - size);
        }

        /// <summary>
        /// Jumps step bytes forward.
        /// </summary>
        /// <param name="step">The amount of bytes to jump.</param>
        /// <remarks>Beware: This method doesn't check for negative values.</remarks>
        public void Jump(int step)
        {
            position += step;
        }

        /// <summary>
        /// The position of the reader.
        /// </summary>
        public byte* Position => position;

        /// <summary>
        /// Reads a bool.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReadBoolean()
        {
            return *position++ != 0x00;
        }

        /// <summary>
        /// Reads a byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte()
        {
            return *position++;
        }

        /// <summary>
        /// Reads a signed byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadSByte()
        {
            return *(sbyte*)position++;
        }

        /// <summary>
        /// Reads an unsigned short.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUInt16()
        {
            position += 2;

            return *(ushort*)(position - 2);
        }

        /// <summary>
        /// Reads a short.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadInt16()
        {
            position += 2;

            return *(short*)(position - 2);
        }

        /// <summary>
        /// Reads a 3 byte integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadUInt24()
        {
            int result = *position++ << 16;

            result |= *(ushort*)position;

            position += 2;

            return result;
        }

        /// <summary>
        /// Reads an unsigned int.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32()
        {
            position += 4;

            return *(uint*)(position - 4);
        }

        /// <summary>
        /// Reads a int.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32()
        {
            position += 4;

            return *(int*)(position - 4);
        }

        /// <summary>
        /// Reads an unsigned long.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64()
        {
            position += 8;

            return *(ulong*)(position - 8);
        }

        /// <summary>
        /// Reads a long.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadInt64()
        {
            position += 8;

            return *(long*)(position - 8);
        }

        /// <summary>
        /// Reads a compressed time span. A compressed time span will wether be stored in 1, 3, 5 or 8 bytes.
        /// This time span needs to be shorter than 7000 years. (If you know it could be bigger: just write
        /// the ticks and read the ticks of a time span.
        /// </summary>        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TimeSpan ReadCompressedTimeSpan()
        {
            long ticks;

            switch (*position & 0b11000000)
            {
                default: // Just so that the compiler won't force us to preinitialize ticks. :D - every operation counts.
                case 0b00000000: // 1 byte length.
                    ticks = *position & 0b00011111;

                    if ((*position++ & 0b00100000) == 0b00100000)
                        ticks = -ticks;
                    break;
                case 0b01000000: // 3 byte length.
                    ticks = (*position & 0b00011111) << 16;
                    ticks |= *(ushort*)(position + 1);

                    if ((*position & 0b00100000) == 0b00100000)
                        ticks = -ticks;

                    position += 3;
                    break;
                case 0b10000000: // 5 byte length.
                    ticks = (*position & 0b00011111L) << 32;
                    ticks |= *(uint*)(position + 1);

                    if ((*position & 0b00100000) == 0b00100000)
                        ticks = -ticks;

                    position += 5;
                    break;
                case 0b11000000: // 8 byte length.
                    ticks = (*position & 0b00011111L) << 56;
                    ticks |= (long)*(uint*)(position + 1) << 24;
                    ticks |= (uint)*(ushort*)(position + 5) << 8;
                    ticks |= *(position + 7);

                    if ((*position & 0b00100000) == 0b00100000)
                        ticks = -ticks;

                    position += 8;
                    break;
            }

            return new TimeSpan(ticks);
        }

        /// <summary>
        /// Reads a 7 bit encoded number.
        /// </summary>
        public ulong Read7BitEncoded()
        {
            ulong result = 0;
            int shift = 0;

            do
            {
                result |= (ulong)(*position & 0x7F) << shift;
                shift += 7;
            } while ((*position++ & 0x80) != 0x00);

            return result;
        }

        /// <summary>
        /// Reads a char.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char ReadChar()
        {
            if ((*position & 0b10000000) == 0b00000000)
            {
                position++;

                return (char)*(position - 1);
            }

            if ((*position & 0b11100000) == 0b11000000)
            {
                position += 2;

                return (char)((*(position - 1) & 0b00111111) | ((*(position - 2) & 0b00011111) << 6));
            }

            if ((*position & 0b11110000) == 0b11100000)
            {
                position += 3;

                return (char)((*(position - 1) & 0b00111111) | ((*(position - 2) & 0b00111111) << 6) | ((*(position - 3) & 0b00001111) << 12));
            }

            throw new InvalidOperationException("Char in memory is too wide for char datatype.");
        }

        /// <summary>
        /// Reads a float.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadSingle()
        {
            position += 4;

            return *(float*)(position - 4);
        }

        /// <summary>
        /// Reads a double.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble()
        {
            position += 8;

            return *(double*)(position - 8);
        }

        /// <summary>
        /// Reads a timespan.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TimeSpan ReadTimeSpan()
        {
            position += 8;

            return new TimeSpan(*(long*)(position - 8));
        }

        /// <summary>
        /// Reads a datetime.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime ReadDateTime()
        {
            position += 8;

            return new DateTime(*(long*)(position - 8));
        }

        /// <summary>
        /// Reads a decimal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal ReadDecimal()
        {
            position += 16;

            return new decimal(new int[] { *(int*)(position - 16), *(int*)(position - 12), *(int*)(position - 8), *(int*)(position - 4) });
        }

        /// <summary>
        /// Reads count bytes from the current position into data starting at offset.
        /// </summary>
        /// <param name="data">The byte array where data will be written to.</param>
        /// <param name="offset">The position in the byte array where those data will be written to.</param>
        /// <param name="count">The amount of bytes which will be written.</param>
        /// <remarks>BEWARE: This method is also NOT DOING input checks of the given parameters.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBytes(byte[] data, int offset, int count)
        {
            fixed (byte* pData = data)
                Buffer.MemoryCopy(position, pData + offset, count, count);

            position += count;
        }

        /// <summary>
        /// Peeks a string encoded in UTF-8 with 7 bit encoded length prefix.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns null if the string is empty.</remarks>
        public string PeekString()
        {
            if (*position == 0x00)
                return null;

            if ((*position & 0x80) == 0x00)
                return Encoding.UTF8.GetString(position + 1, *position);

            if (((*(position + 1)) & 0x80) == 0x00)
                return Encoding.UTF8.GetString(position + 2, (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7));

            if (((*(position + 2)) & 0x80) == 0x00)
                return Encoding.UTF8.GetString(position + 3, (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7) | ((*(position + 2) & 0x7F) << 14));

            if (((*(position + 3)) & 0x80) == 0x00)
                return Encoding.UTF8.GetString(position + 4, (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7) | ((*(position + 2) & 0x7F) << 14) | ((*(position + 3) & 0x7F) << 21));

            if (((*(position + 4)) & 0x80) == 0x00)
                return Encoding.UTF8.GetString(position + 5, (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7) | ((*(position + 2) & 0x7F) << 14) | ((*(position + 3) & 0x7F) << 21) | ((*(position + 4) & 0x0F) << 28));

            return null;
        }

        /// <summary>
        /// Peeks a string encoded in UTF-8 with 7 bit encoded length prefix.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns an empty string if the string is empty and not null.</remarks>
        public string PeekStringNonNull()
        {
            return PeekString() ?? "";
        }

        /// <summary>
        /// Peeks a string encoded in UTF-8 without leading length and without NUL-termination.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns null if the string is empty.</remarks>
        public string PeekVanillaString(int bytes)
        {
            if (bytes <= 0)
                return null;

            return Encoding.UTF8.GetString(position, bytes);
        }

        /// <summary>
        /// Peeks a string encoded in UTF-8 without leading length and without NUL-termination.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns an empty string if the string is empty and not null.</remarks>
        public string PeekVanillaStringNonNull(int bytes)
        {
            return ReadVanillaString(bytes) ?? "";
        }

        /// <summary>
        /// Peeks a boolean.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PeekBoolean()
        {
            return *position != 0x00;
        }

        /// <summary>
        /// Peeks a byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte PeekByte()
        {
            return *position;
        }

        /// <summary>
        /// Peeks a signed byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte PeekSByte()
        {
            return *(sbyte*)position;
        }

        /// <summary>
        /// Peeks an unsigned short.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort PeekUInt16()
        {
            return *(ushort*)position;
        }

        /// <summary>
        /// Peeks a short.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short PeekInt16()
        {
            return *(short*)position;
        }

        /// <summary>
        /// Peeks a 3 byte integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int PeekUInt24()
        {
            return (*position << 16) | *(ushort*)(position + 1);
        }

        /// <summary>
        /// Peeks an unsigned int.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint PeekUInt32()
        {
            return *(uint*)position;
        }

        /// <summary>
        /// Peeks a int.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int PeekInt32()
        {
            return *(int*)position;
        }

        /// <summary>
        /// Peeks an unsigned long.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong PeekUInt64()
        {
            return *(ulong*)position;
        }

        /// <summary>
        /// Peeks a long.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long PeekInt64()
        {
            return *(long*)position;
        }

        /// <summary>
        /// Peeks a 7 bit encoded number.
        /// </summary>
        public ulong Peek7BitEncoded()
        {
            ulong result = 0;
            int shift = 0;

            byte* position = this.position;

            do
            {
                result |= (ulong)(*position & 0x7F) << shift;
                shift += 7;
            } while ((*position++ & 0x80) != 0x00);

            return result;
        }

        /// <summary>
        /// Peeks a char.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char PeekChar()
        {
            if ((*position & 0b10000000) == 0b00000000)
                return (char)*position;

            if ((*position & 0b11100000) == 0b11000000)
                return (char)((*(position + 1) & 0b00111111) | ((*position & 0b00011111) << 6));

            if ((*position & 0b11110000) == 0b11100000)
                return (char)((*(position + 2) & 0b00111111) | ((*(position + 1) & 0b00111111) << 6) | ((*position & 0b00001111) << 12));

            throw new InvalidOperationException("Char in memory is too wide for char datatype.");
        }

        /// <summary>
        /// Peeks a float.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float PeekSingle()
        {
            return *(float*)position;
        }

        /// <summary>
        /// Peeks a double.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double PeekDouble()
        {
            return *(double*)position;
        }

        /// <summary>
        /// Peeks a timespan.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TimeSpan PeekTimeSpan()
        {
            return new TimeSpan(*(long*)position);
        }

        /// <summary>
        /// Peeks a datetime.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime PeekDateTime()
        {
            return new DateTime(*(long*)position);
        }

        /// <summary>
        /// Peeks a decimal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal PeekDecimal()
        {
            return new decimal(new int[] { *(int*)(position), *(int*)(position + 4), *(int*)(position + 8), *(int*)(position + 12) });
        }

        /// <summary>
        /// Reads count bytes from the current position into data starting at offset.
        /// </summary>
        /// <param name="data">The byte array where data will be written to.</param>
        /// <param name="offset">The position in the byte array where those data will be written to.</param>
        /// <param name="count">The amount of bytes which will be written.</param>
        /// <remarks>BEWARE: This method is also NOT DOING input checks of the given parameters.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PeekBytes(byte[] data, int offset, int count)
        {
            fixed (byte* pData = data)
                Buffer.MemoryCopy(position, pData + offset, count, count);
        }
    }
}
