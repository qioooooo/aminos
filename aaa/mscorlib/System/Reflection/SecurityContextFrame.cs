using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace System.Reflection
{
	// Token: 0x020002F2 RID: 754
	internal struct SecurityContextFrame
	{
		// Token: 0x06001E0F RID: 7695
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Push(Assembly assembly);

		// Token: 0x06001E10 RID: 7696
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Pop();

		// Token: 0x04000AEC RID: 2796
		private IntPtr m_GSCookie;

		// Token: 0x04000AED RID: 2797
		private IntPtr __VFN_table;

		// Token: 0x04000AEE RID: 2798
		private IntPtr m_Next;

		// Token: 0x04000AEF RID: 2799
		private IntPtr m_Assembly;
	}
}
