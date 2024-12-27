using System;
using System.Collections;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020003D9 RID: 985
	internal class EnumValAlphaComparer : IComparer
	{
		// Token: 0x06003B0D RID: 15117 RVA: 0x000D5EAC File Offset: 0x000D4EAC
		internal EnumValAlphaComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x000D5EC4 File Offset: 0x000D4EC4
		public int Compare(object a, object b)
		{
			return this.m_compareInfo.Compare(a.ToString(), b.ToString());
		}

		// Token: 0x04001D7F RID: 7551
		private CompareInfo m_compareInfo;

		// Token: 0x04001D80 RID: 7552
		internal static readonly EnumValAlphaComparer Default = new EnumValAlphaComparer();
	}
}
