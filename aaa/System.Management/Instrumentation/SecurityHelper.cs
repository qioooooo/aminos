using System;
using System.Security.Permissions;

namespace System.Management.Instrumentation
{
	// Token: 0x020000A1 RID: 161
	internal sealed class SecurityHelper
	{
		// Token: 0x04000286 RID: 646
		internal static readonly SecurityPermission UnmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
	}
}
