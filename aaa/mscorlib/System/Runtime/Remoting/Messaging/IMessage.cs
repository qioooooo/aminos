using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006C9 RID: 1737
	[ComVisible(true)]
	public interface IMessage
	{
		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003ED8 RID: 16088
		IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}
	}
}
