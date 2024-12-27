using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Transactions;

namespace System.Data.SqlClient
{
	// Token: 0x020002FB RID: 763
	internal sealed class SqlInternalConnectionTds : SqlInternalConnection, IDisposable
	{
		// Token: 0x060027B4 RID: 10164 RVA: 0x0028B284 File Offset: 0x0028A684
		internal SqlInternalConnectionTds(DbConnectionPoolIdentity identity, SqlConnectionString connectionOptions, object providerInfo, string newPassword, SqlConnection owningObject, bool redirectedUserInstance)
			: base(connectionOptions)
		{
			if (connectionOptions.UserInstance && InOutOfProcHelper.InProc)
			{
				throw SQL.UserInstanceNotAvailableInProc();
			}
			this._identity = identity;
			this._poolGroupProviderInfo = (SqlConnectionPoolGroupProviderInfo)providerInfo;
			this._fResetConnection = connectionOptions.ConnectionReset;
			if (this._fResetConnection)
			{
				this._originalDatabase = connectionOptions.InitialCatalog;
				this._originalLanguage = connectionOptions.CurrentLanguage;
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				this.OpenLoginEnlist(owningObject, connectionOptions, newPassword, redirectedUserInstance);
			}
			catch (OutOfMemoryException)
			{
				base.DoomThisConnection();
				throw;
			}
			catch (StackOverflowException)
			{
				base.DoomThisConnection();
				throw;
			}
			catch (ThreadAbortException)
			{
				base.DoomThisConnection();
				throw;
			}
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionTds.ctor|ADV> %d#, constructed new TDS internal connection\n", base.ObjectID);
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060027B5 RID: 10165 RVA: 0x0028B390 File Offset: 0x0028A790
		internal override SqlInternalTransaction CurrentTransaction
		{
			get
			{
				return this._parser.CurrentTransaction;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060027B6 RID: 10166 RVA: 0x0028B3A8 File Offset: 0x0028A7A8
		internal override SqlInternalTransaction AvailableInternalTransaction
		{
			get
			{
				if (!this._parser._fResetConnection)
				{
					return this.CurrentTransaction;
				}
				return null;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060027B7 RID: 10167 RVA: 0x0028B3CC File Offset: 0x0028A7CC
		internal override SqlInternalTransaction PendingTransaction
		{
			get
			{
				return this._parser.PendingTransaction;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x060027B8 RID: 10168 RVA: 0x0028B3E4 File Offset: 0x0028A7E4
		internal DbConnectionPoolIdentity Identity
		{
			get
			{
				return this._identity;
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060027B9 RID: 10169 RVA: 0x0028B3F8 File Offset: 0x0028A7F8
		internal string InstanceName
		{
			get
			{
				return this._instanceName;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060027BA RID: 10170 RVA: 0x0028B40C File Offset: 0x0028A80C
		internal override bool IsLockedForBulkCopy
		{
			get
			{
				return !this.Parser.MARSOn && this.Parser._physicalStateObj.BcpLock;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060027BB RID: 10171 RVA: 0x0028B438 File Offset: 0x0028A838
		protected internal override bool IsNonPoolableTransactionRoot
		{
			get
			{
				return this.IsTransactionRoot && (!this.IsKatmaiOrNewer || null == base.Pool);
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060027BC RID: 10172 RVA: 0x0028B464 File Offset: 0x0028A864
		internal override bool IsShiloh
		{
			get
			{
				return this._loginAck.isVersion8;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060027BD RID: 10173 RVA: 0x0028B47C File Offset: 0x0028A87C
		internal override bool IsYukonOrNewer
		{
			get
			{
				return this._parser.IsYukonOrNewer;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060027BE RID: 10174 RVA: 0x0028B494 File Offset: 0x0028A894
		internal override bool IsKatmaiOrNewer
		{
			get
			{
				return this._parser.IsKatmaiOrNewer;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x0028B4AC File Offset: 0x0028A8AC
		internal int PacketSize
		{
			get
			{
				return this._currentPacketSize;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x0028B4C0 File Offset: 0x0028A8C0
		internal TdsParser Parser
		{
			get
			{
				return this._parser;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x0028B4D4 File Offset: 0x0028A8D4
		internal string ServerProvidedFailOverPartner
		{
			get
			{
				return this._currentFailoverPartner;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060027C2 RID: 10178 RVA: 0x0028B4E8 File Offset: 0x0028A8E8
		internal SqlConnectionPoolGroupProviderInfo PoolGroupProviderInfo
		{
			get
			{
				return this._poolGroupProviderInfo;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x0028B4FC File Offset: 0x0028A8FC
		protected override bool ReadyToPrepareTransaction
		{
			get
			{
				return null == base.FindLiveReader(null);
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060027C4 RID: 10180 RVA: 0x0028B518 File Offset: 0x0028A918
		public override string ServerVersion
		{
			get
			{
				return string.Format(null, "{0:00}.{1:00}.{2:0000}", new object[]
				{
					this._loginAck.majorVersion,
					(short)this._loginAck.minorVersion,
					this._loginAck.buildNum
				});
			}
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x0028B574 File Offset: 0x0028A974
		protected override void ChangeDatabaseInternal(string database)
		{
			database = SqlConnection.FixupDatabaseTransactionName(database);
			this._parser.TdsExecuteSQLBatch("use " + database, base.ConnectionOptions.ConnectTimeout, null, this._parser._physicalStateObj);
			this._parser.Run(RunBehavior.UntilDone, null, null, null, this._parser._physicalStateObj);
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x0028B5D4 File Offset: 0x0028A9D4
		public override void Dispose()
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionTds.Dispose|ADV> %d# disposing\n", base.ObjectID);
			}
			try
			{
				TdsParser tdsParser = Interlocked.Exchange<TdsParser>(ref this._parser, null);
				if (tdsParser != null)
				{
					tdsParser.Disconnect();
				}
			}
			finally
			{
				this._loginAck = null;
				this._fConnectionOpen = false;
			}
			base.Dispose();
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x0028B644 File Offset: 0x0028AA44
		internal override void ValidateConnectionForExecute(SqlCommand command)
		{
			SqlDataReader sqlDataReader = null;
			if (this.Parser.MARSOn)
			{
				if (command != null)
				{
					sqlDataReader = base.FindLiveReader(command);
				}
			}
			else
			{
				sqlDataReader = base.FindLiveReader(null);
			}
			if (sqlDataReader != null)
			{
				throw ADP.OpenReaderExists();
			}
			if (!this.Parser.MARSOn && this.Parser._physicalStateObj._pendingData)
			{
				this.Parser._physicalStateObj.CleanWire();
			}
			this.Parser.RollbackOrphanedAPITransactions();
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x0028B6B8 File Offset: 0x0028AAB8
		protected override void Activate(Transaction transaction)
		{
			this.FailoverPermissionDemand();
			if (null != transaction)
			{
				if (base.ConnectionOptions.Enlist)
				{
					base.Enlist(transaction);
					return;
				}
			}
			else
			{
				base.Enlist(null);
			}
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x0028B6F0 File Offset: 0x0028AAF0
		protected override void InternalDeactivate()
		{
			if (this._asyncCommandCount != 0)
			{
				base.DoomThisConnection();
			}
			if (!this.IsNonPoolableTransactionRoot)
			{
				this._parser.Deactivate(base.IsConnectionDoomed);
				if (!base.IsConnectionDoomed)
				{
					this.ResetConnection();
				}
			}
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x0028B734 File Offset: 0x0028AB34
		private void ResetConnection()
		{
			if (this._fResetConnection)
			{
				if (this.IsShiloh)
				{
					this._parser.PrepareResetConnection(this.IsTransactionRoot && !this.IsNonPoolableTransactionRoot);
				}
				else if (!base.IsEnlistedInTransaction)
				{
					try
					{
						this._parser.TdsExecuteSQLBatch("sp_reset_connection", 30, null, this._parser._physicalStateObj);
						this._parser.Run(RunBehavior.UntilDone, null, null, null, this._parser._physicalStateObj);
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableExceptionType(ex))
						{
							throw;
						}
						base.DoomThisConnection();
						ADP.TraceExceptionWithoutRethrow(ex);
					}
				}
				base.CurrentDatabase = this._originalDatabase;
				this._currentLanguage = this._originalLanguage;
			}
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x0028B808 File Offset: 0x0028AC08
		internal void DecrementAsyncCount()
		{
			Interlocked.Decrement(ref this._asyncCommandCount);
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x0028B824 File Offset: 0x0028AC24
		internal void IncrementAsyncCount()
		{
			Interlocked.Increment(ref this._asyncCommandCount);
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x0028B840 File Offset: 0x0028AC40
		internal override void DisconnectTransaction(SqlInternalTransaction internalTransaction)
		{
			TdsParser parser = this.Parser;
			if (parser != null)
			{
				parser.DisconnectTransaction(internalTransaction);
			}
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x0028B860 File Offset: 0x0028AC60
		internal void ExecuteTransaction(SqlInternalConnection.TransactionRequest transactionRequest, string name, IsolationLevel iso)
		{
			this.ExecuteTransaction(transactionRequest, name, iso, null, false);
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x0028B878 File Offset: 0x0028AC78
		internal override void ExecuteTransaction(SqlInternalConnection.TransactionRequest transactionRequest, string name, IsolationLevel iso, SqlInternalTransaction internalTransaction, bool isDelegateControlRequest)
		{
			if (base.IsConnectionDoomed)
			{
				if (transactionRequest == SqlInternalConnection.TransactionRequest.Rollback || transactionRequest == SqlInternalConnection.TransactionRequest.IfRollback)
				{
					return;
				}
				throw SQL.ConnectionDoomed();
			}
			else
			{
				if ((transactionRequest == SqlInternalConnection.TransactionRequest.Commit || transactionRequest == SqlInternalConnection.TransactionRequest.Rollback || transactionRequest == SqlInternalConnection.TransactionRequest.IfRollback) && !this.Parser.MARSOn && this.Parser._physicalStateObj.BcpLock)
				{
					throw SQL.ConnectionLockedForBcpEvent();
				}
				string text = ((name == null) ? string.Empty : name);
				if (!this._parser.IsYukonOrNewer)
				{
					this.ExecuteTransactionPreYukon(transactionRequest, text, iso, internalTransaction);
					return;
				}
				this.ExecuteTransactionYukon(transactionRequest, text, iso, internalTransaction, isDelegateControlRequest);
				return;
			}
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x0028B900 File Offset: 0x0028AD00
		internal void ExecuteTransactionPreYukon(SqlInternalConnection.TransactionRequest transactionRequest, string transactionName, IsolationLevel iso, SqlInternalTransaction internalTransaction)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (iso <= IsolationLevel.ReadUncommitted)
			{
				if (iso == IsolationLevel.Unspecified)
				{
					goto IL_00DF;
				}
				if (iso == IsolationLevel.Chaos)
				{
					throw SQL.NotSupportedIsolationLevel(iso);
				}
				if (iso == IsolationLevel.ReadUncommitted)
				{
					stringBuilder.Append("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
					stringBuilder.Append(";");
					goto IL_00DF;
				}
			}
			else if (iso <= IsolationLevel.RepeatableRead)
			{
				if (iso == IsolationLevel.ReadCommitted)
				{
					stringBuilder.Append("SET TRANSACTION ISOLATION LEVEL READ COMMITTED");
					stringBuilder.Append(";");
					goto IL_00DF;
				}
				if (iso == IsolationLevel.RepeatableRead)
				{
					stringBuilder.Append("SET TRANSACTION ISOLATION LEVEL REPEATABLE READ");
					stringBuilder.Append(";");
					goto IL_00DF;
				}
			}
			else
			{
				if (iso == IsolationLevel.Serializable)
				{
					stringBuilder.Append("SET TRANSACTION ISOLATION LEVEL SERIALIZABLE");
					stringBuilder.Append(";");
					goto IL_00DF;
				}
				if (iso == IsolationLevel.Snapshot)
				{
					throw SQL.SnapshotNotSupported(IsolationLevel.Snapshot);
				}
			}
			throw ADP.InvalidIsolationLevel(iso);
			IL_00DF:
			if (!ADP.IsEmpty(transactionName))
			{
				transactionName = " " + SqlConnection.FixupDatabaseTransactionName(transactionName);
			}
			switch (transactionRequest)
			{
			case SqlInternalConnection.TransactionRequest.Begin:
				stringBuilder.Append("BEGIN TRANSACTION");
				stringBuilder.Append(transactionName);
				break;
			case SqlInternalConnection.TransactionRequest.Commit:
				stringBuilder.Append("COMMIT TRANSACTION");
				stringBuilder.Append(transactionName);
				break;
			case SqlInternalConnection.TransactionRequest.Rollback:
				stringBuilder.Append("ROLLBACK TRANSACTION");
				stringBuilder.Append(transactionName);
				break;
			case SqlInternalConnection.TransactionRequest.IfRollback:
				stringBuilder.Append("IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION");
				stringBuilder.Append(transactionName);
				break;
			case SqlInternalConnection.TransactionRequest.Save:
				stringBuilder.Append("SAVE TRANSACTION");
				stringBuilder.Append(transactionName);
				break;
			}
			this._parser.TdsExecuteSQLBatch(stringBuilder.ToString(), base.ConnectionOptions.ConnectTimeout, null, this._parser._physicalStateObj);
			this._parser.Run(RunBehavior.UntilDone, null, null, null, this._parser._physicalStateObj);
			if (transactionRequest == SqlInternalConnection.TransactionRequest.Begin)
			{
				this._parser.CurrentTransaction = internalTransaction;
			}
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x0028BAE8 File Offset: 0x0028AEE8
		internal void ExecuteTransactionYukon(SqlInternalConnection.TransactionRequest transactionRequest, string transactionName, IsolationLevel iso, SqlInternalTransaction internalTransaction, bool isDelegateControlRequest)
		{
			TdsEnums.TransactionManagerRequestType transactionManagerRequestType = TdsEnums.TransactionManagerRequestType.Begin;
			if (iso <= IsolationLevel.ReadUncommitted)
			{
				if (iso == IsolationLevel.Unspecified)
				{
					TdsEnums.TransactionManagerIsolationLevel transactionManagerIsolationLevel = TdsEnums.TransactionManagerIsolationLevel.Unspecified;
					goto IL_0073;
				}
				if (iso == IsolationLevel.Chaos)
				{
					throw SQL.NotSupportedIsolationLevel(iso);
				}
				if (iso == IsolationLevel.ReadUncommitted)
				{
					TdsEnums.TransactionManagerIsolationLevel transactionManagerIsolationLevel = TdsEnums.TransactionManagerIsolationLevel.ReadUncommitted;
					goto IL_0073;
				}
			}
			else if (iso <= IsolationLevel.RepeatableRead)
			{
				if (iso == IsolationLevel.ReadCommitted)
				{
					TdsEnums.TransactionManagerIsolationLevel transactionManagerIsolationLevel = TdsEnums.TransactionManagerIsolationLevel.ReadCommitted;
					goto IL_0073;
				}
				if (iso == IsolationLevel.RepeatableRead)
				{
					TdsEnums.TransactionManagerIsolationLevel transactionManagerIsolationLevel = TdsEnums.TransactionManagerIsolationLevel.RepeatableRead;
					goto IL_0073;
				}
			}
			else
			{
				if (iso == IsolationLevel.Serializable)
				{
					TdsEnums.TransactionManagerIsolationLevel transactionManagerIsolationLevel = TdsEnums.TransactionManagerIsolationLevel.Serializable;
					goto IL_0073;
				}
				if (iso == IsolationLevel.Snapshot)
				{
					TdsEnums.TransactionManagerIsolationLevel transactionManagerIsolationLevel = TdsEnums.TransactionManagerIsolationLevel.Snapshot;
					goto IL_0073;
				}
			}
			throw ADP.InvalidIsolationLevel(iso);
			IL_0073:
			TdsParserStateObject tdsParserStateObject = this._parser._physicalStateObj;
			TdsParser parser = this._parser;
			bool flag = false;
			bool flag2 = false;
			try
			{
				switch (transactionRequest)
				{
				case SqlInternalConnection.TransactionRequest.Begin:
					transactionManagerRequestType = TdsEnums.TransactionManagerRequestType.Begin;
					break;
				case SqlInternalConnection.TransactionRequest.Promote:
					transactionManagerRequestType = TdsEnums.TransactionManagerRequestType.Promote;
					break;
				case SqlInternalConnection.TransactionRequest.Commit:
					transactionManagerRequestType = TdsEnums.TransactionManagerRequestType.Commit;
					break;
				case SqlInternalConnection.TransactionRequest.Rollback:
				case SqlInternalConnection.TransactionRequest.IfRollback:
					transactionManagerRequestType = TdsEnums.TransactionManagerRequestType.Rollback;
					break;
				case SqlInternalConnection.TransactionRequest.Save:
					transactionManagerRequestType = TdsEnums.TransactionManagerRequestType.Save;
					break;
				}
				if (internalTransaction != null && internalTransaction.IsDelegated)
				{
					if (this._parser.MARSOn)
					{
						tdsParserStateObject = this._parser.GetSession(this);
						flag = true;
					}
					else
					{
						if (internalTransaction.OpenResultsCount != 0)
						{
							throw SQL.CannotCompleteDelegatedTransactionWithOpenResults();
						}
						Monitor.Enter(tdsParserStateObject);
						flag2 = true;
						if (internalTransaction.OpenResultsCount != 0)
						{
							throw SQL.CannotCompleteDelegatedTransactionWithOpenResults();
						}
					}
				}
				TdsEnums.TransactionManagerIsolationLevel transactionManagerIsolationLevel;
				this._parser.TdsExecuteTransactionManagerRequest(null, transactionManagerRequestType, transactionName, transactionManagerIsolationLevel, base.ConnectionOptions.ConnectTimeout, internalTransaction, tdsParserStateObject, isDelegateControlRequest);
			}
			finally
			{
				if (flag)
				{
					parser.PutSession(tdsParserStateObject);
				}
				if (flag2)
				{
					Monitor.Exit(tdsParserStateObject);
				}
			}
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x0028BC64 File Offset: 0x0028B064
		internal override void DelegatedTransactionEnded()
		{
			base.DelegatedTransactionEnded();
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x0028BC78 File Offset: 0x0028B078
		protected override byte[] GetDTCAddress()
		{
			return this._parser.GetDTCAddress(base.ConnectionOptions.ConnectTimeout, this._parser._physicalStateObj);
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x0028BCA8 File Offset: 0x0028B0A8
		protected override void PropagateTransactionCookie(byte[] cookie)
		{
			this._parser.PropagateDistributedTransaction(cookie, base.ConnectionOptions.ConnectTimeout, this._parser._physicalStateObj);
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x0028BCD8 File Offset: 0x0028B0D8
		private void CompleteLogin(bool enlistOK)
		{
			this._parser.Run(RunBehavior.UntilDone, null, null, null, this._parser._physicalStateObj);
			this._parser._physicalStateObj.SniContext = SniContext.Snix_EnableMars;
			this._parser.EnableMars(base.ConnectionOptions.DataSource);
			this._fConnectionOpen = true;
			if (enlistOK && base.ConnectionOptions.Enlist)
			{
				this._parser._physicalStateObj.SniContext = SniContext.Snix_AutoEnlist;
				Transaction currentTransaction = ADP.GetCurrentTransaction();
				base.Enlist(currentTransaction);
			}
			this._parser._physicalStateObj.SniContext = SniContext.Snix_Login;
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x0028BD70 File Offset: 0x0028B170
		private void Login(ServerInfo server, long timerExpire, string newPassword)
		{
			SqlLogin sqlLogin = new SqlLogin();
			base.CurrentDatabase = base.ConnectionOptions.InitialCatalog;
			this._currentPacketSize = base.ConnectionOptions.PacketSize;
			this._currentLanguage = base.ConnectionOptions.CurrentLanguage;
			int num = 0;
			if (9223372036854775807L != timerExpire)
			{
				long num2 = ADP.TimerRemainingSeconds(timerExpire);
				if (2147483647L > num2)
				{
					num = (int)num2;
				}
			}
			sqlLogin.timeout = num;
			sqlLogin.userInstance = base.ConnectionOptions.UserInstance;
			sqlLogin.hostName = base.ConnectionOptions.ObtainWorkstationId();
			sqlLogin.userName = base.ConnectionOptions.UserID;
			sqlLogin.password = base.ConnectionOptions.Password;
			sqlLogin.applicationName = base.ConnectionOptions.ApplicationName;
			sqlLogin.language = this._currentLanguage;
			if (!sqlLogin.userInstance)
			{
				sqlLogin.database = base.CurrentDatabase;
				sqlLogin.attachDBFilename = base.ConnectionOptions.AttachDBFilename;
			}
			sqlLogin.serverName = server.UserServerName;
			sqlLogin.useReplication = base.ConnectionOptions.Replication;
			sqlLogin.useSSPI = base.ConnectionOptions.IntegratedSecurity;
			sqlLogin.packetSize = this._currentPacketSize;
			sqlLogin.newPassword = newPassword;
			sqlLogin.readOnlyIntent = base.ConnectionOptions.ApplicationIntent == ApplicationIntent.ReadOnly;
			this._parser.TdsLogin(sqlLogin);
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x0028BEC8 File Offset: 0x0028B2C8
		private void LoginFailure()
		{
			Bid.Trace("<sc.SqlInternalConnectionTds.LoginFailure|RES|CPOOL> %d#\n", base.ObjectID);
			if (this._parser != null)
			{
				this._parser.Disconnect();
			}
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x0028BEF8 File Offset: 0x0028B2F8
		private void OpenLoginEnlist(SqlConnection owningObject, SqlConnectionString connectionOptions, string newPassword, bool redirectedUserInstance)
		{
			long num = ADP.TimerCurrent();
			string dataSource = base.ConnectionOptions.DataSource;
			bool flag;
			string text;
			if (this.PoolGroupProviderInfo != null)
			{
				flag = this.PoolGroupProviderInfo.UseFailoverPartner;
				text = this.PoolGroupProviderInfo.FailoverPartner;
			}
			else
			{
				flag = false;
				text = base.ConnectionOptions.FailoverPartner;
			}
			bool flag2 = !ADP.IsEmpty(text);
			try
			{
				if (flag2)
				{
					this.LoginWithFailover(flag, dataSource, text, newPassword, redirectedUserInstance, owningObject, connectionOptions, num);
				}
				else
				{
					this.LoginNoFailover(dataSource, newPassword, redirectedUserInstance, owningObject, connectionOptions, num);
				}
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableExceptionType(ex))
				{
					this.LoginFailure();
				}
				throw;
			}
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x0028BFA8 File Offset: 0x0028B3A8
		private bool IsDoNotRetryConnectError(SqlException exc)
		{
			return 18456 == exc.Number || 18488 == exc.Number || exc._doNotReconnect;
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x0028BFD8 File Offset: 0x0028B3D8
		private void LoginNoFailover(string host, string newPassword, bool redirectedUserInstance, SqlConnection owningObject, SqlConnectionString connectionOptions, long timerStart)
		{
			int num = 0;
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionTds.LoginNoFailover|ADV> %d#, host=%s\n", base.ObjectID, host);
			}
			int connectTimeout = base.ConnectionOptions.ConnectTimeout;
			int num2 = 100;
			ServerInfo serverInfo = new ServerInfo(base.ConnectionOptions.NetworkLibrary, host);
			ServerInfo serverInfo2 = serverInfo;
			this.ResolveExtendedServerName(serverInfo, !redirectedUserInstance, owningObject);
			long num3 = 0L;
			long num4;
			int num5;
			long num6;
			checked
			{
				if (connectionOptions.MultiSubnetFailover)
				{
					if (connectTimeout == 0)
					{
						num3 = 1199L;
					}
					else
					{
						num3 = (long)(unchecked(0.08f * (float)ADP.TimerRemainingMilliseconds((long)connectTimeout)));
					}
				}
				if (connectTimeout == 0)
				{
					num4 = long.MaxValue;
				}
				else
				{
					num4 = timerStart + ADP.TimerFromSeconds(connectTimeout);
				}
				num5 = 0;
				num6 = 0L;
			}
			for (;;)
			{
				if (connectionOptions.MultiSubnetFailover)
				{
					num5++;
					checked
					{
						long num7 = num3 * unchecked((long)num5);
						long num8 = ADP.TimerRemainingMilliseconds(num4);
						if (0L > num8)
						{
							num8 = 0L;
						}
						if (num7 > num8)
						{
						}
						num6 = timerStart + num8 * 10000L;
					}
				}
				if (this._parser != null)
				{
					this._parser.Disconnect();
				}
				this._parser = new TdsParser(base.ConnectionOptions.MARS, base.ConnectionOptions.Asynchronous);
				try
				{
					this.AttemptOneLogin(serverInfo, newPassword, !connectionOptions.MultiSubnetFailover, connectionOptions.MultiSubnetFailover ? num6 : num4, owningObject, false);
					if (connectionOptions.MultiSubnetFailover && this.ServerProvidedFailOverPartner != null)
					{
						throw SQL.MultiSubnetFailoverWithFailoverPartner(true);
					}
					if (this._routingInfo == null)
					{
						goto IL_029F;
					}
					Bid.Trace("<sc.SqlInternalConnectionTds.LoginNoFailover> Routed to %ls", serverInfo.ExtendedServerName);
					if (num > 0)
					{
						throw SQL.ROR_RecursiveRoutingNotSupported();
					}
					if (ADP.TimerHasExpired(num4))
					{
						throw SQL.ROR_TimeoutAfterRoutingInfo();
					}
					serverInfo = new ServerInfo(base.ConnectionOptions, this._routingInfo, serverInfo.ResolvedServerName);
					this._currentPacketSize = base.ConnectionOptions.PacketSize;
					this._currentLanguage = (this._originalLanguage = base.ConnectionOptions.CurrentLanguage);
					base.CurrentDatabase = (this._originalDatabase = base.ConnectionOptions.InitialCatalog);
					this._currentFailoverPartner = null;
					this._instanceName = string.Empty;
					num++;
					continue;
				}
				catch (SqlException ex)
				{
					if (this._parser == null || this._parser.State != TdsParserState.Closed || this.IsDoNotRetryConnectError(ex) || ADP.TimerHasExpired(num4))
					{
						throw;
					}
					long num9 = ADP.TimerRemainingMilliseconds(num4);
					if (num9 <= (long)num2)
					{
						throw;
					}
				}
				if (this.ServerProvidedFailOverPartner != null)
				{
					break;
				}
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlInternalConnectionTds.LoginNoFailover|ADV> %d#, sleeping %d{milisec}\n", base.ObjectID, num2);
				}
				Thread.Sleep(num2);
				num2 = ((num2 < 500) ? (num2 * 2) : 1000);
			}
			if (connectionOptions.MultiSubnetFailover)
			{
				throw SQL.MultiSubnetFailoverWithFailoverPartner(true);
			}
			this.LoginWithFailover(true, host, this.ServerProvidedFailOverPartner, newPassword, redirectedUserInstance, owningObject, connectionOptions, timerStart);
			return;
			IL_029F:
			if (this.PoolGroupProviderInfo != null)
			{
				this.PoolGroupProviderInfo.FailoverCheck(this, false, connectionOptions, this.ServerProvidedFailOverPartner);
			}
			base.CurrentDataSource = serverInfo2.UserServerName;
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x0028C2CC File Offset: 0x0028B6CC
		private void LoginWithFailover(bool useFailoverHost, string primaryHost, string failoverHost, string newPassword, bool redirectedUserInstance, SqlConnection owningObject, SqlConnectionString connectionOptions, long timerStart)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionTds.LoginWithFailover|ADV> %d#, useFailover=%d{bool}, primary=", base.ObjectID, useFailoverHost);
				Bid.PutStr(primaryHost);
				Bid.PutStr(", failover=");
				Bid.PutStr(failoverHost);
				Bid.PutStr("\n");
			}
			int connectTimeout = base.ConnectionOptions.ConnectTimeout;
			int num = 100;
			string networkLibrary = base.ConnectionOptions.NetworkLibrary;
			ServerInfo serverInfo = new ServerInfo(networkLibrary, primaryHost);
			ServerInfo serverInfo2 = new ServerInfo(networkLibrary, failoverHost);
			this.ResolveExtendedServerName(serverInfo, !redirectedUserInstance, owningObject);
			if (this.ServerProvidedFailOverPartner == null)
			{
				this.ResolveExtendedServerName(serverInfo2, !redirectedUserInstance && failoverHost != primaryHost, owningObject);
			}
			checked
			{
				long num2;
				long num4;
				if (connectTimeout == 0)
				{
					num2 = long.MaxValue;
					long num3 = 0L;
					ADP.TimerFromSeconds(15);
					num4 = num3;
				}
				else
				{
					long num5 = ADP.TimerFromSeconds(connectTimeout);
					num2 = timerStart + num5;
					num4 = (long)(unchecked(0.08f * (float)num5));
				}
				bool flag = false;
				long num6 = timerStart + num4;
				int num7 = 0;
				for (;;)
				{
					if (this._parser != null)
					{
						this._parser.Disconnect();
					}
					this._parser = new TdsParser(base.ConnectionOptions.MARS, base.ConnectionOptions.Asynchronous);
					ServerInfo serverInfo3;
					if (useFailoverHost)
					{
						if (!flag)
						{
							this.FailoverPermissionDemand();
							flag = true;
						}
						if (this.ServerProvidedFailOverPartner != null && serverInfo2.ResolvedServerName != this.ServerProvidedFailOverPartner)
						{
							if (Bid.AdvancedOn)
							{
								Bid.Trace("<sc.SqlInternalConnectionTds.LoginWithFailover|ADV> %d#, new failover partner=%s\n", base.ObjectID, this.ServerProvidedFailOverPartner);
							}
							serverInfo2.SetDerivedNames(networkLibrary, this.ServerProvidedFailOverPartner);
						}
						serverInfo3 = serverInfo2;
					}
					else
					{
						serverInfo3 = serverInfo;
					}
					unchecked
					{
						try
						{
							this.AttemptOneLogin(serverInfo3, newPassword, false, (connectTimeout == 0) ? num2 : num6, owningObject, true);
							if (this._routingInfo != null)
							{
								Bid.Trace("<sc.SqlInternalConnectionTds.LoginWithFailover> Routed to %ls", this._routingInfo.ServerName);
								throw SQL.ROR_UnexpectedRoutingInfo();
							}
							break;
						}
						catch (SqlException ex)
						{
							if (this.IsDoNotRetryConnectError(ex) || ADP.TimerHasExpired(num2))
							{
								throw;
							}
							if (base.IsConnectionDoomed)
							{
								throw;
							}
							if (1 == num7 % 2)
							{
								long num8 = ADP.TimerRemainingMilliseconds(num2);
								if (num8 <= (long)num)
								{
									throw;
								}
							}
						}
						if (1 == num7 % 2)
						{
							if (Bid.AdvancedOn)
							{
								Bid.Trace("<sc.SqlInternalConnectionTds.LoginWithFailover|ADV> %d#, sleeping %d{milisec}\n", base.ObjectID, num);
							}
							Thread.Sleep(num);
							num = ((num < 500) ? (num * 2) : 1000);
						}
						num7++;
					}
					num6 = ADP.TimerCurrent() + num4 * unchecked((long)(checked(num7 / 2 + 1)));
					if (num6 > num2)
					{
						num6 = num2;
					}
					useFailoverHost = !useFailoverHost;
				}
				if (useFailoverHost && this.ServerProvidedFailOverPartner == null)
				{
					throw SQL.InvalidPartnerConfiguration(failoverHost, base.CurrentDatabase);
				}
				if (this.PoolGroupProviderInfo != null)
				{
					this.PoolGroupProviderInfo.FailoverCheck(this, useFailoverHost, connectionOptions, this.ServerProvidedFailOverPartner);
				}
				base.CurrentDataSource = (useFailoverHost ? failoverHost : primaryHost);
			}
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x0028C57C File Offset: 0x0028B97C
		private void ResolveExtendedServerName(ServerInfo serverInfo, bool aliasLookup, SqlConnection owningObject)
		{
			if (serverInfo.ExtendedServerName == null)
			{
				string userServerName = serverInfo.UserServerName;
				string userProtocol = serverInfo.UserProtocol;
				if (aliasLookup)
				{
					TdsParserStaticMethods.AliasRegistryLookup(ref userServerName, ref userProtocol);
					if (owningObject != null && ((SqlConnectionString)owningObject.UserConnectionOptions).EnforceLocalHost)
					{
						SqlConnectionString.VerifyLocalHostAndFixup(ref userServerName, true, true);
					}
				}
				serverInfo.SetDerivedNames(userProtocol, userServerName);
			}
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x0028C5D4 File Offset: 0x0028B9D4
		private void AttemptOneLogin(ServerInfo serverInfo, string newPassword, bool ignoreSniOpenTimeout, long timerExpire, SqlConnection owningObject, bool withFailover)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlInternalConnectionTds.AttemptOneLogin|ADV> %d#, timout=%d{ticks}, server=", base.ObjectID, timerExpire - ADP.TimerCurrent());
				Bid.PutStr(serverInfo.ExtendedServerName);
				Bid.Trace("\n");
			}
			this._routingInfo = null;
			this._parser._physicalStateObj.SniContext = SniContext.Snix_Connect;
			this._parser.Connect(serverInfo, this, ignoreSniOpenTimeout, timerExpire, base.ConnectionOptions.Encrypt, base.ConnectionOptions.TrustServerCertificate, base.ConnectionOptions.IntegratedSecurity, owningObject, withFailover);
			this._parser._physicalStateObj.SniContext = SniContext.Snix_Login;
			this.Login(serverInfo, timerExpire, newPassword);
			this.CompleteLogin(!base.ConnectionOptions.Pooling);
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x0028C694 File Offset: 0x0028BA94
		internal void FailoverPermissionDemand()
		{
			if (this.PoolGroupProviderInfo != null)
			{
				this.PoolGroupProviderInfo.FailoverPermissionDemand();
			}
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x0028C6B4 File Offset: 0x0028BAB4
		internal override void AddPreparedCommand(SqlCommand cmd)
		{
			if (this._preparedCommands == null)
			{
				this._preparedCommands = new List<WeakReference>(5);
			}
			for (int i = 0; i < this._preparedCommands.Count; i++)
			{
				if (!this._preparedCommands[i].IsAlive)
				{
					this._preparedCommands[i].Target = cmd;
					return;
				}
			}
			this._preparedCommands.Add(new WeakReference(cmd));
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x0028C724 File Offset: 0x0028BB24
		internal override void ClearPreparedCommands()
		{
			if (this._preparedCommands != null)
			{
				for (int i = 0; i < this._preparedCommands.Count; i++)
				{
					SqlCommand sqlCommand = this._preparedCommands[i].Target as SqlCommand;
					if (sqlCommand != null)
					{
						sqlCommand.Unprepare(true);
						this._preparedCommands[i].Target = null;
					}
				}
				this._preparedCommands = null;
			}
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x0028C78C File Offset: 0x0028BB8C
		internal override void RemovePreparedCommand(SqlCommand cmd)
		{
			if (this._preparedCommands == null || this._preparedCommands.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this._preparedCommands.Count; i++)
			{
				if (this._preparedCommands[i].Target == cmd)
				{
					this._preparedCommands[i].Target = null;
					return;
				}
			}
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x0028C7EC File Offset: 0x0028BBEC
		internal void BreakConnection()
		{
			Bid.Trace("<sc.SqlInternalConnectionTds.BreakConnection|RES|CPOOL> %d#, Breaking connection.\n", base.ObjectID);
			base.DoomThisConnection();
			if (base.Connection != null)
			{
				base.Connection.Close();
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060027E3 RID: 10211 RVA: 0x0028C824 File Offset: 0x0028BC24
		internal bool IgnoreEnvChange
		{
			get
			{
				return this._routingInfo != null;
			}
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x0028C840 File Offset: 0x0028BC40
		internal void OnEnvChange(SqlEnvChange rec)
		{
			switch (rec.type)
			{
			case 1:
				if (!this._fConnectionOpen)
				{
					this._originalDatabase = rec.newValue;
				}
				base.CurrentDatabase = rec.newValue;
				return;
			case 2:
				if (!this._fConnectionOpen)
				{
					this._originalLanguage = rec.newValue;
				}
				this._currentLanguage = rec.newValue;
				return;
			case 3:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 14:
			case 16:
			case 17:
			case 18:
				break;
			case 4:
				this._currentPacketSize = int.Parse(rec.newValue, CultureInfo.InvariantCulture);
				return;
			case 13:
				if (base.ConnectionOptions.ApplicationIntent == ApplicationIntent.ReadOnly)
				{
					throw SQL.ROR_FailoverNotSupportedServer();
				}
				this._currentFailoverPartner = rec.newValue;
				return;
			case 15:
				base.PromotedDTCToken = rec.newBinValue;
				return;
			case 19:
				this._instanceName = rec.newValue;
				return;
			case 20:
				if (string.IsNullOrEmpty(rec.newRoutingInfo.ServerName) || rec.newRoutingInfo.Protocol != 0 || rec.newRoutingInfo.Port == 0)
				{
					throw SQL.ROR_InvalidRoutingInfo();
				}
				this._routingInfo = rec.newRoutingInfo;
				break;
			default:
				return;
			}
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x0028C980 File Offset: 0x0028BD80
		internal void OnLoginAck(SqlLoginAck rec)
		{
			this._loginAck = rec;
		}

		// Token: 0x040018FB RID: 6395
		private readonly SqlConnectionPoolGroupProviderInfo _poolGroupProviderInfo;

		// Token: 0x040018FC RID: 6396
		private TdsParser _parser;

		// Token: 0x040018FD RID: 6397
		private SqlLoginAck _loginAck;

		// Token: 0x040018FE RID: 6398
		private bool _fConnectionOpen;

		// Token: 0x040018FF RID: 6399
		private bool _fResetConnection;

		// Token: 0x04001900 RID: 6400
		private string _originalDatabase;

		// Token: 0x04001901 RID: 6401
		private string _currentFailoverPartner;

		// Token: 0x04001902 RID: 6402
		private string _originalLanguage;

		// Token: 0x04001903 RID: 6403
		private string _currentLanguage;

		// Token: 0x04001904 RID: 6404
		private int _currentPacketSize;

		// Token: 0x04001905 RID: 6405
		private int _asyncCommandCount;

		// Token: 0x04001906 RID: 6406
		private string _instanceName = string.Empty;

		// Token: 0x04001907 RID: 6407
		private DbConnectionPoolIdentity _identity;

		// Token: 0x04001908 RID: 6408
		private List<WeakReference> _preparedCommands;

		// Token: 0x04001909 RID: 6409
		private RoutingInfo _routingInfo;
	}
}
