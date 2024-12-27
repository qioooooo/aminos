using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x020001C1 RID: 449
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	internal class COAUTHIDENTITY_X64
	{
		// Token: 0x0600199A RID: 6554 RVA: 0x00079374 File Offset: 0x00078374
		internal COAUTHIDENTITY_X64(string usr, string dom, string pwd)
		{
			this.user = usr;
			this.userlen = ((this.user == null) ? 0 : this.user.Length);
			this.domain = dom;
			this.domainlen = ((this.domain == null) ? 0 : this.domain.Length);
			this.password = pwd;
			this.passwordlen = ((this.password == null) ? 0 : this.password.Length);
		}

		// Token: 0x0400176C RID: 5996
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string user;

		// Token: 0x0400176D RID: 5997
		internal int userlen;

		// Token: 0x0400176E RID: 5998
		internal int padding1;

		// Token: 0x0400176F RID: 5999
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string domain;

		// Token: 0x04001770 RID: 6000
		internal int domainlen;

		// Token: 0x04001771 RID: 6001
		internal int padding2;

		// Token: 0x04001772 RID: 6002
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string password;

		// Token: 0x04001773 RID: 6003
		internal int passwordlen;

		// Token: 0x04001774 RID: 6004
		internal int flags = 2;
	}
}
