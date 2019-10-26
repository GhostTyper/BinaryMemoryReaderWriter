using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter
{
    /// <summary>
    /// A binary memory writer. This class can be used to write binary data to a pointer.
    /// </summary>
    public unsafe struct BinaryMemoryWriter
    {
        private byte* position;
        private int size;

        private const string spaceError = "Not enough space to complete write operation.";

        /// <summary>
        /// Initializes a binary memory writer.
        /// </summary>
        /// <param name="position">The position you want to start writing.</param>
        /// <param name="size">The remaining bytes we can write to the given position onwards.</param>
        public BinaryMemoryWriter(byte* position, int size)
        {
            this.position = position;
            this.size = size;
        }

        /// <summary>
        /// The remaining bytes we can write to.
        /// </summary>
        public int Size => size;

        /// <summary>
        /// The position of the writer.
        /// </summary>
        public byte* Position => position;

        /// <summary>
        /// Jumps step bytes forward.
        /// </summary>
        /// <param name="step">The amount of bytes to jump.</param>
        public void Jump(int step)
        {
            if (step < 0)
                throw new ArgumentException("step can't be negative.", "step");

            if (size < step)
                throw new OutOfMemoryException(spaceError);

            position += step;
            size -= step;
        }

        /// <summary>
        /// Writes a string in UTF-8 encoding with 7 bit encoded length prefix.
        /// </summary>
        /// <param name="text">The string to write.</param>
        public void Write(string text)
        {
            if (text == null)
            {
                if (size < 1)
                    throw new OutOfMemoryException(spaceError);

                *(position++) = 0x00;
                size--;

                return;
            }

            int length = Encoding.UTF8.GetByteCount(text);

            if (length < 128)
            {
                if (size < length + 1)
                    throw new OutOfMemoryException(spaceError);

                *(position++) = (byte)length;

                fixed (char* chars = text)
                    Encoding.UTF8.GetBytes(chars, text.Length, position, size - 1);

                size -= length + 1;
            }
            else if (length < 16384)
            {
                if (size < length + 2)
                    throw new OutOfMemoryException(spaceError);

                *(position++) = (byte)(length | 0x80);
                *(position++) = (byte)(length >> 7);

                fixed (char* chars = text)
                    Encoding.UTF8.GetBytes(chars, text.Length, position, size - 2);

                size -= length + 2;
            }
            else if (length < 2097152)
            {
                if (size < length + 3)
                    throw new OutOfMemoryException(spaceError);

                *(position++) = (byte)(length | 0x80);
                *(position++) = (byte)((length >> 7) | 0x80);
                *(position++) = (byte)(length >> 14);

                fixed (char* chars = text)
                    Encoding.UTF8.GetBytes(chars, text.Length, position, size - 3);

                size -= length + 3;
            }
            else if (length < 268435456)
            {
                if (size < length + 4)
                    throw new OutOfMemoryException(spaceError);

                *(position++) = (byte)(length | 0x80);
                *(position++) = (byte)((length >> 7) | 0x80);
                *(position++) = (byte)((length >> 14) | 0x80);
                *(position++) = (byte)(length >> 21);

                fixed (char* chars = text)
                    Encoding.UTF8.GetBytes(chars, text.Length, position, size - 4);

                size -= length + 4;
            }
            else
            {
                if (size < length + 5)
                    throw new OutOfMemoryException(spaceError);

                *(position++) = (byte)(length | 0x80);
                *(position++) = (byte)((length >> 7) | 0x80);
                *(position++) = (byte)((length >> 14) | 0x80);
                *(position++) = (byte)((length >> 21) | 0x80);
                *(position++) = (byte)(length >> 28);

                fixed (char* chars = text)
                    Encoding.UTF8.GetBytes(chars, text.Length, position, size - 5);

                size -= length + 5;
            }

            position += length;
        }

        /// <summary>
        /// Writes a string in UTF-8 encoding.
        /// </summary>
        /// <param name="text">The string to write.</param>
        /// <returns>The number of bytes written.</returns>
        public int WriteVanillaString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            if (size < Encoding.UTF8.GetByteCount(text))
                throw new OutOfMemoryException(spaceError);

            int length;

            fixed (char* chars = text)
                length = Encoding.UTF8.GetBytes(chars, text.Length, position, size);

            size -= length;
            position += length;

            return length;
        }

        /// <summary>
        /// Writes a boolean.
        /// </summary>
        /// <param name="data">The byte to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(bool data)
        {
            if (size < 1)
                throw new OutOfMemoryException(spaceError);

            *position++ = data ? (byte)0xFF : (byte)0x00;
            size--;
        }

        /// <summary>
        /// Writes a byte.
        /// </summary>
        /// <param name="data">The byte to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(byte data)
        {
            if (size < 1)
                throw new OutOfMemoryException(spaceError);

            *position++ = data;
            size--;
        }

        /// <summary>
        /// Writes a signed byte.
        /// </summary>
        /// <param name="data">The signed byte to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(sbyte data)
        {
            if (size < 1)
                throw new OutOfMemoryException(spaceError);

            *(sbyte*)position++ = data;
            size--;
        }

        /// <summary>
        /// Writes an unsigned short.
        /// </summary>
        /// <param name="data">The unsigned short to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ushort data)
        {
            if (size < 2)
                throw new OutOfMemoryException(spaceError);

            *(ushort*)position = data;

            position += 2;
            size -= 2;
        }

        /// <summary>
        /// Writes a short.
        /// </summary>
        /// <param name="data">The short to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(short data)
        {
            if (size < 2)
                throw new OutOfMemoryException(spaceError);

            *(short*)position = data;

            position += 2;
            size -= 2;
        }

        /// <summary>
        /// Writes a 3 byte integer.
        /// </summary>
        /// <param name="data">The integer to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteUInt24(int data)
        {
            if (size < 3)
                throw new OutOfMemoryException(spaceError);

            *position++ = (byte)(data / 65536);
            *(ushort*)position = (ushort)data;

            position += 2;
            size -= 3;
        }

        /// <summary>
        /// Writes an unsigned int.
        /// </summary>
        /// <param name="data">The unsigned integer to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(uint data)
        {
            if (size < 4)
                throw new OutOfMemoryException(spaceError);

            *(uint*)position = data;

            position += 4;
            size -= 4;
        }

        /// <summary>
        /// Writes an int.
        /// </summary>
        /// <param name="data">The integer to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int data)
        {
            if (size < 4)
                throw new OutOfMemoryException(spaceError);

            *(int*)position = data;

            position += 4;
            size -= 4;
        }

        /// <summary>
        /// Writes an unsigned long.
        /// </summary>
        /// <param name="data">The unsigned long to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(ulong data)
        {
            if (size < 8)
                throw new OutOfMemoryException(spaceError);

            *(ulong*)position = data;

            position += 8;
            size -= 8;
        }

        /// <summary>
        /// Writes a long.
        /// </summary>
        /// <param name="data">The long to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(long data)
        {
            if (size < 8)
                throw new OutOfMemoryException(spaceError);

            *(long*)position = data;

            position += 8;
            size -= 8;
        }

        /// <summary>
        /// Writes a char.
        /// </summary>
        /// <param name="data">The char to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char data)
        {
            if (data < 128)
            {
                if (size < 1)
                    throw new OutOfMemoryException(spaceError);

                *position = (byte)data;

                size--;
                position++;

                return;
            }

            if (data < 2048)
            {
                if (size < 2)
                    throw new OutOfMemoryException(spaceError);

                *position++ = (byte)((data >> 6) | 0b11000000);
                *position++ = (byte)((data & 0b00111111) | 0b10000000);

                size -= 2;

                return;
            }

            if (size < 3)
                throw new OutOfMemoryException(spaceError);

            *position++ = (byte)((data >> 12) | 0b11100000);
            *position++ = (byte)(((data >> 6) & 0b00111111) | 0b10000000);
            *position++ = (byte)((data & 0b00111111) | 0b10000000);

            size -= 3;
        }

        /// <summary>
        /// Writes a float.
        /// </summary>
        /// <param name="data">The float to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(float data)
        {
            if (size < 4)
                throw new OutOfMemoryException(spaceError);

            *(float*)position = data;

            position += 4;
            size -= 4;
        }

        /// <summary>
        /// Writes a double.
        /// </summary>
        /// <param name="data">The double to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(double data)
        {
            if (size < 8)
                throw new OutOfMemoryException(spaceError);

            *(double*)position = data;

            position += 8;
            size -= 8;
        }

        /// <summary>
        /// Writes a timespan.
        /// </summary>
        /// <param name="data">The timespan to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(TimeSpan data)
        {
            if (size < 8)
                throw new OutOfMemoryException(spaceError);

            *(long*)position = data.Ticks;

            position += 8;
            size -= 8;
        }

        /// <summary>
        /// Writes a datetime.
        /// </summary>
        /// <param name="data">The datetime to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(DateTime data)
        {
            if (size < 8)
                throw new OutOfMemoryException(spaceError);

            *(long*)position = data.Ticks;

            position += 8;
            size -= 8;
        }

        /// <summary>
        /// Writes a decimal.
        /// </summary>
        /// <param name="data">The decimal to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(decimal data)
        {
            if (size < 16)
                throw new OutOfMemoryException(spaceError);

            int[] values = decimal.GetBits(data);

            *(int*)position = values[0];
            position += 4;
            *(int*)position = values[1];
            position += 4;
            *(int*)position = values[2];
            position += 4;
            *(int*)position = values[3];
            position += 4;

            size -= 16;
        }

        /// <summary>
        /// Writes count bytes from the current position into data starting at offset.
        /// </summary>
        /// <param name="data">The byte array where data will be read from.</param>
        /// <param name="offset">The position in the byte array where those data will be read from.</param>
        /// <param name="count">The amount of bytes which will be read.</param>
        /// <remarks>BEWARE: This method is also NOT DOING input checks of the given parameters.</remarks>
        public void WriteBytes(byte[] data, int offset, int count)
        {
            if (data == null)
                throw new ArgumentNullException("data", "data can't be null.");

            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "offset can't be negative.");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "count can't be negative.");

            if (offset + count < data.Length)
                throw new ArgumentOutOfRangeException("count", "offset + count bigger than data.Length.");

            if (size < count)
                throw new OutOfMemoryException(spaceError);

            fixed (byte* pData = data)
                Buffer.MemoryCopy(pData + offset, position, count, count);

            position += count;
            size -= count;
        }

        /// <summary>
        /// Fills the remaining memory with 0x00.
        /// </summary>
        public void Fill()
        {
            while (size > 7)
            {
                *(ulong*)position = 0x0000000000000000;

                size -= 8;
                position += 8;
            }

            while (size-- > 0)
                *position++ = 0x00;
        }

        /// <summary>
        /// Fills the remaining memory with data.
        /// </summary>
        /// <param name="data">The byte to fill teh data.</param>
        public void Fill(byte data)
        {
            if (size > 80)
            {
                ulong oData = ((ulong)data << 56) | ((ulong)data << 48) | ((ulong)data << 40) | ((ulong)data << 32) | ((ulong)data << 24) | ((ulong)data << 16) | ((ulong)data << 8) | (ulong)data;

                while (size > 7)
                {
                    *(ulong*)position = oData;

                    size -= 8;
                    position += 8;
                }
            }

            while (size-- > 0)
                *position++ = data;
        }

        /// <summary>
        /// Fills the remaining memory with random data.
        /// </summary>
        /// <param name="rng">The random number generator to use.</param>
        public void Fill(Random rng)
        {
            for (; size >= 4; position += 4, size -= 4)
                *(int*)position = rng.Next(int.MinValue, int.MaxValue);

            for (; size >= 0; position++, size--)
                *position = (byte)rng.Next(256);
        }

        /// <summary>
        /// Fills the remaining memory with random data.
        /// </summary>
        /// <param name="rng">The random number generator to use.</param>
        public unsafe void Fill(RNGCryptoServiceProvider rng)
        {
            byte[] rData = new byte[size];

            rng.GetBytes(rData);

            fixed (byte* bRData = rData)
                Buffer.MemoryCopy(bRData, position, size, rData.Length);

            position += size;
            size = 0;
        }

        /// <summary>
        /// Fills length bytes memory with data.
        /// </summary>
        /// <param name="data">The byte to fill teh data.</param>
        /// <param name="length">The amount of bytes to fill.</param>
        public void Fill(byte data, int length)
        {
            if (length > size)
                throw new OutOfMemoryException(spaceError);

            size -= length;

            if (length > 80)
            {
                ulong oData = ((ulong)data << 56) | ((ulong)data << 48) | ((ulong)data << 40) | ((ulong)data << 32) | ((ulong)data << 24) | ((ulong)data << 16) | ((ulong)data << 8) | (ulong)data;

                while (length > 7)
                {
                    *(ulong*)position = oData;

                    length -= 8;
                    position += 8;
                }
            }

            while (length-- > 0)
                *position++ = data;
        }

        /// <summary>
        /// Fills length bytes memory with random data.
        /// </summary>
        /// <param name="rng">The random number generator to use.</param>
        /// <param name="length">The amount of bytes to fill.</param>
        public void Fill(Random rng, int length)
        {
            if (length > size)
                throw new OutOfMemoryException(spaceError);

            for (; length >= 4; position += 4, size -= 4, length -= 4)
                *(int*)position = rng.Next(int.MinValue, int.MaxValue);

            for (; length >= 0; position++, size--, length--)
                *position = (byte)rng.Next(256);
        }

        /// <summary>
        /// Fills length bytes memory with random data.
        /// </summary>
        /// <param name="rng">The random number generator to use.</param>
        /// <param name="length">The amount of bytes to fill.</param>
        public void Fill(RNGCryptoServiceProvider rng, int length)
        {
            if (length > size)
                throw new OutOfMemoryException(spaceError);

            byte[] rData = new byte[length];

            rng.GetBytes(rData);

            fixed (byte* bRData = rData)
                Buffer.MemoryCopy(bRData, position, length, rData.Length);

            position += length;
            size -= length;
        }
    }
}
