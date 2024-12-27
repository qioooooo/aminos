using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000577 RID: 1399
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct VARDESC
	{
		// Token: 0x04001B38 RID: 6968
		public int memid;

		// Token: 0x04001B39 RID: 6969
		public string lpstrSchema;

		// Token: 0x04001B3A RID: 6970
		public VARDESC.DESCUNION desc;

		// Token: 0x04001B3B RID: 6971
		public ELEMDESC elemdescVar;

		// Token: 0x04001B3C RID: 6972
		public short wVarFlags;

		// Token: 0x04001B3D RID: 6973
		public VARKIND varkind;

		// Token: 0x02000578 RID: 1400
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04001B3E RID: 6974
			[FieldOffset(0)]
			public int oInst;

			// Token: 0x04001B3F RID: 6975
			[FieldOffset(0)]
			public IntPtr lpvarValue;
		}
	}
}
