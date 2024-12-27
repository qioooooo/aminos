using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000246 RID: 582
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal sealed class tagDBLITERALINFO
	{
		// Token: 0x0600206C RID: 8300 RVA: 0x00262AE8 File Offset: 0x00261EE8
		internal tagDBLITERALINFO()
		{
		}

		// Token: 0x040014C4 RID: 5316
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pwszLiteralValue;

		// Token: 0x040014C5 RID: 5317
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pwszInvalidChars;

		// Token: 0x040014C6 RID: 5318
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pwszInvalidStartingChars;

		// Token: 0x040014C7 RID: 5319
		internal int it;

		// Token: 0x040014C8 RID: 5320
		internal int fSupported;

		// Token: 0x040014C9 RID: 5321
		internal int cchMaxLen;
	}
}
