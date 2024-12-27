using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000161 RID: 353
	// (Invoke) Token: 0x06000B74 RID: 2932
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ComponentEventHandler(object sender, ComponentEventArgs e);
}
