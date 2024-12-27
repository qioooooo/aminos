using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200008A RID: 138
	[ComVisible(true)]
	[Serializable]
	public enum MemberAttributes
	{
		// Token: 0x040008A4 RID: 2212
		Abstract = 1,
		// Token: 0x040008A5 RID: 2213
		Final,
		// Token: 0x040008A6 RID: 2214
		Static,
		// Token: 0x040008A7 RID: 2215
		Override,
		// Token: 0x040008A8 RID: 2216
		Const,
		// Token: 0x040008A9 RID: 2217
		New = 16,
		// Token: 0x040008AA RID: 2218
		Overloaded = 256,
		// Token: 0x040008AB RID: 2219
		Assembly = 4096,
		// Token: 0x040008AC RID: 2220
		FamilyAndAssembly = 8192,
		// Token: 0x040008AD RID: 2221
		Family = 12288,
		// Token: 0x040008AE RID: 2222
		FamilyOrAssembly = 16384,
		// Token: 0x040008AF RID: 2223
		Private = 20480,
		// Token: 0x040008B0 RID: 2224
		Public = 24576,
		// Token: 0x040008B1 RID: 2225
		AccessMask = 61440,
		// Token: 0x040008B2 RID: 2226
		ScopeMask = 15,
		// Token: 0x040008B3 RID: 2227
		VTableMask = 240
	}
}
