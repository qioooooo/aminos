using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B7 RID: 1719
	[ComVisible(true)]
	public interface IChannelSender : IChannel
	{
		// Token: 0x06003E80 RID: 16000
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IMessageSink CreateMessageSink(string url, object remoteChannelData, out string objectURI);
	}
}
