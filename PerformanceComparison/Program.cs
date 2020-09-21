﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SharpFast.BinaryMemoryReaderWriter;
using SharpFast.BinaryMemoryReaderWriter.Numerics;
using System.Collections.Generic;
using System.IO;

namespace PerformanceComparison
{
    public class Program
    {
        static unsafe void Main()
        {
            // Number number = new Number(85070591730234615865.843651857942052864m);

            Number number = new Number(127605887595351923798.765477786913079296m);

            // Number number = new Number(13370000000000.000000000001m);

            System.Console.WriteLine($"Edge-case: {number} != 127605887595351923798,765477786913079296 (cause decimal doesn't have enought digits.)");

            number = new Number(127605887595351923798.765477786913079296);

            System.Console.WriteLine($"Edge-case: {number} != 127605887595351923798,765477786913079296 (cause double doesn't have enought digits.)");

            // Number num = new Number(0.000000000000000001);
            // Number num = new Number(0x1337BEEF / (1000000000000000000.0 / 65536.0));

            //Number num = new Number(0.00001337);

            //ulong a = 5000000000000000000;
            //ulong b = 3000000000000000000;

            //Number number1 = new Number((double)a);
            //Number number2 = new Number((double)b);

            //bool l = number1 < number2;

            //Number result1 = number1 + number2;

            //Number result2 = number1 - number2;

            //System.Console.WriteLine($"{result2}");

            // BenchmarkRunner.Run<Program>();
        }

