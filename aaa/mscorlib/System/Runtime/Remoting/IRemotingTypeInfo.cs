using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x0200071A RID: 1818
	[ComVisible(true)]
	public interface IRemotingTypeInfo
	{
		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060041A2 RID: 16802
		// (set) Token: 0x060041A3 RID: 16803
		string TypeName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get;
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			set;
		}

		// Token: 0x060041A4 RID: 16804
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		bool CanCastTo(Type fromType, object o);
	}
}
