﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFast.BinaryMemoryReaderWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class ManagedCompatibilityTests
    {
        //[TestMethod]
        //public unsafe void MixedRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //        {
        //            writer.Write("abcABCäöüÄÖÜßáÁàÀ♥♦♣♠");
        //            writer.Write((byte)66);
        //            writer.Write(0x48484848);
        //            writer.Write(0x84848484U);
        //            writer.Write("abcABCäöüÄÖÜßáÁàÀ♥♦♣♠");
        //        }

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        Assert.AreEqual(reader.ReadString(), "abcABCäöüÄÖÜßáÁàÀ♥♦♣♠", "BinaryMemoryReader String incompatible to BinaryReader.");
        //        Assert.AreEqual(reader.ReadByte(), (byte)66, "BinaryMemoryReader Byte incompatible to BinaryReader.");
        //        Assert.AreEqual(reader.ReadInt32(), 0x48484848, "BinaryMemoryReader Int incompatible to BinaryReader.");
        //        Assert.AreEqual(reader.ReadUInt32(), 0x84848484U, "BinaryMemoryReader UInt incompatible to BinaryReader.");
        //        Assert.AreEqual(reader.ReadString(), "abcABCäöüÄÖÜßáÁàÀ♥♦♣♠", "BinaryMemoryReader 2nd String incompatible to BinaryReader.");
        //    }
        //}

        [TestMethod]
        public unsafe void MixedWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            writer.Write("abcABCäöüÄÖÜßáÁàÀ♥♦♣♠");
            writer.Write((byte)66);
            writer.Write(0x48484848);
            writer.Write(0x84848484U);
            writer.Write("abcABCäöüÄÖÜßáÁàÀ♥♦♣♠");

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                Assert.AreEqual(reader.ReadString(), "abcABCäöüÄÖÜßáÁàÀ♥♦♣♠", "BinaryMemoryReader String incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadByte(), (byte)66, "BinaryMemoryReader Byte incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadInt32(), 0x48484848, "BinaryMemoryReader Int incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadUInt32(), 0x84848484U, "BinaryMemoryReader UInt incompatible to BinaryReader.");
                Assert.AreEqual(reader.ReadString(), "abcABCäöüÄÖÜßáÁàÀ♥♦♣♠", "BinaryMemoryReader 2nd String incompatible to BinaryReader.");
            }
        }

        //[TestMethod]
        //public unsafe void StringRead()
        //{
        //    foreach (int size in new int[] { 1, 127, 128, 16383, 16384, 2097151, 2097152, 268435455, 268435456, 300000000 })
        //    {
        //        byte[] data;

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (BinaryWriter writer = new BinaryWriter(ms))
        //                writer.Write(new string('A', size));

        //            data = ms.ToArray();
        //        }

        //        fixed (byte* pData = data)
        //        {
        //            BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //            Assert.AreEqual(reader.ReadString(), new string('A', size), "BinaryMemoryReader String incompatible to BinaryReader.");
        //        }
        //    }
        //}

        //[TestMethod]
        //public unsafe void StringNonNullRead()
        //{
        //    foreach (int size in new int[] { 1, 127, 128, 16383, 16384, 2097151, 2097152, 268435455, 268435456, 300000000 })
        //    {
        //        byte[] data;

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (BinaryWriter writer = new BinaryWriter(ms))
        //                writer.Write(new string('A', size));

        //            data = ms.ToArray();
        //        }

        //        fixed (byte* pData = data)
        //        {
        //            BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //            Assert.AreEqual(reader.ReadStringNonNull(), new string('A', size), "BinaryMemoryReader String incompatible to BinaryReader.");
        //        }
        //    }
        //}

        [TestMethod]
        public unsafe void StringWrite()
        {
            foreach (int size in new int[] { 1, 127, 128, 16383, 16384, 2097151, 2097152, 268435455, 268435456, 300000000 })
            {
                ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

                writer.Write(new string('A', size));

                using (MemoryStream ms = new MemoryStream(writer.ToArray()))
                using (BinaryReader reader = new BinaryReader(ms))
                    Assert.AreEqual(reader.ReadString(), new string('A', size), "BinaryMemoryWriter String incompatible to BinaryWriter.");
            }
        }

        [TestMethod]
        public unsafe void BooleanWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write(count % 2 == 0);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadBoolean(), count % 2 == 0, "BinaryMemoryWriter Boolean incompatible to BinaryWriter.");
        }

        [TestMethod]
        public unsafe void ByteWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write((byte)count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadByte(), (byte)count, "BinaryMemoryWriter Byte incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void ByteRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write((byte)count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadByte(), (byte)count, "BinaryMemoryReader Byte incompatible to BinaryWriter.");
        //    }
        //}

        [TestMethod]
        public unsafe void SByteWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write((sbyte)count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadSByte(), (sbyte)count, "BinaryMemoryWriter SByte incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void SByteRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write((sbyte)count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadSByte(), (sbyte)count, "BinaryMemoryReader SByte incompatible to BinaryWriter.");
        //    }
        //}

        [TestMethod]
        public unsafe void ShortWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write((short)count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadInt16(), (short)count, "BinaryMemoryWriter Short incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void ShortRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write((short)count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadInt16(), (short)count, "BinaryMemoryReader Short incompatible to BinaryWriter.");
        //    }
        //}

        [TestMethod]
        public unsafe void UShortWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write((ushort)count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadUInt16(), (ushort)count, "BinaryMemoryWriter UShort incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void UShortRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write((ushort)count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadUInt16(), (ushort)count, "BinaryMemoryReader UShort incompatible to BinaryWriter.");
        //    }
        //}

        [TestMethod]
        public unsafe void IntWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write(count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadInt32(), count, "BinaryMemoryWriter Int incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void IntRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write(count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadInt32(), count, "BinaryMemoryReader Int incompatible to BinaryWriter.");
        //    }
        //}

        [TestMethod]
        public unsafe void UIntWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write((uint)count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadUInt32(), (uint)count, "BinaryMemoryWriter UInt incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void UIntRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write((uint)count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadUInt32(), (uint)count, "BinaryMemoryReader UInt incompatible to BinaryWriter.");
        //    }
        //}

        [TestMethod]
        public unsafe void LongWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write((long)count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadInt64(), (long)count, "BinaryMemoryWriter Long incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void LongRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write((long)count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadInt64(), (long)count, "BinaryMemoryReader Long incompatible to BinaryWriter.");
        //    }
        //}

        [TestMethod]
        public unsafe void ULongWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write((ulong)count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadUInt64(), (ulong)count, "BinaryMemoryWriter ULong incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void ULongRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write((ulong)count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadUInt64(), (ulong)count, "BinaryMemoryReader ULong incompatible to BinaryWriter.");
        //    }
        //}

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

            List<TimeSpan> timeSpans = new List<TimeSpan>();
            foreach (long timeLong in timeLongs)
                timeSpans.Add(new TimeSpan(timeLong));


            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();
            foreach (TimeSpan timeSpan in timeSpans)
                writer.WriteCompressed(timeSpan);

            byte[] data = writer.ToArray();
            Assert.AreEqual(required, data.Length, "The created array has the wrong size.");

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                foreach (TimeSpan timeSpan in timeSpans)
                    Assert.AreEqual(timeSpan, reader.ReadCompressedTimeSpan(), "BinaryMemoryReader TimeSpan Compressed incompatible to BinaryMemoryWriter.");
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

            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int c = 0; c < 40; c++)
                foreach (KeyValuePair<ulong, int> pair in pairs)
                    writer.Write7BitEncoded(pair.Key);

            data = writer.ToArray();

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                for (int c = 0; c < 40; c++)
                    foreach (KeyValuePair<ulong, int> pair in pairs)
                        Assert.AreEqual(pair.Key, reader.Read7BitEncoded(), $"Didn't read what i've wrote with {pair.Key}, turn {c}.");
            }
        }

        [TestMethod]
        public unsafe void CharWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 0xD800; count++)
                writer.Write((char)count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 0xD800; count++)
                {
                    char c = reader.ReadChar();

                    Assert.AreEqual(c, (char)count, $"BinaryMemoryWriter Char incompatible to BinaryWriter: 0x{count:X04} != 0x{(int)c:X04}.");
                }
        }

        //[TestMethod]
        //public unsafe void CharRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 0xD800; count++)
        //                writer.Write((char)count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 0xD800; count++)
        //        {
        //            char c = reader.ReadChar();

        //            Assert.AreEqual(c, (char)count, $"BinaryMemoryReader Char incompatible to BinaryWriter: 0x{count.ToString("X04")} != 0x{((int)c).ToString("X04")}.");
        //        }
        //    }
        //}

        [TestMethod]
        public unsafe void FloatWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write((float)count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadSingle(), (float)count, "BinaryMemoryWriter Float incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void FloatRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write((float)count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadSingle(), (float)count, "BinaryMemoryReader Float incompatible to BinaryWriter.");
        //    }
        //}

        [TestMethod]
        public unsafe void DoubleWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write((double)count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadDouble(), (double)count, "BinaryMemoryWriter Double incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void DoubleRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write((double)count);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadDouble(), (double)count, "BinaryMemoryReader Double incompatible to BinaryWriter.");
        //    }
        //}

        [TestMethod]
        public unsafe void DecimalWrite()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 256; count++)
                writer.Write(count * 12347822345.34m);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            using (BinaryReader reader = new BinaryReader(ms))
                for (int count = 0; count < 256; count++)
                    Assert.AreEqual(reader.ReadDecimal(), count * 12347822345.34m, "UnsafeBinaryMemoryWriter Decimal incompatible to BinaryWriter.");
        }

        //[TestMethod]
        //public unsafe void DecimalRead()
        //{
        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write(count * 12347822345.34m);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //            Assert.AreEqual(reader.ReadDecimal(), count * 12347822345.34m, "UnsafeBinaryMemoryReader Decimal incompatible to BinaryWriter.");
        //    }
        //}

        //[TestMethod]
        //public unsafe void BytesRead()
        //{
        //    Random rng = new Random();

        //    byte[] src = new byte[1024];
        //    byte[] chk = new byte[1024];

        //    rng.NextBytes(src);

        //    byte[] data;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(ms))
        //            for (int count = 0; count < 256; count++)
        //                writer.Write(src);

        //        data = ms.ToArray();
        //    }

        //    fixed (byte* pData = data)
        //    {
        //        BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

        //        for (int count = 0; count < 256; count++)
        //        {
        //            reader.ReadBytes(chk, 0, 1024);

        //            for (int position = 0; position < 1024; position++)
        //                Assert.AreEqual(chk[position], src[position], $"Invalid content at position {position}.");
        //        }
        //    }
        //}

        [TestMethod]
        public unsafe void BytesWrite()
        {
            Random rng = new Random();

            byte[] src = new byte[1024];
            byte[] chk;

            rng.NextBytes(src);

            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 1; count < 1024; count++)
                writer.WriteBytes(src, 0, 1024 - count);

            using (MemoryStream ms = new MemoryStream(writer.ToArray()))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                    for (int count = 1; count < 1024; count++)
                    {
                        chk = reader.ReadBytes(1024 - count);

                        for (int position = 0; position < 1024 - count; position++)
                            Assert.AreEqual(chk[position], src[position], $"Invalid content at position {position}.");
                    }
            }
        }
    }
}
