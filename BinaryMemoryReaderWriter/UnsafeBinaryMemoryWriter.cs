using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace BinaryMemoryReaderWriter
{
    /// <summary>
    /// An UNSAFE binary memory writer. This class can be used to write binary data to a pointer.
    /// </summary>
    /// <remarks>Use this class only if you are sure that you won't write over the memory border.</remarks>
    public unsafe struct UnsafeBinaryMemoryWriter
    {
        private byte* position;

        private const string spaceError = "Not enough space to complete write operation.";

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
    }
}
