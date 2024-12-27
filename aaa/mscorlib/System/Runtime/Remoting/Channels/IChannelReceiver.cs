using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B8 RID: 1720
	[ComVisible(true)]
	public interface IChannelReceiver : IChannel
	{
		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06003E81 RID: 16001
		object ChannelData
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x06003E82 RID: 16002
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		string[] GetUrlsForUri(string objectURI);

		// Token: 0x06003E83 RID: 16003
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void StartListening(object data);

		// Token: 0x06003E84 RID: 16004
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void StopListening(object data);
	}
}
