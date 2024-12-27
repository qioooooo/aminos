using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000136 RID: 310
	// (Invoke) Token: 0x06000A34 RID: 2612
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void RefreshEventHandler(RefreshEventArgs e);
}
