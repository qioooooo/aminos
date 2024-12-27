using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200062E RID: 1582
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class ZoneIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06003994 RID: 14740 RVA: 0x000C252B File Offset: 0x000C152B
		public ZoneIdentityPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x000C253B File Offset: 0x000C153B
		// (set) Token: 0x06003996 RID: 14742 RVA: 0x000C2543 File Offset: 0x000C1543
		public SecurityZone Zone
		{
			get
			{
				return this.m_flag;
			}
			set
			{
				this.m_flag = value;
			}
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x000C254C File Offset: 0x000C154C
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new ZoneIdentityPermission(PermissionState.Unrestricted);
			}
			return new ZoneIdentityPermission(this.m_flag);
		}

		// Token: 0x04001DC5 RID: 7621
		private SecurityZone m_flag = SecurityZone.NoZone;
	}
}
