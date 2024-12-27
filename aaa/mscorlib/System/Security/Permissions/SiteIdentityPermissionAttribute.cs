using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000630 RID: 1584
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class SiteIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039A0 RID: 14752 RVA: 0x000C2642 File Offset: 0x000C1642
		public SiteIdentityPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060039A1 RID: 14753 RVA: 0x000C264B File Offset: 0x000C164B
		// (set) Token: 0x060039A2 RID: 14754 RVA: 0x000C2653 File Offset: 0x000C1653
		public string Site
		{
			get
			{
				return this.m_site;
			}
			set
			{
				this.m_site = value;
			}
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x000C265C File Offset: 0x000C165C
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new SiteIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.m_site == null)
			{
				return new SiteIdentityPermission(PermissionState.None);
			}
			return new SiteIdentityPermission(this.m_site);
		}

		// Token: 0x04001DC9 RID: 7625
		private string m_site;
	}
}
