using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFast.Numerics
{
    /// <summary>
    /// This is the numerical utility class. It manages primitive calculations (eg. adding, subtracting,
    /// negativating, multiplying, dividing, remainder calculations) for big integer operations.
    ///
    /// The static functions in this static class require you to align the numbers little-endian.Make
    /// sure you understand this concept before using this class because it contains unsafe code which
    /// may damage the memory of the program you use those functions in.
    ///
    /// When writing arabic decimal numbers onto a piece of paper the lowest significant number stands
    /// on the right and the number with the highest significance on the left.
    ///
    /// However in the memory when using little-endian alignment the least significant byte can be found
    /// on the left side while the most significant byte is found on the right side.
    ///
    /// This knowledge is crucial since some methods require you to specify your data as uint* and some
    /// as ulong*. Because this library uses little-endian your alignment must make sure that an uint at
    /// a lower address has a lower significance valuewise.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// The quickest way of dividing bigger numbers I could come up with. I challenge you to find
        /// a better way. :)
        ///
        /// The given pointers are limited in their amount by the int-parameters you specify. However
        /// this routine will scan the data you present to it to avoid unused cycles. And no, you didn't
        /// find a better way of dividing, when you just split up this method in various methods for
        /// various amounts of int* or some inner loops.
        ///
        /// *res will contain the result of the division. This result isn't rounded or whatever. *div
        /// should contain the divisor and shouldn't contain 0 in every slot. This method will punish
        /// doing so with a DivideByZeroException. *num should contain the numerator and will contain
        /// the remainder at the end (=after calling this method).
        ///
        /// *res and *num will be limited to nums int pieces. *div is limited to divs int pieces.
        /// 
        /// Works completely on little-endian formattings as described in the class documentation comment.
        /// </summary>
        /// <param name="num">The pointer of an int sequence. This will contain the remainder after the
        /// successful method call.</param>
        /// <param name="nums">The maximum amount of num and also res slots.</param>
        /// <param name="div">The pointer of an little endian formatted int sequence.</param>
        /// <param name="divs">The maximum amount of div slots.</param>
        /// <param name="res">The divided result.</param>
        public static unsafe void Divide(uint* num, int nums, uint* div, int divs, uint* res)
        {
            uint* nptr;  // generally a pointer somewhere pointing to the numerator.
            uint* dptr;  // generally a pointer somewhere pointing to the divisor.
            uint* neptr; // generally a pointer usually pointing to the border numerator of an enumeration.
            uint* deptr; // generally a pointer usually pointing to the border divisor of an enumeration.
            uint* hptr;  // a helping pointer.

            // First we make sure we loop through the smallest amount of int pieces.

            for (hptr = div + divs - 1; *hptr == 0 && hptr > div; hptr--)
                divs--;

            if (divs == 0) // Divisor is 0 and we don't support dividing by zero.
                throw new DivideByZeroException("Can't divide by zero.");

            for (hptr = num + nums - 1; *hptr == 0 && hptr > num; hptr--)
                nums--;

            if (nums < divs) // Numerator is < Divisors, we don't need to do anything here, because
                return;      // the result must be zero.

            // We want to have the most data for our guessing routine available if a set of local ints
            // will be guessed of how often they match into the local numerator. So we shift them to the
            // highest bit.

            int shift = 0;
            int backShift;

            // We measure the amount of bits to shift by already changing the corresponding number to the
            // leftiest location possible. Because we need those data all the time shifted for consecutive
            // runs we build a new shifted version in memory.

            uint* sdiv = stackalloc uint[divs + 1]; // Donating some extra bytes to the moved variables will
            uint* snum = stackalloc uint[nums + 2]; // reduce future branch predictions drastically.

            dptr = sdiv + divs;

            *dptr = *(div + divs - 1);
            *sdiv = 0;
            *snum = 0;
            *(snum + nums + 1) = 0;

            while (*dptr < 4194304)
            {
                shift += 10;
                *dptr <<= 10;
            }

            while (*dptr < 536870912)
            {
                shift += 3;
                *dptr <<= 3;
            }

            while (*dptr < 2147483648U)
            {
                shift++;
                *dptr <<= 1;
            }

            if (shift == 32)
            {
                backShift = 0;

                if (divs > 1)
                    Buffer.MemoryCopy(div + 1, sdiv + 1, (divs - 1) * 4, (divs - 1) * 4);

                Buffer.MemoryCopy(num, snum, nums * 4, nums * 4);
            }
            else
            {
                backShift = 32 - shift;

                if (divs > 1)
                {
                    hptr = sdiv + 1;

                    *hptr++ = *div << shift;

                    for (dptr = div, deptr = div + divs - 2; dptr < deptr; dptr++, hptr++)
                        *hptr = (*(dptr + 1) << shift) | (*dptr >> backShift);

                    *hptr |= *dptr >> backShift;
                }

                hptr = snum + 1;

                *hptr++ = *num << shift;

                for (nptr = num, neptr = num + nums - 1; nptr < neptr; nptr++, hptr++)
                    *hptr = (*(nptr + 1) << shift) | (*nptr >> backShift);

                *hptr = *nptr >> backShift;
            }

            ulong digit;
            uint carry;
            uint nx;

            for (int i = nums; i >= divs; i--)
            {
                int n = i - divs;
                uint t = i < nums ? num[i] : 0;

                ulong valHi = ((ulong)t << 32) | num[i - 1];
                uint valLo = i > 1 ? num[i - 2] : 0;

                if (shift > 0)
                {
                    nx = i > 2 ? num[i - 3] : 0;

                    valHi = (valHi << shift) | (valLo >> backShift);
                    valLo = (valLo << shift) | (nx >> backShift);
                }

                digit = valHi / *(sdiv + divs);

                if (digit > 0xFFFFFFFF)
                    digit = 0xFFFFFFFF;

                while (DivideGuessTooBig(digit, valHi, valLo, *(sdiv + divs), *(sdiv + divs - 1)))
                    --digit;

                if (digit > 0)
                {
                    carry = SubtractDivisor(num + n, nums - n, div, divs, digit);

                    if (carry != t)
                    {
                        carry = AddDivisor(num + n, nums - n, div, divs);
                        --digit;
                    }
                }

                res[n] = (uint)digit;

                if (i < nums)
                    num[i] = 0;
            }
        }

        private static unsafe uint AddDivisor(uint* left, int leftLength,
                                      uint* right, int rightLength)
        {
            // Repairs the dividend, if the last subtract was too much

            ulong carry = 0UL;

            for (int i = 0; i < rightLength; i++)
            {
                ulong digit = (left[i] + carry) + right[i];
                left[i] = unchecked((uint)digit);
                carry = digit >> 32;
            }

            return (uint)carry;
        }

        private static unsafe uint SubtractDivisor(uint* left, int leftLength, uint* right, int rightLength, ulong q)
        {
            // Combines a subtract and a multiply operation, which is naturally
            // more efficient than multiplying and then subtracting...

            ulong carry = 0UL;

            for (int i = 0; i < rightLength; i++)
            {
                carry += right[i] * q;
                uint digit = unchecked((uint)carry);
                carry = carry >> 32;
                if (left[i] < digit)
                    ++carry;
                left[i] = unchecked(left[i] - digit);
            }

            return (uint)carry;
        }

        private static bool DivideGuessTooBig(ulong q, ulong valHi, uint valLo, uint divHi, uint divLo)
        {
            // We multiply the two most significant limbs of the divisor
            // with the current guess for the quotient. If those are bigger
            // than the three most significant limbs of the current dividend
            // we return true, which means the current guess is still too big.

            ulong chkHi = divHi * q;
            ulong chkLo = divLo * q;

            chkHi = chkHi + (chkLo >> 32);
            chkLo = chkLo & 0xFFFFFFFF;

            if (chkHi < valHi)
                return false;
            if (chkHi > valHi)
                return true;

            if (chkLo < valLo)
                return false;
            if (chkLo > valLo)
                return true;

            return false;
        }


    }
}
