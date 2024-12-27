using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x020001C0 RID: 448
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	internal class COAUTHIDENTITY
	{
		// Token: 0x06001999 RID: 6553 RVA: 0x000792F0 File Offset: 0x000782F0
		internal COAUTHIDENTITY(string usr, string dom, string pwd)
		{
			this.user = usr;
			this.userlen = ((this.user == null) ? 0 : this.user.Length);
			this.domain = dom;
			this.domainlen = ((this.domain == null) ? 0 : this.domain.Length);
			this.password = pwd;
			this.passwordlen = ((this.password == null) ? 0 : this.password.Length);
		}

		// Token: 0x04001765 RID: 5989
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string user;

		// Token: 0x04001766 RID: 5990
		internal int userlen;

		// Token: 0x04001767 RID: 5991
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string domain;

		// Token: 0x04001768 RID: 5992
		internal int domainlen;

		// Token: 0x04001769 RID: 5993
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string password;

		// Token: 0x0400176A RID: 5994
		internal int passwordlen;

		// Token: 0x0400176B RID: 5995
		internal int flags = 2;
	}
}
