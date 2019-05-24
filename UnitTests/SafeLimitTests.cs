using BinaryMemoryReaderWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class SafeLimitTests
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
                    catch (Exception e)
                    {
                        Assert.Fail("Should have thrown an OutOfMemoryException.");
                    }
                }
            }
        }
    }
}
