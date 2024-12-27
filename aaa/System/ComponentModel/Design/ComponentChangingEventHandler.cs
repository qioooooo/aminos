using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200015F RID: 351
	// (Invoke) Token: 0x06000B6E RID: 2926
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ComponentChangingEventHandler(object sender, ComponentChangingEventArgs e);
}
