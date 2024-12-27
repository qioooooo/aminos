using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000631 RID: 1585
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039A4 RID: 14756 RVA: 0x000C2687 File Offset: 0x000C1687
		public UrlIdentityPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060039A5 RID: 14757 RVA: 0x000C2690 File Offset: 0x000C1690
		// (set) Token: 0x060039A6 RID: 14758 RVA: 0x000C2698 File Offset: 0x000C1698
		public string Url
		{
			get
			{
				return this.m_url;
			}
			set
			{
				this.m_url = value;
			}
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x000C26A1 File Offset: 0x000C16A1
		public override IPermission CreatePermission()
		{
			if (this.m_unrestricted)
			{
				return new UrlIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.m_url == null)
			{
				return new UrlIdentityPermission(PermissionState.None);
			}
			return new UrlIdentityPermission(this.m_url);
		}

		// Token: 0x04001DCA RID: 7626
		private string m_url;
	}
}
