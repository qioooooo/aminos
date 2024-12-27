using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200052E RID: 1326
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.FILETIME instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	public struct FILETIME
	{
		// Token: 0x040019FB RID: 6651
		public int dwLowDateTime;

		// Token: 0x040019FC RID: 6652
		public int dwHighDateTime;
	}
}
