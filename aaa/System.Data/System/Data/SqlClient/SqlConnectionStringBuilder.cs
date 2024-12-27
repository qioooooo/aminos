using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace System.Data.SqlClient
{
	// Token: 0x020002D7 RID: 727
	[DefaultProperty("DataSource")]
	[TypeConverter(typeof(SqlConnectionStringBuilder.SqlConnectionStringBuilderConverter))]
	public sealed class SqlConnectionStringBuilder : DbConnectionStringBuilder
	{
		// Token: 0x06002541 RID: 9537 RVA: 0x0027B794 File Offset: 0x0027AB94
		static SqlConnectionStringBuilder()
		{
			string[] array = new string[]
			{
				null, null, null, null, null, null, null, null, null, null,
				null, null, null, null, null, null, null, null, null, null,
				null, null, null, null, null, null, null, null, null, null,
				"ApplicationIntent"
			};
			array[23] = "Application Name";
			array[12] = "Asynchronous Processing";
			array[2] = "AttachDbFilename";
			array[13] = "Connection Reset";
			array[27] = "Context Connection";
			array[16] = "Connect Timeout";
			array[24] = "Current Language";
			array[0] = "Data Source";
			array[17] = "Encrypt";
			array[8] = "Enlist";
			array[1] = "Failover Partner";
			array[3] = "Initial Catalog";
			array[4] = "Integrated Security";
			array[19] = "Load Balance Timeout";
			array[11] = "Max Pool Size";
			array[10] = "Min Pool Size";
			array[14] = "MultipleActiveResultSets";
			array[29] = "MultiSubnetFailover";
			array[20] = "Network Library";
			array[21] = "Packet Size";
			array[7] = "Password";
			array[5] = "Persist Security Info";
			array[9] = "Pooling";
			array[15] = "Replication";
			array[28] = "Transaction Binding";
			array[18] = "TrustServerCertificate";
			array[22] = "Type System Version";
			array[6] = "User ID";
			array[26] = "User Instance";
			array[25] = "Workstation ID";
			SqlConnectionStringBuilder._validKeywords = array;
			SqlConnectionStringBuilder._keywords = new Dictionary<string, SqlConnectionStringBuilder.Keywords>(52, StringComparer.OrdinalIgnoreCase)
			{
				{
					"ApplicationIntent",
					SqlConnectionStringBuilder.Keywords.ApplicationIntent
				},
				{
					"Application Name",
					SqlConnectionStringBuilder.Keywords.ApplicationName
				},
				{
					"Asynchronous Processing",
					SqlConnectionStringBuilder.Keywords.AsynchronousProcessing
				},
				{
					"AttachDbFilename",
					SqlConnectionStringBuilder.Keywords.AttachDBFilename
				},
				{
					"Connect Timeout",
					SqlConnectionStringBuilder.Keywords.ConnectTimeout
				},
				{
					"Connection Reset",
					SqlConnectionStringBuilder.Keywords.ConnectionReset
				},
				{
					"Context Connection",
					SqlConnectionStringBuilder.Keywords.ContextConnection
				},
				{
					"Current Language",
					SqlConnectionStringBuilder.Keywords.CurrentLanguage
				},
				{
					"Data Source",
					SqlConnectionStringBuilder.Keywords.DataSource
				},
				{
					"Encrypt",
					SqlConnectionStringBuilder.Keywords.Encrypt
				},
				{
					"Enlist",
					SqlConnectionStringBuilder.Keywords.Enlist
				},
				{
					"Failover Partner",
					SqlConnectionStringBuilder.Keywords.FailoverPartner
				},
				{
					"Initial Catalog",
					SqlConnectionStringBuilder.Keywords.InitialCatalog
				},
				{
					"Integrated Security",
					SqlConnectionStringBuilder.Keywords.IntegratedSecurity
				},
				{
					"Load Balance Timeout",
					SqlConnectionStringBuilder.Keywords.LoadBalanceTimeout
				},
				{
					"MultipleActiveResultSets",
					SqlConnectionStringBuilder.Keywords.MultipleActiveResultSets
				},
				{
					"Max Pool Size",
					SqlConnectionStringBuilder.Keywords.MaxPoolSize
				},
				{
					"Min Pool Size",
					SqlConnectionStringBuilder.Keywords.MinPoolSize
				},
				{
					"MultiSubnetFailover",
					SqlConnectionStringBuilder.Keywords.MultiSubnetFailover
				},
				{
					"Network Library",
					SqlConnectionStringBuilder.Keywords.NetworkLibrary
				},
				{
					"Packet Size",
					SqlConnectionStringBuilder.Keywords.PacketSize
				},
				{
					"Password",
					SqlConnectionStringBuilder.Keywords.Password
				},
				{
					"Persist Security Info",
					SqlConnectionStringBuilder.Keywords.PersistSecurityInfo
				},
				{
					"Pooling",
					SqlConnectionStringBuilder.Keywords.Pooling
				},
				{
					"Replication",
					SqlConnectionStringBuilder.Keywords.Replication
				},
				{
					"Transaction Binding",
					SqlConnectionStringBuilder.Keywords.TransactionBinding
				},
				{
					"TrustServerCertificate",
					SqlConnectionStringBuilder.Keywords.TrustServerCertificate
				},
				{
					"Type System Version",
					SqlConnectionStringBuilder.Keywords.TypeSystemVersion
				},
				{
					"User ID",
					SqlConnectionStringBuilder.Keywords.UserID
				},
				{
					"User Instance",
					SqlConnectionStringBuilder.Keywords.UserInstance
				},
				{
					"Workstation ID",
					SqlConnectionStringBuilder.Keywords.WorkstationID
				},
				{
					"app",
					SqlConnectionStringBuilder.Keywords.ApplicationName
				},
				{
					"async",
					SqlConnectionStringBuilder.Keywords.AsynchronousProcessing
				},
				{
					"extended properties",
					SqlConnectionStringBuilder.Keywords.AttachDBFilename
				},
				{
					"initial file name",
					SqlConnectionStringBuilder.Keywords.AttachDBFilename
				},
				{
					"connection timeout",
					SqlConnectionStringBuilder.Keywords.ConnectTimeout
				},
				{
					"timeout",
					SqlConnectionStringBuilder.Keywords.ConnectTimeout
				},
				{
					"language",
					SqlConnectionStringBuilder.Keywords.CurrentLanguage
				},
				{
					"addr",
					SqlConnectionStringBuilder.Keywords.DataSource
				},
				{
					"address",
					SqlConnectionStringBuilder.Keywords.DataSource
				},
				{
					"network address",
					SqlConnectionStringBuilder.Keywords.DataSource
				},
				{
					"server",
					SqlConnectionStringBuilder.Keywords.DataSource
				},
				{
					"database",
					SqlConnectionStringBuilder.Keywords.InitialCatalog
				},
				{
					"trusted_connection",
					SqlConnectionStringBuilder.Keywords.IntegratedSecurity
				},
				{
					"connection lifetime",
					SqlConnectionStringBuilder.Keywords.LoadBalanceTimeout
				},
				{
					"net",
					SqlConnectionStringBuilder.Keywords.NetworkLibrary
				},
				{
					"network",
					SqlConnectionStringBuilder.Keywords.NetworkLibrary
				},
				{
					"pwd",
					SqlConnectionStringBuilder.Keywords.Password
				},
				{
					"persistsecurityinfo",
					SqlConnectionStringBuilder.Keywords.PersistSecurityInfo
				},
				{
					"uid",
					SqlConnectionStringBuilder.Keywords.UserID
				},
				{
					"user",
					SqlConnectionStringBuilder.Keywords.UserID
				},
				{
					"wsid",
					SqlConnectionStringBuilder.Keywords.WorkstationID
				}
			};
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x0027BB60 File Offset: 0x0027AF60
		public SqlConnectionStringBuilder()
			: this(null)
		{
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x0027BB74 File Offset: 0x0027AF74
		public SqlConnectionStringBuilder(string connectionString)
		{
			if (!ADP.IsEmpty(connectionString))
			{
				base.ConnectionString = connectionString;
			}
		}

		// Token: 0x170005C8 RID: 1480
		public override object this[string keyword]
		{
			get
			{
				SqlConnectionStringBuilder.Keywords index = this.GetIndex(keyword);
				return this.GetAt(index);
			}
			set
			{
				Bid.Trace("<comm.SqlConnectionStringBuilder.set_Item|API> keyword='%ls'\n", keyword);
				if (value == null)
				{
					this.Remove(keyword);
					return;
				}
				switch (this.GetIndex(keyword))
				{
				case SqlConnectionStringBuilder.Keywords.DataSource:
					this.DataSource = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.FailoverPartner:
					this.FailoverPartner = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.AttachDBFilename:
					this.AttachDBFilename = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.InitialCatalog:
					this.InitialCatalog = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.IntegratedSecurity:
					this.IntegratedSecurity = SqlConnectionStringBuilder.ConvertToIntegratedSecurity(value);
					return;
				case SqlConnectionStringBuilder.Keywords.PersistSecurityInfo:
					this.PersistSecurityInfo = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.UserID:
					this.UserID = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.Password:
					this.Password = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.Enlist:
					this.Enlist = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.Pooling:
					this.Pooling = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.MinPoolSize:
					this.MinPoolSize = SqlConnectionStringBuilder.ConvertToInt32(value);
					return;
				case SqlConnectionStringBuilder.Keywords.MaxPoolSize:
					this.MaxPoolSize = SqlConnectionStringBuilder.ConvertToInt32(value);
					return;
				case SqlConnectionStringBuilder.Keywords.AsynchronousProcessing:
					this.AsynchronousProcessing = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.ConnectionReset:
					this.ConnectionReset = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.MultipleActiveResultSets:
					this.MultipleActiveResultSets = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.Replication:
					this.Replication = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.ConnectTimeout:
					this.ConnectTimeout = SqlConnectionStringBuilder.ConvertToInt32(value);
					return;
				case SqlConnectionStringBuilder.Keywords.Encrypt:
					this.Encrypt = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.TrustServerCertificate:
					this.TrustServerCertificate = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.LoadBalanceTimeout:
					this.LoadBalanceTimeout = SqlConnectionStringBuilder.ConvertToInt32(value);
					return;
				case SqlConnectionStringBuilder.Keywords.NetworkLibrary:
					this.NetworkLibrary = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.PacketSize:
					this.PacketSize = SqlConnectionStringBuilder.ConvertToInt32(value);
					return;
				case SqlConnectionStringBuilder.Keywords.TypeSystemVersion:
					this.TypeSystemVersion = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.ApplicationName:
					this.ApplicationName = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.CurrentLanguage:
					this.CurrentLanguage = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.WorkstationID:
					this.WorkstationID = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.UserInstance:
					this.UserInstance = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.ContextConnection:
					this.ContextConnection = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.TransactionBinding:
					this.TransactionBinding = SqlConnectionStringBuilder.ConvertToString(value);
					return;
				case SqlConnectionStringBuilder.Keywords.MultiSubnetFailover:
					this.MultiSubnetFailover = SqlConnectionStringBuilder.ConvertToBoolean(value);
					return;
				case SqlConnectionStringBuilder.Keywords.ApplicationIntent:
					this.ApplicationIntent = SqlConnectionStringBuilder.ConvertToApplicationIntent(keyword, value);
					return;
				default:
					throw ADP.KeywordNotSupported(keyword);
				}
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06002546 RID: 9542 RVA: 0x0027BEBC File Offset: 0x0027B2BC
		// (set) Token: 0x06002547 RID: 9543 RVA: 0x0027BED0 File Offset: 0x0027B2D0
		internal ApplicationIntent ApplicationIntent
		{
			get
			{
				return this._applicationIntent;
			}
			set
			{
				if (!DbConnectionStringBuilderUtil.IsValidApplicationIntentValue(value))
				{
					throw ADP.InvalidEnumerationValue(typeof(ApplicationIntent), (int)value);
				}
				this.SetApplicationIntentValue(value);
				this._applicationIntent = value;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06002548 RID: 9544 RVA: 0x0027BF04 File Offset: 0x0027B304
		// (set) Token: 0x06002549 RID: 9545 RVA: 0x0027BF18 File Offset: 0x0027B318
		internal bool MultiSubnetFailover
		{
			get
			{
				return this._multiSubnetFailover;
			}
			set
			{
				this.SetValue("MultiSubnetFailover", value);
				this._multiSubnetFailover = value;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x0600254A RID: 9546 RVA: 0x0027BF38 File Offset: 0x0027B338
		// (set) Token: 0x0600254B RID: 9547 RVA: 0x0027BF4C File Offset: 0x0027B34C
		[ResCategory("DataCategory_Context")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Application Name")]
		[ResDescription("DbConnectionString_ApplicationName")]
		public string ApplicationName
		{
			get
			{
				return this._applicationName;
			}
			set
			{
				this.SetValue("Application Name", value);
				this._applicationName = value;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x0027BF6C File Offset: 0x0027B36C
		// (set) Token: 0x0600254D RID: 9549 RVA: 0x0027BF80 File Offset: 0x0027B380
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_AsynchronousProcessing")]
		[ResCategory("DataCategory_Initialization")]
		[DisplayName("Asynchronous Processing")]
		public bool AsynchronousProcessing
		{
			get
			{
				return this._asynchronousProcessing;
			}
			set
			{
				this.SetValue("Asynchronous Processing", value);
				this._asynchronousProcessing = value;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x0600254E RID: 9550 RVA: 0x0027BFA0 File Offset: 0x0027B3A0
		// (set) Token: 0x0600254F RID: 9551 RVA: 0x0027BFB4 File Offset: 0x0027B3B4
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("AttachDbFilename")]
		[ResDescription("DbConnectionString_AttachDBFilename")]
		[ResCategory("DataCategory_Source")]
		[Editor("System.Windows.Forms.Design.FileNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string AttachDBFilename
		{
			get
			{
				return this._attachDBFilename;
			}
			set
			{
				this.SetValue("AttachDbFilename", value);
				this._attachDBFilename = value;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06002550 RID: 9552 RVA: 0x0027BFD4 File Offset: 0x0027B3D4
		// (set) Token: 0x06002551 RID: 9553 RVA: 0x0027BFE8 File Offset: 0x0027B3E8
		[ResCategory("DataCategory_Pooling")]
		[Obsolete("ConnectionReset has been deprecated.  SqlConnection will ignore the 'connection reset' keyword and always reset the connection")]
		[DisplayName("Connection Reset")]
		[ResDescription("DbConnectionString_ConnectionReset")]
		[RefreshProperties(RefreshProperties.All)]
		[Browsable(false)]
		public bool ConnectionReset
		{
			get
			{
				return this._connectionReset;
			}
			set
			{
				this.SetValue("Connection Reset", value);
				this._connectionReset = value;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06002552 RID: 9554 RVA: 0x0027C008 File Offset: 0x0027B408
		// (set) Token: 0x06002553 RID: 9555 RVA: 0x0027C01C File Offset: 0x0027B41C
		[ResCategory("DataCategory_Source")]
		[DisplayName("Context Connection")]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_ContextConnection")]
		public bool ContextConnection
		{
			get
			{
				return this._contextConnection;
			}
			set
			{
				this.SetValue("Context Connection", value);
				this._contextConnection = value;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06002554 RID: 9556 RVA: 0x0027C03C File Offset: 0x0027B43C
		// (set) Token: 0x06002555 RID: 9557 RVA: 0x0027C050 File Offset: 0x0027B450
		[ResDescription("DbConnectionString_ConnectTimeout")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Initialization")]
		[DisplayName("Connect Timeout")]
		public int ConnectTimeout
		{
			get
			{
				return this._connectTimeout;
			}
			set
			{
				if (value < 0)
				{
					throw ADP.InvalidConnectionOptionValue("Connect Timeout");
				}
				this.SetValue("Connect Timeout", value);
				this._connectTimeout = value;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06002556 RID: 9558 RVA: 0x0027C080 File Offset: 0x0027B480
		// (set) Token: 0x06002557 RID: 9559 RVA: 0x0027C094 File Offset: 0x0027B494
		[ResCategory("DataCategory_Initialization")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Current Language")]
		[ResDescription("DbConnectionString_CurrentLanguage")]
		public string CurrentLanguage
		{
			get
			{
				return this._currentLanguage;
			}
			set
			{
				this.SetValue("Current Language", value);
				this._currentLanguage = value;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002558 RID: 9560 RVA: 0x0027C0B4 File Offset: 0x0027B4B4
		// (set) Token: 0x06002559 RID: 9561 RVA: 0x0027C0C8 File Offset: 0x0027B4C8
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_DataSource")]
		[DisplayName("Data Source")]
		[ResCategory("DataCategory_Source")]
		[TypeConverter(typeof(SqlConnectionStringBuilder.SqlDataSourceConverter))]
		public string DataSource
		{
			get
			{
				return this._dataSource;
			}
			set
			{
				this.SetValue("Data Source", value);
				this._dataSource = value;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x0600255A RID: 9562 RVA: 0x0027C0E8 File Offset: 0x0027B4E8
		// (set) Token: 0x0600255B RID: 9563 RVA: 0x0027C0FC File Offset: 0x0027B4FC
		[ResDescription("DbConnectionString_Encrypt")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Security")]
		[DisplayName("Encrypt")]
		public bool Encrypt
		{
			get
			{
				return this._encrypt;
			}
			set
			{
				this.SetValue("Encrypt", value);
				this._encrypt = value;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x0027C11C File Offset: 0x0027B51C
		// (set) Token: 0x0600255D RID: 9565 RVA: 0x0027C130 File Offset: 0x0027B530
		[ResDescription("DbConnectionString_TrustServerCertificate")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("TrustServerCertificate")]
		[ResCategory("DataCategory_Security")]
		public bool TrustServerCertificate
		{
			get
			{
				return this._trustServerCertificate;
			}
			set
			{
				this.SetValue("TrustServerCertificate", value);
				this._trustServerCertificate = value;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x0600255E RID: 9566 RVA: 0x0027C150 File Offset: 0x0027B550
		// (set) Token: 0x0600255F RID: 9567 RVA: 0x0027C164 File Offset: 0x0027B564
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Enlist")]
		[ResDescription("DbConnectionString_Enlist")]
		[ResCategory("DataCategory_Pooling")]
		public bool Enlist
		{
			get
			{
				return this._enlist;
			}
			set
			{
				this.SetValue("Enlist", value);
				this._enlist = value;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06002560 RID: 9568 RVA: 0x0027C184 File Offset: 0x0027B584
		// (set) Token: 0x06002561 RID: 9569 RVA: 0x0027C198 File Offset: 0x0027B598
		[ResDescription("DbConnectionString_FailoverPartner")]
		[TypeConverter(typeof(SqlConnectionStringBuilder.SqlDataSourceConverter))]
		[ResCategory("DataCategory_Source")]
		[DisplayName("Failover Partner")]
		[RefreshProperties(RefreshProperties.All)]
		public string FailoverPartner
		{
			get
			{
				return this._failoverPartner;
			}
			set
			{
				this.SetValue("Failover Partner", value);
				this._failoverPartner = value;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06002562 RID: 9570 RVA: 0x0027C1B8 File Offset: 0x0027B5B8
		// (set) Token: 0x06002563 RID: 9571 RVA: 0x0027C1CC File Offset: 0x0027B5CC
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_InitialCatalog")]
		[ResCategory("DataCategory_Source")]
		[DisplayName("Initial Catalog")]
		[TypeConverter(typeof(SqlConnectionStringBuilder.SqlInitialCatalogConverter))]
		public string InitialCatalog
		{
			get
			{
				return this._initialCatalog;
			}
			set
			{
				this.SetValue("Initial Catalog", value);
				this._initialCatalog = value;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06002564 RID: 9572 RVA: 0x0027C1EC File Offset: 0x0027B5EC
		// (set) Token: 0x06002565 RID: 9573 RVA: 0x0027C200 File Offset: 0x0027B600
		[ResDescription("DbConnectionString_IntegratedSecurity")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Integrated Security")]
		[ResCategory("DataCategory_Security")]
		public bool IntegratedSecurity
		{
			get
			{
				return this._integratedSecurity;
			}
			set
			{
				this.SetValue("Integrated Security", value);
				this._integratedSecurity = value;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002566 RID: 9574 RVA: 0x0027C220 File Offset: 0x0027B620
		// (set) Token: 0x06002567 RID: 9575 RVA: 0x0027C234 File Offset: 0x0027B634
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Pooling")]
		[DisplayName("Load Balance Timeout")]
		[ResDescription("DbConnectionString_LoadBalanceTimeout")]
		public int LoadBalanceTimeout
		{
			get
			{
				return this._loadBalanceTimeout;
			}
			set
			{
				if (value < 0)
				{
					throw ADP.InvalidConnectionOptionValue("Load Balance Timeout");
				}
				this.SetValue("Load Balance Timeout", value);
				this._loadBalanceTimeout = value;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06002568 RID: 9576 RVA: 0x0027C264 File Offset: 0x0027B664
		// (set) Token: 0x06002569 RID: 9577 RVA: 0x0027C278 File Offset: 0x0027B678
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Pooling")]
		[DisplayName("Max Pool Size")]
		[ResDescription("DbConnectionString_MaxPoolSize")]
		public int MaxPoolSize
		{
			get
			{
				return this._maxPoolSize;
			}
			set
			{
				if (value < 1)
				{
					throw ADP.InvalidConnectionOptionValue("Max Pool Size");
				}
				this.SetValue("Max Pool Size", value);
				this._maxPoolSize = value;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x0600256A RID: 9578 RVA: 0x0027C2A8 File Offset: 0x0027B6A8
		// (set) Token: 0x0600256B RID: 9579 RVA: 0x0027C2BC File Offset: 0x0027B6BC
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_MinPoolSize")]
		[DisplayName("Min Pool Size")]
		[ResCategory("DataCategory_Pooling")]
		public int MinPoolSize
		{
			get
			{
				return this._minPoolSize;
			}
			set
			{
				if (value < 0)
				{
					throw ADP.InvalidConnectionOptionValue("Min Pool Size");
				}
				this.SetValue("Min Pool Size", value);
				this._minPoolSize = value;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x0600256C RID: 9580 RVA: 0x0027C2EC File Offset: 0x0027B6EC
		// (set) Token: 0x0600256D RID: 9581 RVA: 0x0027C300 File Offset: 0x0027B700
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Advanced")]
		[DisplayName("MultipleActiveResultSets")]
		[ResDescription("DbConnectionString_MultipleActiveResultSets")]
		public bool MultipleActiveResultSets
		{
			get
			{
				return this._multipleActiveResultSets;
			}
			set
			{
				this.SetValue("MultipleActiveResultSets", value);
				this._multipleActiveResultSets = value;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x0600256E RID: 9582 RVA: 0x0027C320 File Offset: 0x0027B720
		// (set) Token: 0x0600256F RID: 9583 RVA: 0x0027C334 File Offset: 0x0027B734
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_NetworkLibrary")]
		[DisplayName("Network Library")]
		[ResCategory("DataCategory_Advanced")]
		[TypeConverter(typeof(SqlConnectionStringBuilder.NetworkLibraryConverter))]
		public string NetworkLibrary
		{
			get
			{
				return this._networkLibrary;
			}
			set
			{
				if (value != null)
				{
					string text;
					switch (text = value.Trim().ToLower(CultureInfo.InvariantCulture))
					{
					case "dbmsadsn":
						value = "dbmsadsn";
						goto IL_011F;
					case "dbmsvinn":
						value = "dbmsvinn";
						goto IL_011F;
					case "dbmsspxn":
						value = "dbmsspxn";
						goto IL_011F;
					case "dbmsrpcn":
						value = "dbmsrpcn";
						goto IL_011F;
					case "dbnmpntw":
						value = "dbnmpntw";
						goto IL_011F;
					case "dbmslpcn":
						value = "dbmslpcn";
						goto IL_011F;
					case "dbmssocn":
						value = "dbmssocn";
						goto IL_011F;
					case "dbmsgnet":
						value = "dbmsgnet";
						goto IL_011F;
					}
					throw ADP.InvalidConnectionOptionValue("Network Library");
				}
				IL_011F:
				this.SetValue("Network Library", value);
				this._networkLibrary = value;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06002570 RID: 9584 RVA: 0x0027C474 File Offset: 0x0027B874
		// (set) Token: 0x06002571 RID: 9585 RVA: 0x0027C488 File Offset: 0x0027B888
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_PacketSize")]
		[DisplayName("Packet Size")]
		[ResCategory("DataCategory_Advanced")]
		public int PacketSize
		{
			get
			{
				return this._packetSize;
			}
			set
			{
				if (value < 512 || 32768 < value)
				{
					throw SQL.InvalidPacketSizeValue();
				}
				this.SetValue("Packet Size", value);
				this._packetSize = value;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06002572 RID: 9586 RVA: 0x0027C4C0 File Offset: 0x0027B8C0
		// (set) Token: 0x06002573 RID: 9587 RVA: 0x0027C4D4 File Offset: 0x0027B8D4
		[ResDescription("DbConnectionString_Password")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Password")]
		[PasswordPropertyText(true)]
		[ResCategory("DataCategory_Security")]
		public string Password
		{
			get
			{
				return this._password;
			}
			set
			{
				this.SetValue("Password", value);
				this._password = value;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x0027C4F4 File Offset: 0x0027B8F4
		// (set) Token: 0x06002575 RID: 9589 RVA: 0x0027C508 File Offset: 0x0027B908
		[ResDescription("DbConnectionString_PersistSecurityInfo")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Persist Security Info")]
		[ResCategory("DataCategory_Security")]
		public bool PersistSecurityInfo
		{
			get
			{
				return this._persistSecurityInfo;
			}
			set
			{
				this.SetValue("Persist Security Info", value);
				this._persistSecurityInfo = value;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06002576 RID: 9590 RVA: 0x0027C528 File Offset: 0x0027B928
		// (set) Token: 0x06002577 RID: 9591 RVA: 0x0027C53C File Offset: 0x0027B93C
		[DisplayName("Pooling")]
		[ResCategory("DataCategory_Pooling")]
		[ResDescription("DbConnectionString_Pooling")]
		[RefreshProperties(RefreshProperties.All)]
		public bool Pooling
		{
			get
			{
				return this._pooling;
			}
			set
			{
				this.SetValue("Pooling", value);
				this._pooling = value;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06002578 RID: 9592 RVA: 0x0027C55C File Offset: 0x0027B95C
		// (set) Token: 0x06002579 RID: 9593 RVA: 0x0027C570 File Offset: 0x0027B970
		[DisplayName("Replication")]
		[ResDescription("DbConnectionString_Replication")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Replication")]
		public bool Replication
		{
			get
			{
				return this._replication;
			}
			set
			{
				this.SetValue("Replication", value);
				this._replication = value;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x0027C590 File Offset: 0x0027B990
		// (set) Token: 0x0600257B RID: 9595 RVA: 0x0027C5A4 File Offset: 0x0027B9A4
		[DisplayName("Transaction Binding")]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_TransactionBinding")]
		[ResCategory("DataCategory_Advanced")]
		public string TransactionBinding
		{
			get
			{
				return this._transactionBinding;
			}
			set
			{
				this.SetValue("Transaction Binding", value);
				this._transactionBinding = value;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x0600257C RID: 9596 RVA: 0x0027C5C4 File Offset: 0x0027B9C4
		// (set) Token: 0x0600257D RID: 9597 RVA: 0x0027C5D8 File Offset: 0x0027B9D8
		[ResCategory("DataCategory_Advanced")]
		[ResDescription("DbConnectionString_TypeSystemVersion")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Type System Version")]
		public string TypeSystemVersion
		{
			get
			{
				return this._typeSystemVersion;
			}
			set
			{
				this.SetValue("Type System Version", value);
				this._typeSystemVersion = value;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600257E RID: 9598 RVA: 0x0027C5F8 File Offset: 0x0027B9F8
		// (set) Token: 0x0600257F RID: 9599 RVA: 0x0027C60C File Offset: 0x0027BA0C
		[DisplayName("User ID")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Security")]
		[ResDescription("DbConnectionString_UserID")]
		public string UserID
		{
			get
			{
				return this._userID;
			}
			set
			{
				this.SetValue("User ID", value);
				this._userID = value;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06002580 RID: 9600 RVA: 0x0027C62C File Offset: 0x0027BA2C
		// (set) Token: 0x06002581 RID: 9601 RVA: 0x0027C640 File Offset: 0x0027BA40
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbConnectionString_UserInstance")]
		[DisplayName("User Instance")]
		[ResCategory("DataCategory_Source")]
		public bool UserInstance
		{
			get
			{
				return this._userInstance;
			}
			set
			{
				this.SetValue("User Instance", value);
				this._userInstance = value;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06002582 RID: 9602 RVA: 0x0027C660 File Offset: 0x0027BA60
		// (set) Token: 0x06002583 RID: 9603 RVA: 0x0027C674 File Offset: 0x0027BA74
		[ResDescription("DbConnectionString_WorkstationID")]
		[RefreshProperties(RefreshProperties.All)]
		[DisplayName("Workstation ID")]
		[ResCategory("DataCategory_Context")]
		public string WorkstationID
		{
			get
			{
				return this._workstationID;
			}
			set
			{
				this.SetValue("Workstation ID", value);
				this._workstationID = value;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06002584 RID: 9604 RVA: 0x0027C694 File Offset: 0x0027BA94
		public override bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06002585 RID: 9605 RVA: 0x0027C6A4 File Offset: 0x0027BAA4
		public override ICollection Keys
		{
			get
			{
				return new ReadOnlyCollection<string>(SqlConnectionStringBuilder._validKeywords);
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x0027C6BC File Offset: 0x0027BABC
		public override ICollection Values
		{
			get
			{
				object[] array = new object[SqlConnectionStringBuilder._validKeywords.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.GetAt((SqlConnectionStringBuilder.Keywords)i);
				}
				return new ReadOnlyCollection<object>(array);
			}
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x0027C6F4 File Offset: 0x0027BAF4
		public override void Clear()
		{
			base.Clear();
			for (int i = 0; i < SqlConnectionStringBuilder._validKeywords.Length; i++)
			{
				this.Reset((SqlConnectionStringBuilder.Keywords)i);
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x0027C720 File Offset: 0x0027BB20
		public override bool ContainsKey(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			return SqlConnectionStringBuilder._keywords.ContainsKey(keyword);
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x0027C744 File Offset: 0x0027BB44
		private static bool ConvertToBoolean(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToBoolean(value);
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x0027C758 File Offset: 0x0027BB58
		private static int ConvertToInt32(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToInt32(value);
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x0027C76C File Offset: 0x0027BB6C
		private static bool ConvertToIntegratedSecurity(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToIntegratedSecurity(value);
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x0027C780 File Offset: 0x0027BB80
		private static string ConvertToString(object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToString(value);
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x0027C794 File Offset: 0x0027BB94
		private static ApplicationIntent ConvertToApplicationIntent(string keyword, object value)
		{
			return DbConnectionStringBuilderUtil.ConvertToApplicationIntent(keyword, value);
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x0027C7A8 File Offset: 0x0027BBA8
		private object GetAt(SqlConnectionStringBuilder.Keywords index)
		{
			switch (index)
			{
			case SqlConnectionStringBuilder.Keywords.DataSource:
				return this.DataSource;
			case SqlConnectionStringBuilder.Keywords.FailoverPartner:
				return this.FailoverPartner;
			case SqlConnectionStringBuilder.Keywords.AttachDBFilename:
				return this.AttachDBFilename;
			case SqlConnectionStringBuilder.Keywords.InitialCatalog:
				return this.InitialCatalog;
			case SqlConnectionStringBuilder.Keywords.IntegratedSecurity:
				return this.IntegratedSecurity;
			case SqlConnectionStringBuilder.Keywords.PersistSecurityInfo:
				return this.PersistSecurityInfo;
			case SqlConnectionStringBuilder.Keywords.UserID:
				return this.UserID;
			case SqlConnectionStringBuilder.Keywords.Password:
				return this.Password;
			case SqlConnectionStringBuilder.Keywords.Enlist:
				return this.Enlist;
			case SqlConnectionStringBuilder.Keywords.Pooling:
				return this.Pooling;
			case SqlConnectionStringBuilder.Keywords.MinPoolSize:
				return this.MinPoolSize;
			case SqlConnectionStringBuilder.Keywords.MaxPoolSize:
				return this.MaxPoolSize;
			case SqlConnectionStringBuilder.Keywords.AsynchronousProcessing:
				return this.AsynchronousProcessing;
			case SqlConnectionStringBuilder.Keywords.ConnectionReset:
				return this.ConnectionReset;
			case SqlConnectionStringBuilder.Keywords.MultipleActiveResultSets:
				return this.MultipleActiveResultSets;
			case SqlConnectionStringBuilder.Keywords.Replication:
				return this.Replication;
			case SqlConnectionStringBuilder.Keywords.ConnectTimeout:
				return this.ConnectTimeout;
			case SqlConnectionStringBuilder.Keywords.Encrypt:
				return this.Encrypt;
			case SqlConnectionStringBuilder.Keywords.TrustServerCertificate:
				return this.TrustServerCertificate;
			case SqlConnectionStringBuilder.Keywords.LoadBalanceTimeout:
				return this.LoadBalanceTimeout;
			case SqlConnectionStringBuilder.Keywords.NetworkLibrary:
				return this.NetworkLibrary;
			case SqlConnectionStringBuilder.Keywords.PacketSize:
				return this.PacketSize;
			case SqlConnectionStringBuilder.Keywords.TypeSystemVersion:
				return this.TypeSystemVersion;
			case SqlConnectionStringBuilder.Keywords.ApplicationName:
				return this.ApplicationName;
			case SqlConnectionStringBuilder.Keywords.CurrentLanguage:
				return this.CurrentLanguage;
			case SqlConnectionStringBuilder.Keywords.WorkstationID:
				return this.WorkstationID;
			case SqlConnectionStringBuilder.Keywords.UserInstance:
				return this.UserInstance;
			case SqlConnectionStringBuilder.Keywords.ContextConnection:
				return this.ContextConnection;
			case SqlConnectionStringBuilder.Keywords.TransactionBinding:
				return this.TransactionBinding;
			case SqlConnectionStringBuilder.Keywords.MultiSubnetFailover:
				return this.MultiSubnetFailover;
			case SqlConnectionStringBuilder.Keywords.ApplicationIntent:
				return this.ApplicationIntent;
			default:
				throw ADP.KeywordNotSupported(SqlConnectionStringBuilder._validKeywords[(int)index]);
			}
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x0027C984 File Offset: 0x0027BD84
		private SqlConnectionStringBuilder.Keywords GetIndex(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			SqlConnectionStringBuilder.Keywords keywords;
			if (SqlConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
			{
				return keywords;
			}
			throw ADP.KeywordNotSupported(keyword);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x0027C9B4 File Offset: 0x0027BDB4
		protected override void GetProperties(Hashtable propertyDescriptors)
		{
			foreach (object obj in TypeDescriptor.GetProperties(this, true))
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				bool flag = false;
				string displayName = propertyDescriptor.DisplayName;
				bool flag2;
				if ("Integrated Security" == displayName)
				{
					flag = true;
					flag2 = propertyDescriptor.IsReadOnly;
				}
				else
				{
					if (!("Password" == displayName) && !("User ID" == displayName))
					{
						continue;
					}
					flag2 = this.IntegratedSecurity;
				}
				Attribute[] attributesFromCollection = base.GetAttributesFromCollection(propertyDescriptor.Attributes);
				propertyDescriptors[displayName] = new DbConnectionStringBuilderDescriptor(propertyDescriptor.Name, propertyDescriptor.ComponentType, propertyDescriptor.PropertyType, flag2, attributesFromCollection)
				{
					RefreshOnChange = flag
				};
			}
			base.GetProperties(propertyDescriptors);
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x0027CAA8 File Offset: 0x0027BEA8
		public override bool Remove(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			SqlConnectionStringBuilder.Keywords keywords;
			if (SqlConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords) && base.Remove(SqlConnectionStringBuilder._validKeywords[(int)keywords]))
			{
				this.Reset(keywords);
				return true;
			}
			return false;
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x0027CAE8 File Offset: 0x0027BEE8
		private void Reset(SqlConnectionStringBuilder.Keywords index)
		{
			switch (index)
			{
			case SqlConnectionStringBuilder.Keywords.DataSource:
				this._dataSource = "";
				return;
			case SqlConnectionStringBuilder.Keywords.FailoverPartner:
				this._failoverPartner = "";
				return;
			case SqlConnectionStringBuilder.Keywords.AttachDBFilename:
				this._attachDBFilename = "";
				return;
			case SqlConnectionStringBuilder.Keywords.InitialCatalog:
				this._initialCatalog = "";
				return;
			case SqlConnectionStringBuilder.Keywords.IntegratedSecurity:
				this._integratedSecurity = false;
				return;
			case SqlConnectionStringBuilder.Keywords.PersistSecurityInfo:
				this._persistSecurityInfo = false;
				return;
			case SqlConnectionStringBuilder.Keywords.UserID:
				this._userID = "";
				return;
			case SqlConnectionStringBuilder.Keywords.Password:
				this._password = "";
				return;
			case SqlConnectionStringBuilder.Keywords.Enlist:
				this._enlist = true;
				return;
			case SqlConnectionStringBuilder.Keywords.Pooling:
				this._pooling = true;
				return;
			case SqlConnectionStringBuilder.Keywords.MinPoolSize:
				this._minPoolSize = 0;
				return;
			case SqlConnectionStringBuilder.Keywords.MaxPoolSize:
				this._maxPoolSize = 100;
				return;
			case SqlConnectionStringBuilder.Keywords.AsynchronousProcessing:
				this._asynchronousProcessing = false;
				return;
			case SqlConnectionStringBuilder.Keywords.ConnectionReset:
				this._connectionReset = true;
				return;
			case SqlConnectionStringBuilder.Keywords.MultipleActiveResultSets:
				this._multipleActiveResultSets = false;
				return;
			case SqlConnectionStringBuilder.Keywords.Replication:
				this._replication = false;
				return;
			case SqlConnectionStringBuilder.Keywords.ConnectTimeout:
				this._connectTimeout = 15;
				return;
			case SqlConnectionStringBuilder.Keywords.Encrypt:
				this._encrypt = false;
				return;
			case SqlConnectionStringBuilder.Keywords.TrustServerCertificate:
				this._trustServerCertificate = false;
				return;
			case SqlConnectionStringBuilder.Keywords.LoadBalanceTimeout:
				this._loadBalanceTimeout = 0;
				return;
			case SqlConnectionStringBuilder.Keywords.NetworkLibrary:
				this._networkLibrary = "";
				return;
			case SqlConnectionStringBuilder.Keywords.PacketSize:
				this._packetSize = 8000;
				return;
			case SqlConnectionStringBuilder.Keywords.TypeSystemVersion:
				this._typeSystemVersion = "Latest";
				return;
			case SqlConnectionStringBuilder.Keywords.ApplicationName:
				this._applicationName = ".Net SqlClient Data Provider";
				return;
			case SqlConnectionStringBuilder.Keywords.CurrentLanguage:
				this._currentLanguage = "";
				return;
			case SqlConnectionStringBuilder.Keywords.WorkstationID:
				this._workstationID = "";
				return;
			case SqlConnectionStringBuilder.Keywords.UserInstance:
				this._userInstance = false;
				return;
			case SqlConnectionStringBuilder.Keywords.ContextConnection:
				this._contextConnection = false;
				return;
			case SqlConnectionStringBuilder.Keywords.TransactionBinding:
				this._transactionBinding = "Implicit Unbind";
				return;
			case SqlConnectionStringBuilder.Keywords.MultiSubnetFailover:
				this._multiSubnetFailover = false;
				return;
			case SqlConnectionStringBuilder.Keywords.ApplicationIntent:
				this._applicationIntent = ApplicationIntent.ReadWrite;
				return;
			default:
				throw ADP.KeywordNotSupported(SqlConnectionStringBuilder._validKeywords[(int)index]);
			}
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x0027CCB8 File Offset: 0x0027C0B8
		private void SetValue(string keyword, bool value)
		{
			base[keyword] = value.ToString(null);
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x0027CCD4 File Offset: 0x0027C0D4
		private void SetValue(string keyword, int value)
		{
			base[keyword] = value.ToString(null);
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x0027CCF0 File Offset: 0x0027C0F0
		private void SetValue(string keyword, string value)
		{
			ADP.CheckArgumentNull(value, keyword);
			base[keyword] = value;
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x0027CD0C File Offset: 0x0027C10C
		private void SetApplicationIntentValue(ApplicationIntent value)
		{
			base["ApplicationIntent"] = DbConnectionStringBuilderUtil.ApplicationIntentToString(value);
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x0027CD2C File Offset: 0x0027C12C
		public override bool ShouldSerialize(string keyword)
		{
			ADP.CheckArgumentNull(keyword, "keyword");
			SqlConnectionStringBuilder.Keywords keywords;
			return SqlConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords) && base.ShouldSerialize(SqlConnectionStringBuilder._validKeywords[(int)keywords]);
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x0027CD64 File Offset: 0x0027C164
		public override bool TryGetValue(string keyword, out object value)
		{
			SqlConnectionStringBuilder.Keywords keywords;
			if (SqlConnectionStringBuilder._keywords.TryGetValue(keyword, out keywords))
			{
				value = this.GetAt(keywords);
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x040017B5 RID: 6069
		private static readonly string[] _validKeywords;

		// Token: 0x040017B6 RID: 6070
		private static readonly Dictionary<string, SqlConnectionStringBuilder.Keywords> _keywords;

		// Token: 0x040017B7 RID: 6071
		private ApplicationIntent _applicationIntent;

		// Token: 0x040017B8 RID: 6072
		private string _applicationName = ".Net SqlClient Data Provider";

		// Token: 0x040017B9 RID: 6073
		private string _attachDBFilename = "";

		// Token: 0x040017BA RID: 6074
		private string _currentLanguage = "";

		// Token: 0x040017BB RID: 6075
		private string _dataSource = "";

		// Token: 0x040017BC RID: 6076
		private string _failoverPartner = "";

		// Token: 0x040017BD RID: 6077
		private string _initialCatalog = "";

		// Token: 0x040017BE RID: 6078
		private string _networkLibrary = "";

		// Token: 0x040017BF RID: 6079
		private string _password = "";

		// Token: 0x040017C0 RID: 6080
		private string _transactionBinding = "Implicit Unbind";

		// Token: 0x040017C1 RID: 6081
		private string _typeSystemVersion = "Latest";

		// Token: 0x040017C2 RID: 6082
		private string _userID = "";

		// Token: 0x040017C3 RID: 6083
		private string _workstationID = "";

		// Token: 0x040017C4 RID: 6084
		private int _connectTimeout = 15;

		// Token: 0x040017C5 RID: 6085
		private int _loadBalanceTimeout;

		// Token: 0x040017C6 RID: 6086
		private int _maxPoolSize = 100;

		// Token: 0x040017C7 RID: 6087
		private int _minPoolSize;

		// Token: 0x040017C8 RID: 6088
		private int _packetSize = 8000;

		// Token: 0x040017C9 RID: 6089
		private bool _asynchronousProcessing;

		// Token: 0x040017CA RID: 6090
		private bool _connectionReset = true;

		// Token: 0x040017CB RID: 6091
		private bool _contextConnection;

		// Token: 0x040017CC RID: 6092
		private bool _encrypt;

		// Token: 0x040017CD RID: 6093
		private bool _trustServerCertificate;

		// Token: 0x040017CE RID: 6094
		private bool _enlist = true;

		// Token: 0x040017CF RID: 6095
		private bool _integratedSecurity;

		// Token: 0x040017D0 RID: 6096
		private bool _multipleActiveResultSets;

		// Token: 0x040017D1 RID: 6097
		private bool _multiSubnetFailover;

		// Token: 0x040017D2 RID: 6098
		private bool _persistSecurityInfo;

		// Token: 0x040017D3 RID: 6099
		private bool _pooling = true;

		// Token: 0x040017D4 RID: 6100
		private bool _replication;

		// Token: 0x040017D5 RID: 6101
		private bool _userInstance;

		// Token: 0x020002D8 RID: 728
		private enum Keywords
		{
			// Token: 0x040017D7 RID: 6103
			DataSource,
			// Token: 0x040017D8 RID: 6104
			FailoverPartner,
			// Token: 0x040017D9 RID: 6105
			AttachDBFilename,
			// Token: 0x040017DA RID: 6106
			InitialCatalog,
			// Token: 0x040017DB RID: 6107
			IntegratedSecurity,
			// Token: 0x040017DC RID: 6108
			PersistSecurityInfo,
			// Token: 0x040017DD RID: 6109
			UserID,
			// Token: 0x040017DE RID: 6110
			Password,
			// Token: 0x040017DF RID: 6111
			Enlist,
			// Token: 0x040017E0 RID: 6112
			Pooling,
			// Token: 0x040017E1 RID: 6113
			MinPoolSize,
			// Token: 0x040017E2 RID: 6114
			MaxPoolSize,
			// Token: 0x040017E3 RID: 6115
			AsynchronousProcessing,
			// Token: 0x040017E4 RID: 6116
			ConnectionReset,
			// Token: 0x040017E5 RID: 6117
			MultipleActiveResultSets,
			// Token: 0x040017E6 RID: 6118
			Replication,
			// Token: 0x040017E7 RID: 6119
			ConnectTimeout,
			// Token: 0x040017E8 RID: 6120
			Encrypt,
			// Token: 0x040017E9 RID: 6121
			TrustServerCertificate,
			// Token: 0x040017EA RID: 6122
			LoadBalanceTimeout,
			// Token: 0x040017EB RID: 6123
			NetworkLibrary,
			// Token: 0x040017EC RID: 6124
			PacketSize,
			// Token: 0x040017ED RID: 6125
			TypeSystemVersion,
			// Token: 0x040017EE RID: 6126
			ApplicationName,
			// Token: 0x040017EF RID: 6127
			CurrentLanguage,
			// Token: 0x040017F0 RID: 6128
			WorkstationID,
			// Token: 0x040017F1 RID: 6129
			UserInstance,
			// Token: 0x040017F2 RID: 6130
			ContextConnection,
			// Token: 0x040017F3 RID: 6131
			TransactionBinding,
			// Token: 0x040017F4 RID: 6132
			MultiSubnetFailover,
			// Token: 0x040017F5 RID: 6133
			ApplicationIntent
		}

		// Token: 0x020002D9 RID: 729
		private sealed class NetworkLibraryConverter : TypeConverter
		{
			// Token: 0x0600259A RID: 9626 RVA: 0x0027CDA4 File Offset: 0x0027C1A4
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return typeof(string) == sourceType || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x0600259B RID: 9627 RVA: 0x0027CDC8 File Offset: 0x0027C1C8
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				string text = value as string;
				if (text == null)
				{
					return base.ConvertFrom(context, culture, value);
				}
				text = text.Trim();
				if (StringComparer.OrdinalIgnoreCase.Equals(text, "Named Pipes (DBNMPNTW)"))
				{
					return "dbnmpntw";
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(text, "Shared Memory (DBMSSOCN)"))
				{
					return "dbmslpcn";
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(text, "TCP/IP (DBMSGNET)"))
				{
					return "dbmssocn";
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(text, "VIA (DBMSGNET)"))
				{
					return "dbmsgnet";
				}
				return text;
			}

			// Token: 0x0600259C RID: 9628 RVA: 0x0027CE54 File Offset: 0x0027C254
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return typeof(string) == destinationType || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x0600259D RID: 9629 RVA: 0x0027CE78 File Offset: 0x0027C278
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				string text = value as string;
				if (text != null && destinationType == typeof(string))
				{
					string text2;
					if ((text2 = text.Trim().ToLower(CultureInfo.InvariantCulture)) != null)
					{
						if (text2 == "dbnmpntw")
						{
							return "Named Pipes (DBNMPNTW)";
						}
						if (text2 == "dbmslpcn")
						{
							return "Shared Memory (DBMSSOCN)";
						}
						if (text2 == "dbmssocn")
						{
							return "TCP/IP (DBMSGNET)";
						}
						if (text2 == "dbmsgnet")
						{
							return "VIA (DBMSGNET)";
						}
					}
					return text;
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			// Token: 0x0600259E RID: 9630 RVA: 0x0027CF0C File Offset: 0x0027C30C
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			// Token: 0x0600259F RID: 9631 RVA: 0x0027CF1C File Offset: 0x0027C31C
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			// Token: 0x060025A0 RID: 9632 RVA: 0x0027CF2C File Offset: 0x0027C32C
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				if (context != null)
				{
					object instance = context.Instance;
				}
				TypeConverter.StandardValuesCollection standardValuesCollection = this._standardValues;
				if (standardValuesCollection == null)
				{
					string[] array = new string[] { "Named Pipes (DBNMPNTW)", "Shared Memory (DBMSSOCN)", "TCP/IP (DBMSGNET)", "VIA (DBMSGNET)" };
					standardValuesCollection = new TypeConverter.StandardValuesCollection(array);
					this._standardValues = standardValuesCollection;
				}
				return standardValuesCollection;
			}

			// Token: 0x040017F6 RID: 6134
			private const string NamedPipes = "Named Pipes (DBNMPNTW)";

			// Token: 0x040017F7 RID: 6135
			private const string SharedMemory = "Shared Memory (DBMSSOCN)";

			// Token: 0x040017F8 RID: 6136
			private const string TCPIP = "TCP/IP (DBMSGNET)";

			// Token: 0x040017F9 RID: 6137
			private const string VIA = "VIA (DBMSGNET)";

			// Token: 0x040017FA RID: 6138
			private TypeConverter.StandardValuesCollection _standardValues;
		}

		// Token: 0x020002DA RID: 730
		private sealed class SqlDataSourceConverter : StringConverter
		{
			// Token: 0x060025A2 RID: 9634 RVA: 0x0027CF9C File Offset: 0x0027C39C
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			// Token: 0x060025A3 RID: 9635 RVA: 0x0027CFAC File Offset: 0x0027C3AC
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			// Token: 0x060025A4 RID: 9636 RVA: 0x0027CFBC File Offset: 0x0027C3BC
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				TypeConverter.StandardValuesCollection standardValuesCollection = this._standardValues;
				if (this._standardValues == null)
				{
					DataTable dataSources = SqlClientFactory.Instance.CreateDataSourceEnumerator().GetDataSources();
					DataColumn dataColumn = dataSources.Columns["ServerName"];
					DataColumn dataColumn2 = dataSources.Columns["InstanceName"];
					DataRowCollection rows = dataSources.Rows;
					string[] array = new string[rows.Count];
					for (int i = 0; i < array.Length; i++)
					{
						string text = rows[i][dataColumn] as string;
						string text2 = rows[i][dataColumn2] as string;
						if (text2 == null || text2.Length == 0 || "MSSQLSERVER" == text2)
						{
							array[i] = text;
						}
						else
						{
							array[i] = text + "\\" + text2;
						}
					}
					Array.Sort<string>(array);
					standardValuesCollection = new TypeConverter.StandardValuesCollection(array);
					this._standardValues = standardValuesCollection;
				}
				return standardValuesCollection;
			}

			// Token: 0x040017FB RID: 6139
			private TypeConverter.StandardValuesCollection _standardValues;
		}

		// Token: 0x020002DB RID: 731
		private sealed class SqlInitialCatalogConverter : StringConverter
		{
			// Token: 0x060025A6 RID: 9638 RVA: 0x0027D0BC File Offset: 0x0027C4BC
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return this.GetStandardValuesSupportedInternal(context);
			}

			// Token: 0x060025A7 RID: 9639 RVA: 0x0027D0D0 File Offset: 0x0027C4D0
			private bool GetStandardValuesSupportedInternal(ITypeDescriptorContext context)
			{
				bool flag = false;
				if (context != null)
				{
					SqlConnectionStringBuilder sqlConnectionStringBuilder = context.Instance as SqlConnectionStringBuilder;
					if (sqlConnectionStringBuilder != null && 0 < sqlConnectionStringBuilder.DataSource.Length && (sqlConnectionStringBuilder.IntegratedSecurity || 0 < sqlConnectionStringBuilder.UserID.Length))
					{
						flag = true;
					}
				}
				return flag;
			}

			// Token: 0x060025A8 RID: 9640 RVA: 0x0027D118 File Offset: 0x0027C518
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			// Token: 0x060025A9 RID: 9641 RVA: 0x0027D128 File Offset: 0x0027C528
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				if (this.GetStandardValuesSupportedInternal(context))
				{
					List<string> list = new List<string>();
					try
					{
						SqlConnectionStringBuilder sqlConnectionStringBuilder = (SqlConnectionStringBuilder)context.Instance;
						using (SqlConnection sqlConnection = new SqlConnection())
						{
							sqlConnection.ConnectionString = sqlConnectionStringBuilder.ConnectionString;
							using (SqlCommand sqlCommand = new SqlCommand("SELECT name FROM master.dbo.sysdatabases ORDER BY name", sqlConnection))
							{
								sqlConnection.Open();
								using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
								{
									while (sqlDataReader.Read())
									{
										list.Add(sqlDataReader.GetString(0));
									}
								}
							}
						}
					}
					catch (SqlException ex)
					{
						ADP.TraceExceptionWithoutRethrow(ex);
					}
					return new TypeConverter.StandardValuesCollection(list);
				}
				return null;
			}
		}

		// Token: 0x020002DC RID: 732
		internal sealed class SqlConnectionStringBuilderConverter : ExpandableObjectConverter
		{
			// Token: 0x060025AB RID: 9643 RVA: 0x0027D248 File Offset: 0x0027C648
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return typeof(InstanceDescriptor) == destinationType || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x060025AC RID: 9644 RVA: 0x0027D26C File Offset: 0x0027C66C
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw ADP.ArgumentNull("destinationType");
				}
				if (typeof(InstanceDescriptor) == destinationType)
				{
					SqlConnectionStringBuilder sqlConnectionStringBuilder = value as SqlConnectionStringBuilder;
					if (sqlConnectionStringBuilder != null)
					{
						return this.ConvertToInstanceDescriptor(sqlConnectionStringBuilder);
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			// Token: 0x060025AD RID: 9645 RVA: 0x0027D2B4 File Offset: 0x0027C6B4
			private InstanceDescriptor ConvertToInstanceDescriptor(SqlConnectionStringBuilder options)
			{
				Type[] array = new Type[] { typeof(string) };
				object[] array2 = new object[] { options.ConnectionString };
				ConstructorInfo constructor = typeof(SqlConnectionStringBuilder).GetConstructor(array);
				return new InstanceDescriptor(constructor, array2);
			}
		}
	}
}
