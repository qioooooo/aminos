using System;
using System.Collections;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200004D RID: 77
	public class SearchResultReferenceCollection : ReadOnlyCollectionBase
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00007690 File Offset: 0x00006690
		internal SearchResultReferenceCollection()
		{
		}

		// Token: 0x1700006B RID: 107
		public SearchResultReference this[int index]
		{
			get
			{
				return (SearchResultReference)base.InnerList[index];
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000076AB File Offset: 0x000066AB
		internal int Add(SearchResultReference reference)
		{
			return base.InnerList.Add(reference);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000076B9 File Offset: 0x000066B9
		public bool Contains(SearchResultReference value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000076C7 File Offset: 0x000066C7
		public int IndexOf(SearchResultReference value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000076D5 File Offset: 0x000066D5
		public void CopyTo(SearchResultReference[] values, int index)
		{
			base.InnerList.CopyTo(values, index);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000076E4 File Offset: 0x000066E4
		internal void Clear()
		{
			base.InnerList.Clear();
		}
	}
}
