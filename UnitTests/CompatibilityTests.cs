using BinaryMemoryReaderWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class CompatibilityTests
    {
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
                    BinaryMemoryReader reader = new BinaryMemoryReader(pData, data.Length);

                    Assert.AreEqual(reader.ReadString(), new string('A', size), "BinaryMemoryReader String incompatible to BinaryReader.");
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
                    BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, data.Length);

                    writer.Write(new string('A', size));
                }

                using (MemoryStream ms = new MemoryStream(data))
                using (BinaryReader reader = new BinaryReader(ms))
                    Assert.AreEqual(reader.ReadString(), new string('A', size), "BinaryMemoryWriter String incompatible to BinaryWriter.");
            }
        }
    }
}
