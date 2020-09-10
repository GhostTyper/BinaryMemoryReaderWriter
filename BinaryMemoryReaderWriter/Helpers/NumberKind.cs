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
        SignedInteger,
        /// <summary>
        /// The type is ulong.
        /// </summary>
        UnsignedInteger,
        /// <summary>
        /// The type is half. (.NET 5.0)
        /// </summary>
        Half,
        /// <summary>
        /// The type is float.
        /// </summary>
        Single,
        /// <summary>
        /// The type is double.
        /// </summary>
        Double,
        /// <summary>
        /// The type is decimal.
        /// </summary>
        Decimal
    }
}
