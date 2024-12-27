using System;

namespace System.Reflection
{
	// Token: 0x020002FE RID: 766
	[Serializable]
	internal enum CorCallingConvention : byte
	{
		// Token: 0x04000B26 RID: 2854
		Default,
		// Token: 0x04000B27 RID: 2855
		Vararg = 5,
		// Token: 0x04000B28 RID: 2856
		Field,
		// Token: 0x04000B29 RID: 2857
		LocalSig,
		// Token: 0x04000B2A RID: 2858
		Property,
		// Token: 0x04000B2B RID: 2859
		Unmanaged,
		// Token: 0x04000B2C RID: 2860
		GenericInstance
	}
}
