using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001B3 RID: 435
	// (Invoke) Token: 0x06000D50 RID: 3408
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ResolveNameEventHandler(object sender, ResolveNameEventArgs e);
}
