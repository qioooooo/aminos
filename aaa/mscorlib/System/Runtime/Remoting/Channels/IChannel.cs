using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006B6 RID: 1718
	[ComVisible(true)]
	public interface IChannel
	{
		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06003E7D RID: 15997
		int ChannelPriority
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06003E7E RID: 15998
		string ChannelName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x06003E7F RID: 15999
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		string Parse(string url, out string objectURI);
	}
}
