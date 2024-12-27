using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006BE RID: 1726
	[ComVisible(true)]
	public interface IServerChannelSinkProvider
	{
		// Token: 0x06003EAF RID: 16047
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		void GetChannelData(IChannelDataStore channelData);

		// Token: 0x06003EB0 RID: 16048
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IServerChannelSink CreateSink(IChannelReceiver channel);

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06003EB1 RID: 16049
		// (set) Token: 0x06003EB2 RID: 16050
		IServerChannelSinkProvider Next
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}
	}
}
