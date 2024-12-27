using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x02000396 RID: 918
	[Serializable]
	internal class PathList
	{
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x0006D741 File Offset: 0x0006C741
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x0006D750 File Offset: 0x0006C750
		public int GetCookiesCount()
		{
			int num = 0;
			foreach (object obj in this.m_list.Values)
			{
				CookieCollection cookieCollection = (CookieCollection)obj;
				num += cookieCollection.Count;
			}
			return num;
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x0006D7B4 File Offset: 0x0006C7B4
		public ICollection Values
		{
			get
			{
				return this.m_list.Values;
			}
		}

		// Token: 0x17000599 RID: 1433
		public object this[string s]
		{
			get
			{
				return this.m_list[s];
			}
			set
			{
				this.m_list[s] = value;
			}
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x0006D7DE File Offset: 0x0006C7DE
		public IEnumerator GetEnumerator()
		{
			return this.m_list.GetEnumerator();
		}

		// Token: 0x04001D36 RID: 7478
		private SortedList m_list = SortedList.Synchronized(new SortedList(PathList.PathListComparer.StaticInstance));

		// Token: 0x02000397 RID: 919
		[Serializable]
		private class PathListComparer : IComparer
		{
			// Token: 0x06001CB7 RID: 7351 RVA: 0x0006D7EC File Offset: 0x0006C7EC
			int IComparer.Compare(object ol, object or)
			{
				string text = CookieParser.CheckQuoted((string)ol);
				string text2 = CookieParser.CheckQuoted((string)or);
				int length = text.Length;
				int length2 = text2.Length;
				int num = Math.Min(length, length2);
				for (int i = 0; i < num; i++)
				{
					if (text[i] != text2[i])
					{
						return (int)(text[i] - text2[i]);
					}
				}
				return length2 - length;
			}

			// Token: 0x04001D37 RID: 7479
			internal static readonly PathList.PathListComparer StaticInstance = new PathList.PathListComparer();
		}
	}
}
