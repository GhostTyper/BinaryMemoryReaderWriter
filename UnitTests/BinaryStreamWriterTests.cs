using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFast.BinaryMemoryReaderWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class BinaryStreamWriterTests
    {
        [TestMethod]
        public unsafe void SmallExternals()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter();

            using (BinaryStreamWriterExternal external = writer.Write(16))
                external.Writer.Write(0x11111111);

            using (BinaryStreamWriterExternal external = writer.Write(16))
                external.Writer.Write(0x22222222);

            using (BinaryStreamWriterExternal external = writer.Write(16))
                external.Writer.Write(0x33333333);

            using (BinaryStreamWriterExternal external = writer.Write(16))
                external.Writer.Write(0x44444444);

            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                writer.PushToStream(ms);

                data = ms.ToArray();
            }

            Assert.AreEqual(16, data.Length);

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                Assert.AreEqual(0x11111111, reader.ReadInt32());
                Assert.AreEqual(0x22222222, reader.ReadInt32());
                Assert.AreEqual(0x33333333, reader.ReadInt32());
                Assert.AreEqual(0x44444444, reader.ReadInt32());
            }
        }

        [TestMethod]
        public unsafe void BigExternals()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter();

            using (BinaryStreamWriterExternal external = writer.Write(80000))
                external.Writer.Write(0x11111111);

            using (BinaryStreamWriterExternal external = writer.Write(80000))
                external.Writer.Write(0x22222222);

            using (BinaryStreamWriterExternal external = writer.Write(80000))
                external.Writer.Write(0x33333333);

            using (BinaryStreamWriterExternal external = writer.Write(80000))
                external.Writer.Write(0x44444444);

            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                writer.PushToStream(ms);

                data = ms.ToArray();
            }

            Assert.AreEqual(16, data.Length);

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                Assert.AreEqual(0x11111111, reader.ReadInt32());
                Assert.AreEqual(0x22222222, reader.ReadInt32());
                Assert.AreEqual(0x33333333, reader.ReadInt32());
                Assert.AreEqual(0x44444444, reader.ReadInt32());
            }
        }

        [TestMethod]
        public unsafe void Bytes()
        {
            byte[] src = new byte[] { 0x44, 0x44, 0x44, 0x44, 0x33, 0x33, 0x33, 0x33, 0x22, 0x22, 0x22, 0x22, 0x11, 0x11, 0x11, 0x11 };

            BinaryStreamWriter writer = new BinaryStreamWriter();

            writer.Write(src, 12, 4);
            writer.Write(src, 8, 4);
            writer.Write(src, 4, 4);
            writer.Write(src, 0, 4);

            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                writer.PushToStream(ms);

                data = ms.ToArray();
            }

            Assert.AreEqual(16, data.Length);

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                Assert.AreEqual(0x11111111, reader.ReadInt32());
                Assert.AreEqual(0x22222222, reader.ReadInt32());
                Assert.AreEqual(0x33333333, reader.ReadInt32());
                Assert.AreEqual(0x44444444, reader.ReadInt32());
            }
        }

        [TestMethod]
        public unsafe void MemoryStream()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter();

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(new byte[] { 0x11, 0x11, 0x11, 0x11 }, 0, 4);
                writer.Write(ms);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(new byte[] { 0x22, 0x22, 0x22, 0x22 }, 0, 4);
                writer.Write(ms);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(new byte[] { 0x33, 0x33, 0x33, 0x33 }, 0, 4);
                writer.Write(ms);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(new byte[] { 0x44, 0x44, 0x44, 0x44 }, 0, 4);
                writer.Write(ms);
            }

            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                writer.PushToStream(ms);

                data = ms.ToArray();
            }

            Assert.AreEqual(16, data.Length);

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                Assert.AreEqual(0x11111111, reader.ReadInt32());
                Assert.AreEqual(0x22222222, reader.ReadInt32());
                Assert.AreEqual(0x33333333, reader.ReadInt32());
                Assert.AreEqual(0x44444444, reader.ReadInt32());
            }
        }

        [TestMethod]
        public unsafe void OutOfMemory()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter(5, 3);

            try
            {
                using (BinaryStreamWriterExternal external = writer.Write(2))
                    external.Writer.Write((ushort)0x1111);

                using (BinaryStreamWriterExternal external = writer.Write(1))
                    external.Writer.Write((byte)0x11);

                using (BinaryStreamWriterExternal external = writer.Write(1))
                    external.Writer.Write((byte)0x11);
            }
            catch (OutOfMemoryException)
            {
                return;
            }

            Assert.Fail("Should have thrown an out of memory exception.");
        }

        [TestMethod]
        public unsafe void WithinMemory()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter(5, 3);

            using (BinaryStreamWriterExternal external = writer.Write(2))
                external.Writer.Write((ushort)0x1111);

            using (BinaryStreamWriterExternal external = writer.Write(1))
                external.Writer.Write((byte)0x11);
        }

        [TestMethod]
        public unsafe void OutOfCommandQueue()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter(2, 5);

            try
            {
                using (BinaryStreamWriterExternal external = writer.Write(2))
                    external.Writer.Write((ushort)0x1111);

                using (BinaryStreamWriterExternal external = writer.Write(1))
                    external.Writer.Write((byte)0x11);

                using (BinaryStreamWriterExternal external = writer.Write(1))
                    external.Writer.Write((byte)0x11);
            }
            catch (OutOfMemoryException)
            {
                return;
            }

            Assert.Fail("Should have thrown an out of memory exception.");
        }

        [TestMethod]
        public unsafe void WithinCommandQueue()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter(2, 5);

            using (BinaryStreamWriterExternal external = writer.Write(2))
                external.Writer.Write((ushort)0x1111);

            using (BinaryStreamWriterExternal external = writer.Write(1))
                external.Writer.Write((byte)0x11);
        }

        [TestMethod]
        public unsafe void InsertionPoint()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter();

            using (BinaryStreamWriterExternal insertionPoint = writer.Write(4))
            using (BinaryStreamWriterExternal external = writer.Write(64))
            {
                external.Writer.Write(0x11111111);
                external.Writer.Write(0x22222222);
                external.Writer.Write(0x33333333);
                external.Writer.Write(0x44444444);

                insertionPoint.Writer.Write((short)0x5555);
            }

            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                writer.PushToStream(ms);

                data = ms.ToArray();
            }

            Assert.AreEqual(18, data.Length);

            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                Assert.AreEqual((short)0x5555, reader.ReadInt16());
                Assert.AreEqual(0x11111111, reader.ReadInt32());
                Assert.AreEqual(0x22222222, reader.ReadInt32());
                Assert.AreEqual(0x33333333, reader.ReadInt32());
                Assert.AreEqual(0x44444444, reader.ReadInt32());
            }
        }

        [TestMethod]
        public unsafe void CommittedLength()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter();

            using (BinaryStreamWriterExternal external = writer.Write(16))
                external.Writer.Write(0x11111111);

            Assert.AreEqual(4, writer.CommittedLength);

            writer.Write(new byte[4], 0, 4);

            Assert.AreEqual(8, writer.CommittedLength);

            using (BinaryStreamWriterExternal external = writer.Write(80000))
                external.Writer.Write(0x33333333);

            Assert.AreEqual(12, writer.CommittedLength);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(new byte[] { 0x44, 0x44 }, 0, 2);
                writer.Write(ms);
            }

            Assert.AreEqual(14, writer.CommittedLength);
        }
    }
}
