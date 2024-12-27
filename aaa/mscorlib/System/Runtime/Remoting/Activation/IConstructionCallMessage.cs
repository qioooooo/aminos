using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x020006CC RID: 1740
	[ComVisible(true)]
	public interface IConstructionCallMessage : IMethodCallMessage, IMethodMessage, IMessage
	{
		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003EE8 RID: 16104
		// (set) Token: 0x06003EE9 RID: 16105
		IActivator Activator
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06003EEA RID: 16106
		object[] CallSiteActivationAttributes
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06003EEB RID: 16107
		string ActivationTypeName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003EEC RID: 16108
		Type ActivationType
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003EED RID: 16109
		IList ContextProperties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}
	}
}
