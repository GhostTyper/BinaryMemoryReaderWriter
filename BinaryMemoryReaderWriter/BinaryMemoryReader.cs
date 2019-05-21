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
    }
}
