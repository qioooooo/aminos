using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x0200037D RID: 893
	[ComVisible(true)]
	[Serializable]
	public class CompareInfo : IDeserializationCallback
	{
		// Token: 0x06002395 RID: 9109
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern byte[] nativeCreateSortKey(void* pSortingFile, string pString, int dwFlags, int win32LCID);

		// Token: 0x06002396 RID: 9110
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int nativeGetGlobalizedHashCode(void* pSortingFile, string pString, int dwFlags, int win32LCID);

		// Token: 0x06002397 RID: 9111
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern bool nativeIsSortable(void* pSortingFile, string pString);

		// Token: 0x06002398 RID: 9112
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeCompareString(int lcid, string string1, int offset1, int length1, string string2, int offset2, int length2, int flags);

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06002399 RID: 9113 RVA: 0x0005B9C4 File Offset: 0x0005A9C4
		private static object InternalSyncObject
		{
			get
			{
				if (CompareInfo.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref CompareInfo.s_InternalSyncObject, obj, null);
				}
				return CompareInfo.s_InternalSyncObject;
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0005B9F0 File Offset: 0x0005A9F0
		public static CompareInfo GetCompareInfo(int culture, Assembly assembly)
		{
			return CompareInfo.GetCompareInfoWithPrefixedLcid(culture, assembly, 0);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0005B9FC File Offset: 0x0005A9FC
		private static CompareInfo GetCompareInfoWithPrefixedLcid(int cultureKey, Assembly assembly, int prefix)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			int num = cultureKey & ~prefix;
			if (CultureTableRecord.IsCustomCultureId(num))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CustomCultureCannotBePassedByNumber", new object[] { "culture" }));
			}
			if (assembly != typeof(object).Module.Assembly)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_OnlyMscorlib"));
			}
			GlobalizationAssembly globalizationAssembly = GlobalizationAssembly.GetGlobalizationAssembly(assembly);
			object obj = globalizationAssembly.compareInfoCache[cultureKey];
			if (obj == null)
			{
				lock (CompareInfo.InternalSyncObject)
				{
					if ((obj = globalizationAssembly.compareInfoCache[cultureKey]) == null)
					{
						obj = new CompareInfo(globalizationAssembly, num);
						Thread.MemoryBarrier();
						globalizationAssembly.compareInfoCache[cultureKey] = obj;
					}
				}
			}
			return (CompareInfo)obj;
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0005BAE8 File Offset: 0x0005AAE8
		private static CompareInfo GetCompareInfoByName(string name, Assembly assembly)
		{
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo(name);
			if (cultureInfo.IsNeutralCulture && !CultureTableRecord.IsCustomCultureId(cultureInfo.cultureID))
			{
				if (cultureInfo.cultureID == 31748)
				{
					cultureInfo = CultureInfo.GetCultureInfo(3076);
				}
				else
				{
					cultureInfo = CultureInfo.GetCultureInfo(cultureInfo.CompareInfoId);
				}
			}
			int num = cultureInfo.CompareInfoId;
			if (cultureInfo.Name == "zh-CHS" || cultureInfo.Name == "zh-CHT")
			{
				num |= int.MinValue;
			}
			CompareInfo compareInfo;
			if (assembly != null)
			{
				compareInfo = CompareInfo.GetCompareInfoWithPrefixedLcid(num, assembly, int.MinValue);
			}
			else
			{
				compareInfo = CompareInfo.GetCompareInfoWithPrefixedLcid(num, int.MinValue);
			}
			compareInfo.m_name = cultureInfo.SortName;
			return compareInfo;
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0005BB98 File Offset: 0x0005AB98
		public static CompareInfo GetCompareInfo(string name, Assembly assembly)
		{
			if (name == null || assembly == null)
			{
				throw new ArgumentNullException((name == null) ? "name" : "assembly");
			}
			if (assembly != typeof(object).Module.Assembly)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_OnlyMscorlib"));
			}
			return CompareInfo.GetCompareInfoByName(name, assembly);
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x0005BBEE File Offset: 0x0005ABEE
		public static CompareInfo GetCompareInfo(int culture)
		{
			return CompareInfo.GetCompareInfoWithPrefixedLcid(culture, 0);
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x0005BBF8 File Offset: 0x0005ABF8
		internal static CompareInfo GetCompareInfoWithPrefixedLcid(int cultureKey, int prefix)
		{
			int num = cultureKey & ~prefix;
			if (CultureTableRecord.IsCustomCultureId(num))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CustomCultureCannotBePassedByNumber", new object[] { "culture" }));
			}
			object obj = GlobalizationAssembly.DefaultInstance.compareInfoCache[cultureKey];
			if (obj == null)
			{
				lock (CompareInfo.InternalSyncObject)
				{
					if ((obj = GlobalizationAssembly.DefaultInstance.compareInfoCache[cultureKey]) == null)
					{
						obj = new CompareInfo(GlobalizationAssembly.DefaultInstance, num);
						Thread.MemoryBarrier();
						GlobalizationAssembly.DefaultInstance.compareInfoCache[cultureKey] = obj;
					}
				}
			}
			return (CompareInfo)obj;
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x0005BCB8 File Offset: 0x0005ACB8
		public static CompareInfo GetCompareInfo(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return CompareInfo.GetCompareInfoByName(name, null);
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x0005BCCF File Offset: 0x0005ACCF
		[ComVisible(false)]
		public static bool IsSortable(char ch)
		{
			return CompareInfo.IsSortable(ch.ToString());
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x0005BCDD File Offset: 0x0005ACDD
		[ComVisible(false)]
		public static bool IsSortable(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			return text.Length != 0 && CompareInfo.nativeIsSortable(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, text);
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x0005BD0C File Offset: 0x0005AD0C
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.culture = -1;
			this.m_sortingLCID = -1;
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x0005BD1C File Offset: 0x0005AD1C
		private void OnDeserialized()
		{
			if (this.m_sortingLCID <= 0)
			{
				this.m_sortingLCID = this.GetSortingLCID(this.culture);
			}
			if (this.m_pSortingTable == null && !this.IsSynthetic)
			{
				this.m_pSortingTable = CompareInfo.InitializeCompareInfo(GlobalizationAssembly.DefaultInstance.pNativeGlobalizationAssembly, this.m_sortingLCID);
			}
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0005BD71 File Offset: 0x0005AD71
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.OnDeserialized();
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0005BD79 File Offset: 0x0005AD79
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.win32LCID = this.m_sortingLCID;
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060023A7 RID: 9127 RVA: 0x0005BD87 File Offset: 0x0005AD87
		[ComVisible(false)]
		public virtual string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = CultureInfo.GetCultureInfo(this.culture).SortName;
				}
				return this.m_name;
			}
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0005BDAD File Offset: 0x0005ADAD
		internal void SetName(string name)
		{
			this.m_name = name;
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x0005BDB8 File Offset: 0x0005ADB8
		internal static void ClearDefaultAssemblyCache()
		{
			lock (CompareInfo.InternalSyncObject)
			{
				GlobalizationAssembly.DefaultInstance.compareInfoCache = new Hashtable(4);
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060023AA RID: 9130 RVA: 0x0005BDFC File Offset: 0x0005ADFC
		internal CultureTableRecord CultureTableRecord
		{
			get
			{
				if (this.m_cultureTableRecord == null)
				{
					this.m_cultureTableRecord = CultureInfo.GetCultureInfo(this.m_sortingLCID).m_cultureTableRecord;
				}
				return this.m_cultureTableRecord;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060023AB RID: 9131 RVA: 0x0005BE22 File Offset: 0x0005AE22
		private bool IsSynthetic
		{
			get
			{
				return this.CultureTableRecord.IsSynthetic;
			}
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x0005BE30 File Offset: 0x0005AE30
		internal CompareInfo(GlobalizationAssembly ga, int culture)
		{
			if (culture < 0)
			{
				throw new ArgumentOutOfRangeException("culture", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			this.m_sortingLCID = this.GetSortingLCID(culture);
			if (!this.IsSynthetic)
			{
				this.m_pSortingTable = CompareInfo.InitializeCompareInfo(GlobalizationAssembly.DefaultInstance.pNativeGlobalizationAssembly, this.m_sortingLCID);
			}
			this.culture = culture;
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x0005BE94 File Offset: 0x0005AE94
		internal int GetSortingLCID(int culture)
		{
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo(culture);
			if (cultureInfo.m_cultureTableRecord.IsSynthetic)
			{
				return culture;
			}
			int num = cultureInfo.CompareInfoId;
			int sortID = CultureInfo.GetSortID(culture);
			if (sortID != 0)
			{
				if (!cultureInfo.m_cultureTableRecord.IsValidSortID(sortID))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_CultureNotSupported"), new object[] { culture }), "culture");
				}
				num |= sortID << 16;
			}
			return num;
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x0005BF10 File Offset: 0x0005AF10
		internal static int GetNativeCompareFlags(CompareOptions options)
		{
			int num = 0;
			if ((options & CompareOptions.IgnoreCase) != CompareOptions.None)
			{
				num |= 1;
			}
			if ((options & CompareOptions.IgnoreKanaType) != CompareOptions.None)
			{
				num |= 65536;
			}
			if ((options & CompareOptions.IgnoreNonSpace) != CompareOptions.None)
			{
				num |= 2;
			}
			if ((options & CompareOptions.IgnoreSymbols) != CompareOptions.None)
			{
				num |= 4;
			}
			if ((options & CompareOptions.IgnoreWidth) != CompareOptions.None)
			{
				num |= 131072;
			}
			if ((options & CompareOptions.StringSort) != CompareOptions.None)
			{
				num |= 4096;
			}
			return num;
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x0005BF67 File Offset: 0x0005AF67
		public virtual int Compare(string string1, string string2)
		{
			return this.Compare(string1, string2, CompareOptions.None);
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x0005BF74 File Offset: 0x0005AF74
		public virtual int Compare(string string1, string string2, CompareOptions options)
		{
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return string.Compare(string1, string2, StringComparison.OrdinalIgnoreCase);
			}
			if ((options & CompareOptions.Ordinal) != CompareOptions.None)
			{
				if (options != CompareOptions.Ordinal)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_CompareOptionOrdinal"), "options");
				}
				if (string1 == null)
				{
					if (string2 == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (string2 == null)
					{
						return 1;
					}
					return string.nativeCompareOrdinal(string1, string2, false);
				}
			}
			else
			{
				if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort)) != CompareOptions.None)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
				}
				if (string1 == null)
				{
					if (string2 == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (string2 == null)
					{
						return 1;
					}
					if (!this.IsSynthetic)
					{
						return CompareInfo.Compare(this.m_pSortingTable, this.m_sortingLCID, string1, string2, options);
					}
					if (options == CompareOptions.Ordinal)
					{
						return CompareInfo.Compare(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, string1, string2, options);
					}
					return CompareInfo.nativeCompareString(this.m_sortingLCID, string1, 0, string1.Length, string2, 0, string2.Length, CompareInfo.GetNativeCompareFlags(options));
				}
			}
		}

		// Token: 0x060023B1 RID: 9137
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int Compare(void* pSortingTable, int sortingLCID, string string1, string string2, CompareOptions options);

		// Token: 0x060023B2 RID: 9138 RVA: 0x0005C061 File Offset: 0x0005B061
		public virtual int Compare(string string1, int offset1, int length1, string string2, int offset2, int length2)
		{
			return this.Compare(string1, offset1, length1, string2, offset2, length2, CompareOptions.None);
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x0005C073 File Offset: 0x0005B073
		public virtual int Compare(string string1, int offset1, string string2, int offset2, CompareOptions options)
		{
			return this.Compare(string1, offset1, (string1 == null) ? 0 : (string1.Length - offset1), string2, offset2, (string2 == null) ? 0 : (string2.Length - offset2), options);
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x0005C09F File Offset: 0x0005B09F
		public virtual int Compare(string string1, int offset1, string string2, int offset2)
		{
			return this.Compare(string1, offset1, string2, offset2, CompareOptions.None);
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x0005C0B0 File Offset: 0x0005B0B0
		public virtual int Compare(string string1, int offset1, int length1, string string2, int offset2, int length2, CompareOptions options)
		{
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				int num = string.Compare(string1, offset1, string2, offset2, (length1 < length2) ? length1 : length2, StringComparison.OrdinalIgnoreCase);
				if (length1 == length2 || num != 0)
				{
					return num;
				}
				if (length1 <= length2)
				{
					return -1;
				}
				return 1;
			}
			else
			{
				if (length1 < 0 || length2 < 0)
				{
					throw new ArgumentOutOfRangeException((length1 < 0) ? "length1" : "length2", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
				}
				if (offset1 < 0 || offset2 < 0)
				{
					throw new ArgumentOutOfRangeException((offset1 < 0) ? "offset1" : "offset2", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
				}
				if (offset1 > ((string1 == null) ? 0 : string1.Length) - length1)
				{
					throw new ArgumentOutOfRangeException("string1", Environment.GetResourceString("ArgumentOutOfRange_OffsetLength"));
				}
				if (offset2 > ((string2 == null) ? 0 : string2.Length) - length2)
				{
					throw new ArgumentOutOfRangeException("string2", Environment.GetResourceString("ArgumentOutOfRange_OffsetLength"));
				}
				if ((options & CompareOptions.Ordinal) != CompareOptions.None)
				{
					if (options != CompareOptions.Ordinal)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_CompareOptionOrdinal"), "options");
					}
					if (string1 == null)
					{
						if (string2 == null)
						{
							return 0;
						}
						return -1;
					}
					else
					{
						if (string2 == null)
						{
							return 1;
						}
						int num2 = string.nativeCompareOrdinalEx(string1, offset1, string2, offset2, (length1 < length2) ? length1 : length2);
						if (length1 == length2 || num2 != 0)
						{
							return num2;
						}
						if (length1 <= length2)
						{
							return -1;
						}
						return 1;
					}
				}
				else
				{
					if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort)) != CompareOptions.None)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
					}
					if (string1 == null)
					{
						if (string2 == null)
						{
							return 0;
						}
						return -1;
					}
					else
					{
						if (string2 == null)
						{
							return 1;
						}
						if (!this.IsSynthetic)
						{
							return CompareInfo.CompareRegion(this.m_pSortingTable, this.m_sortingLCID, string1, offset1, length1, string2, offset2, length2, options);
						}
						if (options == CompareOptions.Ordinal)
						{
							return CompareInfo.CompareRegion(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, string1, offset1, length1, string2, offset2, length2, options);
						}
						return CompareInfo.nativeCompareString(this.m_sortingLCID, string1, offset1, length1, string2, offset2, length2, CompareInfo.GetNativeCompareFlags(options));
					}
				}
			}
		}

		// Token: 0x060023B6 RID: 9142
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int CompareRegion(void* pSortingTable, int sortingLCID, string string1, int offset1, int length1, string string2, int offset2, int length2, CompareOptions options);

		// Token: 0x060023B7 RID: 9143 RVA: 0x0005C294 File Offset: 0x0005B294
		private unsafe static int FindNLSStringWrap(int lcid, int flags, string src, int start, int cchSrc, string value, int cchValue)
		{
			int num = -1;
			fixed (char* ptr = src)
			{
				fixed (char* ptr2 = value)
				{
					if (1 == CompareInfo.fFindNLSStringSupported)
					{
						num = Win32Native.FindNLSString(lcid, flags, ptr + start, cchSrc, ptr2, cchValue, IntPtr.Zero);
					}
					else
					{
						try
						{
							num = Win32Native.FindNLSString(lcid, flags, ptr + start, cchSrc, ptr2, cchValue, IntPtr.Zero);
							CompareInfo.fFindNLSStringSupported = 1;
						}
						catch (EntryPointNotFoundException)
						{
							num = (CompareInfo.fFindNLSStringSupported = -2);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x0005C328 File Offset: 0x0005B328
		private bool SyntheticIsPrefix(string source, int start, int length, string prefix, int nativeCompareFlags)
		{
			if (CompareInfo.fFindNLSStringSupported >= 0)
			{
				int num = CompareInfo.FindNLSStringWrap(this.m_sortingLCID, nativeCompareFlags | 1048576, source, start, length, prefix, prefix.Length);
				if (num >= -1)
				{
					return num != -1;
				}
			}
			for (int i = 1; i <= length; i++)
			{
				if (CompareInfo.nativeCompareString(this.m_sortingLCID, prefix, 0, prefix.Length, source, start, i, nativeCompareFlags) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x0005C394 File Offset: 0x0005B394
		private int SyntheticIsSuffix(string source, int end, int length, string suffix, int nativeCompareFlags)
		{
			if (CompareInfo.fFindNLSStringSupported >= 0)
			{
				int num = CompareInfo.FindNLSStringWrap(this.m_sortingLCID, nativeCompareFlags | 2097152, source, 0, length, suffix, suffix.Length);
				if (num >= -1)
				{
					return num;
				}
			}
			for (int i = 0; i < length; i++)
			{
				if (CompareInfo.nativeCompareString(this.m_sortingLCID, suffix, 0, suffix.Length, source, end - i, i + 1, nativeCompareFlags) == 0)
				{
					return end - i;
				}
			}
			return -1;
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x0005C400 File Offset: 0x0005B400
		private int SyntheticIndexOf(string source, string value, int start, int length, int nativeCompareFlags)
		{
			if (CompareInfo.fFindNLSStringSupported >= 0)
			{
				int num = CompareInfo.FindNLSStringWrap(this.m_sortingLCID, nativeCompareFlags | 4194304, source, start, length, value, value.Length);
				if (num > -1)
				{
					return num + start;
				}
				if (num == -1)
				{
					return num;
				}
			}
			for (int i = 0; i < length; i++)
			{
				if (this.SyntheticIsPrefix(source, start + i, length - i, value, nativeCompareFlags))
				{
					return start + i;
				}
			}
			return -1;
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0005C468 File Offset: 0x0005B468
		private int SyntheticLastIndexOf(string source, string value, int startIndex, int length, int nativeCompareFlags)
		{
			if (CompareInfo.fFindNLSStringSupported >= 0)
			{
				int num = CompareInfo.FindNLSStringWrap(this.m_sortingLCID, nativeCompareFlags | 8388608, source, startIndex - length + 1, length, value, value.Length);
				if (num > -1)
				{
					return num + (startIndex - length + 1);
				}
				if (num == -1)
				{
					return num;
				}
			}
			for (int i = 0; i < length; i++)
			{
				int num2 = this.SyntheticIsSuffix(source, startIndex - i, length - i, value, nativeCompareFlags);
				if (num2 >= 0)
				{
					return num2;
				}
			}
			return -1;
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x0005C4DC File Offset: 0x0005B4DC
		public virtual bool IsPrefix(string source, string prefix, CompareOptions options)
		{
			if (source == null || prefix == null)
			{
				throw new ArgumentNullException((source == null) ? "source" : "prefix", Environment.GetResourceString("ArgumentNull_String"));
			}
			if (prefix.Length == 0)
			{
				return true;
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return source.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (!this.IsSynthetic)
			{
				return CompareInfo.nativeIsPrefix(this.m_pSortingTable, this.m_sortingLCID, source, prefix, options);
			}
			if (options == CompareOptions.Ordinal)
			{
				return CompareInfo.nativeIsPrefix(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, prefix, options);
			}
			return this.SyntheticIsPrefix(source, 0, source.Length, prefix, CompareInfo.GetNativeCompareFlags(options));
		}

		// Token: 0x060023BD RID: 9149
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool nativeIsPrefix(void* pSortingTable, int sortingLCID, string source, string prefix, CompareOptions options);

		// Token: 0x060023BE RID: 9150 RVA: 0x0005C5A6 File Offset: 0x0005B5A6
		public virtual bool IsPrefix(string source, string prefix)
		{
			return this.IsPrefix(source, prefix, CompareOptions.None);
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x0005C5B4 File Offset: 0x0005B5B4
		public virtual bool IsSuffix(string source, string suffix, CompareOptions options)
		{
			if (source == null || suffix == null)
			{
				throw new ArgumentNullException((source == null) ? "source" : "suffix", Environment.GetResourceString("ArgumentNull_String"));
			}
			if (suffix.Length == 0)
			{
				return true;
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return source.EndsWith(suffix, StringComparison.OrdinalIgnoreCase);
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (!this.IsSynthetic)
			{
				return CompareInfo.nativeIsSuffix(this.m_pSortingTable, this.m_sortingLCID, source, suffix, options);
			}
			if (options == CompareOptions.Ordinal)
			{
				return CompareInfo.nativeIsSuffix(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, suffix, options);
			}
			return this.SyntheticIsSuffix(source, source.Length - 1, source.Length, suffix, CompareInfo.GetNativeCompareFlags(options)) >= 0;
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x0005C68B File Offset: 0x0005B68B
		public virtual bool IsSuffix(string source, string suffix)
		{
			return this.IsSuffix(source, suffix, CompareOptions.None);
		}

		// Token: 0x060023C1 RID: 9153
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool nativeIsSuffix(void* pSortingTable, int sortingLCID, string source, string prefix, CompareOptions options);

		// Token: 0x060023C2 RID: 9154 RVA: 0x0005C696 File Offset: 0x0005B696
		public virtual int IndexOf(string source, char value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, 0, source.Length, CompareOptions.None);
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x0005C6B6 File Offset: 0x0005B6B6
		public virtual int IndexOf(string source, string value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, 0, source.Length, CompareOptions.None);
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x0005C6D6 File Offset: 0x0005B6D6
		public virtual int IndexOf(string source, char value, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, 0, source.Length, options);
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x0005C6F6 File Offset: 0x0005B6F6
		public virtual int IndexOf(string source, string value, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, 0, source.Length, options);
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x0005C716 File Offset: 0x0005B716
		public virtual int IndexOf(string source, char value, int startIndex)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, startIndex, source.Length - startIndex, CompareOptions.None);
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x0005C738 File Offset: 0x0005B738
		public virtual int IndexOf(string source, string value, int startIndex)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, startIndex, source.Length - startIndex, CompareOptions.None);
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x0005C75A File Offset: 0x0005B75A
		public virtual int IndexOf(string source, char value, int startIndex, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, startIndex, source.Length - startIndex, options);
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x0005C77D File Offset: 0x0005B77D
		public virtual int IndexOf(string source, string value, int startIndex, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.IndexOf(source, value, startIndex, source.Length - startIndex, options);
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x0005C7A0 File Offset: 0x0005B7A0
		public virtual int IndexOf(string source, char value, int startIndex, int count)
		{
			return this.IndexOf(source, value, startIndex, count, CompareOptions.None);
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0005C7AE File Offset: 0x0005B7AE
		public virtual int IndexOf(string source, string value, int startIndex, int count)
		{
			return this.IndexOf(source, value, startIndex, count, CompareOptions.None);
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x0005C7BC File Offset: 0x0005B7BC
		public virtual int IndexOf(string source, char value, int startIndex, int count, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (startIndex < 0 || startIndex > source.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count < 0 || startIndex > source.Length - count)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return TextInfo.nativeIndexOfCharOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, source, value, startIndex, count);
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (!this.IsSynthetic)
			{
				return CompareInfo.IndexOfChar(this.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
			}
			if (options == CompareOptions.Ordinal)
			{
				return CompareInfo.IndexOfChar(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
			}
			return this.SyntheticIndexOf(source, new string(value, 1), startIndex, count, CompareInfo.GetNativeCompareFlags(options));
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x0005C8C4 File Offset: 0x0005B8C4
		public virtual int IndexOf(string source, string value, int startIndex, int count, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (startIndex > source.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (source.Length == 0)
			{
				if (value.Length == 0)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (startIndex < 0)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (count < 0 || startIndex > source.Length - count)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
				}
				if (options == CompareOptions.OrdinalIgnoreCase)
				{
					return TextInfo.IndexOfStringOrdinalIgnoreCase(source, value, startIndex, count);
				}
				if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
				}
				if (!this.IsSynthetic)
				{
					return CompareInfo.IndexOfString(this.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
				}
				if (options == CompareOptions.Ordinal)
				{
					return CompareInfo.IndexOfString(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
				}
				return this.SyntheticIndexOf(source, value, startIndex, count, CompareInfo.GetNativeCompareFlags(options));
			}
		}

		// Token: 0x060023CE RID: 9166
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int IndexOfChar(void* pSortingTable, int sortingLCID, string source, char value, int startIndex, int count, int options);

		// Token: 0x060023CF RID: 9167
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int IndexOfString(void* pSortingTable, int sortingLCID, string source, string value, int startIndex, int count, int options);

		// Token: 0x060023D0 RID: 9168 RVA: 0x0005C9F5 File Offset: 0x0005B9F5
		public virtual int LastIndexOf(string source, char value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.LastIndexOf(source, value, source.Length - 1, source.Length, CompareOptions.None);
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0005CA1C File Offset: 0x0005BA1C
		public virtual int LastIndexOf(string source, string value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.LastIndexOf(source, value, source.Length - 1, source.Length, CompareOptions.None);
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x0005CA43 File Offset: 0x0005BA43
		public virtual int LastIndexOf(string source, char value, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.LastIndexOf(source, value, source.Length - 1, source.Length, options);
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x0005CA6A File Offset: 0x0005BA6A
		public virtual int LastIndexOf(string source, string value, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return this.LastIndexOf(source, value, source.Length - 1, source.Length, options);
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x0005CA91 File Offset: 0x0005BA91
		public virtual int LastIndexOf(string source, char value, int startIndex)
		{
			return this.LastIndexOf(source, value, startIndex, startIndex + 1, CompareOptions.None);
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x0005CAA0 File Offset: 0x0005BAA0
		public virtual int LastIndexOf(string source, string value, int startIndex)
		{
			return this.LastIndexOf(source, value, startIndex, startIndex + 1, CompareOptions.None);
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x0005CAAF File Offset: 0x0005BAAF
		public virtual int LastIndexOf(string source, char value, int startIndex, CompareOptions options)
		{
			return this.LastIndexOf(source, value, startIndex, startIndex + 1, options);
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x0005CABF File Offset: 0x0005BABF
		public virtual int LastIndexOf(string source, string value, int startIndex, CompareOptions options)
		{
			return this.LastIndexOf(source, value, startIndex, startIndex + 1, options);
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x0005CACF File Offset: 0x0005BACF
		public virtual int LastIndexOf(string source, char value, int startIndex, int count)
		{
			return this.LastIndexOf(source, value, startIndex, count, CompareOptions.None);
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x0005CADD File Offset: 0x0005BADD
		public virtual int LastIndexOf(string source, string value, int startIndex, int count)
		{
			return this.LastIndexOf(source, value, startIndex, count, CompareOptions.None);
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x0005CAEC File Offset: 0x0005BAEC
		public virtual int LastIndexOf(string source, char value, int startIndex, int count, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal && options != CompareOptions.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (source.Length == 0 && (startIndex == -1 || startIndex == 0))
			{
				return -1;
			}
			if (startIndex < 0 || startIndex > source.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (startIndex == source.Length)
			{
				startIndex--;
				if (count > 0)
				{
					count--;
				}
			}
			if (count < 0 || startIndex - count + 1 < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			if (options == CompareOptions.OrdinalIgnoreCase)
			{
				return TextInfo.nativeLastIndexOfCharOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, source, value, startIndex, count);
			}
			if (!this.IsSynthetic)
			{
				return CompareInfo.LastIndexOfChar(this.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
			}
			if (options == CompareOptions.Ordinal)
			{
				return CompareInfo.LastIndexOfChar(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
			}
			return this.SyntheticLastIndexOf(source, new string(value, 1), startIndex, count, CompareInfo.GetNativeCompareFlags(options));
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x0005CC24 File Offset: 0x0005BC24
		public virtual int LastIndexOf(string source, string value, int startIndex, int count, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None && options != CompareOptions.Ordinal && options != CompareOptions.OrdinalIgnoreCase)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (source.Length == 0 && (startIndex == -1 || startIndex == 0))
			{
				if (value.Length != 0)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (startIndex < 0 || startIndex > source.Length)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (startIndex == source.Length)
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
				if (options == CompareOptions.OrdinalIgnoreCase)
				{
					return TextInfo.LastIndexOfStringOrdinalIgnoreCase(source, value, startIndex, count);
				}
				if (!this.IsSynthetic)
				{
					return CompareInfo.LastIndexOfString(this.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
				}
				if (options == CompareOptions.Ordinal)
				{
					return CompareInfo.LastIndexOfString(CultureInfo.InvariantCulture.CompareInfo.m_pSortingTable, this.m_sortingLCID, source, value, startIndex, count, (int)options);
				}
				return this.SyntheticLastIndexOf(source, value, startIndex, count, CompareInfo.GetNativeCompareFlags(options));
			}
		}

		// Token: 0x060023DC RID: 9180
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int LastIndexOfChar(void* pSortingTable, int sortingLCID, string source, char value, int startIndex, int count, int options);

		// Token: 0x060023DD RID: 9181
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int LastIndexOfString(void* pSortingTable, int sortingLCID, string source, string value, int startIndex, int count, int options);

		// Token: 0x060023DE RID: 9182 RVA: 0x0005CD7E File Offset: 0x0005BD7E
		public virtual SortKey GetSortKey(string source, CompareOptions options)
		{
			if (this.IsSynthetic)
			{
				return new SortKey(this.m_sortingLCID, source, options);
			}
			return new SortKey(this.m_pSortingTable, this.m_sortingLCID, source, options);
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x0005CDA9 File Offset: 0x0005BDA9
		public virtual SortKey GetSortKey(string source)
		{
			if (this.IsSynthetic)
			{
				return new SortKey(this.m_sortingLCID, source, CompareOptions.None);
			}
			return new SortKey(this.m_pSortingTable, this.m_sortingLCID, source, CompareOptions.None);
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0005CDD4 File Offset: 0x0005BDD4
		public override bool Equals(object value)
		{
			CompareInfo compareInfo = value as CompareInfo;
			return compareInfo != null && this.m_sortingLCID == compareInfo.m_sortingLCID && this.Name.Equals(compareInfo.Name);
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x0005CE0E File Offset: 0x0005BE0E
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x0005CE1C File Offset: 0x0005BE1C
		internal int GetHashCodeOfString(string source, CompareOptions options)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth)) != CompareOptions.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (source.Length == 0)
			{
				return 0;
			}
			if (this.IsSynthetic)
			{
				return CultureInfo.InvariantCulture.CompareInfo.GetHashCodeOfString(source, options);
			}
			return CompareInfo.nativeGetGlobalizedHashCode(this.m_pSortingTable, source, (int)options, this.m_sortingLCID);
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x0005CE89 File Offset: 0x0005BE89
		public override string ToString()
		{
			return "CompareInfo - " + this.culture;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x0005CEA0 File Offset: 0x0005BEA0
		public int LCID
		{
			get
			{
				return this.culture;
			}
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x0005CEA8 File Offset: 0x0005BEA8
		private unsafe static void* InitializeCompareInfo(void* pNativeGlobalizationAssembly, int sortingLCID)
		{
			void* ptr = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(typeof(CultureTableRecord), ref flag);
				ptr = CompareInfo.InitializeNativeCompareInfo(pNativeGlobalizationAssembly, sortingLCID);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(typeof(CultureTableRecord));
				}
			}
			return ptr;
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x0005CF00 File Offset: 0x0005BF00
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnDeserialized();
		}

		// Token: 0x060023E7 RID: 9191
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* InitializeNativeCompareInfo(void* pNativeGlobalizationAssembly, int sortingLCID);

		// Token: 0x04000F0D RID: 3853
		private const CompareOptions ValidIndexMaskOffFlags = ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);

		// Token: 0x04000F0E RID: 3854
		private const CompareOptions ValidCompareMaskOffFlags = ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort);

		// Token: 0x04000F0F RID: 3855
		private const CompareOptions ValidHashCodeOfStringMaskOffFlags = ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);

		// Token: 0x04000F10 RID: 3856
		private const int TraditionalChineseCultureId = 31748;

		// Token: 0x04000F11 RID: 3857
		private const int HongKongCultureId = 3076;

		// Token: 0x04000F12 RID: 3858
		internal const int CHT_CHS_LCID_COMPAREINFO_KEY_FLAG = -2147483648;

		// Token: 0x04000F13 RID: 3859
		private const int NORM_IGNORECASE = 1;

		// Token: 0x04000F14 RID: 3860
		private const int NORM_IGNOREKANATYPE = 65536;

		// Token: 0x04000F15 RID: 3861
		private const int NORM_IGNORENONSPACE = 2;

		// Token: 0x04000F16 RID: 3862
		private const int NORM_IGNORESYMBOLS = 4;

		// Token: 0x04000F17 RID: 3863
		private const int NORM_IGNOREWIDTH = 131072;

		// Token: 0x04000F18 RID: 3864
		private const int SORT_STRINGSORT = 4096;

		// Token: 0x04000F19 RID: 3865
		private static object s_InternalSyncObject;

		// Token: 0x04000F1A RID: 3866
		private int win32LCID;

		// Token: 0x04000F1B RID: 3867
		private int culture;

		// Token: 0x04000F1C RID: 3868
		[NonSerialized]
		internal unsafe void* m_pSortingTable;

		// Token: 0x04000F1D RID: 3869
		[NonSerialized]
		private int m_sortingLCID;

		// Token: 0x04000F1E RID: 3870
		[NonSerialized]
		private CultureTableRecord m_cultureTableRecord;

		// Token: 0x04000F1F RID: 3871
		[OptionalField(VersionAdded = 2)]
		private string m_name;

		// Token: 0x04000F20 RID: 3872
		[NonSerialized]
		private static int fFindNLSStringSupported;
	}
}
