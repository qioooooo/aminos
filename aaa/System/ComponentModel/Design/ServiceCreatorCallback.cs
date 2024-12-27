using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200019A RID: 410
	// (Invoke) Token: 0x06000CD0 RID: 3280
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public delegate object ServiceCreatorCallback(IServiceContainer container, Type serviceType);
}
