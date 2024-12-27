using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x0200029D RID: 669
	// (Invoke) Token: 0x06001626 RID: 5670
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public delegate void SessionEndedEventHandler(object sender, SessionEndedEventArgs e);
}
