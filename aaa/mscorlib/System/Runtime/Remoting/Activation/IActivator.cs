using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x02000687 RID: 1671
	[ComVisible(true)]
	public interface IActivator
	{
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06003CF4 RID: 15604
		// (set) Token: 0x06003CF5 RID: 15605
		IActivator NextActivator
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}

		// Token: 0x06003CF6 RID: 15606
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IConstructionReturnMessage Activate(IConstructionCallMessage msg);

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06003CF7 RID: 15607
		ActivatorLevel Level
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}
	}
}
