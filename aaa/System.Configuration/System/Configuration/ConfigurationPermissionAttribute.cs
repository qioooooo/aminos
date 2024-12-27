using System;
using System.Security;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x02000035 RID: 53
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class ConfigurationPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06000289 RID: 649 RVA: 0x00010330 File Offset: 0x0000F330
		public ConfigurationPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0001033C File Offset: 0x0000F33C
		public override IPermission CreatePermission()
		{
			PermissionState permissionState = (base.Unrestricted ? PermissionState.Unrestricted : PermissionState.None);
			return new ConfigurationPermission(permissionState);
		}
	}
}
