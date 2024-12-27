using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x0200029F RID: 671
	// (Invoke) Token: 0x0600162E RID: 5678
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void SessionEndingEventHandler(object sender, SessionEndingEventArgs e);
}
