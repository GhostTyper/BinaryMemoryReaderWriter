using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using SharpFast.BinaryMemoryReaderWriter;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class SafePeekTests
    {
        [TestMethod]
        public unsafe void StringPeek()
        {
            foreach (int size in new int[] { 1, 127, 128, 16383, 16384, 2097151, 2097152, 268435455, 268435456, 300000000 })
            {
                byte[] data;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(ms))
                        writer.Write(new string('A', size));

                    data = ms.ToArray();
                }

                fixed (byte* pData = data)
                {
                    BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                    Assert.AreEqual(reader.PeekString(), new string('A', size), "BinaryMemoryReader String incompatible to BinaryReader.");
                }
            }
        }

        [TestMethod]
        public unsafe void VanillaStringPeek()
        {
            foreach (int size in new int[] { 1, 2, 3, 20, 5000 })
            {
                byte[] data = new byte[size];

                for (int i = 0; i < size; i++)
                    data[i] = 65;

                fixed (byte* pData = data)
                {
                    BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                    Assert.AreEqual(reader.PeekVanillaString(size), new string('A', size), "BinaryMemoryReader String incompatible to BinaryReader.");
                }
            }
        }

        [TestMethod]
        public unsafe void StringNonNullPeek()
        {
            foreach (int size in new int[] { 1, 127, 128, 16383, 16384, 2097151, 2097152, 268435455, 268435456, 300000000 })
            {
                byte[] data;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(ms))
                        writer.Write(new string('A', size));

                    data = ms.ToArray();
                }

                fixed (byte* pData = data)
                {
                    BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                    Assert.AreEqual(reader.PeekStringNonNull(), new string('A', size), "BinaryMemoryReader String incompatible to BinaryReader.");
                }
            }
        }

        [TestMethod]
        public unsafe void BooleanPeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write(count % 2 == 0);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekBoolean(), true, "BinaryMemoryReader Boolean incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void BytePeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write((byte)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekByte(), (byte)16, "BinaryMemoryReader Byte incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void SBytePeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write((sbyte)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekSByte(), (sbyte)16, "BinaryMemoryReader SByte incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void ShortPeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write((short)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekInt16(), (short)16, "BinaryMemoryReader Short incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void UShortPeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write((ushort)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekUInt16(), (ushort)16, "BinaryMemoryReader UShort incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void IntPeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write(count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekInt32(), 16, "BinaryMemoryReader Int incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void UIntPeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write((uint)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekUInt32(), (uint)16, "BinaryMemoryReader UInt incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void LongPeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write((long)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekInt64(), 16L, "BinaryMemoryReader Long incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void ULongPeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write((ulong)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekUInt64(), 16UL, "BinaryMemoryReader ULong incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void SevenBitEncodedPeek()
        {
            byte[] data;

            List<KeyValuePair<ulong, int>> pairs = new List<KeyValuePair<ulong, int>>();

            pairs.Add(new KeyValuePair<ulong, int>(0L, 1));

            for (int d = 1; d < 10; d++)
            {
                pairs.Add(new KeyValuePair<ulong, int>((1UL << (d * 7)) - 1, d));
                pairs.Add(new KeyValuePair<ulong, int>(1UL << (d * 7), d + 1));
            }

            pairs.Add(new KeyValuePair<ulong, int>(ulong.MaxValue, 10));

            foreach (KeyValuePair<ulong, int> pair in pairs)
            {
                data = new byte[pair.Value];

                fixed (byte* pData = data)
                {
                    BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, pair.Value);

                    writer.Write7BitEncoded(pair.Key);

                    BinaryMemoryReader reader = new BinaryMemoryReader(pData, pair.Value);

                    try
                    {
                        Assert.AreEqual(pair.Key, reader.Peek7BitEncoded(), $"Didn't read what i've wrote with {pair.Key}.");
                        Assert.AreEqual(pair.Key, reader.Peek7BitEncoded(), $"Didn't read what i've wrote with {pair.Key}.");
                    }
                    catch
                    {
                        Assert.Fail($"Should not have thrown an Exception, but did with {pair.Key}.");
                    }
                }
            }
        }

        [TestMethod]
        public unsafe void CharPeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 0xD800; count++)
                        writer.Write((char)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 0; count < 0xD800; count++)
                {
                    Assert.AreEqual(reader.PeekChar(), (char)count, "BinaryMemoryReader Char incompatible to BinaryWriter.");

                    reader.ReadChar();
                }
            }
        }

        [TestMethod]
        public unsafe void FloatPeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write((float)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekSingle(), 16f, "BinaryMemoryReader Float incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void DoublePeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write((double)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekDouble(), 16.0, "BinaryMemoryReader Double incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void DecimalPeek()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 16; count < 256; count++)
                        writer.Write(count * 12347822345.34m);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 16; count < 256; count++)
                    Assert.AreEqual(reader.PeekDecimal(), 16m * 12347822345.34m, "UnsafeBinaryMemoryReader Decimal incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void BytesPeek()
        {
            Random rng = new Random();

            byte[] src = new byte[1024];
            byte[] chk = new byte[1024];

            rng.NextBytes(src);

            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write(src);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int count = 0; count < 256; count++)
                {
                    reader.PeekBytes(chk, 0, 1024);

                    for (int position = 0; position < 1024; position++)
                        Assert.AreEqual(chk[position], src[position], $"Invalid content at position {position}.");
                }
            }
        }
    }
}
