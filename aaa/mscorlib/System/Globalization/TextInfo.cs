using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace System.Globalization
{
	// Token: 0x020003B4 RID: 948
	[ComVisible(true)]
	[Serializable]
	public class TextInfo : ICloneable, IDeserializationCallback
	{
		// Token: 0x06002721 RID: 10017 RVA: 0x00076180 File Offset: 0x00075180
		unsafe static TextInfo()
		{
			byte* globalizationResourceBytePtr = GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(TextInfo).Assembly, "l_intl.nlp");
			Thread.MemoryBarrier();
			TextInfo.m_pDataTable = globalizationResourceBytePtr;
			TextInfo.TextInfoDataHeader* pDataTable = (TextInfo.TextInfoDataHeader*)TextInfo.m_pDataTable;
			TextInfo.m_exceptionCount = (int)pDataTable->exceptionCount;
			TextInfo.m_exceptionTable = (TextInfo.ExceptionTableItem*)(&pDataTable->exceptionLangId);
			TextInfo.m_exceptionNativeTextInfo = new long[TextInfo.m_exceptionCount];
			TextInfo.m_pDefaultCasingTable = TextInfo.AllocateDefaultCasingTable(TextInfo.m_pDataTable);
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002722 RID: 10018 RVA: 0x000761F0 File Offset: 0x000751F0
		private static object InternalSyncObject
		{
			get
			{
				if (TextInfo.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref TextInfo.s_InternalSyncObject, obj, null);
				}
				return TextInfo.s_InternalSyncObject;
			}
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x0007621C File Offset: 0x0007521C
		internal TextInfo(CultureTableRecord table)
		{
			this.m_cultureTableRecord = table;
			this.m_textInfoID = (int)this.m_cultureTableRecord.ITEXTINFO;
			if (table.IsSynthetic)
			{
				this.m_pNativeTextInfo = TextInfo.InvariantNativeTextInfo;
				return;
			}
			this.m_pNativeTextInfo = TextInfo.GetNativeTextInfo(this.m_textInfoID);
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002724 RID: 10020 RVA: 0x0007626C File Offset: 0x0007526C
		internal unsafe static void* InvariantNativeTextInfo
		{
			get
			{
				if (TextInfo.m_pInvariantNativeTextInfo == null)
				{
					lock (TextInfo.InternalSyncObject)
					{
						if (TextInfo.m_pInvariantNativeTextInfo == null)
						{
							TextInfo.m_pInvariantNativeTextInfo = TextInfo.GetNativeTextInfo(127);
						}
					}
				}
				return TextInfo.m_pInvariantNativeTextInfo;
			}
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000762C4 File Offset: 0x000752C4
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.m_cultureTableRecord = null;
			this.m_win32LangID = 0;
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x000762D4 File Offset: 0x000752D4
		private void OnDeserialized()
		{
			if (this.m_cultureTableRecord == null)
			{
				if (this.m_win32LangID == 0)
				{
					this.m_win32LangID = CultureTableRecord.IdFromEverettDataItem(this.m_nDataItem);
				}
				if (this.customCultureName != null)
				{
					this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.customCultureName, this.m_useUserOverride);
				}
				else
				{
					this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.m_win32LangID, this.m_useUserOverride);
				}
				this.m_textInfoID = (int)this.m_cultureTableRecord.ITEXTINFO;
				if (this.m_cultureTableRecord.IsSynthetic)
				{
					this.m_pNativeTextInfo = TextInfo.InvariantNativeTextInfo;
					return;
				}
				this.m_pNativeTextInfo = TextInfo.GetNativeTextInfo(this.m_textInfoID);
			}
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x00076378 File Offset: 0x00075378
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.OnDeserialized();
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x00076380 File Offset: 0x00075380
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.m_nDataItem = this.m_cultureTableRecord.EverettDataItem();
			this.m_useUserOverride = this.m_cultureTableRecord.UseUserOverride;
			if (CultureTableRecord.IsCustomCultureId(this.m_cultureTableRecord.CultureID))
			{
				this.customCultureName = this.m_cultureTableRecord.SNAME;
				this.m_win32LangID = this.m_textInfoID;
				return;
			}
			this.customCultureName = null;
			this.m_win32LangID = this.m_cultureTableRecord.CultureID;
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000763F8 File Offset: 0x000753F8
		internal unsafe static void* GetNativeTextInfo(int cultureID)
		{
			if (cultureID != 127 || Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				void* ptr = TextInfo.m_pDefaultCasingTable;
				for (int i = 0; i < TextInfo.m_exceptionCount; i++)
				{
					if ((int)TextInfo.m_exceptionTable[i].langID == cultureID)
					{
						if (TextInfo.m_exceptionNativeTextInfo[i] == 0L)
						{
							lock (TextInfo.InternalSyncObject)
							{
								if (TextInfo.m_pExceptionFile == null)
								{
									TextInfo.m_pExceptionFile = GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(TextInfo).Assembly, "l_except.nlp");
								}
								long num = TextInfo.InternalAllocateCasingTable(TextInfo.m_pExceptionFile, (int)TextInfo.m_exceptionTable[i].exceptIndex);
								Thread.MemoryBarrier();
								TextInfo.m_exceptionNativeTextInfo[i] = num;
							}
						}
						ptr = TextInfo.m_exceptionNativeTextInfo[i];
						break;
					}
				}
				return ptr;
			}
			void* ptr2 = TextInfo.nativeGetInvariantTextInfo();
			if (ptr2 != null)
			{
				return ptr2;
			}
			throw new TypeInitializationException(typeof(TextInfo).ToString(), null);
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x00076504 File Offset: 0x00075504
		internal static int CompareOrdinalIgnoreCase(string str1, string str2)
		{
			return TextInfo.nativeCompareOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, str1, str2);
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x00076512 File Offset: 0x00075512
		internal static int CompareOrdinalIgnoreCaseEx(string strA, int indexA, string strB, int indexB, int length)
		{
			return TextInfo.nativeCompareOrdinalIgnoreCaseEx(TextInfo.InvariantNativeTextInfo, strA, indexA, strB, indexB, length);
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x00076524 File Offset: 0x00075524
		internal static int GetHashCodeOrdinalIgnoreCase(string s)
		{
			return TextInfo.nativeGetHashCodeOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, s);
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x00076531 File Offset: 0x00075531
		internal static int IndexOfStringOrdinalIgnoreCase(string source, string value, int startIndex, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return TextInfo.nativeIndexOfStringOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, source, value, startIndex, count);
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x0007654F File Offset: 0x0007554F
		internal static int LastIndexOfStringOrdinalIgnoreCase(string source, string value, int startIndex, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return TextInfo.nativeLastIndexOfStringOrdinalIgnoreCase(TextInfo.InvariantNativeTextInfo, source, value, startIndex, count);
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x0600272F RID: 10031 RVA: 0x0007656D File Offset: 0x0007556D
		public virtual int ANSICodePage
		{
			get
			{
				return (int)this.m_cultureTableRecord.IDEFAULTANSICODEPAGE;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x0007657A File Offset: 0x0007557A
		public virtual int OEMCodePage
		{
			get
			{
				return (int)this.m_cultureTableRecord.IDEFAULTOEMCODEPAGE;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002731 RID: 10033 RVA: 0x00076587 File Offset: 0x00075587
		public virtual int MacCodePage
		{
			get
			{
				return (int)this.m_cultureTableRecord.IDEFAULTMACCODEPAGE;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x00076594 File Offset: 0x00075594
		public virtual int EBCDICCodePage
		{
			get
			{
				return (int)this.m_cultureTableRecord.IDEFAULTEBCDICCODEPAGE;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002733 RID: 10035 RVA: 0x000765A1 File Offset: 0x000755A1
		[ComVisible(false)]
		public int LCID
		{
			get
			{
				return this.m_textInfoID;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x000765A9 File Offset: 0x000755A9
		[ComVisible(false)]
		public string CultureName
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = CultureInfo.GetCultureInfo(this.m_textInfoID).Name;
				}
				return this.m_name;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002735 RID: 10037 RVA: 0x000765CF File Offset: 0x000755CF
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x000765D8 File Offset: 0x000755D8
		[ComVisible(false)]
		public virtual object Clone()
		{
			object obj = base.MemberwiseClone();
			((TextInfo)obj).SetReadOnlyState(false);
			return obj;
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x000765FC File Offset: 0x000755FC
		[ComVisible(false)]
		public static TextInfo ReadOnly(TextInfo textInfo)
		{
			if (textInfo == null)
			{
				throw new ArgumentNullException("textInfo");
			}
			if (textInfo.IsReadOnly)
			{
				return textInfo;
			}
			TextInfo textInfo2 = (TextInfo)textInfo.MemberwiseClone();
			textInfo2.SetReadOnlyState(true);
			return textInfo2;
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x00076635 File Offset: 0x00075635
		private void VerifyWritable()
		{
			if (this.m_isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x0007664F File Offset: 0x0007564F
		internal void SetReadOnlyState(bool readOnly)
		{
			this.m_isReadOnly = readOnly;
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x0600273A RID: 10042 RVA: 0x00076658 File Offset: 0x00075658
		// (set) Token: 0x0600273B RID: 10043 RVA: 0x00076679 File Offset: 0x00075679
		public virtual string ListSeparator
		{
			get
			{
				if (this.m_listSeparator == null)
				{
					this.m_listSeparator = this.m_cultureTableRecord.SLIST;
				}
				return this.m_listSeparator;
			}
			[ComVisible(false)]
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_String"));
				}
				this.m_listSeparator = value;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x0600273C RID: 10044 RVA: 0x000766A0 File Offset: 0x000756A0
		internal TextInfo CasingTextInfo
		{
			get
			{
				if (this.m_casingTextInfo == null)
				{
					if (this.ANSICodePage == 1254)
					{
						this.m_casingTextInfo = CultureInfo.GetCultureInfo("tr-TR").TextInfo;
					}
					else
					{
						this.m_casingTextInfo = CultureInfo.GetCultureInfo("en-US").TextInfo;
					}
				}
				return this.m_casingTextInfo;
			}
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000766F4 File Offset: 0x000756F4
		public virtual char ToLower(char c)
		{
			if (this.m_cultureTableRecord.IsSynthetic)
			{
				return this.CasingTextInfo.ToLower(c);
			}
			return TextInfo.nativeChangeCaseChar(this.m_textInfoID, this.m_pNativeTextInfo, c, false);
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x00076723 File Offset: 0x00075723
		public virtual string ToLower(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (this.m_cultureTableRecord.IsSynthetic)
			{
				return this.CasingTextInfo.ToLower(str);
			}
			return TextInfo.nativeChangeCaseString(this.m_textInfoID, this.m_pNativeTextInfo, str, false);
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x00076760 File Offset: 0x00075760
		public virtual char ToUpper(char c)
		{
			if (this.m_cultureTableRecord.IsSynthetic)
			{
				return this.CasingTextInfo.ToUpper(c);
			}
			return TextInfo.nativeChangeCaseChar(this.m_textInfoID, this.m_pNativeTextInfo, c, true);
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x0007678F File Offset: 0x0007578F
		public virtual string ToUpper(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (this.m_cultureTableRecord.IsSynthetic)
			{
				return this.CasingTextInfo.ToUpper(str);
			}
			return TextInfo.nativeChangeCaseString(this.m_textInfoID, this.m_pNativeTextInfo, str, true);
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000767CC File Offset: 0x000757CC
		public override bool Equals(object obj)
		{
			TextInfo textInfo = obj as TextInfo;
			return textInfo != null && this.CultureName.Equals(textInfo.CultureName);
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000767F6 File Offset: 0x000757F6
		public override int GetHashCode()
		{
			return this.CultureName.GetHashCode();
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x00076803 File Offset: 0x00075803
		public override string ToString()
		{
			return "TextInfo - " + this.m_textInfoID;
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x0007681A File Offset: 0x0007581A
		private bool IsWordSeparator(UnicodeCategory category)
		{
			return (536672256 & (1 << (int)category)) != 0;
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x00076830 File Offset: 0x00075830
		public string ToTitleCase(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (this.m_cultureTableRecord.IsSynthetic)
			{
				if (this.ANSICodePage == 1254)
				{
					return CultureInfo.GetCultureInfo("tr-TR").TextInfo.ToTitleCase(str);
				}
				return CultureInfo.GetCultureInfo("en-US").TextInfo.ToTitleCase(str);
			}
			else
			{
				int length = str.Length;
				if (length == 0)
				{
					return str;
				}
				StringBuilder stringBuilder = new StringBuilder();
				string text = null;
				for (int i = 0; i < length; i++)
				{
					int num;
					UnicodeCategory unicodeCategory = CharUnicodeInfo.InternalGetUnicodeCategory(str, i, out num);
					if (char.CheckLetter(unicodeCategory))
					{
						if (num == 1)
						{
							stringBuilder.Append(TextInfo.nativeGetTitleCaseChar(this.m_pNativeTextInfo, str[i]));
						}
						else
						{
							char c;
							char c2;
							this.ChangeCaseSurrogate(str[i], str[i + 1], out c, out c2, true);
							stringBuilder.Append(c);
							stringBuilder.Append(c2);
						}
						i += num;
						int num2 = i;
						bool flag = unicodeCategory == UnicodeCategory.LowercaseLetter;
						while (i < length)
						{
							unicodeCategory = CharUnicodeInfo.InternalGetUnicodeCategory(str, i, out num);
							if (this.IsLetterCategory(unicodeCategory))
							{
								if (unicodeCategory == UnicodeCategory.LowercaseLetter)
								{
									flag = true;
								}
								i += num;
							}
							else if (str[i] == '\'')
							{
								i++;
								if (flag)
								{
									if (text == null)
									{
										text = this.ToLower(str);
									}
									stringBuilder.Append(text, num2, i - num2);
								}
								else
								{
									stringBuilder.Append(str, num2, i - num2);
								}
								num2 = i;
								flag = true;
							}
							else
							{
								if (this.IsWordSeparator(unicodeCategory))
								{
									break;
								}
								i += num;
							}
						}
						int num3 = i - num2;
						if (num3 > 0)
						{
							if (flag)
							{
								if (text == null)
								{
									text = this.ToLower(str);
								}
								stringBuilder.Append(text, num2, num3);
							}
							else
							{
								stringBuilder.Append(str, num2, num3);
							}
						}
						if (i < length)
						{
							if (num == 1)
							{
								stringBuilder.Append(str[i]);
							}
							else
							{
								stringBuilder.Append(str[i++]);
								stringBuilder.Append(str[i]);
							}
						}
					}
					else if (num == 1)
					{
						stringBuilder.Append(str[i]);
					}
					else
					{
						stringBuilder.Append(str[i++]);
						stringBuilder.Append(str[i]);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002746 RID: 10054 RVA: 0x00076A51 File Offset: 0x00075A51
		[ComVisible(false)]
		public bool IsRightToLeft
		{
			get
			{
				return (this.m_cultureTableRecord.ILINEORIENTATIONS & 32768) != 0;
			}
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x00076A6A File Offset: 0x00075A6A
		private bool IsLetterCategory(UnicodeCategory uc)
		{
			return uc == UnicodeCategory.UppercaseLetter || uc == UnicodeCategory.LowercaseLetter || uc == UnicodeCategory.TitlecaseLetter || uc == UnicodeCategory.ModifierLetter || uc == UnicodeCategory.OtherLetter;
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x00076A81 File Offset: 0x00075A81
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnDeserialized();
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x00076A8C File Offset: 0x00075A8C
		internal int GetCaseInsensitiveHashCode(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (this.m_pNativeTextInfo == null)
			{
				this.OnDeserialized();
			}
			int textInfoID = this.m_textInfoID;
			if (textInfoID == 1055 || textInfoID == 1068)
			{
				str = TextInfo.nativeChangeCaseString(this.m_textInfoID, this.m_pNativeTextInfo, str, true);
			}
			return TextInfo.nativeGetCaseInsHash(str, this.m_pNativeTextInfo);
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x00076AF0 File Offset: 0x00075AF0
		internal unsafe void ChangeCaseSurrogate(char highSurrogate, char lowSurrogate, out char resultHighSurrogate, out char resultLowSurrogate, bool isToUpper)
		{
			fixed (char* ptr = &resultHighSurrogate, ptr2 = &resultLowSurrogate)
			{
				TextInfo.nativeChangeCaseSurrogate(this.m_pNativeTextInfo, highSurrogate, lowSurrogate, ptr, ptr2, isToUpper);
			}
		}

		// Token: 0x0600274B RID: 10059
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* AllocateDefaultCasingTable(byte* ptr);

		// Token: 0x0600274C RID: 10060
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* nativeGetInvariantTextInfo();

		// Token: 0x0600274D RID: 10061
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void* InternalAllocateCasingTable(byte* ptr, int exceptionIndex);

		// Token: 0x0600274E RID: 10062
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeGetCaseInsHash(string str, void* pNativeTextInfo);

		// Token: 0x0600274F RID: 10063
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern char nativeGetTitleCaseChar(void* pNativeTextInfo, char ch);

		// Token: 0x06002750 RID: 10064
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern char nativeChangeCaseChar(int win32LangID, void* pNativeTextInfo, char ch, bool isToUpper);

		// Token: 0x06002751 RID: 10065
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern string nativeChangeCaseString(int win32LangID, void* pNativeTextInfo, string str, bool isToUpper);

		// Token: 0x06002752 RID: 10066
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void nativeChangeCaseSurrogate(void* pNativeTextInfo, char highSurrogate, char lowSurrogate, char* resultHighSurrogate, char* resultLowSurrogate, bool isToUpper);

		// Token: 0x06002753 RID: 10067
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeCompareOrdinalIgnoreCase(void* pNativeTextInfo, string str1, string str2);

		// Token: 0x06002754 RID: 10068
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeCompareOrdinalIgnoreCaseEx(void* pNativeTextInfo, string strA, int indexA, string strB, int indexB, int length);

		// Token: 0x06002755 RID: 10069
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeGetHashCodeOrdinalIgnoreCase(void* pNativeTextInfo, string s);

		// Token: 0x06002756 RID: 10070
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeIndexOfStringOrdinalIgnoreCase(void* pNativeTextInfo, string str, string value, int startIndex, int count);

		// Token: 0x06002757 RID: 10071
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int nativeLastIndexOfStringOrdinalIgnoreCase(void* pNativeTextInfo, string str, string value, int startIndex, int count);

		// Token: 0x06002758 RID: 10072
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int nativeIndexOfCharOrdinalIgnoreCase(void* pNativeTextInfo, string str, char value, int startIndex, int count);

		// Token: 0x06002759 RID: 10073
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int nativeLastIndexOfCharOrdinalIgnoreCase(void* pNativeTextInfo, string str, char value, int startIndex, int count);

		// Token: 0x040011A9 RID: 4521
		private const string CASING_FILE_NAME = "l_intl.nlp";

		// Token: 0x040011AA RID: 4522
		private const string CASING_EXCEPTIONS_FILE_NAME = "l_except.nlp";

		// Token: 0x040011AB RID: 4523
		private const int wordSeparatorMask = 536672256;

		// Token: 0x040011AC RID: 4524
		internal const int TurkishAnsiCodepage = 1254;

		// Token: 0x040011AD RID: 4525
		[OptionalField(VersionAdded = 2)]
		private string m_listSeparator;

		// Token: 0x040011AE RID: 4526
		[OptionalField(VersionAdded = 2)]
		private bool m_isReadOnly;

		// Token: 0x040011AF RID: 4527
		[NonSerialized]
		private int m_textInfoID;

		// Token: 0x040011B0 RID: 4528
		[NonSerialized]
		private string m_name;

		// Token: 0x040011B1 RID: 4529
		[NonSerialized]
		private CultureTableRecord m_cultureTableRecord;

		// Token: 0x040011B2 RID: 4530
		[NonSerialized]
		private TextInfo m_casingTextInfo;

		// Token: 0x040011B3 RID: 4531
		[NonSerialized]
		private unsafe void* m_pNativeTextInfo;

		// Token: 0x040011B4 RID: 4532
		private unsafe static void* m_pInvariantNativeTextInfo;

		// Token: 0x040011B5 RID: 4533
		private unsafe static void* m_pDefaultCasingTable;

		// Token: 0x040011B6 RID: 4534
		private unsafe static byte* m_pDataTable;

		// Token: 0x040011B7 RID: 4535
		private static int m_exceptionCount;

		// Token: 0x040011B8 RID: 4536
		private unsafe static TextInfo.ExceptionTableItem* m_exceptionTable;

		// Token: 0x040011B9 RID: 4537
		private unsafe static byte* m_pExceptionFile;

		// Token: 0x040011BA RID: 4538
		private static long[] m_exceptionNativeTextInfo;

		// Token: 0x040011BB RID: 4539
		private static object s_InternalSyncObject;

		// Token: 0x040011BC RID: 4540
		[OptionalField(VersionAdded = 2)]
		private string customCultureName;

		// Token: 0x040011BD RID: 4541
		internal int m_nDataItem;

		// Token: 0x040011BE RID: 4542
		internal bool m_useUserOverride;

		// Token: 0x040011BF RID: 4543
		internal int m_win32LangID;

		// Token: 0x020003B5 RID: 949
		[StructLayout(LayoutKind.Explicit)]
		internal struct TextInfoDataHeader
		{
			// Token: 0x040011C0 RID: 4544
			[FieldOffset(0)]
			internal char TableName;

			// Token: 0x040011C1 RID: 4545
			[FieldOffset(32)]
			internal ushort version;

			// Token: 0x040011C2 RID: 4546
			[FieldOffset(40)]
			internal uint OffsetToUpperCasingTable;

			// Token: 0x040011C3 RID: 4547
			[FieldOffset(44)]
			internal uint OffsetToLowerCasingTable;

			// Token: 0x040011C4 RID: 4548
			[FieldOffset(48)]
			internal uint OffsetToTitleCaseTable;

			// Token: 0x040011C5 RID: 4549
			[FieldOffset(52)]
			internal uint PlaneOffset;

			// Token: 0x040011C6 RID: 4550
			[FieldOffset(180)]
			internal ushort exceptionCount;

			// Token: 0x040011C7 RID: 4551
			[FieldOffset(182)]
			internal ushort exceptionLangId;
		}

		// Token: 0x020003B6 RID: 950
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		internal struct ExceptionTableItem
		{
			// Token: 0x040011C8 RID: 4552
			internal ushort langID;

			// Token: 0x040011C9 RID: 4553
			internal ushort exceptIndex;
		}
	}
}
