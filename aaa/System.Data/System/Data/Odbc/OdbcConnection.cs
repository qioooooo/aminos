using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Transactions;

namespace System.Data.Odbc
{
	// Token: 0x020001D6 RID: 470
	[DefaultEvent("InfoMessage")]
	public sealed class OdbcConnection : DbConnection, ICloneable
	{
		// Token: 0x060019D9 RID: 6617 RVA: 0x00240EC8 File Offset: 0x002402C8
		public OdbcConnection(string connectionString)
			: this()
		{
			this.ConnectionString = connectionString;
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00240EE4 File Offset: 0x002402E4
		private OdbcConnection(OdbcConnection connection)
			: this()
		{
			this.CopyFrom(connection);
			this.connectionTimeout = connection.connectionTimeout;
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060019DB RID: 6619 RVA: 0x00240F0C File Offset: 0x0024030C
		// (set) Token: 0x060019DC RID: 6620 RVA: 0x00240F20 File Offset: 0x00240320
		internal OdbcConnectionHandle ConnectionHandle
		{
			get
			{
				return this._connectionHandle;
			}
			set
			{
				this._connectionHandle = value;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060019DD RID: 6621 RVA: 0x00240F34 File Offset: 0x00240334
		// (set) Token: 0x060019DE RID: 6622 RVA: 0x00240F48 File Offset: 0x00240348
		[RecommendedAsConfigurable(true)]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Data")]
		[Editor("Microsoft.VSDesigner.Data.Odbc.Design.OdbcConnectionStringEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue("")]
		[ResDescription("OdbcConnection_ConnectionString")]
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

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060019DF RID: 6623 RVA: 0x00240F5C File Offset: 0x0024035C
		// (set) Token: 0x060019E0 RID: 6624 RVA: 0x00240F70 File Offset: 0x00240370
		[DefaultValue(15)]
		[ResCategory("DataCategory_Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ResDescription("OdbcConnection_ConnectionTimeout")]
		public new int ConnectionTimeout
		{
			get
			{
				return this.connectionTimeout;
			}
			set
			{
				if (value < 0)
				{
					throw ODBC.NegativeArgument();
				}
				if (this.IsOpen)
				{
					throw ODBC.CantSetPropertyOnOpenConnection();
				}
				this.connectionTimeout = value;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x00240F9C File Offset: 0x0024039C
		[ResDescription("OdbcConnection_Database")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Database
		{
			get
			{
				if (this.IsOpen && !this.ProviderInfo.NoCurrentCatalog)
				{
					return this.GetConnectAttrString(ODBC32.SQL_ATTR.CURRENT_CATALOG);
				}
				return string.Empty;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060019E2 RID: 6626 RVA: 0x00240FCC File Offset: 0x002403CC
		[ResDescription("OdbcConnection_DataSource")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string DataSource
		{
			get
			{
				if (this.IsOpen)
				{
					return this.GetInfoStringUnhandled(ODBC32.SQL_INFO.SERVER_NAME, true);
				}
				return string.Empty;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x00240FF0 File Offset: 0x002403F0
		[ResDescription("OdbcConnection_ServerVersion")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override string ServerVersion
		{
			get
			{
				return this.InnerConnection.ServerVersion;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060019E4 RID: 6628 RVA: 0x00241008 File Offset: 0x00240408
		internal OdbcConnectionPoolGroupProviderInfo ProviderInfo
		{
			get
			{
				return (OdbcConnectionPoolGroupProviderInfo)this.PoolGroup.ProviderInfo;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060019E5 RID: 6629 RVA: 0x00241028 File Offset: 0x00240428
		internal ConnectionState InternalState
		{
			get
			{
				return this.State | this._extraState;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060019E6 RID: 6630 RVA: 0x00241044 File Offset: 0x00240444
		internal bool IsOpen
		{
			get
			{
				return this.InnerConnection is OdbcConnectionOpen;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x00241060 File Offset: 0x00240460
		// (set) Token: 0x060019E8 RID: 6632 RVA: 0x0024108C File Offset: 0x0024048C
		internal OdbcTransaction LocalTransaction
		{
			get
			{
				OdbcTransaction odbcTransaction = null;
				if (this.weakTransaction != null)
				{
					odbcTransaction = (OdbcTransaction)this.weakTransaction.Target;
				}
				return odbcTransaction;
			}
			set
			{
				this.weakTransaction = null;
				if (value != null)
				{
					this.weakTransaction = new WeakReference(value);
				}
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060019E9 RID: 6633 RVA: 0x002410B0 File Offset: 0x002404B0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[ResDescription("OdbcConnection_Driver")]
		public string Driver
		{
			get
			{
				if (this.IsOpen)
				{
					if (this.ProviderInfo.DriverName == null)
					{
						this.ProviderInfo.DriverName = this.GetInfoStringUnhandled(ODBC32.SQL_INFO.DRIVER_NAME);
					}
					return this.ProviderInfo.DriverName;
				}
				return ADP.StrEmpty;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060019EA RID: 6634 RVA: 0x002410F8 File Offset: 0x002404F8
		internal bool IsV3Driver
		{
			get
			{
				if (this.ProviderInfo.DriverVersion == null)
				{
					this.ProviderInfo.DriverVersion = this.GetInfoStringUnhandled(ODBC32.SQL_INFO.DRIVER_ODBC_VER);
					if (this.ProviderInfo.DriverVersion != null && this.ProviderInfo.DriverVersion.Length >= 2)
					{
						try
						{
							this.ProviderInfo.IsV3Driver = int.Parse(this.ProviderInfo.DriverVersion.Substring(0, 2), CultureInfo.InvariantCulture) >= 3;
							goto IL_0097;
						}
						catch (FormatException ex)
						{
							this.ProviderInfo.IsV3Driver = false;
							ADP.TraceExceptionWithoutRethrow(ex);
							goto IL_0097;
						}
					}
					this.ProviderInfo.DriverVersion = "";
				}
				IL_0097:
				return this.ProviderInfo.IsV3Driver;
			}
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x060019EB RID: 6635 RVA: 0x002411C4 File Offset: 0x002405C4
		// (remove) Token: 0x060019EC RID: 6636 RVA: 0x002411E8 File Offset: 0x002405E8
		[ResDescription("DbConnection_InfoMessage")]
		[ResCategory("DataCategory_InfoMessage")]
		public event OdbcInfoMessageEventHandler InfoMessage
		{
			add
			{
				this.infoMessageEventHandler = (OdbcInfoMessageEventHandler)Delegate.Combine(this.infoMessageEventHandler, value);
			}
			remove
			{
				this.infoMessageEventHandler = (OdbcInfoMessageEventHandler)Delegate.Remove(this.infoMessageEventHandler, value);
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x0024120C File Offset: 0x0024060C
		internal char EscapeChar(string method)
		{
			this.CheckState(method);
			if (!this.ProviderInfo.HasEscapeChar)
			{
				string infoStringUnhandled = this.GetInfoStringUnhandled(ODBC32.SQL_INFO.SEARCH_PATTERN_ESCAPE);
				this.ProviderInfo.EscapeChar = ((infoStringUnhandled.Length == 1) ? infoStringUnhandled[0] : this.QuoteChar(method)[0]);
			}
			return this.ProviderInfo.EscapeChar;
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x0024126C File Offset: 0x0024066C
		internal string QuoteChar(string method)
		{
			this.CheckState(method);
			if (!this.ProviderInfo.HasQuoteChar)
			{
				string infoStringUnhandled = this.GetInfoStringUnhandled(ODBC32.SQL_INFO.IDENTIFIER_QUOTE_CHAR);
				this.ProviderInfo.QuoteChar = ((1 == infoStringUnhandled.Length) ? infoStringUnhandled : "\0");
			}
			return this.ProviderInfo.QuoteChar;
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x002412C0 File Offset: 0x002406C0
		public new OdbcTransaction BeginTransaction()
		{
			return this.BeginTransaction(IsolationLevel.Unspecified);
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x002412D4 File Offset: 0x002406D4
		public new OdbcTransaction BeginTransaction(IsolationLevel isolevel)
		{
			return (OdbcTransaction)this.InnerConnection.BeginTransaction(isolevel);
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x002412F4 File Offset: 0x002406F4
		private void RollbackDeadTransaction()
		{
			WeakReference weakReference = this.weakTransaction;
			if (weakReference != null && !weakReference.IsAlive)
			{
				this.weakTransaction = null;
				this.ConnectionHandle.CompleteTransaction(1);
			}
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x00241328 File Offset: 0x00240728
		public override void ChangeDatabase(string value)
		{
			this.InnerConnection.ChangeDatabase(value);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00241344 File Offset: 0x00240744
		internal void CheckState(string method)
		{
			ConnectionState internalState = this.InternalState;
			if (ConnectionState.Open != internalState)
			{
				throw ADP.OpenConnectionRequired(method, internalState);
			}
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x00241364 File Offset: 0x00240764
		object ICloneable.Clone()
		{
			OdbcConnection odbcConnection = new OdbcConnection(this);
			Bid.Trace("<odbc.OdbcConnection.Clone|API> %d#, clone=%d#\n", this.ObjectID, odbcConnection.ObjectID);
			return odbcConnection;
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x00241390 File Offset: 0x00240790
		internal bool ConnectionIsAlive(Exception innerException)
		{
			if (this.IsOpen)
			{
				if (!this.ProviderInfo.NoConnectionDead)
				{
					int connectAttr = this.GetConnectAttr(ODBC32.SQL_ATTR.CONNECTION_DEAD, ODBC32.HANDLER.IGNORE);
					if (1 == connectAttr)
					{
						this.Close();
						throw ADP.ConnectionIsDisabled(innerException);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x002413D4 File Offset: 0x002407D4
		public new OdbcCommand CreateCommand()
		{
			return new OdbcCommand(string.Empty, this);
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x002413EC File Offset: 0x002407EC
		internal OdbcStatementHandle CreateStatementHandle()
		{
			return new OdbcStatementHandle(this.ConnectionHandle);
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x00241404 File Offset: 0x00240804
		public override void Close()
		{
			this.InnerConnection.CloseConnection(this, this.ConnectionFactory);
			OdbcConnectionHandle connectionHandle = this._connectionHandle;
			if (connectionHandle != null)
			{
				this._connectionHandle = null;
				WeakReference weakReference = this.weakTransaction;
				if (weakReference != null)
				{
					this.weakTransaction = null;
					IDisposable disposable = weakReference.Target as OdbcTransaction;
					if (disposable != null && weakReference.IsAlive)
					{
						disposable.Dispose();
					}
				}
				connectionHandle.Dispose();
			}
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x00241468 File Offset: 0x00240868
		private void DisposeMe(bool disposing)
		{
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x00241478 File Offset: 0x00240878
		public void EnlistDistributedTransaction(ITransaction transaction)
		{
			this.EnlistDistributedTransactionHelper(transaction);
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0024148C File Offset: 0x0024088C
		internal string GetConnectAttrString(ODBC32.SQL_ATTR attribute)
		{
			string text = "";
			int num = 0;
			byte[] array = new byte[100];
			OdbcConnectionHandle connectionHandle = this.ConnectionHandle;
			if (connectionHandle != null)
			{
				ODBC32.RetCode retCode = connectionHandle.GetConnectionAttribute(attribute, array, out num);
				if (array.Length + 2 <= num)
				{
					array = new byte[num + 2];
					retCode = connectionHandle.GetConnectionAttribute(attribute, array, out num);
				}
				if (retCode == ODBC32.RetCode.SUCCESS || ODBC32.RetCode.SUCCESS_WITH_INFO == retCode)
				{
					text = Encoding.Unicode.GetString(array, 0, Math.Min(num, array.Length));
				}
				else if (retCode == ODBC32.RetCode.ERROR)
				{
					string diagSqlState = this.GetDiagSqlState();
					if ("HYC00" == diagSqlState || "HY092" == diagSqlState || "IM001" == diagSqlState)
					{
						this.FlagUnsupportedConnectAttr(attribute);
					}
				}
			}
			return text;
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x00241540 File Offset: 0x00240940
		internal int GetConnectAttr(ODBC32.SQL_ATTR attribute, ODBC32.HANDLER handler)
		{
			int num = -1;
			int num2 = 0;
			byte[] array = new byte[4];
			OdbcConnectionHandle connectionHandle = this.ConnectionHandle;
			if (connectionHandle != null)
			{
				ODBC32.RetCode connectionAttribute = connectionHandle.GetConnectionAttribute(attribute, array, out num2);
				if (connectionAttribute == ODBC32.RetCode.SUCCESS || ODBC32.RetCode.SUCCESS_WITH_INFO == connectionAttribute)
				{
					num = BitConverter.ToInt32(array, 0);
				}
				else
				{
					if (connectionAttribute == ODBC32.RetCode.ERROR)
					{
						string diagSqlState = this.GetDiagSqlState();
						if ("HYC00" == diagSqlState || "HY092" == diagSqlState || "IM001" == diagSqlState)
						{
							this.FlagUnsupportedConnectAttr(attribute);
						}
					}
					if (handler == ODBC32.HANDLER.THROW)
					{
						this.HandleError(connectionHandle, connectionAttribute);
					}
				}
			}
			return num;
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x002415C8 File Offset: 0x002409C8
		private string GetDiagSqlState()
		{
			OdbcConnectionHandle connectionHandle = this.ConnectionHandle;
			string text;
			connectionHandle.GetDiagnosticField(out text);
			return text;
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x002415E8 File Offset: 0x002409E8
		internal ODBC32.RetCode GetInfoInt16Unhandled(ODBC32.SQL_INFO info, out short resultValue)
		{
			byte[] array = new byte[2];
			ODBC32.RetCode info2 = this.ConnectionHandle.GetInfo1(info, array);
			resultValue = BitConverter.ToInt16(array, 0);
			return info2;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x00241614 File Offset: 0x00240A14
		internal ODBC32.RetCode GetInfoInt32Unhandled(ODBC32.SQL_INFO info, out int resultValue)
		{
			byte[] array = new byte[4];
			ODBC32.RetCode info2 = this.ConnectionHandle.GetInfo1(info, array);
			resultValue = BitConverter.ToInt32(array, 0);
			return info2;
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x00241640 File Offset: 0x00240A40
		private int GetInfoInt32Unhandled(ODBC32.SQL_INFO infotype)
		{
			byte[] array = new byte[4];
			this.ConnectionHandle.GetInfo1(infotype, array);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0024166C File Offset: 0x00240A6C
		internal string GetInfoStringUnhandled(ODBC32.SQL_INFO info)
		{
			return this.GetInfoStringUnhandled(info, false);
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x00241684 File Offset: 0x00240A84
		private string GetInfoStringUnhandled(ODBC32.SQL_INFO info, bool handleError)
		{
			string text = null;
			short num = 0;
			byte[] array = new byte[100];
			OdbcConnectionHandle connectionHandle = this.ConnectionHandle;
			if (connectionHandle != null)
			{
				ODBC32.RetCode retCode = connectionHandle.GetInfo2(info, array, out num);
				if (array.Length < (int)(num - 2))
				{
					array = new byte[(int)(num + 2)];
					retCode = connectionHandle.GetInfo2(info, array, out num);
				}
				if (retCode == ODBC32.RetCode.SUCCESS || retCode == ODBC32.RetCode.SUCCESS_WITH_INFO)
				{
					text = Encoding.Unicode.GetString(array, 0, Math.Min((int)num, array.Length));
				}
				else if (handleError)
				{
					this.HandleError(this.ConnectionHandle, retCode);
				}
			}
			else if (handleError)
			{
				text = "";
			}
			return text;
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00241710 File Offset: 0x00240B10
		internal Exception HandleErrorNoThrow(OdbcHandle hrHandle, ODBC32.RetCode retcode)
		{
			switch (retcode)
			{
			case ODBC32.RetCode.SUCCESS:
				break;
			case ODBC32.RetCode.SUCCESS_WITH_INFO:
				if (this.infoMessageEventHandler != null)
				{
					OdbcErrorCollection diagErrors = ODBC32.GetDiagErrors(null, hrHandle, retcode);
					diagErrors.SetSource(this.Driver);
					this.OnInfoMessage(new OdbcInfoMessageEventArgs(diagErrors));
				}
				break;
			default:
			{
				OdbcException ex = OdbcException.CreateException(ODBC32.GetDiagErrors(null, hrHandle, retcode), retcode);
				if (ex != null)
				{
					ex.Errors.SetSource(this.Driver);
				}
				this.ConnectionIsAlive(ex);
				return ex;
			}
			}
			return null;
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x00241788 File Offset: 0x00240B88
		internal void HandleError(OdbcHandle hrHandle, ODBC32.RetCode retcode)
		{
			Exception ex = this.HandleErrorNoThrow(hrHandle, retcode);
			switch (retcode)
			{
			case ODBC32.RetCode.SUCCESS:
			case ODBC32.RetCode.SUCCESS_WITH_INFO:
				return;
			default:
				throw ex;
			}
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x002417B0 File Offset: 0x00240BB0
		public override void Open()
		{
			this.InnerConnection.OpenConnection(this, this.ConnectionFactory);
			if (ADP.NeedManualEnlistment())
			{
				this.EnlistTransaction(Transaction.Current);
			}
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x002417E4 File Offset: 0x00240BE4
		private void OnInfoMessage(OdbcInfoMessageEventArgs args)
		{
			if (this.infoMessageEventHandler != null)
			{
				try
				{
					this.infoMessageEventHandler(this, args);
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableOrSecurityExceptionType(ex))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(ex);
				}
			}
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00241838 File Offset: 0x00240C38
		public static void ReleaseObjectPool()
		{
			new OdbcPermission(PermissionState.Unrestricted).Demand();
			OdbcEnvironment.ReleaseObjectPool();
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x00241858 File Offset: 0x00240C58
		internal OdbcTransaction SetStateExecuting(string method, OdbcTransaction transaction)
		{
			if (this.weakTransaction != null)
			{
				OdbcTransaction odbcTransaction = this.weakTransaction.Target as OdbcTransaction;
				if (transaction != odbcTransaction)
				{
					if (transaction == null)
					{
						throw ADP.TransactionRequired(method);
					}
					if (this != transaction.Connection)
					{
						throw ADP.TransactionConnectionMismatch();
					}
					transaction = null;
				}
			}
			else if (transaction != null)
			{
				if (transaction.Connection != null)
				{
					throw ADP.TransactionConnectionMismatch();
				}
				transaction = null;
			}
			ConnectionState connectionState = this.InternalState;
			if (ConnectionState.Open != connectionState)
			{
				this.NotifyWeakReference(1);
				connectionState = this.InternalState;
				if (ConnectionState.Open != connectionState)
				{
					if ((ConnectionState.Fetching & connectionState) != ConnectionState.Closed)
					{
						throw ADP.OpenReaderExists();
					}
					throw ADP.OpenConnectionRequired(method, connectionState);
				}
			}
			return transaction;
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x002418E8 File Offset: 0x00240CE8
		internal void SetSupportedType(ODBC32.SQL_TYPE sqltype)
		{
			ODBC32.SQL_CVT sql_CVT;
			switch (sqltype)
			{
			case ODBC32.SQL_TYPE.WLONGVARCHAR:
				sql_CVT = ODBC32.SQL_CVT.WLONGVARCHAR;
				break;
			case ODBC32.SQL_TYPE.WVARCHAR:
				sql_CVT = ODBC32.SQL_CVT.WVARCHAR;
				break;
			case ODBC32.SQL_TYPE.WCHAR:
				sql_CVT = ODBC32.SQL_CVT.WCHAR;
				break;
			default:
				if (sqltype != ODBC32.SQL_TYPE.NUMERIC)
				{
					return;
				}
				sql_CVT = ODBC32.SQL_CVT.NUMERIC;
				break;
			}
			this.ProviderInfo.TestedSQLTypes |= (int)sql_CVT;
			this.ProviderInfo.SupportedSQLTypes |= (int)sql_CVT;
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00241954 File Offset: 0x00240D54
		internal void FlagRestrictedSqlBindType(ODBC32.SQL_TYPE sqltype)
		{
			ODBC32.SQL_CVT sql_CVT;
			switch (sqltype)
			{
			case ODBC32.SQL_TYPE.NUMERIC:
				sql_CVT = ODBC32.SQL_CVT.NUMERIC;
				break;
			case ODBC32.SQL_TYPE.DECIMAL:
				sql_CVT = ODBC32.SQL_CVT.DECIMAL;
				break;
			default:
				return;
			}
			this.ProviderInfo.RestrictedSQLBindTypes |= (int)sql_CVT;
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00241990 File Offset: 0x00240D90
		internal void FlagUnsupportedConnectAttr(ODBC32.SQL_ATTR Attribute)
		{
			if (Attribute == ODBC32.SQL_ATTR.CURRENT_CATALOG)
			{
				this.ProviderInfo.NoCurrentCatalog = true;
				return;
			}
			if (Attribute != ODBC32.SQL_ATTR.CONNECTION_DEAD)
			{
				return;
			}
			this.ProviderInfo.NoConnectionDead = true;
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x002419C8 File Offset: 0x00240DC8
		internal void FlagUnsupportedStmtAttr(ODBC32.SQL_ATTR Attribute)
		{
			if (Attribute == ODBC32.SQL_ATTR.QUERY_TIMEOUT)
			{
				this.ProviderInfo.NoQueryTimeout = true;
				return;
			}
			switch (Attribute)
			{
			case (ODBC32.SQL_ATTR)1227:
				this.ProviderInfo.NoSqlSoptSSHiddenColumns = true;
				return;
			case (ODBC32.SQL_ATTR)1228:
				this.ProviderInfo.NoSqlSoptSSNoBrowseTable = true;
				return;
			default:
				return;
			}
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x00241A18 File Offset: 0x00240E18
		internal void FlagUnsupportedColAttr(ODBC32.SQL_DESC v3FieldId, ODBC32.SQL_COLUMN v2FieldId)
		{
			if (this.IsV3Driver)
			{
				if (v3FieldId != (ODBC32.SQL_DESC)1212)
				{
					return;
				}
				this.ProviderInfo.NoSqlCASSColumnKey = true;
			}
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x00241A44 File Offset: 0x00240E44
		internal bool SQLGetFunctions(ODBC32.SQL_API odbcFunction)
		{
			OdbcConnectionHandle connectionHandle = this.ConnectionHandle;
			if (connectionHandle != null)
			{
				short num;
				ODBC32.RetCode functions = connectionHandle.GetFunctions(odbcFunction, out num);
				if (functions != ODBC32.RetCode.SUCCESS)
				{
					this.HandleError(connectionHandle, functions);
				}
				return num != 0;
			}
			throw ADP.InvalidOperation("what is the right exception to throw here?");
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x00241A84 File Offset: 0x00240E84
		internal bool TestTypeSupport(ODBC32.SQL_TYPE sqltype)
		{
			ODBC32.SQL_CONVERT sql_CONVERT;
			ODBC32.SQL_CVT sql_CVT;
			switch (sqltype)
			{
			case ODBC32.SQL_TYPE.WLONGVARCHAR:
				sql_CONVERT = ODBC32.SQL_CONVERT.LONGVARCHAR;
				sql_CVT = ODBC32.SQL_CVT.WLONGVARCHAR;
				break;
			case ODBC32.SQL_TYPE.WVARCHAR:
				sql_CONVERT = ODBC32.SQL_CONVERT.VARCHAR;
				sql_CVT = ODBC32.SQL_CVT.WVARCHAR;
				break;
			case ODBC32.SQL_TYPE.WCHAR:
				sql_CONVERT = ODBC32.SQL_CONVERT.CHAR;
				sql_CVT = ODBC32.SQL_CVT.WCHAR;
				break;
			default:
				if (sqltype != ODBC32.SQL_TYPE.NUMERIC)
				{
					return false;
				}
				sql_CONVERT = ODBC32.SQL_CONVERT.NUMERIC;
				sql_CVT = ODBC32.SQL_CVT.NUMERIC;
				break;
			}
			if ((this.ProviderInfo.TestedSQLTypes & (int)sql_CVT) == 0)
			{
				int num = this.GetInfoInt32Unhandled((ODBC32.SQL_INFO)sql_CONVERT);
				num &= (int)sql_CVT;
				this.ProviderInfo.TestedSQLTypes |= (int)sql_CVT;
				this.ProviderInfo.SupportedSQLTypes |= num;
			}
			return 0 != (this.ProviderInfo.SupportedSQLTypes & (int)sql_CVT);
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00241B2C File Offset: 0x00240F2C
		internal bool TestRestrictedSqlBindType(ODBC32.SQL_TYPE sqltype)
		{
			ODBC32.SQL_CVT sql_CVT;
			switch (sqltype)
			{
			case ODBC32.SQL_TYPE.NUMERIC:
				sql_CVT = ODBC32.SQL_CVT.NUMERIC;
				break;
			case ODBC32.SQL_TYPE.DECIMAL:
				sql_CVT = ODBC32.SQL_CVT.DECIMAL;
				break;
			default:
				return false;
			}
			return 0 != (this.ProviderInfo.RestrictedSQLBindTypes & (int)sql_CVT);
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x00241B6C File Offset: 0x00240F6C
		internal OdbcTransaction Open_BeginTransaction(IsolationLevel isolevel)
		{
			OdbcConnection.ExecutePermission.Demand();
			this.CheckState("BeginTransaction");
			this.RollbackDeadTransaction();
			if (this.weakTransaction != null && this.weakTransaction.IsAlive)
			{
				throw ADP.ParallelTransactionsNotSupported(this);
			}
			IsolationLevel isolationLevel = isolevel;
			if (isolationLevel <= IsolationLevel.ReadUncommitted)
			{
				if (isolationLevel == IsolationLevel.Unspecified)
				{
					goto IL_008E;
				}
				if (isolationLevel == IsolationLevel.Chaos)
				{
					throw ODBC.NotSupportedIsolationLevel(isolevel);
				}
				if (isolationLevel == IsolationLevel.ReadUncommitted)
				{
					goto IL_008E;
				}
			}
			else if (isolationLevel <= IsolationLevel.RepeatableRead)
			{
				if (isolationLevel == IsolationLevel.ReadCommitted || isolationLevel == IsolationLevel.RepeatableRead)
				{
					goto IL_008E;
				}
			}
			else if (isolationLevel == IsolationLevel.Serializable || isolationLevel == IsolationLevel.Snapshot)
			{
				goto IL_008E;
			}
			throw ADP.InvalidIsolationLevel(isolevel);
			IL_008E:
			OdbcConnectionHandle connectionHandle = this.ConnectionHandle;
			ODBC32.RetCode retCode = connectionHandle.BeginTransaction(ref isolevel);
			if (retCode == ODBC32.RetCode.ERROR)
			{
				this.HandleError(connectionHandle, retCode);
			}
			OdbcTransaction odbcTransaction = new OdbcTransaction(this, isolevel, connectionHandle);
			this.weakTransaction = new WeakReference(odbcTransaction);
			return odbcTransaction;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x00241C3C File Offset: 0x0024103C
		internal void Open_ChangeDatabase(string value)
		{
			OdbcConnection.ExecutePermission.Demand();
			this.CheckState("ChangeDatabase");
			if (value == null || value.Trim().Length == 0)
			{
				throw ADP.EmptyDatabaseName();
			}
			if (1024 < value.Length * 2 + 2)
			{
				throw ADP.DatabaseNameTooLong();
			}
			this.RollbackDeadTransaction();
			OdbcConnectionHandle connectionHandle = this.ConnectionHandle;
			ODBC32.RetCode retCode = connectionHandle.SetConnectionAttribute3(ODBC32.SQL_ATTR.CURRENT_CATALOG, value, checked(value.Length * 2));
			if (retCode != ODBC32.RetCode.SUCCESS)
			{
				this.HandleError(connectionHandle, retCode);
			}
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x00241CB8 File Offset: 0x002410B8
		internal void Open_EnlistTransaction(Transaction transaction)
		{
			if (this.weakTransaction != null && this.weakTransaction.IsAlive)
			{
				throw ADP.LocalTransactionPresent();
			}
			IDtcTransaction oletxTransaction = ADP.GetOletxTransaction(transaction);
			OdbcConnectionHandle connectionHandle = this.ConnectionHandle;
			ODBC32.RetCode retCode;
			if (oletxTransaction == null)
			{
				retCode = connectionHandle.SetConnectionAttribute2(ODBC32.SQL_ATTR.SQL_COPT_SS_ENLIST_IN_DTC, (IntPtr)0, 1);
			}
			else
			{
				retCode = connectionHandle.SetConnectionAttribute4(ODBC32.SQL_ATTR.SQL_COPT_SS_ENLIST_IN_DTC, oletxTransaction, 1);
			}
			if (retCode != ODBC32.RetCode.SUCCESS)
			{
				this.HandleError(connectionHandle, retCode);
			}
			((OdbcConnectionOpen)this.InnerConnection).EnlistedTransaction = transaction;
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x00241D30 File Offset: 0x00241130
		internal string Open_GetServerVersion()
		{
			return this.GetInfoStringUnhandled(ODBC32.SQL_INFO.DBMS_VER, true);
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x00241D48 File Offset: 0x00241148
		public OdbcConnection()
		{
			GC.SuppressFinalize(this);
			this._innerConnection = DbConnectionClosedNeverOpened.SingletonInstance;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x00241D84 File Offset: 0x00241184
		private void CopyFrom(OdbcConnection connection)
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

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x00241DD8 File Offset: 0x002411D8
		internal int CloseCount
		{
			get
			{
				return this._closeCount;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001A18 RID: 6680 RVA: 0x00241DEC File Offset: 0x002411EC
		internal DbConnectionFactory ConnectionFactory
		{
			get
			{
				return OdbcConnection._connectionFactory;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x00241E00 File Offset: 0x00241200
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

		// Token: 0x06001A1A RID: 6682 RVA: 0x00241E20 File Offset: 0x00241220
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

		// Token: 0x06001A1B RID: 6683 RVA: 0x00241E60 File Offset: 0x00241260
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

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x00241EF8 File Offset: 0x002412F8
		internal DbConnectionInternal InnerConnection
		{
			get
			{
				return this._innerConnection;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001A1D RID: 6685 RVA: 0x00241F0C File Offset: 0x0024130C
		// (set) Token: 0x06001A1E RID: 6686 RVA: 0x00241F20 File Offset: 0x00241320
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

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001A1F RID: 6687 RVA: 0x00241F34 File Offset: 0x00241334
		[ResDescription("DbConnection_State")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override ConnectionState State
		{
			get
			{
				return this.InnerConnection.State;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x00241F4C File Offset: 0x0024134C
		internal DbConnectionOptions UserConnectionOptions
		{
			get
			{
				return this._userConnectionOptions;
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x00241F60 File Offset: 0x00241360
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

		// Token: 0x06001A22 RID: 6690 RVA: 0x00241FCC File Offset: 0x002413CC
		internal void AddWeakReference(object value, int tag)
		{
			this.InnerConnection.AddWeakReference(value, tag);
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x00241FE8 File Offset: 0x002413E8
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

		// Token: 0x06001A24 RID: 6692 RVA: 0x00242040 File Offset: 0x00241440
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

		// Token: 0x06001A25 RID: 6693 RVA: 0x002420A4 File Offset: 0x002414A4
		private static CodeAccessPermission CreateExecutePermission()
		{
			DBDataPermission dbdataPermission = (DBDataPermission)OdbcConnectionFactory.SingletonInstance.ProviderFactory.CreatePermission(PermissionState.None);
			dbdataPermission.Add(string.Empty, string.Empty, KeyRestrictionBehavior.AllowOnly);
			return dbdataPermission;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x002420DC File Offset: 0x002414DC
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

		// Token: 0x06001A27 RID: 6695 RVA: 0x00242110 File Offset: 0x00241510
		private void EnlistDistributedTransactionHelper(ITransaction transaction)
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(OdbcConnection.ExecutePermission);
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

		// Token: 0x06001A28 RID: 6696 RVA: 0x00242178 File Offset: 0x00241578
		public override void EnlistTransaction(Transaction transaction)
		{
			OdbcConnection.ExecutePermission.Demand();
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

		// Token: 0x06001A29 RID: 6697 RVA: 0x002421D0 File Offset: 0x002415D0
		private DbMetaDataFactory GetMetaDataFactory(DbConnectionInternal internalConnection)
		{
			return this.ConnectionFactory.GetMetaDataFactory(this._poolGroup, internalConnection);
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x002421F0 File Offset: 0x002415F0
		internal DbMetaDataFactory GetMetaDataFactoryInternal(DbConnectionInternal internalConnection)
		{
			return this.GetMetaDataFactory(internalConnection);
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x00242204 File Offset: 0x00241604
		public override DataTable GetSchema()
		{
			return this.GetSchema(DbMetaDataCollectionNames.MetaDataCollections, null);
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00242220 File Offset: 0x00241620
		public override DataTable GetSchema(string collectionName)
		{
			return this.GetSchema(collectionName, null);
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00242238 File Offset: 0x00241638
		public override DataTable GetSchema(string collectionName, string[] restrictionValues)
		{
			OdbcConnection.ExecutePermission.Demand();
			return this.InnerConnection.GetSchema(this.ConnectionFactory, this.PoolGroup, this, collectionName, restrictionValues);
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0024226C File Offset: 0x0024166C
		internal void NotifyWeakReference(int message)
		{
			this.InnerConnection.NotifyWeakReference(message);
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x00242288 File Offset: 0x00241688
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

		// Token: 0x06001A30 RID: 6704 RVA: 0x002422C8 File Offset: 0x002416C8
		internal void RemoveWeakReference(object value)
		{
			this.InnerConnection.RemoveWeakReference(value);
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x002422E4 File Offset: 0x002416E4
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

		// Token: 0x06001A32 RID: 6706 RVA: 0x0024235C File Offset: 0x0024175C
		internal bool SetInnerConnectionFrom(DbConnectionInternal to, DbConnectionInternal from)
		{
			return from == Interlocked.CompareExchange<DbConnectionInternal>(ref this._innerConnection, to, from);
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x0024237C File Offset: 0x0024177C
		internal void SetInnerConnectionTo(DbConnectionInternal to)
		{
			this._innerConnection = to;
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x00242390 File Offset: 0x00241790
		[Conditional("DEBUG")]
		internal static void VerifyExecutePermission()
		{
			try
			{
				OdbcConnection.ExecutePermission.Demand();
			}
			catch (SecurityException)
			{
				throw;
			}
		}

		// Token: 0x04000F82 RID: 3970
		private int connectionTimeout = 15;

		// Token: 0x04000F83 RID: 3971
		private OdbcInfoMessageEventHandler infoMessageEventHandler;

		// Token: 0x04000F84 RID: 3972
		private WeakReference weakTransaction;

		// Token: 0x04000F85 RID: 3973
		private OdbcConnectionHandle _connectionHandle;

		// Token: 0x04000F86 RID: 3974
		private ConnectionState _extraState;

		// Token: 0x04000F87 RID: 3975
		private static readonly DbConnectionFactory _connectionFactory = OdbcConnectionFactory.SingletonInstance;

		// Token: 0x04000F88 RID: 3976
		internal static readonly CodeAccessPermission ExecutePermission = OdbcConnection.CreateExecutePermission();

		// Token: 0x04000F89 RID: 3977
		private DbConnectionOptions _userConnectionOptions;

		// Token: 0x04000F8A RID: 3978
		private DbConnectionPoolGroup _poolGroup;

		// Token: 0x04000F8B RID: 3979
		private DbConnectionInternal _innerConnection;

		// Token: 0x04000F8C RID: 3980
		private int _closeCount;

		// Token: 0x04000F8D RID: 3981
		private static int _objectTypeCount;

		// Token: 0x04000F8E RID: 3982
		internal readonly int ObjectID = Interlocked.Increment(ref OdbcConnection._objectTypeCount);
	}
}
