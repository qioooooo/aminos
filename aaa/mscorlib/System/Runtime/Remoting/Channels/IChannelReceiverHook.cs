using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006CF RID: 1743
	[ComVisible(true)]
	public interface IChannelReceiverHook
	{
		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06003EF4 RID: 16116
		string ChannelScheme
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06003EF5 RID: 16117
		bool WantsToListen
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06003EF6 RID: 16118
		IServerChannelSink ChannelSinkChain
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x06003EF7 RID: 16119
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void AddHookChannelUri(string channelUri);
	}
}
