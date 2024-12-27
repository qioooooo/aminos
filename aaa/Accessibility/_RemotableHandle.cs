using System;
using System.Runtime.InteropServices;

namespace Accessibility
{
	// Token: 0x02000008 RID: 8
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct _RemotableHandle
	{
		// Token: 0x04000004 RID: 4
		public int fContext;

		// Token: 0x04000005 RID: 5
		public __MIDL_IWinTypes_0009 u;
	}
}
