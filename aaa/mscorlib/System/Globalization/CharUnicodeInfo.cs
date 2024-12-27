using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x02000377 RID: 887
	public sealed class CharUnicodeInfo
	{
		// Token: 0x0600233F RID: 9023 RVA: 0x00059ACC File Offset: 0x00058ACC
		unsafe static CharUnicodeInfo()
		{
			CharUnicodeInfo.UnicodeDataHeader* pDataTable = (CharUnicodeInfo.UnicodeDataHeader*)CharUnicodeInfo.m_pDataTable;
			CharUnicodeInfo.m_pCategoryLevel1Index = (ushort*)(CharUnicodeInfo.m_pDataTable + pDataTable->OffsetToCategoriesIndex);
			CharUnicodeInfo.m_pCategoriesValue = CharUnicodeInfo.m_pDataTable + pDataTable->OffsetToCategoriesValue;
			CharUnicodeInfo.m_pNumericLevel1Index = (ushort*)(CharUnicodeInfo.m_pDataTable + pDataTable->OffsetToNumbericIndex);
			CharUnicodeInfo.m_pNumericValues = CharUnicodeInfo.m_pDataTable + pDataTable->OffsetToNumbericValue;
			CharUnicodeInfo.m_pDigitValues = (CharUnicodeInfo.DigitValues*)(CharUnicodeInfo.m_pDataTable + pDataTable->OffsetToDigitValue);
			CharUnicodeInfo.nativeInitTable(CharUnicodeInfo.m_pDataTable);
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x00059B5C File Offset: 0x00058B5C
		private CharUnicodeInfo()
		{
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x00059B64 File Offset: 0x00058B64
		internal static int InternalConvertToUtf32(string s, int index)
		{
			if (index < s.Length - 1)
			{
				int num = (int)(s[index] - '\ud800');
				if (num >= 0 && num <= 1023)
				{
					int num2 = (int)(s[index + 1] - '\udc00');
					if (num2 >= 0 && num2 <= 1023)
					{
						return num * 1024 + num2 + 65536;
					}
				}
			}
			return (int)s[index];
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x00059BCC File Offset: 0x00058BCC
		internal static int InternalConvertToUtf32(string s, int index, out int charLength)
		{
			charLength = 1;
			if (index < s.Length - 1)
			{
				int num = (int)(s[index] - '\ud800');
				if (num >= 0 && num <= 1023)
				{
					int num2 = (int)(s[index + 1] - '\udc00');
					if (num2 >= 0 && num2 <= 1023)
					{
						charLength++;
						return num * 1024 + num2 + 65536;
					}
				}
			}
			return (int)s[index];
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x00059C3C File Offset: 0x00058C3C
		internal static bool IsWhiteSpace(string s, int index)
		{
			switch (CharUnicodeInfo.GetUnicodeCategory(s, index))
			{
			case UnicodeCategory.SpaceSeparator:
			case UnicodeCategory.LineSeparator:
			case UnicodeCategory.ParagraphSeparator:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x00059C70 File Offset: 0x00058C70
		internal static bool IsWhiteSpace(char c)
		{
			switch (CharUnicodeInfo.GetUnicodeCategory(c))
			{
			case UnicodeCategory.SpaceSeparator:
			case UnicodeCategory.LineSeparator:
			case UnicodeCategory.ParagraphSeparator:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x00059CA0 File Offset: 0x00058CA0
		internal unsafe static double InternalGetNumericValue(int ch)
		{
			ushort num = CharUnicodeInfo.m_pNumericLevel1Index[ch >> 8];
			num = CharUnicodeInfo.m_pNumericLevel1Index[(int)num + ((ch >> 4) & 15)];
			byte* ptr = (byte*)(CharUnicodeInfo.m_pNumericLevel1Index + num);
			return *(double*)(CharUnicodeInfo.m_pNumericValues + (IntPtr)ptr[ch & 15] * 8);
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x00059CEC File Offset: 0x00058CEC
		internal unsafe static CharUnicodeInfo.DigitValues* InternalGetDigitValues(int ch)
		{
			ushort num = CharUnicodeInfo.m_pNumericLevel1Index[ch >> 8];
			num = CharUnicodeInfo.m_pNumericLevel1Index[(int)num + ((ch >> 4) & 15)];
			byte* ptr = (byte*)(CharUnicodeInfo.m_pNumericLevel1Index + num);
			return CharUnicodeInfo.m_pDigitValues + ptr[ch & 15];
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x00059D3C File Offset: 0x00058D3C
		internal unsafe static sbyte InternalGetDecimalDigitValue(int ch)
		{
			return CharUnicodeInfo.InternalGetDigitValues(ch)->decimalDigit;
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x00059D49 File Offset: 0x00058D49
		internal unsafe static sbyte InternalGetDigitValue(int ch)
		{
			return CharUnicodeInfo.InternalGetDigitValues(ch)->digit;
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x00059D56 File Offset: 0x00058D56
		public static double GetNumericValue(char ch)
		{
			return CharUnicodeInfo.InternalGetNumericValue((int)ch);
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x00059D5E File Offset: 0x00058D5E
		public static double GetNumericValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return CharUnicodeInfo.InternalGetNumericValue(CharUnicodeInfo.InternalConvertToUtf32(s, index));
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x00059D9C File Offset: 0x00058D9C
		public static int GetDecimalDigitValue(char ch)
		{
			return (int)CharUnicodeInfo.InternalGetDecimalDigitValue((int)ch);
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x00059DA4 File Offset: 0x00058DA4
		public static int GetDecimalDigitValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return (int)CharUnicodeInfo.InternalGetDecimalDigitValue(CharUnicodeInfo.InternalConvertToUtf32(s, index));
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x00059DE2 File Offset: 0x00058DE2
		public static int GetDigitValue(char ch)
		{
			return (int)CharUnicodeInfo.InternalGetDigitValue((int)ch);
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x00059DEA File Offset: 0x00058DEA
		public static int GetDigitValue(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index < 0 || index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return (int)CharUnicodeInfo.InternalGetDigitValue(CharUnicodeInfo.InternalConvertToUtf32(s, index));
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x00059E28 File Offset: 0x00058E28
		public static UnicodeCategory GetUnicodeCategory(char ch)
		{
			return CharUnicodeInfo.InternalGetUnicodeCategory((int)ch);
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x00059E30 File Offset: 0x00058E30
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
			return CharUnicodeInfo.InternalGetUnicodeCategory(s, index);
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x00059E5B File Offset: 0x00058E5B
		internal static UnicodeCategory InternalGetUnicodeCategory(int ch)
		{
			return (UnicodeCategory)CharUnicodeInfo.InternalGetCategoryValue(ch, 0);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x00059E64 File Offset: 0x00058E64
		internal unsafe static byte InternalGetCategoryValue(int ch, int offset)
		{
			ushort num = CharUnicodeInfo.m_pCategoryLevel1Index[ch >> 8];
			num = CharUnicodeInfo.m_pCategoryLevel1Index[(int)num + ((ch >> 4) & 15)];
			byte* ptr = (byte*)(CharUnicodeInfo.m_pCategoryLevel1Index + num);
			byte b = ptr[ch & 15];
			return CharUnicodeInfo.m_pCategoriesValue[(int)(b * 2) + offset];
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x00059EB4 File Offset: 0x00058EB4
		internal static BidiCategory GetBidiCategory(string s, int index)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (index >= s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return (BidiCategory)CharUnicodeInfo.InternalGetCategoryValue(CharUnicodeInfo.InternalConvertToUtf32(s, index), 1);
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x00059EE5 File Offset: 0x00058EE5
		internal static UnicodeCategory InternalGetUnicodeCategory(string value, int index)
		{
			return CharUnicodeInfo.InternalGetUnicodeCategory(CharUnicodeInfo.InternalConvertToUtf32(value, index));
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x00059EF3 File Offset: 0x00058EF3
		internal static UnicodeCategory InternalGetUnicodeCategory(string str, int index, out int charLength)
		{
			return CharUnicodeInfo.InternalGetUnicodeCategory(CharUnicodeInfo.InternalConvertToUtf32(str, index, out charLength));
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x00059F02 File Offset: 0x00058F02
		internal static bool IsCombiningCategory(UnicodeCategory uc)
		{
			return uc == UnicodeCategory.NonSpacingMark || uc == UnicodeCategory.SpacingCombiningMark || uc == UnicodeCategory.EnclosingMark;
		}

		// Token: 0x06002357 RID: 9047
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void nativeInitTable(byte* bytePtr);

		// Token: 0x04000ED3 RID: 3795
		internal const char HIGH_SURROGATE_START = '\ud800';

		// Token: 0x04000ED4 RID: 3796
		internal const char HIGH_SURROGATE_END = '\udbff';

		// Token: 0x04000ED5 RID: 3797
		internal const char LOW_SURROGATE_START = '\udc00';

		// Token: 0x04000ED6 RID: 3798
		internal const char LOW_SURROGATE_END = '\udfff';

		// Token: 0x04000ED7 RID: 3799
		internal const int UNICODE_CATEGORY_OFFSET = 0;

		// Token: 0x04000ED8 RID: 3800
		internal const int BIDI_CATEGORY_OFFSET = 1;

		// Token: 0x04000ED9 RID: 3801
		internal const string UNICODE_INFO_FILE_NAME = "charinfo.nlp";

		// Token: 0x04000EDA RID: 3802
		internal const int UNICODE_PLANE01_START = 65536;

		// Token: 0x04000EDB RID: 3803
		private unsafe static byte* m_pDataTable = GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(CharUnicodeInfo).Assembly, "charinfo.nlp");

		// Token: 0x04000EDC RID: 3804
		private unsafe static ushort* m_pCategoryLevel1Index;

		// Token: 0x04000EDD RID: 3805
		private unsafe static byte* m_pCategoriesValue;

		// Token: 0x04000EDE RID: 3806
		private unsafe static ushort* m_pNumericLevel1Index;

		// Token: 0x04000EDF RID: 3807
		private unsafe static byte* m_pNumericValues;

		// Token: 0x04000EE0 RID: 3808
		private unsafe static CharUnicodeInfo.DigitValues* m_pDigitValues;

		// Token: 0x02000378 RID: 888
		[StructLayout(LayoutKind.Explicit)]
		internal struct UnicodeDataHeader
		{
			// Token: 0x04000EE1 RID: 3809
			[FieldOffset(0)]
			internal char TableName;

			// Token: 0x04000EE2 RID: 3810
			[FieldOffset(32)]
			internal ushort version;

			// Token: 0x04000EE3 RID: 3811
			[FieldOffset(40)]
			internal uint OffsetToCategoriesIndex;

			// Token: 0x04000EE4 RID: 3812
			[FieldOffset(44)]
			internal uint OffsetToCategoriesValue;

			// Token: 0x04000EE5 RID: 3813
			[FieldOffset(48)]
			internal uint OffsetToNumbericIndex;

			// Token: 0x04000EE6 RID: 3814
			[FieldOffset(52)]
			internal uint OffsetToDigitValue;

			// Token: 0x04000EE7 RID: 3815
			[FieldOffset(56)]
			internal uint OffsetToNumbericValue;
		}

		// Token: 0x02000379 RID: 889
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		internal struct DigitValues
		{
			// Token: 0x04000EE8 RID: 3816
			internal sbyte decimalDigit;

			// Token: 0x04000EE9 RID: 3817
			internal sbyte digit;
		}
	}
}
