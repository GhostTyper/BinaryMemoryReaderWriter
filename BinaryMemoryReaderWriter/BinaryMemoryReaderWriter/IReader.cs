using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter
{
    /// <summary>
    /// A generic reader interface for BinaryMemoryReaderWriters.
    /// </summary>
    public interface IReader
    {
        /// <summary>
        /// Reads a string encoded in UTF-8 with 7 bit encoded length prefix.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns null if the string is empty.</remarks>
        string ReadString();

        /// <summary>
        /// Reads a string encoded in UTF-8 with 7 bit encoded length prefix.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns an empty string if the string is empty and not null.</remarks>
        string ReadStringNonNull();

        /// <summary>
        /// Reads a string encoded in UTF-8 without leading length and without NUL-termination.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns null if the string is empty.</remarks>
        string ReadVanillaString(int bytes);

        /// <summary>
        /// Reads a string encoded in UTF-8 without leading length and without NUL-termination.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns an empty string if the string is empty and not null.</remarks>
        string ReadVanillaStringNonNull(int bytes);

        /// <summary>
        /// Reads a boolean.
        /// </summary>
        bool ReadBoolean();

        /// <summary>
        /// Reads a byte.
        /// </summary>
        byte ReadByte();

        /// <summary>
        /// Reads a signed byte.
        /// </summary>
        sbyte ReadSByte();

        /// <summary>
        /// Jumps step bytes forward.
        /// </summary>
        /// <param name="step">The amount of bytes to jump.</param>
        void Jump(int step);

        /// <summary>
        /// Reads an unsigned short.
        /// </summary>
        ushort ReadUInt16();

        /// <summary>
        /// Reads a short.
        /// </summary>
        short ReadInt16();

        /// <summary>
        /// Reads a 3 byte integer.
        /// </summary>
        int ReadUInt24();

        /// <summary>
        /// Reads an unsigned int.
        /// </summary>
        uint ReadUInt32();

        /// <summary>
        /// Reads a int.
        /// </summary>
        int ReadInt32();

        /// <summary>
        /// Reads an unsigned long.
        /// </summary>
        ulong ReadUInt64();

        /// <summary>
        /// Reads a compressed time span. A compressed time span will wether be stored in 1, 3, 5 or 8 bytes.
        /// This time span needs to be shorter than 7000 years. (If you know it could be bigger: just write
        /// the ticks and read the ticks of a time span.
        /// </summary>
        TimeSpan ReadCompressedTimeSpan();

        /// <summary>
        /// Reads a 7 bit encoded number.
        /// </summary>
        ulong Read7BitEncoded();

        /// <summary>
        /// Reads a long.
        /// </summary>
        long ReadInt64();

        /// <summary>
        /// Reads a float.
        /// </summary>
        float ReadSingle();

        /// <summary>
        /// Reads a char.
        /// </summary>
        char ReadChar();

        /// <summary>
        /// Reads a double.
        /// </summary>
        double ReadDouble();

        /// <summary>
        /// Reads a timespan.
        /// </summary>
        TimeSpan ReadTimeSpan();

        /// <summary>
        /// Reads a datetime.
        /// </summary>
        DateTime ReadDateTime();

        /// <summary>
        /// Reads a decimal.
        /// </summary>
        decimal ReadDecimal();

        /// <summary>
        /// Reads count bytes from the current position into data starting at offset.
        /// </summary>
        /// <param name="data">The byte array where data will be written to.</param>
        /// <param name="offset">The position in the byte array where those data will be written to.</param>
        /// <param name="count">The amount of bytes which will be written.</param>
        void ReadBytes(byte[] data, int offset, int count);

        /// <summary>
        /// Peeks a string encoded in UTF-8 with 7 bit encoded length prefix.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns null if the string is empty.</remarks>
        string PeekString();

        /// <summary>
        /// Peeks a string encoded in UTF-8 with 7 bit encoded length prefix.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns an empty string if the string is empty and not null.</remarks>
        string PeekStringNonNull();

        /// <summary>
        /// Peeks a string encoded in UTF-8 without leading length and without NUL-termination.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns null if the string is empty.</remarks>
        string PeekVanillaString(int bytes);

        /// <summary>
        /// Peeks a string encoded in UTF-8 without leading length and without NUL-termination.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>Returns an empty string if the string is empty and not null.</remarks>
        string PeekVanillaStringNonNull(int bytes);

        /// <summary>
        /// Peeks a boolean.
        /// </summary>
        bool PeekBoolean();

        /// <summary>
        /// Peeks a byte.
        /// </summary>
        byte PeekByte();

        /// <summary>
        /// Peeks a signed byte.
        /// </summary>
        sbyte PeekSByte();

        /// <summary>
        /// Peeks an unsigned short.
        /// </summary>
        ushort PeekUInt16();

        /// <summary>
        /// Peeks a short.
        /// </summary>
        short PeekInt16();

        /// <summary>
        /// Peeks a 3 byte integer.
        /// </summary>
        int PeekUInt24();

        /// <summary>
        /// Peeks an unsigned int.
        /// </summary>
        uint PeekUInt32();

        /// <summary>
        /// Peeks a int.
        /// </summary>
        int PeekInt32();

        /// <summary>
        /// Peeks an unsigned long.
        /// </summary>
        ulong PeekUInt64();

        /// <summary>
        /// Peeks a 7 bit encoded number.
        /// </summary>
        ulong Peek7BitEncoded();

        /// <summary>
        /// Peeks a long.
        /// </summary>
        long PeekInt64();

        /// <summary>
        /// Peeks a char.
        /// </summary>
        char PeekChar();

        /// <summary>
        /// Peeks a float.
        /// </summary>
        float PeekSingle();

        /// <summary>
        /// Peeks a double.
        /// </summary>
        double PeekDouble();

        /// <summary>
        /// Peeks a timespan.
        /// </summary>
        TimeSpan PeekTimeSpan();

        /// <summary>
        /// Peeks a datetime.
        /// </summary>
        DateTime PeekDateTime();

        /// <summary>
        /// Peeks a decimal.
        /// </summary>
        decimal PeekDecimal();

        /// <summary>
        /// Peeks count bytes from the current position into data starting at offset.
        /// </summary>
        /// <param name="data">The byte array where data will be written to.</param>
        /// <param name="offset">The position in the byte array where those data will be written to.</param>
        /// <param name="count">The amount of bytes which will be written.</param>
        void PeekBytes(byte[] data, int offset, int count);
    }
}
