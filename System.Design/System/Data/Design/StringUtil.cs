using System;

namespace System.Data.Design
{
	internal sealed class StringUtil
	{
		private StringUtil()
		{
		}

		internal static bool Empty(string str)
		{
			return str == null || 0 >= str.Length;
		}

		internal static bool EmptyOrSpace(string str)
		{
			return str == null || 0 >= str.Trim().Length;
		}

		internal static bool EqualValue(string str1, string str2)
		{
			return StringUtil.EqualValue(str1, str2, false);
		}

		internal static bool EqualValue(string str1, string str2, bool caseInsensitive)
		{
			if (str1 != null && str2 != null)
			{
				StringComparison stringComparison = (caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
				return string.Equals(str1, str2, stringComparison);
			}
			return str1 == str2;
		}

		internal static bool NotEmpty(string str)
		{
			return !StringUtil.Empty(str);
		}

		public static bool NotEmptyAfterTrim(string str)
		{
			return !StringUtil.EmptyOrSpace(str);
		}
	}
}
