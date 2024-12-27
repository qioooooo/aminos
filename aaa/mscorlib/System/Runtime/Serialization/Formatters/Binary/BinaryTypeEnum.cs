using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007A9 RID: 1961
	[Serializable]
	internal enum BinaryTypeEnum
	{
		// Token: 0x040022E5 RID: 8933
		Primitive,
		// Token: 0x040022E6 RID: 8934
		String,
		// Token: 0x040022E7 RID: 8935
		Object,
		// Token: 0x040022E8 RID: 8936
		ObjectUrt,
		// Token: 0x040022E9 RID: 8937
		ObjectUser,
		// Token: 0x040022EA RID: 8938
		ObjectArray,
		// Token: 0x040022EB RID: 8939
		StringArray,
		// Token: 0x040022EC RID: 8940
		PrimitiveArray
	}
}
