using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000D1 RID: 209
	public class ReadOnlyActiveDirectorySchemaClassCollection : ReadOnlyCollectionBase
	{
		// Token: 0x0600067C RID: 1660 RVA: 0x00022B29 File Offset: 0x00021B29
		internal ReadOnlyActiveDirectorySchemaClassCollection()
		{
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00022B31 File Offset: 0x00021B31
		internal ReadOnlyActiveDirectorySchemaClassCollection(ICollection values)
		{
			if (values != null)
			{
				base.InnerList.AddRange(values);
			}
		}

		// Token: 0x17000187 RID: 391
		public ActiveDirectorySchemaClass this[int index]
		{
			get
			{
				return (ActiveDirectorySchemaClass)base.InnerList[index];
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00022B5C File Offset: 0x00021B5C
		public bool Contains(ActiveDirectorySchemaClass schemaClass)
		{
			if (schemaClass == null)
			{
				throw new ArgumentNullException("schemaClass");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaClass activeDirectorySchemaClass = (ActiveDirectorySchemaClass)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaClass.Name, schemaClass.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00022BB8 File Offset: 0x00021BB8
		public int IndexOf(ActiveDirectorySchemaClass schemaClass)
		{
			if (schemaClass == null)
			{
				throw new ArgumentNullException("schemaClass");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaClass activeDirectorySchemaClass = (ActiveDirectorySchemaClass)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaClass.Name, schemaClass.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00022C11 File Offset: 0x00021C11
		public void CopyTo(ActiveDirectorySchemaClass[] classes, int index)
		{
			base.InnerList.CopyTo(classes, index);
		}
	}
}
