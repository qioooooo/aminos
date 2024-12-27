using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200069E RID: 1694
	[ComVisible(true)]
	public interface IClientResponseChannelSinkStack
	{
		// Token: 0x06003DA1 RID: 15777
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void AsyncProcessResponse(ITransportHeaders headers, Stream stream);

		// Token: 0x06003DA2 RID: 15778
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void DispatchReplyMessage(IMessage msg);

		// Token: 0x06003DA3 RID: 15779
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void DispatchException(Exception e);
	}
}
