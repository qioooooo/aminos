using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053F RID: 1343
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.PARAMDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct PARAMDESC
	{
		// Token: 0x04001A61 RID: 6753
		public IntPtr lpVarValue;

		// Token: 0x04001A62 RID: 6754
		public PARAMFLAG wParamFlags;
	}
}
