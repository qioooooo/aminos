using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System
{
	// Token: 0x02000024 RID: 36
	[ComVisible(true)]
	[Serializable]
	public sealed class String : IComparable, ICloneable, IConvertible, IComparable<string>, IEnumerable<char>, IEnumerable, IEquatable<string>
	{
		// Token: 0x06000138 RID: 312 RVA: 0x00006536 File Offset: 0x00005536
		public static string Join(string separator, string[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return string.Join(separator, value, 0, value.Length);
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00006551 File Offset: 0x00005551
		internal char FirstChar
		{
			get
			{
				return this.m_firstChar;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000655C File Offset: 0x0000555C
		public unsafe static string Join(string separator, string[] value, int startIndex, int count)
		{
			if (separator == null)
			{
				separator = string.Empty;
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
			}
			if (startIndex > value.Length - count)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (count == 0)
			{
				return string.Empty;
			}
			int num = 0;
			int num2 = startIndex + count - 1;
			for (int i = startIndex; i <= num2; i++)
			{
				if (value[i] != null)
				{
					num += value[i].Length;
				}
			}
			num += (count - 1) * separator.Length;
			if (num < 0 || num + 1 < 0)
			{
				throw new OutOfMemoryException();
			}
			if (num == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(num);
			fixed (char* ptr = &text.m_firstChar)
			{
				UnSafeCharBuffer unSafeCharBuffer = new UnSafeCharBuffer(ptr, num);
				unSafeCharBuffer.AppendString(value[startIndex]);
				for (int j = startIndex + 1; j <= num2; j++)
				{
					unSafeCharBuffer.AppendString(separator);
					unSafeCharBuffer.AppendString(value[j]);
				}
			}
			return text;
		}

		// Token: 0x0600013B RID: 315
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeCompareOrdinal(string strA, string strB, bool bIgnoreCase);

		// Token: 0x0600013C RID: 316
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeCompareOrdinalEx(string strA, int indexA, string strB, int indexB, int count);

		// Token: 0x0600013D RID: 317
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int nativeCompareOrdinalWC(string strA, char* strBChars, bool bIgnoreCase, out bool success);

		// Token: 0x0600013E RID: 318 RVA: 0x00006674 File Offset: 0x00005674
		internal unsafe static string SmallCharToUpper(string strIn)
		{
			int length = strIn.Length;
			string text = string.FastAllocateString(length);
			fixed (char* ptr = &strIn.m_firstChar, ptr2 = &text.m_firstChar)
			{
				int num = -33;
				for (int i = 0; i < length; i++)
				{
					char c = ptr[i];
					if (c >= 'a' && c <= 'z')
					{
						c = (char)((int)c & num);
					}
					ptr2[i] = c;
				}
			}
			return text;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000066E4 File Offset: 0x000056E4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private unsafe static bool EqualsHelper(string strA, string strB)
		{
			int i = strA.Length;
			if (i != strB.Length)
			{
				return false;
			}
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = strA);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			IntPtr intPtr4;
			IntPtr intPtr3 = (intPtr4 = strB);
			if (intPtr3 != 0)
			{
				intPtr4 = (IntPtr)((int)intPtr3 + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr2 = intPtr4;
			char* ptr3 = ptr;
			char* ptr4 = ptr2;
			while (i >= 10)
			{
				if (*(int*)ptr3 != *(int*)ptr4 || *(int*)(ptr3 + 2) != *(int*)(ptr4 + 2) || *(int*)(ptr3 + 4) != *(int*)(ptr4 + 4) || *(int*)(ptr3 + 6) != *(int*)(ptr4 + 6) || *(int*)(ptr3 + 8) != *(int*)(ptr4 + 8))
				{
					IL_00A9:
					while (i > 0 && *(int*)ptr3 == *(int*)ptr4)
					{
						ptr3 += 2;
						ptr4 += 2;
						i -= 2;
					}
					return i <= 0;
				}
				ptr3 += 10;
				ptr4 += 10;
				i -= 10;
			}
			goto IL_00A9;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000067AC File Offset: 0x000057AC
		private unsafe static int CompareOrdinalHelper(string strA, string strB)
		{
			int i = Math.Min(strA.Length, strB.Length);
			int num = -1;
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = strA);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			IntPtr intPtr4;
			IntPtr intPtr3 = (intPtr4 = strB);
			if (intPtr3 != 0)
			{
				intPtr4 = (IntPtr)((int)intPtr3 + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr2 = intPtr4;
			char* ptr3 = ptr;
			char* ptr4 = ptr2;
			while (i >= 10)
			{
				if (*(int*)ptr3 != *(int*)ptr4)
				{
					num = 0;
					break;
				}
				if (*(int*)(ptr3 + 2) != *(int*)(ptr4 + 2))
				{
					num = 2;
					break;
				}
				if (*(int*)(ptr3 + 4) != *(int*)(ptr4 + 4))
				{
					num = 4;
					break;
				}
				if (*(int*)(ptr3 + 6) != *(int*)(ptr4 + 6))
				{
					num = 6;
					break;
				}
				if (*(int*)(ptr3 + 8) != *(int*)(ptr4 + 8))
				{
					num = 8;
					break;
				}
				ptr3 += 10;
				ptr4 += 10;
				i -= 10;
			}
			int num3;
			if (num != -1)
			{
				ptr3 += num;
				ptr4 += num;
				int num2;
				if ((num2 = (int)(*ptr3 - *ptr4)) != 0)
				{
					num3 = num2;
				}
				else
				{
					num3 = (int)(ptr3[1] - ptr4[1]);
				}
			}
			else
			{
				while (i > 0 && *(int*)ptr3 == *(int*)ptr4)
				{
					ptr3 += 2;
					ptr4 += 2;
					i -= 2;
				}
				if (i > 0)
				{
					int num4;
					if ((num4 = (int)(*ptr3 - *ptr4)) != 0)
					{
						num3 = num4;
					}
					else
					{
						num3 = (int)(ptr3[1] - ptr4[1]);
					}
				}
				else
				{
					num3 = strA.Length - strB.Length;
				}
			}
			return num3;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000068F8 File Offset: 0x000058F8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public override bool Equals(object obj)
		{
			string text = obj as string;
			return (text != null || this == null) && string.EqualsHelper(this, text);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000691B File Offset: 0x0000591B
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public bool Equals(string value)
		{
			return (value != null || this == null) && string.EqualsHelper(this, value);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000692C File Offset: 0x0000592C
		public bool Equals(string value, StringComparison comparisonType)
		{
			if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
			if (this == value)
			{
				return true;
			}
			if (value == null)
			{
				return false;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(this, value, CompareOptions.None) == 0;
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(this, value, CompareOptions.IgnoreCase) == 0;
			case StringComparison.InvariantCulture:
				return CultureInfo.InvariantCulture.CompareInfo.Compare(this, value, CompareOptions.None) == 0;
			case StringComparison.InvariantCultureIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.Compare(this, value, CompareOptions.IgnoreCase) == 0;
			case StringComparison.Ordinal:
				return this.Equals(value);
			case StringComparison.OrdinalIgnoreCase:
				if (this.Length != value.Length)
				{
					return false;
				}
				if (this.IsAscii() && value.IsAscii())
				{
					return string.nativeCompareOrdinal(this, value, true) == 0;
				}
				return TextInfo.CompareOrdinalIgnoreCase(this, value) == 0;
			default:
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00006A31 File Offset: 0x00005A31
		public static bool Equals(string a, string b)
		{
			return a == b || (a != null && b != null && string.EqualsHelper(a, b));
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006A48 File Offset: 0x00005A48
		public static bool Equals(string a, string b, StringComparison comparisonType)
		{
			if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.None) == 0;
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(a, b, CompareOptions.IgnoreCase) == 0;
			case StringComparison.InvariantCulture:
				return CultureInfo.InvariantCulture.CompareInfo.Compare(a, b, CompareOptions.None) == 0;
			case StringComparison.InvariantCultureIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.Compare(a, b, CompareOptions.IgnoreCase) == 0;
			case StringComparison.Ordinal:
				return string.EqualsHelper(a, b);
			case StringComparison.OrdinalIgnoreCase:
				if (a.Length != b.Length)
				{
					return false;
				}
				if (a.IsAscii() && b.IsAscii())
				{
					return string.nativeCompareOrdinal(a, b, true) == 0;
				}
				return TextInfo.CompareOrdinalIgnoreCase(a, b) == 0;
			default:
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006B50 File Offset: 0x00005B50
		public static bool operator ==(string a, string b)
		{
			return string.Equals(a, b);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006B59 File Offset: 0x00005B59
		public static bool operator !=(string a, string b)
		{
			return !string.Equals(a, b);
		}

		// Token: 0x1700001A RID: 26
		[IndexerName("Chars")]
		public extern char this[int index]
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00006B68 File Offset: 0x00005B68
		public unsafe void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
			}
			if (sourceIndex < 0)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count > this.Length - sourceIndex)
			{
				throw new ArgumentOutOfRangeException("sourceIndex", Environment.GetResourceString("ArgumentOutOfRange_IndexCount"));
			}
			if (destinationIndex > destination.Length - count || destinationIndex < 0)
			{
				throw new ArgumentOutOfRangeException("destinationIndex", Environment.GetResourceString("ArgumentOutOfRange_IndexCount"));
			}
			if (count > 0)
			{
				fixed (char* ptr = &this.m_firstChar)
				{
					fixed (char* ptr2 = destination)
					{
						string.wstrcpy(ptr2 + destinationIndex, ptr + sourceIndex, count);
					}
				}
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00006C38 File Offset: 0x00005C38
		public unsafe char[] ToCharArray()
		{
			int length = this.Length;
			char[] array = new char[length];
			if (length > 0)
			{
				fixed (char* ptr = &this.m_firstChar)
				{
					fixed (char* ptr2 = array)
					{
						string.wstrcpyPtrAligned(ptr2, ptr, length);
					}
				}
			}
			return array;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00006C8C File Offset: 0x00005C8C
		public unsafe char[] ToCharArray(int startIndex, int length)
		{
			if (startIndex < 0 || startIndex > this.Length || startIndex > this.Length - length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			char[] array = new char[length];
			if (length > 0)
			{
				fixed (char* ptr = &this.m_firstChar)
				{
					fixed (char* ptr2 = array)
					{
						string.wstrcpy(ptr2, ptr + startIndex, length);
					}
				}
			}
			return array;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006D1E File Offset: 0x00005D1E
		public static bool IsNullOrEmpty(string value)
		{
			return value == null || value.Length == 0;
		}

		// Token: 0x0600014D RID: 333
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int InternalMarvin32HashString(string s, int sLen, long additionalEntropy);

		// Token: 0x0600014E RID: 334 RVA: 0x00006D30 File Offset: 0x00005D30
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public unsafe override int GetHashCode()
		{
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = this);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			int num = 352654597;
			int num2 = num;
			int* ptr2 = (int*)ptr;
			for (int i = this.Length; i > 0; i -= 4)
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

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600014F RID: 335
		public extern int Length
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00006DA4 File Offset: 0x00005DA4
		internal int ArrayLength
		{
			get
			{
				return this.m_arrayLength;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00006DAC File Offset: 0x00005DAC
		internal int Capacity
		{
			get
			{
				return this.m_arrayLength - 1;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006DB6 File Offset: 0x00005DB6
		public string[] Split(params char[] separator)
		{
			return this.Split(separator, int.MaxValue, StringSplitOptions.None);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006DC5 File Offset: 0x00005DC5
		public string[] Split(char[] separator, int count)
		{
			return this.Split(separator, count, StringSplitOptions.None);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00006DD0 File Offset: 0x00005DD0
		[ComVisible(false)]
		public string[] Split(char[] separator, StringSplitOptions options)
		{
			return this.Split(separator, int.MaxValue, options);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006DE0 File Offset: 0x00005DE0
		[ComVisible(false)]
		public string[] Split(char[] separator, int count, StringSplitOptions options)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
			}
			if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)options }));
			}
			bool flag = options == StringSplitOptions.RemoveEmptyEntries;
			if (count == 0 || (flag && this.Length == 0))
			{
				return new string[0];
			}
			int[] array = new int[this.Length];
			int num = this.MakeSeparatorList(separator, ref array);
			if (num == 0 || count == 1)
			{
				return new string[] { this };
			}
			if (flag)
			{
				return this.InternalSplitOmitEmptyEntries(array, null, num, count);
			}
			return this.InternalSplitKeepEmptyEntries(array, null, num, count);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006E8E File Offset: 0x00005E8E
		[ComVisible(false)]
		public string[] Split(string[] separator, StringSplitOptions options)
		{
			return this.Split(separator, int.MaxValue, options);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006EA0 File Offset: 0x00005EA0
		[ComVisible(false)]
		public string[] Split(string[] separator, int count, StringSplitOptions options)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NegativeCount"));
			}
			if (options < StringSplitOptions.None || options > StringSplitOptions.RemoveEmptyEntries)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)options }));
			}
			bool flag = options == StringSplitOptions.RemoveEmptyEntries;
			if (separator == null || separator.Length == 0)
			{
				return this.Split(null, count, options);
			}
			if (count == 0 || (flag && this.Length == 0))
			{
				return new string[0];
			}
			int[] array = new int[this.Length];
			int[] array2 = new int[this.Length];
			int num = this.MakeSeparatorList(separator, ref array, ref array2);
			if (num == 0 || count == 1)
			{
				return new string[] { this };
			}
			if (flag)
			{
				return this.InternalSplitOmitEmptyEntries(array, array2, num, count);
			}
			return this.InternalSplitKeepEmptyEntries(array, array2, num, count);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006F74 File Offset: 0x00005F74
		private string[] InternalSplitKeepEmptyEntries(int[] sepList, int[] lengthList, int numReplaces, int count)
		{
			int num = 0;
			int num2 = 0;
			count--;
			int num3 = ((numReplaces < count) ? numReplaces : count);
			string[] array = new string[num3 + 1];
			int num4 = 0;
			while (num4 < num3 && num < this.Length)
			{
				array[num2++] = this.Substring(num, sepList[num4] - num);
				num = sepList[num4] + ((lengthList == null) ? 1 : lengthList[num4]);
				num4++;
			}
			if (num < this.Length && num3 >= 0)
			{
				array[num2] = this.Substring(num);
			}
			else if (num2 == num3)
			{
				array[num2] = string.Empty;
			}
			return array;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007004 File Offset: 0x00006004
		private string[] InternalSplitOmitEmptyEntries(int[] sepList, int[] lengthList, int numReplaces, int count)
		{
			int num = ((numReplaces < count) ? (numReplaces + 1) : count);
			string[] array = new string[num];
			int num2 = 0;
			int num3 = 0;
			int i = 0;
			while (i < numReplaces && num2 < this.Length)
			{
				if (sepList[i] - num2 > 0)
				{
					array[num3++] = this.Substring(num2, sepList[i] - num2);
				}
				num2 = sepList[i] + ((lengthList == null) ? 1 : lengthList[i]);
				if (num3 == count - 1)
				{
					while (i < numReplaces - 1)
					{
						if (num2 != sepList[++i])
						{
							break;
						}
						num2 += ((lengthList == null) ? 1 : lengthList[i]);
					}
					break;
				}
				i++;
			}
			if (num2 < this.Length)
			{
				array[num3++] = this.Substring(num2);
			}
			string[] array2 = array;
			if (num3 != num)
			{
				array2 = new string[num3];
				for (int j = 0; j < num3; j++)
				{
					array2[j] = array[j];
				}
			}
			return array2;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000070DC File Offset: 0x000060DC
		private unsafe int MakeSeparatorList(char[] separator, ref int[] sepList)
		{
			int num = 0;
			if (separator == null || separator.Length == 0)
			{
				fixed (char* ptr = &this.m_firstChar)
				{
					int num2 = 0;
					while (num2 < this.Length && num < sepList.Length)
					{
						if (char.IsWhiteSpace(ptr[num2]))
						{
							sepList[num++] = num2;
						}
						num2++;
					}
				}
			}
			else
			{
				int num3 = sepList.Length;
				int num4 = separator.Length;
				fixed (char* ptr2 = &this.m_firstChar, ptr3 = separator)
				{
					int num5 = 0;
					while (num5 < this.Length && num < num3)
					{
						char* ptr4 = ptr3;
						int i = 0;
						while (i < num4)
						{
							if (ptr2[num5] == *ptr4)
							{
								sepList[num++] = num5;
								break;
							}
							i++;
							ptr4++;
						}
						num5++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000071BC File Offset: 0x000061BC
		private unsafe int MakeSeparatorList(string[] separators, ref int[] sepList, ref int[] lengthList)
		{
			int num = 0;
			int num2 = sepList.Length;
			int num3 = separators.Length;
			fixed (char* ptr = &this.m_firstChar)
			{
				int num4 = 0;
				while (num4 < this.Length && num < num2)
				{
					foreach (string text in separators)
					{
						if (!string.IsNullOrEmpty(text))
						{
							int length = text.Length;
							if (ptr[num4] == text[0] && length <= this.Length - num4 && (length == 1 || string.CompareOrdinal(this, num4, text, 0, length) == 0))
							{
								sepList[num] = num4;
								lengthList[num] = length;
								num++;
								num4 += length - 1;
								break;
							}
						}
					}
					num4++;
				}
			}
			return num;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000726A File Offset: 0x0000626A
		public string Substring(int startIndex)
		{
			return this.Substring(startIndex, this.Length - startIndex);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000727B File Offset: 0x0000627B
		public string Substring(int startIndex, int length)
		{
			return this.InternalSubStringWithChecks(startIndex, length, false);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007288 File Offset: 0x00006288
		internal string InternalSubStringWithChecks(int startIndex, int length, bool fAlwaysCopy)
		{
			int length2 = this.Length;
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			if (startIndex > length2)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndexLargerThanLength"));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
			}
			if (startIndex > length2 - length)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_IndexLength"));
			}
			if (length == 0)
			{
				return string.Empty;
			}
			return this.InternalSubString(startIndex, length, fAlwaysCopy);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007314 File Offset: 0x00006314
		private unsafe string InternalSubString(int startIndex, int length, bool fAlwaysCopy)
		{
			if (startIndex == 0 && length == this.Length && !fAlwaysCopy)
			{
				return this;
			}
			string text = string.FastAllocateString(length);
			fixed (char* ptr = &text.m_firstChar)
			{
				fixed (char* ptr2 = &this.m_firstChar)
				{
					string.wstrcpy(ptr, ptr2 + startIndex, length);
				}
			}
			return text;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000735D File Offset: 0x0000635D
		public string Trim(params char[] trimChars)
		{
			if (trimChars == null || trimChars.Length == 0)
			{
				trimChars = string.WhitespaceChars;
			}
			return this.TrimHelper(trimChars, 2);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007376 File Offset: 0x00006376
		public string TrimStart(params char[] trimChars)
		{
			if (trimChars == null || trimChars.Length == 0)
			{
				trimChars = string.WhitespaceChars;
			}
			return this.TrimHelper(trimChars, 0);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000738F File Offset: 0x0000638F
		public string TrimEnd(params char[] trimChars)
		{
			if (trimChars == null || trimChars.Length == 0)
			{
				trimChars = string.WhitespaceChars;
			}
			return this.TrimHelper(trimChars, 1);
		}

		// Token: 0x06000163 RID: 355
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(char* value);

		// Token: 0x06000164 RID: 356
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(char* value, int startIndex, int length);

		// Token: 0x06000165 RID: 357
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(sbyte* value);

		// Token: 0x06000166 RID: 358
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(sbyte* value, int startIndex, int length);

		// Token: 0x06000167 RID: 359
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe extern String(sbyte* value, int startIndex, int length, Encoding enc);

		// Token: 0x06000168 RID: 360 RVA: 0x000073A8 File Offset: 0x000063A8
		private unsafe static string CreateString(sbyte* value, int startIndex, int length, Encoding enc)
		{
			if (enc == null)
			{
				return new string(value, startIndex, length);
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			if (value + startIndex < value)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
			}
			byte[] array = new byte[length];
			try
			{
				Buffer.memcpy((byte*)value, startIndex, array, 0, length);
			}
			catch (NullReferenceException)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
			}
			return enc.GetString(array);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007450 File Offset: 0x00006450
		internal unsafe static string CreateStringFromEncoding(byte* bytes, int byteLength, Encoding encoding)
		{
			int charCount = encoding.GetCharCount(bytes, byteLength, null);
			if (charCount == 0)
			{
				return string.Empty;
			}
			string text = string.FastAllocateString(charCount);
			fixed (char* ptr = &text.m_firstChar)
			{
				encoding.GetChars(bytes, byteLength, ptr, charCount, null);
			}
			return text;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007490 File Offset: 0x00006490
		internal unsafe byte[] ConvertToAnsi_BestFit_Throw(int iMaxDBCSCharByteSize)
		{
			int num = (this.Length + 3) * iMaxDBCSCharByteSize;
			byte[] array = new byte[num];
			uint num2 = 0U;
			uint num3 = 0U;
			int num4;
			fixed (byte* ptr = array)
			{
				fixed (char* ptr2 = &this.m_firstChar)
				{
					num4 = Win32Native.WideCharToMultiByte(0U, num2, ptr2, this.Length, ptr, num, IntPtr.Zero, new IntPtr((void*)(&num3)));
				}
			}
			if (num3 != 0U)
			{
				throw new ArgumentException(Environment.GetResourceString("Interop_Marshal_Unmappable_Char"));
			}
			array[num4] = 0;
			return array;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000751B File Offset: 0x0000651B
		public bool IsNormalized()
		{
			return this.IsNormalized(NormalizationForm.FormC);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007524 File Offset: 0x00006524
		public bool IsNormalized(NormalizationForm normalizationForm)
		{
			return (this.IsFastSort() && (normalizationForm == NormalizationForm.FormC || normalizationForm == NormalizationForm.FormKC || normalizationForm == NormalizationForm.FormD || normalizationForm == NormalizationForm.FormKD)) || Normalization.IsNormalized(this, normalizationForm);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007547 File Offset: 0x00006547
		public string Normalize()
		{
			return this.Normalize(NormalizationForm.FormC);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007550 File Offset: 0x00006550
		public string Normalize(NormalizationForm normalizationForm)
		{
			if (this.IsAscii() && (normalizationForm == NormalizationForm.FormC || normalizationForm == NormalizationForm.FormKC || normalizationForm == NormalizationForm.FormD || normalizationForm == NormalizationForm.FormKD))
			{
				return this;
			}
			return Normalization.Normalize(this, normalizationForm);
		}

		// Token: 0x0600016F RID: 367
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string FastAllocateString(int length);

		// Token: 0x06000170 RID: 368 RVA: 0x00007574 File Offset: 0x00006574
		private unsafe static void FillStringChecked(string dest, int destPos, string src)
		{
			int length = src.Length;
			if (length > dest.Length - destPos)
			{
				throw new IndexOutOfRangeException();
			}
			fixed (char* ptr = &dest.m_firstChar)
			{
				fixed (char* ptr2 = &src.m_firstChar)
				{
					string.wstrcpy(ptr + destPos, ptr2, length);
				}
			}
		}

		// Token: 0x06000171 RID: 369
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(char[] value, int startIndex, int length);

		// Token: 0x06000172 RID: 370
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(char[] value);

		// Token: 0x06000173 RID: 371 RVA: 0x000075BC File Offset: 0x000065BC
		private unsafe static void wstrcpyPtrAligned(char* dmem, char* smem, int charCount)
		{
			while (charCount >= 8)
			{
				*(int*)dmem = (int)(*(uint*)smem);
				*(int*)(dmem + 2) = (int)(*(uint*)(smem + 2));
				*(int*)(dmem + 4) = (int)(*(uint*)(smem + 4));
				*(int*)(dmem + 6) = (int)(*(uint*)(smem + 6));
				dmem += 8;
				smem += 8;
				charCount -= 8;
			}
			if ((charCount & 4) != 0)
			{
				*(int*)dmem = (int)(*(uint*)smem);
				*(int*)(dmem + 2) = (int)(*(uint*)(smem + 2));
				dmem += 4;
				smem += 4;
			}
			if ((charCount & 2) != 0)
			{
				*(int*)dmem = (int)(*(uint*)smem);
				dmem += 2;
				smem += 2;
			}
			if ((charCount & 1) != 0)
			{
				*dmem = *smem;
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007644 File Offset: 0x00006644
		private unsafe static void wstrcpy(char* dmem, char* smem, int charCount)
		{
			if (charCount > 0)
			{
				if ((dmem & 2) != 0)
				{
					*dmem = *smem;
					dmem++;
					smem++;
					charCount--;
				}
				while (charCount >= 8)
				{
					*(int*)dmem = (int)(*(uint*)smem);
					*(int*)(dmem + 2) = (int)(*(uint*)(smem + 2));
					*(int*)(dmem + 4) = (int)(*(uint*)(smem + 4));
					*(int*)(dmem + 6) = (int)(*(uint*)(smem + 6));
					dmem += 8;
					smem += 8;
					charCount -= 8;
				}
				if ((charCount & 4) != 0)
				{
					*(int*)dmem = (int)(*(uint*)smem);
					*(int*)(dmem + 2) = (int)(*(uint*)(smem + 2));
					dmem += 4;
					smem += 4;
				}
				if ((charCount & 2) != 0)
				{
					*(int*)dmem = (int)(*(uint*)smem);
					dmem += 2;
					smem += 2;
				}
				if ((charCount & 1) != 0)
				{
					*dmem = *smem;
				}
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000076F0 File Offset: 0x000066F0
		private unsafe string CtorCharArray(char[] value)
		{
			if (value != null && value.Length != 0)
			{
				string text = string.FastAllocateString(value.Length);
				fixed (char* ptr = text, ptr2 = value)
				{
					string.wstrcpyPtrAligned(ptr, ptr2, value.Length);
				}
				return text;
			}
			return string.Empty;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007750 File Offset: 0x00006750
		private unsafe string CtorCharArrayStartLength(char[] value, int startIndex, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
			}
			if (startIndex > value.Length - length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (length > 0)
			{
				string text = string.FastAllocateString(length);
				fixed (char* ptr = text, ptr2 = value)
				{
					string.wstrcpy(ptr, ptr2 + startIndex, length);
				}
				return text;
			}
			return string.Empty;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007808 File Offset: 0x00006808
		private unsafe string CtorCharCount(char c, int count)
		{
			if (count > 0)
			{
				string text = string.FastAllocateString(count);
				fixed (char* ptr = text)
				{
					char* ptr2 = ptr;
					while ((ptr2 & 3U) != 0U && count > 0)
					{
						*(ptr2++) = c;
						count--;
					}
					uint num = (uint)(((uint)c << 16) | c);
					if (count >= 4)
					{
						count -= 4;
						do
						{
							*(int*)ptr2 = (int)num;
							*(int*)(ptr2 + 2) = (int)num;
							ptr2 += 4;
							count -= 4;
						}
						while (count >= 0);
					}
					if ((count & 2) != 0)
					{
						*(int*)ptr2 = (int)num;
						ptr2 += 2;
					}
					if ((count & 1) != 0)
					{
						*ptr2 = c;
					}
				}
				return text;
			}
			if (count == 0)
			{
				return string.Empty;
			}
			throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_MustBeNonNegNum", new object[] { "count" }));
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000078BC File Offset: 0x000068BC
		private unsafe static int wcslen(char* ptr)
		{
			char* ptr2 = ptr;
			while ((ptr2 & 3U) != 0U && *ptr2 != '\0')
			{
				ptr2++;
			}
			if (*ptr2 != '\0')
			{
				for (;;)
				{
					if ((*ptr2 & ptr2[1]) == '\0')
					{
						if (*ptr2 == '\0')
						{
							break;
						}
						if (ptr2[1] == '\0')
						{
							break;
						}
					}
					ptr2 += 2;
				}
			}
			while (*ptr2 != '\0')
			{
				ptr2++;
			}
			return (int)((long)(ptr2 - ptr));
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007910 File Offset: 0x00006910
		private unsafe string CtorCharPtr(char* ptr)
		{
			if (ptr >= 64000)
			{
				try
				{
					int num = string.wcslen(ptr);
					string text = string.FastAllocateString(num);
					try
					{
						fixed (char* ptr2 = text)
						{
							string.wstrcpy(ptr2, ptr, num);
						}
					}
					finally
					{
						string text2 = null;
					}
					return text;
				}
				catch (NullReferenceException)
				{
					throw new ArgumentOutOfRangeException("ptr", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
				}
			}
			if (ptr == null)
			{
				return string.Empty;
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_MustBeStringPtrNotAtom"));
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000079A4 File Offset: 0x000069A4
		private unsafe string CtorCharPtrStartLength(char* ptr, int startIndex, int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			char* ptr2 = ptr + startIndex;
			if (ptr2 < ptr)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
			}
			string text = string.FastAllocateString(length);
			string text3;
			try
			{
				try
				{
					fixed (char* ptr3 = text)
					{
						string.wstrcpy(ptr3, ptr2, length);
					}
				}
				finally
				{
					string text2 = null;
				}
				text3 = text;
			}
			catch (NullReferenceException)
			{
				throw new ArgumentOutOfRangeException("ptr", Environment.GetResourceString("ArgumentOutOfRange_PartialWCHAR"));
			}
			return text3;
		}

		// Token: 0x0600017B RID: 379
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern String(char c, int count);

		// Token: 0x0600017C RID: 380 RVA: 0x00007A60 File Offset: 0x00006A60
		public static int Compare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007A74 File Offset: 0x00006A74
		public static int Compare(string strA, string strB, bool ignoreCase)
		{
			if (ignoreCase)
			{
				return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
			}
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007A9E File Offset: 0x00006A9E
		public static int Compare(string strA, string strB, CultureInfo culture, CompareOptions options)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.CompareInfo.Compare(strA, strB, options);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007ABC File Offset: 0x00006ABC
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, CultureInfo culture, CompareOptions options)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			int num = length;
			int num2 = length;
			if (strA != null && strA.Length - indexA < num)
			{
				num = strA.Length - indexA;
			}
			if (strB != null && strB.Length - indexB < num2)
			{
				num2 = strB.Length - indexB;
			}
			return culture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, options);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007B20 File Offset: 0x00006B20
		public static int Compare(string strA, string strB, StringComparison comparisonType)
		{
			if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
			if (strA == strB)
			{
				return 0;
			}
			if (strA == null)
			{
				return -1;
			}
			if (strB == null)
			{
				return 1;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, strB, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return string.CompareOrdinalHelper(strA, strB);
			case StringComparison.OrdinalIgnoreCase:
				if (strA.IsAscii() && strB.IsAscii())
				{
					return string.nativeCompareOrdinal(strA, strB, true);
				}
				return TextInfo.CompareOrdinalIgnoreCase(strA, strB);
			default:
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_StringComparison"));
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007C00 File Offset: 0x00006C00
		public static int Compare(string strA, string strB, bool ignoreCase, CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			if (ignoreCase)
			{
				return culture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase);
			}
			return culture.CompareInfo.Compare(strA, strB, CompareOptions.None);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007C30 File Offset: 0x00006C30
		public static int Compare(string strA, int indexA, string strB, int indexB, int length)
		{
			int num = length;
			int num2 = length;
			if (strA != null && strA.Length - indexA < num)
			{
				num = strA.Length - indexA;
			}
			if (strB != null && strB.Length - indexB < num2)
			{
				num2 = strB.Length - indexB;
			}
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.None);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007C88 File Offset: 0x00006C88
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase)
		{
			int num = length;
			int num2 = length;
			if (strA != null && strA.Length - indexA < num)
			{
				num = strA.Length - indexA;
			}
			if (strB != null && strB.Length - indexB < num2)
			{
				num2 = strB.Length - indexB;
			}
			if (ignoreCase)
			{
				return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.IgnoreCase);
			}
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.None);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007CFC File Offset: 0x00006CFC
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, bool ignoreCase, CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			int num = length;
			int num2 = length;
			if (strA != null && strA.Length - indexA < num)
			{
				num = strA.Length - indexA;
			}
			if (strB != null && strB.Length - indexB < num2)
			{
				num2 = strB.Length - indexB;
			}
			if (ignoreCase)
			{
				return culture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.IgnoreCase);
			}
			return culture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.None);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007D78 File Offset: 0x00006D78
		public static int Compare(string strA, int indexA, string strB, int indexB, int length, StringComparison comparisonType)
		{
			if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
			if (strA == null || strB == null)
			{
				if (strA == strB)
				{
					return 0;
				}
				if (strA != null)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (length < 0)
				{
					throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NegativeLength"));
				}
				if (indexA < 0)
				{
					throw new ArgumentOutOfRangeException("indexA", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (indexB < 0)
				{
					throw new ArgumentOutOfRangeException("indexB", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (strA.Length - indexA < 0)
				{
					throw new ArgumentOutOfRangeException("indexA", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (strB.Length - indexB < 0)
				{
					throw new ArgumentOutOfRangeException("indexB", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (length == 0 || (strA == strB && indexA == indexB))
				{
					return 0;
				}
				int num = length;
				int num2 = length;
				if (strA != null && strA.Length - indexA < num)
				{
					num = strA.Length - indexA;
				}
				if (strB != null && strB.Length - indexB < num2)
				{
					num2 = strB.Length - indexB;
				}
				switch (comparisonType)
				{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.None);
				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.IgnoreCase);
				case StringComparison.InvariantCulture:
					return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.None);
				case StringComparison.InvariantCultureIgnoreCase:
					return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, num, strB, indexB, num2, CompareOptions.IgnoreCase);
				case StringComparison.Ordinal:
					return string.nativeCompareOrdinalEx(strA, indexA, strB, indexB, length);
				case StringComparison.OrdinalIgnoreCase:
					return TextInfo.CompareOrdinalIgnoreCaseEx(strA, indexA, strB, indexB, length);
				default:
					throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"));
				}
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00007F30 File Offset: 0x00006F30
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is string))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeString"));
			}
			return string.Compare(this, (string)value, StringComparison.CurrentCulture);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007F5C File Offset: 0x00006F5C
		public int CompareTo(string strB)
		{
			if (strB == null)
			{
				return 1;
			}
			return CultureInfo.CurrentCulture.CompareInfo.Compare(this, strB, CompareOptions.None);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007F75 File Offset: 0x00006F75
		public static int CompareOrdinal(string strA, string strB)
		{
			if (strA == strB)
			{
				return 0;
			}
			if (strA == null)
			{
				return -1;
			}
			if (strB == null)
			{
				return 1;
			}
			return string.CompareOrdinalHelper(strA, strB);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00007F8E File Offset: 0x00006F8E
		public static int CompareOrdinal(string strA, int indexA, string strB, int indexB, int length)
		{
			if (strA != null && strB != null)
			{
				return string.nativeCompareOrdinalEx(strA, indexA, strB, indexB, length);
			}
			if (strA == strB)
			{
				return 0;
			}
			if (strA != null)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00007FAE File Offset: 0x00006FAE
		public bool Contains(string value)
		{
			return this.IndexOf(value, StringComparison.Ordinal) >= 0;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007FBE File Offset: 0x00006FBE
		public bool EndsWith(string value)
		{
			return this.EndsWith(value, false, null);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007FCC File Offset: 0x00006FCC
		[ComVisible(false)]
		public bool EndsWith(string value, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
			if (this == value)
			{
				return true;
			}
			if (value.Length == 0)
			{
				return true;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CultureInfo.InvariantCulture.CompareInfo.IsSuffix(this, value, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.IsSuffix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return this.Length >= value.Length && string.nativeCompareOrdinalEx(this, this.Length - value.Length, value, 0, value.Length) == 0;
			case StringComparison.OrdinalIgnoreCase:
				return this.Length >= value.Length && TextInfo.CompareOrdinalIgnoreCaseEx(this, this.Length - value.Length, value, 0, value.Length) == 0;
			default:
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000080F8 File Offset: 0x000070F8
		public bool EndsWith(string value, bool ignoreCase, CultureInfo culture)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this == value)
			{
				return true;
			}
			CultureInfo cultureInfo = ((culture == null) ? CultureInfo.CurrentCulture : culture);
			return cultureInfo.CompareInfo.IsSuffix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000813C File Offset: 0x0000713C
		internal bool EndsWith(char value)
		{
			int length = this.Length;
			return length != 0 && this[length - 1] == value;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008162 File Offset: 0x00007162
		public int IndexOf(char value)
		{
			return this.IndexOf(value, 0, this.Length);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008172 File Offset: 0x00007172
		public int IndexOf(char value, int startIndex)
		{
			return this.IndexOf(value, startIndex, this.Length - startIndex);
		}

		// Token: 0x06000191 RID: 401
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int IndexOf(char value, int startIndex, int count);

		// Token: 0x06000192 RID: 402 RVA: 0x00008184 File Offset: 0x00007184
		public int IndexOfAny(char[] anyOf)
		{
			return this.IndexOfAny(anyOf, 0, this.Length);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00008194 File Offset: 0x00007194
		public int IndexOfAny(char[] anyOf, int startIndex)
		{
			return this.IndexOfAny(anyOf, startIndex, this.Length - startIndex);
		}

		// Token: 0x06000194 RID: 404
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int IndexOfAny(char[] anyOf, int startIndex, int count);

		// Token: 0x06000195 RID: 405 RVA: 0x000081A6 File Offset: 0x000071A6
		public int IndexOf(string value)
		{
			return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000081B9 File Offset: 0x000071B9
		public int IndexOf(string value, int startIndex)
		{
			return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000081D0 File Offset: 0x000071D0
		public int IndexOf(string value, int startIndex, int count)
		{
			if (startIndex < 0 || startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count < 0 || count > this.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.None);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00008237 File Offset: 0x00007237
		public int IndexOf(string value, StringComparison comparisonType)
		{
			return this.IndexOf(value, 0, this.Length, comparisonType);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008248 File Offset: 0x00007248
		public int IndexOf(string value, int startIndex, StringComparison comparisonType)
		{
			return this.IndexOf(value, startIndex, this.Length - startIndex, comparisonType);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000825C File Offset: 0x0000725C
		public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex < 0 || startIndex > this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count < 0 || startIndex > this.Length - count)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CultureInfo.InvariantCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return CultureInfo.InvariantCulture.CompareInfo.IndexOf(this, value, startIndex, count, CompareOptions.Ordinal);
			case StringComparison.OrdinalIgnoreCase:
				return TextInfo.IndexOfStringOrdinalIgnoreCase(this, value, startIndex, count);
			default:
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000836B File Offset: 0x0000736B
		public int LastIndexOf(char value)
		{
			return this.LastIndexOf(value, this.Length - 1, this.Length);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008382 File Offset: 0x00007382
		public int LastIndexOf(char value, int startIndex)
		{
			return this.LastIndexOf(value, startIndex, startIndex + 1);
		}

		// Token: 0x0600019D RID: 413
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int LastIndexOf(char value, int startIndex, int count);

		// Token: 0x0600019E RID: 414 RVA: 0x0000838F File Offset: 0x0000738F
		public int LastIndexOfAny(char[] anyOf)
		{
			return this.LastIndexOfAny(anyOf, this.Length - 1, this.Length);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000083A6 File Offset: 0x000073A6
		public int LastIndexOfAny(char[] anyOf, int startIndex)
		{
			return this.LastIndexOfAny(anyOf, startIndex, startIndex + 1);
		}

		// Token: 0x060001A0 RID: 416
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int LastIndexOfAny(char[] anyOf, int startIndex, int count);

		// Token: 0x060001A1 RID: 417 RVA: 0x000083B3 File Offset: 0x000073B3
		public int LastIndexOf(string value)
		{
			return this.LastIndexOf(value, this.Length - 1, this.Length, StringComparison.CurrentCulture);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000083CB File Offset: 0x000073CB
		public int LastIndexOf(string value, int startIndex)
		{
			return this.LastIndexOf(value, startIndex, startIndex + 1, StringComparison.CurrentCulture);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000083D9 File Offset: 0x000073D9
		public int LastIndexOf(string value, int startIndex, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.None);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008408 File Offset: 0x00007408
		public int LastIndexOf(string value, StringComparison comparisonType)
		{
			return this.LastIndexOf(value, this.Length - 1, this.Length, comparisonType);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00008420 File Offset: 0x00007420
		public int LastIndexOf(string value, int startIndex, StringComparison comparisonType)
		{
			return this.LastIndexOf(value, startIndex, startIndex + 1, comparisonType);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008430 File Offset: 0x00007430
		public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.Length == 0 && (startIndex == -1 || startIndex == 0))
			{
				if (value.Length != 0)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (startIndex < 0 || startIndex > this.Length)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (startIndex == this.Length)
				{
					startIndex--;
					if (count > 0)
					{
						count--;
					}
					if (value.Length == 0 && count >= 0 && startIndex - count + 1 >= 0)
					{
						return startIndex;
					}
				}
				if (count < 0 || startIndex - count + 1 < 0)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
				}
				switch (comparisonType)
				{
				case StringComparison.CurrentCulture:
					return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.None);
				case StringComparison.CurrentCultureIgnoreCase:
					return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
				case StringComparison.InvariantCulture:
					return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.None);
				case StringComparison.InvariantCultureIgnoreCase:
					return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.IgnoreCase);
				case StringComparison.Ordinal:
					return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(this, value, startIndex, count, CompareOptions.Ordinal);
				case StringComparison.OrdinalIgnoreCase:
					return TextInfo.LastIndexOfStringOrdinalIgnoreCase(this, value, startIndex, count);
				default:
					throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
				}
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008584 File Offset: 0x00007584
		public string PadLeft(int totalWidth)
		{
			return this.PadHelper(totalWidth, ' ', false);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008590 File Offset: 0x00007590
		public string PadLeft(int totalWidth, char paddingChar)
		{
			return this.PadHelper(totalWidth, paddingChar, false);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000859B File Offset: 0x0000759B
		public string PadRight(int totalWidth)
		{
			return this.PadHelper(totalWidth, ' ', true);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000085A7 File Offset: 0x000075A7
		public string PadRight(int totalWidth, char paddingChar)
		{
			return this.PadHelper(totalWidth, paddingChar, true);
		}

		// Token: 0x060001AB RID: 427
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string PadHelper(int totalWidth, char paddingChar, bool isRightPadded);

		// Token: 0x060001AC RID: 428 RVA: 0x000085B2 File Offset: 0x000075B2
		public bool StartsWith(string value)
		{
			return this.StartsWith(value, false, null);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000085C0 File Offset: 0x000075C0
		[ComVisible(false)]
		public bool StartsWith(string value, StringComparison comparisonType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (comparisonType < StringComparison.CurrentCulture || comparisonType > StringComparison.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
			if (this == value)
			{
				return true;
			}
			if (value.Length == 0)
			{
				return true;
			}
			switch (comparisonType)
			{
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CultureInfo.InvariantCulture.CompareInfo.IsPrefix(this, value, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.IsPrefix(this, value, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return this.Length >= value.Length && string.nativeCompareOrdinalEx(this, 0, value, 0, value.Length) == 0;
			case StringComparison.OrdinalIgnoreCase:
				return this.Length >= value.Length && TextInfo.CompareOrdinalIgnoreCaseEx(this, 0, value, 0, value.Length) == 0;
			default:
				throw new ArgumentException(Environment.GetResourceString("NotSupported_StringComparison"), "comparisonType");
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000086D4 File Offset: 0x000076D4
		public bool StartsWith(string value, bool ignoreCase, CultureInfo culture)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this == value)
			{
				return true;
			}
			CultureInfo cultureInfo = ((culture == null) ? CultureInfo.CurrentCulture : culture);
			return cultureInfo.CompareInfo.IsPrefix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008715 File Offset: 0x00007715
		public string ToLower()
		{
			return this.ToLower(CultureInfo.CurrentCulture);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00008722 File Offset: 0x00007722
		public string ToLower(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.TextInfo.ToLower(this);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000873E File Offset: 0x0000773E
		public string ToLowerInvariant()
		{
			return this.ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000874B File Offset: 0x0000774B
		public string ToUpper()
		{
			return this.ToUpper(CultureInfo.CurrentCulture);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00008758 File Offset: 0x00007758
		public string ToUpper(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return culture.TextInfo.ToUpper(this);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008774 File Offset: 0x00007774
		public string ToUpperInvariant()
		{
			return this.ToUpper(CultureInfo.InvariantCulture);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00008781 File Offset: 0x00007781
		public override string ToString()
		{
			return this;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00008784 File Offset: 0x00007784
		public string ToString(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008787 File Offset: 0x00007787
		public object Clone()
		{
			return this;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000878A File Offset: 0x0000778A
		public string Trim()
		{
			return this.TrimHelper(string.WhitespaceChars, 2);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008798 File Offset: 0x00007798
		private string TrimHelper(char[] trimChars, int trimType)
		{
			int i = this.Length - 1;
			int j = 0;
			if (trimType != 1)
			{
				for (j = 0; j < this.Length; j++)
				{
					char c = this[j];
					int num = 0;
					while (num < trimChars.Length && trimChars[num] != c)
					{
						num++;
					}
					if (num == trimChars.Length)
					{
						break;
					}
				}
			}
			if (trimType != 0)
			{
				for (i = this.Length - 1; i >= j; i--)
				{
					char c2 = this[i];
					int num2 = 0;
					while (num2 < trimChars.Length && trimChars[num2] != c2)
					{
						num2++;
					}
					if (num2 == trimChars.Length)
					{
						break;
					}
				}
			}
			int num3 = i - j + 1;
			if (num3 == this.Length)
			{
				return this;
			}
			if (num3 == 0)
			{
				return string.Empty;
			}
			return this.InternalSubString(j, num3, false);
		}

		// Token: 0x060001BA RID: 442
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string Insert(int startIndex, string value);

		// Token: 0x060001BB RID: 443
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string Replace(char oldChar, char newChar);

		// Token: 0x060001BC RID: 444
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string Replace(string oldValue, string newValue);

		// Token: 0x060001BD RID: 445
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string Remove(int startIndex, int count);

		// Token: 0x060001BE RID: 446 RVA: 0x00008854 File Offset: 0x00007854
		public string Remove(int startIndex)
		{
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndex"));
			}
			if (startIndex >= this.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_StartIndexLessThanLength"));
			}
			return this.Substring(0, startIndex);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000088A0 File Offset: 0x000078A0
		public static string Format(string format, object arg0)
		{
			return string.Format(null, format, new object[] { arg0 });
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000088C0 File Offset: 0x000078C0
		public static string Format(string format, object arg0, object arg1)
		{
			return string.Format(null, format, new object[] { arg0, arg1 });
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000088E4 File Offset: 0x000078E4
		public static string Format(string format, object arg0, object arg1, object arg2)
		{
			return string.Format(null, format, new object[] { arg0, arg1, arg2 });
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000890C File Offset: 0x0000790C
		public static string Format(string format, params object[] args)
		{
			return string.Format(null, format, args);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008918 File Offset: 0x00007918
		public static string Format(IFormatProvider provider, string format, params object[] args)
		{
			if (format == null || args == null)
			{
				throw new ArgumentNullException((format == null) ? "format" : "args");
			}
			StringBuilder stringBuilder = new StringBuilder(format.Length + args.Length * 8);
			stringBuilder.AppendFormat(provider, format, args);
			return stringBuilder.ToString();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008964 File Offset: 0x00007964
		public unsafe static string Copy(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			int length = str.Length;
			string text = string.FastAllocateString(length);
			fixed (char* ptr = &text.m_firstChar)
			{
				fixed (char* ptr2 = &str.m_firstChar)
				{
					string.wstrcpyPtrAligned(ptr, ptr2, length);
				}
			}
			return text;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000089AC File Offset: 0x000079AC
		internal unsafe static string InternalCopy(string str)
		{
			int length = str.Length;
			string text = string.FastAllocateString(length);
			fixed (char* ptr = &text.m_firstChar)
			{
				fixed (char* ptr2 = &str.m_firstChar)
				{
					string.wstrcpyPtrAligned(ptr, ptr2, length);
				}
			}
			return text;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000089E6 File Offset: 0x000079E6
		public static string Concat(object arg0)
		{
			if (arg0 == null)
			{
				return string.Empty;
			}
			return arg0.ToString();
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000089F7 File Offset: 0x000079F7
		public static string Concat(object arg0, object arg1)
		{
			if (arg0 == null)
			{
				arg0 = string.Empty;
			}
			if (arg1 == null)
			{
				arg1 = string.Empty;
			}
			return arg0.ToString() + arg1.ToString();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00008A1E File Offset: 0x00007A1E
		public static string Concat(object arg0, object arg1, object arg2)
		{
			if (arg0 == null)
			{
				arg0 = string.Empty;
			}
			if (arg1 == null)
			{
				arg1 = string.Empty;
			}
			if (arg2 == null)
			{
				arg2 = string.Empty;
			}
			return arg0.ToString() + arg1.ToString() + arg2.ToString();
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008A58 File Offset: 0x00007A58
		[CLSCompliant(false)]
		public static string Concat(object arg0, object arg1, object arg2, object arg3, __arglist)
		{
			ArgIterator argIterator = new ArgIterator(__arglist);
			int num = argIterator.GetRemainingCount() + 4;
			object[] array = new object[num];
			array[0] = arg0;
			array[1] = arg1;
			array[2] = arg2;
			array[3] = arg3;
			for (int i = 4; i < num; i++)
			{
				array[i] = TypedReference.ToObject(argIterator.GetNextArg());
			}
			return string.Concat(array);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008AB0 File Offset: 0x00007AB0
		public static string Concat(params object[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			string[] array = new string[args.Length];
			int num = 0;
			for (int i = 0; i < args.Length; i++)
			{
				object obj = args[i];
				array[i] = ((obj == null) ? string.Empty : obj.ToString());
				num += array[i].Length;
				if (num < 0)
				{
					throw new OutOfMemoryException();
				}
			}
			return string.ConcatArray(array, num);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008B18 File Offset: 0x00007B18
		public static string Concat(string str0, string str1)
		{
			if (string.IsNullOrEmpty(str0))
			{
				if (string.IsNullOrEmpty(str1))
				{
					return string.Empty;
				}
				return str1;
			}
			else
			{
				if (string.IsNullOrEmpty(str1))
				{
					return str0;
				}
				int length = str0.Length;
				string text = string.FastAllocateString(length + str1.Length);
				string.FillStringChecked(text, 0, str0);
				string.FillStringChecked(text, length, str1);
				return text;
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008B70 File Offset: 0x00007B70
		public static string Concat(string str0, string str1, string str2)
		{
			if (str0 == null && str1 == null && str2 == null)
			{
				return string.Empty;
			}
			if (str0 == null)
			{
				str0 = string.Empty;
			}
			if (str1 == null)
			{
				str1 = string.Empty;
			}
			if (str2 == null)
			{
				str2 = string.Empty;
			}
			int num = str0.Length + str1.Length + str2.Length;
			string text = string.FastAllocateString(num);
			string.FillStringChecked(text, 0, str0);
			string.FillStringChecked(text, str0.Length, str1);
			string.FillStringChecked(text, str0.Length + str1.Length, str2);
			return text;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00008BF0 File Offset: 0x00007BF0
		public static string Concat(string str0, string str1, string str2, string str3)
		{
			if (str0 == null && str1 == null && str2 == null && str3 == null)
			{
				return string.Empty;
			}
			if (str0 == null)
			{
				str0 = string.Empty;
			}
			if (str1 == null)
			{
				str1 = string.Empty;
			}
			if (str2 == null)
			{
				str2 = string.Empty;
			}
			if (str3 == null)
			{
				str3 = string.Empty;
			}
			int num = str0.Length + str1.Length + str2.Length + str3.Length;
			string text = string.FastAllocateString(num);
			string.FillStringChecked(text, 0, str0);
			string.FillStringChecked(text, str0.Length, str1);
			string.FillStringChecked(text, str0.Length + str1.Length, str2);
			string.FillStringChecked(text, str0.Length + str1.Length + str2.Length, str3);
			return text;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00008CA0 File Offset: 0x00007CA0
		private static string ConcatArray(string[] values, int totalLength)
		{
			string text = string.FastAllocateString(totalLength);
			int num = 0;
			for (int i = 0; i < values.Length; i++)
			{
				string.FillStringChecked(text, num, values[i]);
				num += values[i].Length;
			}
			return text;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00008CDC File Offset: 0x00007CDC
		public static string Concat(params string[] values)
		{
			int num = 0;
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			string[] array = new string[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				string text = values[i];
				array[i] = ((text == null) ? string.Empty : text);
				num += array[i].Length;
				if (num < 0)
				{
					throw new OutOfMemoryException();
				}
			}
			return string.ConcatArray(array, num);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00008D3E File Offset: 0x00007D3E
		public static string Intern(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return Thread.GetDomain().GetOrInternString(str);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008D59 File Offset: 0x00007D59
		public static string IsInterned(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return Thread.GetDomain().IsStringInterned(str);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00008D74 File Offset: 0x00007D74
		public TypeCode GetTypeCode()
		{
			return TypeCode.String;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008D78 File Offset: 0x00007D78
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this, provider);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00008D81 File Offset: 0x00007D81
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this, provider);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00008D8A File Offset: 0x00007D8A
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this, provider);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008D93 File Offset: 0x00007D93
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this, provider);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00008D9C File Offset: 0x00007D9C
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this, provider);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00008DA5 File Offset: 0x00007DA5
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this, provider);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00008DAE File Offset: 0x00007DAE
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this, provider);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008DB7 File Offset: 0x00007DB7
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this, provider);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00008DC0 File Offset: 0x00007DC0
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this, provider);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00008DC9 File Offset: 0x00007DC9
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this, provider);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008DD2 File Offset: 0x00007DD2
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this, provider);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00008DDB File Offset: 0x00007DDB
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this, provider);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008DE4 File Offset: 0x00007DE4
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this, provider);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008DED File Offset: 0x00007DED
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return Convert.ToDateTime(this, provider);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008DF6 File Offset: 0x00007DF6
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x060001E2 RID: 482
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsFastSort();

		// Token: 0x060001E3 RID: 483
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsAscii();

		// Token: 0x060001E4 RID: 484 RVA: 0x00008E00 File Offset: 0x00007E00
		internal unsafe void SetChar(int index, char value)
		{
			if (index >= this.Length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			fixed (char* ptr = &this.m_firstChar)
			{
				ptr[index] = value;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008E40 File Offset: 0x00007E40
		internal unsafe void AppendInPlace(char value, int currentLength)
		{
			fixed (char* ptr = &this.m_firstChar)
			{
				ptr[currentLength] = value;
				currentLength++;
				ptr[currentLength] = '\0';
				this.m_stringLength = currentLength;
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00008E78 File Offset: 0x00007E78
		internal unsafe void AppendInPlace(char value, int repeatCount, int currentLength)
		{
			int num = currentLength + repeatCount;
			fixed (char* ptr = &this.m_firstChar)
			{
				int i;
				for (i = currentLength; i < num; i++)
				{
					ptr[i] = value;
				}
				ptr[i] = '\0';
			}
			this.m_stringLength = num;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00008EB8 File Offset: 0x00007EB8
		internal unsafe void AppendInPlace(string value, int currentLength)
		{
			int length = value.Length;
			int num = currentLength + length;
			fixed (char* ptr = &this.m_firstChar)
			{
				fixed (char* ptr2 = &value.m_firstChar)
				{
					string.wstrcpy(ptr + currentLength, ptr2, length);
				}
				ptr[num] = '\0';
			}
			this.m_stringLength = num;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008F04 File Offset: 0x00007F04
		internal unsafe void AppendInPlace(string value, int startIndex, int count, int currentLength)
		{
			int num = currentLength + count;
			fixed (char* ptr = &this.m_firstChar)
			{
				fixed (char* ptr2 = &value.m_firstChar)
				{
					string.wstrcpy(ptr + currentLength, ptr2 + startIndex, count);
				}
				ptr[num] = '\0';
			}
			this.m_stringLength = num;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008F50 File Offset: 0x00007F50
		internal unsafe void AppendInPlace(char* value, int count, int currentLength)
		{
			int num = currentLength + count;
			fixed (char* ptr = &this.m_firstChar)
			{
				string.wstrcpy(ptr + currentLength, value, count);
				ptr[num] = '\0';
			}
			this.m_stringLength = num;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008F8C File Offset: 0x00007F8C
		internal unsafe void AppendInPlace(char[] value, int start, int count, int currentLength)
		{
			int num = currentLength + count;
			fixed (char* ptr = &this.m_firstChar)
			{
				if (count > 0)
				{
					fixed (char* ptr2 = value)
					{
						string.wstrcpy(ptr + currentLength, ptr2 + start, count);
					}
				}
				ptr[num] = '\0';
			}
			this.m_stringLength = num;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00008FEC File Offset: 0x00007FEC
		internal unsafe void ReplaceCharInPlace(char oldChar, char newChar, int startIndex, int count, int currentLength)
		{
			int num = startIndex + count;
			fixed (char* ptr = &this.m_firstChar)
			{
				for (int i = startIndex; i < num; i++)
				{
					if (ptr[i] == oldChar)
					{
						ptr[i] = newChar;
					}
				}
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00009028 File Offset: 0x00008028
		internal static string GetStringForStringBuilder(string value, int capacity)
		{
			return string.GetStringForStringBuilder(value, 0, value.Length, capacity);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00009038 File Offset: 0x00008038
		internal unsafe static string GetStringForStringBuilder(string value, int startIndex, int length, int capacity)
		{
			string text = string.FastAllocateString(capacity);
			if (value.Length == 0)
			{
				text.SetLength(0);
				return text;
			}
			fixed (char* ptr = &text.m_firstChar)
			{
				fixed (char* ptr2 = &value.m_firstChar)
				{
					string.wstrcpy(ptr, ptr2 + startIndex, length);
				}
			}
			text.SetLength(length);
			return text;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009088 File Offset: 0x00008088
		private unsafe void NullTerminate()
		{
			fixed (char* ptr = &this.m_firstChar)
			{
				ptr[this.m_stringLength] = '\0';
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000090B0 File Offset: 0x000080B0
		internal unsafe void ClearPostNullChar()
		{
			int num = this.Length + 1;
			if (num < this.m_arrayLength)
			{
				fixed (char* ptr = &this.m_firstChar)
				{
					ptr[num] = '\0';
				}
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000090E2 File Offset: 0x000080E2
		internal void SetLength(int newLength)
		{
			this.m_stringLength = newLength;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000090EB File Offset: 0x000080EB
		public CharEnumerator GetEnumerator()
		{
			return new CharEnumerator(this);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000090F3 File Offset: 0x000080F3
		IEnumerator<char> IEnumerable<char>.GetEnumerator()
		{
			return new CharEnumerator(this);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000090FB File Offset: 0x000080FB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CharEnumerator(this);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00009104 File Offset: 0x00008104
		internal unsafe void InternalSetCharNoBoundsCheck(int index, char value)
		{
			fixed (char* ptr = &this.m_firstChar)
			{
				ptr[index] = value;
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00009124 File Offset: 0x00008124
		internal unsafe static void InternalCopy(string src, IntPtr dest, int len)
		{
			if (len == 0)
			{
				return;
			}
			fixed (char* ptr = &src.m_firstChar)
			{
				byte* ptr2 = (byte*)ptr;
				byte* ptr3 = (byte*)dest.ToPointer();
				Buffer.memcpyimpl(ptr2, ptr3, len);
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00009154 File Offset: 0x00008154
		internal unsafe static void InternalMemCpy(string src, int srcOffset, string dst, int destOffset, int len)
		{
			if (len == 0)
			{
				return;
			}
			fixed (char* ptr = &src.m_firstChar)
			{
				fixed (char* ptr2 = &dst.m_firstChar)
				{
					Buffer.memcpyimpl((byte*)ptr + (IntPtr)srcOffset * 2, (byte*)ptr2 + (IntPtr)destOffset * 2, len);
				}
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00009190 File Offset: 0x00008190
		internal unsafe static void revmemcpyimpl(byte* src, byte* dest, int len)
		{
			if (len == 0)
			{
				return;
			}
			dest += len;
			src += len;
			if ((src & 3L) != null)
			{
				do
				{
					dest--;
					src--;
					len--;
					*dest = *src;
				}
				while (len > 0 && (src & 3L) != null);
			}
			if (len >= 16)
			{
				len -= 16;
				do
				{
					dest = (dest - 16) / 1;
					src = (src - 16) / 1;
					*(int*)(dest + 12) = *(int*)(src + 12);
					*(int*)(dest + 8) = *(int*)(src + 8);
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(int*)dest = *(int*)src;
				}
				while ((len -= 16) >= 0);
			}
			if ((len & 8) > 0)
			{
				dest = (dest - 8) / 1;
				src = (src - 8) / 1;
				*(int*)(dest + 4) = *(int*)(src + 4);
				*(int*)dest = *(int*)src;
			}
			if ((len & 4) > 0)
			{
				dest = (dest - 4) / 1;
				src = (src - 4) / 1;
				*(int*)dest = *(int*)src;
			}
			if ((len & 2) != 0)
			{
				dest = (dest - 2) / 1;
				src = (src - 2) / 1;
				*(short*)dest = *(short*)src;
			}
			if ((len & 1) != 0)
			{
				dest--;
				src--;
				*dest = *src;
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000092A0 File Offset: 0x000082A0
		internal unsafe void InsertInPlace(int index, string value, int repeatCount, int currentLength, int requiredLength)
		{
			fixed (char* ptr = &this.m_firstChar)
			{
				fixed (char* ptr2 = &value.m_firstChar)
				{
					string.revmemcpyimpl((byte*)ptr + (IntPtr)index * 2, (byte*)ptr + (IntPtr)index * 2 + (IntPtr)(value.Length * repeatCount) * 2, (currentLength - index) * 2);
					for (int i = 0; i < repeatCount; i++)
					{
						Buffer.memcpyimpl((byte*)ptr2, (byte*)ptr + (IntPtr)index * 2 + (IntPtr)(i * value.Length) * 2, value.Length * 2);
					}
				}
			}
			this.SetLength(requiredLength);
			this.NullTerminate();
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00009324 File Offset: 0x00008324
		internal unsafe void InsertInPlace(int index, char[] value, int startIndex, int charCount, int currentLength, int requiredLength)
		{
			fixed (char* ptr = &this.m_firstChar)
			{
				fixed (char* ptr2 = value)
				{
					string.revmemcpyimpl((byte*)ptr + (IntPtr)index * 2, (byte*)ptr + (IntPtr)index * 2 + (IntPtr)charCount * 2, (currentLength - index) * 2);
					Buffer.memcpyimpl((byte*)ptr2 + (IntPtr)startIndex * 2, (byte*)ptr + (IntPtr)index * 2, charCount * 2);
				}
			}
			this.SetLength(requiredLength);
			this.NullTerminate();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000939C File Offset: 0x0000839C
		internal void RemoveInPlace(int index, int charCount, int currentLength)
		{
			string.InternalMemCpy(this, index + charCount, this, index, (currentLength - charCount - index) * 2);
			int num = currentLength - charCount;
			this.SetLength(num);
			this.NullTerminate();
		}

		// Token: 0x04000091 RID: 145
		private const int TrimHead = 0;

		// Token: 0x04000092 RID: 146
		private const int TrimTail = 1;

		// Token: 0x04000093 RID: 147
		private const int TrimBoth = 2;

		// Token: 0x04000094 RID: 148
		private const int charPtrAlignConst = 1;

		// Token: 0x04000095 RID: 149
		private const int alignConst = 3;

		// Token: 0x04000096 RID: 150
		[NonSerialized]
		private int m_arrayLength;

		// Token: 0x04000097 RID: 151
		[NonSerialized]
		private int m_stringLength;

		// Token: 0x04000098 RID: 152
		[NonSerialized]
		private char m_firstChar;

		// Token: 0x04000099 RID: 153
		public static readonly string Empty = "";

		// Token: 0x0400009A RID: 154
		internal static readonly char[] WhitespaceChars = new char[]
		{
			'\t', '\n', '\v', '\f', '\r', ' ', '\u0085', '\u00a0', '\u1680', '\u2000',
			'\u2001', '\u2002', '\u2003', '\u2004', '\u2005', '\u2006', '\u2007', '\u2008', '\u2009', '\u200a',
			'\u200b', '\u2028', '\u2029', '\u3000', '\ufeff'
		};
	}
}
