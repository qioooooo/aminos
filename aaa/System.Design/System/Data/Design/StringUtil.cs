using System;

namespace System.Data.Design
{
	// Token: 0x020000BC RID: 188
	internal sealed class StringUtil
	{
		// Token: 0x06000857 RID: 2135 RVA: 0x00015578 File Offset: 0x00014578
		private StringUtil()
		{
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00015580 File Offset: 0x00014580
		internal static bool Empty(string str)
		{
			return str == null || 0 >= str.Length;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00015593 File Offset: 0x00014593
		internal static bool EmptyOrSpace(string str)
		{
			return str == null || 0 >= str.Trim().Length;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000155AB File Offset: 0x000145AB
		internal static bool EqualValue(string str1, string str2)
		{
			return StringUtil.EqualValue(str1, str2, false);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x000155B8 File Offset: 0x000145B8
		internal static bool EqualValue(string str1, string str2, bool caseInsensitive)
		{
			if (str1 != null && str2 != null)
			{
				StringComparison stringComparison = (caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
				return string.Equals(str1, str2, stringComparison);
			}
			return str1 == str2;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x000155E3 File Offset: 0x000145E3
		internal static bool NotEmpty(string str)
		{
			return !StringUtil.Empty(str);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x000155EE File Offset: 0x000145EE
		public static bool NotEmptyAfterTrim(string str)
		{
			return !StringUtil.EmptyOrSpace(str);
		}
	}
}
