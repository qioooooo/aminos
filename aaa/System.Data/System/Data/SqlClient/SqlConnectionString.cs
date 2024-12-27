using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Security;
using System.Security.Permissions;

namespace System.Data.SqlClient
{
	// Token: 0x020002D4 RID: 724
	internal sealed class SqlConnectionString : DbConnectionOptions
	{
		// Token: 0x06002515 RID: 9493 RVA: 0x0027A60C File Offset: 0x00279A0C
		internal SqlConnectionString(string connectionString)
			: base(connectionString, SqlConnectionString.GetParseSynonyms(), false)
		{
			bool inProc = InOutOfProcHelper.InProc;
			this._integratedSecurity = base.ConvertValueToIntegratedSecurity();
			this._async = base.ConvertValueToBoolean("asynchronous processing", false);
			this._connectionReset = base.ConvertValueToBoolean("connection reset", true);
			this._contextConnection = base.ConvertValueToBoolean("context connection", false);
			this._encrypt = base.ConvertValueToBoolean("encrypt", false);
			this._enlist = base.ConvertValueToBoolean("enlist", ADP.IsWindowsNT);
			this._mars = base.ConvertValueToBoolean("multipleactiveresultsets", false);
			this._persistSecurityInfo = base.ConvertValueToBoolean("persist security info", false);
			this._pooling = base.ConvertValueToBoolean("pooling", true);
			this._replication = base.ConvertValueToBoolean("replication", false);
			this._userInstance = base.ConvertValueToBoolean("user instance", false);
			this._multiSubnetFailover = base.ConvertValueToBoolean("multisubnetfailover", false);
			this._connectTimeout = base.ConvertValueToInt32("connect timeout", 15);
			this._loadBalanceTimeout = base.ConvertValueToInt32("load balance timeout", 0);
			this._maxPoolSize = base.ConvertValueToInt32("max pool size", 100);
			this._minPoolSize = base.ConvertValueToInt32("min pool size", 0);
			this._packetSize = base.ConvertValueToInt32("packet size", 8000);
			this._applicationIntent = this.ConvertValueToApplicationIntent();
			this._applicationName = base.ConvertValueToString("application name", ".Net SqlClient Data Provider");
			this._attachDBFileName = base.ConvertValueToString("attachdbfilename", "");
			this._currentLanguage = base.ConvertValueToString("current language", "");
			this._dataSource = base.ConvertValueToString("data source", "");
			this._localDBInstance = LocalDBAPI.GetLocalDbInstanceNameFromServerName(this._dataSource);
			this._failoverPartner = base.ConvertValueToString("failover partner", "");
			this._initialCatalog = base.ConvertValueToString("initial catalog", "");
			this._networkLibrary = base.ConvertValueToString("network library", null);
			this._password = base.ConvertValueToString("password", "");
			this._trustServerCertificate = base.ConvertValueToBoolean("trustservercertificate", false);
			string text = base.ConvertValueToString("type system version", null);
			string text2 = base.ConvertValueToString("transaction binding", null);
			this._userID = base.ConvertValueToString("user id", "");
			this._workstationId = base.ConvertValueToString("workstation id", null);
			if (this._contextConnection)
			{
				if (!inProc)
				{
					throw SQL.ContextUnavailableOutOfProc();
				}
				foreach (object obj in base.Parsetable)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if ((string)dictionaryEntry.Key != "context connection" && (string)dictionaryEntry.Key != "type system version")
					{
						throw SQL.ContextAllowsLimitedKeywords();
					}
				}
			}
			if (!this._encrypt)
			{
				object obj2 = ADP.LocalMachineRegistryValue("Software\\Microsoft\\MSSQLServer\\Client\\SuperSocketNetLib", "Encrypt");
				if (obj2 is int && 1 == (int)obj2)
				{
					this._encrypt = true;
				}
			}
			if (this._loadBalanceTimeout < 0)
			{
				throw ADP.InvalidConnectionOptionValue("load balance timeout");
			}
			if (this._connectTimeout < 0)
			{
				throw ADP.InvalidConnectionOptionValue("connect timeout");
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
			if (this._packetSize < 512 || 32768 < this._packetSize)
			{
				throw SQL.InvalidPacketSizeValue();
			}
			if (this._networkLibrary != null)
			{
				string text3 = this._networkLibrary.Trim().ToLower(CultureInfo.InvariantCulture);
				Hashtable hashtable = SqlConnectionString.NetlibMapping();
				if (!hashtable.ContainsKey(text3))
				{
					throw ADP.InvalidConnectionOptionValue("network library");
				}
				this._networkLibrary = (string)hashtable[text3];
			}
			else
			{
				this._networkLibrary = "";
			}
			this.ValidateValueLength(this._applicationName, 128, "application name");
			this.ValidateValueLength(this._currentLanguage, 128, "current language");
			this.ValidateValueLength(this._dataSource, 128, "data source");
			this.ValidateValueLength(this._failoverPartner, 128, "failover partner");
			this.ValidateValueLength(this._initialCatalog, 128, "initial catalog");
			this.ValidateValueLength(this._password, 128, "password");
			this.ValidateValueLength(this._userID, 128, "user id");
			if (this._workstationId != null)
			{
				this.ValidateValueLength(this._workstationId, 128, "workstation id");
			}
			if (!string.Equals("", this._failoverPartner, StringComparison.OrdinalIgnoreCase))
			{
				if (this._multiSubnetFailover)
				{
					throw SQL.MultiSubnetFailoverWithFailoverPartner(false);
				}
				if (string.Equals("", this._initialCatalog, StringComparison.OrdinalIgnoreCase))
				{
					throw ADP.MissingConnectionOptionValue("failover partner", "initial catalog");
				}
			}
			string text4 = null;
			this._expandedAttachDBFilename = DbConnectionOptions.ExpandDataDirectory("attachdbfilename", this._attachDBFileName, ref text4);
			if (this._expandedAttachDBFilename != null)
			{
				if (0 <= this._expandedAttachDBFilename.IndexOf('|'))
				{
					throw ADP.InvalidConnectionOptionValue("attachdbfilename");
				}
				this.ValidateValueLength(this._expandedAttachDBFilename, 260, "attachdbfilename");
				if (this._localDBInstance == null)
				{
					string dataSource = this._dataSource;
					string networkLibrary = this._networkLibrary;
					TdsParserStaticMethods.AliasRegistryLookup(ref dataSource, ref networkLibrary);
					SqlConnectionString.VerifyLocalHostAndFixup(ref dataSource, true, false);
				}
			}
			else
			{
				if (0 <= this._attachDBFileName.IndexOf('|'))
				{
					throw ADP.InvalidConnectionOptionValue("attachdbfilename");
				}
				this.ValidateValueLength(this._attachDBFileName, 260, "attachdbfilename");
			}
			if (this._async && inProc)
			{
				throw SQL.AsyncInProcNotSupported();
			}
			if (this._userInstance && !ADP.IsEmpty(this._failoverPartner))
			{
				throw SQL.UserInstanceFailoverNotCompatible();
			}
			if (ADP.IsEmpty(text))
			{
				text = "Latest";
			}
			if (text.Equals("Latest", StringComparison.OrdinalIgnoreCase))
			{
				this._typeSystemVersion = SqlConnectionString.TypeSystem.Latest;
			}
			else if (text.Equals("SQL Server 2000", StringComparison.OrdinalIgnoreCase))
			{
				if (this._contextConnection)
				{
					throw SQL.ContextAllowsOnlyTypeSystem2005();
				}
				this._typeSystemVersion = SqlConnectionString.TypeSystem.SQLServer2000;
			}
			else if (text.Equals("SQL Server 2005", StringComparison.OrdinalIgnoreCase))
			{
				this._typeSystemVersion = SqlConnectionString.TypeSystem.SQLServer2005;
			}
			else
			{
				if (!text.Equals("SQL Server 2008", StringComparison.OrdinalIgnoreCase))
				{
					throw ADP.InvalidConnectionOptionValue("type system version");
				}
				this._typeSystemVersion = SqlConnectionString.TypeSystem.Latest;
			}
			if (ADP.IsEmpty(text2))
			{
				text2 = "Implicit Unbind";
			}
			if (text2.Equals("Implicit Unbind", StringComparison.OrdinalIgnoreCase))
			{
				this._transactionBinding = SqlConnectionString.TransactionBindingEnum.ImplicitUnbind;
			}
			else
			{
				if (!text2.Equals("Explicit Unbind", StringComparison.OrdinalIgnoreCase))
				{
					throw ADP.InvalidConnectionOptionValue("transaction binding");
				}
				this._transactionBinding = SqlConnectionString.TransactionBindingEnum.ExplicitUnbind;
			}
			if (this._applicationIntent == ApplicationIntent.ReadOnly && !string.IsNullOrEmpty(this._failoverPartner))
			{
				throw SQL.ROR_FailoverNotSupportedConnString();
			}
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x0027AD04 File Offset: 0x0027A104
		internal SqlConnectionString(SqlConnectionString connectionOptions, bool userInstance, string dataSource)
			: base(connectionOptions)
		{
			this._integratedSecurity = connectionOptions._integratedSecurity;
			this._async = connectionOptions._async;
			this._connectionReset = connectionOptions._connectionReset;
			this._contextConnection = connectionOptions._contextConnection;
			this._encrypt = connectionOptions._encrypt;
			this._enlist = connectionOptions._enlist;
			this._mars = connectionOptions._mars;
			this._persistSecurityInfo = connectionOptions._persistSecurityInfo;
			this._pooling = connectionOptions._pooling;
			this._replication = connectionOptions._replication;
			this._userInstance = userInstance;
			this._connectTimeout = connectionOptions._connectTimeout;
			this._loadBalanceTimeout = connectionOptions._loadBalanceTimeout;
			this._maxPoolSize = connectionOptions._maxPoolSize;
			this._minPoolSize = connectionOptions._minPoolSize;
			this._multiSubnetFailover = connectionOptions._multiSubnetFailover;
			this._packetSize = connectionOptions._packetSize;
			this._applicationName = connectionOptions._applicationName;
			this._attachDBFileName = connectionOptions._attachDBFileName;
			this._currentLanguage = connectionOptions._currentLanguage;
			this._dataSource = dataSource;
			this._localDBInstance = LocalDBAPI.GetLocalDbInstanceNameFromServerName(this._dataSource);
			this._failoverPartner = connectionOptions._failoverPartner;
			this._initialCatalog = connectionOptions._initialCatalog;
			this._password = connectionOptions._password;
			this._userID = connectionOptions._userID;
			this._networkLibrary = connectionOptions._networkLibrary;
			this._workstationId = connectionOptions._workstationId;
			this._expandedAttachDBFilename = connectionOptions._expandedAttachDBFilename;
			this._typeSystemVersion = connectionOptions._typeSystemVersion;
			this._transactionBinding = connectionOptions._transactionBinding;
			this._applicationIntent = connectionOptions._applicationIntent;
			this.ValidateValueLength(this._dataSource, 128, "data source");
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06002517 RID: 9495 RVA: 0x0027AEAC File Offset: 0x0027A2AC
		internal bool IntegratedSecurity
		{
			get
			{
				return this._integratedSecurity;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06002518 RID: 9496 RVA: 0x0027AEC0 File Offset: 0x0027A2C0
		internal bool Asynchronous
		{
			get
			{
				return this._async;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x0027AED4 File Offset: 0x0027A2D4
		internal bool ConnectionReset
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x0600251A RID: 9498 RVA: 0x0027AEE4 File Offset: 0x0027A2E4
		internal bool ContextConnection
		{
			get
			{
				return this._contextConnection;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x0027AEF8 File Offset: 0x0027A2F8
		internal bool Encrypt
		{
			get
			{
				return this._encrypt;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x0027AF0C File Offset: 0x0027A30C
		internal bool TrustServerCertificate
		{
			get
			{
				return this._trustServerCertificate;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x0600251D RID: 9501 RVA: 0x0027AF20 File Offset: 0x0027A320
		internal bool Enlist
		{
			get
			{
				return this._enlist;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x0027AF34 File Offset: 0x0027A334
		internal bool MARS
		{
			get
			{
				return this._mars;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600251F RID: 9503 RVA: 0x0027AF48 File Offset: 0x0027A348
		internal bool MultiSubnetFailover
		{
			get
			{
				return this._multiSubnetFailover;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x0027AF5C File Offset: 0x0027A35C
		internal bool Pooling
		{
			get
			{
				return this._pooling;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x0027AF70 File Offset: 0x0027A370
		internal bool Replication
		{
			get
			{
				return this._replication;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002522 RID: 9506 RVA: 0x0027AF84 File Offset: 0x0027A384
		internal bool UserInstance
		{
			get
			{
				return this._userInstance;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x0027AF98 File Offset: 0x0027A398
		internal int ConnectTimeout
		{
			get
			{
				return this._connectTimeout;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x0027AFAC File Offset: 0x0027A3AC
		internal int LoadBalanceTimeout
		{
			get
			{
				return this._loadBalanceTimeout;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06002525 RID: 9509 RVA: 0x0027AFC0 File Offset: 0x0027A3C0
		internal int MaxPoolSize
		{
			get
			{
				return this._maxPoolSize;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x0027AFD4 File Offset: 0x0027A3D4
		internal int MinPoolSize
		{
			get
			{
				return this._minPoolSize;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x0027AFE8 File Offset: 0x0027A3E8
		internal int PacketSize
		{
			get
			{
				return this._packetSize;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06002528 RID: 9512 RVA: 0x0027AFFC File Offset: 0x0027A3FC
		internal ApplicationIntent ApplicationIntent
		{
			get
			{
				return this._applicationIntent;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002529 RID: 9513 RVA: 0x0027B010 File Offset: 0x0027A410
		internal string ApplicationName
		{
			get
			{
				return this._applicationName;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x0600252A RID: 9514 RVA: 0x0027B024 File Offset: 0x0027A424
		internal string AttachDBFilename
		{
			get
			{
				return this._attachDBFileName;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x0600252B RID: 9515 RVA: 0x0027B038 File Offset: 0x0027A438
		internal string CurrentLanguage
		{
			get
			{
				return this._currentLanguage;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x0600252C RID: 9516 RVA: 0x0027B04C File Offset: 0x0027A44C
		internal string DataSource
		{
			get
			{
				return this._dataSource;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x0600252D RID: 9517 RVA: 0x0027B060 File Offset: 0x0027A460
		internal string LocalDBInstance
		{
			get
			{
				return this._localDBInstance;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x0600252E RID: 9518 RVA: 0x0027B074 File Offset: 0x0027A474
		internal string FailoverPartner
		{
			get
			{
				return this._failoverPartner;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x0600252F RID: 9519 RVA: 0x0027B088 File Offset: 0x0027A488
		internal string InitialCatalog
		{
			get
			{
				return this._initialCatalog;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002530 RID: 9520 RVA: 0x0027B09C File Offset: 0x0027A49C
		internal string NetworkLibrary
		{
			get
			{
				return this._networkLibrary;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06002531 RID: 9521 RVA: 0x0027B0B0 File Offset: 0x0027A4B0
		internal string Password
		{
			get
			{
				return this._password;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x0027B0C4 File Offset: 0x0027A4C4
		internal string UserID
		{
			get
			{
				return this._userID;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06002533 RID: 9523 RVA: 0x0027B0D8 File Offset: 0x0027A4D8
		internal string WorkstationId
		{
			get
			{
				return this._workstationId;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06002534 RID: 9524 RVA: 0x0027B0EC File Offset: 0x0027A4EC
		internal SqlConnectionString.TypeSystem TypeSystemVersion
		{
			get
			{
				return this._typeSystemVersion;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06002535 RID: 9525 RVA: 0x0027B100 File Offset: 0x0027A500
		internal SqlConnectionString.TransactionBindingEnum TransactionBinding
		{
			get
			{
				return this._transactionBinding;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06002536 RID: 9526 RVA: 0x0027B114 File Offset: 0x0027A514
		internal bool EnforceLocalHost
		{
			get
			{
				return this._expandedAttachDBFilename != null && null == this._localDBInstance;
			}
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x0027B134 File Offset: 0x0027A534
		protected internal override PermissionSet CreatePermissionSet()
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new SqlClientPermission(this));
			return permissionSet;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x0027B158 File Offset: 0x0027A558
		protected internal override string Expand()
		{
			if (this._expandedAttachDBFilename != null)
			{
				return base.ExpandKeyword("attachdbfilename", this._expandedAttachDBFilename);
			}
			return base.Expand();
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x0027B188 File Offset: 0x0027A588
		private static bool CompareHostName(ref string host, string name, bool fixup)
		{
			bool flag = false;
			if (host.Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				if (fixup)
				{
					host = ".";
				}
				flag = true;
			}
			else if (host.StartsWith(name + "\\", StringComparison.OrdinalIgnoreCase))
			{
				if (fixup)
				{
					host = "." + host.Substring(name.Length);
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x0027B1E4 File Offset: 0x0027A5E4
		internal static Hashtable GetParseSynonyms()
		{
			Hashtable hashtable = SqlConnectionString._sqlClientSynonyms;
			if (hashtable == null)
			{
				hashtable = new Hashtable(52);
				hashtable.Add("applicationintent", "applicationintent");
				hashtable.Add("application name", "application name");
				hashtable.Add("asynchronous processing", "asynchronous processing");
				hashtable.Add("attachdbfilename", "attachdbfilename");
				hashtable.Add("connect timeout", "connect timeout");
				hashtable.Add("connection reset", "connection reset");
				hashtable.Add("context connection", "context connection");
				hashtable.Add("current language", "current language");
				hashtable.Add("data source", "data source");
				hashtable.Add("encrypt", "encrypt");
				hashtable.Add("enlist", "enlist");
				hashtable.Add("failover partner", "failover partner");
				hashtable.Add("initial catalog", "initial catalog");
				hashtable.Add("integrated security", "integrated security");
				hashtable.Add("load balance timeout", "load balance timeout");
				hashtable.Add("multipleactiveresultsets", "multipleactiveresultsets");
				hashtable.Add("max pool size", "max pool size");
				hashtable.Add("min pool size", "min pool size");
				hashtable.Add("multisubnetfailover", "multisubnetfailover");
				hashtable.Add("network library", "network library");
				hashtable.Add("packet size", "packet size");
				hashtable.Add("password", "password");
				hashtable.Add("persist security info", "persist security info");
				hashtable.Add("pooling", "pooling");
				hashtable.Add("replication", "replication");
				hashtable.Add("trustservercertificate", "trustservercertificate");
				hashtable.Add("transaction binding", "transaction binding");
				hashtable.Add("type system version", "type system version");
				hashtable.Add("user id", "user id");
				hashtable.Add("user instance", "user instance");
				hashtable.Add("workstation id", "workstation id");
				hashtable.Add("app", "application name");
				hashtable.Add("async", "asynchronous processing");
				hashtable.Add("extended properties", "attachdbfilename");
				hashtable.Add("initial file name", "attachdbfilename");
				hashtable.Add("connection timeout", "connect timeout");
				hashtable.Add("timeout", "connect timeout");
				hashtable.Add("language", "current language");
				hashtable.Add("addr", "data source");
				hashtable.Add("address", "data source");
				hashtable.Add("network address", "data source");
				hashtable.Add("server", "data source");
				hashtable.Add("database", "initial catalog");
				hashtable.Add("trusted_connection", "integrated security");
				hashtable.Add("connection lifetime", "load balance timeout");
				hashtable.Add("net", "network library");
				hashtable.Add("network", "network library");
				hashtable.Add("pwd", "password");
				hashtable.Add("persistsecurityinfo", "persist security info");
				hashtable.Add("uid", "user id");
				hashtable.Add("user", "user id");
				hashtable.Add("wsid", "workstation id");
				SqlConnectionString._sqlClientSynonyms = hashtable;
			}
			return hashtable;
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x0027B54C File Offset: 0x0027A94C
		internal string ObtainWorkstationId()
		{
			string text = this.WorkstationId;
			if (text == null)
			{
				text = ADP.MachineName();
				this.ValidateValueLength(text, 128, "workstation id");
			}
			return text;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x0027B57C File Offset: 0x0027A97C
		internal static Hashtable NetlibMapping()
		{
			Hashtable hashtable = SqlConnectionString._netlibMapping;
			if (hashtable == null)
			{
				hashtable = new Hashtable(8);
				hashtable.Add("dbmssocn", "tcp");
				hashtable.Add("dbnmpntw", "np");
				hashtable.Add("dbmsrpcn", "rpc");
				hashtable.Add("dbmsvinn", "bv");
				hashtable.Add("dbmsadsn", "adsp");
				hashtable.Add("dbmsspxn", "spx");
				hashtable.Add("dbmsgnet", "via");
				hashtable.Add("dbmslpcn", "lpc");
				SqlConnectionString._netlibMapping = hashtable;
			}
			return hashtable;
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x0027B624 File Offset: 0x0027AA24
		internal static bool ValidProtocal(string protocal)
		{
			return protocal != null && (protocal == "tcp" || protocal == "np" || protocal == "via" || protocal == "lpc");
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x0027B670 File Offset: 0x0027AA70
		private void ValidateValueLength(string value, int limit, string key)
		{
			if (limit < value.Length)
			{
				throw ADP.InvalidConnectionOptionValueLength(key, limit);
			}
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x0027B690 File Offset: 0x0027AA90
		internal static void VerifyLocalHostAndFixup(ref string host, bool enforceLocalHost, bool fixup)
		{
			if (ADP.IsEmpty(host))
			{
				if (fixup)
				{
					host = ".";
					return;
				}
			}
			else if (!SqlConnectionString.CompareHostName(ref host, ".", fixup) && !SqlConnectionString.CompareHostName(ref host, "(local)", fixup))
			{
				string computerNameDnsFullyQualified = ADP.GetComputerNameDnsFullyQualified();
				if (!SqlConnectionString.CompareHostName(ref host, computerNameDnsFullyQualified, fixup))
				{
					int num = computerNameDnsFullyQualified.IndexOf('.');
					if ((num <= 0 || !SqlConnectionString.CompareHostName(ref host, computerNameDnsFullyQualified.Substring(0, num), fixup)) && enforceLocalHost)
					{
						throw ADP.InvalidConnectionOptionValue("attachdbfilename");
					}
				}
			}
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x0027B70C File Offset: 0x0027AB0C
		internal ApplicationIntent ConvertValueToApplicationIntent()
		{
			object obj = base.Parsetable["applicationintent"];
			if (obj == null)
			{
				return ApplicationIntent.ReadWrite;
			}
			ApplicationIntent applicationIntent;
			try
			{
				applicationIntent = DbConnectionStringBuilderUtil.ConvertToApplicationIntent("applicationintent", obj);
			}
			catch (FormatException ex)
			{
				throw ADP.InvalidConnectionOptionValue("applicationintent", ex);
			}
			catch (OverflowException ex2)
			{
				throw ADP.InvalidConnectionOptionValue("applicationintent", ex2);
			}
			return applicationIntent;
		}

		// Token: 0x0400178A RID: 6026
		private static Hashtable _sqlClientSynonyms;

		// Token: 0x0400178B RID: 6027
		private static Hashtable _netlibMapping;

		// Token: 0x0400178C RID: 6028
		private readonly bool _integratedSecurity;

		// Token: 0x0400178D RID: 6029
		private readonly bool _async;

		// Token: 0x0400178E RID: 6030
		private readonly bool _connectionReset;

		// Token: 0x0400178F RID: 6031
		private readonly bool _contextConnection;

		// Token: 0x04001790 RID: 6032
		private readonly bool _encrypt;

		// Token: 0x04001791 RID: 6033
		private readonly bool _trustServerCertificate;

		// Token: 0x04001792 RID: 6034
		private readonly bool _enlist;

		// Token: 0x04001793 RID: 6035
		private readonly bool _mars;

		// Token: 0x04001794 RID: 6036
		private readonly bool _persistSecurityInfo;

		// Token: 0x04001795 RID: 6037
		private readonly bool _pooling;

		// Token: 0x04001796 RID: 6038
		private readonly bool _replication;

		// Token: 0x04001797 RID: 6039
		private readonly bool _userInstance;

		// Token: 0x04001798 RID: 6040
		private readonly bool _multiSubnetFailover;

		// Token: 0x04001799 RID: 6041
		private readonly int _connectTimeout;

		// Token: 0x0400179A RID: 6042
		private readonly int _loadBalanceTimeout;

		// Token: 0x0400179B RID: 6043
		private readonly int _maxPoolSize;

		// Token: 0x0400179C RID: 6044
		private readonly int _minPoolSize;

		// Token: 0x0400179D RID: 6045
		private readonly int _packetSize;

		// Token: 0x0400179E RID: 6046
		private readonly ApplicationIntent _applicationIntent;

		// Token: 0x0400179F RID: 6047
		private readonly string _applicationName;

		// Token: 0x040017A0 RID: 6048
		private readonly string _attachDBFileName;

		// Token: 0x040017A1 RID: 6049
		private readonly string _currentLanguage;

		// Token: 0x040017A2 RID: 6050
		private readonly string _dataSource;

		// Token: 0x040017A3 RID: 6051
		private readonly string _localDBInstance;

		// Token: 0x040017A4 RID: 6052
		private readonly string _failoverPartner;

		// Token: 0x040017A5 RID: 6053
		private readonly string _initialCatalog;

		// Token: 0x040017A6 RID: 6054
		private readonly string _password;

		// Token: 0x040017A7 RID: 6055
		private readonly string _userID;

		// Token: 0x040017A8 RID: 6056
		private readonly string _networkLibrary;

		// Token: 0x040017A9 RID: 6057
		private readonly string _workstationId;

		// Token: 0x040017AA RID: 6058
		private readonly SqlConnectionString.TypeSystem _typeSystemVersion;

		// Token: 0x040017AB RID: 6059
		private readonly SqlConnectionString.TransactionBindingEnum _transactionBinding;

		// Token: 0x040017AC RID: 6060
		private readonly string _expandedAttachDBFilename;

		// Token: 0x020002D5 RID: 725
		internal enum TypeSystem
		{
			// Token: 0x040017AE RID: 6062
			Latest = 2008,
			// Token: 0x040017AF RID: 6063
			SQLServer2000 = 2000,
			// Token: 0x040017B0 RID: 6064
			SQLServer2005 = 2005,
			// Token: 0x040017B1 RID: 6065
			SQLServer2008 = 2008
		}

		// Token: 0x020002D6 RID: 726
		internal enum TransactionBindingEnum
		{
			// Token: 0x040017B3 RID: 6067
			ImplicitUnbind,
			// Token: 0x040017B4 RID: 6068
			ExplicitUnbind
		}
	}
}
