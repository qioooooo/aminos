using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x0200048F RID: 1167
	[ComVisible(true)]
	public interface IIdentityPermissionFactory
	{
		// Token: 0x06002EE2 RID: 12002
		IPermission CreateIdentityPermission(Evidence evidence);
	}
}
