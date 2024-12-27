using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000B0 RID: 176
	public class GlobalCatalogCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060005DC RID: 1500 RVA: 0x00022024 File Offset: 0x00021024
		internal GlobalCatalogCollection()
		{
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0002202C File Offset: 0x0002102C
		internal GlobalCatalogCollection(ArrayList values)
		{
			if (values != null)
			{
				base.InnerList.AddRange(values);
			}
		}

		// Token: 0x17000165 RID: 357
		public GlobalCatalog this[int index]
		{
			get
			{
				return (GlobalCatalog)base.InnerList[index];
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00022058 File Offset: 0x00021058
		public bool Contains(GlobalCatalog globalCatalog)
		{
			if (globalCatalog == null)
			{
				throw new ArgumentNullException("globalCatalog");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				GlobalCatalog globalCatalog2 = (GlobalCatalog)base.InnerList[i];
				if (Utils.Compare(globalCatalog2.Name, globalCatalog.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000220B4 File Offset: 0x000210B4
		public int IndexOf(GlobalCatalog globalCatalog)
		{
			if (globalCatalog == null)
			{
				throw new ArgumentNullException("globalCatalog");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				GlobalCatalog globalCatalog2 = (GlobalCatalog)base.InnerList[i];
				if (Utils.Compare(globalCatalog2.Name, globalCatalog.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0002210D File Offset: 0x0002110D
		public void CopyTo(GlobalCatalog[] globalCatalogs, int index)
		{
			base.InnerList.CopyTo(globalCatalogs, index);
		}
	}
}
