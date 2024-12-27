using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000175 RID: 373
	// (Invoke) Token: 0x06000C12 RID: 3090
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void DesignerEventHandler(object sender, DesignerEventArgs e);
}
