using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryMemoryReaderWriter
{
    public unsafe struct BinaryMemoryReader
    {
        private byte* position;
        private int size;

        public BinaryMemoryReader(byte* position, int size)
        {
            this.position = position;
            this.size = size;
        }

        public int Size => size;

        public string ReadString()
        {
            int length;
            string result;

            if (size <= 0)
                throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");

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
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");

                position++;
                size--;

                result = Encoding.UTF8.GetString(position, size);

                position += length;
                size -= length;

                return result;
            }

            if (size < 2)
                throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");

            if (((*(position + 1)) & 0x80) == 0x00)
            {
                length = (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7);

                if (size < length + 2)
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");

                position += 2;
                size -= 2;

                result = Encoding.UTF8.GetString(position, size);

                position += length;
                size -= length;

                return result;
            }

            if (size < 3)
                throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");

            if (((*(position + 2)) & 0x80) == 0x00)
            {
                length = (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7) | ((*(position + 2) & 0x7F) << 14);

                if (size < length + 3)
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");

                position += 3;
                size -= 3;

                result = Encoding.UTF8.GetString(position, size);

                position += length;
                size -= length;

                return result;
            }

            if (size < 4)
                throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");

            if (((*(position + 3)) & 0x80) == 0x00)
            {
                length = (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7) | ((*(position + 2) & 0x7F) << 14) | ((*(position + 3) & 0x7F) << 21);

                if (size < length + 4)
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");

                position += 4;
                size -= 4;

                result = Encoding.UTF8.GetString(position, size);

                position += length;
                size -= length;

                return result;
            }

            if (size < 5)
                throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");

            if (((*(position + 4)) & 0x80) == 0x00)
            {
                length = (*position & 0x7F) | ((*(position + 1) & 0x7F) << 7) | ((*(position + 2) & 0x7F) << 14) | ((*(position + 3) & 0x7F) << 21) | (((*(position + 4) & 0x7F) << 28) & 0x0F);

                if (length < 0)
                    throw new System.IO.InvalidDataException("Ambiguous length information.");

                if (size < length + 5)
                    throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");

                position += 5;
                size -= 5;

                result = Encoding.UTF8.GetString(position, size);

                position += length;
                size -= length;

                return result;
            }

            throw new OutOfMemoryException("Not enough reserved memory for BinaryMemoryReader.");
        }
    }
}
