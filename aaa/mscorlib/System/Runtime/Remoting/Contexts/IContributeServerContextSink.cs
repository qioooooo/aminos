using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006E4 RID: 1764
	[ComVisible(true)]
	public interface IContributeServerContextSink
	{
		// Token: 0x06003F46 RID: 16198
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IMessageSink GetServerContextSink(IMessageSink nextSink);
	}
}
