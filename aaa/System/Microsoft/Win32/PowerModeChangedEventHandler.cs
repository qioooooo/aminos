using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000298 RID: 664
	// (Invoke) Token: 0x0600160C RID: 5644
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void PowerModeChangedEventHandler(object sender, PowerModeChangedEventArgs e);
}
