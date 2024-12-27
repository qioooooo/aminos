using System;
using System.Data.ProviderBase;

namespace System.Data.Odbc
{
	// Token: 0x020001DF RID: 479
	internal sealed class OdbcConnectionPoolGroupProviderInfo : DbConnectionPoolGroupProviderInfo
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x002441FC File Offset: 0x002435FC
		// (set) Token: 0x06001AC1 RID: 6849 RVA: 0x00244210 File Offset: 0x00243610
		internal string DriverName
		{
			get
			{
				return this._driverName;
			}
			set
			{
				this._driverName = value;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x00244224 File Offset: 0x00243624
		// (set) Token: 0x06001AC3 RID: 6851 RVA: 0x00244238 File Offset: 0x00243638
		internal string DriverVersion
		{
			get
			{
				return this._driverVersion;
			}
			set
			{
				this._driverVersion = value;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x0024424C File Offset: 0x0024364C
		internal bool HasQuoteChar
		{
			get
			{
				return this._hasQuoteChar;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x00244260 File Offset: 0x00243660
		internal bool HasEscapeChar
		{
			get
			{
				return this._hasEscapeChar;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001AC6 RID: 6854 RVA: 0x00244274 File Offset: 0x00243674
		// (set) Token: 0x06001AC7 RID: 6855 RVA: 0x00244288 File Offset: 0x00243688
		internal string QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			set
			{
				this._quoteChar = value;
				this._hasQuoteChar = true;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001AC8 RID: 6856 RVA: 0x002442A4 File Offset: 0x002436A4
		// (set) Token: 0x06001AC9 RID: 6857 RVA: 0x002442B8 File Offset: 0x002436B8
		internal char EscapeChar
		{
			get
			{
				return this._escapeChar;
			}
			set
			{
				this._escapeChar = value;
				this._hasEscapeChar = true;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001ACA RID: 6858 RVA: 0x002442D4 File Offset: 0x002436D4
		// (set) Token: 0x06001ACB RID: 6859 RVA: 0x002442E8 File Offset: 0x002436E8
		internal bool IsV3Driver
		{
			get
			{
				return this._isV3Driver;
			}
			set
			{
				this._isV3Driver = value;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001ACC RID: 6860 RVA: 0x002442FC File Offset: 0x002436FC
		// (set) Token: 0x06001ACD RID: 6861 RVA: 0x00244310 File Offset: 0x00243710
		internal int SupportedSQLTypes
		{
			get
			{
				return this._supportedSQLTypes;
			}
			set
			{
				this._supportedSQLTypes = value;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001ACE RID: 6862 RVA: 0x00244324 File Offset: 0x00243724
		// (set) Token: 0x06001ACF RID: 6863 RVA: 0x00244338 File Offset: 0x00243738
		internal int TestedSQLTypes
		{
			get
			{
				return this._testedSQLTypes;
			}
			set
			{
				this._testedSQLTypes = value;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001AD0 RID: 6864 RVA: 0x0024434C File Offset: 0x0024374C
		// (set) Token: 0x06001AD1 RID: 6865 RVA: 0x00244360 File Offset: 0x00243760
		internal int RestrictedSQLBindTypes
		{
			get
			{
				return this._restrictedSQLBindTypes;
			}
			set
			{
				this._restrictedSQLBindTypes = value;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001AD2 RID: 6866 RVA: 0x00244374 File Offset: 0x00243774
		// (set) Token: 0x06001AD3 RID: 6867 RVA: 0x00244388 File Offset: 0x00243788
		internal bool NoCurrentCatalog
		{
			get
			{
				return this._noCurrentCatalog;
			}
			set
			{
				this._noCurrentCatalog = value;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001AD4 RID: 6868 RVA: 0x0024439C File Offset: 0x0024379C
		// (set) Token: 0x06001AD5 RID: 6869 RVA: 0x002443B0 File Offset: 0x002437B0
		internal bool NoConnectionDead
		{
			get
			{
				return this._noConnectionDead;
			}
			set
			{
				this._noConnectionDead = value;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x002443C4 File Offset: 0x002437C4
		// (set) Token: 0x06001AD7 RID: 6871 RVA: 0x002443D8 File Offset: 0x002437D8
		internal bool NoQueryTimeout
		{
			get
			{
				return this._noQueryTimeout;
			}
			set
			{
				this._noQueryTimeout = value;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x002443EC File Offset: 0x002437EC
		// (set) Token: 0x06001AD9 RID: 6873 RVA: 0x00244400 File Offset: 0x00243800
		internal bool NoSqlSoptSSNoBrowseTable
		{
			get
			{
				return this._noSqlSoptSSNoBrowseTable;
			}
			set
			{
				this._noSqlSoptSSNoBrowseTable = value;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x00244414 File Offset: 0x00243814
		// (set) Token: 0x06001ADB RID: 6875 RVA: 0x00244428 File Offset: 0x00243828
		internal bool NoSqlSoptSSHiddenColumns
		{
			get
			{
				return this._noSqlSoptSSHiddenColumns;
			}
			set
			{
				this._noSqlSoptSSHiddenColumns = value;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001ADC RID: 6876 RVA: 0x0024443C File Offset: 0x0024383C
		// (set) Token: 0x06001ADD RID: 6877 RVA: 0x00244450 File Offset: 0x00243850
		internal bool NoSqlCASSColumnKey
		{
			get
			{
				return this._noSqlCASSColumnKey;
			}
			set
			{
				this._noSqlCASSColumnKey = value;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x00244464 File Offset: 0x00243864
		// (set) Token: 0x06001ADF RID: 6879 RVA: 0x00244478 File Offset: 0x00243878
		internal bool NoSqlPrimaryKeys
		{
			get
			{
				return this._noSqlPrimaryKeys;
			}
			set
			{
				this._noSqlPrimaryKeys = value;
			}
		}

		// Token: 0x04000FB6 RID: 4022
		private string _driverName;

		// Token: 0x04000FB7 RID: 4023
		private string _driverVersion;

		// Token: 0x04000FB8 RID: 4024
		private string _quoteChar;

		// Token: 0x04000FB9 RID: 4025
		private char _escapeChar;

		// Token: 0x04000FBA RID: 4026
		private bool _hasQuoteChar;

		// Token: 0x04000FBB RID: 4027
		private bool _hasEscapeChar;

		// Token: 0x04000FBC RID: 4028
		private bool _isV3Driver;

		// Token: 0x04000FBD RID: 4029
		private int _supportedSQLTypes;

		// Token: 0x04000FBE RID: 4030
		private int _testedSQLTypes;

		// Token: 0x04000FBF RID: 4031
		private int _restrictedSQLBindTypes;

		// Token: 0x04000FC0 RID: 4032
		private bool _noCurrentCatalog;

		// Token: 0x04000FC1 RID: 4033
		private bool _noConnectionDead;

		// Token: 0x04000FC2 RID: 4034
		private bool _noQueryTimeout;

		// Token: 0x04000FC3 RID: 4035
		private bool _noSqlSoptSSNoBrowseTable;

		// Token: 0x04000FC4 RID: 4036
		private bool _noSqlSoptSSHiddenColumns;

		// Token: 0x04000FC5 RID: 4037
		private bool _noSqlCASSColumnKey;

		// Token: 0x04000FC6 RID: 4038
		private bool _noSqlPrimaryKeys;
	}
}
