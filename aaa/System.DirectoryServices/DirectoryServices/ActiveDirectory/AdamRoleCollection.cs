using System;
using System.Collections;
using System.ComponentModel;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000EB RID: 235
	public class AdamRoleCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000730 RID: 1840 RVA: 0x00025D0D File Offset: 0x00024D0D
		internal AdamRoleCollection()
		{
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00025D15 File Offset: 0x00024D15
		internal AdamRoleCollection(ArrayList values)
		{
			if (values != null)
			{
				base.InnerList.AddRange(values);
			}
		}

		// Token: 0x170001C7 RID: 455
		public AdamRole this[int index]
		{
			get
			{
				return (AdamRole)base.InnerList[index];
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00025D40 File Offset: 0x00024D40
		public bool Contains(AdamRole role)
		{
			if (role < AdamRole.SchemaRole || role > AdamRole.NamingRole)
			{
				throw new InvalidEnumArgumentException("role", (int)role, typeof(AdamRole));
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

		// Token: 0x06000734 RID: 1844 RVA: 0x00025D9C File Offset: 0x00024D9C
		public int IndexOf(AdamRole role)
		{
			if (role < AdamRole.SchemaRole || role > AdamRole.NamingRole)
			{
				throw new InvalidEnumArgumentException("role", (int)role, typeof(AdamRole));
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

		// Token: 0x06000735 RID: 1845 RVA: 0x00025DF6 File Offset: 0x00024DF6
		public void CopyTo(AdamRole[] roles, int index)
		{
			base.InnerList.CopyTo(roles, index);
		}
	}
}
