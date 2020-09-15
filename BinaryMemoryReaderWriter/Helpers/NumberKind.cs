using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFast.BinaryMemoryReaderWriter.Helpers
{
    /// <summary>
    /// Specifies the raw number type.
    /// </summary>
    public enum NumberKind : byte
    {
        /// <summary>
        /// The type is long.
        /// </summary>
        SignedInteger = 0,
        /// <summary>
        /// The type is long and negative but transferred as positive.
        /// </summary>
        NegativeSignedInterger = 1,
        /// <summary>
        /// The type is ulong.
        /// </summary>
        UnsignedInteger = 2,
        /// <summary>
        /// The type is half. (.NET 5.0)
        /// </summary>
        Half = 16,
        /// <summary>
        /// The type is float.
        /// </summary>
        Single = 17,
        /// <summary>
        /// The type is double.
        /// </summary>
        Double = 18,
        /// <summary>
        /// The type is decimal.
        /// </summary>
        Decimal = 32
    }
}
