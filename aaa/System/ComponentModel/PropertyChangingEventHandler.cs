using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200012B RID: 299
	// (Invoke) Token: 0x06000973 RID: 2419
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void PropertyChangingEventHandler(object sender, PropertyChangingEventArgs e);
}
