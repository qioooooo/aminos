using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200056E RID: 1390
	public struct FUNCDESC
	{
		// Token: 0x04001B0E RID: 6926
		public int memid;

		// Token: 0x04001B0F RID: 6927
		public IntPtr lprgscode;

		// Token: 0x04001B10 RID: 6928
		public IntPtr lprgelemdescParam;

		// Token: 0x04001B11 RID: 6929
		public FUNCKIND funckind;

		// Token: 0x04001B12 RID: 6930
		public INVOKEKIND invkind;

		// Token: 0x04001B13 RID: 6931
		public CALLCONV callconv;

		// Token: 0x04001B14 RID: 6932
		public short cParams;

		// Token: 0x04001B15 RID: 6933
		public short cParamsOpt;

		// Token: 0x04001B16 RID: 6934
		public short oVft;

		// Token: 0x04001B17 RID: 6935
		public short cScodes;

		// Token: 0x04001B18 RID: 6936
		public ELEMDESC elemdescFunc;

		// Token: 0x04001B19 RID: 6937
		public short wFuncFlags;
	}
}
