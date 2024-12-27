using System;
using System.Security.Util;

namespace System.Security
{
	// Token: 0x02000662 RID: 1634
	internal struct PermissionSetEnumeratorInternal
	{
		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06003B92 RID: 15250 RVA: 0x000CC0AE File Offset: 0x000CB0AE
		public object Current
		{
			get
			{
				return this.enm.Current;
			}
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x000CC0BB File Offset: 0x000CB0BB
		internal PermissionSetEnumeratorInternal(PermissionSet permSet)
		{
			this.m_permSet = permSet;
			this.enm = new TokenBasedSetEnumerator(permSet.m_permSet);
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x000CC0D5 File Offset: 0x000CB0D5
		public int GetCurrentIndex()
		{
			return this.enm.Index;
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x000CC0E2 File Offset: 0x000CB0E2
		public void Reset()
		{
			this.enm.Reset();
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x000CC0F0 File Offset: 0x000CB0F0
		public bool MoveNext()
		{
			while (this.enm.MoveNext())
			{
				object current = this.enm.Current;
				IPermission permission = current as IPermission;
				if (permission != null)
				{
					this.enm.Current = permission;
					return true;
				}
				SecurityElement securityElement = current as SecurityElement;
				if (securityElement != null)
				{
					permission = this.m_permSet.CreatePermission(securityElement, this.enm.Index);
					if (permission != null)
					{
						this.enm.Current = permission;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04001E8E RID: 7822
		private PermissionSet m_permSet;

		// Token: 0x04001E8F RID: 7823
		private TokenBasedSetEnumerator enm;
	}
}
