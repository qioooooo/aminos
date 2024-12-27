using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Globalization
{
	// Token: 0x020003AE RID: 942
	[ComVisible(true)]
	[Serializable]
	public class RegionInfo
	{
		// Token: 0x060026BC RID: 9916 RVA: 0x000748EC File Offset: 0x000738EC
		public RegionInfo(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidRegionName", new object[] { name }), "name");
			}
			this.m_name = name.ToUpper(CultureInfo.InvariantCulture);
			this.m_cultureId = 0;
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecordForRegion(name, true);
			if (this.m_cultureTableRecord.IsNeutralCulture)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNeutralRegionName", new object[] { name }), "name");
			}
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x0007498C File Offset: 0x0007398C
		public RegionInfo(int culture)
		{
			if (culture == 127)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NoRegionInvariantCulture"));
			}
			if (CultureTableRecord.IsCustomCultureId(culture))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CustomCultureCannotBePassedByNumber", new object[] { "culture" }));
			}
			if (CultureInfo.GetSubLangID(culture) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CultureIsNeutral", new object[] { culture }), "culture");
			}
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(culture, true);
			if (this.m_cultureTableRecord.IsNeutralCulture)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_CultureIsNeutral", new object[] { culture }), "culture");
			}
			this.m_name = this.m_cultureTableRecord.SREGIONNAME;
			this.m_cultureId = culture;
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x00074A62 File Offset: 0x00073A62
		internal RegionInfo(CultureTableRecord table)
		{
			this.m_cultureTableRecord = table;
			this.m_name = this.m_cultureTableRecord.SREGIONNAME;
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x00074A84 File Offset: 0x00073A84
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_name == null)
			{
				this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(CultureTableRecord.IdFromEverettRegionInfoDataItem(this.m_dataItem), true);
				this.m_name = this.m_cultureTableRecord.SREGIONNAME;
				return;
			}
			if (this.m_cultureId != 0)
			{
				this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecord(this.m_cultureId, true);
				return;
			}
			this.m_cultureTableRecord = CultureTableRecord.GetCultureTableRecordForRegion(this.m_name, true);
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x00074AEF File Offset: 0x00073AEF
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.m_dataItem = this.m_cultureTableRecord.EverettRegionDataItem();
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060026C1 RID: 9921 RVA: 0x00074B04 File Offset: 0x00073B04
		public static RegionInfo CurrentRegion
		{
			get
			{
				RegionInfo regionInfo = RegionInfo.m_currentRegionInfo;
				if (regionInfo == null)
				{
					regionInfo = new RegionInfo(CultureInfo.CurrentCulture.m_cultureTableRecord);
					if (regionInfo.m_cultureTableRecord.IsCustomCulture)
					{
						regionInfo.m_name = regionInfo.m_cultureTableRecord.SNAME;
					}
					RegionInfo.m_currentRegionInfo = regionInfo;
				}
				return regionInfo;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x00074B4F File Offset: 0x00073B4F
		public virtual string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = this.m_cultureTableRecord.SREGIONNAME;
				}
				return this.m_name;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060026C3 RID: 9923 RVA: 0x00074B70 File Offset: 0x00073B70
		public virtual string EnglishName
		{
			get
			{
				return this.m_cultureTableRecord.SENGCOUNTRY;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060026C4 RID: 9924 RVA: 0x00074B80 File Offset: 0x00073B80
		public virtual string DisplayName
		{
			get
			{
				if (this.m_cultureTableRecord.IsCustomCulture)
				{
					if (!this.m_cultureTableRecord.IsReplacementCulture)
					{
						return this.m_cultureTableRecord.SNATIVECOUNTRY;
					}
					if (this.m_cultureTableRecord.IsSynthetic)
					{
						return this.m_cultureTableRecord.RegionNativeDisplayName;
					}
					return Environment.GetResourceString("Globalization.ri_" + this.m_cultureTableRecord.SREGIONNAME);
				}
				else
				{
					if (this.m_cultureTableRecord.IsSynthetic)
					{
						return this.m_cultureTableRecord.RegionNativeDisplayName;
					}
					return Environment.GetResourceString("Globalization.ri_" + this.m_cultureTableRecord.SREGIONNAME);
				}
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060026C5 RID: 9925 RVA: 0x00074C1A File Offset: 0x00073C1A
		[ComVisible(false)]
		public virtual string NativeName
		{
			get
			{
				return this.m_cultureTableRecord.SNATIVECOUNTRY;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060026C6 RID: 9926 RVA: 0x00074C27 File Offset: 0x00073C27
		public virtual string TwoLetterISORegionName
		{
			get
			{
				return this.m_cultureTableRecord.SISO3166CTRYNAME;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060026C7 RID: 9927 RVA: 0x00074C34 File Offset: 0x00073C34
		public virtual string ThreeLetterISORegionName
		{
			get
			{
				return this.m_cultureTableRecord.SISO3166CTRYNAME2;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x00074C44 File Offset: 0x00073C44
		public virtual bool IsMetric
		{
			get
			{
				int imeasure = (int)this.m_cultureTableRecord.IMEASURE;
				return imeasure == 0;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060026C9 RID: 9929 RVA: 0x00074C61 File Offset: 0x00073C61
		[ComVisible(false)]
		public virtual int GeoId
		{
			get
			{
				return (int)this.m_cultureTableRecord.IGEOID;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060026CA RID: 9930 RVA: 0x00074C6E File Offset: 0x00073C6E
		public virtual string ThreeLetterWindowsRegionName
		{
			get
			{
				return this.m_cultureTableRecord.SABBREVCTRYNAME;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060026CB RID: 9931 RVA: 0x00074C7B File Offset: 0x00073C7B
		[ComVisible(false)]
		public virtual string CurrencyEnglishName
		{
			get
			{
				return this.m_cultureTableRecord.SENGLISHCURRENCY;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x00074C88 File Offset: 0x00073C88
		[ComVisible(false)]
		public virtual string CurrencyNativeName
		{
			get
			{
				return this.m_cultureTableRecord.SNATIVECURRENCY;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060026CD RID: 9933 RVA: 0x00074C95 File Offset: 0x00073C95
		public virtual string CurrencySymbol
		{
			get
			{
				return this.m_cultureTableRecord.SCURRENCY;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x00074CA2 File Offset: 0x00073CA2
		public virtual string ISOCurrencySymbol
		{
			get
			{
				return this.m_cultureTableRecord.SINTLSYMBOL;
			}
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x00074CB0 File Offset: 0x00073CB0
		public override bool Equals(object value)
		{
			RegionInfo regionInfo = value as RegionInfo;
			return regionInfo != null && this.Name.Equals(regionInfo.Name);
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x00074CDA File Offset: 0x00073CDA
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x00074CE7 File Offset: 0x00073CE7
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04001182 RID: 4482
		internal string m_name;

		// Token: 0x04001183 RID: 4483
		[OptionalField(VersionAdded = 2)]
		private int m_cultureId;

		// Token: 0x04001184 RID: 4484
		[NonSerialized]
		internal CultureTableRecord m_cultureTableRecord;

		// Token: 0x04001185 RID: 4485
		internal static RegionInfo m_currentRegionInfo;

		// Token: 0x04001186 RID: 4486
		internal int m_dataItem;
	}
}
