using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000541 RID: 1345
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.ELEMDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ELEMDESC
	{
		// Token: 0x04001A65 RID: 6757
		public TYPEDESC tdesc;

		// Token: 0x04001A66 RID: 6758
		public ELEMDESC.DESCUNION desc;

		// Token: 0x02000542 RID: 1346
		[ComVisible(false)]
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04001A67 RID: 6759
			[FieldOffset(0)]
			public IDLDESC idldesc;

			// Token: 0x04001A68 RID: 6760
			[FieldOffset(0)]
			public PARAMDESC paramdesc;
		}
	}
}
