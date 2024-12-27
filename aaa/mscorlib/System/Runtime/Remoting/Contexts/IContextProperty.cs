using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000685 RID: 1669
	[ComVisible(true)]
	public interface IContextProperty
	{
		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06003CE9 RID: 15593
		string Name
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x06003CEA RID: 15594
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool IsNewContextOK(Context newCtx);

		// Token: 0x06003CEB RID: 15595
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Freeze(Context newContext);
	}
}
