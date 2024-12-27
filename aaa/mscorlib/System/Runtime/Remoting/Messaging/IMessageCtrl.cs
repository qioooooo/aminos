using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006ED RID: 1773
	[ComVisible(true)]
	public interface IMessageCtrl
	{
		// Token: 0x06003F99 RID: 16281
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Cancel(int msToCancel);
	}
}
