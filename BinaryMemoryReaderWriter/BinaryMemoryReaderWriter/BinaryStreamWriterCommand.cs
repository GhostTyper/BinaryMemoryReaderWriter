using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter
{
    struct BinaryStreamWriterCommand
    {
        public readonly byte[] Data;
        public readonly int Offset;
        public int Length;

        public BinaryStreamWriterCommand(byte[] data, int offset)
        {
            Data = data;
            Offset = offset;
            Length = -1;
        }

        public BinaryStreamWriterCommand(byte[] data, int offset, int length)
        {
            Data = data;
            Offset = offset;
            Length = length;
        }
    }
}
