using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x0200071C RID: 1820
	[ComVisible(true)]
	public interface IEnvoyInfo
	{
		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x060041A7 RID: 16807
		// (set) Token: 0x060041A8 RID: 16808
		IMessageSink EnvoySinks
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}
	}
}
