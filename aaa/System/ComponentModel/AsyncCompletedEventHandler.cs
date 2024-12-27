using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000098 RID: 152
	// (Invoke) Token: 0x06000577 RID: 1399
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void AsyncCompletedEventHandler(object sender, AsyncCompletedEventArgs e);
}
