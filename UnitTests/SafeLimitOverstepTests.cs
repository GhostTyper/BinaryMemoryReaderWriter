using SharpFast.BinaryMemoryReaderWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class SafeLimitOverstepTests
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
                    BinaryMemoryReader reader = new BinaryMemoryReader(pData, size - 1);

                    try
                    {
                        reader.ReadString();

                        Assert.Fail("Should have thrown an OutOfMemoryException.");
                    }
                    catch (OutOfMemoryException) { }
                    catch (Exception)
                    {
                        Assert.Fail("Should have thrown an OutOfMemoryException.");
                    }

                    BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, size - 1);

                    try
                    {
                        writer.Write(new string('A', size));

                        Assert.Fail("Should have thrown an OutOfMemoryException.");
                    }
                    catch (OutOfMemoryException) { }
                    catch (Exception)
                    {
                        Assert.Fail("Should have thrown an OutOfMemoryException.");
                    }
                }
            }
        }

        [TestMethod]
        public unsafe void VanillaStringLimits()
        {
            foreach (int size in new int[] { 1, 2, 3, 20, 5000 })
            {
                byte[] data = new byte[size];

                for (int i = 0; i < size; i++)
                    data[i] = 65;

                fixed (byte* pData = data)
                {
                    BinaryMemoryReader reader = new BinaryMemoryReader(pData, size - 1);

                    try
                    {
                        reader.ReadVanillaString(size);

                        Assert.Fail("Should have thrown an OutOfMemoryException.");
                    }
                    catch (OutOfMemoryException) { }
                    catch (Exception)
                    {
                        Assert.Fail("Should have thrown an OutOfMemoryException.");
                    }

                    BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, size - 1);

                    try
                    {
                        writer.WriteVanillaString(new string('A', size));

                        Assert.Fail("Should have thrown an OutOfMemoryException.");
                    }
                    catch (OutOfMemoryException) { }
                    catch (Exception)
                    {
                        Assert.Fail("Should have thrown an OutOfMemoryException.");
                    }
                }
            }
        }

        [TestMethod]
        public unsafe void BooleanLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(true);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadBoolean();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write(true);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadByte();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write((byte)0x55);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadSByte();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write((sbyte)0x55);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadInt16();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write((short)0x5555);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadUInt16();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write((ushort)0x5555);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadInt32();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write(0x55555555);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadUInt32();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write(0x55555555u);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadInt64();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write(0x5555555555555555L);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadUInt64();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write(0x5555555555555555UL);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
            }
        }

        [TestMethod]
        public unsafe void CharLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write('Ä');

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadChar();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write('Ä');

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
            }
        }

        [TestMethod]
        public unsafe void FloatLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(133.7f);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadSingle();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write(133.7f);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
            }
        }

        [TestMethod]
        public unsafe void DoubleLimits()
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                    writer.Write(133.7);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadDouble();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write(133.7);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadDateTime();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write(DateTime.UtcNow);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadTimeSpan();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write(new TimeSpan(23876482734L));

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                    writer.Write(2873645.2345m);

                data = ms.ToArray();
            }

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadDecimal();

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.Write(2873645.2345m);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
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
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length - 1);

                try
                {
                    reader.ReadBytes(src, 0, 64);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }

                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length - 1);

                try
                {
                    writer.WriteBytes(src, 0, 64);

                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
                catch (OutOfMemoryException) { }
                catch (Exception)
                {
                    Assert.Fail("Should have thrown an OutOfMemoryException.");
                }
            }
        }
    }
}
