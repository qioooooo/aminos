using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000078 RID: 120
	internal static class CharUtil
	{
		// Token: 0x060006F7 RID: 1783 RVA: 0x000254EC File Offset: 0x000244EC
		public static bool IsAlphaNumeric(char ch)
		{
			int unicodeCategory = (int)char.GetUnicodeCategory(ch);
			return unicodeCategory <= 4 || (unicodeCategory <= 10 && unicodeCategory >= 8);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00025514 File Offset: 0x00024514
		public static bool IsDecimalDigitOne(char ch)
		{
			int unicodeCategory = (int)char.GetUnicodeCategory(ch -= '\u0001');
			return unicodeCategory == 8 && char.GetNumericValue(ch) == 0.0;
		}
	}
}
