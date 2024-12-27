using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006CD RID: 1741
	[ComVisible(true)]
	public interface IMethodReturnMessage : IMethodMessage, IMessage
	{
		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003EEE RID: 16110
		int OutArgCount
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x06003EEF RID: 16111
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		string GetOutArgName(int index);

		// Token: 0x06003EF0 RID: 16112
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		object GetOutArg(int argNum);

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003EF1 RID: 16113
		object[] OutArgs
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06003EF2 RID: 16114
		Exception Exception
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06003EF3 RID: 16115
		object ReturnValue
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}
	}
}
