using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFast.BinaryMemoryReaderWriter;
using SharpFast.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class UniversalNumberTests
    {
        [TestMethod]
        public unsafe void CompareEqualTests()
        {
            {
                UniversalNumber numberInt = new UniversalNumber(1000000000);
                UniversalNumber numberUInt = new UniversalNumber((uint)1000000000);
                UniversalNumber numberSingle = new UniversalNumber((float)10000000000000000000);
                UniversalNumber numberDouble = new UniversalNumber((double)10000000000000000000);
                UniversalNumber numberDecimal = new UniversalNumber((decimal)10000000000000000000);

                string intString = numberInt.ToString();

                UniversalNumber numberIntCompare = new UniversalNumber(1000000000);
                UniversalNumber numberUIntCompare = new UniversalNumber((uint)1000000000);
                UniversalNumber numberSingleCompare = new UniversalNumber((float)10000000000000000000);
                UniversalNumber numberDoubleCompare = new UniversalNumber((double)10000000000000000000);
                UniversalNumber numberDecimalCompare = new UniversalNumber((decimal)10000000000000000000);

                Assert.IsTrue(numberInt == numberIntCompare);
                Assert.IsTrue(numberUInt == numberUIntCompare);
                Assert.IsTrue(numberSingle == numberSingleCompare);
                Assert.IsTrue(numberDouble == numberDoubleCompare);
                Assert.IsTrue(numberDecimal == numberDecimalCompare);
            }
            {
                UniversalNumber numberInt = new UniversalNumber(30000000);
                UniversalNumber numberUInt = new UniversalNumber((uint)30000000);
                UniversalNumber numberSingle = new UniversalNumber((float)30000000);
                UniversalNumber numberDouble = new UniversalNumber((double)30000000);
                UniversalNumber numberDecimal = new UniversalNumber((decimal)30000000);

                UniversalNumber numberIntCompare = new UniversalNumber(30000000);
                UniversalNumber numberUIntCompare = new UniversalNumber((uint)30000000);
                UniversalNumber numberSingleCompare = new UniversalNumber((float)30000000);
                UniversalNumber numberDoubleCompare = new UniversalNumber((double)30000000);
                UniversalNumber numberDecimalCompare = new UniversalNumber((decimal)30000000);

                Assert.IsTrue(numberInt == numberIntCompare);
                Assert.IsTrue(numberUInt == numberIntCompare);
                Assert.IsTrue(numberSingle == numberIntCompare);
                Assert.IsTrue(numberDouble == numberIntCompare);
                Assert.IsTrue(numberDecimal == numberIntCompare);

                Assert.IsTrue(numberInt == numberUIntCompare);
                Assert.IsTrue(numberUInt == numberUIntCompare);
                Assert.IsTrue(numberSingle == numberUIntCompare);
                Assert.IsTrue(numberDouble == numberUIntCompare);
                Assert.IsTrue(numberDecimal == numberUIntCompare);

                Assert.IsTrue(numberInt == numberSingleCompare);
                Assert.IsTrue(numberUInt == numberSingleCompare);
                Assert.IsTrue(numberSingle == numberSingleCompare);
                Assert.IsTrue(numberDouble == numberSingleCompare);
                Assert.IsTrue(numberDecimal == numberSingleCompare);

                Assert.IsTrue(numberInt == numberDoubleCompare);
                Assert.IsTrue(numberUInt == numberDoubleCompare);
                Assert.IsTrue(numberSingle == numberDoubleCompare);
                Assert.IsTrue(numberDouble == numberDoubleCompare);
                Assert.IsTrue(numberDecimal == numberDoubleCompare);

                Assert.IsTrue(numberInt == numberDecimalCompare);
                Assert.IsTrue(numberUInt == numberDecimalCompare);
                Assert.IsTrue(numberSingle == numberDecimalCompare);
                Assert.IsTrue(numberDouble == numberDecimalCompare);
                Assert.IsTrue(numberDecimal == numberDecimalCompare);
            }

            {
                UniversalNumber numberSingle = new UniversalNumber((float)10000000000000000000);
                UniversalNumber numberDouble = new UniversalNumber((double)1000000000000000000);
                UniversalNumber numberDecimal = new UniversalNumber((decimal)10000000000000000000);

                UniversalNumber numberSingleCompare = new UniversalNumber((float)10000000000000000000);
                UniversalNumber numberDoubleCompare = new UniversalNumber((double)10000000000000000000);
                UniversalNumber numberDecimalCompare = new UniversalNumber((decimal)10000000000000000000);

                Assert.IsTrue(numberSingle == numberSingleCompare);
                Assert.IsTrue(numberSingle == numberDoubleCompare);
                Assert.IsTrue(numberSingle == numberDecimalCompare);

                Assert.IsTrue(numberDouble == numberSingleCompare);
                Assert.IsTrue(numberDouble == numberDoubleCompare);
                Assert.IsTrue(numberDouble == numberDecimalCompare);

                Assert.IsTrue(numberDecimal == numberSingleCompare);
                Assert.IsTrue(numberDecimal == numberDoubleCompare);
                Assert.IsTrue(numberDecimal == numberDecimalCompare);
            }

            {
                UniversalNumber numberSingle = new UniversalNumber((float)10000000000000000000);
                UniversalNumber numberDouble = new UniversalNumber((double)10000000000000000000);
                UniversalNumber numberDecimal = new UniversalNumber((decimal)10000000000000000000);

                UniversalNumber numberSingleCompare = new UniversalNumber((float)10000000000000000000);
                UniversalNumber numberDoubleCompare = new UniversalNumber((double)10000000000000000000);
                UniversalNumber numberDecimalCompare = new UniversalNumber((decimal)10000000000000000000);

                Assert.IsFalse(numberSingleCompare == numberDecimal);
                Assert.IsFalse(numberDecimal == numberSingleCompare);

                Assert.IsFalse(numberDoubleCompare == numberDecimal);
                Assert.IsFalse(numberDecimal == numberDoubleCompare);
            }
        }
    }
}
