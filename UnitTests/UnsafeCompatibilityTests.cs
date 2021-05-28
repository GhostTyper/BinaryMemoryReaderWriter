using SharpFast.BinaryMemoryReaderWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class UnsafeCompatibilityTests
    {
        [TestMethod]
        public unsafe void MixedRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write("abcABCäöüÄÖÜßáÁàÀ♥♦♣♠");
                    writer.Write((byte)66);
                    writer.Write(0x48484848);
                    writer.Write(0x84848484U);
                    writer.Write("abcABCäöüÄÖÜßáÁàÀ♥♦♣♠");
                }

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                Assert.AreEqual(reader.ReadString(), "abcABCäöüÄÖÜßáÁàÀ♥♦♣♠", "BinaryMemoryReader String incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadByte(), (byte)66, "BinaryMemoryReader Byte incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadInt32(), 0x48484848, "BinaryMemoryReader Int incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadUInt32(), 0x84848484U, "BinaryMemoryReader UInt incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadString(), "abcABCäöüÄÖÜßáÁàÀ♥♦♣♠", "BinaryMemoryReader 2nd String incompatible to BinaryReader.");
            }
        }

        [TestMethod]
        public unsafe void MixedWrite()
        {
            byte[] data = new byte[256];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                writer.Write("abcABCäöüÄÖÜßáÁàÀ♥♦♣♠");
                writer.Write((byte)66);
                writer.Write(0x48484848);
                writer.Write(0x84848484U);
                writer.Write("abcABCäöüÄÖÜßáÁàÀ♥♦♣♠");
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                Assert.AreEqual(reader.ReadString(), "abcABCäöüÄÖÜßáÁàÀ♥♦♣♠", "BinaryMemoryReader String incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadByte(), (byte)66, "BinaryMemoryReader Byte incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadInt32(), 0x48484848, "BinaryMemoryReader Int incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadUInt32(), 0x84848484U, "BinaryMemoryReader UInt incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadString(), "abcABCäöüÄÖÜßáÁàÀ♥♦♣♠", "BinaryMemoryReader 2nd String incompatible to BinaryReader.");
            }
        }

        [TestMethod]
        public unsafe void StringRead()
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
                    UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                    Assert.AreEqual(reader.ReadString(), new string('A', size), "UnsafeBinaryMemoryReader String incompatible to BinaryReader.");
                }
            }
        }

        [TestMethod]
        public unsafe void StringNonNullRead()
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
                    UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                    Assert.AreEqual(reader.ReadStringNonNull(), new string('A', size), "UnsafeBinaryMemoryReader String incompatible to BinaryReader.");
                }
            }
        }

        [TestMethod]
        public unsafe void StringWrite()
        {
            foreach (int size in new int[] { 1, 127, 128, 16383, 16384, 2097151, 2097152, 268435455, 268435456, 300000000 })
            {
                byte[] data = new byte[size + 8];

                fixed (byte* pData = data)
                {
                    UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                    writer.Write(new string('A', size));
                }

                using (MemoryStream ms = new MemoryStream(data))
                using (BinaryReader reader = new BinaryReader(ms))
                    Assert.AreEqual(reader.ReadString(), new string('A', size), "UnsafeBinaryMemoryWriter String incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void BooleanWrite()
        {
            byte[] data = new byte[256];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write(count % 2 == 0);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadBoolean(), count % 2 == 0, "UnsafeBinaryMemoryWriter Boolean incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void BooleanRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write(count % 2 == 0);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadBoolean(), count % 2 == 0, "UnsafeBinaryMemoryReader Boolean incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void ByteWrite()
        {
            byte[] data = new byte[256];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write((byte)count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadByte(), (byte)count, "UnsafeBinaryMemoryWriter Byte incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void ByteRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write((byte)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadByte(), (byte)count, "UnsafeBinaryMemoryReader Byte incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void SByteWrite()
        {
            byte[] data = new byte[256];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write((sbyte)count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadSByte(), (sbyte)count, "UnsafeBinaryMemoryWriter SByte incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void SByteRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write((sbyte)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadSByte(), (sbyte)count, "UnsafeBinaryMemoryReader SByte incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void ShortWrite()
        {
            byte[] data = new byte[512];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write((short)count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadInt16(), (short)count, "UnsafeBinaryMemoryWriter Short incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void ShortRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write((short)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadInt16(), (short)count, "UnsafeBinaryMemoryReader Short incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void UShortWrite()
        {
            byte[] data = new byte[512];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write((ushort)count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadUInt16(), (ushort)count, "UnsafeBinaryMemoryWriter UShort incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void UShortRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write((ushort)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadUInt16(), (ushort)count, "UnsafeBinaryMemoryReader UShort incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void IntWrite()
        {
            byte[] data = new byte[1024];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write(count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadInt32(), count, "UnsafeBinaryMemoryWriter Int incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void IntRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write(count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadInt32(), count, "UnsafeBinaryMemoryReader Int incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void UIntWrite()
        {
            byte[] data = new byte[1024];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write((uint)count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadUInt32(), (uint)count, "UnsafeBinaryMemoryWriter UInt incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void UIntRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write((uint)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadUInt32(), (uint)count, "UnsafeBinaryMemoryReader UInt incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void LongWrite()
        {
            byte[] data = new byte[2048];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write((long)count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadInt64(), (long)count, "UnsafeBinaryMemoryWriter Long incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void LongRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write((long)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadInt64(), (long)count, "UnsafeBinaryMemoryReader Long incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void ULongWrite()
        {
            byte[] data = new byte[2048];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write((ulong)count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadUInt64(), (ulong)count, "UnsafeBinaryMemoryWriter ULong incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void ULongRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write((ulong)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadUInt64(), (ulong)count, "UnsafeBinaryMemoryReader ULong incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void CompressedTimeSpanRead()
        {
            int required = 0;

            List<long> timeLongs = new List<long>()
            {
                //Limits 1 Byte -> 3 Byte
                0, 1, -1, 0x1F, -0x1F, 0x20, -0x20,

                0xFF_FF, -0xFF_FF, 0x01_00_00, -0x01_00_00,

                //Limits 3 Byte -> 5 Byte                
                0x1F_FF_FF, -0x1F_FF_FF, 0x20_00_00, -0x20_00_00,

                0x01_00_00_00, -0x01_00_00_00,
                0xFF_FF_FF_FF, -0xFF_FF_FF_FF,
                0x01_00_00_00_00, -0x01_00_00_00_00,
                0x01_01_00_00_00, -0x01_01_00_00_00,
                0x01_00_01_00_00, -0x01_00_01_00_00,
                0x01_00_00_01_00, -0x01_00_00_01_00,
                0x01_00_00_00_01, -0x01_00_00_00_01,

                //Limits 5 Byte -> 8 Byte
                0x1F_FF_FF_FF_FF, -0x1F_FF_FF_FF_FF, 0x20_00_00_00_00, -0x20_00_00_00_00,

                0x01_00_00_00_00, -0x01_00_00_00_00,
                0xFF_FF_FF_FF_FF, -0xFF_FF_FF_FF_FF,
                0xFF_00_FF_FF_FF, -0xFF_00_FF_FF_FF,
                0xFF_FF_00_FF_FF, -0xFF_FF_00_FF_FF,
                0xFF_FF_FF_00_FF, -0xFF_FF_FF_00_FF,
                0xFF_FF_FF_FF_00, -0xFF_FF_FF_FF_00,
                0x01_00_00_00_00_00, -0x01_00_00_00_00_00,
                0x01_00_00_01_00_00, -0x01_00_00_01_00_00,
                0x01_00_00_00_01_00, -0x01_00_00_00_01_00,
                0x01_00_00_00_00_01, -0x01_00_00_00_00_01,
                0xFF_FF_FF_FF_FF_FF, -0xFF_FF_FF_FF_FF_FF,
                0x01_00_00_00_00_00_00, -0x01_00_00_00_00_00_00,
                0x01_00_00_00_00_01_00, -0x01_00_00_00_00_01_00,
                0x01_00_00_00_00_00_01, -0x01_00_00_00_00_00_01,
                0xFF_FF_FF_FF_FF_FF_FF, -0xFF_FF_FF_FF_FF_FF_FF,
                0x01_00_00_00_00_00_00_00, -0x01_00_00_00_00_00_00_00,
                0x01_00_00_00_00_00_01_00, -0x01_00_00_00_00_00_01_00,
                0x01_00_00_00_00_00_00_01, -0x01_00_00_00_00_00_00_01,

                //Limits -> 8 Byte
                0x1F_FF_FF_FF_FF_FF_FF_FF, -0x1F_FF_FF_FF_FF_FF_FF_FF
            };

            foreach (long timeLong in timeLongs)
            {
                long checkLong = timeLong;
                if (checkLong < 0)
                    checkLong = -checkLong;

                if (checkLong >= 137438953472)
                    required += 8;
                else if (checkLong >= 2097152)
                    required += 5;
                else if (checkLong >= 32)
                    required += 3;
                else
                    required += 1;
            }

            byte[] data = new byte[required];

            List<TimeSpan> timeSpans = new List<TimeSpan>();
            foreach (long timeLong in timeLongs)
                timeSpans.Add(new TimeSpan(timeLong));

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                foreach (TimeSpan timeSpan in timeSpans)
                    writer.WriteCompressed(timeSpan);
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                foreach (TimeSpan timeSpan in timeSpans)
                    Assert.AreEqual(timeSpan, reader.ReadCompressedTimeSpan(), "UnsafeBinaryMemoryReader TimeSpan Compressed incompatible to UnsafeBinaryMemoryWriter.");
            }
        }

        [TestMethod]
        public unsafe void SevenBitEncoded()
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
                data = new byte[pair.Value * 3];

                fixed (byte* pData = data)
                {
                    UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                    writer.Write7BitEncoded(pair.Key);
                    writer.Write7BitEncoded(pair.Key);
                    writer.Write7BitEncoded(pair.Key);

                    UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                    try
                    {
                        Assert.AreEqual(pair.Key, reader.Read7BitEncoded(), $"Didn't read what i've wrote with {pair.Key}.");
                        Assert.AreEqual(pair.Key, reader.Read7BitEncoded(), $"Didn't read what i've wrote with {pair.Key}.");
                        Assert.AreEqual(pair.Key, reader.Read7BitEncoded(), $"Didn't read what i've wrote with {pair.Key}.");
                    }
                    catch
                    {
                        Assert.Fail($"Should not have thrown an Exception, but did with {pair.Key}.");
                    }
                }
            }
        }

        [TestMethod]
        public unsafe void CharWrite()
        {
            byte[] data = new byte[2048];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write((char)count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadChar(), (char)count, "UnsafeBinaryMemoryWriter Char incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void CharRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write((char)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadChar(), (char)count, "UnsafeBinaryMemoryReader Char incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void SingleWrite()
        {
            byte[] data = new byte[2048];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write((float)count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadSingle(), count, "UnsafeBinaryMemoryWriter Float incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void SingleRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write((float)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadSingle(), count, "UnsafeBinaryMemoryReader Float incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void DoubleWrite()
        {
            byte[] data = new byte[2048];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write((double)count);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadDouble(), count, "UnsafeBinaryMemoryWriter Double incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void DoubleRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write((double)count);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadDouble(), count, "UnsafeBinaryMemoryReader Double incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void DecimalWrite()
        {
            byte[] data = new byte[4096];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.Write(count * 12347822345.34m);
            }

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadDecimal(), count * 12347822345.34m, "UnsafeBinaryMemoryWriter Decimal incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void DecimalRead()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    for (int count = 0; count < 256; count++)
                        writer.Write(count * 12347822345.34m);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadDecimal(), count * 12347822345.34m, "UnsafeBinaryMemoryReader Decimal incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void BytesRead()
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
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 256; count++)
                {
                    reader.ReadBytes(chk, 0, 1024);

                    for (int position = 0; position < 1024; position++)
                        Assert.AreEqual(chk[position], src[position], $"Invalid content at position {position}.");
                }
            }
        }

        [TestMethod]
        public unsafe void BytesWrite()
        {
            Random rng = new Random();

            byte[] src = new byte[1024];
            byte[] chk;

            rng.NextBytes(src);

            byte[] data = new byte[262144];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 256; count++)
                    writer.WriteBytes(src, 0, 1024);
            }

            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                    for (int count = 0; count < 256; count++)
                    {
                        chk = reader.ReadBytes(1024);

                        for (int position = 0; position < 1024; position++)
                            Assert.AreEqual(chk[position], src[position], $"Invalid content at position {position}.");
                    }

                data = ms.ToArray();
            }
        }
    }
}