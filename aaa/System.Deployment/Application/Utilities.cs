using System;

namespace System.Deployment.Application
{
	// Token: 0x020000E8 RID: 232
	internal static class Utilities
	{
		// Token: 0x060005E7 RID: 1511 RVA: 0x0001E8D0 File Offset: 0x0001D8D0
		public static int CompareWithNullEqEmpty(string s1, string s2, StringComparison comparisonType)
		{
			if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
			{
				return 0;
			}
			return string.Compare(s1, s2, comparisonType);
		}
	}
}
