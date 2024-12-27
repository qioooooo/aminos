using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200015D RID: 349
	// (Invoke) Token: 0x06000B67 RID: 2919
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ComponentChangedEventHandler(object sender, ComponentChangedEventArgs e);
}
