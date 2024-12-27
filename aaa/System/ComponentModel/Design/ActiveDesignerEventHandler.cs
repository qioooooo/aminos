using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000159 RID: 345
	// (Invoke) Token: 0x06000B52 RID: 2898
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ActiveDesignerEventHandler(object sender, ActiveDesignerEventArgs e);
}
