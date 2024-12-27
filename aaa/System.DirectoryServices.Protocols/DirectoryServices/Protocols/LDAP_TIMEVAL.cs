using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000094 RID: 148
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LDAP_TIMEVAL
	{
		// Token: 0x040002E9 RID: 745
		public int tv_sec;

		// Token: 0x040002EA RID: 746
		public int tv_usec;
	}
}
