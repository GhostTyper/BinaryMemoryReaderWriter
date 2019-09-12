using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFast.BinaryMemoryReaderWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class ManagementGenerationTests
    {
        [TestMethod]
        public void ToArrayNew()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int i = 0; i < 20000000; i++)
                writer.Write((byte)0xAA);

            byte[] data = writer.ToArray();

            Assert.AreEqual(data.Length, 20000000, "Wrong length of generated data.");

            foreach (byte b in data)
                Assert.AreEqual(b, 0xAA, "Wrong Byte in data.");
        }

        [TestMethod]
        public void ToArrayExisting()
        {
            byte[] data = new byte[40000000];

            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int i = 0; i < 20000000; i++)
                writer.Write((byte)0xAA);

            int size = writer.ToArray(data, 0);

            Assert.AreEqual(size, 20000000, "Wrong length of generated data.");

            for (int position = 0; position < size; position++)
                Assert.AreEqual(data[position], 0xAA, "Wrong Byte in data.");
        }

        [TestMethod]
        public void ToStream()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int i = 0; i < 20000000; i++)
                writer.Write((byte)0xAA);

            MemoryStream stream = new MemoryStream();

            int size = (int)writer.ToStream(stream);

            Assert.AreEqual(size, 20000000, "Wrong length of generated data.");
            Assert.AreEqual(stream.Position, 20000000, "Wrong length of generated data.");

            foreach (byte b in stream.ToArray())
                Assert.AreEqual(b, 0xAA, "Wrong Byte in data.");
        }
    }
}
