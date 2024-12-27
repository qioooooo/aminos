using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006A2 RID: 1698
	[ComVisible(true)]
	public interface IServerResponseChannelSinkStack
	{
		// Token: 0x06003DAE RID: 15790
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void AsyncProcessResponse(IMessage msg, ITransportHeaders headers, Stream stream);

		// Token: 0x06003DAF RID: 15791
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		Stream GetResponseStream(IMessage msg, ITransportHeaders headers);
	}
}
