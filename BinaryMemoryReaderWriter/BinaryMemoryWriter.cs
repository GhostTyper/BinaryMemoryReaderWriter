using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryMemoryReaderWriter
{
    public unsafe struct BinaryMemoryWriter
    {
        private byte* position;
        private int size;

        public BinaryMemoryWriter(byte* position, int size)
        {
            this.position = position;
            this.size = size;
        }

        public void Write(string text)
        {
            if (text == null)
            {
                if (size < 1)
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryWriter.");

                *(position++) = 0x00;
                size--;

                return;
            }

            int length = Encoding.UTF8.GetByteCount(text);

            if (length < 128)
            {
                if (size < length + 1)
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryWriter.");

                *(position++) = (byte)length;
                Encoding.UTF8.GetBytes(text.AsSpan(), new Span<byte>(position, size - 1));

                size -= length + 1;
            }
            else if (length < 16384)
            {
                if (size < length + 2)
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryWriter.");

                *(position++) = (byte)(length | 0x80);
                *(position++) = (byte)(length >> 7);
                Encoding.UTF8.GetBytes(text.AsSpan(), new Span<byte>(position, size - 2));

                size -= length + 2;
            }
            else if (length < 2097152)
            {
                if (size < length + 3)
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryWriter.");

                *(position++) = (byte)(length | 0x80);
                *(position++) = (byte)((length >> 7) | 0x80);
                *(position++) = (byte)(length >> 14);
                Encoding.UTF8.GetBytes(text.AsSpan(), new Span<byte>(position, size - 3));

                size -= length + 3;
            }
            else if (length < 268435456)
            {
                if (size < length + 4)
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryWriter.");

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
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryWriter.");

                *(position++) = (byte)(length | 0x80);
                *(position++) = (byte)((length >> 7) | 0x80);
                *(position++) = (byte)((length >> 14) | 0x80);
                *(position++) = (byte)((length >> 21) | 0x80);
                *(position++) = (byte)(length >> 28);
                Encoding.UTF8.GetBytes(text.AsSpan(), new Span<byte>(position, size - 5));

                size -= length + 5;
            }
        }
    }
}
