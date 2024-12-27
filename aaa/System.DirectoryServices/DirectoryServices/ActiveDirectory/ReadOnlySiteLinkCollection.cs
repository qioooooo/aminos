using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000D6 RID: 214
	public class ReadOnlySiteLinkCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060006A0 RID: 1696 RVA: 0x00023141 File Offset: 0x00022141
		internal ReadOnlySiteLinkCollection()
		{
		}

		// Token: 0x1700018C RID: 396
		public ActiveDirectorySiteLink this[int index]
		{
			get
			{
				return (ActiveDirectorySiteLink)base.InnerList[index];
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0002315C File Offset: 0x0002215C
		public bool Contains(ActiveDirectorySiteLink link)
		{
			if (link == null)
			{
				throw new ArgumentNullException("link");
			}
			string text = (string)PropertyManager.GetPropertyValue(link.context, link.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySiteLink activeDirectorySiteLink = (ActiveDirectorySiteLink)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySiteLink.context, activeDirectorySiteLink.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000231E4 File Offset: 0x000221E4
		public int IndexOf(ActiveDirectorySiteLink link)
		{
			if (link == null)
			{
				throw new ArgumentNullException("link");
			}
			string text = (string)PropertyManager.GetPropertyValue(link.context, link.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySiteLink activeDirectorySiteLink = (ActiveDirectorySiteLink)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySiteLink.context, activeDirectorySiteLink.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0002326B File Offset: 0x0002226B
		public void CopyTo(ActiveDirectorySiteLink[] links, int index)
		{
			base.InnerList.CopyTo(links, index);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0002327A File Offset: 0x0002227A
		internal int Add(ActiveDirectorySiteLink link)
		{
			return base.InnerList.Add(link);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00023288 File Offset: 0x00022288
		internal void Clear()
		{
			base.InnerList.Clear();
		}
	}
}
