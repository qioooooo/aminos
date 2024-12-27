using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Xml;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x020002C4 RID: 708
	[DefaultEvent("RecordsAffected")]
	[ToolboxItem(true)]
	[Designer("Microsoft.VSDesigner.Data.VS.SqlCommandDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class SqlCommand : DbCommand, ICloneable
	{
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060023BF RID: 9151 RVA: 0x00272858 File Offset: 0x00271C58
		private SqlCommand.CachedAsyncState cachedAsyncState
		{
			get
			{
				if (this._cachedAsyncState == null)
				{
					this._cachedAsyncState = new SqlCommand.CachedAsyncState();
				}
				return this._cachedAsyncState;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060023C0 RID: 9152 RVA: 0x00272880 File Offset: 0x00271C80
		private SqlCommand.CommandEventSink EventSink
		{
			get
			{
				if (this._smiEventSink == null)
				{
					this._smiEventSink = new SqlCommand.CommandEventSink(this);
				}
				this._smiEventSink.Parent = this.InternalSmiConnection.CurrentEventSink;
				return this._smiEventSink;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060023C1 RID: 9153 RVA: 0x002728C0 File Offset: 0x00271CC0
		private SmiEventSink_DeferedProcessing OutParamEventSink
		{
			get
			{
				if (this._outParamEventSink == null)
				{
					this._outParamEventSink = new SmiEventSink_DeferedProcessing(this.EventSink);
				}
				else
				{
					this._outParamEventSink.Parent = this.EventSink;
				}
				return this._outParamEventSink;
			}
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x00272900 File Offset: 0x00271D00
		public SqlCommand()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x00272950 File Offset: 0x00271D50
		public SqlCommand(string cmdText)
			: this()
		{
			this.CommandText = cmdText;
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x0027296C File Offset: 0x00271D6C
		public SqlCommand(string cmdText, SqlConnection connection)
			: this()
		{
			this.CommandText = cmdText;
			this.Connection = connection;
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x00272990 File Offset: 0x00271D90
		public SqlCommand(string cmdText, SqlConnection connection, SqlTransaction transaction)
			: this()
		{
			this.CommandText = cmdText;
			this.Connection = connection;
			this.Transaction = transaction;
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x002729B8 File Offset: 0x00271DB8
		private SqlCommand(SqlCommand from)
			: this()
		{
			this.CommandText = from.CommandText;
			this.CommandTimeout = from.CommandTimeout;
			this.CommandType = from.CommandType;
			this.Connection = from.Connection;
			this.DesignTimeVisible = from.DesignTimeVisible;
			this.Transaction = from.Transaction;
			this.UpdatedRowSource = from.UpdatedRowSource;
			SqlParameterCollection parameters = this.Parameters;
			foreach (object obj in from.Parameters)
			{
				parameters.Add((obj is ICloneable) ? (obj as ICloneable).Clone() : obj);
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060023C7 RID: 9159 RVA: 0x00272A90 File Offset: 0x00271E90
		// (set) Token: 0x060023C8 RID: 9160 RVA: 0x00272AA4 File Offset: 0x00271EA4
		[DefaultValue(null)]
		[ResDescription("DbCommand_Connection")]
		[Editor("Microsoft.VSDesigner.Data.Design.DbConnectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResCategory("DataCategory_Data")]
		public new SqlConnection Connection
		{
			get
			{
				return this._activeConnection;
			}
			set
			{
				if (this._activeConnection != value && this._activeConnection != null && this.cachedAsyncState.PendingAsyncOperation)
				{
					throw SQL.CannotModifyPropertyAsyncOperationInProgress("Connection");
				}
				if (this._transaction != null && this._transaction.Connection == null)
				{
					this._transaction = null;
				}
				this._activeConnection = value;
				Bid.Trace("<sc.SqlCommand.set_Connection|API> %d#, %d#\n", this.ObjectID, (value != null) ? value.ObjectID : (-1));
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060023C9 RID: 9161 RVA: 0x00272B1C File Offset: 0x00271F1C
		// (set) Token: 0x060023CA RID: 9162 RVA: 0x00272B30 File Offset: 0x00271F30
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
			set
			{
				this.Connection = (SqlConnection)value;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060023CB RID: 9163 RVA: 0x00272B4C File Offset: 0x00271F4C
		private SqlInternalConnectionSmi InternalSmiConnection
		{
			get
			{
				return (SqlInternalConnectionSmi)this._activeConnection.InnerConnection;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x00272B6C File Offset: 0x00271F6C
		private SqlInternalConnectionTds InternalTdsConnection
		{
			get
			{
				return (SqlInternalConnectionTds)this._activeConnection.InnerConnection;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060023CD RID: 9165 RVA: 0x00272B8C File Offset: 0x00271F8C
		private bool IsShiloh
		{
			get
			{
				return this._activeConnection != null && this._activeConnection.IsShiloh;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060023CE RID: 9166 RVA: 0x00272BB0 File Offset: 0x00271FB0
		// (set) Token: 0x060023CF RID: 9167 RVA: 0x00272BC4 File Offset: 0x00271FC4
		[DefaultValue(true)]
		[ResCategory("DataCategory_Notification")]
		[ResDescription("SqlCommand_NotificationAutoEnlist")]
		public bool NotificationAutoEnlist
		{
			get
			{
				return this._notificationAutoEnlist;
			}
			set
			{
				this._notificationAutoEnlist = value;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x00272BD8 File Offset: 0x00271FD8
		// (set) Token: 0x060023D1 RID: 9169 RVA: 0x00272BEC File Offset: 0x00271FEC
		[ResDescription("SqlCommand_Notification")]
		[Browsable(false)]
		[ResCategory("DataCategory_Notification")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SqlNotificationRequest Notification
		{
			get
			{
				return this._notification;
			}
			set
			{
				Bid.Trace("<sc.SqlCommand.set_Notification|API> %d#\n", this.ObjectID);
				this._sqlDep = null;
				this._notification = value;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x00272C18 File Offset: 0x00272018
		internal SqlStatistics Statistics
		{
			get
			{
				if (this._activeConnection != null && this._activeConnection.StatisticsEnabled)
				{
					return this._activeConnection.Statistics;
				}
				return null;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x00272C48 File Offset: 0x00272048
		// (set) Token: 0x060023D4 RID: 9172 RVA: 0x00272C78 File Offset: 0x00272078
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[ResDescription("DbCommand_Transaction")]
		public new SqlTransaction Transaction
		{
			get
			{
				if (this._transaction != null && this._transaction.Connection == null)
				{
					this._transaction = null;
				}
				return this._transaction;
			}
			set
			{
				if (this._transaction != value && this._activeConnection != null && this.cachedAsyncState.PendingAsyncOperation)
				{
					throw SQL.CannotModifyPropertyAsyncOperationInProgress("Transaction");
				}
				Bid.Trace("<sc.SqlCommand.set_Transaction|API> %d#\n", this.ObjectID);
				this._transaction = value;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060023D5 RID: 9173 RVA: 0x00272CC8 File Offset: 0x002720C8
		// (set) Token: 0x060023D6 RID: 9174 RVA: 0x00272CDC File Offset: 0x002720DC
		protected override DbTransaction DbTransaction
		{
			get
			{
				return this.Transaction;
			}
			set
			{
				this.Transaction = (SqlTransaction)value;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x060023D7 RID: 9175 RVA: 0x00272CF8 File Offset: 0x002720F8
		// (set) Token: 0x060023D8 RID: 9176 RVA: 0x00272D18 File Offset: 0x00272118
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbCommand_CommandText")]
		[DefaultValue("")]
		[Editor("Microsoft.VSDesigner.Data.SQL.Design.SqlCommandTextEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResCategory("DataCategory_Data")]
		public override string CommandText
		{
			get
			{
				string commandText = this._commandText;
				if (commandText == null)
				{
					return ADP.StrEmpty;
				}
				return commandText;
			}
			set
			{
				if (Bid.TraceOn)
				{
					Bid.Trace("<sc.SqlCommand.set_CommandText|API> %d#, '", this.ObjectID);
					Bid.PutStr(value);
					Bid.Trace("'\n");
				}
				if (ADP.SrcCompare(this._commandText, value) != 0)
				{
					this.PropertyChanging();
					this._commandText = value;
				}
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060023D9 RID: 9177 RVA: 0x00272D68 File Offset: 0x00272168
		// (set) Token: 0x060023DA RID: 9178 RVA: 0x00272D7C File Offset: 0x0027217C
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbCommand_CommandTimeout")]
		public override int CommandTimeout
		{
			get
			{
				return this._commandTimeout;
			}
			set
			{
				Bid.Trace("<sc.SqlCommand.set_CommandTimeout|API> %d#, %d\n", this.ObjectID, value);
				if (value < 0)
				{
					throw ADP.InvalidCommandTimeout(value);
				}
				if (value != this._commandTimeout)
				{
					this.PropertyChanging();
					this._commandTimeout = value;
				}
			}
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x00272DBC File Offset: 0x002721BC
		public void ResetCommandTimeout()
		{
			if (30 != this._commandTimeout)
			{
				this.PropertyChanging();
				this._commandTimeout = 30;
			}
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x00272DE4 File Offset: 0x002721E4
		private bool ShouldSerializeCommandTimeout()
		{
			return 30 != this._commandTimeout;
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060023DD RID: 9181 RVA: 0x00272E00 File Offset: 0x00272200
		// (set) Token: 0x060023DE RID: 9182 RVA: 0x00272E1C File Offset: 0x0027221C
		[DefaultValue(CommandType.Text)]
		[ResDescription("DbCommand_CommandType")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Data")]
		public override CommandType CommandType
		{
			get
			{
				CommandType commandType = this._commandType;
				if (commandType == (CommandType)0)
				{
					return CommandType.Text;
				}
				return commandType;
			}
			set
			{
				Bid.Trace("<sc.SqlCommand.set_CommandType|API> %d#, %d{ds.CommandType}\n", this.ObjectID, (int)value);
				if (this._commandType == value)
				{
					return;
				}
				if (value == CommandType.Text || value == CommandType.StoredProcedure)
				{
					this.PropertyChanging();
					this._commandType = value;
					return;
				}
				if (value != CommandType.TableDirect)
				{
					throw ADP.InvalidCommandType(value);
				}
				throw SQL.NotSupportedCommandType(value);
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060023DF RID: 9183 RVA: 0x00272E74 File Offset: 0x00272274
		// (set) Token: 0x060023E0 RID: 9184 RVA: 0x00272E8C File Offset: 0x0027228C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignOnly(true)]
		[DefaultValue(true)]
		[Browsable(false)]
		public override bool DesignTimeVisible
		{
			get
			{
				return !this._designTimeInvisible;
			}
			set
			{
				this._designTimeInvisible = !value;
				TypeDescriptor.Refresh(this);
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060023E1 RID: 9185 RVA: 0x00272EAC File Offset: 0x002722AC
		[ResDescription("DbCommand_Parameters")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[ResCategory("DataCategory_Data")]
		public new SqlParameterCollection Parameters
		{
			get
			{
				if (this._parameters == null)
				{
					this._parameters = new SqlParameterCollection();
				}
				return this._parameters;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x00272ED4 File Offset: 0x002722D4
		protected override DbParameterCollection DbParameterCollection
		{
			get
			{
				return this.Parameters;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060023E3 RID: 9187 RVA: 0x00272EE8 File Offset: 0x002722E8
		// (set) Token: 0x060023E4 RID: 9188 RVA: 0x00272EFC File Offset: 0x002722FC
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbCommand_UpdatedRowSource")]
		[DefaultValue(UpdateRowSource.Both)]
		public override UpdateRowSource UpdatedRowSource
		{
			get
			{
				return this._updatedRowSource;
			}
			set
			{
				switch (value)
				{
				case UpdateRowSource.None:
				case UpdateRowSource.OutputParameters:
				case UpdateRowSource.FirstReturnedRecord:
				case UpdateRowSource.Both:
					this._updatedRowSource = value;
					return;
				default:
					throw ADP.InvalidUpdateRowSource(value);
				}
			}
		}

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060023E5 RID: 9189 RVA: 0x00272F34 File Offset: 0x00272334
		// (remove) Token: 0x060023E6 RID: 9190 RVA: 0x00272F58 File Offset: 0x00272358
		[ResDescription("DbCommand_StatementCompleted")]
		[ResCategory("DataCategory_StatementCompleted")]
		public event StatementCompletedEventHandler StatementCompleted
		{
			add
			{
				this._statementCompletedEventHandler = (StatementCompletedEventHandler)Delegate.Combine(this._statementCompletedEventHandler, value);
			}
			remove
			{
				this._statementCompletedEventHandler = (StatementCompletedEventHandler)Delegate.Remove(this._statementCompletedEventHandler, value);
			}
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x00272F7C File Offset: 0x0027237C
		internal void OnStatementCompleted(int recordCount)
		{
			if (0 <= recordCount)
			{
				StatementCompletedEventHandler statementCompletedEventHandler = this._statementCompletedEventHandler;
				if (statementCompletedEventHandler != null)
				{
					try
					{
						Bid.Trace("<sc.SqlCommand.OnStatementCompleted|INFO> %d#, recordCount=%d\n", this.ObjectID, recordCount);
						statementCompletedEventHandler(this, new StatementCompletedEventArgs(recordCount));
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
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x00272FE8 File Offset: 0x002723E8
		private void PropertyChanging()
		{
			this.IsDirty = true;
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x00272FFC File Offset: 0x002723FC
		public override void Prepare()
		{
			SqlConnection.ExecutePermission.Demand();
			this._pendingCancel = false;
			if (this._activeConnection.IsContextConnection)
			{
				return;
			}
			SqlStatistics sqlStatistics = null;
			SqlDataReader sqlDataReader = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlCommand.Prepare|API> %d#", this.ObjectID);
			sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
			if ((this.IsPrepared && !this.IsDirty) || this.CommandType == CommandType.StoredProcedure || (CommandType.Text == this.CommandType && this.GetParameterCount(this._parameters) == 0))
			{
				if (this.Statistics != null)
				{
					this.Statistics.SafeIncrement(ref this.Statistics._prepares);
				}
				this._hiddenPrepare = false;
			}
			else
			{
				bool flag = true;
				SNIHandle snihandle = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
					this.ValidateCommand("Prepare", false);
					this.GetStateObject();
					if (this._parameters != null)
					{
						int count = this._parameters.Count;
						for (int i = 0; i < count; i++)
						{
							this._parameters[i].Prepare(this);
						}
					}
					sqlDataReader = this.InternalPrepare(CommandBehavior.Default);
				}
				catch (OutOfMemoryException ex)
				{
					flag = false;
					this._activeConnection.Abort(ex);
					throw;
				}
				catch (StackOverflowException ex2)
				{
					flag = false;
					this._activeConnection.Abort(ex2);
					throw;
				}
				catch (ThreadAbortException ex3)
				{
					flag = false;
					this._activeConnection.Abort(ex3);
					SqlInternalConnection.BestEffortCleanup(snihandle);
					throw;
				}
				catch (Exception ex4)
				{
					flag = ADP.IsCatchableExceptionType(ex4);
					throw;
				}
				finally
				{
					if (flag)
					{
						this._hiddenPrepare = false;
						if (sqlDataReader != null)
						{
							this._cachedMetaData = sqlDataReader.MetaData;
							sqlDataReader.Close();
						}
						this.PutStateObject();
					}
				}
			}
			SqlStatistics.StopTimer(sqlStatistics);
			Bid.ScopeLeave(ref intPtr);
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x00273214 File Offset: 0x00272614
		private SqlDataReader InternalPrepare(CommandBehavior behavior)
		{
			SqlDataReader sqlDataReader = null;
			if (this.IsDirty)
			{
				this.Unprepare(false);
				this.IsDirty = false;
			}
			if (this._activeConnection.IsShiloh)
			{
				this._execType = SqlCommand.EXECTYPE.PREPAREPENDING;
			}
			else
			{
				this.BuildPrepare(behavior);
				this._inPrepare = true;
				sqlDataReader = new SqlDataReader(this, behavior);
				try
				{
					this._stateObj.Parser.TdsExecuteRPC(this._rpcArrayOf1, this.CommandTimeout, false, null, this._stateObj, CommandType.StoredProcedure == this.CommandType);
					this._stateObj.Parser.Run(RunBehavior.UntilDone, this, sqlDataReader, null, this._stateObj);
				}
				catch
				{
					this._inPrepare = false;
					throw;
				}
				sqlDataReader.Bind(this._stateObj);
				this._execType = SqlCommand.EXECTYPE.PREPARED;
				Bid.Trace("<sc.SqlCommand.Prepare|INFO> %d#, Command prepared.\n", this.ObjectID);
			}
			if (this.Statistics != null)
			{
				this.Statistics.SafeIncrement(ref this.Statistics._prepares);
			}
			this._activeConnection.AddPreparedCommand(this);
			return sqlDataReader;
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x0027332C File Offset: 0x0027272C
		internal void Unprepare(bool isClosing)
		{
			if (this._activeConnection.IsContextConnection)
			{
				return;
			}
			bool flag = false;
			bool flag2 = true;
			try
			{
				if (this._stateObj == null)
				{
					this.GetStateObject();
					flag = true;
				}
				this.InternalUnprepare(isClosing);
			}
			catch (Exception ex)
			{
				flag2 = ADP.IsCatchableExceptionType(ex);
				throw;
			}
			finally
			{
				if (flag2 && flag)
				{
					this.PutStateObject();
				}
			}
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x002733B4 File Offset: 0x002727B4
		private void InternalUnprepare(bool isClosing)
		{
			if (this.IsShiloh)
			{
				this._execType = SqlCommand.EXECTYPE.PREPAREPENDING;
				if (isClosing)
				{
					this._prepareHandle = -1;
				}
			}
			else
			{
				if (this._prepareHandle != -1)
				{
					this.BuildUnprepare();
					this._stateObj.Parser.TdsExecuteRPC(this._rpcArrayOf1, this.CommandTimeout, false, null, this._stateObj, CommandType.StoredProcedure == this.CommandType);
					this._stateObj.Parser.Run(RunBehavior.UntilDone, this, null, null, this._stateObj);
					this._prepareHandle = -1;
				}
				this._execType = SqlCommand.EXECTYPE.UNPREPARED;
			}
			this._cachedMetaData = null;
			if (!isClosing)
			{
				this._activeConnection.RemovePreparedCommand(this);
			}
			Bid.Trace("<sc.SqlCommand.Prepare|INFO> %d#, Command unprepared.\n", this.ObjectID);
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x00273468 File Offset: 0x00272868
		public override void Cancel()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlCommand.Cancel|API> %d#", this.ObjectID);
			SqlStatistics sqlStatistics = null;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				if (this._activeConnection != null)
				{
					SqlInternalConnectionTds sqlInternalConnectionTds = this._activeConnection.InnerConnection as SqlInternalConnectionTds;
					if (sqlInternalConnectionTds != null)
					{
						lock (sqlInternalConnectionTds)
						{
							if (sqlInternalConnectionTds == this._activeConnection.InnerConnection as SqlInternalConnectionTds)
							{
								if (sqlInternalConnectionTds.Parser != null)
								{
									SNIHandle snihandle = null;
									RuntimeHelpers.PrepareConstrainedRegions();
									try
									{
										snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
										if (!this._pendingCancel)
										{
											this._pendingCancel = true;
											if (this._stateObj != null)
											{
												this._stateObj.Cancel(this.ObjectID);
											}
											else
											{
												SqlDataReader sqlDataReader = sqlInternalConnectionTds.FindLiveReader(this);
												if (sqlDataReader != null)
												{
													sqlDataReader.Cancel(this.ObjectID);
												}
											}
										}
									}
									catch (OutOfMemoryException ex)
									{
										this._activeConnection.Abort(ex);
										throw;
									}
									catch (StackOverflowException ex2)
									{
										this._activeConnection.Abort(ex2);
										throw;
									}
									catch (ThreadAbortException ex3)
									{
										this._activeConnection.Abort(ex3);
										SqlInternalConnection.BestEffortCleanup(snihandle);
										throw;
									}
								}
							}
						}
					}
				}
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x0027361C File Offset: 0x00272A1C
		public new SqlParameter CreateParameter()
		{
			return new SqlParameter();
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x00273630 File Offset: 0x00272A30
		protected override DbParameter CreateDbParameter()
		{
			return this.CreateParameter();
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x00273644 File Offset: 0x00272A44
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._cachedMetaData = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x00273664 File Offset: 0x00272A64
		public override object ExecuteScalar()
		{
			SqlConnection.ExecutePermission.Demand();
			this._pendingCancel = false;
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlCommand.ExecuteScalar|API> %d#", this.ObjectID);
			object obj;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				SqlDataReader sqlDataReader = this.RunExecuteReader(CommandBehavior.Default, RunBehavior.ReturnImmediately, true, "ExecuteScalar");
				obj = this.CompleteExecuteScalar(sqlDataReader, false);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
			return obj;
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x002736EC File Offset: 0x00272AEC
		private object CompleteExecuteScalar(SqlDataReader ds, bool returnSqlValue)
		{
			object obj = null;
			try
			{
				if (ds.Read() && ds.FieldCount > 0)
				{
					if (returnSqlValue)
					{
						obj = ds.GetSqlValue(0);
					}
					else
					{
						obj = ds.GetValue(0);
					}
				}
			}
			finally
			{
				ds.Close();
			}
			return obj;
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x00273748 File Offset: 0x00272B48
		public override int ExecuteNonQuery()
		{
			SqlConnection.ExecutePermission.Demand();
			this._pendingCancel = false;
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlCommand.ExecuteNonQuery|API> %d#", this.ObjectID);
			int num;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				num = this.InternalExecuteNonQuery(null, "ExecuteNonQuery", false);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x002737C4 File Offset: 0x00272BC4
		internal void ExecuteToPipe(SmiContext pipeContext)
		{
			SqlConnection.ExecutePermission.Demand();
			this._pendingCancel = false;
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlCommand.ExecuteToPipe|INFO> %d#", this.ObjectID);
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this.InternalExecuteNonQuery(null, "ExecuteNonQuery", true);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x00273840 File Offset: 0x00272C40
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteNonQuery()
		{
			return this.BeginExecuteNonQuery(null, null);
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x00273858 File Offset: 0x00272C58
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteNonQuery(AsyncCallback callback, object stateObject)
		{
			SqlConnection.ExecutePermission.Demand();
			this._pendingCancel = false;
			this.ValidateAsyncCommand();
			SqlStatistics sqlStatistics = null;
			IAsyncResult asyncResult;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				ExecutionContext executionContext = ((callback == null) ? null : ExecutionContext.Capture());
				DbAsyncResult dbAsyncResult = new DbAsyncResult(this, "EndExecuteNonQuery", callback, stateObject, executionContext);
				try
				{
					this.InternalExecuteNonQuery(dbAsyncResult, "BeginExecuteNonQuery", false);
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableOrSecurityExceptionType(ex))
					{
						throw;
					}
					this.PutStateObject();
					throw;
				}
				SNIHandle snihandle = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
					this.cachedAsyncState.SetActiveConnectionAndResult(dbAsyncResult, this._activeConnection);
					this._stateObj.ReadSni(dbAsyncResult, this._stateObj);
				}
				catch (OutOfMemoryException ex2)
				{
					this._activeConnection.Abort(ex2);
					throw;
				}
				catch (StackOverflowException ex3)
				{
					this._activeConnection.Abort(ex3);
					throw;
				}
				catch (ThreadAbortException ex4)
				{
					this._activeConnection.Abort(ex4);
					SqlInternalConnection.BestEffortCleanup(snihandle);
					throw;
				}
				catch (Exception)
				{
					if (this._cachedAsyncState != null)
					{
						this._cachedAsyncState.ResetAsyncState();
					}
					this.PutStateObject();
					throw;
				}
				asyncResult = dbAsyncResult;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return asyncResult;
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x00273A08 File Offset: 0x00272E08
		private void VerifyEndExecuteState(DbAsyncResult dbAsyncResult, string endMethod)
		{
			if (dbAsyncResult == null)
			{
				throw ADP.ArgumentNull("asyncResult");
			}
			if (dbAsyncResult.EndMethodName != endMethod)
			{
				throw ADP.MismatchedAsyncResult(dbAsyncResult.EndMethodName, endMethod);
			}
			if (!this.cachedAsyncState.IsActiveConnectionValid(this._activeConnection))
			{
				throw ADP.CommandAsyncOperationCompleted();
			}
			dbAsyncResult.CompareExchangeOwner(this, endMethod);
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x00273A60 File Offset: 0x00272E60
		private void WaitForAsyncResults(IAsyncResult asyncResult)
		{
			DbAsyncResult dbAsyncResult = (DbAsyncResult)asyncResult;
			if (!asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			dbAsyncResult.Reset();
			this._activeConnection.GetOpenTdsConnection().DecrementAsyncCount();
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x00273AA0 File Offset: 0x00272EA0
		public int EndExecuteNonQuery(IAsyncResult asyncResult)
		{
			SqlStatistics sqlStatistics = null;
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			int rowsAffected;
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this.VerifyEndExecuteState((DbAsyncResult)asyncResult, "EndExecuteNonQuery");
				this.WaitForAsyncResults(asyncResult);
				bool flag = true;
				try
				{
					this.NotifyDependency();
					this.CheckThrowSNIException();
					if (CommandType.Text == this.CommandType && this.GetParameterCount(this._parameters) == 0)
					{
						try
						{
							this._stateObj.Parser.Run(RunBehavior.UntilDone, this, null, null, this._stateObj);
							goto IL_0097;
						}
						finally
						{
							this.cachedAsyncState.ResetAsyncState();
						}
					}
					SqlDataReader sqlDataReader = this.CompleteAsyncExecuteReader();
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
					}
					IL_0097:;
				}
				catch (Exception ex)
				{
					flag = ADP.IsCatchableExceptionType(ex);
					throw;
				}
				finally
				{
					if (flag)
					{
						this.PutStateObject();
					}
				}
				rowsAffected = this._rowsAffected;
			}
			catch (OutOfMemoryException ex2)
			{
				this._activeConnection.Abort(ex2);
				throw;
			}
			catch (StackOverflowException ex3)
			{
				this._activeConnection.Abort(ex3);
				throw;
			}
			catch (ThreadAbortException ex4)
			{
				this._activeConnection.Abort(ex4);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return rowsAffected;
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x00273C60 File Offset: 0x00273060
		private int InternalExecuteNonQuery(DbAsyncResult result, string methodName, bool sendToPipe)
		{
			bool flag = null != result;
			SqlStatistics statistics = this.Statistics;
			this._rowsAffected = -1;
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			int rowsAffected;
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
				this.ValidateCommand(methodName, null != result);
				this.CheckNotificationStateAndAutoEnlist();
				if (this._activeConnection.IsContextConnection)
				{
					if (statistics != null)
					{
						statistics.SafeIncrement(ref statistics._unpreparedExecs);
					}
					this.RunExecuteNonQuerySmi(sendToPipe);
				}
				else if (!this.BatchRPCMode && CommandType.Text == this.CommandType && this.GetParameterCount(this._parameters) == 0)
				{
					if (statistics != null)
					{
						if (!this.IsDirty && this.IsPrepared)
						{
							statistics.SafeIncrement(ref statistics._preparedExecs);
						}
						else
						{
							statistics.SafeIncrement(ref statistics._unpreparedExecs);
						}
					}
					this.RunExecuteNonQueryTds(methodName, flag);
				}
				else
				{
					Bid.Trace("<sc.SqlCommand.ExecuteNonQuery|INFO> %d#, Command executed as RPC.\n", this.ObjectID);
					SqlDataReader sqlDataReader = this.RunExecuteReader(CommandBehavior.Default, RunBehavior.UntilDone, false, methodName, result);
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
					}
				}
				rowsAffected = this._rowsAffected;
			}
			catch (OutOfMemoryException ex)
			{
				this._activeConnection.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._activeConnection.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._activeConnection.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			return rowsAffected;
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x00273DE8 File Offset: 0x002731E8
		public XmlReader ExecuteXmlReader()
		{
			SqlConnection.ExecutePermission.Demand();
			this._pendingCancel = false;
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlCommand.ExecuteXmlReader|API> %d#", this.ObjectID);
			XmlReader xmlReader;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				SqlDataReader sqlDataReader = this.RunExecuteReader(CommandBehavior.SequentialAccess, RunBehavior.ReturnImmediately, true, "ExecuteXmlReader");
				xmlReader = this.CompleteXmlReader(sqlDataReader);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
			return xmlReader;
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x00273E70 File Offset: 0x00273270
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteXmlReader()
		{
			return this.BeginExecuteXmlReader(null, null);
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x00273E88 File Offset: 0x00273288
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteXmlReader(AsyncCallback callback, object stateObject)
		{
			SqlConnection.ExecutePermission.Demand();
			this._pendingCancel = false;
			this.ValidateAsyncCommand();
			SqlStatistics sqlStatistics = null;
			IAsyncResult asyncResult;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				ExecutionContext executionContext = ((callback == null) ? null : ExecutionContext.Capture());
				DbAsyncResult dbAsyncResult = new DbAsyncResult(this, "EndExecuteXmlReader", callback, stateObject, executionContext);
				try
				{
					this.RunExecuteReader(CommandBehavior.SequentialAccess, RunBehavior.ReturnImmediately, true, "BeginExecuteXmlReader", dbAsyncResult);
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableOrSecurityExceptionType(ex))
					{
						throw;
					}
					this.PutStateObject();
					throw;
				}
				SNIHandle snihandle = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
					this.cachedAsyncState.SetActiveConnectionAndResult(dbAsyncResult, this._activeConnection);
					this._stateObj.ReadSni(dbAsyncResult, this._stateObj);
				}
				catch (OutOfMemoryException ex2)
				{
					this._activeConnection.Abort(ex2);
					throw;
				}
				catch (StackOverflowException ex3)
				{
					this._activeConnection.Abort(ex3);
					throw;
				}
				catch (ThreadAbortException ex4)
				{
					this._activeConnection.Abort(ex4);
					SqlInternalConnection.BestEffortCleanup(snihandle);
					throw;
				}
				catch (Exception)
				{
					if (this._cachedAsyncState != null)
					{
						this._cachedAsyncState.ResetAsyncState();
					}
					this.PutStateObject();
					throw;
				}
				asyncResult = dbAsyncResult;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return asyncResult;
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x0027403C File Offset: 0x0027343C
		public XmlReader EndExecuteXmlReader(IAsyncResult asyncResult)
		{
			return this.CompleteXmlReader(this.InternalEndExecuteReader(asyncResult, "EndExecuteXmlReader"));
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x0027405C File Offset: 0x0027345C
		private XmlReader CompleteXmlReader(SqlDataReader ds)
		{
			XmlReader xmlReader = null;
			SmiExtendedMetaData[] internalSmiMetaData = ds.GetInternalSmiMetaData();
			bool flag = internalSmiMetaData != null && internalSmiMetaData.Length == 1 && (internalSmiMetaData[0].SqlDbType == SqlDbType.NText || internalSmiMetaData[0].SqlDbType == SqlDbType.NVarChar || internalSmiMetaData[0].SqlDbType == SqlDbType.Xml);
			if (flag)
			{
				try
				{
					SqlStream sqlStream = new SqlStream(ds, true, internalSmiMetaData[0].SqlDbType != SqlDbType.Xml);
					xmlReader = sqlStream.ToXmlReader();
				}
				catch (Exception ex)
				{
					if (ADP.IsCatchableExceptionType(ex))
					{
						ds.Close();
					}
					throw;
				}
			}
			if (xmlReader == null)
			{
				ds.Close();
				throw SQL.NonXmlResult();
			}
			return xmlReader;
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x0027410C File Offset: 0x0027350C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteReader()
		{
			return this.BeginExecuteReader(null, null, CommandBehavior.Default);
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x00274124 File Offset: 0x00273524
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject)
		{
			return this.BeginExecuteReader(callback, stateObject, CommandBehavior.Default);
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x0027413C File Offset: 0x0027353C
		protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
		{
			return this.ExecuteReader(behavior, "ExecuteReader");
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x00274158 File Offset: 0x00273558
		public new SqlDataReader ExecuteReader()
		{
			SqlStatistics sqlStatistics = null;
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlCommand.ExecuteReader|API> %d#", this.ObjectID);
			SqlDataReader sqlDataReader;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				sqlDataReader = this.ExecuteReader(CommandBehavior.Default, "ExecuteReader");
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				Bid.ScopeLeave(ref intPtr);
			}
			return sqlDataReader;
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x002741C0 File Offset: 0x002735C0
		public new SqlDataReader ExecuteReader(CommandBehavior behavior)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlCommand.ExecuteReader|API> %d#, behavior=%d{ds.CommandBehavior}", this.ObjectID, (int)behavior);
			SqlDataReader sqlDataReader;
			try
			{
				sqlDataReader = this.ExecuteReader(behavior, "ExecuteReader");
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return sqlDataReader;
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x00274214 File Offset: 0x00273614
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteReader(CommandBehavior behavior)
		{
			return this.BeginExecuteReader(null, null, behavior);
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x0027422C File Offset: 0x0027362C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject, CommandBehavior behavior)
		{
			SqlConnection.ExecutePermission.Demand();
			this._pendingCancel = false;
			SqlStatistics sqlStatistics = null;
			IAsyncResult asyncResult;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				asyncResult = this.InternalBeginExecuteReader(callback, stateObject, behavior);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return asyncResult;
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x0027428C File Offset: 0x0027368C
		internal SqlDataReader ExecuteReader(CommandBehavior behavior, string method)
		{
			SqlConnection.ExecutePermission.Demand();
			this._pendingCancel = false;
			SqlStatistics sqlStatistics = null;
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			SqlDataReader sqlDataReader2;
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				SqlDataReader sqlDataReader = this.RunExecuteReader(behavior, RunBehavior.ReturnImmediately, true, method);
				sqlDataReader2 = sqlDataReader;
			}
			catch (OutOfMemoryException ex)
			{
				this._activeConnection.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._activeConnection.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._activeConnection.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return sqlDataReader2;
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x0027438C File Offset: 0x0027378C
		public SqlDataReader EndExecuteReader(IAsyncResult asyncResult)
		{
			SqlStatistics sqlStatistics = null;
			SqlDataReader sqlDataReader;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				sqlDataReader = this.InternalEndExecuteReader(asyncResult, "EndExecuteReader");
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return sqlDataReader;
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x002743DC File Offset: 0x002737DC
		private IAsyncResult InternalBeginExecuteReader(AsyncCallback callback, object stateObject, CommandBehavior behavior)
		{
			ExecutionContext executionContext = ((callback == null) ? null : ExecutionContext.Capture());
			DbAsyncResult dbAsyncResult = new DbAsyncResult(this, "EndExecuteReader", callback, stateObject, executionContext);
			this.ValidateAsyncCommand();
			try
			{
				this.RunExecuteReader(behavior, RunBehavior.ReturnImmediately, true, "BeginExecuteReader", dbAsyncResult);
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(ex))
				{
					throw;
				}
				this.PutStateObject();
				throw;
			}
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
				this.cachedAsyncState.SetActiveConnectionAndResult(dbAsyncResult, this._activeConnection);
				this._stateObj.ReadSni(dbAsyncResult, this._stateObj);
			}
			catch (OutOfMemoryException ex2)
			{
				this._activeConnection.Abort(ex2);
				throw;
			}
			catch (StackOverflowException ex3)
			{
				this._activeConnection.Abort(ex3);
				throw;
			}
			catch (ThreadAbortException ex4)
			{
				this._activeConnection.Abort(ex4);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			catch (Exception)
			{
				if (this._cachedAsyncState != null)
				{
					this._cachedAsyncState.ResetAsyncState();
				}
				this.PutStateObject();
				throw;
			}
			return dbAsyncResult;
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x00274544 File Offset: 0x00273944
		private SqlDataReader InternalEndExecuteReader(IAsyncResult asyncResult, string endMethod)
		{
			this.VerifyEndExecuteState((DbAsyncResult)asyncResult, endMethod);
			this.WaitForAsyncResults(asyncResult);
			this.CheckThrowSNIException();
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			SqlDataReader sqlDataReader2;
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
				SqlDataReader sqlDataReader = this.CompleteAsyncExecuteReader();
				sqlDataReader2 = sqlDataReader;
			}
			catch (OutOfMemoryException ex)
			{
				this._activeConnection.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._activeConnection.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._activeConnection.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			return sqlDataReader2;
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x00274614 File Offset: 0x00273A14
		private static string UnquoteProcedurePart(string part)
		{
			if (part != null && 2 <= part.Length && '[' == part[0] && ']' == part[part.Length - 1])
			{
				part = part.Substring(1, part.Length - 2);
				part = part.Replace("]]", "]");
			}
			return part;
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x00274670 File Offset: 0x00273A70
		private static string UnquoteProcedureName(string name, out object groupNumber)
		{
			groupNumber = null;
			string text = name;
			if (text != null)
			{
				if (char.IsDigit(text[text.Length - 1]))
				{
					int num = text.LastIndexOf(';');
					if (num != -1)
					{
						string text2 = text.Substring(num + 1);
						int num2 = 0;
						if (int.TryParse(text2, out num2))
						{
							groupNumber = num2;
							text = text.Substring(0, num);
						}
					}
				}
				text = SqlCommand.UnquoteProcedurePart(text);
			}
			return text;
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x002746D8 File Offset: 0x00273AD8
		internal void DeriveParameters()
		{
			CommandType commandType = this.CommandType;
			if (commandType == CommandType.Text)
			{
				throw ADP.DeriveParametersNotSupported(this);
			}
			if (commandType != CommandType.StoredProcedure)
			{
				if (commandType != CommandType.TableDirect)
				{
					throw ADP.InvalidCommandType(this.CommandType);
				}
				throw ADP.DeriveParametersNotSupported(this);
			}
			else
			{
				this.ValidateCommand("DeriveParameters", false);
				string[] array = MultipartIdentifier.ParseMultipartIdentifier(this.CommandText, "[\"", "]\"", "SQL_SqlCommandCommandText", false);
				if (array[3] == null || ADP.IsEmpty(array[3]))
				{
					throw ADP.NoStoredProcedureExists(this.CommandText);
				}
				SqlCommand sqlCommand = null;
				StringBuilder stringBuilder = new StringBuilder();
				if (!ADP.IsEmpty(array[0]))
				{
					SqlCommandSet.BuildStoredProcedureName(stringBuilder, array[0]);
					stringBuilder.Append(".");
				}
				if (ADP.IsEmpty(array[1]))
				{
					array[1] = this.Connection.Database;
				}
				SqlCommandSet.BuildStoredProcedureName(stringBuilder, array[1]);
				stringBuilder.Append(".");
				string[] array2;
				bool flag;
				if (this.Connection.IsKatmaiOrNewer)
				{
					stringBuilder.Append("[sys].[").Append("sp_procedure_params_100_managed").Append("]");
					array2 = SqlCommand.KatmaiProcParamsNames;
					flag = true;
				}
				else
				{
					if (this.Connection.IsYukonOrNewer)
					{
						stringBuilder.Append("[sys].[").Append("sp_procedure_params_managed").Append("]");
					}
					else
					{
						stringBuilder.Append(".[").Append("sp_procedure_params_rowset").Append("]");
					}
					array2 = SqlCommand.PreKatmaiProcParamsNames;
					flag = false;
				}
				sqlCommand = new SqlCommand(stringBuilder.ToString(), this.Connection, this.Transaction);
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.Parameters.Add(new SqlParameter("@procedure_name", SqlDbType.NVarChar, 255));
				object obj;
				sqlCommand.Parameters[0].Value = SqlCommand.UnquoteProcedureName(array[3], out obj);
				if (obj != null)
				{
					SqlParameter sqlParameter = sqlCommand.Parameters.Add(new SqlParameter("@group_number", SqlDbType.Int));
					sqlParameter.Value = obj;
				}
				if (!ADP.IsEmpty(array[2]))
				{
					SqlParameter sqlParameter2 = sqlCommand.Parameters.Add(new SqlParameter("@procedure_schema", SqlDbType.NVarChar, 255));
					sqlParameter2.Value = SqlCommand.UnquoteProcedurePart(array[2]);
				}
				SqlDataReader sqlDataReader = null;
				List<SqlParameter> list = new List<SqlParameter>();
				bool flag2 = true;
				try
				{
					sqlDataReader = sqlCommand.ExecuteReader();
					while (sqlDataReader.Read())
					{
						SqlParameter sqlParameter3 = new SqlParameter();
						sqlParameter3.ParameterName = (string)sqlDataReader[array2[0]];
						if (flag)
						{
							sqlParameter3.SqlDbType = (SqlDbType)((short)sqlDataReader[array2[3]]);
							SqlDbType sqlDbType = sqlParameter3.SqlDbType;
							if (sqlDbType != SqlDbType.Image)
							{
								if (sqlDbType == SqlDbType.NText)
								{
									sqlParameter3.SqlDbType = SqlDbType.NVarChar;
									goto IL_02F3;
								}
								switch (sqlDbType)
								{
								case SqlDbType.Text:
									sqlParameter3.SqlDbType = SqlDbType.VarChar;
									goto IL_02F3;
								case SqlDbType.Timestamp:
									break;
								default:
									goto IL_02F3;
								}
							}
							sqlParameter3.SqlDbType = SqlDbType.VarBinary;
						}
						else
						{
							sqlParameter3.SqlDbType = MetaType.GetSqlDbTypeFromOleDbType((short)sqlDataReader[array2[2]], ADP.IsNull(sqlDataReader[array2[9]]) ? ADP.StrEmpty : ((string)sqlDataReader[array2[9]]));
						}
						IL_02F3:
						object obj2 = sqlDataReader[array2[4]];
						if (obj2 is int)
						{
							int num = (int)obj2;
							if (num == 0 && (sqlParameter3.SqlDbType == SqlDbType.NVarChar || sqlParameter3.SqlDbType == SqlDbType.VarBinary || sqlParameter3.SqlDbType == SqlDbType.VarChar))
							{
								num = -1;
							}
							sqlParameter3.Size = num;
						}
						sqlParameter3.Direction = this.ParameterDirectionFromOleDbDirection((short)sqlDataReader[array2[1]]);
						if (sqlParameter3.SqlDbType == SqlDbType.Decimal)
						{
							sqlParameter3.ScaleInternal = (byte)((short)sqlDataReader[array2[6]] & 255);
							sqlParameter3.PrecisionInternal = (byte)((short)sqlDataReader[array2[5]] & 255);
						}
						if (SqlDbType.Udt == sqlParameter3.SqlDbType)
						{
							string text;
							if (flag)
							{
								text = (string)sqlDataReader[array2[9]];
							}
							else
							{
								text = (string)sqlDataReader[array2[13]];
							}
							sqlParameter3.UdtTypeName = string.Concat(new object[]
							{
								sqlDataReader[array2[7]],
								".",
								sqlDataReader[array2[8]],
								".",
								text
							});
						}
						if (SqlDbType.Structured == sqlParameter3.SqlDbType)
						{
							sqlParameter3.TypeName = string.Concat(new object[]
							{
								sqlDataReader[array2[7]],
								".",
								sqlDataReader[array2[8]],
								".",
								sqlDataReader[array2[9]]
							});
						}
						if (SqlDbType.Xml == sqlParameter3.SqlDbType)
						{
							object obj3 = sqlDataReader[array2[10]];
							sqlParameter3.XmlSchemaCollectionDatabase = (ADP.IsNull(obj3) ? string.Empty : ((string)obj3));
							obj3 = sqlDataReader[array2[11]];
							sqlParameter3.XmlSchemaCollectionOwningSchema = (ADP.IsNull(obj3) ? string.Empty : ((string)obj3));
							obj3 = sqlDataReader[array2[12]];
							sqlParameter3.XmlSchemaCollectionName = (ADP.IsNull(obj3) ? string.Empty : ((string)obj3));
						}
						if (MetaType._IsVarTime(sqlParameter3.SqlDbType))
						{
							object obj4 = sqlDataReader[array2[14]];
							if (obj4 is int)
							{
								sqlParameter3.ScaleInternal = (byte)((int)obj4 & 255);
							}
						}
						list.Add(sqlParameter3);
					}
				}
				catch (Exception ex)
				{
					flag2 = ADP.IsCatchableExceptionType(ex);
					throw;
				}
				finally
				{
					if (flag2)
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
						sqlCommand.Connection = null;
					}
				}
				if (list.Count == 0)
				{
					throw ADP.NoStoredProcedureExists(this.CommandText);
				}
				this.Parameters.Clear();
				foreach (SqlParameter sqlParameter4 in list)
				{
					this._parameters.Add(sqlParameter4);
				}
				return;
			}
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x00274CF0 File Offset: 0x002740F0
		private ParameterDirection ParameterDirectionFromOleDbDirection(short oledbDirection)
		{
			switch (oledbDirection)
			{
			case 2:
				return ParameterDirection.InputOutput;
			case 3:
				return ParameterDirection.Output;
			case 4:
				return ParameterDirection.ReturnValue;
			default:
				return ParameterDirection.Input;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x00274D1C File Offset: 0x0027411C
		internal _SqlMetaDataSet MetaData
		{
			get
			{
				return this._cachedMetaData;
			}
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x00274D30 File Offset: 0x00274130
		private void CheckNotificationStateAndAutoEnlist()
		{
			if (this.NotificationAutoEnlist && this._activeConnection.IsYukonOrNewer)
			{
				string text = SqlCommand.SqlNotificationContext();
				if (!ADP.IsEmpty(text))
				{
					SqlDependency sqlDependency = SqlDependencyPerAppDomainDispatcher.SingletonInstance.LookupDependencyEntry(text);
					if (sqlDependency != null)
					{
						sqlDependency.AddCommandDependency(this);
					}
				}
			}
			if (this.Notification != null && this._sqlDep != null)
			{
				if (this._sqlDep.Options == null)
				{
					SqlInternalConnectionTds sqlInternalConnectionTds = this._activeConnection.InnerConnection as SqlInternalConnectionTds;
					SqlDependency.IdentityUserNamePair identityUserNamePair;
					if (sqlInternalConnectionTds.Identity != null)
					{
						identityUserNamePair = new SqlDependency.IdentityUserNamePair(sqlInternalConnectionTds.Identity, null);
					}
					else
					{
						identityUserNamePair = new SqlDependency.IdentityUserNamePair(null, sqlInternalConnectionTds.ConnectionOptions.UserID);
					}
					this.Notification.Options = SqlDependency.GetDefaultComposedOptions(this._activeConnection.DataSource, this.InternalTdsConnection.ServerProvidedFailOverPartner, identityUserNamePair, this._activeConnection.Database);
				}
				this.Notification.UserData = this._sqlDep.ComputeHashAndAddToDispatcher(this);
				this._sqlDep.AddToServerList(this._activeConnection.DataSource);
			}
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x00274E34 File Offset: 0x00274234
		[SecurityPermission(SecurityAction.Assert, Infrastructure = true)]
		internal static string SqlNotificationContext()
		{
			return CallContext.GetData("MS.SqlDependencyCookie") as string;
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x00274E50 File Offset: 0x00274250
		private void RunExecuteNonQueryTds(string methodName, bool async)
		{
			bool flag = true;
			try
			{
				this.GetStateObject();
				Bid.Trace("<sc.SqlCommand.ExecuteNonQuery|INFO> %d#, Command executed as SQLBATCH.\n", this.ObjectID);
				this._stateObj.Parser.TdsExecuteSQLBatch(this.CommandText, this.CommandTimeout, this.Notification, this._stateObj);
				this.NotifyDependency();
				if (async)
				{
					this._activeConnection.GetOpenTdsConnection(methodName).IncrementAsyncCount();
				}
				else
				{
					this._stateObj.Parser.Run(RunBehavior.UntilDone, this, null, null, this._stateObj);
				}
			}
			catch (Exception ex)
			{
				flag = ADP.IsCatchableExceptionType(ex);
				throw;
			}
			finally
			{
				if (flag && !async)
				{
					this.PutStateObject();
				}
			}
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x00274F28 File Offset: 0x00274328
		private void RunExecuteNonQuerySmi(bool sendToPipe)
		{
			SqlInternalConnectionSmi internalSmiConnection = this.InternalSmiConnection;
			try
			{
				this.SetUpSmiRequest(internalSmiConnection);
				SmiExecuteType smiExecuteType;
				if (sendToPipe)
				{
					smiExecuteType = SmiExecuteType.ToPipe;
				}
				else
				{
					smiExecuteType = SmiExecuteType.NonQuery;
				}
				SmiEventStream smiEventStream = null;
				bool flag = true;
				try
				{
					long num;
					Transaction transaction;
					internalSmiConnection.GetCurrentTransactionPair(out num, out transaction);
					if (Bid.AdvancedOn)
					{
						Bid.Trace("<sc.SqlCommand.RunExecuteNonQuerySmi|ADV> %d#, innerConnection=%d#, transactionId=0x%I64x, cmdBehavior=%d.\n", this.ObjectID, internalSmiConnection.ObjectID, num, 0);
					}
					if (SmiContextFactory.Instance.NegotiatedSmiVersion >= 210UL)
					{
						smiEventStream = this._smiRequest.Execute(internalSmiConnection.SmiConnection, num, transaction, CommandBehavior.Default, smiExecuteType);
					}
					else
					{
						smiEventStream = this._smiRequest.Execute(internalSmiConnection.SmiConnection, num, CommandBehavior.Default, smiExecuteType);
					}
					while (smiEventStream.HasEvents)
					{
						smiEventStream.ProcessEvent(this.EventSink);
					}
				}
				catch (Exception ex)
				{
					flag = ADP.IsCatchableExceptionType(ex);
					throw;
				}
				finally
				{
					if (smiEventStream != null && flag)
					{
						smiEventStream.Close(this.EventSink);
					}
				}
				this.EventSink.ProcessMessagesAndThrow();
			}
			catch (Exception ex2)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(ex2))
				{
					throw;
				}
				this.DisposeSmiRequest();
				throw;
			}
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x00275068 File Offset: 0x00274468
		internal SqlDataReader RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, string method)
		{
			return this.RunExecuteReader(cmdBehavior, runBehavior, returnStream, method, null);
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x00275084 File Offset: 0x00274484
		internal SqlDataReader RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, string method, DbAsyncResult result)
		{
			bool flag = null != result;
			this._rowsAffected = -1;
			if ((CommandBehavior.SingleRow & cmdBehavior) != CommandBehavior.Default)
			{
				cmdBehavior |= CommandBehavior.SingleResult;
			}
			this.ValidateCommand(method, null != result);
			this.CheckNotificationStateAndAutoEnlist();
			SNIHandle snihandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			SqlDataReader sqlDataReader;
			try
			{
				snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
				SqlStatistics statistics = this.Statistics;
				if (statistics != null)
				{
					if ((!this.IsDirty && this.IsPrepared && !this._hiddenPrepare) || (this.IsPrepared && this._execType == SqlCommand.EXECTYPE.PREPAREPENDING))
					{
						statistics.SafeIncrement(ref statistics._preparedExecs);
					}
					else
					{
						statistics.SafeIncrement(ref statistics._unpreparedExecs);
					}
				}
				if (this._activeConnection.IsContextConnection)
				{
					sqlDataReader = this.RunExecuteReaderSmi(cmdBehavior, runBehavior, returnStream);
				}
				else
				{
					sqlDataReader = this.RunExecuteReaderTds(cmdBehavior, runBehavior, returnStream, flag);
				}
			}
			catch (OutOfMemoryException ex)
			{
				this._activeConnection.Abort(ex);
				throw;
			}
			catch (StackOverflowException ex2)
			{
				this._activeConnection.Abort(ex2);
				throw;
			}
			catch (ThreadAbortException ex3)
			{
				this._activeConnection.Abort(ex3);
				SqlInternalConnection.BestEffortCleanup(snihandle);
				throw;
			}
			return sqlDataReader;
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x002751D8 File Offset: 0x002745D8
		private SqlDataReader RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, bool async)
		{
			bool flag = CommandBehavior.Default != (cmdBehavior & CommandBehavior.SchemaOnly);
			SqlDataReader sqlDataReader = null;
			_SqlRPC sqlRPC = null;
			string text = null;
			bool flag2 = true;
			try
			{
				this.GetStateObject();
				if (this.BatchRPCMode)
				{
					this._stateObj.Parser.TdsExecuteRPC(this._SqlRPCBatchArray, this.CommandTimeout, flag, this.Notification, this._stateObj, CommandType.StoredProcedure == this.CommandType);
				}
				else if (CommandType.Text == this.CommandType && this.GetParameterCount(this._parameters) == 0)
				{
					if (returnStream)
					{
						Bid.Trace("<sc.SqlCommand.ExecuteReader|INFO> %d#, Command executed as SQLBATCH.\n", this.ObjectID);
					}
					string text2 = this.GetCommandText(cmdBehavior) + this.GetResetOptionsString(cmdBehavior);
					this._stateObj.Parser.TdsExecuteSQLBatch(text2, this.CommandTimeout, this.Notification, this._stateObj);
				}
				else if (CommandType.Text == this.CommandType)
				{
					if (this.IsDirty)
					{
						if (this._execType == SqlCommand.EXECTYPE.PREPARED)
						{
							this._hiddenPrepare = true;
						}
						this.InternalUnprepare(false);
						this.IsDirty = false;
					}
					if (this._execType == SqlCommand.EXECTYPE.PREPARED)
					{
						sqlRPC = this.BuildExecute(flag);
					}
					else if (this._execType == SqlCommand.EXECTYPE.PREPAREPENDING)
					{
						sqlRPC = this.BuildPrepExec(cmdBehavior);
						this._execType = SqlCommand.EXECTYPE.PREPARED;
						this._activeConnection.AddPreparedCommand(this);
						this._inPrepare = true;
					}
					else
					{
						this.BuildExecuteSql(cmdBehavior, null, this._parameters, ref sqlRPC);
					}
					if (this._activeConnection.IsShiloh)
					{
						sqlRPC.options = 2;
					}
					if (returnStream)
					{
						Bid.Trace("<sc.SqlCommand.ExecuteReader|INFO> %d#, Command executed as RPC.\n", this.ObjectID);
					}
					this._stateObj.Parser.TdsExecuteRPC(this._rpcArrayOf1, this.CommandTimeout, flag, this.Notification, this._stateObj, CommandType.StoredProcedure == this.CommandType);
				}
				else
				{
					this.BuildRPC(flag, this._parameters, ref sqlRPC);
					text = this.GetSetOptionsString(cmdBehavior);
					if (returnStream)
					{
						Bid.Trace("<sc.SqlCommand.ExecuteReader|INFO> %d#, Command executed as RPC.\n", this.ObjectID);
					}
					if (text != null)
					{
						this._stateObj.Parser.TdsExecuteSQLBatch(text, this.CommandTimeout, this.Notification, this._stateObj);
						this._stateObj.Parser.Run(RunBehavior.UntilDone, this, null, null, this._stateObj);
						text = this.GetResetOptionsString(cmdBehavior);
					}
					this._activeConnection.CheckSQLDebug();
					this._stateObj.Parser.TdsExecuteRPC(this._rpcArrayOf1, this.CommandTimeout, flag, this.Notification, this._stateObj, CommandType.StoredProcedure == this.CommandType);
				}
				if (returnStream)
				{
					sqlDataReader = new SqlDataReader(this, cmdBehavior);
				}
				if (async)
				{
					this._activeConnection.GetOpenTdsConnection().IncrementAsyncCount();
					this.cachedAsyncState.SetAsyncReaderState(sqlDataReader, runBehavior, text);
				}
				else
				{
					this.FinishExecuteReader(sqlDataReader, runBehavior, text);
				}
			}
			catch (Exception ex)
			{
				flag2 = ADP.IsCatchableExceptionType(ex);
				throw;
			}
			finally
			{
				if (flag2 && !async)
				{
					this.PutStateObject();
				}
			}
			return sqlDataReader;
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x002754C4 File Offset: 0x002748C4
		private SqlDataReader RunExecuteReaderSmi(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream)
		{
			SqlInternalConnectionSmi internalSmiConnection = this.InternalSmiConnection;
			SmiEventStream smiEventStream = null;
			SqlDataReader sqlDataReader = null;
			try
			{
				this.SetUpSmiRequest(internalSmiConnection);
				long num;
				Transaction transaction;
				internalSmiConnection.GetCurrentTransactionPair(out num, out transaction);
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlCommand.RunExecuteReaderSmi|ADV> %d#, innerConnection=%d#, transactionId=0x%I64x, commandBehavior=%d.\n", this.ObjectID, internalSmiConnection.ObjectID, num, (int)cmdBehavior);
				}
				if (SmiContextFactory.Instance.NegotiatedSmiVersion >= 210UL)
				{
					smiEventStream = this._smiRequest.Execute(internalSmiConnection.SmiConnection, num, transaction, cmdBehavior, SmiExecuteType.Reader);
				}
				else
				{
					smiEventStream = this._smiRequest.Execute(internalSmiConnection.SmiConnection, num, cmdBehavior, SmiExecuteType.Reader);
				}
				if ((runBehavior & RunBehavior.UntilDone) != (RunBehavior)0)
				{
					while (smiEventStream.HasEvents)
					{
						smiEventStream.ProcessEvent(this.EventSink);
					}
					smiEventStream.Close(this.EventSink);
				}
				if (returnStream)
				{
					sqlDataReader = new SqlDataReaderSmi(smiEventStream, this, cmdBehavior, internalSmiConnection, this.EventSink);
					sqlDataReader.NextResult();
					this._activeConnection.AddWeakReference(sqlDataReader, 1);
				}
				this.EventSink.ProcessMessagesAndThrow();
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableOrSecurityExceptionType(ex))
				{
					throw;
				}
				if (smiEventStream != null)
				{
					smiEventStream.Close(this.EventSink);
				}
				this.DisposeSmiRequest();
				throw;
			}
			return sqlDataReader;
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x002755EC File Offset: 0x002749EC
		private SqlDataReader CompleteAsyncExecuteReader()
		{
			SqlDataReader cachedAsyncReader = this.cachedAsyncState.CachedAsyncReader;
			bool flag = true;
			try
			{
				this.FinishExecuteReader(cachedAsyncReader, this.cachedAsyncState.CachedRunBehavior, this.cachedAsyncState.CachedSetOptions);
			}
			catch (Exception ex)
			{
				flag = ADP.IsCatchableExceptionType(ex);
				throw;
			}
			finally
			{
				if (flag)
				{
					this.cachedAsyncState.ResetAsyncState();
					this.PutStateObject();
				}
			}
			return cachedAsyncReader;
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x00275680 File Offset: 0x00274A80
		private void FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, string resetOptionsString)
		{
			this.NotifyDependency();
			if (runBehavior == RunBehavior.UntilDone)
			{
				try
				{
					this._stateObj.Parser.Run(RunBehavior.UntilDone, this, ds, null, this._stateObj);
				}
				catch (Exception ex)
				{
					if (ADP.IsCatchableExceptionType(ex))
					{
						if (this._inPrepare)
						{
							this._inPrepare = false;
							this.IsDirty = true;
							this._execType = SqlCommand.EXECTYPE.PREPAREPENDING;
						}
						if (ds != null)
						{
							ds.Close();
						}
					}
					throw;
				}
			}
			if (ds != null)
			{
				ds.Bind(this._stateObj);
				this._stateObj = null;
				ds.ResetOptionsString = resetOptionsString;
				this._activeConnection.AddWeakReference(ds, 1);
				try
				{
					this._cachedMetaData = ds.MetaData;
					ds.IsInitialized = true;
				}
				catch (Exception ex2)
				{
					if (ADP.IsCatchableExceptionType(ex2))
					{
						if (this._inPrepare)
						{
							this._inPrepare = false;
							this.IsDirty = true;
							this._execType = SqlCommand.EXECTYPE.PREPAREPENDING;
						}
						ds.Close();
					}
					throw;
				}
			}
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x0027578C File Offset: 0x00274B8C
		private void NotifyDependency()
		{
			if (this._sqlDep != null)
			{
				this._sqlDep.StartTimer(this.Notification);
			}
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x002757B4 File Offset: 0x00274BB4
		public SqlCommand Clone()
		{
			SqlCommand sqlCommand = new SqlCommand(this);
			Bid.Trace("<sc.SqlCommand.Clone|API> %d#, clone=%d#\n", this.ObjectID, sqlCommand.ObjectID);
			return sqlCommand;
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x002757E0 File Offset: 0x00274BE0
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x002757F4 File Offset: 0x00274BF4
		private void ValidateCommand(string method, bool async)
		{
			if (this._activeConnection == null)
			{
				throw ADP.ConnectionRequired(method);
			}
			SqlInternalConnectionTds sqlInternalConnectionTds = this._activeConnection.InnerConnection as SqlInternalConnectionTds;
			if (sqlInternalConnectionTds != null && sqlInternalConnectionTds.Parser.State != TdsParserState.OpenLoggedIn)
			{
				if (sqlInternalConnectionTds.Parser.State == TdsParserState.Closed)
				{
					throw ADP.OpenConnectionRequired(method, ConnectionState.Closed);
				}
				throw ADP.OpenConnectionRequired(method, ConnectionState.Broken);
			}
			else
			{
				this.ValidateAsyncCommand();
				SNIHandle snihandle = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					snihandle = SqlInternalConnection.GetBestEffortCleanupTarget(this._activeConnection);
					this._activeConnection.ValidateConnectionForExecute(method, this);
				}
				catch (OutOfMemoryException ex)
				{
					this._activeConnection.Abort(ex);
					throw;
				}
				catch (StackOverflowException ex2)
				{
					this._activeConnection.Abort(ex2);
					throw;
				}
				catch (ThreadAbortException ex3)
				{
					this._activeConnection.Abort(ex3);
					SqlInternalConnection.BestEffortCleanup(snihandle);
					throw;
				}
				if (this._transaction != null && this._transaction.Connection == null)
				{
					this._transaction = null;
				}
				if (this._activeConnection.HasLocalTransactionFromAPI && this._transaction == null)
				{
					throw ADP.TransactionRequired(method);
				}
				if (this._transaction != null && this._activeConnection != this._transaction.Connection)
				{
					throw ADP.TransactionConnectionMismatch();
				}
				if (ADP.IsEmpty(this.CommandText))
				{
					throw ADP.CommandTextRequired(method);
				}
				if (this.Notification != null && !this._activeConnection.IsYukonOrNewer)
				{
					throw SQL.NotificationsRequireYukon();
				}
				if (async && !this._activeConnection.Asynchronous)
				{
					throw SQL.AsyncConnectionRequired();
				}
				return;
			}
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x002759A0 File Offset: 0x00274DA0
		private void ValidateAsyncCommand()
		{
			if (this.cachedAsyncState.PendingAsyncOperation)
			{
				if (this.cachedAsyncState.IsActiveConnectionValid(this._activeConnection))
				{
					throw SQL.PendingBeginXXXExists();
				}
				this._stateObj = null;
				this.cachedAsyncState.ResetAsyncState();
			}
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x002759E8 File Offset: 0x00274DE8
		private void GetStateObject()
		{
			if (this._pendingCancel)
			{
				this._pendingCancel = false;
				throw SQL.OperationCancelled();
			}
			TdsParserStateObject session = this._activeConnection.Parser.GetSession(this);
			session.StartSession(this.ObjectID);
			this._stateObj = session;
			if (this._pendingCancel)
			{
				this._pendingCancel = false;
				throw SQL.OperationCancelled();
			}
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x00275A4C File Offset: 0x00274E4C
		private void PutStateObject()
		{
			TdsParserStateObject stateObj = this._stateObj;
			this._stateObj = null;
			if (stateObj != null)
			{
				stateObj.CloseSession();
			}
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x00275A70 File Offset: 0x00274E70
		internal void OnDoneProc()
		{
			if (this.BatchRPCMode)
			{
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].cumulativeRecordsAffected = this._rowsAffected;
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].recordsAffected = new int?((0 < this._currentlyExecutingBatch && 0 <= this._rowsAffected) ? (this._rowsAffected - Math.Max(this._SqlRPCBatchArray[this._currentlyExecutingBatch - 1].cumulativeRecordsAffected, 0)) : this._rowsAffected);
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].errorsIndexStart = ((0 < this._currentlyExecutingBatch) ? this._SqlRPCBatchArray[this._currentlyExecutingBatch - 1].errorsIndexEnd : 0);
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].errorsIndexEnd = this._stateObj.Parser.Errors.Count;
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].errors = this._stateObj.Parser.Errors;
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].warningsIndexStart = ((0 < this._currentlyExecutingBatch) ? this._SqlRPCBatchArray[this._currentlyExecutingBatch - 1].warningsIndexEnd : 0);
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].warningsIndexEnd = this._stateObj.Parser.Warnings.Count;
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].warnings = this._stateObj.Parser.Warnings;
				this._currentlyExecutingBatch++;
			}
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x00275BF8 File Offset: 0x00274FF8
		internal void OnReturnStatus(int status)
		{
			if (this._inPrepare)
			{
				return;
			}
			SqlParameterCollection sqlParameterCollection = this._parameters;
			if (this.BatchRPCMode)
			{
				if (this._parameterCollectionList.Count > this._currentlyExecutingBatch)
				{
					sqlParameterCollection = this._parameterCollectionList[this._currentlyExecutingBatch];
				}
				else
				{
					sqlParameterCollection = null;
				}
			}
			int parameterCount = this.GetParameterCount(sqlParameterCollection);
			int i = 0;
			while (i < parameterCount)
			{
				SqlParameter sqlParameter = sqlParameterCollection[i];
				if (sqlParameter.Direction == ParameterDirection.ReturnValue)
				{
					object value = sqlParameter.Value;
					if (value != null && value.GetType() == typeof(SqlInt32))
					{
						sqlParameter.Value = new SqlInt32(status);
						return;
					}
					sqlParameter.Value = status;
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x00275CA8 File Offset: 0x002750A8
		internal void OnReturnValue(SqlReturnValue rec)
		{
			if (this._inPrepare)
			{
				if (!rec.value.IsNull)
				{
					this._prepareHandle = rec.value.Int32;
				}
				this._inPrepare = false;
				return;
			}
			SqlParameterCollection currentParameterCollection = this.GetCurrentParameterCollection();
			int parameterCount = this.GetParameterCount(currentParameterCollection);
			SqlParameter parameterForOutputValueExtraction = this.GetParameterForOutputValueExtraction(currentParameterCollection, rec.parameter, parameterCount);
			if (parameterForOutputValueExtraction != null)
			{
				object value = parameterForOutputValueExtraction.Value;
				if (SqlDbType.Udt == parameterForOutputValueExtraction.SqlDbType)
				{
					try
					{
						SqlConnection.CheckGetExtendedUDTInfo(rec, true);
						object obj;
						if (rec.value.IsNull)
						{
							obj = DBNull.Value;
						}
						else
						{
							obj = rec.value.ByteArray;
						}
						parameterForOutputValueExtraction.Value = this.Connection.GetUdtValue(obj, rec, false);
					}
					catch (FileNotFoundException ex)
					{
						parameterForOutputValueExtraction.SetUdtLoadError(ex);
					}
					catch (FileLoadException ex2)
					{
						parameterForOutputValueExtraction.SetUdtLoadError(ex2);
					}
					return;
				}
				parameterForOutputValueExtraction.SetSqlBuffer(rec.value);
				MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(rec.type, rec.isMultiValued);
				if (rec.type == SqlDbType.Decimal)
				{
					parameterForOutputValueExtraction.ScaleInternal = rec.scale;
					parameterForOutputValueExtraction.PrecisionInternal = rec.precision;
				}
				else if (metaTypeFromSqlDbType.IsVarTime)
				{
					parameterForOutputValueExtraction.ScaleInternal = rec.scale;
				}
				else if (rec.type == SqlDbType.Xml)
				{
					SqlCachedBuffer sqlCachedBuffer = parameterForOutputValueExtraction.Value as SqlCachedBuffer;
					if (sqlCachedBuffer != null)
					{
						parameterForOutputValueExtraction.Value = sqlCachedBuffer.ToString();
					}
				}
				if (rec.collation != null)
				{
					parameterForOutputValueExtraction.Collation = rec.collation;
				}
			}
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x00275E3C File Offset: 0x0027523C
		internal void OnParametersAvailableSmi(SmiParameterMetaData[] paramMetaData, ITypedGettersV3 parameterValues)
		{
			for (int i = 0; i < paramMetaData.Length; i++)
			{
				this.OnParameterAvailableSmi(paramMetaData[i], parameterValues, i);
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x00275E64 File Offset: 0x00275264
		internal void OnParameterAvailableSmi(SmiParameterMetaData metaData, ITypedGettersV3 parameterValues, int ordinal)
		{
			if (ParameterDirection.Input != metaData.Direction)
			{
				string text = null;
				if (ParameterDirection.ReturnValue != metaData.Direction)
				{
					text = metaData.Name;
				}
				SqlParameterCollection currentParameterCollection = this.GetCurrentParameterCollection();
				int parameterCount = this.GetParameterCount(currentParameterCollection);
				SqlParameter parameterForOutputValueExtraction = this.GetParameterForOutputValueExtraction(currentParameterCollection, text, parameterCount);
				if (parameterForOutputValueExtraction != null)
				{
					parameterForOutputValueExtraction.LocaleId = (int)metaData.LocaleId;
					parameterForOutputValueExtraction.CompareInfo = metaData.CompareOptions;
					SqlBuffer sqlBuffer = new SqlBuffer();
					object obj;
					if (this._activeConnection.IsKatmaiOrNewer)
					{
						obj = ValueUtilsSmi.GetOutputParameterV200Smi(this.OutParamEventSink, (SmiTypedGetterSetter)parameterValues, ordinal, metaData, this._smiRequestContext, sqlBuffer);
					}
					else
					{
						obj = ValueUtilsSmi.GetOutputParameterV3Smi(this.OutParamEventSink, parameterValues, ordinal, metaData, this._smiRequestContext, sqlBuffer);
					}
					if (obj != null)
					{
						parameterForOutputValueExtraction.Value = obj;
						return;
					}
					parameterForOutputValueExtraction.SetSqlBuffer(sqlBuffer);
				}
			}
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x00275F24 File Offset: 0x00275324
		private SqlParameterCollection GetCurrentParameterCollection()
		{
			if (!this.BatchRPCMode)
			{
				return this._parameters;
			}
			if (this._parameterCollectionList.Count > this._currentlyExecutingBatch)
			{
				return this._parameterCollectionList[this._currentlyExecutingBatch];
			}
			return null;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x00275F68 File Offset: 0x00275368
		private SqlParameter GetParameterForOutputValueExtraction(SqlParameterCollection parameters, string paramName, int paramCount)
		{
			SqlParameter sqlParameter = null;
			bool flag = false;
			if (paramName == null)
			{
				for (int i = 0; i < paramCount; i++)
				{
					sqlParameter = parameters[i];
					if (sqlParameter.Direction == ParameterDirection.ReturnValue)
					{
						flag = true;
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < paramCount; j++)
				{
					sqlParameter = parameters[j];
					if (sqlParameter.Direction != ParameterDirection.Input && sqlParameter.Direction != ParameterDirection.ReturnValue && paramName == sqlParameter.ParameterNameFixed)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return sqlParameter;
			}
			return null;
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x00275FE0 File Offset: 0x002753E0
		private void GetRPCObject(int paramCount, ref _SqlRPC rpc)
		{
			if (rpc == null)
			{
				if (this._rpcArrayOf1 == null)
				{
					this._rpcArrayOf1 = new _SqlRPC[1];
					this._rpcArrayOf1[0] = new _SqlRPC();
				}
				rpc = this._rpcArrayOf1[0];
			}
			rpc.ProcID = 0;
			rpc.rpcName = null;
			rpc.options = 0;
			rpc.recordsAffected = null;
			rpc.cumulativeRecordsAffected = -1;
			rpc.errorsIndexStart = 0;
			rpc.errorsIndexEnd = 0;
			rpc.errors = null;
			rpc.warningsIndexStart = 0;
			rpc.warningsIndexEnd = 0;
			rpc.warnings = null;
			if (rpc.parameters == null || rpc.parameters.Length < paramCount)
			{
				rpc.parameters = new SqlParameter[paramCount];
			}
			else if (rpc.parameters.Length > paramCount)
			{
				rpc.parameters[paramCount] = null;
			}
			if (rpc.paramoptions == null || rpc.paramoptions.Length < paramCount)
			{
				rpc.paramoptions = new byte[paramCount];
				return;
			}
			for (int i = 0; i < paramCount; i++)
			{
				rpc.paramoptions[i] = 0;
			}
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x002760EC File Offset: 0x002754EC
		private void SetUpRPCParameters(_SqlRPC rpc, int startCount, bool inSchema, SqlParameterCollection parameters)
		{
			int parameterCount = this.GetParameterCount(parameters);
			int num = startCount;
			TdsParser parser = this._activeConnection.Parser;
			bool isYukonOrNewer = parser.IsYukonOrNewer;
			for (int i = 0; i < parameterCount; i++)
			{
				SqlParameter sqlParameter = parameters[i];
				sqlParameter.Validate(i, CommandType.StoredProcedure == this.CommandType);
				sqlParameter.ValidateTypeLengths(isYukonOrNewer);
				if (SqlCommand.ShouldSendParameter(sqlParameter))
				{
					rpc.parameters[num] = sqlParameter;
					if (sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Output)
					{
						rpc.paramoptions[num] = 1;
					}
					if (sqlParameter.Direction != ParameterDirection.Output && sqlParameter.Value == null && (!inSchema || SqlDbType.Structured == sqlParameter.SqlDbType))
					{
						byte[] paramoptions = rpc.paramoptions;
						int num2 = num;
						paramoptions[num2] |= 2;
					}
					num++;
				}
			}
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x002761B8 File Offset: 0x002755B8
		private _SqlRPC BuildPrepExec(CommandBehavior behavior)
		{
			int num = 3;
			int num2 = this.CountSendableParameters(this._parameters);
			_SqlRPC sqlRPC = null;
			this.GetRPCObject(num2 + num, ref sqlRPC);
			sqlRPC.ProcID = 13;
			sqlRPC.rpcName = "sp_prepexec";
			SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
			sqlParameter.Direction = ParameterDirection.InputOutput;
			sqlParameter.Value = this._prepareHandle;
			sqlRPC.parameters[0] = sqlParameter;
			sqlRPC.paramoptions[0] = 1;
			string text = this.BuildParamList(this._stateObj.Parser, this._parameters);
			sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
			sqlParameter.Value = text;
			sqlRPC.parameters[1] = sqlParameter;
			string commandText = this.GetCommandText(behavior);
			sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, commandText.Length);
			sqlParameter.Value = commandText;
			sqlRPC.parameters[2] = sqlParameter;
			this.SetUpRPCParameters(sqlRPC, num, false, this._parameters);
			return sqlRPC;
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x002762C0 File Offset: 0x002756C0
		private static bool ShouldSendParameter(SqlParameter p)
		{
			switch (p.Direction)
			{
			case ParameterDirection.Input:
			case ParameterDirection.Output:
			case ParameterDirection.InputOutput:
				return true;
			case ParameterDirection.ReturnValue:
				return false;
			}
			return false;
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x002762FC File Offset: 0x002756FC
		private int CountSendableParameters(SqlParameterCollection parameters)
		{
			int num = 0;
			if (parameters != null)
			{
				int count = parameters.Count;
				for (int i = 0; i < count; i++)
				{
					if (SqlCommand.ShouldSendParameter(parameters[i]))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x00276334 File Offset: 0x00275734
		private int GetParameterCount(SqlParameterCollection parameters)
		{
			if (parameters == null)
			{
				return 0;
			}
			return parameters.Count;
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0027634C File Offset: 0x0027574C
		private void BuildRPC(bool inSchema, SqlParameterCollection parameters, ref _SqlRPC rpc)
		{
			int num = this.CountSendableParameters(parameters);
			this.GetRPCObject(num, ref rpc);
			rpc.rpcName = this.CommandText;
			this.SetUpRPCParameters(rpc, 0, inSchema, parameters);
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x00276384 File Offset: 0x00275784
		private _SqlRPC BuildUnprepare()
		{
			_SqlRPC sqlRPC = null;
			this.GetRPCObject(1, ref sqlRPC);
			sqlRPC.ProcID = 15;
			sqlRPC.rpcName = "sp_unprepare";
			SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
			sqlParameter.Value = this._prepareHandle;
			sqlRPC.parameters[0] = sqlParameter;
			return sqlRPC;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x002763D4 File Offset: 0x002757D4
		private _SqlRPC BuildExecute(bool inSchema)
		{
			int num = 1;
			int num2 = this.CountSendableParameters(this._parameters);
			_SqlRPC sqlRPC = null;
			this.GetRPCObject(num2 + num, ref sqlRPC);
			sqlRPC.ProcID = 12;
			sqlRPC.rpcName = "sp_execute";
			SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
			sqlParameter.Value = this._prepareHandle;
			sqlRPC.parameters[0] = sqlParameter;
			this.SetUpRPCParameters(sqlRPC, num, inSchema, this._parameters);
			return sqlRPC;
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x00276444 File Offset: 0x00275844
		private void BuildExecuteSql(CommandBehavior behavior, string commandText, SqlParameterCollection parameters, ref _SqlRPC rpc)
		{
			int num = this.CountSendableParameters(parameters);
			int num2;
			if (num > 0)
			{
				num2 = 2;
			}
			else
			{
				num2 = 1;
			}
			this.GetRPCObject(num + num2, ref rpc);
			rpc.ProcID = 10;
			rpc.rpcName = "sp_executesql";
			if (commandText == null)
			{
				commandText = this.GetCommandText(behavior);
			}
			SqlParameter sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, commandText.Length);
			sqlParameter.Value = commandText;
			rpc.parameters[0] = sqlParameter;
			if (num > 0)
			{
				string text = this.BuildParamList(this._stateObj.Parser, this.BatchRPCMode ? parameters : this._parameters);
				sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
				sqlParameter.Value = text;
				rpc.parameters[1] = sqlParameter;
				bool flag = CommandBehavior.Default != (behavior & CommandBehavior.SchemaOnly);
				this.SetUpRPCParameters(rpc, num2, flag, parameters);
			}
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x00276538 File Offset: 0x00275938
		internal string BuildParamList(TdsParser parser, SqlParameterCollection parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool isYukonOrNewer = parser.IsYukonOrNewer;
			int count = parameters.Count;
			for (int i = 0; i < count; i++)
			{
				SqlParameter sqlParameter = parameters[i];
				sqlParameter.Validate(i, CommandType.StoredProcedure == this.CommandType);
				if (SqlCommand.ShouldSendParameter(sqlParameter))
				{
					if (flag)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(sqlParameter.ParameterNameFixed);
					MetaType metaType = sqlParameter.InternalMetaType;
					stringBuilder.Append(" ");
					if (metaType.SqlDbType == SqlDbType.Udt)
					{
						string udtTypeName = sqlParameter.UdtTypeName;
						if (ADP.IsEmpty(udtTypeName))
						{
							throw SQL.MustSetUdtTypeNameForUdtParams();
						}
						stringBuilder.Append(udtTypeName);
					}
					else if (metaType.SqlDbType == SqlDbType.Structured)
					{
						string typeName = sqlParameter.TypeName;
						if (ADP.IsEmpty(typeName))
						{
							throw SQL.MustSetTypeNameForParam(metaType.TypeName, sqlParameter.ParameterNameFixed);
						}
						stringBuilder.Append(typeName);
						stringBuilder.Append(" READONLY");
					}
					else
					{
						metaType = sqlParameter.ValidateTypeLengths(isYukonOrNewer);
						stringBuilder.Append(metaType.TypeName);
					}
					flag = true;
					if (metaType.SqlDbType == SqlDbType.Decimal)
					{
						byte b = sqlParameter.GetActualPrecision();
						byte actualScale = sqlParameter.GetActualScale();
						stringBuilder.Append('(');
						if (b == 0)
						{
							if (this.IsShiloh)
							{
								b = 29;
							}
							else
							{
								b = 28;
							}
						}
						stringBuilder.Append(b);
						stringBuilder.Append(',');
						stringBuilder.Append(actualScale);
						stringBuilder.Append(')');
					}
					else if (metaType.IsVarTime)
					{
						byte actualScale2 = sqlParameter.GetActualScale();
						stringBuilder.Append('(');
						stringBuilder.Append(actualScale2);
						stringBuilder.Append(')');
					}
					else if (!metaType.IsFixed && !metaType.IsLong && metaType.SqlDbType != SqlDbType.Timestamp && metaType.SqlDbType != SqlDbType.Udt && SqlDbType.Structured != metaType.SqlDbType)
					{
						int num = sqlParameter.Size;
						stringBuilder.Append('(');
						if (metaType.IsAnsiType)
						{
							object coercedValue = sqlParameter.GetCoercedValue();
							string text = null;
							if (coercedValue != null && DBNull.Value != coercedValue)
							{
								text = coercedValue as string;
								if (text == null)
								{
									SqlString sqlString = ((coercedValue is SqlString) ? ((SqlString)coercedValue) : SqlString.Null);
									if (!sqlString.IsNull)
									{
										text = sqlString.Value;
									}
								}
							}
							if (text != null)
							{
								int encodingCharLength = parser.GetEncodingCharLength(text, sqlParameter.GetActualSize(), sqlParameter.Offset, null);
								if (encodingCharLength > num)
								{
									num = encodingCharLength;
								}
							}
						}
						if (num == 0)
						{
							num = (metaType.IsSizeInCharacters ? 4000 : 8000);
						}
						stringBuilder.Append(num);
						stringBuilder.Append(')');
					}
					else if (metaType.IsPlp && metaType.SqlDbType != SqlDbType.Xml && metaType.SqlDbType != SqlDbType.Udt)
					{
						stringBuilder.Append("(max) ");
					}
					if (sqlParameter.Direction != ParameterDirection.Input)
					{
						stringBuilder.Append(" output");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0027681C File Offset: 0x00275C1C
		private string GetSetOptionsString(CommandBehavior behavior)
		{
			string text = null;
			if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly) || CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
			{
				text = " SET FMTONLY OFF;";
				if (CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
				{
					text += " SET NO_BROWSETABLE ON;";
				}
				if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly))
				{
					text += " SET FMTONLY ON;";
				}
			}
			return text;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x00276864 File Offset: 0x00275C64
		private string GetResetOptionsString(CommandBehavior behavior)
		{
			string text = null;
			if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly))
			{
				text += " SET FMTONLY OFF;";
			}
			if (CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
			{
				text += " SET NO_BROWSETABLE OFF;";
			}
			return text;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x00276898 File Offset: 0x00275C98
		private string GetCommandText(CommandBehavior behavior)
		{
			return this.GetSetOptionsString(behavior) + this.CommandText;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x002768B8 File Offset: 0x00275CB8
		private _SqlRPC BuildPrepare(CommandBehavior behavior)
		{
			_SqlRPC sqlRPC = null;
			this.GetRPCObject(3, ref sqlRPC);
			sqlRPC.ProcID = 11;
			sqlRPC.rpcName = "sp_prepare";
			SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
			sqlParameter.Direction = ParameterDirection.Output;
			sqlRPC.parameters[0] = sqlParameter;
			sqlRPC.paramoptions[0] = 1;
			string text = this.BuildParamList(this._stateObj.Parser, this._parameters);
			sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
			sqlParameter.Value = text;
			sqlRPC.parameters[1] = sqlParameter;
			string commandText = this.GetCommandText(behavior);
			sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, commandText.Length);
			sqlParameter.Value = commandText;
			sqlRPC.parameters[2] = sqlParameter;
			return sqlRPC;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x0027698C File Offset: 0x00275D8C
		private void CheckThrowSNIException()
		{
			if (this._stateObj != null && this._stateObj._error != null)
			{
				this._stateObj.Parser.Errors.Add(this._stateObj._error);
				this._stateObj._error = null;
				this._stateObj.Parser.ThrowExceptionAndWarning(this._stateObj);
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x002769F0 File Offset: 0x00275DF0
		private bool IsPrepared
		{
			get
			{
				return this._execType != SqlCommand.EXECTYPE.UNPREPARED;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x00276A0C File Offset: 0x00275E0C
		private bool IsUserPrepared
		{
			get
			{
				return this.IsPrepared && !this._hiddenPrepare && !this.IsDirty;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x0600243A RID: 9274 RVA: 0x00276A34 File Offset: 0x00275E34
		// (set) Token: 0x0600243B RID: 9275 RVA: 0x00276A6C File Offset: 0x00275E6C
		internal bool IsDirty
		{
			get
			{
				return this.IsPrepared && (this._dirty || (this._parameters != null && this._parameters.IsDirty));
			}
			set
			{
				this._dirty = value && this.IsPrepared;
				if (this._parameters != null)
				{
					this._parameters.IsDirty = this._dirty;
				}
				this._cachedMetaData = null;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x00276AAC File Offset: 0x00275EAC
		// (set) Token: 0x0600243D RID: 9277 RVA: 0x00276AC0 File Offset: 0x00275EC0
		internal int InternalRecordsAffected
		{
			get
			{
				return this._rowsAffected;
			}
			set
			{
				if (-1 == this._rowsAffected)
				{
					this._rowsAffected = value;
					return;
				}
				if (0 < value)
				{
					this._rowsAffected += value;
				}
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x00276AF0 File Offset: 0x00275EF0
		// (set) Token: 0x0600243F RID: 9279 RVA: 0x00276B04 File Offset: 0x00275F04
		internal bool BatchRPCMode
		{
			get
			{
				return this._batchRPCMode;
			}
			set
			{
				this._batchRPCMode = value;
				if (!this._batchRPCMode)
				{
					this.ClearBatchCommand();
					return;
				}
				if (this._RPCList == null)
				{
					this._RPCList = new List<_SqlRPC>();
				}
				if (this._parameterCollectionList == null)
				{
					this._parameterCollectionList = new List<SqlParameterCollection>();
				}
			}
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x00276B50 File Offset: 0x00275F50
		internal void ClearBatchCommand()
		{
			List<_SqlRPC> rpclist = this._RPCList;
			if (rpclist != null)
			{
				rpclist.Clear();
			}
			if (this._parameterCollectionList != null)
			{
				this._parameterCollectionList.Clear();
			}
			this._SqlRPCBatchArray = null;
			this._currentlyExecutingBatch = 0;
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x00276B90 File Offset: 0x00275F90
		internal void AddBatchCommand(string commandText, SqlParameterCollection parameters, CommandType cmdType)
		{
			_SqlRPC sqlRPC = new _SqlRPC();
			this.CommandText = commandText;
			this.CommandType = cmdType;
			this.GetStateObject();
			if (cmdType == CommandType.StoredProcedure)
			{
				this.BuildRPC(false, parameters, ref sqlRPC);
			}
			else
			{
				this.BuildExecuteSql(CommandBehavior.Default, commandText, parameters, ref sqlRPC);
			}
			this._RPCList.Add(sqlRPC);
			this._parameterCollectionList.Add(parameters);
			this.PutStateObject();
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x00276BF0 File Offset: 0x00275FF0
		internal int ExecuteBatchRPCCommand()
		{
			this._SqlRPCBatchArray = this._RPCList.ToArray();
			this._currentlyExecutingBatch = 0;
			return this.ExecuteNonQuery();
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x00276C1C File Offset: 0x0027601C
		internal int? GetRecordsAffected(int commandIndex)
		{
			return this._SqlRPCBatchArray[commandIndex].recordsAffected;
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x00276C38 File Offset: 0x00276038
		internal SqlException GetErrors(int commandIndex)
		{
			SqlException ex = null;
			int num = this._SqlRPCBatchArray[commandIndex].errorsIndexEnd - this._SqlRPCBatchArray[commandIndex].errorsIndexStart;
			if (0 < num)
			{
				SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
				for (int i = this._SqlRPCBatchArray[commandIndex].errorsIndexStart; i < this._SqlRPCBatchArray[commandIndex].errorsIndexEnd; i++)
				{
					sqlErrorCollection.Add(this._SqlRPCBatchArray[commandIndex].errors[i]);
				}
				for (int j = this._SqlRPCBatchArray[commandIndex].warningsIndexStart; j < this._SqlRPCBatchArray[commandIndex].warningsIndexEnd; j++)
				{
					sqlErrorCollection.Add(this._SqlRPCBatchArray[commandIndex].warnings[j]);
				}
				ex = SqlException.CreateException(sqlErrorCollection, this.Connection.ServerVersion);
			}
			return ex;
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x00276D00 File Offset: 0x00276100
		private void DisposeSmiRequest()
		{
			if (this._smiRequest != null)
			{
				SmiRequestExecutor smiRequest = this._smiRequest;
				this._smiRequest = null;
				this._smiRequestContext = null;
				smiRequest.Close(this.EventSink);
				this.EventSink.ProcessMessagesAndThrow();
			}
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x00276D44 File Offset: 0x00276144
		private void SetUpSmiRequest(SqlInternalConnectionSmi innerConnection)
		{
			this.DisposeSmiRequest();
			if (this.Notification != null)
			{
				throw SQL.NotificationsNotAvailableOnContextConnection();
			}
			SmiParameterMetaData[] array = null;
			ParameterPeekAheadValue[] array2 = null;
			int parameterCount = this.GetParameterCount(this.Parameters);
			if (0 < parameterCount)
			{
				array = new SmiParameterMetaData[parameterCount];
				array2 = new ParameterPeekAheadValue[parameterCount];
				for (int i = 0; i < parameterCount; i++)
				{
					SqlParameter sqlParameter = this.Parameters[i];
					sqlParameter.Validate(i, CommandType.StoredProcedure == this.CommandType);
					array[i] = sqlParameter.MetaDataForSmi(out array2[i]);
					if (!innerConnection.IsKatmaiOrNewer)
					{
						MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(array[i].SqlDbType, array[i].IsMultiValued);
						if (!metaTypeFromSqlDbType.Is90Supported)
						{
							throw ADP.VersionDoesNotSupportDataType(metaTypeFromSqlDbType.TypeName);
						}
					}
				}
			}
			CommandType commandType = this.CommandType;
			this._smiRequestContext = innerConnection.InternalContext;
			this._smiRequest = this._smiRequestContext.CreateRequestExecutor(this.CommandText, commandType, array, this.EventSink);
			this.EventSink.ProcessMessagesAndThrow();
			for (int j = 0; j < parameterCount; j++)
			{
				if (ParameterDirection.Output != array[j].Direction && ParameterDirection.ReturnValue != array[j].Direction)
				{
					SqlParameter sqlParameter2 = this.Parameters[j];
					object coercedValue = sqlParameter2.GetCoercedValue();
					ExtendedClrTypeCode extendedClrTypeCode = MetaDataUtilsSmi.DetermineExtendedTypeCodeForUseWithSqlDbType(array[j].SqlDbType, array[j].IsMultiValued, coercedValue, null, SmiContextFactory.Instance.NegotiatedSmiVersion);
					if (CommandType.StoredProcedure == commandType && ExtendedClrTypeCode.Empty == extendedClrTypeCode)
					{
						this._smiRequest.SetDefault(j);
					}
					else
					{
						int size = sqlParameter2.Size;
						if (size != 0 && (long)size != -1L && !sqlParameter2.SizeInferred)
						{
							SqlDbType sqlDbType = array[j].SqlDbType;
							if (sqlDbType != SqlDbType.Image)
							{
								switch (sqlDbType)
								{
								case SqlDbType.NText:
									if (size != 1073741823)
									{
										throw SQL.ParameterSizeRestrictionFailure(j);
									}
									goto IL_02CF;
								case SqlDbType.NVarChar:
									if (size > 0 && size != 1073741823 && array[j].MaxLength == -1L)
									{
										throw SQL.ParameterSizeRestrictionFailure(j);
									}
									goto IL_02CF;
								case SqlDbType.Real:
								case SqlDbType.UniqueIdentifier:
								case SqlDbType.SmallDateTime:
								case SqlDbType.SmallInt:
								case SqlDbType.SmallMoney:
								case SqlDbType.TinyInt:
								case (SqlDbType)24:
									goto IL_02CF;
								case SqlDbType.Text:
									break;
								case SqlDbType.Timestamp:
									if ((long)size < SmiMetaData.DefaultTimestamp.MaxLength)
									{
										throw SQL.ParameterSizeRestrictionFailure(j);
									}
									goto IL_02CF;
								case SqlDbType.VarBinary:
								case SqlDbType.VarChar:
									if (size > 0 && size != 2147483647 && array[j].MaxLength == -1L)
									{
										throw SQL.ParameterSizeRestrictionFailure(j);
									}
									goto IL_02CF;
								case SqlDbType.Variant:
								{
									if (coercedValue == null)
									{
										goto IL_02CF;
									}
									MetaType metaTypeFromValue = MetaType.GetMetaTypeFromValue(coercedValue);
									if ((metaTypeFromValue.IsNCharType && (long)size < 4000L) || (metaTypeFromValue.IsBinType && (long)size < 8000L) || (metaTypeFromValue.IsAnsiType && (long)size < 8000L))
									{
										throw SQL.ParameterSizeRestrictionFailure(j);
									}
									goto IL_02CF;
								}
								case SqlDbType.Xml:
									if (coercedValue != null && ExtendedClrTypeCode.SqlXml != extendedClrTypeCode)
									{
										throw SQL.ParameterSizeRestrictionFailure(j);
									}
									goto IL_02CF;
								default:
									goto IL_02CF;
								}
							}
							if (size != 2147483647)
							{
								throw SQL.ParameterSizeRestrictionFailure(j);
							}
						}
						IL_02CF:
						if (innerConnection.IsKatmaiOrNewer)
						{
							ValueUtilsSmi.SetCompatibleValueV200(this.EventSink, this._smiRequest, j, array[j], coercedValue, extendedClrTypeCode, sqlParameter2.Offset, sqlParameter2.Size, array2[j]);
						}
						else
						{
							ValueUtilsSmi.SetCompatibleValue(this.EventSink, this._smiRequest, j, array[j], coercedValue, extendedClrTypeCode, sqlParameter2.Offset);
						}
					}
				}
			}
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x00277084 File Offset: 0x00276484
		// Note: this type is marked as 'beforefieldinit'.
		static SqlCommand()
		{
			string[] array = new string[15];
			array[0] = "PARAMETER_NAME";
			array[1] = "PARAMETER_TYPE";
			array[2] = "DATA_TYPE";
			array[4] = "CHARACTER_MAXIMUM_LENGTH";
			array[5] = "NUMERIC_PRECISION";
			array[6] = "NUMERIC_SCALE";
			array[7] = "UDT_CATALOG";
			array[8] = "UDT_SCHEMA";
			array[9] = "TYPE_NAME";
			array[10] = "XML_CATALOGNAME";
			array[11] = "XML_SCHEMANAME";
			array[12] = "XML_SCHEMACOLLECTIONNAME";
			array[13] = "UDT_NAME";
			SqlCommand.PreKatmaiProcParamsNames = array;
			SqlCommand.KatmaiProcParamsNames = new string[]
			{
				"PARAMETER_NAME", "PARAMETER_TYPE", null, "MANAGED_DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH", "NUMERIC_PRECISION", "NUMERIC_SCALE", "TYPE_CATALOG_NAME", "TYPE_SCHEMA_NAME", "TYPE_NAME",
				"XML_CATALOGNAME", "XML_SCHEMANAME", "XML_SCHEMACOLLECTIONNAME", null, "SS_DATETIME_PRECISION"
			};
		}

		// Token: 0x0400170F RID: 5903
		private static int _objectTypeCount;

		// Token: 0x04001710 RID: 5904
		internal readonly int ObjectID = Interlocked.Increment(ref SqlCommand._objectTypeCount);

		// Token: 0x04001711 RID: 5905
		private string _commandText;

		// Token: 0x04001712 RID: 5906
		private CommandType _commandType;

		// Token: 0x04001713 RID: 5907
		private int _commandTimeout = 30;

		// Token: 0x04001714 RID: 5908
		private UpdateRowSource _updatedRowSource = UpdateRowSource.Both;

		// Token: 0x04001715 RID: 5909
		private bool _designTimeInvisible;

		// Token: 0x04001716 RID: 5910
		internal SqlDependency _sqlDep;

		// Token: 0x04001717 RID: 5911
		private bool _inPrepare;

		// Token: 0x04001718 RID: 5912
		private int _prepareHandle = -1;

		// Token: 0x04001719 RID: 5913
		private bool _hiddenPrepare;

		// Token: 0x0400171A RID: 5914
		private SqlParameterCollection _parameters;

		// Token: 0x0400171B RID: 5915
		private SqlConnection _activeConnection;

		// Token: 0x0400171C RID: 5916
		private bool _dirty;

		// Token: 0x0400171D RID: 5917
		private SqlCommand.EXECTYPE _execType;

		// Token: 0x0400171E RID: 5918
		private _SqlRPC[] _rpcArrayOf1;

		// Token: 0x0400171F RID: 5919
		private _SqlMetaDataSet _cachedMetaData;

		// Token: 0x04001720 RID: 5920
		private SqlCommand.CachedAsyncState _cachedAsyncState;

		// Token: 0x04001721 RID: 5921
		internal int _rowsAffected = -1;

		// Token: 0x04001722 RID: 5922
		private SqlNotificationRequest _notification;

		// Token: 0x04001723 RID: 5923
		private bool _notificationAutoEnlist = true;

		// Token: 0x04001724 RID: 5924
		private SqlTransaction _transaction;

		// Token: 0x04001725 RID: 5925
		private StatementCompletedEventHandler _statementCompletedEventHandler;

		// Token: 0x04001726 RID: 5926
		private TdsParserStateObject _stateObj;

		// Token: 0x04001727 RID: 5927
		private volatile bool _pendingCancel;

		// Token: 0x04001728 RID: 5928
		private bool _batchRPCMode;

		// Token: 0x04001729 RID: 5929
		private List<_SqlRPC> _RPCList;

		// Token: 0x0400172A RID: 5930
		private _SqlRPC[] _SqlRPCBatchArray;

		// Token: 0x0400172B RID: 5931
		private List<SqlParameterCollection> _parameterCollectionList;

		// Token: 0x0400172C RID: 5932
		private int _currentlyExecutingBatch;

		// Token: 0x0400172D RID: 5933
		private SmiRequestExecutor _smiRequest;

		// Token: 0x0400172E RID: 5934
		private SmiContext _smiRequestContext;

		// Token: 0x0400172F RID: 5935
		private SqlCommand.CommandEventSink _smiEventSink;

		// Token: 0x04001730 RID: 5936
		private SmiEventSink_DeferedProcessing _outParamEventSink;

		// Token: 0x04001731 RID: 5937
		internal static readonly string[] PreKatmaiProcParamsNames;

		// Token: 0x04001732 RID: 5938
		internal static readonly string[] KatmaiProcParamsNames;

		// Token: 0x020002C5 RID: 709
		private enum EXECTYPE
		{
			// Token: 0x04001734 RID: 5940
			UNPREPARED,
			// Token: 0x04001735 RID: 5941
			PREPAREPENDING,
			// Token: 0x04001736 RID: 5942
			PREPARED
		}

		// Token: 0x020002C6 RID: 710
		private class CachedAsyncState
		{
			// Token: 0x06002448 RID: 9288 RVA: 0x00277188 File Offset: 0x00276588
			internal CachedAsyncState()
			{
			}

			// Token: 0x17000576 RID: 1398
			// (get) Token: 0x06002449 RID: 9289 RVA: 0x002771AC File Offset: 0x002765AC
			internal SqlDataReader CachedAsyncReader
			{
				get
				{
					return this._cachedAsyncReader;
				}
			}

			// Token: 0x17000577 RID: 1399
			// (get) Token: 0x0600244A RID: 9290 RVA: 0x002771C0 File Offset: 0x002765C0
			internal RunBehavior CachedRunBehavior
			{
				get
				{
					return this._cachedRunBehavior;
				}
			}

			// Token: 0x17000578 RID: 1400
			// (get) Token: 0x0600244B RID: 9291 RVA: 0x002771D4 File Offset: 0x002765D4
			internal string CachedSetOptions
			{
				get
				{
					return this._cachedSetOptions;
				}
			}

			// Token: 0x17000579 RID: 1401
			// (get) Token: 0x0600244C RID: 9292 RVA: 0x002771E8 File Offset: 0x002765E8
			internal bool PendingAsyncOperation
			{
				get
				{
					return null != this._cachedAsyncResult;
				}
			}

			// Token: 0x0600244D RID: 9293 RVA: 0x00277204 File Offset: 0x00276604
			internal bool IsActiveConnectionValid(SqlConnection activeConnection)
			{
				return this._cachedAsyncConnection == activeConnection && this._cachedAsyncCloseCount == activeConnection.CloseCount;
			}

			// Token: 0x0600244E RID: 9294 RVA: 0x0027722C File Offset: 0x0027662C
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			internal void ResetAsyncState()
			{
				this._cachedAsyncCloseCount = -1;
				this._cachedAsyncResult = null;
				if (this._cachedAsyncConnection != null)
				{
					this._cachedAsyncConnection.AsycCommandInProgress = false;
					this._cachedAsyncConnection = null;
				}
				this._cachedAsyncReader = null;
				this._cachedRunBehavior = RunBehavior.ReturnImmediately;
				this._cachedSetOptions = null;
			}

			// Token: 0x0600244F RID: 9295 RVA: 0x00277278 File Offset: 0x00276678
			internal void SetActiveConnectionAndResult(DbAsyncResult result, SqlConnection activeConnection)
			{
				this._cachedAsyncCloseCount = activeConnection.CloseCount;
				this._cachedAsyncResult = result;
				if (activeConnection != null && !activeConnection.Parser.MARSOn && activeConnection.AsycCommandInProgress)
				{
					throw SQL.MARSUnspportedOnConnection();
				}
				this._cachedAsyncConnection = activeConnection;
				this._cachedAsyncConnection.AsycCommandInProgress = true;
			}

			// Token: 0x06002450 RID: 9296 RVA: 0x002772CC File Offset: 0x002766CC
			internal void SetAsyncReaderState(SqlDataReader ds, RunBehavior runBehavior, string optionSettings)
			{
				this._cachedAsyncReader = ds;
				this._cachedRunBehavior = runBehavior;
				this._cachedSetOptions = optionSettings;
			}

			// Token: 0x04001737 RID: 5943
			private int _cachedAsyncCloseCount = -1;

			// Token: 0x04001738 RID: 5944
			private DbAsyncResult _cachedAsyncResult;

			// Token: 0x04001739 RID: 5945
			private SqlConnection _cachedAsyncConnection;

			// Token: 0x0400173A RID: 5946
			private SqlDataReader _cachedAsyncReader;

			// Token: 0x0400173B RID: 5947
			private RunBehavior _cachedRunBehavior = RunBehavior.ReturnImmediately;

			// Token: 0x0400173C RID: 5948
			private string _cachedSetOptions;
		}

		// Token: 0x020002C7 RID: 711
		private sealed class CommandEventSink : SmiEventSink_Default
		{
			// Token: 0x06002451 RID: 9297 RVA: 0x002772F0 File Offset: 0x002766F0
			internal CommandEventSink(SqlCommand command)
			{
				this._command = command;
			}

			// Token: 0x06002452 RID: 9298 RVA: 0x0027730C File Offset: 0x0027670C
			internal override void StatementCompleted(int rowsAffected)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlCommand.CommandEventSink.StatementCompleted|ADV> %d#, rowsAffected=%d.\n", this._command.ObjectID, rowsAffected);
				}
				this._command.InternalRecordsAffected = rowsAffected;
			}

			// Token: 0x06002453 RID: 9299 RVA: 0x00277344 File Offset: 0x00276744
			internal override void BatchCompleted()
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlCommand.CommandEventSink.BatchCompleted|ADV> %d#.\n", this._command.ObjectID);
				}
			}

			// Token: 0x06002454 RID: 9300 RVA: 0x00277370 File Offset: 0x00276770
			internal override void ParametersAvailable(SmiParameterMetaData[] metaData, ITypedGettersV3 parameterValues)
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.SqlCommand.CommandEventSink.ParametersAvailable|ADV> %d# metaData.Length=%d.\n", this._command.ObjectID, (metaData != null) ? metaData.Length : (-1));
					if (metaData != null)
					{
						for (int i = 0; i < metaData.Length; i++)
						{
							Bid.Trace("<sc.SqlCommand.CommandEventSink.ParametersAvailable|ADV> %d#, metaData[%d] is %s%s\n", this._command.ObjectID, i, metaData[i].GetType().ToString(), metaData[i].TraceString());
						}
					}
				}
				this._command.OnParametersAvailableSmi(metaData, parameterValues);
			}

			// Token: 0x06002455 RID: 9301 RVA: 0x002773EC File Offset: 0x002767EC
			internal override void ParameterAvailable(SmiParameterMetaData metaData, SmiTypedGetterSetter parameterValues, int ordinal)
			{
				if (Bid.AdvancedOn && metaData != null)
				{
					Bid.Trace("<sc.SqlCommand.CommandEventSink.ParameterAvailable|ADV> %d#, metaData[%d] is %s%s\n", this._command.ObjectID, ordinal, metaData.GetType().ToString(), metaData.TraceString());
				}
				this._command.OnParameterAvailableSmi(metaData, parameterValues, ordinal);
			}

			// Token: 0x0400173D RID: 5949
			private SqlCommand _command;
		}
	}
}
