using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003AF RID: 943
	[ComVisible(true)]
	[Serializable]
	public class SortKey
	{
		// Token: 0x060026D2 RID: 9938 RVA: 0x00074CF0 File Offset: 0x00073CF0
		internal unsafe SortKey(void* pSortingFile, int win32LCID, string str, CompareOptions options)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort)) != CompareOptions.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			this.win32LCID = win32LCID;
			this.options = options;
			this.m_String = str;
			this.m_KeyData = CompareInfo.nativeCreateSortKey(pSortingFile, str, (int)options, win32LCID);
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x00074D58 File Offset: 0x00073D58
		internal SortKey(int win32LCID, string str, CompareOptions options)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if ((options & ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort)) != CompareOptions.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "options");
			}
			if (CultureInfo.GetNativeSortKey(win32LCID, CompareInfo.GetNativeCompareFlags(options), str, str.Length, out this.m_KeyData) < 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFlag"), "str");
			}
			this.win32LCID = win32LCID;
			this.options = options;
			this.m_String = str;
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060026D4 RID: 9940 RVA: 0x00074DDD File Offset: 0x00073DDD
		public virtual string OriginalString
		{
			get
			{
				return this.m_String;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x00074DE5 File Offset: 0x00073DE5
		public virtual byte[] KeyData
		{
			get
			{
				return (byte[])this.m_KeyData.Clone();
			}
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x00074DF8 File Offset: 0x00073DF8
		public static int Compare(SortKey sortkey1, SortKey sortkey2)
		{
			if (sortkey1 == null || sortkey2 == null)
			{
				throw new ArgumentNullException((sortkey1 == null) ? "sortkey1" : "sortkey2");
			}
			byte[] keyData = sortkey1.m_KeyData;
			byte[] keyData2 = sortkey2.m_KeyData;
			if (keyData.Length == 0)
			{
				if (keyData2.Length == 0)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (keyData2.Length == 0)
				{
					return 1;
				}
				int num = ((keyData.Length < keyData2.Length) ? keyData.Length : keyData2.Length);
				for (int i = 0; i < num; i++)
				{
					if (keyData[i] > keyData2[i])
					{
						return 1;
					}
					if (keyData[i] < keyData2[i])
					{
						return -1;
					}
				}
				return 0;
			}
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x00074E78 File Offset: 0x00073E78
		public override bool Equals(object value)
		{
			SortKey sortKey = value as SortKey;
			return sortKey != null && SortKey.Compare(this, sortKey) == 0;
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x00074E9B File Offset: 0x00073E9B
		public override int GetHashCode()
		{
			return CompareInfo.GetCompareInfo(this.win32LCID).GetHashCodeOfString(this.m_String, this.options);
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x00074EBC File Offset: 0x00073EBC
		public override string ToString()
		{
			return string.Concat(new object[] { "SortKey - ", this.win32LCID, ", ", this.options, ", ", this.m_String });
		}

		// Token: 0x04001187 RID: 4487
		private const CompareOptions ValidSortkeyCtorMaskOffFlags = ~(CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.StringSort);

		// Token: 0x04001188 RID: 4488
		internal int win32LCID;

		// Token: 0x04001189 RID: 4489
		internal CompareOptions options;

		// Token: 0x0400118A RID: 4490
		internal string m_String;

		// Token: 0x0400118B RID: 4491
		internal byte[] m_KeyData;
	}
}
