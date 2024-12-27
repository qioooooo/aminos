using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007AA RID: 1962
	[Serializable]
	internal enum BinaryArrayTypeEnum
	{
		// Token: 0x040022EE RID: 8942
		Single,
		// Token: 0x040022EF RID: 8943
		Jagged,
		// Token: 0x040022F0 RID: 8944
		Rectangular,
		// Token: 0x040022F1 RID: 8945
		SingleOffset,
		// Token: 0x040022F2 RID: 8946
		JaggedOffset,
		// Token: 0x040022F3 RID: 8947
		RectangularOffset
	}
}
