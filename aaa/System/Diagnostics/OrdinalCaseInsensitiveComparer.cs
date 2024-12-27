using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x0200077B RID: 1915
	internal class OrdinalCaseInsensitiveComparer : IComparer
	{
		// Token: 0x06003B27 RID: 15143 RVA: 0x000FBD30 File Offset: 0x000FAD30
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return string.CompareOrdinal(text.ToUpperInvariant(), text2.ToUpperInvariant());
			}
			return Comparer.Default.Compare(a, b);
		}

		// Token: 0x040033CC RID: 13260
		internal static readonly OrdinalCaseInsensitiveComparer Default = new OrdinalCaseInsensitiveComparer();
	}
}
