using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x0200037E RID: 894
	[ComVisible(true)]
	[Serializable]
	public class CultureInfo : ICloneable, IFormatProvider
	{
		// Token: 0x060023E8 RID: 9192
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsValidLCID(int LCID, int flag);

		// Token: 0x060023E9 RID: 9193
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsWin9xInstalledCulture(string cultureKey, int LCID);

		// Token: 0x060023EA RID: 9194
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern string nativeGetUserDefaultLCID(int* LCID, int lcidType);

		// Token: 0x060023EB RID: 9195
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern string nativeGetUserDefaultUILanguage(int* LCID);

		// Token: 0x060023EC RID: 9196
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern string nativeGetSystemDefaultUILanguage(int* LCID);

		// Token: 0x060023ED RID: 9197
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeSetThreadLocale(int LCID);

		// Token: 0x060023EE RID: 9198
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetLocaleInfo(int LCID, int field);

		// Token: 0x060023EF RID: 9199
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeGetCurrentCalendar();

		// Token: 0x060023F0 RID: 9200
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeGetDTFIUserValues(int lcid, ref DTFIUserOverrideValues values);

		// Token: 0x060023F1 RID: 9201
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeGetNFIUserValues(int lcid, NumberFormatInfo nfi);

		// Token: 0x060023F2 RID: 9202
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeGetCultureData(int lcid, ref CultureData cultureData);

		// Token: 0x060023F3 RID: 9203
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeEnumSystemLocales(out int[] localesArray);

		// Token: 0x060023F4 RID: 9204
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetCultureName(int lcid, bool useSNameLCType, bool getMonthName);

		// Token: 0x060023F5 RID: 9205
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetWindowsDirectory();

		// Token: 0x060023F6 RID: 9206
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool nativeFileExists(string fileName);

		// Token: 0x060023F7 RID: 9207
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int* nativeGetStaticInt32DataTable(int type, out int tableSize);

		// Token: 0x060023F8 RID: 9208 RVA: 0x0005CF08 File Offset: 0x0005BF08
		internal unsafe static int GetNativeSortKey(int lcid, int flags, string source, int cchSrc, out byte[] sortKey)
		{
			sortKey = null;
			if (string.IsNullOrEmpty(source) || cchSrc == 0)
			{
				sortKey = new byte[0];
				source = "\0";
				cchSrc = 1;
			}
			int num;
			fixed (char* ptr = source)
			{
				num = Win32Native.LCMapStringW(lcid, flags | 1024, ptr, cchSrc, null, 0);
				if (num == 0)
				{
					return -1;
				}
				if (sortKey == null)
				{
					sortKey = new byte[num];
					fixed (byte* ptr2 = sortKey)
					{
						num = Win32Native.LCMapStringW(lcid, flags | 1024, ptr, cchSrc, (char*)ptr2, num);
					}
				}
			}
			return num;
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x0005CFA8 File Offset: 0x0005BFA8
		static CultureInfo()
		{
			if (CultureInfo.m_InvariantCultureInfo == null)
			{
				CultureInfo.m_InvariantCultureInfo = new CultureInfo(127, false)
				{
					m_isReadOnly = true
				};
			}
			CultureInfo.m_userDefaultCulture = (CultureInfo.m_userDefaultUICulture = CultureInfo.m_InvariantCultureInfo);
			CultureInfo.m_userDefaultCulture = CultureInfo.InitUserDefaultCulture();
			CultureInfo.m_userDefaultUICulture = CultureInfo.InitUserDefaultUICulture();
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x0005CFF8 File Offset: 0x0005BFF8
		private unsafe static CultureInfo InitUserDefaultCulture()
		{
			int num;
			string text = CultureInfo.nativeGetUserDefaultLCID(&num, 1024);
			CultureInfo cultureInfo = CultureInfo.GetCultureByLCIDOrName(num, text);
			if (cultureInfo == null)
			{
				text = CultureInfo.nativeGetUserDefaultLCID(&num, 2048);
				cultureInfo = CultureInfo.GetCultureByLCIDOrName(num, text);
				if (cultureInfo == null)
				{
					return CultureInfo.InvariantCulture;
				}
			}
			cultureInfo.m_isReadOnly = true;
			return cultureInfo;
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x0005D048 File Offset: 0x0005C048
		private unsafe static CultureInfo InitUserDefaultUICulture()
		{
			int num;
			string text = CultureInfo.nativeGetUserDefaultUILanguage(&num);
			if (num == CultureInfo.UserDefaultCulture.LCID || text == CultureInfo.UserDefaultCulture.Name)
			{
				return CultureInfo.UserDefaultCulture;
			}
			CultureInfo cultureInfo = CultureInfo.GetCultureByLCIDOrName(num, text);
			if (cultureInfo == null)
			{
				text = CultureInfo.nativeGetSystemDefaultUILanguage(&num);
				cultureInfo = CultureInfo.GetCultureByLCIDOrName(num, text);
			}
			if (cultureInfo == null)
			{
				return CultureInfo.InvariantCulture;
			}
			cultureInfo.m_isReadOnly = true;
			return cultureInfo;
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x0005D0B0 File Offset: 0x0005C0B0
		public CultureInfo(string name)
			: this(name, true)
		{
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x0005D0BC File Offset: 0x0005C0BC
		public CultureInfo(string name, bool useUserOverride)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", Environment.GetResourceString("ArgumentNull_String"));
			}
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(name, useUserOverride);
			this.cultureID = this.m_cultureTableRecord.ActualCultureID;
			this.m_name = this.m_cultureTableRecord.ActualName;
			this.m_isInherited = base.GetType() != typeof(CultureInfo);
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x0005D131 File Offset: 0x0005C131
		public CultureInfo(int culture)
			: this(culture, true)
		{
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x0005D13C File Offset: 0x0005C13C
		public CultureInfo(int culture, bool useUserOverride)
		{
			if (culture < 0)
			{
				throw new ArgumentOutOfRangeException("culture", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (culture <= 1024)
			{
				if (culture != 0 && culture != 1024)
				{
					goto IL_0075;
				}
			}
			else if (culture != 2048 && culture != 3072 && culture != 4096)
			{
				goto IL_0075;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_CultureNotSupported", new object[] { culture }), "culture");
			IL_0075:
			this.cultureID = culture;
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.cultureID, useUserOverride);
			this.m_name = this.m_cultureTableRecord.ActualName;
			this.m_isInherited = base.GetType() != typeof(CultureInfo);
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x0005D204 File Offset: 0x0005C204
		internal static void CheckDomainSafetyObject(object obj, object container)
		{
			if (obj.GetType().Assembly != typeof(CultureInfo).Assembly)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_SubclassedObject"), new object[]
				{
					obj.GetType(),
					container.GetType()
				}));
			}
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x0005D264 File Offset: 0x0005C264
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_name != null && this.cultureID != 1034)
			{
				this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.m_name, this.m_useUserOverride);
			}
			else
			{
				this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.cultureID, this.m_useUserOverride);
			}
			this.m_isInherited = base.GetType() != typeof(CultureInfo);
			if (this.m_name == null)
			{
				this.m_name = this.m_cultureTableRecord.ActualName;
			}
			if (base.GetType().Assembly == typeof(CultureInfo).Assembly)
			{
				if (this.textInfo != null)
				{
					CultureInfo.CheckDomainSafetyObject(this.textInfo, this);
				}
				if (this.compareInfo != null)
				{
					CultureInfo.CheckDomainSafetyObject(this.compareInfo, this);
				}
			}
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x0005D32E File Offset: 0x0005C32E
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.m_name = this.m_cultureTableRecord.CultureName;
			this.m_useUserOverride = this.m_cultureTableRecord.UseUserOverride;
			this.m_dataItem = this.m_cultureTableRecord.EverettDataItem();
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06002403 RID: 9219 RVA: 0x0005D363 File Offset: 0x0005C363
		internal bool IsSafeCrossDomain
		{
			get
			{
				return this.m_isSafeCrossDomain;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002404 RID: 9220 RVA: 0x0005D36B File Offset: 0x0005C36B
		internal int CreatedDomainID
		{
			get
			{
				return this.m_createdDomainID;
			}
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x0005D373 File Offset: 0x0005C373
		internal void StartCrossDomainTracking()
		{
			if (this.m_createdDomainID != 0)
			{
				return;
			}
			if (base.GetType() == typeof(CultureInfo))
			{
				this.m_isSafeCrossDomain = true;
			}
			Thread.MemoryBarrier();
			this.m_createdDomainID = Thread.GetDomainID();
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x0005D3A8 File Offset: 0x0005C3A8
		internal CultureInfo(string cultureName, string textAndCompareCultureName)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("cultureName", Environment.GetResourceString("ArgumentNull_String"));
			}
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(cultureName, false);
			this.cultureID = this.m_cultureTableRecord.ActualCultureID;
			this.m_name = this.m_cultureTableRecord.ActualName;
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo(textAndCompareCultureName);
			this.compareInfo = cultureInfo.CompareInfo;
			this.textInfo = cultureInfo.TextInfo;
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x0005D424 File Offset: 0x0005C424
		private static CultureInfo GetCultureByLCIDOrName(int preferLCID, string fallbackToString)
		{
			CultureInfo cultureInfo = null;
			if ((preferLCID & 1023) != 0)
			{
				try
				{
					cultureInfo = new CultureInfo(preferLCID);
				}
				catch (ArgumentException)
				{
				}
			}
			if (cultureInfo == null && fallbackToString != null && fallbackToString.Length > 0)
			{
				try
				{
					cultureInfo = new CultureInfo(fallbackToString);
				}
				catch (ArgumentException)
				{
				}
			}
			return cultureInfo;
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0005D480 File Offset: 0x0005C480
		public static CultureInfo CreateSpecificCulture(string name)
		{
			CultureInfo cultureInfo;
			try
			{
				cultureInfo = new CultureInfo(name);
			}
			catch (ArgumentException ex)
			{
				cultureInfo = null;
				for (int i = 0; i < name.Length; i++)
				{
					if ('-' == name[i])
					{
						try
						{
							cultureInfo = new CultureInfo(name.Substring(0, i));
							break;
						}
						catch (ArgumentException)
						{
							throw ex;
						}
					}
				}
				if (cultureInfo == null)
				{
					throw ex;
				}
			}
			if (!cultureInfo.IsNeutralCulture)
			{
				return cultureInfo;
			}
			int lcid = cultureInfo.LCID;
			if ((lcid & 1023) == 4)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NoSpecificCulture"));
			}
			return new CultureInfo(cultureInfo.m_cultureTableRecord.SSPECIFICCULTURE);
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x0005D528 File Offset: 0x0005C528
		internal static bool VerifyCultureName(CultureInfo culture, bool throwException)
		{
			if (!culture.m_isInherited)
			{
				return true;
			}
			string name = culture.Name;
			int i = 0;
			while (i < name.Length)
			{
				char c = name[i];
				if (!char.IsLetterOrDigit(c) && c != '-' && c != '_')
				{
					if (throwException)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidResourceCultureName", new object[] { name }));
					}
					return false;
				}
				else
				{
					i++;
				}
			}
			return true;
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x0005D593 File Offset: 0x0005C593
		internal static int GetSubLangID(int culture)
		{
			return (culture >> 10) & 63;
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x0005D59C File Offset: 0x0005C59C
		internal static int GetLangID(int culture)
		{
			return culture & 65535;
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x0005D5A5 File Offset: 0x0005C5A5
		internal static int GetSortID(int lcid)
		{
			return (lcid >> 16) & 15;
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x0600240D RID: 9229 RVA: 0x0005D5AE File Offset: 0x0005C5AE
		public static CultureInfo CurrentCulture
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x0005D5BC File Offset: 0x0005C5BC
		internal static CultureInfo UserDefaultCulture
		{
			get
			{
				CultureInfo cultureInfo = CultureInfo.m_userDefaultCulture;
				if (cultureInfo == null)
				{
					CultureInfo.m_userDefaultCulture = CultureInfo.InvariantCulture;
					cultureInfo = CultureInfo.InitUserDefaultCulture();
					CultureInfo.m_userDefaultCulture = cultureInfo;
				}
				return cultureInfo;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x0005D5EC File Offset: 0x0005C5EC
		internal static CultureInfo UserDefaultUICulture
		{
			get
			{
				CultureInfo cultureInfo = CultureInfo.m_userDefaultUICulture;
				if (cultureInfo == null)
				{
					CultureInfo.m_userDefaultUICulture = CultureInfo.InvariantCulture;
					cultureInfo = CultureInfo.InitUserDefaultUICulture();
					CultureInfo.m_userDefaultUICulture = cultureInfo;
				}
				return cultureInfo;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x0005D619 File Offset: 0x0005C619
		public static CultureInfo CurrentUICulture
		{
			get
			{
				return Thread.CurrentThread.CurrentUICulture;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002411 RID: 9233 RVA: 0x0005D628 File Offset: 0x0005C628
		public unsafe static CultureInfo InstalledUICulture
		{
			get
			{
				CultureInfo cultureInfo = CultureInfo.m_InstalledUICultureInfo;
				if (cultureInfo == null)
				{
					int num;
					string text = CultureInfo.nativeGetSystemDefaultUILanguage(&num);
					cultureInfo = CultureInfo.GetCultureByLCIDOrName(num, text);
					if (cultureInfo == null)
					{
						cultureInfo = new CultureInfo(127, true);
					}
					cultureInfo.m_isReadOnly = true;
					CultureInfo.m_InstalledUICultureInfo = cultureInfo;
				}
				return cultureInfo;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x0005D669 File Offset: 0x0005C669
		public static CultureInfo InvariantCulture
		{
			get
			{
				return CultureInfo.m_InvariantCultureInfo;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x0005D670 File Offset: 0x0005C670
		public virtual CultureInfo Parent
		{
			get
			{
				if (this.m_parent == null)
				{
					try
					{
						int iparent = (int)this.m_cultureTableRecord.IPARENT;
						if (iparent == 127)
						{
							this.m_parent = CultureInfo.InvariantCulture;
						}
						else if (CultureTableRecord.IsCustomCultureId(iparent) || CultureTable.IsOldNeutralChineseCulture(this))
						{
							this.m_parent = new CultureInfo(this.m_cultureTableRecord.SPARENT);
						}
						else
						{
							this.m_parent = new CultureInfo(iparent, this.m_cultureTableRecord.UseUserOverride);
						}
					}
					catch (ArgumentException)
					{
						this.m_parent = CultureInfo.InvariantCulture;
					}
				}
				return this.m_parent;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x0005D708 File Offset: 0x0005C708
		public virtual int LCID
		{
			get
			{
				return this.cultureID;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x0005D710 File Offset: 0x0005C710
		[ComVisible(false)]
		public virtual int KeyboardLayoutId
		{
			get
			{
				return (int)this.m_cultureTableRecord.IINPUTLANGUAGEHANDLE;
			}
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x0005D72A File Offset: 0x0005C72A
		public static CultureInfo[] GetCultures(CultureTypes types)
		{
			return CultureTable.Default.GetCultures(types);
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x0005D737 File Offset: 0x0005C737
		public virtual string Name
		{
			get
			{
				if (this.m_nonSortName == null)
				{
					this.m_nonSortName = this.m_cultureTableRecord.CultureName;
				}
				return this.m_nonSortName;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x0005D758 File Offset: 0x0005C758
		internal string SortName
		{
			get
			{
				if (this.m_sortName == null)
				{
					if (CultureTableRecord.IsCustomCultureId(this.cultureID))
					{
						CultureInfo cultureInfo = CultureInfo.GetCultureInfo(this.CompareInfoId);
						if (CultureTableRecord.IsCustomCultureId(cultureInfo.cultureID))
						{
							this.m_sortName = this.m_cultureTableRecord.SNAME;
						}
						else
						{
							this.m_sortName = cultureInfo.SortName;
						}
					}
					else
					{
						this.m_sortName = this.m_name;
					}
				}
				return this.m_sortName;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x0005D7C6 File Offset: 0x0005C7C6
		[ComVisible(false)]
		public string IetfLanguageTag
		{
			get
			{
				if (CultureTable.IsOldNeutralChineseCulture(this))
				{
					if (this.LCID == 31748)
					{
						return "zh-Hant";
					}
					if (this.LCID == 4)
					{
						return "zh-Hans";
					}
				}
				return this.Name;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x0005D7F8 File Offset: 0x0005C7F8
		public virtual string DisplayName
		{
			get
			{
				if (this.m_cultureTableRecord.IsCustomCulture)
				{
					if (!this.m_cultureTableRecord.IsReplacementCulture)
					{
						return this.m_cultureTableRecord.SNATIVEDISPLAYNAME;
					}
					if (this.m_cultureTableRecord.IsSynthetic)
					{
						return this.m_cultureTableRecord.CultureNativeDisplayName;
					}
					return Environment.GetResourceString("Globalization.ci_" + this.m_name);
				}
				else
				{
					if (this.m_cultureTableRecord.IsSynthetic)
					{
						return this.m_cultureTableRecord.CultureNativeDisplayName;
					}
					return Environment.GetResourceString("Globalization.ci_" + this.m_name);
				}
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x0005D888 File Offset: 0x0005C888
		public virtual string NativeName
		{
			get
			{
				return this.m_cultureTableRecord.SNATIVEDISPLAYNAME;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x0005D895 File Offset: 0x0005C895
		public virtual string EnglishName
		{
			get
			{
				return this.m_cultureTableRecord.SENGDISPLAYNAME;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x0005D8A2 File Offset: 0x0005C8A2
		public virtual string TwoLetterISOLanguageName
		{
			get
			{
				return this.m_cultureTableRecord.SISO639LANGNAME;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x0005D8AF File Offset: 0x0005C8AF
		public virtual string ThreeLetterISOLanguageName
		{
			get
			{
				return this.m_cultureTableRecord.SISO639LANGNAME2;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x0005D8BC File Offset: 0x0005C8BC
		public virtual string ThreeLetterWindowsLanguageName
		{
			get
			{
				return this.m_cultureTableRecord.SABBREVLANGNAME;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x0005D8CC File Offset: 0x0005C8CC
		public virtual CompareInfo CompareInfo
		{
			get
			{
				if (this.compareInfo == null)
				{
					int num;
					if (this.IsNeutralCulture && !CultureTableRecord.IsCustomCultureId(this.cultureID))
					{
						num = this.cultureID;
					}
					else
					{
						num = this.CompareInfoId;
					}
					if (this.Name == "zh-CHS" || this.Name == "zh-CHT")
					{
						num |= int.MinValue;
					}
					CompareInfo compareInfoWithPrefixedLcid = CompareInfo.GetCompareInfoWithPrefixedLcid(num, int.MinValue);
					compareInfoWithPrefixedLcid.SetName(this.SortName);
					this.compareInfo = compareInfoWithPrefixedLcid;
				}
				return this.compareInfo;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x0005D958 File Offset: 0x0005C958
		internal int CompareInfoId
		{
			get
			{
				int num;
				if (this.cultureID == 1034)
				{
					num = 1034;
				}
				else if (CultureInfo.GetSortID(this.cultureID) != 0)
				{
					num = this.cultureID;
				}
				else
				{
					num = (int)this.m_cultureTableRecord.ICOMPAREINFO;
				}
				return num;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x0005D9A0 File Offset: 0x0005C9A0
		public virtual TextInfo TextInfo
		{
			get
			{
				if (this.textInfo == null)
				{
					TextInfo textInfo = new TextInfo(this.m_cultureTableRecord);
					textInfo.SetReadOnlyState(this.m_isReadOnly);
					this.textInfo = textInfo;
				}
				return this.textInfo;
			}
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x0005D9DC File Offset: 0x0005C9DC
		public override bool Equals(object value)
		{
			if (object.ReferenceEquals(this, value))
			{
				return true;
			}
			CultureInfo cultureInfo = value as CultureInfo;
			return cultureInfo != null && this.Name.Equals(cultureInfo.Name) && this.CompareInfo.Equals(cultureInfo.CompareInfo);
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x0005DA26 File Offset: 0x0005CA26
		public override int GetHashCode()
		{
			return this.Name.GetHashCode() + this.CompareInfo.GetHashCode();
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x0005DA3F File Offset: 0x0005CA3F
		public override string ToString()
		{
			return this.m_name;
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x0005DA47 File Offset: 0x0005CA47
		public virtual object GetFormat(Type formatType)
		{
			if (formatType == typeof(NumberFormatInfo))
			{
				return this.NumberFormat;
			}
			if (formatType == typeof(DateTimeFormatInfo))
			{
				return this.DateTimeFormat;
			}
			return null;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x0005DA74 File Offset: 0x0005CA74
		internal static void CheckNeutral(CultureInfo culture)
		{
			if (culture.IsNeutralCulture)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_CultureInvalidFormat", new object[] { culture.m_name }));
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002428 RID: 9256 RVA: 0x0005DAAA File Offset: 0x0005CAAA
		public virtual bool IsNeutralCulture
		{
			get
			{
				return this.m_cultureTableRecord.IsNeutralCulture;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002429 RID: 9257 RVA: 0x0005DAB8 File Offset: 0x0005CAB8
		[ComVisible(false)]
		public CultureTypes CultureTypes
		{
			get
			{
				CultureTypes cultureTypes = (CultureTypes)0;
				if (this.m_cultureTableRecord.IsNeutralCulture)
				{
					cultureTypes |= CultureTypes.NeutralCultures;
				}
				else
				{
					cultureTypes |= CultureTypes.SpecificCultures;
				}
				if (this.m_cultureTableRecord.IsSynthetic)
				{
					cultureTypes |= CultureTypes.InstalledWin32Cultures | CultureTypes.WindowsOnlyCultures;
				}
				else
				{
					if (CultureTable.IsInstalledLCID(this.cultureID))
					{
						cultureTypes |= CultureTypes.InstalledWin32Cultures;
					}
					if (!this.m_cultureTableRecord.IsCustomCulture || this.m_cultureTableRecord.IsReplacementCulture)
					{
						cultureTypes |= CultureTypes.FrameworkCultures;
					}
				}
				if (this.m_cultureTableRecord.IsCustomCulture)
				{
					cultureTypes |= CultureTypes.UserCustomCulture;
					if (this.m_cultureTableRecord.IsReplacementCulture)
					{
						cultureTypes |= CultureTypes.ReplacementCultures;
					}
				}
				return cultureTypes;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600242A RID: 9258 RVA: 0x0005DB48 File Offset: 0x0005CB48
		// (set) Token: 0x0600242B RID: 9259 RVA: 0x0005DB88 File Offset: 0x0005CB88
		public virtual NumberFormatInfo NumberFormat
		{
			get
			{
				CultureInfo.CheckNeutral(this);
				if (this.numInfo == null)
				{
					this.numInfo = new NumberFormatInfo(this.m_cultureTableRecord)
					{
						isReadOnly = this.m_isReadOnly
					};
				}
				return this.numInfo;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				this.numInfo = value;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x0005DBB0 File Offset: 0x0005CBB0
		// (set) Token: 0x0600242D RID: 9261 RVA: 0x0005DC06 File Offset: 0x0005CC06
		public virtual DateTimeFormatInfo DateTimeFormat
		{
			get
			{
				if (this.dateTimeInfo == null)
				{
					CultureInfo.CheckNeutral(this);
					DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo(this.m_cultureTableRecord, CultureInfo.GetLangID(this.cultureID), this.Calendar);
					dateTimeFormatInfo.m_isReadOnly = this.m_isReadOnly;
					Thread.MemoryBarrier();
					this.dateTimeInfo = dateTimeFormatInfo;
				}
				return this.dateTimeInfo;
			}
			set
			{
				this.VerifyWritable();
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_Obj"));
				}
				this.dateTimeInfo = value;
			}
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0005DC2D File Offset: 0x0005CC2D
		public void ClearCachedData()
		{
			CultureInfo.m_userDefaultUICulture = null;
			CultureInfo.m_userDefaultCulture = null;
			RegionInfo.m_currentRegionInfo = null;
			TimeZone.ResetTimeZone();
			CultureInfo.m_LcidCachedCultures = null;
			CultureInfo.m_NameCachedCultures = null;
			CultureTableRecord.ResetCustomCulturesCache();
			CompareInfo.ClearDefaultAssemblyCache();
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x0005DC5C File Offset: 0x0005CC5C
		internal static Calendar GetCalendarInstance(int calType)
		{
			if (calType == 1)
			{
				return new GregorianCalendar();
			}
			return CultureInfo.GetCalendarInstanceRare(calType);
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0005DC70 File Offset: 0x0005CC70
		internal static Calendar GetCalendarInstanceRare(int calType)
		{
			switch (calType)
			{
			case 2:
			case 9:
			case 10:
			case 11:
			case 12:
				return new GregorianCalendar((GregorianCalendarTypes)calType);
			case 3:
				return new JapaneseCalendar();
			case 4:
				return new TaiwanCalendar();
			case 5:
				return new KoreanCalendar();
			case 6:
				return new HijriCalendar();
			case 7:
				return new ThaiBuddhistCalendar();
			case 8:
				return new HebrewCalendar();
			case 14:
				return new JapaneseLunisolarCalendar();
			case 15:
				return new ChineseLunisolarCalendar();
			case 20:
				return new KoreanLunisolarCalendar();
			case 21:
				return new TaiwanLunisolarCalendar();
			case 22:
				return new PersianCalendar();
			case 23:
				return new UmAlQuraCalendar();
			}
			return new GregorianCalendar();
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x0005DD38 File Offset: 0x0005CD38
		public virtual Calendar Calendar
		{
			get
			{
				if (this.calendar == null)
				{
					int icalendartype = (int)this.m_cultureTableRecord.ICALENDARTYPE;
					Calendar calendarInstance = CultureInfo.GetCalendarInstance(icalendartype);
					Thread.MemoryBarrier();
					calendarInstance.SetReadOnlyState(this.m_isReadOnly);
					this.calendar = calendarInstance;
				}
				return this.calendar;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002432 RID: 9266 RVA: 0x0005DD80 File Offset: 0x0005CD80
		public virtual Calendar[] OptionalCalendars
		{
			get
			{
				int[] ioptionalcalendars = this.m_cultureTableRecord.IOPTIONALCALENDARS;
				Calendar[] array = new Calendar[ioptionalcalendars.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = CultureInfo.GetCalendarInstance(ioptionalcalendars[i]);
				}
				return array;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x0005DDBC File Offset: 0x0005CDBC
		public bool UseUserOverride
		{
			get
			{
				return this.m_cultureTableRecord.UseUserOverride;
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0005DDCC File Offset: 0x0005CDCC
		[ComVisible(false)]
		public CultureInfo GetConsoleFallbackUICulture()
		{
			CultureInfo cultureInfo = this.m_consoleFallbackCulture;
			if (cultureInfo == null)
			{
				cultureInfo = CultureInfo.GetCultureInfo(this.m_cultureTableRecord.SCONSOLEFALLBACKNAME);
				cultureInfo.m_isReadOnly = true;
				this.m_consoleFallbackCulture = cultureInfo;
			}
			return cultureInfo;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x0005DE04 File Offset: 0x0005CE04
		public virtual object Clone()
		{
			CultureInfo cultureInfo = (CultureInfo)base.MemberwiseClone();
			cultureInfo.m_isReadOnly = false;
			if (!cultureInfo.IsNeutralCulture)
			{
				if (!this.m_isInherited)
				{
					if (this.dateTimeInfo != null)
					{
						cultureInfo.dateTimeInfo = (DateTimeFormatInfo)this.dateTimeInfo.Clone();
					}
					if (this.numInfo != null)
					{
						cultureInfo.numInfo = (NumberFormatInfo)this.numInfo.Clone();
					}
				}
				else
				{
					cultureInfo.DateTimeFormat = (DateTimeFormatInfo)this.DateTimeFormat.Clone();
					cultureInfo.NumberFormat = (NumberFormatInfo)this.NumberFormat.Clone();
				}
			}
			if (this.textInfo != null)
			{
				cultureInfo.textInfo = (TextInfo)this.textInfo.Clone();
			}
			if (this.calendar != null)
			{
				cultureInfo.calendar = (Calendar)this.calendar.Clone();
			}
			return cultureInfo;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x0005DEDC File Offset: 0x0005CEDC
		public static CultureInfo ReadOnly(CultureInfo ci)
		{
			if (ci == null)
			{
				throw new ArgumentNullException("ci");
			}
			if (ci.IsReadOnly)
			{
				return ci;
			}
			CultureInfo cultureInfo = (CultureInfo)ci.MemberwiseClone();
			if (!ci.IsNeutralCulture)
			{
				if (!ci.m_isInherited)
				{
					if (ci.dateTimeInfo != null)
					{
						cultureInfo.dateTimeInfo = DateTimeFormatInfo.ReadOnly(ci.dateTimeInfo);
					}
					if (ci.numInfo != null)
					{
						cultureInfo.numInfo = NumberFormatInfo.ReadOnly(ci.numInfo);
					}
				}
				else
				{
					cultureInfo.DateTimeFormat = DateTimeFormatInfo.ReadOnly(ci.DateTimeFormat);
					cultureInfo.NumberFormat = NumberFormatInfo.ReadOnly(ci.NumberFormat);
				}
			}
			if (ci.textInfo != null)
			{
				cultureInfo.textInfo = TextInfo.ReadOnly(ci.textInfo);
			}
			if (ci.calendar != null)
			{
				cultureInfo.calendar = Calendar.ReadOnly(ci.calendar);
			}
			cultureInfo.m_isReadOnly = true;
			return cultureInfo;
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x0005DFAD File Offset: 0x0005CFAD
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0005DFB5 File Offset: 0x0005CFB5
		private void VerifyWritable()
		{
			if (this.m_isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0005DFD0 File Offset: 0x0005CFD0
		internal static CultureInfo GetCultureInfoHelper(int lcid, string name, string altName)
		{
			Hashtable hashtable = CultureInfo.m_NameCachedCultures;
			if (name != null)
			{
				name = CultureTableRecord.AnsiToLower(name);
			}
			if (altName != null)
			{
				altName = CultureTableRecord.AnsiToLower(altName);
			}
			CultureInfo cultureInfo;
			if (hashtable == null)
			{
				hashtable = Hashtable.Synchronized(new Hashtable());
			}
			else if (lcid == -1)
			{
				cultureInfo = (CultureInfo)hashtable[name + '\ufffd' + altName];
				if (cultureInfo != null)
				{
					return cultureInfo;
				}
			}
			else if (lcid == 0)
			{
				cultureInfo = (CultureInfo)hashtable[name];
				if (cultureInfo != null)
				{
					return cultureInfo;
				}
			}
			Hashtable hashtable2 = CultureInfo.m_LcidCachedCultures;
			if (hashtable2 == null)
			{
				hashtable2 = Hashtable.Synchronized(new Hashtable());
			}
			else if (lcid > 0)
			{
				cultureInfo = (CultureInfo)hashtable2[lcid];
				if (cultureInfo != null)
				{
					return cultureInfo;
				}
			}
			try
			{
				switch (lcid)
				{
				case -1:
					cultureInfo = new CultureInfo(name, altName);
					break;
				case 0:
					cultureInfo = new CultureInfo(name, false);
					break;
				default:
					if (CultureInfo.m_userDefaultCulture != null && CultureInfo.m_userDefaultCulture.LCID == lcid)
					{
						cultureInfo = (CultureInfo)CultureInfo.m_userDefaultCulture.Clone();
						cultureInfo.m_cultureTableRecord = cultureInfo.m_cultureTableRecord.CloneWithUserOverride(false);
					}
					else
					{
						cultureInfo = new CultureInfo(lcid, false);
					}
					break;
				}
			}
			catch (ArgumentException)
			{
				return null;
			}
			cultureInfo.m_isReadOnly = true;
			if (lcid == -1)
			{
				hashtable[name + '\ufffd' + altName] = cultureInfo;
				cultureInfo.TextInfo.SetReadOnlyState(true);
			}
			else
			{
				if (!CultureTable.IsNewNeutralChineseCulture(cultureInfo))
				{
					hashtable2[cultureInfo.LCID] = cultureInfo;
				}
				string text = CultureTableRecord.AnsiToLower(cultureInfo.m_name);
				hashtable[text] = cultureInfo;
			}
			if (-1 != lcid)
			{
				CultureInfo.m_LcidCachedCultures = hashtable2;
			}
			CultureInfo.m_NameCachedCultures = hashtable;
			return cultureInfo;
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x0005E16C File Offset: 0x0005D16C
		public static CultureInfo GetCultureInfo(int culture)
		{
			if (culture <= 0)
			{
				throw new ArgumentOutOfRangeException("culture", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			CultureInfo cultureInfoHelper = CultureInfo.GetCultureInfoHelper(culture, null, null);
			if (cultureInfoHelper == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CultureNotSupported", new object[] { culture }), "culture");
			}
			return cultureInfoHelper;
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x0005E1C8 File Offset: 0x0005D1C8
		public static CultureInfo GetCultureInfo(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			CultureInfo cultureInfoHelper = CultureInfo.GetCultureInfoHelper(0, name, null);
			if (cultureInfoHelper == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidCultureName"), new object[] { name }), "name");
			}
			return cultureInfoHelper;
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x0005E21C File Offset: 0x0005D21C
		public static CultureInfo GetCultureInfo(string name, string altName)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (altName == null)
			{
				throw new ArgumentNullException("altName");
			}
			CultureInfo cultureInfoHelper = CultureInfo.GetCultureInfoHelper(-1, name, altName);
			if (cultureInfoHelper == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_OneOfCulturesNotSupported"), new object[] { name, altName }), "name");
			}
			return cultureInfoHelper;
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0005E284 File Offset: 0x0005D284
		public static CultureInfo GetCultureInfoByIetfLanguageTag(string name)
		{
			if ("zh-CHT".Equals(name, StringComparison.OrdinalIgnoreCase) || "zh-CHS".Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_CultureIetfNotSupported"), new object[] { name }), "name");
			}
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo(name);
			if (CultureInfo.GetSortID(cultureInfo.cultureID) != 0 || cultureInfo.cultureID == 1034)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_CultureIetfNotSupported"), new object[] { name }), "name");
			}
			return cultureInfo;
		}

		// Token: 0x04000F21 RID: 3873
		internal const int zh_CHT_CultureID = 31748;

		// Token: 0x04000F22 RID: 3874
		internal const int zh_CHS_CultureID = 4;

		// Token: 0x04000F23 RID: 3875
		internal const int sr_CultureID = 31770;

		// Token: 0x04000F24 RID: 3876
		internal const int sr_SP_Latn_CultureID = 2074;

		// Token: 0x04000F25 RID: 3877
		internal const int LOCALE_INVARIANT = 127;

		// Token: 0x04000F26 RID: 3878
		private const int LOCALE_NEUTRAL = 0;

		// Token: 0x04000F27 RID: 3879
		internal const int LOCALE_USER_DEFAULT = 1024;

		// Token: 0x04000F28 RID: 3880
		internal const int LOCALE_SYSTEM_DEFAULT = 2048;

		// Token: 0x04000F29 RID: 3881
		internal const int LOCALE_CUSTOM_DEFAULT = 3072;

		// Token: 0x04000F2A RID: 3882
		internal const int LOCALE_CUSTOM_UNSPECIFIED = 4096;

		// Token: 0x04000F2B RID: 3883
		internal const int LOCALE_TRADITIONAL_SPANISH = 1034;

		// Token: 0x04000F2C RID: 3884
		internal const int LCID_INSTALLED = 1;

		// Token: 0x04000F2D RID: 3885
		internal const int LCID_SUPPORTED = 2;

		// Token: 0x04000F2E RID: 3886
		internal int cultureID;

		// Token: 0x04000F2F RID: 3887
		internal bool m_isReadOnly;

		// Token: 0x04000F30 RID: 3888
		internal CompareInfo compareInfo;

		// Token: 0x04000F31 RID: 3889
		internal TextInfo textInfo;

		// Token: 0x04000F32 RID: 3890
		internal NumberFormatInfo numInfo;

		// Token: 0x04000F33 RID: 3891
		internal DateTimeFormatInfo dateTimeInfo;

		// Token: 0x04000F34 RID: 3892
		internal Calendar calendar;

		// Token: 0x04000F35 RID: 3893
		[NonSerialized]
		internal CultureTableRecord m_cultureTableRecord;

		// Token: 0x04000F36 RID: 3894
		[NonSerialized]
		internal bool m_isInherited;

		// Token: 0x04000F37 RID: 3895
		[NonSerialized]
		private bool m_isSafeCrossDomain;

		// Token: 0x04000F38 RID: 3896
		[NonSerialized]
		private int m_createdDomainID;

		// Token: 0x04000F39 RID: 3897
		[NonSerialized]
		private CultureInfo m_consoleFallbackCulture;

		// Token: 0x04000F3A RID: 3898
		internal string m_name;

		// Token: 0x04000F3B RID: 3899
		[NonSerialized]
		private string m_nonSortName;

		// Token: 0x04000F3C RID: 3900
		[NonSerialized]
		private string m_sortName;

		// Token: 0x04000F3D RID: 3901
		private static CultureInfo m_userDefaultCulture;

		// Token: 0x04000F3E RID: 3902
		private static CultureInfo m_InvariantCultureInfo;

		// Token: 0x04000F3F RID: 3903
		private static CultureInfo m_userDefaultUICulture;

		// Token: 0x04000F40 RID: 3904
		private static CultureInfo m_InstalledUICultureInfo;

		// Token: 0x04000F41 RID: 3905
		private static Hashtable m_LcidCachedCultures;

		// Token: 0x04000F42 RID: 3906
		private static Hashtable m_NameCachedCultures;

		// Token: 0x04000F43 RID: 3907
		[NonSerialized]
		private CultureInfo m_parent;

		// Token: 0x04000F44 RID: 3908
		private int m_dataItem;

		// Token: 0x04000F45 RID: 3909
		private bool m_useUserOverride;
	}
}
