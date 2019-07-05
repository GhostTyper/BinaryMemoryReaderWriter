﻿using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter
{
    /// <summary>
    /// An UNSAFE binary memory writer. This class can be used to write binary data to a pointer.
    /// </summary>
    /// <remarks>Use this class only if you are sure that you won't write over the memory border.</remarks>
    public unsafe struct UnsafeBinaryMemoryWriter
    {
        private byte* position;

        /// <summary>
        /// Initializes an UNSAFE binary memory writer.
        /// </summary>
        /// <param name="position">The position you want to start writing.</param>
        /// <remarks>Use this class only if you are sure that you won't write over the memory border.</remarks>
        public UnsafeBinaryMemoryWriter(byte* position)
        {
            this.position = position;
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
        /// Writes a string in UTF-8 encoding with 7 bit encoded length prefix.
        /// </summary>
        /// <param name="text">The string to write.</param>
        public void Write(string text)
        {
            if (text == null)
            {
                *(position++) = 0x00;

                return;
            }

            int length = Encoding.UTF8.GetByteCount(text);

            int splitLength = length;

            while (splitLength > 0)
            {
                if (splitLength >= 128)
                    *(position++) = (byte)(0x80 | splitLength);
                else
                    *(position++) = (byte)splitLength;

                splitLength >>= 7;
            }

            length = Encoding.UTF8.GetBytes(text.AsSpan(), new Span<byte>(position, length));

            position += length;
        }

        /// <summary>
        /// The position of the writer.
        /// </summary>
        public byte* Position => position;

        /// <summary>
        /// Writes a byte.
        /// </summary>
        /// <param name="data">The byte to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(byte data)
        {
            *position++ = data;
        }

        /// <summary>
        /// Writes a signed byte.
        /// </summary>
        /// <param name="data">The signed byte to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(sbyte data)
        {
            *(sbyte*)position++ = data;
        }

        /// <summary>
        /// Writes an unsigned short.
        /// </summary>
        /// <param name="data">The unsigned short to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ushort data)
        {
            *(ushort*)position = data;

            position += 2;
        }

        /// <summary>
        /// Writes a short.
        /// </summary>
        /// <param name="data">The short to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(short data)
        {
            *(short*)position = data;

            position += 2;
        }

        /// <summary>
        /// Writes a 3 byte integer.
        /// </summary>
        /// <param name="data">The integer to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteInt24(int data)
        {
            *position++ = (byte)(data / 65536);
            *(ushort*)position = (ushort)data;

            position += 2;
        }

        /// <summary>
        /// Writes an unsigned int.
        /// </summary>
        /// <param name="data">The unsigned integer to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(uint data)
        {
            *(uint*)position = data;

            position += 4;
        }

        /// <summary>
        /// Writes an int.
        /// </summary>
        /// <param name="data">The integer to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int data)
        {
            *(int*)position = data;

            position += 4;
        }

        /// <summary>
        /// Writes an unsigned long.
        /// </summary>
        /// <param name="data">The unsigned long to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ulong data)
        {
            *(ulong*)position = data;

            position += 8;
        }

        /// <summary>
        /// Writes a long.
        /// </summary>
        /// <param name="data">The long to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(long data)
        {
            *(long*)position = data;

            position += 8;
        }

        /// <summary>
        /// Writes a float.
        /// </summary>
        /// <param name="data">The float to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(float data)
        {
            *(float*)position = data;

            position += 4;
        }

        /// <summary>
        /// Writes a double.
        /// </summary>
        /// <param name="data">The double to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(double data)
        {
            *(double*)position = data;

            position += 8;
        }

        /// <summary>
        /// Writes a timespan.
        /// </summary>
        /// <param name="data">The timespan to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(TimeSpan data)
        {
            *(long*)position = data.Ticks;

            position += 8;
        }

        /// <summary>
        /// Writes a datetime.
        /// </summary>
        /// <param name="data">The datetime to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(DateTime data)
        {
            *(long*)position = data.Ticks;

            position += 8;
        }

        /// <summary>
        /// Writes a decimal.
        /// </summary>
        /// <param name="data">The decimal to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(decimal data)
        {
            int[] values = decimal.GetBits(data);

            *(int*)position = values[0];
            position += 4;
            *(int*)position = values[1];
            position += 4;
            *(int*)position = values[2];
            position += 4;
            *(int*)position = values[3];
            position += 4;
        }

        /// <summary>
        /// Writes count bytes from the current position into data starting at offset.
        /// </summary>
        /// <param name="data">The byte array where data will be read from.</param>
        /// <param name="offset">The position in the byte array where those data will be read from.</param>
        /// <param name="count">The amount of bytes which will be read.</param>
        /// <remarks>BEWARE: This method is also NOT DOING input checks of the given parameters.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBytes(byte[] data, int offset, int count)
        {
            fixed (byte* pData = data)
                Buffer.MemoryCopy(pData + offset, position, count, count);

            position += count;
        }
    }
}
