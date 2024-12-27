using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x020002AA RID: 682
	// (Invoke) Token: 0x06001690 RID: 5776
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void UserPreferenceChangedEventHandler(object sender, UserPreferenceChangedEventArgs e);
}
