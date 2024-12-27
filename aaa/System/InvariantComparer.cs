using System;
using System.Collections;
using System.Globalization;

namespace System
{
	// Token: 0x020007A2 RID: 1954
	[Serializable]
	internal class InvariantComparer : IComparer
	{
		// Token: 0x06003C19 RID: 15385 RVA: 0x00100FC5 File Offset: 0x000FFFC5
		internal InvariantComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x00100FE0 File Offset: 0x000FFFE0
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

		// Token: 0x04003507 RID: 13575
		private CompareInfo m_compareInfo;

		// Token: 0x04003508 RID: 13576
		internal static readonly InvariantComparer Default = new InvariantComparer();
	}
}
