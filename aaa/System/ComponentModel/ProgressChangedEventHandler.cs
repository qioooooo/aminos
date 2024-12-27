using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000127 RID: 295
	// (Invoke) Token: 0x06000967 RID: 2407
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
}
