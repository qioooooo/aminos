using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006D0 RID: 1744
	[ComVisible(true)]
	public interface IClientChannelSinkProvider
	{
		// Token: 0x06003EF8 RID: 16120
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IClientChannelSink CreateSink(IChannelSender channel, string url, object remoteChannelData);

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06003EF9 RID: 16121
		// (set) Token: 0x06003EFA RID: 16122
		IClientChannelSinkProvider Next
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}
	}
}
