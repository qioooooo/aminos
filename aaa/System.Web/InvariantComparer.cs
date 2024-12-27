using System;
using System.Collections;
using System.Globalization;

namespace System
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	internal class InvariantComparer : IComparer
	{
		// Token: 0x0600000D RID: 13 RVA: 0x0000229D File Offset: 0x0000129D
		internal InvariantComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022B8 File Offset: 0x000012B8
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

		// Token: 0x04000C79 RID: 3193
		private CompareInfo m_compareInfo;

		// Token: 0x04000C7A RID: 3194
		internal static readonly InvariantComparer Default = new InvariantComparer();
	}
}
