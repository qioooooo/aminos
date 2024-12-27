using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x0200071B RID: 1819
	[ComVisible(true)]
	public interface IChannelInfo
	{
		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x060041A5 RID: 16805
		// (set) Token: 0x060041A6 RID: 16806
		object[] ChannelData
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}
	}
}
