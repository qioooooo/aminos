using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F8 RID: 760
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum FieldAttributes
	{
		// Token: 0x04000AFE RID: 2814
		FieldAccessMask = 7,
		// Token: 0x04000AFF RID: 2815
		PrivateScope = 0,
		// Token: 0x04000B00 RID: 2816
		Private = 1,
		// Token: 0x04000B01 RID: 2817
		FamANDAssem = 2,
		// Token: 0x04000B02 RID: 2818
		Assembly = 3,
		// Token: 0x04000B03 RID: 2819
		Family = 4,
		// Token: 0x04000B04 RID: 2820
		FamORAssem = 5,
		// Token: 0x04000B05 RID: 2821
		Public = 6,
		// Token: 0x04000B06 RID: 2822
		Static = 16,
		// Token: 0x04000B07 RID: 2823
		InitOnly = 32,
		// Token: 0x04000B08 RID: 2824
		Literal = 64,
		// Token: 0x04000B09 RID: 2825
		NotSerialized = 128,
		// Token: 0x04000B0A RID: 2826
		SpecialName = 512,
		// Token: 0x04000B0B RID: 2827
		PinvokeImpl = 8192,
		// Token: 0x04000B0C RID: 2828
		ReservedMask = 38144,
		// Token: 0x04000B0D RID: 2829
		RTSpecialName = 1024,
		// Token: 0x04000B0E RID: 2830
		HasFieldMarshal = 4096,
		// Token: 0x04000B0F RID: 2831
		HasDefault = 32768,
		// Token: 0x04000B10 RID: 2832
		HasFieldRVA = 256
	}
}
