using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006E0 RID: 1760
	[ComVisible(true)]
	public interface IContributeClientContextSink
	{
		// Token: 0x06003F42 RID: 16194
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		IMessageSink GetClientContextSink(IMessageSink nextSink);
	}
}
