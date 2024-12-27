using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053D RID: 1341
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IDLDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct IDLDESC
	{
		// Token: 0x04001A56 RID: 6742
		public int dwReserved;

		// Token: 0x04001A57 RID: 6743
		public IDLFLAG wIDLFlags;
	}
}
