using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200068F RID: 1679
	[ComVisible(true)]
	public interface IMessageSink
	{
		// Token: 0x06003D1F RID: 15647
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IMessage SyncProcessMessage(IMessage msg);

		// Token: 0x06003D20 RID: 15648
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink);

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06003D21 RID: 15649
		IMessageSink NextSink
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}
	}
}
