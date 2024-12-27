using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000C0 RID: 192
	internal abstract class XmlSortKey : IComparable
	{
		// Token: 0x17000164 RID: 356
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x0002C25C File Offset: 0x0002B25C
		public int Priority
		{
			set
			{
				for (XmlSortKey xmlSortKey = this; xmlSortKey != null; xmlSortKey = xmlSortKey.nextKey)
				{
					xmlSortKey.priority = value;
				}
			}
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0002C27E File Offset: 0x0002B27E
		public XmlSortKey AddSortKey(XmlSortKey sortKey)
		{
			if (this.nextKey != null)
			{
				this.nextKey.AddSortKey(sortKey);
			}
			else
			{
				this.nextKey = sortKey;
			}
			return this;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002C29F File Offset: 0x0002B29F
		protected int BreakSortingTie(XmlSortKey that)
		{
			if (this.nextKey != null)
			{
				return this.nextKey.CompareTo(that.nextKey);
			}
			if (this.priority >= that.priority)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002C2CC File Offset: 0x0002B2CC
		protected int CompareToEmpty(object obj)
		{
			XmlEmptySortKey xmlEmptySortKey = obj as XmlEmptySortKey;
			if (!xmlEmptySortKey.IsEmptyGreatest)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x0600095B RID: 2395
		public abstract int CompareTo(object that);

		// Token: 0x040005BF RID: 1471
		private int priority;

		// Token: 0x040005C0 RID: 1472
		private XmlSortKey nextKey;
	}
}
