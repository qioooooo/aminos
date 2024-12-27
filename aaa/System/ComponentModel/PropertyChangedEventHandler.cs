using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000129 RID: 297
	// (Invoke) Token: 0x0600096D RID: 2413
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);
}
