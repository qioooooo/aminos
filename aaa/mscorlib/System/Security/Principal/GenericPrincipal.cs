using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004B1 RID: 1201
	[ComVisible(true)]
	[Serializable]
	public class GenericPrincipal : IPrincipal
	{
		// Token: 0x0600309C RID: 12444 RVA: 0x000A7790 File Offset: 0x000A6790
		public GenericPrincipal(IIdentity identity, string[] roles)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.m_identity = identity;
			if (roles != null)
			{
				this.m_roles = new string[roles.Length];
				for (int i = 0; i < roles.Length; i++)
				{
					this.m_roles[i] = roles[i];
				}
				return;
			}
			this.m_roles = null;
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x0600309D RID: 12445 RVA: 0x000A77EA File Offset: 0x000A67EA
		public virtual IIdentity Identity
		{
			get
			{
				return this.m_identity;
			}
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x000A77F4 File Offset: 0x000A67F4
		public virtual bool IsInRole(string role)
		{
			if (role == null || this.m_roles == null)
			{
				return false;
			}
			for (int i = 0; i < this.m_roles.Length; i++)
			{
				if (this.m_roles[i] != null && string.Compare(this.m_roles[i], role, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400182E RID: 6190
		private IIdentity m_identity;

		// Token: 0x0400182F RID: 6191
		private string[] m_roles;
	}
}
