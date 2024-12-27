using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Transactions;

namespace System.Data.OleDb
{
	// Token: 0x02000214 RID: 532
	[DefaultEvent("InfoMessage")]
	public sealed class OleDbConnection : DbConnection, ICloneable, IDbConnection, IDisposable
	{
		// Token: 0x06001E2C RID: 7724 RVA: 0x00256380 File Offset: 0x00255780
		public OleDbConnection(string connectionString)
			: this()
		{
			this.ConnectionString = connectionString;
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x0025639C File Offset: 0x0025579C
		private OleDbConnection(OleDbConnection connection)
			: this()
		{
			this.CopyFrom(connection);
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001E2E RID: 7726 RVA: 0x002563B8 File Offset: 0x002557B8
		// (set) Token: 0x06001E2F RID: 7727 RVA: 0x002563CC File Offset: 0x002557CC
		[RecommendedAsConfigurable(true)]
		[ResCategory("DataCategory_Data")]
		[Editor("Microsoft.VSDesigner.Data.ADO.Design.OleDbConnectionStringEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("OleDbConnection_ConnectionString")]
		[DefaultValue("")]
		public override string ConnectionString
		{
			get
			{
				return this.ConnectionString_Get();
			}
			set
			{
				this.ConnectionString_Set(value);
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001E30 RID: 7728 RVA: 0x002563E0 File Offset: 0x002557E0
		private OleDbConnectionString OleDbConnectionStringValue
		{
			get
			{
				return (OleDbConnectionString)this.ConnectionOptions;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x002563F8 File Offset: 0x002557F8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ResDescription("OleDbConnection_ConnectionTimeout")]
		public override int ConnectionTimeout
		{
			get
			{
				IntPtr intPtr;
				Bid.ScopeEnter(out intPtr, "<oledb.OleDbConnection.get_ConnectionTimeout|API> %d#\n", this.ObjectID);
				int num;
				try
				{
					object obj;
					if (this.IsOpen)
					{
						obj = this.GetDataSourceValue(OleDbPropertySetGuid.DBInit, 66);
					}
					else
					{
						OleDbConnectionString oleDbConnectionStringValue = this.OleDbConnectionStringValue;
						obj = ((oleDbConnectionStringValue != null) ? oleDbConnectionStringValue.ConnectTimeout : 15);
					}
					if (obj != null)
					{
						num = Convert.ToInt32(obj, CultureInfo.InvariantCulture);
					}
					else
					{
						num = 15;
					}
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
				return num;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001E32 RID: 7730 RVA: 0x00256488 File Offset: 0x00255888
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ResDescription("OleDbConnection_Database")]
		public override string Database
		{
			get
			{
				IntPtr intPtr;
				Bid.ScopeEnter(out intPtr, "<oledb.OleDbConnection.get_Database|API> %d#\n", this.ObjectID);
				string text;
				try
				{
					OleDbConnectionString oleDbConnectionString = (OleDbConnectionString)this.UserConnectionOptions;
					object obj = ((oleDbConnectionString != null) ? oleDbConnectionString.InitialCatalog : ADP.StrEmpty);
					if (obj != null && !((string)obj).StartsWith("|datadirectory|", StringComparison.OrdinalIgnoreCase))
					{
						OleDbConnectionInternal openConnection = this.GetOpenConnection();
						if (openConnection != null)
						{
							if (openConnection.HasSession)
							{
								obj = this.GetDataSourceValue(OleDbPropertySetGuid.DataSource, 37);
							}
							else
							{
								obj = this.GetDataSourceValue(OleDbPropertySetGuid.DBInit, 233);
							}
						}
						else
						{
							oleDbConnectionString = this.OleDbConnectionStringValue;
							obj = ((oleDbConnectionString != null) ? oleDbConnectionString.InitialCatalog : ADP.StrEmpty);
						}
					}
					text = Convert.ToString(obj, CultureInfo.InvariantCulture);
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
				return text;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001E33 RID: 7731 RVA: 0x0025655C File Offset: 0x0025595C
		[Browsable(true)]
		[ResDescription("OleDbConnection_DataSource")]
		public override string DataSource
		{
			get
			{
				IntPtr intPtr;
				Bid.ScopeEnter(out intPtr, "<oledb.OleDbConnection.get_DataSource|API> %d#\n", this.ObjectID);
				string text;
				try
				{
					OleDbConnectionString oleDbConnectionString = (OleDbConnectionString)this.UserConnectionOptions;
					object obj = ((oleDbConnectionString != null) ? oleDbConnectionString.DataSource : ADP.StrEmpty);
					if (obj != null && !((string)obj).StartsWith("|datadirectory|", StringComparison.OrdinalIgnoreCase))
					{
						if (this.IsOpen)
						{
							obj = this.GetDataSourceValue(OleDbPropertySetGuid.DBInit, 59);
							if (obj == null || (obj is string && (obj as string).Length == 0))
							{
								obj = this.GetDataSourceValue(OleDbPropertySetGuid.DataSourceInfo, 38);
							}
						}
						else
						{
							oleDbConnectionString = this.OleDbConnectionStringValue;
							obj = ((oleDbConnectionString != null) ? oleDbConnectionString.DataSource : ADP.StrEmpty);
						}
					}
					text = Convert.ToString(obj, CultureInfo.InvariantCulture);
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
				return text;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001E34 RID: 7732 RVA: 0x00256638 File Offset: 0x00255A38
		internal bool IsOpen
		{
			get
			{
				return null != this.GetOpenConnection();
			}
		}

		// Token: 0x17000415 RID: 1045
		// (set) Token: 0x06001E35 RID: 7733 RVA: 0x00256654 File Offset: 0x00255A54
		internal OleDbTransaction LocalTransaction
		{
			set
			{
				OleDbConnectionInternal openConnection = this.GetOpenConnection();
				if (openConnection != null)
				{
					openConnection.LocalTransaction = value;
				}
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001E36 RID: 7734 RVA: 0x00256674 File Offset: 0x00255A74
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ResDescription("OleDbConnection_Provider")]
		[Browsable(true)]
		[ResCategory("DataCategory_Data")]
		public string Provider
		{
			get
			{
				Bid.Trace("<oledb.OleDbConnection.get_Provider|API> %d#\n", this.ObjectID);
				OleDbConnectionString oleDbConnectionStringValue = this.OleDbConnectionStringValue;
				string text = ((oleDbConnectionStringValue != null) ? oleDbConnectionStringValue.ConvertValueToString("provider", null) : null);
				if (text == null)
				{
					return ADP.StrEmpty;
				}
				return text;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001E37 RID: 7735 RVA: 0x002566B8 File Offset: 0x00255AB8
		internal OleDbConnectionPoolGroupProviderInfo ProviderInfo
		{
			get
			{
				return (OleDbConnectionPoolGroupProviderInfo)this.PoolGroup.ProviderInfo;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001E38 RID: 7736 RVA: 0x002566D8 File Offset: 0x00255AD8
		[ResDescription("OleDbConnection_ServerVersion")]
		public override string ServerVersion
		{
			get
			{
				return this.InnerConnection.ServerVersion;
			}
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x002566F0 File Offset: 0x00255AF0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void ResetState()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbCommand.ResetState|API> %d#\n", this.ObjectID);
			try
			{
				if (this.IsOpen)
				{
					object dataSourcePropertyValue = this.GetDataSourcePropertyValue(OleDbPropertySetGuid.DataSourceInfo, 244);
					if (dataSourcePropertyValue is int)
					{
						switch ((int)dataSourcePropertyValue)
						{
						case 0:
						case 2:
							this.GetOpenConnection().DoomThisConnection();
							this.NotifyWeakReference(-1);
							this.Close();
							break;
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06001E3A RID: 7738 RVA: 0x0025678C File Offset: 0x00255B8C
		// (remove) Token: 0x06001E3B RID: 7739 RVA: 0x002567AC File Offset: 0x00255BAC
		[ResCategory("DataCategory_InfoMessage")]
		[ResDescription("DbConnection_InfoMessage")]
		public event OleDbInfoMessageEventHandler InfoMessage
		{
			add
			{
				base.Events.AddHandler(OleDbConnection.EventInfoMessage, value);
			}
			remove
			{
				base.Events.RemoveHandler(OleDbConnection.EventInfoMessage, value);
			}
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x002567CC File Offset: 0x00255BCC
		internal UnsafeNativeMethods.ICommandText ICommandText()
		{
			return this.GetOpenConnection().ICommandText();
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x002567E4 File Offset: 0x00255BE4
		private IDBPropertiesWrapper IDBProperties()
		{
			return this.GetOpenConnection().IDBProperties();
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x002567FC File Offset: 0x00255BFC
		internal IOpenRowsetWrapper IOpenRowset()
		{
			return this.GetOpenConnection().IOpenRowset();
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x00256814 File Offset: 0x00255C14
		internal int SqlSupport()
		{
			return this.OleDbConnectionStringValue.GetSqlSupport(this);
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x00256830 File Offset: 0x00255C30
		internal bool SupportMultipleResults()
		{
			return this.OleDbConnectionStringValue.GetSupportMultipleResults(this);
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0025684C File Offset: 0x00255C4C
		internal bool SupportIRow(OleDbCommand cmd)
		{
			return this.OleDbConnectionStringValue.GetSupportIRow(this, cmd);
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x00256868 File Offset: 0x00255C68
		internal int QuotedIdentifierCase()
		{
			object dataSourcePropertyValue = this.GetDataSourcePropertyValue(OleDbPropertySetGuid.DataSourceInfo, 100);
			int num;
			if (dataSourcePropertyValue is int)
			{
				num = (int)dataSourcePropertyValue;
			}
			else
			{
				num = -1;
			}
			return num;
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x00256898 File Offset: 0x00255C98
		public new OleDbTransaction BeginTransaction()
		{
			return this.BeginTransaction(IsolationLevel.Unspecified);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x002568AC File Offset: 0x00255CAC
		public new OleDbTransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			return (OleDbTransaction)this.InnerConnection.BeginTransaction(isolationLevel);
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x002568CC File Offset: 0x00255CCC
		public override void ChangeDatabase(string value)
		{
			OleDbConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbConnection.ChangeDatabase|API> %d#, value='%ls'\n", this.ObjectID, value);
			try
			{
				this.CheckStateOpen("ChangeDatabase");
				if (value == null || value.Trim().Length == 0)
				{
					throw ADP.EmptyDatabaseName();
				}
				this.SetDataSourcePropertyValue(OleDbPropertySetGuid.DataSource, 37, "current catalog", true, value);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x00256954 File Offset: 0x00255D54
		internal void CheckStateOpen(string method)
		{
			ConnectionState state = this.State;
			if (ConnectionState.Open != state)
			{
				throw ADP.OpenConnectionRequired(method, state);
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x00256974 File Offset: 0x00255D74
		object ICloneable.Clone()
		{
			OleDbConnection oleDbConnection = new OleDbConnection(this);
			Bid.Trace("<oledb.OleDbConnection.Clone|API> %d#, clone=%d#\n", this.ObjectID, oleDbConnection.ObjectID);
			return oleDbConnection;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x002569A0 File Offset: 0x00255DA0
		public override void Close()
		{
			this.InnerConnection.CloseConnection(this, this.ConnectionFactory);
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x002569C0 File Offset: 0x00255DC0
		public new OleDbCommand CreateCommand()
		{
			return new OleDbCommand("", this);
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x002569D8 File Offset: 0x00255DD8
		private void DisposeMe(bool disposing)
		{
			if (disposing && base.DesignMode)
			{
				OleDbConnection.ReleaseObjectPool();
			}
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x002569F8 File Offset: 0x00255DF8
		public void EnlistDistributedTransaction(ITransaction transaction)
		{
			this.EnlistDistributedTransactionHelper(transaction);
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x00256A0C File Offset: 0x00255E0C
		internal object GetDataSourcePropertyValue(Guid propertySet, int propertyID)
		{
			OleDbConnectionInternal openConnection = this.GetOpenConnection();
			return openConnection.GetDataSourcePropertyValue(propertySet, propertyID);
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x00256A28 File Offset: 0x00255E28
		internal object GetDataSourceValue(Guid propertySet, int propertyID)
		{
			object obj = this.GetDataSourcePropertyValue(propertySet, propertyID);
			if (obj is OleDbPropertyStatus || Convert.IsDBNull(obj))
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x00256A54 File Offset: 0x00255E54
		private OleDbConnectionInternal GetOpenConnection()
		{
			DbConnectionInternal innerConnection = this.InnerConnection;
			return innerConnection as OleDbConnectionInternal;
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x00256A70 File Offset: 0x00255E70
		internal void GetLiteralQuotes(string method, out string quotePrefix, out string quoteSuffix)
		{
			this.CheckStateOpen(method);
			OleDbConnectionPoolGroupProviderInfo providerInfo = this.ProviderInfo;
			if (providerInfo.HasQuoteFix)
			{
				quotePrefix = providerInfo.QuotePrefix;
				quoteSuffix = providerInfo.QuoteSuffix;
				return;
			}
			OleDbConnectionInternal openConnection = this.GetOpenConnection();
			quotePrefix = openConnection.GetLiteralInfo(15);
			quoteSuffix = openConnection.GetLiteralInfo(28);
			if (quotePrefix == null)
			{
				quotePrefix = "";
			}
			if (quoteSuffix == null)
			{
				quoteSuffix = quotePrefix;
			}
			providerInfo.SetQuoteFix(quotePrefix, quoteSuffix);
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x00256ADC File Offset: 0x00255EDC
		public DataTable GetOleDbSchemaTable(Guid schema, object[] restrictions)
		{
			OleDbConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbConnection.GetOleDbSchemaTable|API> %d#, schema=%p{GUID}, restrictions\n", this.ObjectID, schema);
			DataTable dataTable;
			try
			{
				this.CheckStateOpen("GetOleDbSchemaTable");
				OleDbConnectionInternal openConnection = this.GetOpenConnection();
				if (OleDbSchemaGuid.DbInfoLiterals == schema)
				{
					if (restrictions != null && restrictions.Length != 0)
					{
						throw ODB.InvalidRestrictionsDbInfoLiteral("restrictions");
					}
					dataTable = openConnection.BuildInfoLiterals();
				}
				else if (OleDbSchemaGuid.SchemaGuids == schema)
				{
					if (restrictions != null && restrictions.Length != 0)
					{
						throw ODB.InvalidRestrictionsSchemaGuids("restrictions");
					}
					dataTable = openConnection.BuildSchemaGuids();
				}
				else if (OleDbSchemaGuid.DbInfoKeywords == schema)
				{
					if (restrictions != null && restrictions.Length != 0)
					{
						throw ODB.InvalidRestrictionsDbInfoKeywords("restrictions");
					}
					dataTable = openConnection.BuildInfoKeywords();
				}
				else
				{
					if (!openConnection.SupportSchemaRowset(schema))
					{
						using (IDBSchemaRowsetWrapper idbschemaRowsetWrapper = openConnection.IDBSchemaRowset())
						{
							if (idbschemaRowsetWrapper.Value == null)
							{
								throw ODB.SchemaRowsetsNotSupported(this.Provider);
							}
						}
						throw ODB.NotSupportedSchemaTable(schema, this);
					}
					dataTable = openConnection.GetSchemaRowset(schema, restrictions);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable;
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x00256C20 File Offset: 0x00256020
		internal DataTable GetSchemaRowset(Guid schema, object[] restrictions)
		{
			return this.GetOpenConnection().GetSchemaRowset(schema, restrictions);
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x00256C3C File Offset: 0x0025603C
		internal bool HasLiveReader(OleDbCommand cmd)
		{
			bool flag = false;
			OleDbConnectionInternal openConnection = this.GetOpenConnection();
			if (openConnection != null)
			{
				flag = openConnection.HasLiveReader(cmd);
			}
			return flag;
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x00256C60 File Offset: 0x00256060
		internal void OnInfoMessage(UnsafeNativeMethods.IErrorInfo errorInfo, OleDbHResult errorCode)
		{
			OleDbInfoMessageEventHandler oleDbInfoMessageEventHandler = (OleDbInfoMessageEventHandler)base.Events[OleDbConnection.EventInfoMessage];
			if (oleDbInfoMessageEventHandler != null)
			{
				try
				{
					OleDbException ex = OleDbException.CreateException(errorInfo, errorCode, null);
					OleDbInfoMessageEventArgs oleDbInfoMessageEventArgs = new OleDbInfoMessageEventArgs(ex);
					if (Bid.TraceOn)
					{
						Bid.Trace("<oledb.OledbConnection.OnInfoMessage|API|INFO> %d#, Message='%ls'\n", this.ObjectID, oleDbInfoMessageEventArgs.Message);
					}
					oleDbInfoMessageEventHandler(this, oleDbInfoMessageEventArgs);
				}
				catch (Exception ex2)
				{
					if (!ADP.IsCatchableOrSecurityExceptionType(ex2))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(ex2);
				}
			}
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x00256CEC File Offset: 0x002560EC
		public override void Open()
		{
			this.InnerConnection.OpenConnection(this, this.ConnectionFactory);
			if ((2 & ((OleDbConnectionString)this.ConnectionOptions).OleDbServices) != 0 && ADP.NeedManualEnlistment())
			{
				this.GetOpenConnection().EnlistTransactionInternal(Transaction.Current, true);
			}
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x00256D38 File Offset: 0x00256138
		internal void SetDataSourcePropertyValue(Guid propertySet, int propertyID, string description, bool required, object value)
		{
			this.CheckStateOpen("SetProperties");
			using (IDBPropertiesWrapper idbpropertiesWrapper = this.IDBProperties())
			{
				using (DBPropSet dbpropSet = DBPropSet.CreateProperty(propertySet, propertyID, required, value))
				{
					Bid.Trace("<oledb.IDBProperties.SetProperties|API|OLEDB> %d#\n", this.ObjectID);
					OleDbHResult oleDbHResult = idbpropertiesWrapper.Value.SetProperties(dbpropSet.PropertySetCount, dbpropSet);
					Bid.Trace("<oledb.IDBProperties.SetProperties|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
					if (oleDbHResult < OleDbHResult.S_OK)
					{
						Exception ex = OleDbConnection.ProcessResults(oleDbHResult, null, this);
						if (OleDbHResult.DB_E_ERRORSOCCURRED == oleDbHResult)
						{
							StringBuilder stringBuilder = new StringBuilder();
							tagDBPROP[] propertySet2 = dbpropSet.GetPropertySet(0, out propertySet);
							ODB.PropsetSetFailure(stringBuilder, description, propertySet2[0].dwStatus);
							ex = ODB.PropsetSetFailure(stringBuilder.ToString(), ex);
						}
						if (ex != null)
						{
							throw ex;
						}
					}
					else
					{
						SafeNativeMethods.Wrapper.ClearErrorInfo();
					}
				}
			}
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x00256E30 File Offset: 0x00256230
		internal bool SupportSchemaRowset(Guid schema)
		{
			return this.GetOpenConnection().SupportSchemaRowset(schema);
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x00256E4C File Offset: 0x0025624C
		internal OleDbTransaction ValidateTransaction(OleDbTransaction transaction, string method)
		{
			return this.GetOpenConnection().ValidateTransaction(transaction, method);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x00256E68 File Offset: 0x00256268
		internal static Exception ProcessResults(OleDbHResult hresult, OleDbConnection connection, object src)
		{
			if (OleDbHResult.S_OK <= hresult && (connection == null || connection.Events[OleDbConnection.EventInfoMessage] == null))
			{
				SafeNativeMethods.Wrapper.ClearErrorInfo();
				return null;
			}
			Exception ex = null;
			UnsafeNativeMethods.IErrorInfo errorInfo = null;
			if (UnsafeNativeMethods.GetErrorInfo(0, out errorInfo) == OleDbHResult.S_OK && errorInfo != null)
			{
				if (hresult < OleDbHResult.S_OK)
				{
					ex = OleDbException.CreateException(errorInfo, hresult, null);
					if (OleDbHResult.DB_E_OBJECTOPEN == hresult)
					{
						ex = ADP.OpenReaderExists(ex);
					}
					OleDbConnection.ResetState(connection);
				}
				else if (connection != null)
				{
					connection.OnInfoMessage(errorInfo, hresult);
				}
				else
				{
					Bid.Trace("<oledb.OledbConnection|WARN|INFO> ErrorInfo available, but not connection %08X{HRESULT}\n", hresult);
				}
				Marshal.ReleaseComObject(errorInfo);
			}
			else if (OleDbHResult.S_OK < hresult)
			{
				Bid.Trace("<oledb.OledbConnection|ERR|INFO> ErrorInfo not available %08X{HRESULT}\n", hresult);
			}
			else if (hresult < OleDbHResult.S_OK)
			{
				ex = ODB.NoErrorInformation((connection != null) ? connection.Provider : null, hresult, null);
				OleDbConnection.ResetState(connection);
			}
			if (ex != null)
			{
				ADP.TraceExceptionAsReturnValue(ex);
			}
			return ex;
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x00256F28 File Offset: 0x00256328
		public static void ReleaseObjectPool()
		{
			new OleDbPermission(PermissionState.Unrestricted).Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbConnection.ReleaseObjectPool|API>\n");
			try
			{
				OleDbConnectionString.ReleaseObjectPool();
				OleDbConnectionInternal.ReleaseObjectPool();
				OleDbConnectionFactory.SingletonInstance.ClearAllPools();
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x00256F88 File Offset: 0x00256388
		private static void ResetState(OleDbConnection connection)
		{
			if (connection != null)
			{
				connection.ResetState();
			}
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x00256FA0 File Offset: 0x002563A0
		public OleDbConnection()
		{
			GC.SuppressFinalize(this);
			this._innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x00256FD4 File Offset: 0x002563D4
		private void CopyFrom(OleDbConnection connection)
		{
			ADP.CheckArgumentNull(connection, "connection");
			this._userConnectionOptions = connection.UserConnectionOptions;
			this._poolGroup = connection.PoolGroup;
			if (DbConnectionClosedNeverOpened.SingletonInstance == connection._innerConnection)
			{
				this._innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
				return;
			}
			this._innerConnection = DbConnectionClosedPreviouslyOpened.SingletonInstance;
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x00257028 File Offset: 0x00256428
		internal int CloseCount
		{
			get
			{
				return this._closeCount;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001E5E RID: 7774 RVA: 0x0025703C File Offset: 0x0025643C
		internal DbConnectionFactory ConnectionFactory
		{
			get
			{
				return OleDbConnection._connectionFactory;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001E5F RID: 7775 RVA: 0x00257050 File Offset: 0x00256450
		internal DbConnectionOptions ConnectionOptions
		{
			get
			{
				DbConnectionPoolGroup poolGroup = this.PoolGroup;
				if (poolGroup == null)
				{
					return null;
				}
				return poolGroup.ConnectionOptions;
			}
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x00257070 File Offset: 0x00256470
		private string ConnectionString_Get()
		{
			Bid.Trace("<prov.DbConnectionHelper.ConnectionString_Get|API> %d#\n", this.ObjectID);
			bool shouldHidePassword = this.InnerConnection.ShouldHidePassword;
			DbConnectionOptions userConnectionOptions = this.UserConnectionOptions;
			if (userConnectionOptions == null)
			{
				return "";
			}
			return userConnectionOptions.UsersConnectionString(shouldHidePassword);
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x002570B0 File Offset: 0x002564B0
		private void ConnectionString_Set(string value)
		{
			DbConnectionOptions dbConnectionOptions = null;
			DbConnectionPoolGroup connectionPoolGroup = this.ConnectionFactory.GetConnectionPoolGroup(value, null, ref dbConnectionOptions);
			DbConnectionInternal innerConnection = this.InnerConnection;
			bool flag = innerConnection.AllowSetConnectionString;
			if (flag)
			{
				flag = this.SetInnerConnectionFrom(DbConnectionClosedBusy.SingletonInstance, innerConnection);
				if (flag)
				{
					this._userConnectionOptions = dbConnectionOptions;
					this._poolGroup = connectionPoolGroup;
					this._innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
				}
			}
			if (!flag)
			{
				throw ADP.OpenConnectionPropertySet("ConnectionString", innerConnection.State);
			}
			if (Bid.TraceOn)
			{
				string text = ((dbConnectionOptions != null) ? dbConnectionOptions.UsersConnectionStringForTrace() : "");
				Bid.Trace("<prov.DbConnectionHelper.ConnectionString_Set|API> %d#, '%ls'\n", this.ObjectID, text);
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x00257148 File Offset: 0x00256548
		internal DbConnectionInternal InnerConnection
		{
			get
			{
				return this._innerConnection;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x0025715C File Offset: 0x0025655C
		// (set) Token: 0x06001E64 RID: 7780 RVA: 0x00257170 File Offset: 0x00256570
		internal DbConnectionPoolGroup PoolGroup
		{
			get
			{
				return this._poolGroup;
			}
			set
			{
				this._poolGroup = value;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x00257184 File Offset: 0x00256584
		[Browsable(false)]
		[ResDescription("DbConnection_State")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override ConnectionState State
		{
			get
			{
				return this.InnerConnection.State;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001E66 RID: 7782 RVA: 0x0025719C File Offset: 0x0025659C
		internal DbConnectionOptions UserConnectionOptions
		{
			get
			{
				return this._userConnectionOptions;
			}
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x002571B0 File Offset: 0x002565B0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Abort(Exception e)
		{
			DbConnectionInternal innerConnection = this._innerConnection;
			if (ConnectionState.Open == innerConnection.State)
			{
				Interlocked.CompareExchange<DbConnectionInternal>(ref this._innerConnection, DbConnectionClosedPreviouslyOpened.SingletonInstance, innerConnection);
				innerConnection.DoomThisConnection();
			}
			if (e is OutOfMemoryException)
			{
				Bid.Trace("<prov.DbConnectionHelper.Abort|RES|INFO|CPOOL> %d#, Aborting operation due to asynchronous exception: %ls\n", this.ObjectID, "OutOfMemory");
				return;
			}
			Bid.Trace("<prov.DbConnectionHelper.Abort|RES|INFO|CPOOL> %d#, Aborting operation due to asynchronous exception: %ls\n", this.ObjectID, e.ToString());
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0025721C File Offset: 0x0025661C
		internal void AddWeakReference(object value, int tag)
		{
			this.InnerConnection.AddWeakReference(value, tag);
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x00257238 File Offset: 0x00256638
		protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<prov.DbConnectionHelper.BeginDbTransaction|API> %d#, isolationLevel=%d{ds.IsolationLevel}", this.ObjectID, (int)isolationLevel);
			DbTransaction dbTransaction2;
			try
			{
				DbTransaction dbTransaction = this.InnerConnection.BeginTransaction(isolationLevel);
				dbTransaction2 = dbTransaction;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dbTransaction2;
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x00257290 File Offset: 0x00256690
		protected override DbCommand CreateDbCommand()
		{
			DbCommand dbCommand = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<prov.DbConnectionHelper.CreateDbCommand|API> %d#\n", this.ObjectID);
			try
			{
				DbProviderFactory providerFactory = this.ConnectionFactory.ProviderFactory;
				dbCommand = providerFactory.CreateCommand();
				dbCommand.Connection = this;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dbCommand;
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x002572F4 File Offset: 0x002566F4
		private static CodeAccessPermission CreateExecutePermission()
		{
			DBDataPermission dbdataPermission = (DBDataPermission)OleDbConnectionFactory.SingletonInstance.ProviderFactory.CreatePermission(PermissionState.None);
			dbdataPermission.Add(string.Empty, string.Empty, KeyRestrictionBehavior.AllowOnly);
			return dbdataPermission;
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0025732C File Offset: 0x0025672C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._userConnectionOptions = null;
				this._poolGroup = null;
				this.Close();
			}
			this.DisposeMe(disposing);
			base.Dispose(disposing);
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x00257360 File Offset: 0x00256760
		private void EnlistDistributedTransactionHelper(ITransaction transaction)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(OleDbConnection.ExecutePermission);
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.Demand();
			Bid.Trace("<prov.DbConnectionHelper.EnlistDistributedTransactionHelper|RES|TRAN> %d#, Connection enlisting in a transaction.\n", this.ObjectID);
			Transaction transaction2 = null;
			if (transaction != null)
			{
				transaction2 = TransactionInterop.GetTransactionFromDtcTransaction((IDtcTransaction)transaction);
			}
			this.InnerConnection.EnlistTransaction(transaction2);
			GC.KeepAlive(this);
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x002573C8 File Offset: 0x002567C8
		public override void EnlistTransaction(Transaction transaction)
		{
			OleDbConnection.ExecutePermission.Demand();
			Bid.Trace("<prov.DbConnectionHelper.EnlistTransaction|RES|TRAN> %d#, Connection enlisting in a transaction.\n", this.ObjectID);
			DbConnectionInternal innerConnection = this.InnerConnection;
			if (!innerConnection.HasEnlistedTransaction)
			{
				innerConnection.EnlistTransaction(transaction);
				GC.KeepAlive(this);
				return;
			}
			if (innerConnection.EnlistedTransaction.Equals(transaction))
			{
				return;
			}
			throw ADP.TransactionPresent();
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x00257420 File Offset: 0x00256820
		private DbMetaDataFactory GetMetaDataFactory(DbConnectionInternal internalConnection)
		{
			return this.ConnectionFactory.GetMetaDataFactory(this._poolGroup, internalConnection);
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x00257440 File Offset: 0x00256840
		internal DbMetaDataFactory GetMetaDataFactoryInternal(DbConnectionInternal internalConnection)
		{
			return this.GetMetaDataFactory(internalConnection);
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x00257454 File Offset: 0x00256854
		public override DataTable GetSchema()
		{
			return this.GetSchema(DbMetaDataCollectionNames.MetaDataCollections, null);
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x00257470 File Offset: 0x00256870
		public override DataTable GetSchema(string collectionName)
		{
			return this.GetSchema(collectionName, null);
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x00257488 File Offset: 0x00256888
		public override DataTable GetSchema(string collectionName, string[] restrictionValues)
		{
			OleDbConnection.ExecutePermission.Demand();
			return this.InnerConnection.GetSchema(this.ConnectionFactory, this.PoolGroup, this, collectionName, restrictionValues);
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x002574BC File Offset: 0x002568BC
		internal void NotifyWeakReference(int message)
		{
			this.InnerConnection.NotifyWeakReference(message);
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x002574D8 File Offset: 0x002568D8
		internal void PermissionDemand()
		{
			DbConnectionPoolGroup poolGroup = this.PoolGroup;
			DbConnectionOptions dbConnectionOptions = ((poolGroup != null) ? poolGroup.ConnectionOptions : null);
			if (dbConnectionOptions == null || dbConnectionOptions.IsEmpty)
			{
				throw ADP.NoConnectionString();
			}
			DbConnectionOptions userConnectionOptions = this.UserConnectionOptions;
			userConnectionOptions.DemandPermission();
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x00257518 File Offset: 0x00256918
		internal void RemoveWeakReference(object value)
		{
			this.InnerConnection.RemoveWeakReference(value);
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x00257534 File Offset: 0x00256934
		internal void SetInnerConnectionEvent(DbConnectionInternal to)
		{
			ConnectionState connectionState = this._innerConnection.State & ConnectionState.Open;
			ConnectionState connectionState2 = to.State & ConnectionState.Open;
			if (connectionState != connectionState2 && connectionState2 == ConnectionState.Closed)
			{
				this._closeCount++;
			}
			this._innerConnection = to;
			if (connectionState == ConnectionState.Closed && ConnectionState.Open == connectionState2)
			{
				this.OnStateChange(DbConnectionInternal.StateChangeOpen);
				return;
			}
			if (ConnectionState.Open == connectionState && connectionState2 == ConnectionState.Closed)
			{
				this.OnStateChange(DbConnectionInternal.StateChangeClosed);
				return;
			}
			if (connectionState != connectionState2)
			{
				this.OnStateChange(new StateChangeEventArgs(connectionState, connectionState2));
			}
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x002575AC File Offset: 0x002569AC
		internal bool SetInnerConnectionFrom(DbConnectionInternal to, DbConnectionInternal from)
		{
			return from == Interlocked.CompareExchange<DbConnectionInternal>(ref this._innerConnection, to, from);
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x002575CC File Offset: 0x002569CC
		internal void SetInnerConnectionTo(DbConnectionInternal to)
		{
			this._innerConnection = to;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x002575E0 File Offset: 0x002569E0
		[Conditional("DEBUG")]
		internal static void VerifyExecutePermission()
		{
			try
			{
				OleDbConnection.ExecutePermission.Demand();
			}
			catch (SecurityException)
			{
				throw;
			}
		}

		// Token: 0x0400125C RID: 4700
		private static readonly object EventInfoMessage = new object();

		// Token: 0x0400125D RID: 4701
		private static readonly DbConnectionFactory _connectionFactory = OleDbConnectionFactory.SingletonInstance;

		// Token: 0x0400125E RID: 4702
		internal static readonly CodeAccessPermission ExecutePermission = OleDbConnection.CreateExecutePermission();

		// Token: 0x0400125F RID: 4703
		private DbConnectionOptions _userConnectionOptions;

		// Token: 0x04001260 RID: 4704
		private DbConnectionPoolGroup _poolGroup;

		// Token: 0x04001261 RID: 4705
		private DbConnectionInternal _innerConnection;

		// Token: 0x04001262 RID: 4706
		private int _closeCount;

		// Token: 0x04001263 RID: 4707
		private static int _objectTypeCount;

		// Token: 0x04001264 RID: 4708
		internal readonly int ObjectID = Interlocked.Increment(ref OleDbConnection._objectTypeCount);
	}
}
