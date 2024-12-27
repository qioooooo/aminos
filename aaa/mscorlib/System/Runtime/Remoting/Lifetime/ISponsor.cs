using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x020006A7 RID: 1703
	[ComVisible(true)]
	public interface ISponsor
	{
		// Token: 0x06003DC1 RID: 15809
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		TimeSpan Renewal(ILease lease);
	}
}
