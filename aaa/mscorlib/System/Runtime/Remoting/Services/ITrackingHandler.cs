using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Services
{
	// Token: 0x02000791 RID: 1937
	[ComVisible(true)]
	public interface ITrackingHandler
	{
		// Token: 0x0600455C RID: 17756
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void MarshaledObject(object obj, ObjRef or);

		// Token: 0x0600455D RID: 17757
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void UnmarshaledObject(object obj, ObjRef or);

		// Token: 0x0600455E RID: 17758
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void DisconnectedObject(object obj);
	}
}
