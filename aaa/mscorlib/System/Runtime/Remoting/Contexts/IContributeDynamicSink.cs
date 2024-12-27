using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006E1 RID: 1761
	[ComVisible(true)]
	public interface IContributeDynamicSink
	{
		// Token: 0x06003F43 RID: 16195
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IDynamicMessageSink GetDynamicSink();
	}
}
