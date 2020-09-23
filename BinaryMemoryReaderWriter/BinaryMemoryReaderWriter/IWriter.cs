using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter
{
    /// <summary>
    /// A generic writer interface for BinaryMemoryReaderWriters.
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// Writes a string in UTF-8 encoding with 7 bit encoded length prefix.
        /// </summary>
        /// <param name="text">The string to write.</param>
         void Write(string text);

        /// <summary>
        /// Writes a string in UTF-8 encoding without leading length and without NUL-termination.
        /// </summary>
        /// <param name="text">The string to write.</param>
        /// <returns>The number of bytes written.</returns>
         int WriteVanillaString(string text);

        /// <summary>
        /// Writes a boolean.
        /// </summary>
        /// <param name="data">The byte to write.</param>
         void Write(bool data);

        /// <summary>
        /// Writes a byte.
        /// </summary>
        /// <param name="data">The byte to write.</param>
         void Write(byte data);

        /// <summary>
        /// Writes a signed byte.
        /// </summary>
        /// <param name="data">The signed byte to write.</param>
         void Write(sbyte data);

        /// <summary>
        /// Writes an unsigned short.
        /// </summary>
        /// <param name="data">The unsigned short to write.</param>
         void Write(ushort data);

        /// <summary>
        /// Writes a short.
        /// </summary>
        /// <param name="data">The short to write.</param>
         void Write(short data);

        /// <summary>
        /// Writes a 3 byte integer.
        /// </summary>
        /// <param name="data">The integer to write.</param>
         void WriteUInt24(int data);

        /// <summary>
        /// Writes an unsigned int.
        /// </summary>
        /// <param name="data">The unsigned integer to write.</param>
         void Write(uint data);

        /// <summary>
        /// Writes an int.
        /// </summary>
        /// <param name="data">The integer to write.</param>
         void Write(int data);

        /// <summary>
        /// Writes an unsigned long.
        /// </summary>
        /// <param name="data">The unsigned long to write.</param>
         void Write(ulong data);

        /// <summary>
        /// Writes a long.
        /// </summary>
        /// <param name="data">The long to write.</param>
         void Write(long data);

        /// <summary>
        /// Writes an unsigned number 7 bit encoded. (variable length.)
        /// </summary>
        /// <param name="data">The unsigned long to write 7 bit encoded.</param>
         void Write7BitEncoded(ulong data);

        /// <summary>
        /// Writes a char.
        /// </summary>
        /// <param name="data">The char to write.</param>
         void Write(char data);

        /// <summary>
        /// Writes a float.
        /// </summary>
        /// <param name="data">The float to write.</param>
         void Write(float data);

        /// <summary>
        /// Writes a double.
        /// </summary>
        /// <param name="data">The double to write.</param>
         void Write(double data);

        /// <summary>
        /// Writes a timespan.
        /// </summary>
        /// <param name="data">The timespan to write.</param>
         void Write(TimeSpan data);

        /// <summary>
        /// Writes a datetime.
        /// </summary>
        /// <param name="data">The datetime to write.</param>
         void Write(DateTime data);

        /// <summary>
        /// Writes a decimal.
        /// </summary>
        /// <param name="data">The decimal to write.</param>
         void Write(decimal data);

        /// <summary>
        /// Writes count bytes from the current position into data starting at offset.
        /// </summary>
        /// <param name="data">The byte array where data will be read from.</param>
        /// <param name="offset">The position in the byte array where those data will be read from.</param>
        /// <param name="count">The amount of bytes which will be read.</param>
        /// <remarks>BEWARE: This method is also NOT DOING input checks of the given parameters.</remarks>
        void WriteBytes(byte[] data, int offset, int count);
    }
}
