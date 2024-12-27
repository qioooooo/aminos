using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000574 RID: 1396
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ELEMDESC
	{
		// Token: 0x04001B2F RID: 6959
		public TYPEDESC tdesc;

		// Token: 0x04001B30 RID: 6960
		public ELEMDESC.DESCUNION desc;

		// Token: 0x02000575 RID: 1397
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04001B31 RID: 6961
			[FieldOffset(0)]
			public IDLDESC idldesc;

			// Token: 0x04001B32 RID: 6962
			[FieldOffset(0)]
			public PARAMDESC paramdesc;
		}
	}
}
