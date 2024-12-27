using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000D4 RID: 212
	public class ReadOnlySiteCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000691 RID: 1681 RVA: 0x00022E5D File Offset: 0x00021E5D
		internal ReadOnlySiteCollection()
		{
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00022E68 File Offset: 0x00021E68
		internal ReadOnlySiteCollection(ArrayList sites)
		{
			for (int i = 0; i < sites.Count; i++)
			{
				this.Add((ActiveDirectorySite)sites[i]);
			}
		}

		// Token: 0x1700018A RID: 394
		public ActiveDirectorySite this[int index]
		{
			get
			{
				return (ActiveDirectorySite)base.InnerList[index];
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00022EB4 File Offset: 0x00021EB4
		public bool Contains(ActiveDirectorySite site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			string text = (string)PropertyManager.GetPropertyValue(site.context, site.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySite activeDirectorySite = (ActiveDirectorySite)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySite.context, activeDirectorySite.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00022F3C File Offset: 0x00021F3C
		public int IndexOf(ActiveDirectorySite site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			string text = (string)PropertyManager.GetPropertyValue(site.context, site.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySite activeDirectorySite = (ActiveDirectorySite)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySite.context, activeDirectorySite.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00022FC3 File Offset: 0x00021FC3
		public void CopyTo(ActiveDirectorySite[] sites, int index)
		{
			base.InnerList.CopyTo(sites, index);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00022FD2 File Offset: 0x00021FD2
		internal int Add(ActiveDirectorySite site)
		{
			return base.InnerList.Add(site);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00022FE0 File Offset: 0x00021FE0
		internal void Clear()
		{
			base.InnerList.Clear();
		}
	}
}
