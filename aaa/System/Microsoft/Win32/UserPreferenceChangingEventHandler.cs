using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x020002AC RID: 684
	// (Invoke) Token: 0x06001696 RID: 5782
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void UserPreferenceChangingEventHandler(object sender, UserPreferenceChangingEventArgs e);
}
