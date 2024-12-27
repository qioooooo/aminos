using System;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006EF RID: 1775
	public interface ISecurableChannel
	{
		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06003F9C RID: 16284
		// (set) Token: 0x06003F9D RID: 16285
		bool IsSecured
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}
	}
}
