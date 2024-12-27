using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053B RID: 1339
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.FUNCDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	public struct FUNCDESC
	{
		// Token: 0x04001A44 RID: 6724
		public int memid;

		// Token: 0x04001A45 RID: 6725
		public IntPtr lprgscode;

		// Token: 0x04001A46 RID: 6726
		public IntPtr lprgelemdescParam;

		// Token: 0x04001A47 RID: 6727
		public FUNCKIND funckind;

		// Token: 0x04001A48 RID: 6728
		public INVOKEKIND invkind;

		// Token: 0x04001A49 RID: 6729
		public CALLCONV callconv;

		// Token: 0x04001A4A RID: 6730
		public short cParams;

		// Token: 0x04001A4B RID: 6731
		public short cParamsOpt;

		// Token: 0x04001A4C RID: 6732
		public short oVft;

		// Token: 0x04001A4D RID: 6733
		public short cScodes;

		// Token: 0x04001A4E RID: 6734
		public ELEMDESC elemdescFunc;

		// Token: 0x04001A4F RID: 6735
		public short wFuncFlags;
	}
}
