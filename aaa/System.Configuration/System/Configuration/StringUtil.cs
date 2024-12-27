using System;

namespace System.Configuration
{
	// Token: 0x0200009E RID: 158
	internal static class StringUtil
	{
		// Token: 0x06000636 RID: 1590 RVA: 0x0001CCD8 File Offset: 0x0001BCD8
		internal static bool EqualsNE(string s1, string s2)
		{
			if (s1 == null)
			{
				s1 = string.Empty;
			}
			if (s2 == null)
			{
				s2 = string.Empty;
			}
			return string.Equals(s1, s2, StringComparison.Ordinal);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001CCF6 File Offset: 0x0001BCF6
		internal static bool EqualsIgnoreCase(string s1, string s2)
		{
			return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001CD00 File Offset: 0x0001BD00
		internal static bool StartsWith(string s1, string s2)
		{
			return s2 != null && 0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.Ordinal);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001CD1A File Offset: 0x0001BD1A
		internal static bool StartsWithIgnoreCase(string s1, string s2)
		{
			return s2 != null && 0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001CD34 File Offset: 0x0001BD34
		internal static string[] ObjectArrayToStringArray(object[] objectArray)
		{
			string[] array = new string[objectArray.Length];
			objectArray.CopyTo(array, 0);
			return array;
		}
	}
}
