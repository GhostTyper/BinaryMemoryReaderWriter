using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter
{
    /// <summary>
    /// A external data source for writing into the StreamWriter.
    /// </summary>
    public class BinaryStreamWriterExternal : IDisposable
    {
        private BinaryStreamWriterCommand[] commands;
        private int command;

        private GCHandle handle;
        private byte[] data;
        
        private BinaryMemoryWriter @base;

        /// <summary>
        /// The writer you need to use to write to the stream.
        /// </summary>
        public BinaryMemoryWriter Writer;

        internal unsafe BinaryStreamWriterExternal(BinaryStreamWriterCommand[] commands, int command, int size)
        {
            this.commands = commands;
            this.command = command;

            data = new byte[size];
            handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            fixed (byte* bpData = data)
                Writer = new BinaryMemoryWriter(bpData, size);

            @base = Writer;

            commands[command] = new BinaryStreamWriterCommand(data, 0);
        }

        internal unsafe BinaryStreamWriterExternal(BinaryStreamWriterCommand[] commands, int command, byte[] data, int position, int size)
        {
            this.commands = commands;
            this.command = command;

            this.data = data;
            handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            fixed (byte* bpData = data)
                Writer = new BinaryMemoryWriter(bpData + position, size);

            @base = Writer;

            commands[command] = new BinaryStreamWriterCommand(data, position);
        }

        /// <summary>
        /// Cleansup the current external writer.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public unsafe void Dispose()
        {
            if (commands[command].Length != -1)
                throw new InvalidOperationException("External writers can only be finalized once.");

            commands[command].Length = (int)(Writer.Position - @base.Position);

            handle.Free();
        }
    }
}
