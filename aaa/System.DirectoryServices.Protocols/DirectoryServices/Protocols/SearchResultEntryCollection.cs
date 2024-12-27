using System;
using System.Collections;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200004F RID: 79
	public class SearchResultEntryCollection : ReadOnlyCollectionBase
	{
		// Token: 0x0600019A RID: 410 RVA: 0x000079E0 File Offset: 0x000069E0
		internal SearchResultEntryCollection()
		{
		}

		// Token: 0x1700006F RID: 111
		public SearchResultEntry this[int index]
		{
			get
			{
				return (SearchResultEntry)base.InnerList[index];
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000079FB File Offset: 0x000069FB
		internal int Add(SearchResultEntry entry)
		{
			return base.InnerList.Add(entry);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007A09 File Offset: 0x00006A09
		public bool Contains(SearchResultEntry value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007A17 File Offset: 0x00006A17
		public int IndexOf(SearchResultEntry value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007A25 File Offset: 0x00006A25
		public void CopyTo(SearchResultEntry[] values, int index)
		{
			base.InnerList.CopyTo(values, index);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007A34 File Offset: 0x00006A34
		internal void Clear()
		{
			base.InnerList.Clear();
		}
	}
}
