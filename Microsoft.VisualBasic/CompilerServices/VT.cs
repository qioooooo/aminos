using System;

namespace Microsoft.VisualBasic.CompilerServices
{
	internal enum VT : short
	{
		Error = 10,
		Boolean,
		Byte = 17,
		Short = 2,
		Integer,
		Decimal = 14,
		Single = 4,
		Double,
		String = 8,
		ByteArray = 8209,
		CharArray,
		Date = 7,
		Long = 20,
		Char = 18,
		Variant = 12,
		Array = 8192,
		DBNull = 1,
		Empty = 0,
		Structure = 36,
		Currency = 6
	}
}
