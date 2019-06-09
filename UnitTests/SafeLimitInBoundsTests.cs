using SharpFast.BinaryMemoryReaderWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class SafeLimitInBoundsTests
    {
        [TestMethod]
        public unsafe void StringLimits()
        {
            foreach (int size in new int[] { 0, 1, 2, 126, 127, 128, 129, 16382, 16383, 16384, 16385, 2097150, 2097151, 2097152, 2097153, 268435454, 268435455, 268435456, 268435457 })
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

                    try
                    {
                        reader.ReadString();
                    }
                    catch
                    {
                        Assert.Fail("Should not have thrown an Exception.");
                    }

                    BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                    try
                    {
                        writer.Write(new string('A', size));
                    }
                    catch
                    {
                        Assert.Fail("Should not have thrown an Exception.");
                    }
                }
            }
        }

        [TestMethod]
        public unsafe void ByteLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write((byte)0x55);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadByte();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write((byte)0x55);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void SByteLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write((sbyte)0x55);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadSByte();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write((sbyte)0x55);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void ShortLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write((short)0x5555);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadInt16();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write((short)0x5555);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void UShortLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write((ushort)0x5555);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadUInt16();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write((ushort)0x5555);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void IntLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(0x55555555);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadInt32();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write(0x55555555);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void UIntLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(0x55555555u);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadUInt32();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write(0x55555555u);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void LongLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(0x5555555555555555L);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadInt64();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write(0x5555555555555555L);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void ULongLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(0x5555555555555555UL);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadUInt64();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write(0x5555555555555555UL);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void DateTimeLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(DateTime.UtcNow.Ticks);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadDateTime();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write(DateTime.UtcNow);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void TimeSpanLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(DateTime.UtcNow.Ticks);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadTimeSpan();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write(new TimeSpan(23876482734L));
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void DecimalLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(1827364.2134324m);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadDecimal();
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.Write(1827364.2134324m);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }

        [TestMethod]
        public unsafe void BytesLimits()
        {
            Random rng = new Random();

            byte[] src = new byte[64];

            rng.NextBytes(src);

            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(src);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                try
                {
                    reader.ReadBytes(src, 0, 64);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                try
                {
                    writer.WriteBytes(src, 0, 64);
                }
                catch
                {
                    Assert.Fail("Should not have thrown an Exception.");
                }
            }
        }
    }
}
