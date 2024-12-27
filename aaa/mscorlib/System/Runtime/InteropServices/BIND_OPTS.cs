using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000520 RID: 1312
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.BIND_OPTS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	public struct BIND_OPTS
	{
		// Token: 0x040019F5 RID: 6645
		public int cbStruct;

		// Token: 0x040019F6 RID: 6646
		public int grfFlags;

		// Token: 0x040019F7 RID: 6647
		public int grfMode;

		// Token: 0x040019F8 RID: 6648
		public int dwTickCountDeadline;
	}
}
