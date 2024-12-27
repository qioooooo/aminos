using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006D8 RID: 1752
	[ComVisible(true)]
	public interface ITransportHeaders
	{
		// Token: 0x17000AA9 RID: 2729
		object this[object key]
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}

		// Token: 0x06003F0C RID: 16140
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IEnumerator GetEnumerator();
	}
}
