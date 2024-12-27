using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200007C RID: 124
	[ComVisible(true)]
	[Serializable]
	public struct Char : IComparable, IConvertible, IComparable<char>, IEquatable<char>
	{
		// Token: 0x060006F2 RID: 1778 RVA: 0x00016CD2 File Offset: 0x00015CD2
		private static bool IsLatin1(char ch)
		{
			return ch <= 'ÿ';
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00016CDF File Offset: 0x00015CDF
		private static bool IsAscii(char ch)
		{
			return ch <= '\u007f';
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00016CE9 File Offset: 0x00015CE9
		private static UnicodeCategory GetLatin1UnicodeCategory(char ch)
		{
			return (UnicodeCategory)char.categoryForLatin1[(int)ch];
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00016CF2 File Offset: 0x00015CF2
		public override int GetHashCode()
		{
			return (int)(this | ((int)this << 16));
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00016CFC File Offset: 0x00015CFC
		public override bool Equals(object obj)
		{
			return obj is char && this == (char)obj;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00016D12 File Offset: 0x00015D12
		public bool Equals(char obj)
		{
			return this == obj;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00016D19 File Offset: 0x00015D19
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is char))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeChar"));
			}
			return (int)(this - (char)value);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00016D41 File Offset: 0x00015D41
		public int CompareTo(char value)
		{
			return (int)(this - value);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00016D47 File Offset: 0x00015D47
		public override string ToString()
		{
			return char.ToString(this);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00016D50 File Offset: 0x00015D50
		public string ToString(IFormatProvider provider)
		{
			return char.ToString(this);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00016D59 File Offset: 0x00015D59
		public static string ToString(char c)
		{
			return new string(c, 1);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00016D62 File Offset: 0x00015D62
		public static char Parse(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length != 1)
			{
				throw new FormatException(Environment.GetResourceString("Format_NeedSingleChar"));
			}
			return s[0];
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00016D92 File Offset: 0x00015D92
		public static bool TryParse(string s, out char result)
		{
			result = '\0';
			if (s == null)
			{
				return false;
			}
			if (s.Length != 1)
			{
				return false;
			}
			result = s[0];
			return true;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00016DB1 File Offset: 0x00015DB1
		public static bool IsDigit(char c)
		{
			if (char.IsLatin1(c))
			{
				return c >= '0' && c <= '9';
			}
			return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.DecimalDigitNumber;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00016DD4 File Offset: 0x00015DD4
		internal static bool CheckLetter(UnicodeCategory uc)
		{
			switch (uc)
			{
			case UnicodeCategory.UppercaseLetter:
			case UnicodeCategory.LowercaseLetter:
			case UnicodeCategory.TitlecaseLetter:
			case UnicodeCategory.ModifierLetter:
			case UnicodeCategory.OtherLetter:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00016E02 File Offset: 0x00015E02
		public static bool IsLetter(char c)
		{
			if (!char.IsLatin1(c))
			{
				return char.CheckLetter(CharUnicodeInfo.GetUnicodeCategory(c));
			}
			if (char.IsAscii(c))
			{
				c |= ' ';
				return c >= 'a' && c <= 'z';
			}
			return char.CheckLetter(char.GetLatin1UnicodeCategory(c));
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00016E42 File Offset: 0x00015E42
		private static bool IsWhiteSpaceLatin1(char c)
		{
			return c == ' ' || (c >= '\t' && c <= '\r') || c == '\u00a0' || c == '\u0085';
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00016E66 File Offset: 0x00015E66
		public static bool IsWhiteSpace(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.IsWhiteSpaceLatin1(c);
			}
			return CharUnicodeInfo.IsWhiteSpace(c);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00016E7D File Offset: 0x00015E7D
		public static bool IsUpper(char c)
		{
			if (!char.IsLatin1(c))
			{
				return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.UppercaseLetter;
			}
			if (char.IsAscii(c))
			{
				return c >= 'A' && c <= 'Z';
			}
			return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.UppercaseLetter;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00016EB2 File Offset: 0x00015EB2
		public static bool IsLower(char c)
		{
			if (!char.IsLatin1(c))
			{
				return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter;
			}
			if (char.IsAscii(c))
			{
				return c >= 'a' && c <= 'z';
			}
			return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.LowercaseLetter;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00016EE8 File Offset: 0x00015EE8
		internal static bool CheckPunctuation(UnicodeCategory uc)
		{
			switch (uc)
			{
			case UnicodeCategory.ConnectorPunctuation:
			case UnicodeCategory.DashPunctuation:
			case UnicodeCategory.OpenPunctuation:
			case UnicodeCategory.ClosePunctuation:
			case UnicodeCategory.InitialQuotePunctuation:
			case UnicodeCategory.FinalQuotePunctuation:
			case UnicodeCategory.OtherPunctuation:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00016F21 File Offset: 0x00015F21
		public static bool IsPunctuation(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.CheckPunctuation(char.GetLatin1UnicodeCategory(c));
			}
			return char.CheckPunctuation(CharUnicodeInfo.GetUnicodeCategory(c));
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00016F44 File Offset: 0x00015F44
		internal static bool CheckLetterOrDigit(UnicodeCategory uc)
		{
			switch (uc)
			{
			case UnicodeCategory.UppercaseLetter:
			case UnicodeCategory.LowercaseLetter:
			case UnicodeCategory.TitlecaseLetter:
			case UnicodeCategory.ModifierLetter:
			case UnicodeCategory.OtherLetter:
			case UnicodeCategory.DecimalDigitNumber:
				return true;
			}
			return false;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00016F82 File Offset: 0x00015F82
		public static bool IsLetterOrDigit(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.CheckLetterOrDigit(char.GetLatin1UnicodeCategory(c));
			}
			return char.CheckLetterOrDigit(CharUnicodeInfo.GetUnicodeCategory(c));
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00016FA3 File Offset: 0x00015FA3
		public static char ToUpper(char c, CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.TextInfo.ToUpper(c);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00016FBF File Offset: 0x00015FBF
		public static char ToUpper(char c)
		{
			return char.ToUpper(c, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00016FCC File Offset: 0x00015FCC
		public static char ToUpperInvariant(char c)
		{
			return char.ToUpper(c, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00016FD9 File Offset: 0x00015FD9
		public static char ToLower(char c, CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.TextInfo.ToLower(c);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00016FF5 File Offset: 0x00015FF5
		public static char ToLower(char c)
		{
			return char.ToLower(c, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00017002 File Offset: 0x00016002
		public static char ToLowerInvariant(char c)
		{
			return char.ToLower(c, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001700F File Offset: 0x0001600F
		public TypeCode GetTypeCode()
		{
			return TypeCode.Char;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00017014 File Offset: 0x00016014
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Char", "Boolean" }));
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00017052 File Offset: 0x00016052
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00017056 File Offset: 0x00016056
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001705F File Offset: 0x0001605F
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00017068 File Offset: 0x00016068
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00017071 File Offset: 0x00016071
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001707A File Offset: 0x0001607A
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00017083 File Offset: 0x00016083
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001708C File Offset: 0x0001608C
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00017095 File Offset: 0x00016095
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000170A0 File Offset: 0x000160A0
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Char", "Single" }));
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x000170E0 File Offset: 0x000160E0
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Char", "Double" }));
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00017120 File Offset: 0x00016120
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Char", "Decimal" }));
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00017160 File Offset: 0x00016160
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Char", "DateTime" }));
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001719E File Offset: 0x0001619E
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x000171AE File Offset: 0x000161AE
		public static bool IsControl(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.Control;
			}
			return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.Control;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000171D0 File Offset: 0x000161D0
		public static bool IsControl(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (char.IsLatin1(c))
			{
				return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.Control;
			}
			return CharUnicodeInfo.GetUnicodeCategory(s, index) == UnicodeCategory.Control;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00017228 File Offset: 0x00016228
		public static bool IsDigit(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (char.IsLatin1(c))
			{
				return c >= '0' && c <= '9';
			}
			return CharUnicodeInfo.GetUnicodeCategory(s, index) == UnicodeCategory.DecimalDigitNumber;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00017284 File Offset: 0x00016284
		public static bool IsLetter(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (!char.IsLatin1(c))
			{
				return char.CheckLetter(CharUnicodeInfo.GetUnicodeCategory(s, index));
			}
			if (char.IsAscii(c))
			{
				c |= ' ';
				return c >= 'a' && c <= 'z';
			}
			return char.CheckLetter(char.GetLatin1UnicodeCategory(c));
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x000172FC File Offset: 0x000162FC
		public static bool IsLetterOrDigit(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (char.IsLatin1(c))
			{
				return char.CheckLetterOrDigit(char.GetLatin1UnicodeCategory(c));
			}
			return char.CheckLetterOrDigit(CharUnicodeInfo.GetUnicodeCategory(s, index));
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00017354 File Offset: 0x00016354
		public static bool IsLower(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (!char.IsLatin1(c))
			{
				return CharUnicodeInfo.GetUnicodeCategory(s, index) == UnicodeCategory.LowercaseLetter;
			}
			if (char.IsAscii(c))
			{
				return c >= 'a' && c <= 'z';
			}
			return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.LowercaseLetter;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000173C0 File Offset: 0x000163C0
		internal static bool CheckNumber(UnicodeCategory uc)
		{
			switch (uc)
			{
			case UnicodeCategory.DecimalDigitNumber:
			case UnicodeCategory.LetterNumber:
			case UnicodeCategory.OtherNumber:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x000173E8 File Offset: 0x000163E8
		public static bool IsNumber(char c)
		{
			if (!char.IsLatin1(c))
			{
				return char.CheckNumber(CharUnicodeInfo.GetUnicodeCategory(c));
			}
			if (char.IsAscii(c))
			{
				return c >= '0' && c <= '9';
			}
			return char.CheckNumber(char.GetLatin1UnicodeCategory(c));
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00017424 File Offset: 0x00016424
		public static bool IsNumber(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (!char.IsLatin1(c))
			{
				return char.CheckNumber(CharUnicodeInfo.GetUnicodeCategory(s, index));
			}
			if (char.IsAscii(c))
			{
				return c >= '0' && c <= '9';
			}
			return char.CheckNumber(char.GetLatin1UnicodeCategory(c));
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00017494 File Offset: 0x00016494
		public static bool IsPunctuation(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (char.IsLatin1(c))
			{
				return char.CheckPunctuation(char.GetLatin1UnicodeCategory(c));
			}
			return char.CheckPunctuation(CharUnicodeInfo.GetUnicodeCategory(s, index));
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x000174EC File Offset: 0x000164EC
		internal static bool CheckSeparator(UnicodeCategory uc)
		{
			switch (uc)
			{
			case UnicodeCategory.SpaceSeparator:
			case UnicodeCategory.LineSeparator:
			case UnicodeCategory.ParagraphSeparator:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00017515 File Offset: 0x00016515
		private static bool IsSeparatorLatin1(char c)
		{
			return c == ' ' || c == '\u00a0';
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00017526 File Offset: 0x00016526
		public static bool IsSeparator(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.IsSeparatorLatin1(c);
			}
			return char.CheckSeparator(CharUnicodeInfo.GetUnicodeCategory(c));
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00017544 File Offset: 0x00016544
		public static bool IsSeparator(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (char.IsLatin1(c))
			{
				return char.IsSeparatorLatin1(c);
			}
			return char.CheckSeparator(CharUnicodeInfo.GetUnicodeCategory(s, index));
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00017596 File Offset: 0x00016596
		public static bool IsSurrogate(char c)
		{
			return c >= '\ud800' && c <= '\udfff';
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x000175AD File Offset: 0x000165AD
		public static bool IsSurrogate(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return char.IsSurrogate(s[index]);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x000175E0 File Offset: 0x000165E0
		internal static bool CheckSymbol(UnicodeCategory uc)
		{
			switch (uc)
			{
			case UnicodeCategory.MathSymbol:
			case UnicodeCategory.CurrencySymbol:
			case UnicodeCategory.ModifierSymbol:
			case UnicodeCategory.OtherSymbol:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001760D File Offset: 0x0001660D
		public static bool IsSymbol(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.CheckSymbol(char.GetLatin1UnicodeCategory(c));
			}
			return char.CheckSymbol(CharUnicodeInfo.GetUnicodeCategory(c));
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00017630 File Offset: 0x00016630
		public static bool IsSymbol(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (char.IsLatin1(s[index]))
			{
				return char.CheckSymbol(char.GetLatin1UnicodeCategory(s[index]));
			}
			return char.CheckSymbol(CharUnicodeInfo.GetUnicodeCategory(s, index));
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001768C File Offset: 0x0001668C
		public static bool IsUpper(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			char c = s[index];
			if (!char.IsLatin1(c))
			{
				return CharUnicodeInfo.GetUnicodeCategory(s, index) == UnicodeCategory.UppercaseLetter;
			}
			if (char.IsAscii(c))
			{
				return c >= 'A' && c <= 'Z';
			}
			return char.GetLatin1UnicodeCategory(c) == UnicodeCategory.UppercaseLetter;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x000176F8 File Offset: 0x000166F8
		public static bool IsWhiteSpace(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (char.IsLatin1(s[index]))
			{
				return char.IsWhiteSpaceLatin1(s[index]);
			}
			return CharUnicodeInfo.IsWhiteSpace(s, index);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00017749 File Offset: 0x00016749
		public static UnicodeCategory GetUnicodeCategory(char c)
		{
			if (char.IsLatin1(c))
			{
				return char.GetLatin1UnicodeCategory(c);
			}
			return CharUnicodeInfo.InternalGetUnicodeCategory((int)c);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00017760 File Offset: 0x00016760
		public static UnicodeCategory GetUnicodeCategory(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (char.IsLatin1(s[index]))
			{
				return char.GetLatin1UnicodeCategory(s[index]);
			}
			return CharUnicodeInfo.InternalGetUnicodeCategory(s, index);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000177B1 File Offset: 0x000167B1
		public static double GetNumericValue(char c)
		{
			return CharUnicodeInfo.GetNumericValue(c);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x000177B9 File Offset: 0x000167B9
		public static double GetNumericValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return CharUnicodeInfo.GetNumericValue(s, index);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x000177E4 File Offset: 0x000167E4
		public static bool IsHighSurrogate(char c)
		{
			return c >= '\ud800' && c <= '\udbff';
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x000177FB File Offset: 0x000167FB
		public static bool IsHighSurrogate(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return char.IsHighSurrogate(s[index]);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001782F File Offset: 0x0001682F
		public static bool IsLowSurrogate(char c)
		{
			return c >= '\udc00' && c <= '\udfff';
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00017846 File Offset: 0x00016846
		public static bool IsLowSurrogate(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return char.IsLowSurrogate(s[index]);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001787C File Offset: 0x0001687C
		public static bool IsSurrogatePair(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return index + 1 < s.Length && char.IsSurrogatePair(s[index], s[index + 1]);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000178D1 File Offset: 0x000168D1
		public static bool IsSurrogatePair(char highSurrogate, char lowSurrogate)
		{
			return highSurrogate >= '\ud800' && highSurrogate <= '\udbff' && lowSurrogate >= '\udc00' && lowSurrogate <= '\udfff';
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000178FC File Offset: 0x000168FC
		public static string ConvertFromUtf32(int utf32)
		{
			if (utf32 < 0 || utf32 > 1114111 || (utf32 >= 55296 && utf32 <= 57343))
			{
				throw new ArgumentOutOfRangeException("utf32", Environment.GetResourceString("ArgumentOutOfRange_InvalidUTF32"));
			}
			if (utf32 < 65536)
			{
				return char.ToString((char)utf32);
			}
			utf32 -= 65536;
			return new string(new char[]
			{
				(char)(utf32 / 1024 + 55296),
				(char)(utf32 % 1024 + 56320)
			});
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00017984 File Offset: 0x00016984
		public static int ConvertToUtf32(char highSurrogate, char lowSurrogate)
		{
			if (!char.IsHighSurrogate(highSurrogate))
			{
				throw new ArgumentOutOfRangeException("highSurrogate", Environment.GetResourceString("ArgumentOutOfRange_InvalidHighSurrogate"));
			}
			if (!char.IsLowSurrogate(lowSurrogate))
			{
				throw new ArgumentOutOfRangeException("lowSurrogate", Environment.GetResourceString("ArgumentOutOfRange_InvalidLowSurrogate"));
			}
			return (int)((highSurrogate - '\ud800') * 'Ѐ' + (lowSurrogate - '\udc00')) + 65536;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000179E8 File Offset: 0x000169E8
		public static int ConvertToUtf32(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			int num = (int)(s[index] - '\ud800');
			if (num < 0 || num > 2047)
			{
				return (int)s[index];
			}
			if (num > 1023)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidLowSurrogate", new object[] { index }), "s");
			}
			if (index >= s.Length - 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidHighSurrogate", new object[] { index }), "s");
			}
			int num2 = (int)(s[index + 1] - '\udc00');
			if (num2 >= 0 && num2 <= 1023)
			{
				return num * 1024 + num2 + 65536;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidHighSurrogate", new object[] { index }), "s");
		}

		// Token: 0x04000221 RID: 545
		public const char MaxValue = '\uffff';

		// Token: 0x04000222 RID: 546
		public const char MinValue = '\0';

		// Token: 0x04000223 RID: 547
		internal const int UNICODE_PLANE00_END = 65535;

		// Token: 0x04000224 RID: 548
		internal const int UNICODE_PLANE01_START = 65536;

		// Token: 0x04000225 RID: 549
		internal const int UNICODE_PLANE16_END = 1114111;

		// Token: 0x04000226 RID: 550
		internal const int HIGH_SURROGATE_START = 55296;

		// Token: 0x04000227 RID: 551
		internal const int LOW_SURROGATE_END = 57343;

		// Token: 0x04000228 RID: 552
		internal char m_value;

		// Token: 0x04000229 RID: 553
		private static readonly byte[] categoryForLatin1 = new byte[]
		{
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 11, 24, 24, 24, 26, 24, 24, 24,
			20, 21, 24, 25, 24, 19, 24, 24, 8, 8,
			8, 8, 8, 8, 8, 8, 8, 8, 24, 24,
			25, 25, 25, 24, 24, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 20, 24, 21, 27, 18, 27, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 20, 25, 21, 25, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
			11, 24, 26, 26, 26, 26, 28, 28, 27, 28,
			1, 22, 25, 19, 28, 27, 28, 25, 10, 10,
			27, 1, 28, 24, 27, 10, 1, 23, 10, 10,
			10, 24, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 25, 0, 0, 0, 0,
			0, 0, 0, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 25, 1, 1,
			1, 1, 1, 1, 1, 1
		};
	}
}
