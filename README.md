# Introduction

`BinaryMemoryReaderWriter` is a memory reader and writer mostly compatible to the `BinaryReader` and `BinaryWriter` of the .NET Framework.

The main aim is to build readers and writer which performs the best possible way. The `BinaryReader` or -`Writer` of the .NET framework uses a `Stream` to write to, which is generally a slow approach, if written to the memory (via `MemoryStream`, for instance).

# Should you use these structs?

Do you have a performance problem which really is caused by the CPU? (You most probably don't.) Don't use this if your performance issues are caused by other components, like the network interfaces or disks.

# How to use those structs?

There are 2 ways:

1. Check out the project, build the library and use it.
2. Copy one of the structs to your code and use it.

# Coding example

You should look at the Tests of the project. There you may find out how all of the methods can be used.

You can write an `unsigned integer` to a `byte[]` like this:

```csharp
byte[] data;

unsafe
{
    data = new byte[4];

    fixed (byte* pData = data)
    {
        BinaryMemoryWriter writer = new BinaryMemoryWriter(pData, 4);

        writer.Write(0xDEADBEEFu);
    }
}
```

# What to consider?

* Those readers and writers aren't classes. They are structs. This means, you should pass them via the `in` keyword, if you want to pass them to another method.
* The readers with *Unsafe* in their name are *unsafe*. They will just continue writing over the memory borders. Beware!