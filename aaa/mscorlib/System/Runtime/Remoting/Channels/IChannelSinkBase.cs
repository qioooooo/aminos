using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006C0 RID: 1728
	[ComVisible(true)]
	public interface IChannelSinkBase
	{
		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003EB8 RID: 16056
		IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}
	}
}
