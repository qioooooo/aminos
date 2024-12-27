using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000163 RID: 355
	// (Invoke) Token: 0x06000B7C RID: 2940
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate void ComponentRenameEventHandler(object sender, ComponentRenameEventArgs e);
}