        public IEnumerable<byte[]> DataSource()
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms))
                for (int count = 0; count < 10240; count++)
                    writer.Write("ABCDEFGHI");

            yield return ms.ToArray();
        }

        [Benchmark]
        public void MemoryStreamBinaryWriter_Initialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write((byte)0x5A);
        }

        [Benchmark]
        public void MemoryStreamBinaryReader_Initialize()
        {
            MemoryStream ms = new MemoryStream(new byte[1]);
            BinaryReader reader = new BinaryReader(ms);
            reader.ReadByte();
        }

        [Benchmark]
        public unsafe void ManagedBinaryMemoryWriter_Initialize()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            writer.Write((byte)0x5A);
        }

        [Benchmark]
        public unsafe void BinaryMemoryWriter_Initialize()
        {
            byte[] data = new byte[1];

            fixed (byte* pData = data)
            {
                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, 1);
                writer.Write((byte)0x5A);
            }
        }

        [Benchmark]
        public unsafe void BinaryMemoryReader_Initialize()
        {
            byte[] data = new byte[1];

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, 1);
                reader.ReadByte();
            }
        }

        [Benchmark]
        public unsafe void UnsafeBinaryMemoryWriter_Initialize()
        {
            byte[] data = new byte[1];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);
                writer.Write((byte)0x5A);
            }
        }

        [Benchmark]
        public unsafe void UnsafeBinaryMemoryReader_Initialize()
        {
            byte[] data = new byte[1];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);
                reader.ReadByte();
            }
        }

        [Benchmark]
        public void MemoryStreamBinaryWriter_Bytes()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            for (int count = 0; count < 10240; count++)
                writer.Write((byte)0x5A);
        }

        [Benchmark]
        public void MemoryStreamBinaryReader_Bytes()
        {
            MemoryStream ms = new MemoryStream(new byte[10240]);
            BinaryReader reader = new BinaryReader(ms);

            for (int count = 0; count < 10240; count++)
                reader.ReadByte();
        }

        [Benchmark]
        public unsafe void ManagedBinaryMemoryWriter_Bytes()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 10240; count++)
                writer.Write((byte)0x5A);
        }

        [Benchmark]
        public unsafe void BinaryMemoryWriter_Bytes()
        {
            byte[] data = new byte[10240];

            fixed (byte* pData = data)
            {
                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, 10240);

                for (int count = 0; count < 10240; count++)
                    writer.Write((byte)0x5A);
            }
        }

        [Benchmark]
        public unsafe void BinaryMemoryReader_Bytes()
        {
            byte[] data = new byte[10240];

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, 10240);

                for (int count = 0; count < 10240; count++)
                    reader.ReadByte();
            }
        }

        [Benchmark]
        public unsafe void UnsafeBinaryMemoryWriter_Bytes()
        {
            byte[] data = new byte[10240];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 10240; count++)
                    writer.Write((byte)0x5A);
            }
        }

        [Benchmark]
        public unsafe void UnsafeBinaryMemoryReader_Bytes()
        {
            byte[] data = new byte[10240];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 10240; count++)
                    reader.ReadByte();
            }
        }

        [Benchmark]
        public void MemoryStreamBinaryWriter_Short()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            for (int count = 0; count < 10240; count++)
                writer.Write((short)0x5AA5);
        }

        [Benchmark]
        public void MemoryStreamBinaryReader_Short()
        {
            MemoryStream ms = new MemoryStream(new byte[20480]);
            BinaryReader reader = new BinaryReader(ms);

            for (int count = 0; count < 10240; count++)
                reader.ReadInt16();
        }

        [Benchmark]
        public unsafe void ManagedBinaryMemoryWriter_Short()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 10240; count++)
                writer.Write((short)0x5AA5);
        }

        [Benchmark]
        public unsafe void BinaryMemoryWriter_Short()
        {
            byte[] data = new byte[20480];

            fixed (byte* pData = data)
            {
                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, 20480);

                for (int count = 0; count < 10240; count++)
                    writer.Write((short)0x5AA5);
            }
        }

        [Benchmark]
        public unsafe void BinaryMemoryReader_Short()
        {
            byte[] data = new byte[20480];

            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, 20480);

                for (int count = 0; count < 10240; count++)
                    reader.ReadInt16();
            }
        }

        [Benchmark]
        public unsafe void UnsafeBinaryMemoryWriter_Short()
        {
            byte[] data = new byte[20480];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 10240; count++)
                    writer.Write((short)0x5AA5);
            }
        }

        [Benchmark]
        public unsafe void UnsafeBinaryMemoryReader_Short()
        {
            byte[] data = new byte[20480];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 10240; count++)
                    reader.ReadInt16();
            }
        }

        [Benchmark]
        public void MemoryStreamBinaryWriter_Strings()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            for (int count = 0; count < 10240; count++)
                writer.Write("ABCDEFGHI");
        }

        [Benchmark]
        [ArgumentsSource(nameof(DataSource))]
        public void MemoryStreamBinaryReader_Strings(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(ms);

            for (int count = 0; count < 10240; count++)
                reader.ReadString();
        }

        [Benchmark]
        public unsafe void ManagedBinaryMemoryWriter_Strings()
        {
            ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

            for (int count = 0; count < 10240; count++)
                writer.Write("ABCDEFGHI");
        }

        [Benchmark]
        public unsafe void BinaryMemoryWriter_Strings()
        {
            byte[] data = new byte[102400];

            fixed (byte* pData = data)
            {
                BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, 102400);

                for (int count = 0; count < 10240; count++)
                    writer.Write("ABCDEFGHI");
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(DataSource))]
        public unsafe void BinaryMemoryReader_Strings(byte[] data)
        {
            fixed (byte* pData = data)
            {
                BinaryMemoryReader reader = new BinaryMemoryReader(pData, 102400);

                for (int count = 0; count < 10240; count++)
                    reader.ReadString();
            }
        }

        [Benchmark]
        public unsafe void UnsafeBinaryMemoryWriter_Strings()
        {
            byte[] data = new byte[102400];

            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

                for (int count = 0; count < 10240; count++)
                    writer.Write("ABCDEFGHI");
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(DataSource))]
        public unsafe void UnsafeBinaryMemoryReader_Strings(byte[] data)
        {
            fixed (byte* pData = data)
            {
                UnsafeBinaryMemoryReader reader = new UnsafeBinaryMemoryReader(pData);

                for (int count = 0; count < 10240; count++)
                    reader.ReadString();
            }
        }
    }
}
