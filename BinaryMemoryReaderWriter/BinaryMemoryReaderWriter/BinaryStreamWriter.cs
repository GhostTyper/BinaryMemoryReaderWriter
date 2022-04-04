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
        private BinaryStreamWriterCommand[] commands;
        private byte[] additionalData;

        private int currentCommand;
        private int currentData;

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
                result = new BinaryStreamWriterExternal(commands, currentCommand, additionalData, currentData, size);

                currentCommand++;
                currentData += size;

                if (currentData >= additionalData.Length)
                    throw new OutOfMemoryException("additional data space exceedet.");

                return result;
            }

            result = new BinaryStreamWriterExternal(commands, currentCommand, size);

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
