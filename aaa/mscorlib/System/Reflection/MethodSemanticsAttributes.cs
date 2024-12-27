using System;

namespace System.Reflection
{
	// Token: 0x02000304 RID: 772
	[Flags]
	[Serializable]
	internal enum MethodSemanticsAttributes
	{
		// Token: 0x04000B7D RID: 2941
		Setter = 1,
		// Token: 0x04000B7E RID: 2942
		Getter = 2,
		// Token: 0x04000B7F RID: 2943
		Other = 4,
		// Token: 0x04000B80 RID: 2944
		AddOn = 8,
		// Token: 0x04000B81 RID: 2945
		RemoveOn = 16,
		// Token: 0x04000B82 RID: 2946
		Fire = 32
	}
}
