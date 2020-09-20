using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFast.BinaryMemoryReaderWriter;
using SharpFast.BinaryMemoryReaderWriter.Numerics;
using SharpFast.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class NumericsTest
    {
        [TestMethod]
        public unsafe void CompareEqualTests()
        {
            {
                Tiny tiny = 5;
                Assert.IsTrue(tiny == 5);
                tiny = 13;
                Assert.IsTrue(tiny == 12.7);
                tiny = -13;
                Assert.IsTrue(tiny == -12.8);

                Assert.ThrowsException<ArgumentException>(new Action(throwsException1));
            }
            {
                Small small = 5;
                Assert.IsTrue(small == 5);
                small = 328;
                Assert.IsTrue(small == new Small(327.67));
                small = -328;
                Assert.IsTrue(small == new Small(-327.68));

                Assert.ThrowsException<ArgumentException>(new Action(throwsException3));
            }
            {
                Medium medium = 5;
                Assert.IsTrue(medium == 5);
                medium = 2147484;
                Assert.IsTrue(medium == new Medium(2147483.647));
                medium = -2147484;
                Assert.IsTrue(medium == new Medium(-2147483.648));

                Assert.ThrowsException<ArgumentException>(new Action(throwsException2));
            }
        }

        public void throwsException1()
        {
            Tiny tiny = double.NaN;
        }

        public void throwsException2()
        {
            Medium medium = double.NaN;
        }

        public void throwsException3()
        {
            Small small = double.NaN;
        }
    }
}
