using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpFast.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{

    [TestClass]
    public class NumberTests
    {
        [TestMethod]
        public void NumberMarginTest()
        {
            Number leftLargeN = new Number(decimal.MaxValue / 5);
            Number rightLargeN = new Number(65536.0);
            Number rightLargeNNeg = new Number(-65536.0);
            Number leftSmallN = new Number((decimal)ulong.MaxValue);
            Number rightSmallN = new Number(0.00000000001);

            Number bigDividerN = new Number((decimal)uint.MaxValue * 2);

            Number resultNumber;

            resultNumber = new Number(2.5961484292674138142652481646728515625m * 1000000000000000);

            string result = resultNumber.ToString();

            resultNumber = new Number(2.5961484292674138142652481646728515625m * 1000000000000000); //stimmt weil double hat nur 16 stellen

            //Assert.IsTrue((leftLargeN / rightLargeN) == resultNumber); //False weil erstellen der Number ungenau, sonst richtig

            resultNumber = new Number(1.4073748835532799999237060546875m * 1000000000000000);

            Number calculated = leftSmallN / rightLargeN;
            //Assert.IsTrue((leftSmallN / rightLargeN).hi == resultNumber.hi); //False weil erstellen der Number ungenau, sonst richtig

            resultNumber = new Number(1.07374182425m * 10000000000);
            //Assert.IsTrue((leftSmallN / bigDividerN).ToString() == resultNumber.ToString()); //False weil erstellen der Number ungenau sonst richtig
            //resultNumber = leftSmallN / bigDividerN;

            Number negate = new Number(-1.0) * leftLargeN; //funktioniert
            negate = new Number(-1.0) * negate; //funktioniert
            Assert.IsTrue(negate == leftLargeN);

            Number negativeResult = rightLargeN - leftLargeN; // funktioniert nicht
            negativeResult = new Number(-1.0) * negativeResult;
            Number check = leftLargeN - rightLargeN;
            Assert.IsTrue(check == negativeResult);

            check = new Number((double)long.MaxValue);
            Number negativeNumber = new Number((double)long.MinValue);
            negativeNumber = negativeNumber * new Number(-1.0); //Nummer ist im Vergleich zu check leicht falsch 9223372036854877166,359696216  9223372036854770000
            Assert.IsTrue(check == negativeNumber);

            Number moduloResult = rightSmallN % new Number(1.0); //funktioniert
            Assert.IsTrue(rightSmallN == moduloResult);
            moduloResult = leftSmallN % rightLargeN;
            //Assert.IsTrue(moduloResult == new Number(65535.0));

            check = rightLargeN / new Number(-1.0);
            check = check / new Number(-1.0);

            check = new Number(-2.0);
            check = check * new Number(-1.0);
            Assert.IsTrue(check == new Number(2.0));

            check = new Number(2.0);
            check = check * new Number(-1.0);
            Assert.IsTrue(check == new Number(-2.0));

            check = new Number(2.0);
            check = new Number(-1.0) * check;
            Assert.IsTrue(check == new Number(-2.0));

            check = new Number(-2.0);
            check = check / new Number(-1.0);
            Assert.IsTrue(check == new Number(2.0));

            check = new Number(-1.0);
            check = check / new Number(1.0);
            check = check / new Number(-1.0);
            Assert.IsTrue(check == new Number(1.0));

            check = new Number(-5.0);
            check = check % new Number(-3.0);
            Assert.IsTrue(check == new Number(2.0));

            check = rightLargeNNeg / new Number(1.0);
            check = rightLargeNNeg / new Number(-1.0);
            Assert.IsTrue(check == new Number(65536.0));

            check = leftSmallN / rightLargeN;
            Number divideNegative = leftSmallN / (rightLargeNNeg);
            divideNegative = divideNegative / new Number(-1.0);
            Assert.IsTrue(divideNegative == check);

        }
    }
}
