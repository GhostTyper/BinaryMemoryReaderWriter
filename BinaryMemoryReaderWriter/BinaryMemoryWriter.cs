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
    }
}
