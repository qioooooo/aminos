using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000274 RID: 628
	public struct STGMEDIUM
	{
		// Token: 0x04001244 RID: 4676
		public TYMED tymed;

		// Token: 0x04001245 RID: 4677
		public IntPtr unionmember;

		// Token: 0x04001246 RID: 4678
		[MarshalAs(UnmanagedType.IUnknown)]
		public object pUnkForRelease;
	}
}
