using System;
using System.Collections;
using System.Globalization;

namespace System
{
	// Token: 0x02000147 RID: 327
	[Serializable]
	internal class InvariantComparer : IComparer
	{
		// Token: 0x06000A3F RID: 2623 RVA: 0x00047D85 File Offset: 0x00046D85
		internal InvariantComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00047DA0 File Offset: 0x00046DA0
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return this.m_compareInfo.Compare(text, text2);
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x0400064B RID: 1611
		private CompareInfo m_compareInfo;

		// Token: 0x0400064C RID: 1612
		internal static readonly InvariantComparer Default = new InvariantComparer();
	}
}
