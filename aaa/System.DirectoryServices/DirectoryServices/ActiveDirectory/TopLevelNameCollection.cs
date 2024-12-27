using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000F3 RID: 243
	public class TopLevelNameCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000742 RID: 1858 RVA: 0x00025EE7 File Offset: 0x00024EE7
		internal TopLevelNameCollection()
		{
		}

		// Token: 0x170001CA RID: 458
		public TopLevelName this[int index]
		{
			get
			{
				return (TopLevelName)base.InnerList[index];
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00025F02 File Offset: 0x00024F02
		public bool Contains(TopLevelName name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return base.InnerList.Contains(name);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00025F1E File Offset: 0x00024F1E
		public int IndexOf(TopLevelName name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return base.InnerList.IndexOf(name);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00025F3A File Offset: 0x00024F3A
		public void CopyTo(TopLevelName[] names, int index)
		{
			base.InnerList.CopyTo(names, index);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00025F49 File Offset: 0x00024F49
		internal int Add(TopLevelName name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return base.InnerList.Add(name);
		}
	}
}
