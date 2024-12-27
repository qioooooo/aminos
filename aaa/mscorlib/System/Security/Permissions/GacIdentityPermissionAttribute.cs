using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000645 RID: 1605
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class GacIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003A63 RID: 14947 RVA: 0x000C6097 File Offset: 0x000C5097
		public GacIdentityPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x000C60A0 File Offset: 0x000C50A0
		public override IPermission CreatePermission()
		{
			return new GacIdentityPermission();
		}
	}
}
