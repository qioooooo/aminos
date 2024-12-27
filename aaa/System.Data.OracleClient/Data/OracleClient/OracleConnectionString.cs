using System;
using System.Collections;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.OracleClient
{
	// Token: 0x0200005D RID: 93
	internal sealed class OracleConnectionString : DbConnectionOptions
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x00063254 File Offset: 0x00062654
		public OracleConnectionString(string connectionString)
			: base(connectionString, OracleConnectionString.GetParseSynonyms(), false)
		{
			this._integratedSecurity = base.ConvertValueToIntegratedSecurity();
			this._enlist = base.ConvertValueToBoolean("enlist", ADP.IsWindowsNT);
			this._persistSecurityInfo = base.ConvertValueToBoolean("persist security info", false);
			this._pooling = base.ConvertValueToBoolean("pooling", true);
			this._unicode = base.ConvertValueToBoolean("unicode", false);
			this._omitOracleConnectionName = base.ConvertValueToBoolean("omit oracle connection name", false);
			this._loadBalanceTimeout = base.ConvertValueToInt32("load balance timeout", 0);
			this._maxPoolSize = base.ConvertValueToInt32("max pool size", 100);
			this._minPoolSize = base.ConvertValueToInt32("min pool size", 0);
			this._dataSource = base.ConvertValueToString("data source", "");
			this._userId = base.ConvertValueToString("user id", "");
			this._password = base.ConvertValueToString("password", "");
			if (this._userId.Length > 30)
			{
				throw ADP.InvalidConnectionOptionLength("user id", 30);
			}
			if (this._password.Length > 30)
			{
				throw ADP.InvalidConnectionOptionLength("password", 30);
			}
			if (this._loadBalanceTimeout < 0)
			{
				throw ADP.InvalidConnectionOptionValue("load balance timeout");
			}
			if (this._maxPoolSize < 1)
			{
				throw ADP.InvalidConnectionOptionValue("max pool size");
			}
			if (this._minPoolSize < 0)
			{
				throw ADP.InvalidConnectionOptionValue("min pool size");
			}
			if (this._maxPoolSize < this._minPoolSize)
			{
				throw ADP.InvalidMinMaxPoolSizeValues();
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x000633DC File Offset: 0x000627DC
		internal bool Enlist
		{
			get
			{
				return this._enlist;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003BA RID: 954 RVA: 0x000633F0 File Offset: 0x000627F0
		internal bool IntegratedSecurity
		{
			get
			{
				return this._integratedSecurity;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00063404 File Offset: 0x00062804
		internal bool Pooling
		{
			get
			{
				return this._pooling;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00063418 File Offset: 0x00062818
		internal bool Unicode
		{
			get
			{
				return this._unicode;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0006342C File Offset: 0x0006282C
		internal bool OmitOracleConnectionName
		{
			get
			{
				return this._omitOracleConnectionName;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00063440 File Offset: 0x00062840
		internal int LoadBalanceTimeout
		{
			get
			{
				return this._loadBalanceTimeout;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00063454 File Offset: 0x00062854
		internal int MaxPoolSize
		{
			get
			{
				return this._maxPoolSize;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x00063468 File Offset: 0x00062868
		internal int MinPoolSize
		{
			get
			{
				return this._minPoolSize;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0006347C File Offset: 0x0006287C
		internal string DataSource
		{
			get
			{
				return this._dataSource;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00063490 File Offset: 0x00062890
		internal string UserId
		{
			get
			{
				return this._userId;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x000634A4 File Offset: 0x000628A4
		internal string Password
		{
			get
			{
				return this._password;
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000634B8 File Offset: 0x000628B8
		protected internal override PermissionSet CreatePermissionSet()
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new OraclePermission(this));
			return permissionSet;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000634DC File Offset: 0x000628DC
		internal static Hashtable GetParseSynonyms()
		{
			Hashtable hashtable = OracleConnectionString._validKeyNamesAndSynonyms;
			if (hashtable == null)
			{
				hashtable = new Hashtable(19);
				hashtable.Add("data source", "data source");
				hashtable.Add("enlist", "enlist");
				hashtable.Add("integrated security", "integrated security");
				hashtable.Add("load balance timeout", "load balance timeout");
				hashtable.Add("max pool size", "max pool size");
				hashtable.Add("min pool size", "min pool size");
				hashtable.Add("omit oracle connection name", "omit oracle connection name");
				hashtable.Add("password", "password");
				hashtable.Add("persist security info", "persist security info");
				hashtable.Add("pooling", "pooling");
				hashtable.Add("unicode", "unicode");
				hashtable.Add("user id", "user id");
				hashtable.Add("server", "data source");
				hashtable.Add("pwd", "password");
				hashtable.Add("persistsecurityinfo", "persist security info");
				hashtable.Add("uid", "user id");
				hashtable.Add("user", "user id");
				hashtable.Add("connection lifetime", "load balance timeout");
				hashtable.Add("workaround oracle bug 914652", "omit oracle connection name");
				OracleConnectionString._validKeyNamesAndSynonyms = hashtable;
			}
			return hashtable;
		}

		// Token: 0x040003D7 RID: 983
		private static Hashtable _validKeyNamesAndSynonyms;

		// Token: 0x040003D8 RID: 984
		private readonly bool _enlist;

		// Token: 0x040003D9 RID: 985
		private readonly bool _integratedSecurity;

		// Token: 0x040003DA RID: 986
		private readonly bool _persistSecurityInfo;

		// Token: 0x040003DB RID: 987
		private readonly bool _pooling;

		// Token: 0x040003DC RID: 988
		private readonly bool _unicode;

		// Token: 0x040003DD RID: 989
		private readonly bool _omitOracleConnectionName;

		// Token: 0x040003DE RID: 990
		private readonly int _loadBalanceTimeout;

		// Token: 0x040003DF RID: 991
		private readonly int _maxPoolSize;

		// Token: 0x040003E0 RID: 992
		private readonly int _minPoolSize;

		// Token: 0x040003E1 RID: 993
		private readonly string _dataSource;

		// Token: 0x040003E2 RID: 994
		private readonly string _password;

		// Token: 0x040003E3 RID: 995
		private readonly string _userId;
	}
}
