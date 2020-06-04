using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFast.BinaryMemoryReaderWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class ManagedInsertionpointTests
    {
        [TestMethod]
        public unsafe void InsertionPointAndLength()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            Assert.AreEqual(writer.Length, 0, "Wrong size.");

            writer.Write(0x55555555);

            Assert.AreEqual(writer.Length, 4, "Wrong size.");

            ManagedBinaryMemoryWriterSegment segment = writer.MakeInsertionpoint(2);

            Assert.AreEqual(writer.Length, 4, "Wrong size.");

            writer.Write(0xAAAAAAAA);
            writer.Write(0xAAAAAAAA);

            Assert.AreEqual(writer.Length, 12, "Wrong size.");

            writer.Flush();

            Assert.AreEqual(writer.Length, 12, "Wrong size.");

            writer.Write(0xAAAAAAAA);
            writer.Write(0xAAAAAAAA);

            Assert.AreEqual(writer.Length, 20, "Wrong size.");

            segment.Write((ushort)0x55AA);

            Assert.AreEqual(writer.Length, 20, "Wrong size.");

            segment.Finish();

            Assert.AreEqual(writer.Length, 22, "Wrong size.");

            writer.Flush();

            Assert.AreEqual(writer.Length, 22, "Wrong size.");

            byte[] data = writer.ToArray();

            Assert.AreEqual(data.Length, 22, "Wrong result size.");

            byte[] shouldBe = new byte[] { 0x55, 0x55, 0x55, 0x55, 0xAA, 0x55, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA };

            for (int position = 0; position < data.Length; position++)
                Assert.AreEqual(data[position], shouldBe[position], $"Value at position {position} is wrong.");
        }

        [TestMethod]
        public unsafe void InsertionPointFail()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            Assert.AreEqual(writer.Length, 0, "Wrong size.");

            writer.Write(0x55555555);

            Assert.AreEqual(writer.Length, 4, "Wrong size.");

            ManagedBinaryMemoryWriterSegment segment = writer.MakeInsertionpoint(2);

            Assert.AreEqual(writer.Length, 4, "Wrong size.");

            writer.Write(0xAAAAAAAA);
            writer.Write(0xAAAAAAAA);

            Assert.AreEqual(writer.Length, 12, "Wrong size.");

            writer.Flush();

            Assert.AreEqual(writer.Length, 12, "Wrong size.");

            writer.Write(0xAAAAAAAA);
            writer.Write(0xAAAAAAAA);

            Assert.AreEqual(writer.Length, 20, "Wrong size.");

            try
            {
                segment.Write(0x55AA55AA);

                Assert.Fail("Should have thrown an exception.");
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception is InvalidOperationException, "Wrong exception kind.");
                Assert.AreEqual(exception.Message, "An insertionpoint can't increase in size.", "Wrong exception message.");
            }

            Assert.AreEqual(writer.Length, 20, "Wrong size.");

            segment.Write((ushort)0x55AA);

            Assert.AreEqual(writer.Length, 20, "Wrong size.");

            segment.Finish();

            Assert.AreEqual(writer.Length, 22, "Wrong size.");

            writer.Flush();

            Assert.AreEqual(writer.Length, 22, "Wrong size.");

            byte[] data = writer.ToArray();

            Assert.AreEqual(data.Length, 22, "Wrong result size.");

            byte[] shouldBe = new byte[] { 0x55, 0x55, 0x55, 0x55, 0xAA, 0x55, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA };

            for (int position = 0; position < data.Length; position++)
                Assert.AreEqual(data[position], shouldBe[position], $"Value at position {position} is wrong.");
        }
    }
}