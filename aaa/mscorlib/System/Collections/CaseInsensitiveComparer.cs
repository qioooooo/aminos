using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000245 RID: 581
	[ComVisible(true)]
	[Serializable]
	public class CaseInsensitiveComparer : IComparer
	{
		// Token: 0x06001741 RID: 5953 RVA: 0x0003C5D0 File Offset: 0x0003B5D0
		public CaseInsensitiveComparer()
		{
			this.m_compareInfo = CultureInfo.CurrentCulture.CompareInfo;
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x0003C5E8 File Offset: 0x0003B5E8
		public CaseInsensitiveComparer(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this.m_compareInfo = culture.CompareInfo;
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x0003C60A File Offset: 0x0003B60A
		public static CaseInsensitiveComparer Default
		{
			get
			{
				return new CaseInsensitiveComparer(CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x0003C616 File Offset: 0x0003B616
		public static CaseInsensitiveComparer DefaultInvariant
		{
			get
			{
				if (CaseInsensitiveComparer.m_InvariantCaseInsensitiveComparer == null)
				{
					CaseInsensitiveComparer.m_InvariantCaseInsensitiveComparer = new CaseInsensitiveComparer(CultureInfo.InvariantCulture);
				}
				return CaseInsensitiveComparer.m_InvariantCaseInsensitiveComparer;
			}
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x0003C634 File Offset: 0x0003B634
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return this.m_compareInfo.Compare(text, text2, CompareOptions.IgnoreCase);
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x04000934 RID: 2356
		private CompareInfo m_compareInfo;

		// Token: 0x04000935 RID: 2357
		private static CaseInsensitiveComparer m_InvariantCaseInsensitiveComparer;
	}
}
