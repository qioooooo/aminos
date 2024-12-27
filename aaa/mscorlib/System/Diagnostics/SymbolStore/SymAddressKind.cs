using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002BF RID: 703
	[ComVisible(true)]
	[Serializable]
	public enum SymAddressKind
	{
		// Token: 0x04000A5D RID: 2653
		ILOffset = 1,
		// Token: 0x04000A5E RID: 2654
		NativeRVA,
		// Token: 0x04000A5F RID: 2655
		NativeRegister,
		// Token: 0x04000A60 RID: 2656
		NativeRegisterRelative,
		// Token: 0x04000A61 RID: 2657
		NativeOffset,
		// Token: 0x04000A62 RID: 2658
		NativeRegisterRegister,
		// Token: 0x04000A63 RID: 2659
		NativeRegisterStack,
		// Token: 0x04000A64 RID: 2660
		NativeStackRegister,
		// Token: 0x04000A65 RID: 2661
		BitField,
		// Token: 0x04000A66 RID: 2662
		NativeSectionOffset
	}
}
