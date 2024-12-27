using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006CB RID: 1739
	[ComVisible(true)]
	public interface IMethodCallMessage : IMethodMessage, IMessage
	{
		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06003EE4 RID: 16100
		int InArgCount
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x06003EE5 RID: 16101
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		string GetInArgName(int index);

		// Token: 0x06003EE6 RID: 16102
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		object GetInArg(int argNum);

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06003EE7 RID: 16103
		object[] InArgs
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}
	}
}
