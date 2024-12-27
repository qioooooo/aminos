using System;
using System.Collections;
using System.Globalization;

namespace System
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	internal class InvariantComparer : IComparer
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00002E02 File Offset: 0x00001E02
		internal InvariantComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002E1C File Offset: 0x00001E1C
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

		// Token: 0x040000CF RID: 207
		private CompareInfo m_compareInfo;

		// Token: 0x040000D0 RID: 208
		internal static readonly InvariantComparer Default = new InvariantComparer();
	}
}
