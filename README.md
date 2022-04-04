# Introduction

`BinaryMemoryReaderWriter` is a memory reader and writer mostly compatible to the `BinaryReader` and `BinaryWriter` of the .NET Framework. It offers various flavours of readers and writers suitable for different situations. It is compatible to .NET Standard 2.0 and therefore free of `Span<T>`. This means: Those classes and structs are also compatible to the classic .NET Framework.

The main aim is to build readers and writers which perform the best possible way. The `BinaryReader` or -`Writer` of the .NET framework uses a `Stream` to write to. If you want to write to a `byte[]` this approach is generally more slow compared to writing directly to the memory like the `BinaryReaderWriter` of this library does.

Those readers and writers are supported:

* `ManagedBinaryMemoryWriter`: This is a writer which is fully managed, including it's own memory management. This writer builds a linked list with written segments. Smaller write commands will be combined into the same segment. This writer is about double as fast as the `BinaryWriter` -> `MemoryStream` combination from the .NET framework and should be safe to use.
* `BinaryMemoryReader` and `BinaryMemoryWriter`: This pair of reader and writer is about 3-4 times faster than the solution of the .NET framework. However it requires you to pin the pointer you give to the reader or writer.
* `UnsafeBinaryMemoryReader` and `UnsafeBinaryMemoryWriter`: These reader and writer are the fastest and are actually unsafe. You should only use them, if you are sure that you can trust all the input variables. However they offer the highest performance available and are avoiding bounds-checking. Also non destination checks are removed, so this writer (and reader) just takes your comand and does as you pleased. You have been warned!

The performance compared to the `BinaryWriter` and `MemoryStream` combination looks as follows:

![Graphical Overview Writer Performance](./performance.png)

*Those values are generated (see `PerformanceComparison` sub project) via `BenchmarkDotNet`. If you want to run the benchmarks you need to select the right project as "Starting Project". Also: Initialize means initialize and write one byte. Write `byte`, `short` or `string` means writing 10000 of them in a for-loop which also goes into the measurement.*

# Should you use these classes or structs?

Do you have a performance problem which really is caused by the CPU? (You most probably don't.) Don't use these readers and writers if your performance issues are caused by other components, like the network interfaces or disks.

It generally is a good idea to write code with good performance. However, you should also consider compatibility, etc. And the most compatible way of doing things is to just use components and classes out of the primary framework (.NET Framework).

# How to use those classes or structs?

There are 2 ways:

1. Check out the project, build the library and use it. You should also do the tests to check if everything is all right.
2. Copy the structs you need to your code and use them. With this method you don't need an external library and therefore you have no external dependency.

You need to compile your program with `/unsafe` when copying this code to your codebase.

# Coding example

You should look at the Tests of the project. There you may find out how all of the methods can be used.

You can write an `unsigned integer` to a `byte[]` like this:

```csharp
byte[] data = new byte[4];

unsafe
{
    fixed (byte* pData = data)
    {
        BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, 4);

        writer.Write(0xDEADBEEFu);
    }
}
```

You can also do more complex scenarios with these readers and writers. I, for instance use them to build advanced data structures where some information are available after writing the main content:

```csharp
byte[] data = new byte[1024];

unsafe
{
    fixed (byte* pData = data)
    {
        UnsafeBinaryMemoryWriter writer = new UnsafeBinaryMemoryWriter(pData);

        writer.Write(1337); // Write something header like.

        // Store the current position in size.
        UnsafeBinaryMemoryWriter size = writer;

        // Make a free slot of 2 bytes.
        writer.Jump(2);

        writer.Write(27392L); // Some other data.

        // Fill the previous slot with information which got available right now.
        size.Write((short)42);
    }
}
```

Storing the current location like this with one of the structs is quite effective because it's just making a copy of one pointer. Or in the case of the not `Unsafe` writer it's additionally the size which will be copied. Beware that `ManagedBinaryMemoryWriter` is a class.

Since version 1.1 `ManagedBinaryMemoryWriter` also supports insertionpoints like the more complex scenario above:

```csharp
ManagedBinaryMemoryWriter writer = new ManagedBinaryMemoryWriter();

writer.Write(1337); // Write something header like.

// Store the current position in size. You have space of 2 bytes in this insertionpoint.
ManagedBinaryMemoryWriterSegment size = writer.MakeInsertionpoint(2);

// We don't need to make an additional free slot of 2 bytes.

writer.Write(27392L); // Some other data.

// Fill the previous slot with information which got available right now.
size.Write((short)42);

// We need to call finish on the insertion point so that all counters like writer.Length will
// be updated accordingly.
size.Finish();
```

I added an StreamWriter which should dam GCWait secitions in threads. The writer can be used like this:

```csharp
BinaryStreamWriter writer = new BinaryStreamWriter();

using (BinaryStreamWriterExternal insertionPoint = writer.Write(4))
using (BinaryStreamWriterExternal external = writer.Write(64))
{
    external.Writer.Write(0x11111111);
    external.Writer.Write(0x22222222);
    external.Writer.Write(0x33333333);
    external.Writer.Write(0x44444444);

    // Later we write what we wanted to insert
    insertionPoint.Writer.Write((short)0x5555);
}

byte[] data;

using (MemoryStream ms = new MemoryStream())
{
    writer.PushToStream(ms);

    data = ms.ToArray();
}

// The final layout (in data) looks like this:
// 0x555511111111222222223333333344444444
```

It may be a good advice to add a `BufferedStream` in between the target stream because every insertion command will be a single write.

# What to consider?

* Those readers and writers aren't classes (except `ManagedBinaryMemoryWriter`). They are structs. This means, you should pass them via the `ref` keyword, if you want to pass them to another method.
* The readers with `Unsafe` in their name are *unsafe*. They will just continue writing over the memory borders. Also the `Unsafe` versions will not check parameters like the `step` size of the `Jump` method. Beware!
* Char operations are not exactly compatible with the BinaryWriter or Reader from the .NET framework. These BinaryReaders and Writers here also write `Surrogate` characters without exception.
* `ManagedBinaryMemoryWriter` is a class. There currently exists no reader, because you usually can initialize an existing array easily with one of the other readers. If there is a real demand, I will write a ManagedBinaryMemoryReader.
* `BinaryStreamWriter` is also a class and it also doesn't have a reader.