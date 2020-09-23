using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter.Numerics
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    //[DebuggerDisplay("Nothing, hahaha")]
    public struct Number // : IComparable<Number>
    {
        private readonly static decimal[] decimalPowers = { 0.000000000000000001m, 0.000000000000000002m, 0.000000000000000004m, 0.000000000000000008m, 0.000000000000000016m, 0.000000000000000032m,
                                                            0.000000000000000064m, 0.000000000000000128m, 0.000000000000000256m, 0.000000000000000512m, 0.000000000000001024m, 0.000000000000002048m,
                                                            0.000000000000004096m, 0.000000000000008192m, 0.000000000000016384m, 0.000000000000032768m, 0.000000000000065536m, 0.000000000000131072m,
                                                            0.000000000000262144m, 0.000000000000524288m, 0.000000000001048576m, 0.000000000002097152m, 0.000000000004194304m, 0.000000000008388608m,
                                                            0.000000000016777216m, 0.000000000033554432m, 0.000000000067108864m, 0.000000000134217728m, 0.000000000268435456m, 0.000000000536870912m,
                                                            0.000000001073741824m, 0.000000002147483648m, 0.000000004294967296m, 0.000000008589934592m, 0.000000017179869184m, 0.000000034359738368m,
                                                            0.000000068719476736m, 0.000000137438953472m, 0.000000274877906944m, 0.000000549755813888m, 0.000001099511627776m, 0.000002199023255552m,
                                                            0.000004398046511104m, 0.000008796093022208m, 0.000017592186044416m, 0.000035184372088832m, 0.000070368744177664m, 0.000140737488355328m,
                                                            0.000281474976710656m, 0.000562949953421312m, 0.001125899906842624m, 0.002251799813685248m, 0.004503599627370496m, 0.009007199254740992m,
                                                            0.018014398509481984m, 0.036028797018963968m, 0.072057594037927936m, 0.144115188075855872m, 0.288230376151711744m, 0.576460752303423488m,
                                                            1.152921504606846976m, 2.305843009213693952m, 4.611686018427387904m, 9.223372036854775808m, 18.446744073709551616m, 36.893488147419103232m,
                                                            73.786976294838206464m, 147.573952589676412928m, 295.147905179352825856m, 590.295810358705651712m, 1180.591620717411303424m,
                                                            2361.183241434822606848m, 4722.366482869645213696m, 9444.732965739290427392m, 18889.465931478580854784m, 37778.931862957161709568m,
                                                            75557.863725914323419136m, 151115.727451828646838272m, 302231.454903657293676544m, 604462.909807314587353088m, 1208925.819614629174706176m,
                                                            2417851.639229258349412352m, 4835703.278458516698824704m, 9671406.556917033397649408m, 19342813.113834066795298816m, 38685626.227668133590597632m,
                                                            77371252.455336267181195264m, 154742504.910672534362390528m, 309485009.821345068724781056m, 618970019.642690137449562112m,
                                                            1237940039.285380274899124224m, 2475880078.570760549798248448m, 4951760157.141521099596496896m, 9903520314.283042199192993792m,
                                                            19807040628.566084398385987584m, 39614081257.132168796771975168m, 79228162514.264337593543950336m, 158456325028.528675187087900672m,
                                                            316912650057.057350374175801344m, 633825300114.114700748351602688m, 1267650600228.229401496703205376m, 2535301200456.458802993406410752m,
                                                            5070602400912.917605986812821504m, 10141204801825.835211973625643008m, 20282409603651.670423947251286016m, 40564819207303.340847894502572032m,
                                                            81129638414606.681695789005144064m, 162259276829213.363391578010288128m, 324518553658426.726783156020576256m, 649037107316853.453566312041152512m,
                                                            1298074214633706.907132624082305024m, 2596148429267413.814265248164610048m, 5192296858534827.628530496329220096m,
                                                            10384593717069655.257060992658440192m, 20769187434139310.514121985316880384m, 41538374868278621.028243970633760768m,
                                                            83076749736557242.056487941267521536m, 166153499473114484.112975882535043072m, 332306998946228968.225951765070086144m,
                                                            664613997892457936.451903530140172288m, 1329227995784915872.903807060280344576m, 2658455991569831745.807614120560689152m,
                                                            5316911983139663491.615228241121378304m, 10633823966279326983.230456482242756608m, 21267647932558653966.460912964485513216m,
                                                            42535295865117307932.921825928971026432m, 85070591730234615865.843651857942052864m };

        //private readonly static double[] doublePowers = { 0.000000000000000001, 0.000000000000000002, 0.000000000000000004, 0.000000000000000008, 0.000000000000000016, 0.000000000000000032,
        //                                                  0.000000000000000064, 0.000000000000000128, 0.000000000000000256, 0.000000000000000512, 0.000000000000001024, 0.000000000000002048,
        //                                                  0.000000000000004096, 0.000000000000008192, 0.000000000000016384, 0.000000000000032768, 0.000000000000065536, 0.000000000000131072,
        //                                                  0.000000000000262144, 0.000000000000524288, 0.000000000001048576, 0.000000000002097152, 0.000000000004194304, 0.000000000008388608,
        //                                                  0.000000000016777216, 0.000000000033554432, 0.000000000067108864, 0.000000000134217728, 0.000000000268435456, 0.000000000536870912,
        //                                                  0.000000001073741824, 0.000000002147483648, 0.000000004294967296, 0.000000008589934592, 0.000000017179869184, 0.000000034359738368,
        //                                                  0.000000068719476736, 0.000000137438953472, 0.000000274877906944, 0.000000549755813888, 0.000001099511627776, 0.000002199023255552,
        //                                                  0.000004398046511104, 0.000008796093022208, 0.000017592186044416, 0.000035184372088832, 0.000070368744177664, 0.000140737488355328,
        //                                                  0.000281474976710656, 0.000562949953421312, 0.001125899906842624, 0.002251799813685248, 0.004503599627370496, 0.009007199254740992,
        //                                                  0.018014398509481984, 0.036028797018963968, 0.072057594037927936, 0.144115188075855872, 0.288230376151711744, 0.576460752303423488,
        //                                                  1.152921504606846976, 2.305843009213693952, 4.611686018427387904, 9.223372036854775808, 18.446744073709551616, 36.893488147419103232,
        //                                                  73.786976294838206464, 147.573952589676412928, 295.147905179352825856, 590.295810358705651712, 1180.591620717411303424,
        //                                                  2361.183241434822606848, 4722.366482869645213696, 9444.732965739290427392, 18889.465931478580854784, 37778.931862957161709568,
        //                                                  75557.863725914323419136, 151115.727451828646838272, 302231.454903657293676544, 604462.909807314587353088, 1208925.819614629174706176,
        //                                                  2417851.639229258349412352, 4835703.278458516698824704, 9671406.556917033397649408, 19342813.113834066795298816, 38685626.227668133590597632,
        //                                                  77371252.455336267181195264, 154742504.910672534362390528, 309485009.821345068724781056, 618970019.642690137449562112,
        //                                                  1237940039.285380274899124224, 2475880078.570760549798248448, 4951760157.141521099596496896, 9903520314.283042199192993792,
        //                                                  19807040628.566084398385987584, 39614081257.132168796771975168, 79228162514.264337593543950336, 158456325028.528675187087900672,
        //                                                  316912650057.057350374175801344, 633825300114.114700748351602688, 1267650600228.229401496703205376, 2535301200456.458802993406410752,
        //                                                  5070602400912.917605986812821504, 10141204801825.835211973625643008, 20282409603651.670423947251286016, 40564819207303.340847894502572032,
        //                                                  81129638414606.681695789005144064, 162259276829213.363391578010288128, 324518553658426.726783156020576256, 649037107316853.453566312041152512,
        //                                                  1298074214633706.907132624082305024, 2596148429267413.814265248164610048, 5192296858534827.628530496329220096,
        //                                                  10384593717069655.257060992658440192, 20769187434139310.514121985316880384, 41538374868278621.028243970633760768,
        //                                                  83076749736557242.056487941267521536, 166153499473114484.112975882535043072, 332306998946228968.225951765070086144,
        //                                                  664613997892457936.451903530140172288, 1329227995784915872.903807060280344576, 2658455991569831745.807614120560689152,
        //                                                  5316911983139663491.615228241121378304, 10633823966279326983.230456482242756608, 21267647932558653966.460912964485513216,
        //                                                  42535295865117307932.921825928971026432, 85070591730234615865.843651857942052864 };

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        private ulong hi;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(8)]
        private ulong lo;

        public static readonly Number Zero = new Number();

        private Number(ulong hi, ulong lo)
        {
            this.hi = hi;
            this.lo = lo;
        }

        private Number(ulong lo)
        {
            hi = 0;
            this.lo = lo;
        }

        //// OLD METHOD. (Ungenau gegen hintere Werte des doubles.)
        //public Number(double number)
        //{
        //    if (double.IsNaN(number))
        //        throw new ArgumentException("Symphony Number doesn't support NaN.", "number");

        //    if (double.IsPositiveInfinity(number) || number >= 170141183460469231731.687303715884105728)
        //    {
        //        hi = 0x7FFFFFFFFFFFFFFF;
        //        lo = 0xFFFFFFFFFFFFFFFF;

        //        return;
        //    }

        //    if (double.IsNegativeInfinity(number) || number <= -170141183460469231731.687303715884105728)
        //    {
        //        hi = 0x8000000000000000;
        //        lo = 0;

        //        return;
        //    }

        //    hi = 0;
        //    lo = 0;

        //    if (number < 0)
        //        number = -number;

        //    for (int bit = 126; bit >= 64; bit--)
        //        if (number >= doublePowers[bit])
        //        {
        //            number -= doublePowers[bit];
        //            hi |= 1UL << (bit - 64);
        //        }

        //    for (int bit = 63; bit >= 0; bit--)
        //        if (number >= doublePowers[bit])
        //        {
        //            number -= doublePowers[bit];
        //            lo |= 1UL << bit;
        //        }
        //}

        public unsafe Number(double number)
        {
            if (double.IsNaN(number))
                throw new ArgumentException("Symphony Number doesn't support NaN.", "number");

            if (double.IsPositiveInfinity(number) || number >= 170141183460469231731.687303715884105728)
            {
                hi = 0x7FFFFFFFFFFFFFFF;
                lo = 0xFFFFFFFFFFFFFFFF;

                return;
            }

            if (double.IsNegativeInfinity(number) || number <= -170141183460469231731.687303715884105728)
            {
                hi = 0x8000000000000000;
                lo = 0;

                return;
            }

            number *= 1000000000000000000.0;

            int sign = 1 - ((int)(*(ulong*)&number >> 62) & 2);
            ulong mantisse = *(ulong*)&number & 0x000FFFFFFFFFFFFF;
            int exponent = (int)(*(ulong*)&number >> 52) & 0x7FF;

            if (exponent == 0)
            {
                if (mantisse != 0)
                    exponent = -1074;
            }
            else
            {
                mantisse |= 0x0010000000000000;
                exponent -= 1075;
            }

            if (mantisse == 0)
            {
                hi = 0;
                lo = 0;

                return;
            }

            if (exponent < -63)
            {
                lo = 0;
                hi = 0;
            }
            else if (exponent < 0)
            {
                lo = mantisse >> -exponent;
                hi = 0;
            }
            else if (exponent == 0)
            {
                lo = mantisse;
                hi = 0;
            }
            else if (exponent < 64)
            {
                lo = mantisse << exponent;
                hi = (mantisse >> (64 - exponent)) & 0x7FFFFFFFFFFFFFFFUL;
            }
            else if (exponent < 128)
            {
                lo = 0;
                hi = (mantisse << (exponent - 64)) & 0x7FFFFFFFFFFFFFFFUL;
            }
            else
            {
                hi = 0x7FFFFFFFFFFFFFFF;
                lo = 0xFFFFFFFFFFFFFFFF;
            }

            if (sign < 0)
                negate(ref hi, ref lo);

            // Hier müssen wir leider hinter den signifikaten Stellen unzulässige Ziffern löschen. Geht leider erst mit Modulo.
        }

        public Number(decimal number)
        {
            if (number >= 170141183460469231731.687303715884105728m)
            {
                hi = 0x7FFFFFFFFFFFFFFF;
                lo = 0xFFFFFFFFFFFFFFFF;

                return;
            }

            if (number <= -170141183460469231731.687303715884105728m)
            {
                hi = 0x8000000000000000;
                lo = 0;

                return;
            }

            hi = 0;
            lo = 0;

            if (number < 0)
                number = -number;

            for (int bit = 126; bit >= 64; bit--)
                if (number >= decimalPowers[bit])
                {
                    number -= decimalPowers[bit];
                    hi |= 1UL << (bit - 64);
                }

            for (int bit = 63; bit >= 0; bit--)
                if (number >= decimalPowers[bit])
                {
                    number -= decimalPowers[bit];
                    lo |= 1UL << bit;
                }
        }

        public static bool operator ==(Number l, Number r)
        {
            return l.hi == r.hi && l.lo == r.lo;
        }

        public static bool operator !=(Number l, Number r)
        {
            return l.hi != r.hi || l.lo != r.lo;
        }

        public static bool operator >(Number l, Number r)
        {
            return l.hi > r.hi || l.lo > r.lo;
        }

        public static bool operator <(Number l, Number r)
        {
            return l.hi < r.hi || l.lo < r.lo;
        }

        public static Number operator *(Number l, Number r)
        {
            ulong oli1, oli0;

            if (l.hi > long.MaxValue)
            {
                negate(ref l.hi, ref l.lo);

                if (r.hi > long.MaxValue)
                {
                    negate(ref r.hi, ref r.lo);

                    multiply(out oli1, out oli0, l, r);
                    return new Number(oli1, oli0);
                }
                else
                {
                    multiply(out oli1, out oli0, l, r);
                    negate(ref oli1, ref oli0);
                    return new Number(oli1, oli0);
                }
            }
            else if (r.hi > long.MaxValue)
            {
                negate(ref r.hi, ref r.lo);
                multiply(out oli1, out oli0, l, r);
                negate(ref oli1, ref oli0);
                return new Number(oli1, oli0);
            }
            else
            {
                multiply(out oli1, out oli0, l, r);
                return new Number(oli1, oli0);
            }
        }

        public static Number operator /(Number l, Number r)
        {
            if (r.hi == 0 && r.lo == 0)
                throw new DivideByZeroException($"Sorry, can't divide by zero. (You tried to divide {l}/{r}.)");

            Number result;

            if (l.hi > long.MaxValue)
            {
                negate(ref l.hi, ref l.lo);

                if (r.hi > long.MaxValue)
                {
                    negate(ref r.hi, ref r.lo);

                    divide(out result.hi, out result.lo, l, r);
                }
                else
                {
                    divide(out result.hi, out result.lo, l, r);
                    negate(ref result.hi, ref result.lo);
                }
            }
            else if (r.hi > long.MaxValue)
            {
                negate(ref r.hi, ref r.lo);
                divide(out result.hi, out result.lo, l, r);
                negate(ref result.hi, ref result.lo);
            }
            else
            {
                divide(out result.hi, out result.lo, l, r);
            }

            return result;
        }

        public static Number operator %(Number l, Number r)
        {
            if (r.hi == 0 && r.lo == 0)
                return Zero;

            Number result;

            if (l.hi > long.MaxValue)
            {
                negate(ref l.hi, ref l.lo);

                if (r.hi > long.MaxValue)
                {
                    negate(ref r.hi, ref r.lo);

                    remainder(out result.hi, out result.lo, l, r);
                }
                else
                {
                    remainder(out result.hi, out result.lo, l, r);
                    negate(ref result.hi, ref result.lo);
                }
            }
            else if (r.hi > long.MaxValue)
            {
                negate(ref r.hi, ref r.lo);
                remainder(out result.hi, out result.lo, l, r);
                negate(ref result.hi, ref result.lo);
            }
            else
            {
                remainder(out result.hi, out result.lo, l, r);
            }

            return result;
        }

        private static unsafe void remainder(out ulong oli1, out ulong oli0, Number l, Number r)
        {
            //ulong m00l, m00h, m01l, m01h, m10l, m10h, m11l, m11h, oli2;

            //subMultiply(out m00h, out m00l, l.lo, 1000000000000000000UL);
            //subMultiply(out m01h, out m01l, l.lo, 0);
            //subMultiply(out m10h, out m10l, l.hi, 1000000000000000000UL);
            //subMultiply(out m11h, out m11l, l.hi, 0);

            //uint c1 = 0;
            //uint c2 = 0;

            //oli0 = m00l;
            //oli1 = add(add(m00h, m01l, ref c1), m10l, ref c1);
            //oli2 = add(add(add(m01h, m10h, ref c2), m11l, ref c2), c1, ref c2);

            //uint* num = stackalloc uint[6];
            //uint* res = stackalloc uint[6];

            uint* num = stackalloc uint[4];
            uint* res = stackalloc uint[4];

            //num[0] = (uint)oli0;
            //num[1] = (uint)(oli0 >> 32);
            //num[2] = (uint)oli1;
            //num[3] = (uint)(oli1 >> 32);
            //num[4] = (uint)oli2;
            //num[5] = (uint)(oli2 >> 32);

            num[0] = (uint)l.lo;
            num[1] = (uint)(l.lo >> 32);
            num[2] = (uint)l.hi;
            num[3] = (uint)(l.hi >> 32);

            uint div0 = (uint)r.lo;
            uint div1 = (uint)(r.lo >> 32);
            uint div2 = (uint)r.hi;
            uint div3 = (uint)(r.hi >> 32);

            //oli0 = (ulong)num[0] + ((div0 >> 1) | (div1 << 31));
            //num[0] = (uint)oli0;
            //oli0 = num[1] + (oli0 >> 32) + ((div1 >> 1) | (div2 << 31));
            //num[1] = (uint)oli0;
            //oli0 = num[2] + (oli0 >> 32) + ((div2 >> 1) | (div3 << 31));
            //num[2] = (uint)oli0;
            //oli0 = num[3] + (oli0 >> 32) + (div3 >> 1);
            //num[3] = (uint)oli0;
            //oli0 = num[4] + (oli0 >> 32);
            //num[4] = (uint)oli0;
            //oli0 = num[5] + (oli0 >> 32);
            //num[5] = (uint)oli0;

            if (div3 != 0)
            {
                //if (num[5] > 0)
                //    divide(num, 6, div0, div1, div2, div3, res);
                //else if (num[4] > 0)
                //    divide(num, 5, div0, div1, div2, div3, res);
                //else if (num[3] > 0)
                    divide(num, 4, div0, div1, div2, div3, res);
            }
            else if (div2 != 0)
            {
                //if (num[5] > 0)
                //    divide(num, 6, div0, div1, div2, res);
                //else if (num[4] > 0)
                //    divide(num, 5, div0, div1, div2, res);
                //else if (num[3] > 0)
                if (num[3] > 0)
                    divide(num, 4, div0, div1, div2, res);
                else if (num[2] > 0)
                    divide(num, 3, div0, div1, div2, res);
            }
            else if (div1 != 0)
            {
                //if (num[5] > 0)
                //    divide(num, 6, div0, div1, res);
                //else if (num[4] > 0)
                //    divide(num, 5, div0, div1, res);
                if (num[3] > 0)
                    divide(num, 4, div0, div1, res);
                else if (num[2] > 0)
                    divide(num, 3, div0, div1, res);
                else if (num[1] > 0)
                    divide(num, 2, div0, div1, res);
            }
            else if (div0 != 0)
            {
                //if (num[5] > 0)
                //    divide(num, 6, div0, res);
                //else if (num[4] > 0)
                //    divide(num, 5, div0, res);
                if (num[3] > 0)
                    divide(num, 4, div0, res);
                else if (num[2] > 0)
                    divide(num, 3, div0, res);
                else if (num[1] > 0)
                    divide(num, 2, div0, res);
                else if (num[0] > 0)
                    divide(num, 1, div0, res);
            }
            else
                throw new DivideByZeroException("Sorry, can't divide by zero.");

            oli0 = ((ulong)num[1] << 32) | num[0];
            oli1 = ((ulong)num[3] << 32) | num[2];
        }

        private static unsafe bool divide(out ulong oli1, out ulong oli0, Number l, Number r)
        {
            ulong m00l, m00h, m01l, m01h, m10l, m10h, m11l, m11h, oli2;

            subMultiply(out m00h, out m00l, l.lo, 1000000000000000000UL);
            subMultiply(out m01h, out m01l, l.lo, 0);
            subMultiply(out m10h, out m10l, l.hi, 1000000000000000000UL);
            subMultiply(out m11h, out m11l, l.hi, 0);

            uint c1 = 0;
            uint c2 = 0;

            oli0 = m00l;
            oli1 = add(add(m00h, m01l, ref c1), m10l, ref c1);
            oli2 = add(add(add(m01h, m10h, ref c2), m11l, ref c2), c1, ref c2);

            uint* num = stackalloc uint[6];
            uint* res = stackalloc uint[6];

            num[0] = (uint)oli0;
            num[1] = (uint)(oli0 >> 32);
            num[2] = (uint)oli1;
            num[3] = (uint)(oli1 >> 32);
            num[4] = (uint)oli2;
            num[5] = (uint)(oli2 >> 32);

            uint div0 = (uint)r.lo;
            uint div1 = (uint)(r.lo >> 32);
            uint div2 = (uint)r.hi;
            uint div3 = (uint)(r.hi >> 32);

            //BigInteger before = new BigInteger(new byte[] { (byte)num[0], (byte)(num[0] >> 8), (byte)(num[0] >> 16), (byte)(num[0] >> 24), (byte)num[1], (byte)(num[1] >> 8), (byte)(num[1] >> 16), (byte)(num[1] >> 24),
            //                                                 (byte)num[2], (byte)(num[2] >> 8), (byte)(num[2] >> 16), (byte)(num[2] >> 24), (byte)num[3], (byte)(num[3] >> 8), (byte)(num[3] >> 16), (byte)(num[3] >> 24),
            //                                                 (byte)num[4], (byte)(num[4] >> 8), (byte)(num[4] >> 16), (byte)(num[4] >> 24), (byte)num[5], (byte)(num[5] >> 8), (byte)(num[5] >> 16), (byte)(num[5] >> 24) });

            oli0 = (ulong)num[0] + ((div0 >> 1) | (div1 << 31));
            num[0] = (uint)oli0;
            oli0 = num[1] + (oli0 >> 32) + ((div1 >> 1) | (div2 << 31));
            num[1] = (uint)oli0;
            oli0 = num[2] + (oli0 >> 32) + ((div2 >> 1) | (div3 << 31));
            num[2] = (uint)oli0;
            oli0 = num[3] + (oli0 >> 32) + (div3 >> 1);
            num[3] = (uint)oli0;
            oli0 = num[4] + (oli0 >> 32);
            num[4] = (uint)oli0;
            oli0 = num[5] + (oli0 >> 32);
            num[5] = (uint)oli0;

            if (div3 != 0)
            {
                if (num[5] > 0)
                    divide(num, 6, div0, div1, div2, div3, res);
                else if (num[4] > 0)
                    divide(num, 5, div0, div1, div2, div3, res);
                else if (num[3] > 0)
                    divide(num, 4, div0, div1, div2, div3, res);
            }
            else if (div2 != 0)
            {
                if (num[5] > 0)
                    divide(num, 6, div0, div1, div2, res);
                else if (num[4] > 0)
                    divide(num, 5, div0, div1, div2, res);
                else if (num[3] > 0)
                    divide(num, 4, div0, div1, div2, res);
                else if (num[2] > 0)
                    divide(num, 3, div0, div1, div2, res);
            }
            else if (div1 != 0)
            {
                if (num[5] > 0)
                    divide(num, 6, div0, div1, res);
                else if (num[4] > 0)
                    divide(num, 5, div0, div1, res);
                else if (num[3] > 0)
                    divide(num, 4, div0, div1, res);
                else if (num[2] > 0)
                    divide(num, 3, div0, div1, res);
                else if (num[1] > 0)
                    divide(num, 2, div0, div1, res);
            }
            else if (div0 != 0)
            {
                if (num[5] > 0)
                    divide(num, 6, div0, res);
                else if (num[4] > 0)
                    divide(num, 5, div0, res);
                else if (num[3] > 0)
                    divide(num, 4, div0, res);
                else if (num[2] > 0)
                    divide(num, 3, div0, res);
                else if (num[1] > 0)
                    divide(num, 2, div0, res);
                else if (num[0] > 0)
                    divide(num, 1, div0, res);
            }
            else
                throw new DivideByZeroException("Sorry, can't divide by zero.");

            if (res[4] != 0 || res[5] != 0 || (res[3] & 0x8000000000000000UL) == 0x8000000000000000UL)
            {
                oli0 = ulong.MaxValue;
                oli1 = long.MaxValue;

                return false;
            }

            oli0 = ((ulong)res[1] << 32) | res[0];
            oli1 = ((ulong)res[3] << 32) | res[2];

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong add(ulong l, ulong r, ref uint c)
        {
            ulong result = l + r;

            if (result < l || result < r)
                c++;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void negate(ref ulong hi, ref ulong lo)
        {
            if (hi != 0)
                hi = 0 - hi;

            if (lo != 0)
            {
                lo = 0 - lo;
                lo--;
            }
        }

        public static unsafe void divide(uint* num, int nums, uint div0, uint* result)
        {
            ulong digit;
            uint carry;
            uint nx;
            int n;
            uint t;
            ulong valHi;
            uint valLo;

            uint divHi = div0;
            uint divLo = 0;

            int backShift = bitLength(divHi);
            int shift = 32 - backShift;

            if (shift > 0)
            {
                nx = 0;

                divHi = (divHi << shift) | (divLo >> backShift);
                divLo = (divLo << shift) | (nx >> backShift);
            }

            for (int i = nums; i >= 1; i--)
            {
                n = i - 1;
                t = i < nums ? num[i] : 0;

                valHi = ((ulong)t << 32) | num[i - 1];
                valLo = i > 1 ? num[i - 2] : 0;

                if (shift > 0)
                {
                    nx = i > 2 ? num[i - 3] : 0;

                    valHi = (valHi << shift) | (valLo >> backShift);
                    valLo = (valLo << shift) | (nx >> backShift);
                }

                digit = valHi / divHi;

                if (digit > 0xFFFFFFFF)
                    digit = 0xFFFFFFFF;

                while (divideGuessTooBig(digit, valHi, valLo, divHi, divLo))
                    --digit;

                if (digit > 0)
                {
                    carry = subtractDivisor(num + n, div0, digit);

                    if (carry != t)
                    {
                        carry = addDivisor(num + n, div0);
                        --digit;
                    }
                }

                result[n] = (uint)digit;

                if (i < nums)
                    num[i] = 0;
            }
        }

        public static unsafe void divide(uint* num, int nums, uint div0, uint div1, uint* result)
        {
            ulong digit;
            uint carry;
            uint nx;
            int n;
            uint t;
            ulong valHi;
            uint valLo;

            uint divHi = div1;
            uint divLo = div0;

            int backShift = bitLength(divHi);
            int shift = 32 - backShift;

            if (shift > 0)
            {
                nx = 0;

                divHi = (divHi << shift) | (divLo >> backShift);
                divLo = (divLo << shift) | (nx >> backShift);
            }

            for (int i = nums; i >= 2; i--)
            {
                n = i - 2;
                t = i < nums ? num[i] : 0;

                valHi = ((ulong)t << 32) | num[i - 1];
                valLo = i > 1 ? num[i - 2] : 0;

                if (shift > 0)
                {
                    nx = i > 2 ? num[i - 3] : 0;

                    valHi = (valHi << shift) | (valLo >> backShift);
                    valLo = (valLo << shift) | (nx >> backShift);
                }

                digit = valHi / divHi;

                if (digit > 0xFFFFFFFF)
                    digit = 0xFFFFFFFF;

                while (divideGuessTooBig(digit, valHi, valLo, divHi, divLo))
                    --digit;

                if (digit > 0)
                {
                    carry = subtractDivisor(num + n, div0, div1, digit);

                    if (carry != t)
                    {
                        carry = addDivisor(num + n, div0, div1);
                        --digit;
                    }
                }

                result[n] = (uint)digit;

                if (i < nums)
                    num[i] = 0;
            }
        }

        public static unsafe void divide(uint* num, int nums, uint div0, uint div1, uint div2, uint* result)
        {
            ulong digit;
            uint carry;
            uint nx;

            uint divHi = div2;
            uint divLo = div1;

            int backShift = bitLength(divHi);
            int shift = 32 - backShift;

            if (shift > 0)
            {
                nx = div1;

                divHi = (divHi << shift) | (divLo >> backShift);
                divLo = (divLo << shift) | (nx >> backShift);
            }

            for (int i = nums; i >= 3; i--)
            {
                int n = i - 3;
                uint t = i < nums ? num[i] : 0;

                ulong valHi = ((ulong)t << 32) | num[i - 1];
                uint valLo = i > 1 ? num[i - 2] : 0;

                if (shift > 0)
                {
                    nx = i > 2 ? num[i - 3] : 0;

                    valHi = (valHi << shift) | (valLo >> backShift);
                    valLo = (valLo << shift) | (nx >> backShift);
                }

                digit = valHi / divHi;

                if (digit > 0xFFFFFFFF)
                    digit = 0xFFFFFFFF;

                while (divideGuessTooBig(digit, valHi, valLo, divHi, divLo))
                    --digit;

                if (digit > 0)
                {
                    carry = subtractDivisor(num + n, div0, div1, div2, digit);

                    if (carry != t)
                    {
                        carry = addDivisor(num + n, div0, div1, div2);
                        --digit;
                    }
                }

                result[n] = (uint)digit;

                if (i < nums)
                    num[i] = 0;
            }
        }

        public static unsafe void divide(uint* num, int nums, uint div0, uint div1, uint div2, uint div3, uint* result)
        {
            // Division wie in der Schule, nur dass die Zahlen nicht bis 10, sondern 2^32 gehen.

            ulong digit;
            uint carry;
            uint nx;

            uint divHi = div3; // Der letzte Divisor.
            uint divLo = div2; // Der vorletzte Divisor oder 0.

            int backShift = bitLength(divHi);
            int shift = 32 - backShift;

            // Signifikantes Bit setzen.
            if (shift > 0)
            {
                nx = div1; // Das vor vor letzte Element oder 0.

                divHi = (divHi << shift) | (divLo >> backShift);
                divLo = (divLo << shift) | (nx >> backShift);
            }

            for (int i = nums; i >= 4; i--) // Von Numeratoren bis Länge Divisoren.
            {
                int n = i - 4; // Durchlauf Minus Divisoren.
                uint t = i < nums ? num[i] : 0; // 0 für den ersten Durchlauf, num[i] für die folgenden.

                ulong valHi = ((ulong)t << 32) | num[i - 1];
                uint valLo = i > 1 ? num[i - 2] : 0;

                if (shift > 0)
                {
                    nx = i > 2 ? num[i - 3] : 0;

                    valHi = (valHi << shift) | (valLo >> backShift);
                    valLo = (valLo << shift) | (nx >> backShift);
                }

                digit = valHi / divHi;

                if (digit > 0xFFFFFFFF)
                    digit = 0xFFFFFFFF;

                while (divideGuessTooBig(digit, valHi, valLo, divHi, divLo))
                    --digit;

                if (digit > 0)
                {
                    carry = subtractDivisor(num + n, div0, div1, div2, div3, digit);

                    if (carry != t)
                    {
                        carry = addDivisor(num + n, div0, div1, div2, div3);
                        --digit;
                    }
                }

                result[n] = (uint)digit;

                if (i < nums)
                    num[i] = 0;
            }
        }

        private static unsafe uint addDivisor(uint* left, uint div0, uint div1, uint div2)
        {
            // Loop 0 (Initial)

            ulong digit = left[0] + div0;
            left[0] = (uint)digit;

            // Loop 1

            digit = left[1] + (digit >> 32) + div1;
            left[1] = (uint)digit;

            // Loop 2

            digit = left[2] + (digit >> 32) + div2;
            left[2] = (uint)digit;

            return (uint)(digit >> 32);
        }

        private static unsafe uint addDivisor(uint* left, uint div0, uint div1, uint div2, uint div3)
        {
            // Loop 0 (Initial)

            ulong digit = left[0] + div0;
            left[0] = (uint)digit;

            // Loop 1

            digit = left[1] + (digit >> 32) + div1;
            left[1] = (uint)digit;

            // Loop 2

            digit = left[2] + (digit >> 32) + div2;
            left[2] = (uint)digit;

            // Loop 3

            digit = left[3] + (digit >> 32) + div3;
            left[3] = (uint)digit;

            return (uint)(digit >> 32);
        }

        private static unsafe uint addDivisor(uint* left, uint div0, uint div1)
        {
            // Loop 0 (Initial)

            ulong digit = left[0] + div0;
            left[0] = (uint)digit;

            // Loop 1

            digit = left[1] + (digit >> 32) + div1;
            left[1] = (uint)digit;

            return (uint)(digit >> 32);
        }

        private static unsafe uint addDivisor(uint* left, uint div0)
        {
            // Loop 0 (Initial)

            ulong digit = left[0] + div0;
            left[0] = (uint)digit;

            return (uint)(digit >> 32);
        }

        private static unsafe uint subtractDivisor(uint* left, uint div0, uint div1, uint div2, uint div3, ulong q)
        {
            // Loop: 0 (Initial)

            uint digit = (uint)(div0 * q);
            ulong carry = (div0 * q) >> 32;

            if (*left < digit)
                carry++;

            *left -= digit;

            // Loop: 1

            carry += div1 * q;
            digit = (uint)carry;
            carry >>= 32;

            if (left[1] < digit)
                carry++;

            left[1] -= digit;

            // Loop: 2

            carry += div2 * q;
            digit = (uint)carry;
            carry >>= 32;

            if (left[2] < digit)
                carry++;

            left[2] -= digit;

            // Loop: 3

            carry += div3 * q;
            digit = (uint)carry;
            carry >>= 32;

            if (left[3] < digit)
                carry++;

            left[3] -= digit;

            return (uint)carry;
        }

        private static unsafe uint subtractDivisor(uint* left, uint div0, uint div1, uint div2, ulong q)
        {
            // Loop: 0 (Initial)

            uint digit = (uint)(div0 * q);
            ulong carry = (div0 * q) >> 32;

            if (*left < digit)
                carry++;

            *left -= digit;

            // Loop: 1

            carry += div1 * q;
            digit = (uint)carry;
            carry >>= 32;

            if (left[1] < digit)
                carry++;

            left[1] -= digit;

            // Loop: 2

            carry += div2 * q;
            digit = (uint)carry;
            carry >>= 32;

            if (left[2] < digit)
                carry++;

            left[2] -= digit;

            return (uint)carry;
        }

        private static unsafe uint subtractDivisor(uint* left, uint div0, uint div1, ulong q)
        {
            // Loop: 0 (Initial)

            uint digit = (uint)(div0 * q);
            ulong carry = (div0 * q) >> 32;

            if (*left < digit)
                carry++;

            *left -= digit;

            // Loop: 1

            carry += div1 * q;
            digit = (uint)carry;
            carry >>= 32;

            if (left[1] < digit)
                carry++;

            left[1] -= digit;

            return (uint)carry;
        }

        private static unsafe uint subtractDivisor(uint* left, uint div0, ulong q)
        {
            // Loop: 0 (Initial)

            uint digit = (uint)(div0 * q);
            ulong carry = (div0 * q) >> 32;

            if (*left < digit)
                carry++;

            *left -= digit;

            return (uint)carry;
        }

        private static bool divideGuessTooBig(ulong q, ulong valHi, uint valLo, uint divHi, uint divLo)
        {
            ulong chkHi = divHi * q + ((divLo * q) >> 32);
            ulong chkLo = (uint)(divLo * q);

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

        private static int bitLength(uint number)
        {
            if (number == 0)
                return 0;

            int result;

            if (number < 256)
                result = 0;
            else if (number < 65536)
            {
                result = 8;
                number >>= 8;
            }
            else if (number < 16777216)
            {
                result = 16;
                number >>= 16;
            }
            else
            {
                result = 24;
                number >>= 24;
            }

            if (number < 2)
                return result + 1;

            if (number < 4)
                return result + 2;

            if (number < 8)
                return result + 3;

            if (number < 16)
                return result + 4;

            if (number < 32)
                return result + 5;

            if (number < 64)
                return result + 6;

            if (number < 128)
                return result + 7;

            return result + 8;
        }

        private static unsafe bool multiply(out ulong oli1, out ulong oli0, Number l, Number r)
        {
            ulong m00l, m00h, m01l, m01h, m10l, m10h, m11l, m11h, oli2;

            subMultiply(out m00h, out m00l, l.lo, r.lo);
            subMultiply(out m01h, out m01l, l.lo, r.hi);
            subMultiply(out m10h, out m10l, l.hi, r.lo);
            subMultiply(out m11h, out m11l, l.hi, r.hi);

            uint c1 = 0;
            uint c2 = 0;

            oli0 = m00l;
            oli1 = add(add(m00h, m01l, ref c1), m10l, ref c1);
            oli2 = add(add(add(m01h, m10h, ref c2), m11l, ref c2), c1, ref c2);

            if (m11h + c2 != 0 || oli2 > 0b110111100000101101101011001110100111011001000000000000000000 || oli2 == 0b110111100000101101101011001110100111011001000000000000000000 && oli1 != 0 && oli0 != 0)
            {
                oli0 = ulong.MaxValue;
                oli1 = long.MaxValue;

                return false;
            }

            uint* num = stackalloc uint[6];
            uint* res = stackalloc uint[4];

            num[0] = (uint)oli0;
            num[1] = (uint)(oli0 >> 32);
            num[2] = (uint)oli1;
            num[3] = (uint)(oli1 >> 32);
            num[4] = (uint)oli2;
            num[5] = (uint)(oli2 >> 32);

            oli0 = num[0] + 0xd3b1FFFFUL;
            num[0] = (uint)oli0;
            oli0 = num[1] + (oli0 >> 32) + 0x6f05b59UL;
            num[1] = (uint)oli0;
            oli0 = num[2] + (oli0 >> 32);
            num[2] = (uint)oli0;
            oli0 = num[3] + (oli0 >> 32);
            num[3] = (uint)oli0;
            oli0 = num[4] + (oli0 >> 32);
            num[4] = (uint)oli0;
            oli0 = num[5] + (oli0 >> 32);
            num[5] = (uint)oli0;

            if (num[5] > 0)
                divide(num, 6, 0b10100111011001000000000000000000, 0b1101111000001011011010110011, res);
            else if (num[4] > 0)
                divide(num, 5, 0b10100111011001000000000000000000, 0b1101111000001011011010110011, res);
            else if (num[3] > 0)
                divide(num, 4, 0b10100111011001000000000000000000, 0b1101111000001011011010110011, res);
            else if (num[2] > 0)
                divide(num, 3, 0b10100111011001000000000000000000, 0b1101111000001011011010110011, res);
            else if (num[1] > 0)
                divide(num, 2, 0b10100111011001000000000000000000, 0b1101111000001011011010110011, res);
            else if (num[0] > 0)
                divide(num, 1, 0b10100111011001000000000000000000, 0b1101111000001011011010110011, res);

            oli0 = ((ulong)res[1] << 32) | res[0];
            oli1 = ((ulong)res[3] << 32) | res[2];

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void subMultiply(out ulong hi, out ulong lo, ulong u, ulong v)
        {
            ulong carry = (uint)((((ulong)(uint)u * (uint)v) >> 32) + (uint)u * (v >> 32)) + (u >> 32) * (uint)v;
            lo = carry << 32 | ((uint)u * (uint)v);
            hi = (carry >> 32) + (((((ulong)(uint)u * (uint)v) >> 32) + (uint)u * (v >> 32)) >> 32) + (u >> 32) * (v >> 32);
        }

        public static Number operator +(Number l, Number r)
        {
            Number result = new Number();

            result.lo = l.lo + r.lo;
            result.hi = l.hi + r.hi;

            if (result.lo < l.lo && result.lo < r.lo)
                ++result.hi;

            return result;
        }

        public static Number operator -(Number l, Number r)
        {
            Number result = new Number();

            result.lo = l.lo - r.lo;
            result.hi = l.hi - r.hi;

            if (l.lo < r.lo)
                --l.hi;

            return result;
        }

        public static implicit operator Number(double src)
        {
            return new Number(src);
        }

        public static implicit operator Number(decimal src)
        {
            return new Number(src);
        }

        public static implicit operator decimal(Number src)
        {
            decimal result = 0m;

            for (int bit = 0; bit < 64; bit++)
                if ((src.lo & (1UL << bit)) == (1UL << bit))
                    result += decimalPowers[bit];

            for (int bit = 0; bit < 63; bit++)
                if ((src.hi & (1UL << bit)) == (1UL << bit))
                    result += decimalPowers[bit + 64];

            return result;
        }

        // CHG: Musch' leider Neu machen, meiner Methode von oben entsprechend. Ich hoffe des isch' ned zu kompliziert.
        //public static implicit operator double(Number src)
        //{
        //    double result = 0.0;

        //    for (int bit = 0; bit < 64; bit++)
        //        if ((src.lo & (1UL << bit)) == (1UL << bit))
        //            result += doublePowers[bit];

        //    for (int bit = 0; bit < 63; bit++)
        //        if ((src.hi & (1UL << bit)) == (1UL << bit))
        //            result += doublePowers[bit + 64];

        //    return result;
        //}


        public override string ToString()
        {
            decimal result = 0m;

            for (int bit = 0; bit < 64; bit++)
                if ((lo & (1UL << bit)) == (1UL << bit))
                    result += decimalPowers[bit];

            for (int bit = 0; bit < 63; bit++)
                if ((hi & (1UL << bit)) == (1UL << bit))
                    result += decimalPowers[bit + 64];

            return result.ToString("0.##################");
        }
    }
}
