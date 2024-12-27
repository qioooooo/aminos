using System;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006EB RID: 1771
	internal interface IInternalMessage
	{
		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06003F86 RID: 16262
		// (set) Token: 0x06003F87 RID: 16263
		ServerIdentity ServerIdentityObject
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06003F88 RID: 16264
		// (set) Token: 0x06003F89 RID: 16265
		Identity IdentityObject
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}

		// Token: 0x06003F8A RID: 16266
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void SetURI(string uri);

		// Token: 0x06003F8B RID: 16267
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void SetCallContext(LogicalCallContext callContext);

		// Token: 0x06003F8C RID: 16268
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool HasProperties();
	}
}
