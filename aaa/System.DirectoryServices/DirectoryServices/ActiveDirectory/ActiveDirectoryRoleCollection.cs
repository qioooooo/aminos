using System;
using System.Collections;
using System.ComponentModel;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000EA RID: 234
	public class ActiveDirectoryRoleCollection : ReadOnlyCollectionBase
	{
		// Token: 0x0600072A RID: 1834 RVA: 0x00025C16 File Offset: 0x00024C16
		internal ActiveDirectoryRoleCollection()
		{
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00025C1E File Offset: 0x00024C1E
		internal ActiveDirectoryRoleCollection(ArrayList values)
		{
			if (values != null)
			{
				base.InnerList.AddRange(values);
			}
		}

		// Token: 0x170001C6 RID: 454
		public ActiveDirectoryRole this[int index]
		{
			get
			{
				return (ActiveDirectoryRole)base.InnerList[index];
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00025C48 File Offset: 0x00024C48
		public bool Contains(ActiveDirectoryRole role)
		{
			if (role < ActiveDirectoryRole.SchemaRole || role > ActiveDirectoryRole.InfrastructureRole)
			{
				throw new InvalidEnumArgumentException("role", (int)role, typeof(ActiveDirectoryRole));
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				int num = (int)base.InnerList[i];
				if (num == (int)role)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00025CA4 File Offset: 0x00024CA4
		public int IndexOf(ActiveDirectoryRole role)
		{
			if (role < ActiveDirectoryRole.SchemaRole || role > ActiveDirectoryRole.InfrastructureRole)
			{
				throw new InvalidEnumArgumentException("role", (int)role, typeof(ActiveDirectoryRole));
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				int num = (int)base.InnerList[i];
				if (num == (int)role)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00025CFE File Offset: 0x00024CFE
		public void CopyTo(ActiveDirectoryRole[] roles, int index)
		{
			base.InnerList.CopyTo(roles, index);
		}
	}
}
