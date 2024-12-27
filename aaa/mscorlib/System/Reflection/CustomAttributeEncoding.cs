using System;

namespace System.Reflection
{
	// Token: 0x020002EE RID: 750
	[Serializable]
	internal enum CustomAttributeEncoding
	{
		// Token: 0x04000ACD RID: 2765
		Undefined,
		// Token: 0x04000ACE RID: 2766
		Boolean = 2,
		// Token: 0x04000ACF RID: 2767
		Char,
		// Token: 0x04000AD0 RID: 2768
		SByte,
		// Token: 0x04000AD1 RID: 2769
		Byte,
		// Token: 0x04000AD2 RID: 2770
		Int16,
		// Token: 0x04000AD3 RID: 2771
		UInt16,
		// Token: 0x04000AD4 RID: 2772
		Int32,
		// Token: 0x04000AD5 RID: 2773
		UInt32,
		// Token: 0x04000AD6 RID: 2774
		Int64,
		// Token: 0x04000AD7 RID: 2775
		UInt64,
		// Token: 0x04000AD8 RID: 2776
		Float,
		// Token: 0x04000AD9 RID: 2777
		Double,
		// Token: 0x04000ADA RID: 2778
		String,
		// Token: 0x04000ADB RID: 2779
		Array = 29,
		// Token: 0x04000ADC RID: 2780
		Type = 80,
		// Token: 0x04000ADD RID: 2781
		Object,
		// Token: 0x04000ADE RID: 2782
		Field = 83,
		// Token: 0x04000ADF RID: 2783
		Property,
		// Token: 0x04000AE0 RID: 2784
		Enum
	}
}
