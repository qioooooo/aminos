using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006CA RID: 1738
	[ComVisible(true)]
	public interface IMethodMessage : IMessage
	{
		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003ED9 RID: 16089
		string Uri
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003EDA RID: 16090
		string MethodName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003EDB RID: 16091
		string TypeName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06003EDC RID: 16092
		object MethodSignature
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06003EDD RID: 16093
		int ArgCount
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x06003EDE RID: 16094
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		string GetArgName(int index);

		// Token: 0x06003EDF RID: 16095
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		object GetArg(int argNum);

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06003EE0 RID: 16096
		object[] Args
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06003EE1 RID: 16097
		bool HasVarArgs
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06003EE2 RID: 16098
		LogicalCallContext LogicalCallContext
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06003EE3 RID: 16099
		MethodBase MethodBase
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
		}
	}
}
