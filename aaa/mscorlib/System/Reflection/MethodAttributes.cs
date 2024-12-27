using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000317 RID: 791
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum MethodAttributes
	{
		// Token: 0x04000CF3 RID: 3315
		MemberAccessMask = 7,
		// Token: 0x04000CF4 RID: 3316
		PrivateScope = 0,
		// Token: 0x04000CF5 RID: 3317
		Private = 1,
		// Token: 0x04000CF6 RID: 3318
		FamANDAssem = 2,
		// Token: 0x04000CF7 RID: 3319
		Assembly = 3,
		// Token: 0x04000CF8 RID: 3320
		Family = 4,
		// Token: 0x04000CF9 RID: 3321
		FamORAssem = 5,
		// Token: 0x04000CFA RID: 3322
		Public = 6,
		// Token: 0x04000CFB RID: 3323
		Static = 16,
		// Token: 0x04000CFC RID: 3324
		Final = 32,
		// Token: 0x04000CFD RID: 3325
		Virtual = 64,
		// Token: 0x04000CFE RID: 3326
		HideBySig = 128,
		// Token: 0x04000CFF RID: 3327
		CheckAccessOnOverride = 512,
		// Token: 0x04000D00 RID: 3328
		VtableLayoutMask = 256,
		// Token: 0x04000D01 RID: 3329
		ReuseSlot = 0,
		// Token: 0x04000D02 RID: 3330
		NewSlot = 256,
		// Token: 0x04000D03 RID: 3331
		Abstract = 1024,
		// Token: 0x04000D04 RID: 3332
		SpecialName = 2048,
		// Token: 0x04000D05 RID: 3333
		PinvokeImpl = 8192,
		// Token: 0x04000D06 RID: 3334
		UnmanagedExport = 8,
		// Token: 0x04000D07 RID: 3335
		RTSpecialName = 4096,
		// Token: 0x04000D08 RID: 3336
		ReservedMask = 53248,
		// Token: 0x04000D09 RID: 3337
		HasSecurity = 16384,
		// Token: 0x04000D0A RID: 3338
		RequireSecObject = 32768
	}
}
