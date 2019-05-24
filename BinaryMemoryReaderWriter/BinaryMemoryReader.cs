using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BinaryMemoryReaderWriter
{
    /// <summary>
    /// A binary memory reader. This class can be used to read binary data from a pointer.
    /// </summary>
    public unsafe struct BinaryMemoryReader
    {
        private byte* position;
        private int size;

        private const string spaceError = "Not enough space to complete read operation.";

        /// <summary>
        /// Initializes an UNSAFE binary memory reader.
        /// </summary>
        /// <param name="position">The position you want to start reading from.</param>
        /// <param name="size">The remaining bytes we can read from the given position onwards.</param>
        public BinaryMemoryReader(byte* position, int size)
        {
            this.position = position;
            this.size = size;
        }

        /// <summary>
        /// The remaining bytes we can read.
        /// </summary>
        public int Size => size;


        /// <summary>
        /// Reads a string encoded in UTF-8 with 7 bit encoded length prefix.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns null if the string is empty.</remarks>
        public string ReadString()
        {
            int length;
            string result;

            if (size <= 0)
                throw new OutOfMemoryException(spaceError);

            if (*position == 0x00)
            {
                position++;
                size--;

                return null;
            }

            if ((*position & 0x80) == 0x00)
            {
                length = *position;

                if (size < length + 1)
                    throw new OutOfMemoryException(spaceError);

                position++;
                size--;

                result = Encoding.UTF8.GetString(position, size);

                position += length;
                size -= length;

                return result;
            }

            if (size < 2)
                throw new OutOfMemoryException(spaceError);

            if (((*(position + 1)) & 0x80) == 0x00)
            {
                length = (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7);

                if (size < length + 2)
                    throw new OutOfMemoryException(spaceError);

                position += 2;
                size -= 2;

                result = Encoding.UTF8.GetString(position, size);

                position += length;
                size -= length;

                return result;
            }

            if (size < 3)
                throw new OutOfMemoryException(spaceError);

            if (((*(position + 2)) & 0x80) == 0x00)
            {
                length = (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7) | ((*(position + 2) & 0x7F) << 14);

                if (size < length + 3)
                    throw new OutOfMemoryException(spaceError);

                position += 3;
                size -= 3;

                result = Encoding.UTF8.GetString(position, size);

                position += length;
                size -= length;

                return result;
            }

            if (size < 4)
                throw new OutOfMemoryException(spaceError);

            if (((*(position + 3)) & 0x80) == 0x00)
            {
                length = (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7) | ((*(position + 2) & 0x7F) << 14) | ((*(position + 3) & 0x7F) << 21);

                if (size < length + 4)
                    throw new OutOfMemoryException(spaceError);

                position += 4;
                size -= 4;

                result = Encoding.UTF8.GetString(position, size);

                position += length;
                size -= length;

                return result;
            }

            if (size < 5)
                throw new OutOfMemoryException(spaceError);

            if (((*(position + 4)) & 0x80) == 0x00)
            {
                length = (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7) | ((*(position + 2) & 0x7F) << 14) | ((*(position + 3) & 0x7F) << 21) | ((*(position + 4) & 0x0F) << 28);

                if (length < 0)
                    throw new System.IO.InvalidDataException("Ambiguous length information.");

                if (size < length + 5)
                    throw new OutOfMemoryException(spaceError);

                position += 5;
                size -= 5;

                result = Encoding.UTF8.GetString(position, size);

                position += length;
                size -= length;

                return result;
            }

            throw new OutOfMemoryException(spaceError);
        }

        /// <summary>
        /// Reads a string encoded in UTF-8 with 7 bit encoded length prefix.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns an empty string if the string is empty and not null.</remarks>
        public string ReadStringNonNull()
        {
            return ReadString() ?? "";
        }

        /// <summary>
        /// Reads a byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte()
        {
            if (size < 1)
                throw new OutOfMemoryException(spaceError);

            size--;
            return *position++;
        }

        /// <summary>
        /// Reads a signed byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadSByte()
        {
            if (size < 1)
                throw new OutOfMemoryException(spaceError);

            size--;
            return *(sbyte*)position++;
        }

        /// <summary>
        /// Reads an unsigned short.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUInt16()
        {
            if (size < 2)
                throw new OutOfMemoryException(spaceError);

            size -= 2;
            position += 2;

            return *(ushort*)(position - 2);
        }

        /// <summary>
        /// Reads a short.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadInt16()
        {
            if (size < 2)
                throw new OutOfMemoryException(spaceError);

            size -= 2;
            position += 2;

            return *(short*)(position - 2);
        }

        /// <summary>
        /// Reads a 3 byte integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt24()
        {
            if (size < 3)
                throw new OutOfMemoryException(spaceError);

            size -= 3;

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
            if (size < 4)
                throw new OutOfMemoryException(spaceError);

            size -= 4;
            position += 4;

            return *(uint*)(position - 4);
        }

        /// <summary>
        /// Reads a int.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32()
        {
            if (size < 4)
                throw new OutOfMemoryException(spaceError);

            size -= 4;
            position += 4;

            return *(int*)(position - 4);
        }

        /// <summary>
        /// Reads an unsigned long.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64()
        {
            if (size < 8)
                throw new OutOfMemoryException(spaceError);

            size -= 8;
            position += 8;

            return *(ulong*)(position - 8);
        }

        /// <summary>
        /// Reads a long.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadInt64()
        {
            if (size < 8)
                throw new OutOfMemoryException(spaceError);

            size -= 8;
            position += 8;

            return *(long*)(position - 8);
        }

        /// <summary>
        /// Reads a timespan.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TimeSpan ReadTimeSpan()
        {
            if (size < 8)
                throw new OutOfMemoryException(spaceError);

            size -= 8;
            position += 8;

            return new TimeSpan(*(long*)(position - 8));
        }

        /// <summary>
        /// Reads a datetime.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime ReadDateTime()
        {
            if (size < 8)
                throw new OutOfMemoryException(spaceError);

            size -= 8;
            position += 8;

            return new DateTime(*(long*)(position - 8));
        }
    }
}
