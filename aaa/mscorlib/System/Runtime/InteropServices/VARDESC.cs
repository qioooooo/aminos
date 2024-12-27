using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000543 RID: 1347
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.VARDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct VARDESC
	{
		// Token: 0x04001A69 RID: 6761
		public int memid;

		// Token: 0x04001A6A RID: 6762
		public string lpstrSchema;

		// Token: 0x04001A6B RID: 6763
		public ELEMDESC elemdescVar;

		// Token: 0x04001A6C RID: 6764
		public short wVarFlags;

		// Token: 0x04001A6D RID: 6765
		public VarEnum varkind;

		// Token: 0x02000544 RID: 1348
		[ComVisible(false)]
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04001A6E RID: 6766
			[FieldOffset(0)]
			public int oInst;

			// Token: 0x04001A6F RID: 6767
			[FieldOffset(0)]
			public IntPtr lpvarValue;
		}
	}
}
