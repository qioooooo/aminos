using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020003A7 RID: 935
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class DnsPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06001D41 RID: 7489 RVA: 0x0006FE99 File Offset: 0x0006EE99
		public DnsPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x0006FEA2 File Offset: 0x0006EEA2
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new DnsPermission(PermissionState.Unrestricted);
			}
			return new DnsPermission(PermissionState.None);
		}
	}
}
