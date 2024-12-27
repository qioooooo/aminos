using System;

namespace System.Xml.Xsl
{
	// Token: 0x02000121 RID: 289
	[Flags]
	internal enum XslFlags
	{
		// Token: 0x04000894 RID: 2196
		None = 0,
		// Token: 0x04000895 RID: 2197
		String = 1,
		// Token: 0x04000896 RID: 2198
		Number = 2,
		// Token: 0x04000897 RID: 2199
		Boolean = 4,
		// Token: 0x04000898 RID: 2200
		Node = 8,
		// Token: 0x04000899 RID: 2201
		Nodeset = 16,
		// Token: 0x0400089A RID: 2202
		Rtf = 32,
		// Token: 0x0400089B RID: 2203
		TypeFilter = 63,
		// Token: 0x0400089C RID: 2204
		AnyType = 63,
		// Token: 0x0400089D RID: 2205
		Current = 256,
		// Token: 0x0400089E RID: 2206
		Position = 512,
		// Token: 0x0400089F RID: 2207
		Last = 1024,
		// Token: 0x040008A0 RID: 2208
		FocusFilter = 1792,
		// Token: 0x040008A1 RID: 2209
		FullFocus = 1792,
		// Token: 0x040008A2 RID: 2210
		HasCalls = 4096,
		// Token: 0x040008A3 RID: 2211
		MayBeDefault = 8192,
		// Token: 0x040008A4 RID: 2212
		SideEffects = 16384,
		// Token: 0x040008A5 RID: 2213
		Stop = 32768
	}
}
