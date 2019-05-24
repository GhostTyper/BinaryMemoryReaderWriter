using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BinaryMemoryReaderWriter
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
                Encoding.UTF8.GetBytes(text.AsSpan(), new Span<byte>(position, size - 1));

                size -= length + 1;
            }
            else if (length < 16384)
            {
                if (size < length + 2)
                    throw new OutOfMemoryException(spaceError);

                *(position++) = (byte)(length | 0x80);
                *(position++) = (byte)(length >> 7);
                Encoding.UTF8.GetBytes(text.AsSpan(), new Span<byte>(position, size - 2));

                size -= length + 2;
            }
            else if (length < 2097152)
            {
                if (size < length + 3)
                    throw new OutOfMemoryException(spaceError);

                *(position++) = (byte)(length | 0x80);
                *(position++) = (byte)((length >> 7) | 0x80);
                *(position++) = (byte)(length >> 14);
                Encoding.UTF8.GetBytes(text.AsSpan(), new Span<byte>(position, size - 3));

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
                Encoding.UTF8.GetBytes(text.AsSpan(), new Span<byte>(position, size - 4));

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
                Encoding.UTF8.GetBytes(text.AsSpan(), new Span<byte>(position, size - 5));

                size -= length + 5;
            }
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
        public void WriteInt24(int data)
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
    }
}
