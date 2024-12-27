using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x020002A2 RID: 674
	// (Invoke) Token: 0x06001634 RID: 5684
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void SessionSwitchEventHandler(object sender, SessionSwitchEventArgs e);
}
