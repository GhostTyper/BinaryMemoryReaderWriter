using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter
{
    /// <summary>
    /// Represents a helper-class which supports writing byte[] blocks in an optimized way into
    /// the DeflateStream.
    /// </summary>
    public class BinaryStreamWriter
    {
        internal BinaryStreamWriterCommand[] commands;
        private byte[] additionalData;

        private int currentCommand;
        private int currentData;

        /// <summary>
        /// The length of all committed write commands together.
        /// </summary>
        internal long committedLength;

        /// <summary>
        /// Creates an instance of a BinaryStreamWriter.
        /// </summary>
        /// <param name="commandQueue">The size of the command queue.</param>
        /// <param name="additionalData">The size of additional storable data.</param>
        public BinaryStreamWriter(int commandQueue = 131072, int additionalData = 8388608)
        {
            commands = new BinaryStreamWriterCommand[commandQueue];
            this.additionalData = new byte[additionalData];
        }

        /// <summary>
        /// The length of all committed write commands together.
        /// </summary>
        public long CommittedLength => committedLength;

        /// <summary>
        /// Writes various data to the StreamWriter.
        /// </summary>
        /// <param name="size">The maximum amount of data written.</param>
        /// <returns>An external writer manager.</returns>
        /// <exception cref="OutOfMemoryException">Thrown if you try to reserve more memory then available within the additionalData buffer.</exception>
        public BinaryStreamWriterExternal Write(int size)
        {
            if (currentCommand >= commands.Length)
                throw new OutOfMemoryException("commandQueue is full.");

            BinaryStreamWriterExternal result;

            if (size < 65536)
            {
                result = new BinaryStreamWriterExternal(this, currentCommand, additionalData, currentData, size);

                currentCommand++;
                currentData += size;

                if (currentData > additionalData.Length)
                    throw new OutOfMemoryException("additional data space exceedet.");

                return result;
            }

            result = new BinaryStreamWriterExternal(this, currentCommand, size);

            currentCommand++;

            return result;
        }

        /// <summary>
        /// Write pieces of a byte[].
        /// </summary>
        /// <param name="data">The byte[] to write.</param>
        /// <param name="offset">The begin from where data should be written.</param>
        /// <param name="length">The bytes to write.</param>
        public void Write(byte[] data, int offset, int length)
        {
            if (currentCommand >= commands.Length)
                throw new OutOfMemoryException("commandQueue is full.");

            commands[currentCommand++] = new BinaryStreamWriterCommand(data, offset, length);
            committedLength += length;
        }

        /// <summary>
        /// Writes the complete content of the memory stream. If you alter the contents, altered contents may be written.
        /// </summary>
        /// <param name="ms">The memory stream to write.</param>
        /// <exception cref="OutOfMemoryException">Out of memory exception, if the command queue is full or the memory stream is too large.</exception>
        public void Write(MemoryStream ms)
        {
            if (currentCommand >= commands.Length)
                throw new OutOfMemoryException("commandQueue is full.");

            if (ms.Position > int.MaxValue)
                throw new OutOfMemoryException("The memory stream is too large.");

            commands[currentCommand++] = new BinaryStreamWriterCommand(ms.GetBuffer(), 0, (int)ms.Position);
            committedLength += ms.Position;
        }

        /// <summary>
        /// Pushes all data to the given stream.
        /// </summary>
        /// <param name="stream">The stream where to push data.</param>
        public void PushToStream(Stream stream)
        {
            for (int position = 0; position < currentCommand; position++)
                if (commands[position].Length > 0)
                    stream.Write(commands[position].Data, commands[position].Offset, commands[position].Length);
        }
    }
}
