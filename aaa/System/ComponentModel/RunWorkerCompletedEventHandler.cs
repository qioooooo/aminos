using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000139 RID: 313
	// (Invoke) Token: 0x06000A41 RID: 2625
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void RunWorkerCompletedEventHandler(object sender, RunWorkerCompletedEventArgs e);
}
