using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200008C RID: 140
	// (Invoke) Token: 0x060004E1 RID: 1249
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void AddingNewEventHandler(object sender, AddingNewEventArgs e);
}
