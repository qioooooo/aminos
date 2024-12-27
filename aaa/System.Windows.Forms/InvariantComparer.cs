using System;
using System.Collections;
using System.Globalization;

namespace System
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	internal class InvariantComparer : IComparer
	{
		// Token: 0x060001CA RID: 458 RVA: 0x00006CA5 File Offset: 0x00005CA5
		internal InvariantComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00006CC0 File Offset: 0x00005CC0
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

		// Token: 0x04000B64 RID: 2916
		private CompareInfo m_compareInfo;

		// Token: 0x04000B65 RID: 2917
		internal static readonly InvariantComparer Default = new InvariantComparer();
	}
}
