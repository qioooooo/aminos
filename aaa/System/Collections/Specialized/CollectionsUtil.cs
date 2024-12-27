using System;

namespace System.Collections.Specialized
{
	// Token: 0x0200024D RID: 589
	public class CollectionsUtil
	{
		// Token: 0x06001438 RID: 5176 RVA: 0x00043399 File Offset: 0x00042399
		public static Hashtable CreateCaseInsensitiveHashtable()
		{
			return new Hashtable(StringComparer.CurrentCultureIgnoreCase);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x000433A5 File Offset: 0x000423A5
		public static Hashtable CreateCaseInsensitiveHashtable(int capacity)
		{
			return new Hashtable(capacity, StringComparer.CurrentCultureIgnoreCase);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x000433B2 File Offset: 0x000423B2
		public static Hashtable CreateCaseInsensitiveHashtable(IDictionary d)
		{
			return new Hashtable(d, StringComparer.CurrentCultureIgnoreCase);
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x000433BF File Offset: 0x000423BF
		public static SortedList CreateCaseInsensitiveSortedList()
		{
			return new SortedList(CaseInsensitiveComparer.Default);
		}
	}
}
