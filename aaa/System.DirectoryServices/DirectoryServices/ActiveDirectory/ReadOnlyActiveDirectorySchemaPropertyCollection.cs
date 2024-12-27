using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000D2 RID: 210
	public class ReadOnlyActiveDirectorySchemaPropertyCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000682 RID: 1666 RVA: 0x00022C20 File Offset: 0x00021C20
		internal ReadOnlyActiveDirectorySchemaPropertyCollection()
		{
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00022C28 File Offset: 0x00021C28
		internal ReadOnlyActiveDirectorySchemaPropertyCollection(ArrayList values)
		{
			if (values != null)
			{
				base.InnerList.AddRange(values);
			}
		}

		// Token: 0x17000188 RID: 392
		public ActiveDirectorySchemaProperty this[int index]
		{
			get
			{
				return (ActiveDirectorySchemaProperty)base.InnerList[index];
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00022C54 File Offset: 0x00021C54
		public bool Contains(ActiveDirectorySchemaProperty schemaProperty)
		{
			if (schemaProperty == null)
			{
				throw new ArgumentNullException("schemaProperty");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaProperty activeDirectorySchemaProperty = (ActiveDirectorySchemaProperty)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaProperty.Name, schemaProperty.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00022CB0 File Offset: 0x00021CB0
		public int IndexOf(ActiveDirectorySchemaProperty schemaProperty)
		{
			if (schemaProperty == null)
			{
				throw new ArgumentNullException("schemaProperty");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaProperty activeDirectorySchemaProperty = (ActiveDirectorySchemaProperty)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaProperty.Name, schemaProperty.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00022D09 File Offset: 0x00021D09
		public void CopyTo(ActiveDirectorySchemaProperty[] properties, int index)
		{
			base.InnerList.CopyTo(properties, index);
		}
	}
}
