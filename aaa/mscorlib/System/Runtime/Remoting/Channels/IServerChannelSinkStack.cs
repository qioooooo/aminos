using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006A3 RID: 1699
	[ComVisible(true)]
	public interface IServerChannelSinkStack : IServerResponseChannelSinkStack
	{
		// Token: 0x06003DB0 RID: 15792
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Push(IServerChannelSink sink, object state);

		// Token: 0x06003DB1 RID: 15793
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		object Pop(IServerChannelSink sink);

		// Token: 0x06003DB2 RID: 15794
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void Store(IServerChannelSink sink, object state);

		// Token: 0x06003DB3 RID: 15795
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void StoreAndDispatch(IServerChannelSink sink, object state);

		// Token: 0x06003DB4 RID: 15796
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void ServerCallback(IAsyncResult ar);
	}
}
