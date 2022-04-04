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
        public unsafe void OutOfMemory()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter();

            try
            {
                for (int block = 0; block < 257; block++)
                    using (BinaryStreamWriterExternal external = writer.Write(32768))
                        external.Writer.Write(0x11111111);
            }
            catch (OutOfMemoryException)
            {
                return;
            }

            Assert.Fail("Should have thrown an out of memory exception.");
        }

        [TestMethod]
        public unsafe void OutOfCommandQueue()
        {
            BinaryStreamWriter writer = new BinaryStreamWriter();

            try
            {
                for (int block = 0; block < 131073; block++)
                    using (BinaryStreamWriterExternal external = writer.Write(4))
                        external.Writer.Write(0x00000000);
            }
            catch (OutOfMemoryException)
            {
                return;
            }

            Assert.Fail("Should have thrown an out of memory exception.");
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
    }
}
