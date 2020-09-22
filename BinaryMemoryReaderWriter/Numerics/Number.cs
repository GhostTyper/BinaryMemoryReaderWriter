using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter.Numerics
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    [DebuggerDisplay("Nothing, hahaha")]
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

        private readonly static double[] doublePowers = { 0.000000000000000001, 0.000000000000000002, 0.000000000000000004, 0.000000000000000008, 0.000000000000000016, 0.000000000000000032,
                                                          0.000000000000000064, 0.000000000000000128, 0.000000000000000256, 0.000000000000000512, 0.000000000000001024, 0.000000000000002048,
                                                          0.000000000000004096, 0.000000000000008192, 0.000000000000016384, 0.000000000000032768, 0.000000000000065536, 0.000000000000131072,
                                                          0.000000000000262144, 0.000000000000524288, 0.000000000001048576, 0.000000000002097152, 0.000000000004194304, 0.000000000008388608,
                                                          0.000000000016777216, 0.000000000033554432, 0.000000000067108864, 0.000000000134217728, 0.000000000268435456, 0.000000000536870912,
                                                          0.000000001073741824, 0.000000002147483648, 0.000000004294967296, 0.000000008589934592, 0.000000017179869184, 0.000000034359738368,
                                                          0.000000068719476736, 0.000000137438953472, 0.000000274877906944, 0.000000549755813888, 0.000001099511627776, 0.000002199023255552,
                                                          0.000004398046511104, 0.000008796093022208, 0.000017592186044416, 0.000035184372088832, 0.000070368744177664, 0.000140737488355328,
                                                          0.000281474976710656, 0.000562949953421312, 0.001125899906842624, 0.002251799813685248, 0.004503599627370496, 0.009007199254740992,
                                                          0.018014398509481984, 0.036028797018963968, 0.072057594037927936, 0.144115188075855872, 0.288230376151711744, 0.576460752303423488,
                                                          1.152921504606846976, 2.305843009213693952, 4.611686018427387904, 9.223372036854775808, 18.446744073709551616, 36.893488147419103232,
                                                          73.786976294838206464, 147.573952589676412928, 295.147905179352825856, 590.295810358705651712, 1180.591620717411303424,
                                                          2361.183241434822606848, 4722.366482869645213696, 9444.732965739290427392, 18889.465931478580854784, 37778.931862957161709568,
                                                          75557.863725914323419136, 151115.727451828646838272, 302231.454903657293676544, 604462.909807314587353088, 1208925.819614629174706176,
                                                          2417851.639229258349412352, 4835703.278458516698824704, 9671406.556917033397649408, 19342813.113834066795298816, 38685626.227668133590597632,
                                                          77371252.455336267181195264, 154742504.910672534362390528, 309485009.821345068724781056, 618970019.642690137449562112,
                                                          1237940039.285380274899124224, 2475880078.570760549798248448, 4951760157.141521099596496896, 9903520314.283042199192993792,
                                                          19807040628.566084398385987584, 39614081257.132168796771975168, 79228162514.264337593543950336, 158456325028.528675187087900672,
                                                          316912650057.057350374175801344, 633825300114.114700748351602688, 1267650600228.229401496703205376, 2535301200456.458802993406410752,
                                                          5070602400912.917605986812821504, 10141204801825.835211973625643008, 20282409603651.670423947251286016, 40564819207303.340847894502572032,
                                                          81129638414606.681695789005144064, 162259276829213.363391578010288128, 324518553658426.726783156020576256, 649037107316853.453566312041152512,
                                                          1298074214633706.907132624082305024, 2596148429267413.814265248164610048, 5192296858534827.628530496329220096,
                                                          10384593717069655.257060992658440192, 20769187434139310.514121985316880384, 41538374868278621.028243970633760768,
                                                          83076749736557242.056487941267521536, 166153499473114484.112975882535043072, 332306998946228968.225951765070086144,
                                                          664613997892457936.451903530140172288, 1329227995784915872.903807060280344576, 2658455991569831745.807614120560689152,
                                                          5316911983139663491.615228241121378304, 10633823966279326983.230456482242756608, 21267647932558653966.460912964485513216,
                                                          42535295865117307932.921825928971026432, 85070591730234615865.843651857942052864 };

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(0)]
        private ulong hi;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [FieldOffset(8)]
        private ulong lo;

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

        public Number(double number)
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

            hi = 0;
            lo = 0;

            if (number < 0)
                number = -number;

            for (int bit = 126; bit >= 64; bit--)
                if (number >= doublePowers[bit])
                {
                    number -= doublePowers[bit];
                    hi |= 1UL << (bit - 64);
                }

            for (int bit = 63; bit >= 0; bit--)
                if (number >= doublePowers[bit])
                {
                    number -= doublePowers[bit];
                    lo |= 1UL << bit;
                }
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
            ulong oli3, oli2, oli1, oli0;

            if (l.hi > long.MaxValue)
            {
                negate(ref l.hi, ref l.lo);

                if (r.hi > long.MaxValue)
                {
                    negate(ref r.hi, ref r.lo);

                    multiply(out oli3, out oli2, out oli1, out oli0, l, r);

                }
                else
                {
                    multiply(out oli3, out oli2, out oli1, out oli0, l, r);

                    negate(ref r.hi, ref r.lo);
                }
            }
            else if (r.hi > long.MaxValue)
            {
                negate(ref r.hi, ref r.lo);

                multiply(out oli3, out oli2, out oli1, out oli0, l, r);
            }
            else
                multiply(out oli3, out oli2, out oli1, out oli0, l, r);

            return new Number();
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

        public static void divide(uint num5, uint num4, uint num3, uint num2, uint num1, uint num0, ulong den, out ulong hi, out ulong lo)
        {
            int dneg = bitLength((uint)(den >> 32));
            int d = 32 - dneg;

            ulong denPrime = den << d;

            uint den0 = (uint)(denPrime >> 32);
            uint den1 = (uint)denPrime;

            uint num6;

            if (d != 0)
            {
                num6 = num5 >> dneg;
                num5 = num5 << d | num4 >> dneg;
                num4 = num4 << d | num3 >> dneg;
                num3 = num3 << d | num2 >> dneg;
                num2 = num2 << d | num1 >> dneg;
                num1 = num1 << d | num0 >> dneg;
                num0 <<= d;
            }
            else
                num6 = 0;

            subDivide(num6, ref num5, ref num4, den0, den1);
            hi = (ulong)subDivide(num5, ref num4, ref num3, den0, den1) << 32;
            hi |= subDivide(num4, ref num3, ref num2, den0, den1);
            lo = (ulong)subDivide(num3, ref num2, ref num1, den0, den1) << 32;
            lo |= subDivide(num2, ref num1, ref num0, den0, den1);
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

        private static ulong DivRem96(out UInt128 rem, ref UInt128 a, ref UInt128 b)
        {
            var d = 32 - GetBitLength(b.r2);
            UInt128 v;
            LeftShift64(out v, ref b, d);
            var r4 = (uint)LeftShift64(out rem, ref a, d);
            var v1 = v.r2;
            var v2 = v.r1;
            var v3 = v.r0;
            var r3 = rem.r3;
            var r2 = rem.r2;
            var r1 = rem.r1;
            var r0 = rem.r0;
            var q1 = DivRem(r4, ref r3, ref r2, ref r1, v1, v2, v3);
            var q0 = DivRem(r3, ref r2, ref r1, ref r0, v1, v2, v3);
            Create(out rem, r0, r1, r2, 0);
            var div = (ulong)q1 << 32 | q0;
            RightShift64(ref rem, d);
            Debug.Assert((BigInteger)div == (BigInteger)a / (BigInteger)b);
            Debug.Assert((BigInteger)rem == (BigInteger)a % (BigInteger)b);
            return div;
        }

        public static void Divide128(out ulong w1, out ulong w0, long u1, long u0, ulong v)
        {
            w0 = 0;
            w1 = 0;

            var dneg = bitLength((uint)(v >> 32));
            var d = 32 - dneg;
            var vPrime = v << d;

            var v1 = (uint)(vPrime >> 32);
            var v2 = (uint)vPrime;

            var r0 = (uint)u0;
            var r1 = (uint)(u0 << 32);
            var r2 = (uint)u1;
            var r3 = (uint)(u1 << 32);
            var r4 = (uint)0;

            if (d != 0)
            {
                r4 = r3 >> dneg;
                r3 = r3 << d | r2 >> dneg;
                r2 = r2 << d | r1 >> dneg;
                r1 = r1 << d | r0 >> dneg;
                r0 <<= d;
            }

            w1 = DivRem(r4, ref r3, ref r2, v1, v2);
            var q1 = DivRem(r3, ref r2, ref r1, v1, v2);
            var q0 = DivRem(r2, ref r1, ref r0, v1, v2);
            w0 = (ulong)q1 << 32 | q0;
        }

        private static uint DivRem(uint u0, ref uint u1, ref uint u2, uint v1, uint v2)
        {
            var qhat = Q(u0, u1, u2, v1, v2);
            var carry = qhat * v2;
            var borrow = (long)u2 - (uint)carry;

            carry >>= 32;
            u2 = (uint)borrow;

            borrow >>= 32;
            carry += qhat * v1;

            borrow += (long)u1 - (uint)carry;

            carry >>= 32;
            u1 = (uint)borrow;

            borrow >>= 32;
            borrow += (long)u0 - (uint)carry;

            if (borrow != 0)
            {
                --qhat;
                carry = (ulong)u2 + v2;
                u2 = (uint)carry;
                carry >>= 32;
                carry += (ulong)u1 + v1;
                u1 = (uint)carry;
            }
            return (uint)qhat;
        }

        private static ulong Q(uint u0, uint u1, uint u2, uint v1, uint v2)
        {
            var u0u1 = (ulong)u0 << 32 | u1;
            var qhat = u0 == v1 ? uint.MaxValue : u0u1 / v1;
            var r = u0u1 - qhat * v1;
            if (r == (uint)r && v2 * qhat > (r << 32 | u2))
            {
                --qhat;
                r += v1;
                if (r == (uint)r && v2 * qhat > (r << 32 | u2))
                {
                    --qhat;
                    r += v1;
                }
            }
            return qhat;
        }

        public static uint subDivide(uint oli0, ref uint oli1, ref uint oli2, uint n1, uint n2)
        {
            ulong qhat = oli0 == n1 ? uint.MaxValue : ((ulong)oli0 << 32 | oli1) / n1;
            ulong carry = ((ulong)oli0 << 32 | oli1) - qhat * n1;
            long borrow;

            if (carry == (uint)carry && n2 * qhat > (carry << 32 | oli2))
            {
                qhat--;
                carry += n1;

                if (carry == (uint)carry && n2 * qhat > (carry << 32 | oli2))
                    qhat--;
            }

            carry = qhat * n2;
            borrow = oli2 - (long)(uint)carry;

            oli2 = (uint)borrow;

            carry = (carry >> 32) + qhat * n1;
            borrow = (borrow >> 32) + n1 - (uint)carry;

            oli1 = (uint)borrow;

            borrow = (borrow >> 32) + oli0 - (uint)(carry >> 32);

            if (borrow != 0)
            {
                qhat--;

                carry = (ulong)oli2 + n2;
                oli2 = (uint)carry;
                oli1 = (uint)((carry >> 32) + oli1 + n1);
            }

            return (uint)qhat;
        }

        private static uint subDivide(uint oli0, ref uint oli1, ref uint oli2, ref uint oli3, uint n1, uint n2, uint n3)
        {
            ulong qhat = oli0 == n1 ? uint.MaxValue : ((ulong)oli0 << 32 | oli1) / n1;
            ulong carry = ((ulong)oli0 << 32 | oli1) - qhat * n1;
            long borrow;

            if (carry == (uint)carry && n2 * qhat > (carry << 32 | oli2))
            {
                qhat--;
                carry += n1;

                if (carry == (uint)carry && n2 * qhat > (carry << 32 | oli2))
                    qhat--;
            }

            carry = qhat * n3;
            borrow = oli3 - (long)(uint)carry;

            carry >>= 32;
            oli3 = (uint)borrow;

            borrow >>= 32;
            carry += qhat * n2;
            borrow += (long)oli2 - (uint)carry;

            carry >>= 32;
            oli2 = (uint)borrow;

            borrow >>= 32;
            carry += qhat * n1;
            borrow += (long)oli1 - (uint)carry;

            carry >>= 32;
            oli1 = (uint)borrow;
            borrow = (borrow >> 32) + oli0 - (uint)carry;

            if (borrow != 0)
            {
                qhat--;
                carry = (ulong)oli3 + n3;
                oli3 = (uint)carry;
                carry = (carry >> 32) + oli2 + n2;

                oli2 = (uint)carry;
                oli1 = (uint)((carry >> 32) + oli1 + n1);
            }

            return (uint)qhat;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong q(uint oli0, uint oli1, uint oli2, uint n1, uint n2)
        {
            ulong qhat = oli0 == n1 ? uint.MaxValue : ((ulong)oli0 << 32 | oli1) / n1;
            ulong r = ((ulong)oli0 << 32 | oli1) - qhat * n1;

            if (r == (uint)r && n2 * qhat > (r << 32 | oli2))
            {
                qhat--;
                r += n1;

                if (r == (uint)r && n2 * qhat > (r << 32 | oli2))
                    qhat--;
            }

            return qhat;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void multiply(out ulong oli3, out ulong oli2, out ulong oli1, out ulong oli0, Number l, Number r)
        {
            ulong m00l, m00h, m01l, m01h, m10l, m10h, m11l, m11h;

            subMultiply(out m00h, out m00l, l.lo, r.lo);
            subMultiply(out m01h, out m01l, l.lo, r.hi);
            subMultiply(out m10h, out m10l, l.hi, r.lo);
            subMultiply(out m11h, out m11l, l.hi, r.hi);

            uint c1 = 0;
            uint c2 = 0;

            oli0 = m00l;
            oli1 = add(add(m00h, m01l, ref c1), m10l, ref c1);
            oli2 = add(add(add(m01h, m10h, ref c2), m11l, ref c2), c1, ref c2);
            oli3 = m11h + c2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void subMultiply(out ulong hi, out ulong lo, ulong u, ulong v)
        {
            // ulong u0 = (ulong)(uint)u;
            // ulong u1 = u >> 32;
            // ulong v0 = (ulong)(uint)v;
            // ulong v1 = v >> 32;
            // ulong carry = (ulong)(uint)u * (uint)v;
            // uint r0 = (uint)carry;
            // ulong carry = (((ulong)(uint)u * (uint)v) >> 32) + (uint)u * (v >> 32);
            // ulong r2 = carry >> 32;
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

        public static implicit operator double(Number src)
        {
            double result = 0.0;

            for (int bit = 0; bit < 64; bit++)
                if ((src.lo & (1UL << bit)) == (1UL << bit))
                    result += doublePowers[bit];

            for (int bit = 0; bit < 63; bit++)
                if ((src.hi & (1UL << bit)) == (1UL << bit))
                    result += doublePowers[bit + 64];

            return result;
        }


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
