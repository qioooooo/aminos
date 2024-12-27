using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Web.Util
{
	// Token: 0x0200078F RID: 1935
	internal static class StringUtil
	{
		// Token: 0x06005D10 RID: 23824 RVA: 0x00174D7F File Offset: 0x00173D7F
		internal static string CheckAndTrimString(string paramValue, string paramName)
		{
			return StringUtil.CheckAndTrimString(paramValue, paramName, true);
		}

		// Token: 0x06005D11 RID: 23825 RVA: 0x00174D89 File Offset: 0x00173D89
		internal static string CheckAndTrimString(string paramValue, string paramName, bool throwIfNull)
		{
			return StringUtil.CheckAndTrimString(paramValue, paramName, throwIfNull, -1);
		}

		// Token: 0x06005D12 RID: 23826 RVA: 0x00174D94 File Offset: 0x00173D94
		internal static string CheckAndTrimString(string paramValue, string paramName, bool throwIfNull, int lengthToCheck)
		{
			if (paramValue == null)
			{
				if (throwIfNull)
				{
					throw new ArgumentNullException(paramName);
				}
				return null;
			}
			else
			{
				string text = paramValue.Trim();
				if (text.Length == 0)
				{
					throw new ArgumentException(SR.GetString("PersonalizationProviderHelper_TrimmedEmptyString", new object[] { paramName }));
				}
				if (lengthToCheck > -1 && text.Length > lengthToCheck)
				{
					throw new ArgumentException(SR.GetString("StringUtil_Trimmed_String_Exceed_Maximum_Length", new object[]
					{
						paramValue,
						paramName,
						lengthToCheck.ToString(CultureInfo.InvariantCulture)
					}));
				}
				return text;
			}
		}

		// Token: 0x06005D13 RID: 23827 RVA: 0x00174E18 File Offset: 0x00173E18
		internal static bool Equals(string s1, string s2)
		{
			return s1 == s2 || (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2));
		}

		// Token: 0x06005D14 RID: 23828 RVA: 0x00174E38 File Offset: 0x00173E38
		internal static bool EqualsIgnoreCase(string s1, string s2)
		{
			return (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2)) || (!string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2) && s2.Length == s1.Length && 0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x06005D15 RID: 23829 RVA: 0x00174E8C File Offset: 0x00173E8C
		internal static bool EqualsIgnoreCase(string s1, int index1, string s2, int index2, int length)
		{
			return string.Compare(s1, index1, s2, index2, length, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06005D16 RID: 23830 RVA: 0x00174E9D File Offset: 0x00173E9D
		internal unsafe static string StringFromWCharPtr(IntPtr ip, int length)
		{
			return new string((char*)(void*)ip, 0, length);
		}

		// Token: 0x06005D17 RID: 23831 RVA: 0x00174EAC File Offset: 0x00173EAC
		internal static string StringFromCharPtr(IntPtr ip, int length)
		{
			return Marshal.PtrToStringAnsi(ip, length);
		}

		// Token: 0x06005D18 RID: 23832 RVA: 0x00174EB8 File Offset: 0x00173EB8
		internal static bool StringEndsWith(string s, char c)
		{
			int length = s.Length;
			return length != 0 && s[length - 1] == c;
		}

		// Token: 0x06005D19 RID: 23833 RVA: 0x00174EE0 File Offset: 0x00173EE0
		internal unsafe static bool StringEndsWith(string s1, string s2)
		{
			int num = s1.Length - s2.Length;
			if (num < 0)
			{
				return false;
			}
			fixed (char* ptr = s1, ptr2 = s2)
			{
				char* ptr3 = ptr + num;
				char* ptr4 = ptr2;
				int length = s2.Length;
				while (length-- > 0)
				{
					if (*(ptr3++) != *(ptr4++))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005D1A RID: 23834 RVA: 0x00174F60 File Offset: 0x00173F60
		internal static bool StringEndsWithIgnoreCase(string s1, string s2)
		{
			int num = s1.Length - s2.Length;
			return num >= 0 && 0 == string.Compare(s1, num, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06005D1B RID: 23835 RVA: 0x00174F94 File Offset: 0x00173F94
		internal static bool StringStartsWith(string s, char c)
		{
			return s.Length != 0 && s[0] == c;
		}

		// Token: 0x06005D1C RID: 23836 RVA: 0x00174FAC File Offset: 0x00173FAC
		internal unsafe static bool StringStartsWith(string s1, string s2)
		{
			if (s2.Length > s1.Length)
			{
				return false;
			}
			fixed (char* ptr = s1, ptr2 = s2)
			{
				char* ptr3 = ptr;
				char* ptr4 = ptr2;
				int length = s2.Length;
				while (length-- > 0)
				{
					if (*(ptr3++) != *(ptr4++))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005D1D RID: 23837 RVA: 0x00175020 File Offset: 0x00174020
		internal static bool StringStartsWithIgnoreCase(string s1, string s2)
		{
			return !string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2) && s2.Length <= s1.Length && 0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06005D1E RID: 23838 RVA: 0x00175058 File Offset: 0x00174058
		internal unsafe static void UnsafeStringCopy(string src, int srcIndex, char[] dest, int destIndex, int len)
		{
			int num = len * 2;
			fixed (char* ptr = src, ptr2 = dest)
			{
				byte* ptr3 = (byte*)(ptr + srcIndex);
				byte* ptr4 = (byte*)ptr2 + (IntPtr)destIndex * 2;
				StringUtil.memcpyimpl(ptr3, ptr4, num);
			}
		}

		// Token: 0x06005D1F RID: 23839 RVA: 0x001750B4 File Offset: 0x001740B4
		internal static bool StringArrayEquals(string[] a, string[] b)
		{
			if (a == null != (b == null))
			{
				return false;
			}
			if (a == null)
			{
				return true;
			}
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005D20 RID: 23840 RVA: 0x001750FC File Offset: 0x001740FC
		internal unsafe static int GetStringHashCode(string s)
		{
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = s);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			int num = 352654597;
			int num2 = num;
			int* ptr2 = (int*)ptr;
			for (int i = s.Length; i > 0; i -= 4)
			{
				num = ((num << 5) + num + (num >> 27)) ^ *ptr2;
				if (i <= 2)
				{
					break;
				}
				num2 = ((num2 << 5) + num2 + (num2 >> 27)) ^ ptr2[1];
				ptr2 += 2;
			}
			return num + num2 * 1566083941;
		}

		// Token: 0x06005D21 RID: 23841 RVA: 0x00175170 File Offset: 0x00174170
		internal unsafe static void memcpyimpl(byte* src, byte* dest, int len)
		{
			if (len >= 16)
			{
				do
				{
					*(int*)dest = *(int*)src;
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(int*)(dest + 8) = *(int*)(src + 8);
					*(int*)(dest + 12) = *(int*)(src + 12);
					dest += 16;
					src += 16;
				}
				while ((len -= 16) >= 16);
			}
			if (len > 0)
			{
				if ((len & 8) != 0)
				{
					*(int*)dest = *(int*)src;
					*(int*)(dest + 4) = *(int*)(src + 4);
					dest += 8;
					src += 8;
				}
				if ((len & 4) != 0)
				{
					*(int*)dest = *(int*)src;
					dest += 4;
					src += 4;
				}
				if ((len & 2) != 0)
				{
					*(short*)dest = *(short*)src;
					dest += 2;
					src += 2;
				}
				if ((len & 1) != 0)
				{
					*(dest++) = *(src++);
				}
			}
		}

		// Token: 0x06005D22 RID: 23842 RVA: 0x00175224 File Offset: 0x00174224
		internal static string[] ObjectArrayToStringArray(object[] objectArray)
		{
			string[] array = new string[objectArray.Length];
			objectArray.CopyTo(array, 0);
			return array;
		}
	}
}
