using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000629 RID: 1577
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class PrincipalPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x0600394A RID: 14666 RVA: 0x000C1F3C File Offset: 0x000C0F3C
		public PrincipalPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x000C1F4C File Offset: 0x000C0F4C
		// (set) Token: 0x0600394C RID: 14668 RVA: 0x000C1F54 File Offset: 0x000C0F54
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x000C1F5D File Offset: 0x000C0F5D
		// (set) Token: 0x0600394E RID: 14670 RVA: 0x000C1F65 File Offset: 0x000C0F65
		public string Role
		{
			get
			{
				return this.m_role;
			}
			set
			{
				this.m_role = value;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x0600394F RID: 14671 RVA: 0x000C1F6E File Offset: 0x000C0F6E
		// (set) Token: 0x06003950 RID: 14672 RVA: 0x000C1F76 File Offset: 0x000C0F76
		public bool Authenticated
		{
			get
			{
				return this.m_authenticated;
			}
			set
			{
				this.m_authenticated = value;
			}
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x000C1F7F File Offset: 0x000C0F7F
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new PrincipalPermission(PermissionState.Unrestricted);
			}
			return new PrincipalPermission(this.m_name, this.m_role, this.m_authenticated);
		}

		// Token: 0x04001DB9 RID: 7609
		private string m_name;

		// Token: 0x04001DBA RID: 7610
		private string m_role;

		// Token: 0x04001DBB RID: 7611
		private bool m_authenticated = true;
	}
}
