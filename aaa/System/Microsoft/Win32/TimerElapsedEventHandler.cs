using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x020002A7 RID: 679
	// (Invoke) Token: 0x0600168A RID: 5770
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void TimerElapsedEventHandler(object sender, TimerElapsedEventArgs e);
}
