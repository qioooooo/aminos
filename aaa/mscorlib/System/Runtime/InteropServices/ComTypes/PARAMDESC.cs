using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000572 RID: 1394
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct PARAMDESC
	{
		// Token: 0x04001B2B RID: 6955
		public IntPtr lpVarValue;

		// Token: 0x04001B2C RID: 6956
		public PARAMFLAG wParamFlags;
	}
}
