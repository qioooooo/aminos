using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005EB RID: 1515
	internal static class JitHelpers
	{
		// Token: 0x060037AD RID: 14253
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UnsafeSetArrayElement(object[] target, int index, object element);
	}
}
