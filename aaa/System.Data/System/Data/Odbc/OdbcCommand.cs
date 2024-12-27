using System;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Data.Odbc
{
	// Token: 0x020001D3 RID: 467
	[Designer("Microsoft.VSDesigner.Data.VS.OdbcCommandDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("RecordsAffected")]
	[ToolboxItem(true)]
	public sealed class OdbcCommand : DbCommand, ICloneable
	{
		// Token: 0x06001978 RID: 6520 RVA: 0x0023F5EC File Offset: 0x0023E9EC
		public OdbcCommand()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x0023F624 File Offset: 0x0023EA24
		public OdbcCommand(string cmdText)
			: this()
		{
			this.CommandText = cmdText;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0023F640 File Offset: 0x0023EA40
		public OdbcCommand(string cmdText, OdbcConnection connection)
			: this()
		{
			this.CommandText = cmdText;
			this.Connection = connection;
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x0023F664 File Offset: 0x0023EA64
		public OdbcCommand(string cmdText, OdbcConnection connection, OdbcTransaction transaction)
			: this()
		{
			this.CommandText = cmdText;
			this.Connection = connection;
			this.Transaction = transaction;
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x0023F68C File Offset: 0x0023EA8C
		private void DisposeDeadDataReader()
		{
			if (ConnectionState.Fetching == this.cmdState && this.weakDataReaderReference != null && !this.weakDataReaderReference.IsAlive)
			{
				if (this._cmdWrapper != null)
				{
					this._cmdWrapper.FreeKeyInfoStatementHandle(ODBC32.STMT.CLOSE);
					this._cmdWrapper.FreeStatementHandle(ODBC32.STMT.CLOSE);
				}
				this.CloseFromDataReader();
			}
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x0023F6E0 File Offset: 0x0023EAE0
		private void DisposeDataReader()
		{
			if (this.weakDataReaderReference != null)
			{
				IDisposable disposable = (IDisposable)this.weakDataReaderReference.Target;
				if (disposable != null && this.weakDataReaderReference.IsAlive)
				{
					disposable.Dispose();
				}
				this.CloseFromDataReader();
			}
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x0023F724 File Offset: 0x0023EB24
		internal void DisconnectFromDataReaderAndConnection()
		{
			OdbcDataReader odbcDataReader = null;
			if (this.weakDataReaderReference != null)
			{
				OdbcDataReader odbcDataReader2 = (OdbcDataReader)this.weakDataReaderReference.Target;
				if (this.weakDataReaderReference.IsAlive)
				{
					odbcDataReader = odbcDataReader2;
				}
			}
			if (odbcDataReader != null)
			{
				odbcDataReader.Command = null;
			}
			this._transaction = null;
			if (this._connection != null)
			{
				this._connection.RemoveWeakReference(this);
				this._connection = null;
			}
			if (odbcDataReader == null)
			{
				this.CloseCommandWrapper();
			}
			this._cmdWrapper = null;
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x0023F798 File Offset: 0x0023EB98
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.DisconnectFromDataReaderAndConnection();
				this._parameterCollection = null;
				this.CommandText = null;
			}
			this._cmdWrapper = null;
			this._isPrepared = false;
			base.Dispose(disposing);
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x0023F7D4 File Offset: 0x0023EBD4
		internal bool Canceling
		{
			get
			{
				return this._cmdWrapper.Canceling;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001981 RID: 6529 RVA: 0x0023F7EC File Offset: 0x0023EBEC
		// (set) Token: 0x06001982 RID: 6530 RVA: 0x0023F80C File Offset: 0x0023EC0C
		[ResDescription("DbCommand_CommandText")]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.All)]
		[Editor("Microsoft.VSDesigner.Data.Odbc.Design.OdbcCommandTextEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
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
					Bid.Trace("<odbc.OdbcCommand.set_CommandText|API> %d#, '", this.ObjectID);
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

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001983 RID: 6531 RVA: 0x0023F85C File Offset: 0x0023EC5C
		// (set) Token: 0x06001984 RID: 6532 RVA: 0x0023F870 File Offset: 0x0023EC70
		[ResDescription("DbCommand_CommandTimeout")]
		[ResCategory("DataCategory_Data")]
		public override int CommandTimeout
		{
			get
			{
				return this._commandTimeout;
			}
			set
			{
				Bid.Trace("<odbc.OdbcCommand.set_CommandTimeout|API> %d#, %d\n", this.ObjectID, value);
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

		// Token: 0x06001985 RID: 6533 RVA: 0x0023F8B0 File Offset: 0x0023ECB0
		public void ResetCommandTimeout()
		{
			if (30 != this._commandTimeout)
			{
				this.PropertyChanging();
				this._commandTimeout = 30;
			}
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x0023F8D8 File Offset: 0x0023ECD8
		private bool ShouldSerializeCommandTimeout()
		{
			return 30 != this._commandTimeout;
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001987 RID: 6535 RVA: 0x0023F8F4 File Offset: 0x0023ECF4
		// (set) Token: 0x06001988 RID: 6536 RVA: 0x0023F910 File Offset: 0x0023ED10
		[DefaultValue(CommandType.Text)]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbCommand_CommandType")]
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
				throw ODBC.NotSupportedCommandType(value);
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06001989 RID: 6537 RVA: 0x0023F94C File Offset: 0x0023ED4C
		// (set) Token: 0x0600198A RID: 6538 RVA: 0x0023F960 File Offset: 0x0023ED60
		[ResDescription("DbCommand_Connection")]
		[DefaultValue(null)]
		[ResCategory("DataCategory_Behavior")]
		[Editor("Microsoft.VSDesigner.Data.Design.DbConnectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public new OdbcConnection Connection
		{
			get
			{
				return this._connection;
			}
			set
			{
				if (value != this._connection)
				{
					this.PropertyChanging();
					this.DisconnectFromDataReaderAndConnection();
					this._connection = value;
				}
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x0023F98C File Offset: 0x0023ED8C
		// (set) Token: 0x0600198C RID: 6540 RVA: 0x0023F9A0 File Offset: 0x0023EDA0
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
			set
			{
				this.Connection = (OdbcConnection)value;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600198D RID: 6541 RVA: 0x0023F9BC File Offset: 0x0023EDBC
		protected override DbParameterCollection DbParameterCollection
		{
			get
			{
				return this.Parameters;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600198E RID: 6542 RVA: 0x0023F9D0 File Offset: 0x0023EDD0
		// (set) Token: 0x0600198F RID: 6543 RVA: 0x0023F9E4 File Offset: 0x0023EDE4
		protected override DbTransaction DbTransaction
		{
			get
			{
				return this.Transaction;
			}
			set
			{
				this.Transaction = (OdbcTransaction)value;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001990 RID: 6544 RVA: 0x0023FA00 File Offset: 0x0023EE00
		// (set) Token: 0x06001991 RID: 6545 RVA: 0x0023FA18 File Offset: 0x0023EE18
		[DesignOnly(true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06001992 RID: 6546 RVA: 0x0023FA38 File Offset: 0x0023EE38
		internal bool HasParameters
		{
			get
			{
				return null != this._parameterCollection;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x0023FA54 File Offset: 0x0023EE54
		[ResCategory("DataCategory_Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[ResDescription("DbCommand_Parameters")]
		public new OdbcParameterCollection Parameters
		{
			get
			{
				if (this._parameterCollection == null)
				{
					this._parameterCollection = new OdbcParameterCollection();
				}
				return this._parameterCollection;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06001994 RID: 6548 RVA: 0x0023FA7C File Offset: 0x0023EE7C
		// (set) Token: 0x06001995 RID: 6549 RVA: 0x0023FAAC File Offset: 0x0023EEAC
		[ResDescription("DbCommand_Transaction")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new OdbcTransaction Transaction
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
				if (this._transaction != value)
				{
					this.PropertyChanging();
					this._transaction = value;
				}
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001996 RID: 6550 RVA: 0x0023FAD0 File Offset: 0x0023EED0
		// (set) Token: 0x06001997 RID: 6551 RVA: 0x0023FAE4 File Offset: 0x0023EEE4
		[ResDescription("DbCommand_UpdatedRowSource")]
		[ResCategory("DataCategory_Update")]
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

		// Token: 0x06001998 RID: 6552 RVA: 0x0023FB1C File Offset: 0x0023EF1C
		internal OdbcDescriptorHandle GetDescriptorHandle(ODBC32.SQL_ATTR attribute)
		{
			return this._cmdWrapper.GetDescriptorHandle(attribute);
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0023FB38 File Offset: 0x0023EF38
		internal CMDWrapper GetStatementHandle()
		{
			if (this._cmdWrapper == null)
			{
				this._cmdWrapper = new CMDWrapper(this._connection);
				this._connection.AddWeakReference(this, 1);
			}
			if (this._cmdWrapper._dataReaderBuf == null)
			{
				this._cmdWrapper._dataReaderBuf = new CNativeBuffer(4096);
			}
			if (this._cmdWrapper.StatementHandle == null)
			{
				this._isPrepared = false;
				this._cmdWrapper.CreateStatementHandle();
			}
			else if (this._parameterCollection != null && this._parameterCollection.RebindCollection)
			{
				this._cmdWrapper.FreeStatementHandle(ODBC32.STMT.RESET_PARAMS);
			}
			return this._cmdWrapper;
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0023FBD8 File Offset: 0x0023EFD8
		public override void Cancel()
		{
			CMDWrapper cmdWrapper = this._cmdWrapper;
			if (cmdWrapper != null)
			{
				cmdWrapper.Canceling = true;
				OdbcStatementHandle statementHandle = cmdWrapper.StatementHandle;
				if (statementHandle != null)
				{
					lock (statementHandle)
					{
						ODBC32.RetCode retCode = statementHandle.Cancel();
						switch (retCode)
						{
						case ODBC32.RetCode.SUCCESS:
						case ODBC32.RetCode.SUCCESS_WITH_INFO:
							break;
						default:
							throw cmdWrapper.Connection.HandleErrorNoThrow(statementHandle, retCode);
						}
					}
				}
			}
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0023FC54 File Offset: 0x0023F054
		object ICloneable.Clone()
		{
			OdbcCommand odbcCommand = new OdbcCommand();
			Bid.Trace("<odbc.OdbcCommand.Clone|API> %d#, clone=%d#\n", this.ObjectID, odbcCommand.ObjectID);
			odbcCommand.CommandText = this.CommandText;
			odbcCommand.CommandTimeout = this.CommandTimeout;
			odbcCommand.CommandType = this.CommandType;
			odbcCommand.Connection = this.Connection;
			odbcCommand.Transaction = this.Transaction;
			odbcCommand.UpdatedRowSource = this.UpdatedRowSource;
			if (this._parameterCollection != null && 0 < this.Parameters.Count)
			{
				OdbcParameterCollection parameters = odbcCommand.Parameters;
				foreach (object obj in this.Parameters)
				{
					ICloneable cloneable = (ICloneable)obj;
					parameters.Add(cloneable.Clone());
				}
			}
			return odbcCommand;
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0023FD44 File Offset: 0x0023F144
		internal bool RecoverFromConnection()
		{
			this.DisposeDeadDataReader();
			return ConnectionState.Closed == this.cmdState;
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0023FD60 File Offset: 0x0023F160
		private void CloseCommandWrapper()
		{
			CMDWrapper cmdWrapper = this._cmdWrapper;
			if (cmdWrapper != null)
			{
				try
				{
					cmdWrapper.Dispose();
					if (this._connection != null)
					{
						this._connection.RemoveWeakReference(this);
					}
				}
				finally
				{
					this._cmdWrapper = null;
				}
			}
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0023FDB8 File Offset: 0x0023F1B8
		internal void CloseFromConnection()
		{
			if (this._parameterCollection != null)
			{
				this._parameterCollection.RebindCollection = true;
			}
			this.DisposeDataReader();
			this.CloseCommandWrapper();
			this._isPrepared = false;
			this._transaction = null;
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0023FDF4 File Offset: 0x0023F1F4
		internal void CloseFromDataReader()
		{
			this.weakDataReaderReference = null;
			this.cmdState = ConnectionState.Closed;
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0023FE10 File Offset: 0x0023F210
		public new OdbcParameter CreateParameter()
		{
			return new OdbcParameter();
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0023FE24 File Offset: 0x0023F224
		protected override DbParameter CreateDbParameter()
		{
			return this.CreateParameter();
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0023FE38 File Offset: 0x0023F238
		protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
		{
			return this.ExecuteReader(behavior);
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0023FE4C File Offset: 0x0023F24C
		public override int ExecuteNonQuery()
		{
			OdbcConnection.ExecutePermission.Demand();
			int recordsAffected;
			using (OdbcDataReader odbcDataReader = this.ExecuteReaderObject(CommandBehavior.Default, "ExecuteNonQuery", false))
			{
				odbcDataReader.Close();
				recordsAffected = odbcDataReader.RecordsAffected;
			}
			return recordsAffected;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0023FEA8 File Offset: 0x0023F2A8
		public new OdbcDataReader ExecuteReader()
		{
			return this.ExecuteReader(CommandBehavior.Default);
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0023FEBC File Offset: 0x0023F2BC
		public new OdbcDataReader ExecuteReader(CommandBehavior behavior)
		{
			OdbcConnection.ExecutePermission.Demand();
			return this.ExecuteReaderObject(behavior, "ExecuteReader", true);
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0023FEE0 File Offset: 0x0023F2E0
		internal OdbcDataReader ExecuteReaderFromSQLMethod(object[] methodArguments, ODBC32.SQL_API method)
		{
			return this.ExecuteReaderObject(CommandBehavior.Default, method.ToString(), true, methodArguments, method);
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0023FF04 File Offset: 0x0023F304
		private OdbcDataReader ExecuteReaderObject(CommandBehavior behavior, string method, bool needReader)
		{
			if (this.CommandText == null || this.CommandText.Length == 0)
			{
				throw ADP.CommandTextRequired(method);
			}
			return this.ExecuteReaderObject(behavior, method, needReader, null, ODBC32.SQL_API.SQLEXECDIRECT);
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0023FF3C File Offset: 0x0023F33C
		private OdbcDataReader ExecuteReaderObject(CommandBehavior behavior, string method, bool needReader, object[] methodArguments, ODBC32.SQL_API odbcApiMethod)
		{
			OdbcDataReader odbcDataReader = null;
			try
			{
				this.DisposeDeadDataReader();
				this.ValidateConnectionAndTransaction(method);
				if ((CommandBehavior.SingleRow & behavior) != CommandBehavior.Default)
				{
					behavior |= CommandBehavior.SingleResult;
				}
				OdbcStatementHandle statementHandle = this.GetStatementHandle().StatementHandle;
				this._cmdWrapper.Canceling = false;
				if (this.weakDataReaderReference != null && this.weakDataReaderReference.IsAlive)
				{
					object target = this.weakDataReaderReference.Target;
					if (target != null && this.weakDataReaderReference.IsAlive && !((OdbcDataReader)target).IsClosed)
					{
						throw ADP.OpenReaderExists();
					}
				}
				odbcDataReader = new OdbcDataReader(this, this._cmdWrapper, behavior);
				if (!this.Connection.ProviderInfo.NoQueryTimeout)
				{
					this.TrySetStatementAttribute(statementHandle, ODBC32.SQL_ATTR.QUERY_TIMEOUT, (IntPtr)this.CommandTimeout);
				}
				if (needReader && this.Connection.IsV3Driver && !this.Connection.ProviderInfo.NoSqlSoptSSNoBrowseTable && !this.Connection.ProviderInfo.NoSqlSoptSSHiddenColumns)
				{
					if (odbcDataReader.IsBehavior(CommandBehavior.KeyInfo))
					{
						if (!this._cmdWrapper._ssKeyInfoModeOn)
						{
							this.TrySetStatementAttribute(statementHandle, (ODBC32.SQL_ATTR)1228, (IntPtr)1L);
							this.TrySetStatementAttribute(statementHandle, (ODBC32.SQL_ATTR)1227, (IntPtr)1L);
							this._cmdWrapper._ssKeyInfoModeOff = false;
							this._cmdWrapper._ssKeyInfoModeOn = true;
						}
					}
					else if (!this._cmdWrapper._ssKeyInfoModeOff)
					{
						this.TrySetStatementAttribute(statementHandle, (ODBC32.SQL_ATTR)1228, (IntPtr)0L);
						this.TrySetStatementAttribute(statementHandle, (ODBC32.SQL_ATTR)1227, (IntPtr)0L);
						this._cmdWrapper._ssKeyInfoModeOff = true;
						this._cmdWrapper._ssKeyInfoModeOn = false;
					}
				}
				if (odbcDataReader.IsBehavior(CommandBehavior.KeyInfo) || odbcDataReader.IsBehavior(CommandBehavior.SchemaOnly))
				{
					ODBC32.RetCode retCode = statementHandle.Prepare(this.CommandText);
					if (retCode != ODBC32.RetCode.SUCCESS)
					{
						this._connection.HandleError(statementHandle, retCode);
					}
				}
				bool flag = false;
				CNativeBuffer cnativeBuffer = this._cmdWrapper._nativeParameterBuffer;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					if (this._parameterCollection != null && 0 < this._parameterCollection.Count)
					{
						int num = this._parameterCollection.CalcParameterBufferSize(this);
						if (cnativeBuffer == null || cnativeBuffer.Length < num)
						{
							if (cnativeBuffer != null)
							{
								cnativeBuffer.Dispose();
							}
							cnativeBuffer = new CNativeBuffer(num);
							this._cmdWrapper._nativeParameterBuffer = cnativeBuffer;
						}
						else
						{
							cnativeBuffer.ZeroMemory();
						}
						cnativeBuffer.DangerousAddRef(ref flag);
						this._parameterCollection.Bind(this, this._cmdWrapper, cnativeBuffer);
					}
					if (!odbcDataReader.IsBehavior(CommandBehavior.SchemaOnly))
					{
						ODBC32.RetCode retCode;
						if ((odbcDataReader.IsBehavior(CommandBehavior.KeyInfo) || odbcDataReader.IsBehavior(CommandBehavior.SchemaOnly)) && this.CommandType != CommandType.StoredProcedure)
						{
							short num2;
							retCode = statementHandle.NumberOfResultColumns(out num2);
							if (retCode == ODBC32.RetCode.SUCCESS || retCode == ODBC32.RetCode.SUCCESS_WITH_INFO)
							{
								if (num2 > 0)
								{
									odbcDataReader.GetSchemaTable();
								}
							}
							else if (retCode != ODBC32.RetCode.NO_DATA)
							{
								this._connection.HandleError(statementHandle, retCode);
							}
						}
						if (odbcApiMethod <= ODBC32.SQL_API.SQLCOLUMNS)
						{
							if (odbcApiMethod != ODBC32.SQL_API.SQLEXECDIRECT)
							{
								if (odbcApiMethod == ODBC32.SQL_API.SQLCOLUMNS)
								{
									retCode = statementHandle.Columns((string)methodArguments[0], (string)methodArguments[1], (string)methodArguments[2], (string)methodArguments[3]);
									goto IL_0421;
								}
							}
							else
							{
								if (odbcDataReader.IsBehavior(CommandBehavior.KeyInfo) || this._isPrepared)
								{
									retCode = statementHandle.Execute();
									goto IL_0421;
								}
								retCode = statementHandle.ExecuteDirect(this.CommandText);
								goto IL_0421;
							}
						}
						else
						{
							if (odbcApiMethod == ODBC32.SQL_API.SQLGETTYPEINFO)
							{
								retCode = statementHandle.GetTypeInfo((short)methodArguments[0]);
								goto IL_0421;
							}
							switch (odbcApiMethod)
							{
							case ODBC32.SQL_API.SQLSTATISTICS:
								retCode = statementHandle.Statistics((string)methodArguments[0], (string)methodArguments[1], (string)methodArguments[2], (short)methodArguments[3], (short)methodArguments[4]);
								goto IL_0421;
							case ODBC32.SQL_API.SQLTABLES:
								retCode = statementHandle.Tables((string)methodArguments[0], (string)methodArguments[1], (string)methodArguments[2], (string)methodArguments[3]);
								goto IL_0421;
							default:
								switch (odbcApiMethod)
								{
								case ODBC32.SQL_API.SQLPROCEDURECOLUMNS:
									retCode = statementHandle.ProcedureColumns((string)methodArguments[0], (string)methodArguments[1], (string)methodArguments[2], (string)methodArguments[3]);
									goto IL_0421;
								case ODBC32.SQL_API.SQLPROCEDURES:
									retCode = statementHandle.Procedures((string)methodArguments[0], (string)methodArguments[1], (string)methodArguments[2]);
									goto IL_0421;
								}
								break;
							}
						}
						throw ADP.InvalidOperation(method.ToString());
						IL_0421:
						if (retCode != ODBC32.RetCode.SUCCESS && ODBC32.RetCode.NO_DATA != retCode)
						{
							this._connection.HandleError(statementHandle, retCode);
						}
					}
				}
				finally
				{
					if (flag)
					{
						cnativeBuffer.DangerousRelease();
					}
				}
				this.weakDataReaderReference = new WeakReference(odbcDataReader);
				if (!odbcDataReader.IsBehavior(CommandBehavior.SchemaOnly))
				{
					odbcDataReader.FirstResult();
				}
				this.cmdState = ConnectionState.Fetching;
			}
			finally
			{
				if (ConnectionState.Fetching != this.cmdState)
				{
					if (odbcDataReader != null)
					{
						if (this._parameterCollection != null)
						{
							this._parameterCollection.ClearBindings();
						}
						((IDisposable)odbcDataReader).Dispose();
					}
					if (this.cmdState != ConnectionState.Closed)
					{
						this.cmdState = ConnectionState.Closed;
					}
				}
			}
			return odbcDataReader;
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0024041C File Offset: 0x0023F81C
		public override object ExecuteScalar()
		{
			OdbcConnection.ExecutePermission.Demand();
			object obj = null;
			using (IDataReader dataReader = this.ExecuteReaderObject(CommandBehavior.Default, "ExecuteScalar", false))
			{
				if (dataReader.Read() && 0 < dataReader.FieldCount)
				{
					obj = dataReader.GetValue(0);
				}
				dataReader.Close();
			}
			return obj;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0024048C File Offset: 0x0023F88C
		internal string GetDiagSqlState()
		{
			return this._cmdWrapper.GetDiagSqlState();
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x002404A4 File Offset: 0x0023F8A4
		private void PropertyChanging()
		{
			this._isPrepared = false;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x002404B8 File Offset: 0x0023F8B8
		public override void Prepare()
		{
			OdbcConnection.ExecutePermission.Demand();
			this.ValidateOpenConnection("Prepare");
			if ((ConnectionState.Fetching & this._connection.InternalState) != ConnectionState.Closed)
			{
				throw ADP.OpenReaderExists();
			}
			if (this.CommandType == CommandType.TableDirect)
			{
				return;
			}
			this.DisposeDeadDataReader();
			this.GetStatementHandle();
			OdbcStatementHandle statementHandle = this._cmdWrapper.StatementHandle;
			ODBC32.RetCode retCode = statementHandle.Prepare(this.CommandText);
			if (retCode != ODBC32.RetCode.SUCCESS)
			{
				this._connection.HandleError(statementHandle, retCode);
			}
			this._isPrepared = true;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0024053C File Offset: 0x0023F93C
		private void TrySetStatementAttribute(OdbcStatementHandle stmt, ODBC32.SQL_ATTR stmtAttribute, IntPtr value)
		{
			ODBC32.RetCode retCode = stmt.SetStatementAttribute(stmtAttribute, value, ODBC32.SQL_IS.UINTEGER);
			if (retCode == ODBC32.RetCode.ERROR)
			{
				string text;
				stmt.GetDiagnosticField(out text);
				if (text == "HYC00" || text == "HY092")
				{
					this.Connection.FlagUnsupportedStmtAttr(stmtAttribute);
				}
			}
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x00240588 File Offset: 0x0023F988
		private void ValidateOpenConnection(string methodName)
		{
			OdbcConnection connection = this.Connection;
			if (connection == null)
			{
				throw ADP.ConnectionRequired(methodName);
			}
			ConnectionState state = connection.State;
			if (ConnectionState.Open != state)
			{
				throw ADP.OpenConnectionRequired(methodName, state);
			}
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x002405BC File Offset: 0x0023F9BC
		private void ValidateConnectionAndTransaction(string method)
		{
			if (this._connection == null)
			{
				throw ADP.ConnectionRequired(method);
			}
			this._transaction = this._connection.SetStateExecuting(method, this.Transaction);
			this.cmdState = ConnectionState.Executing;
		}

		// Token: 0x04000F6A RID: 3946
		private static int _objectTypeCount;

		// Token: 0x04000F6B RID: 3947
		internal readonly int ObjectID = Interlocked.Increment(ref OdbcCommand._objectTypeCount);

		// Token: 0x04000F6C RID: 3948
		private string _commandText;

		// Token: 0x04000F6D RID: 3949
		private CommandType _commandType;

		// Token: 0x04000F6E RID: 3950
		private int _commandTimeout = 30;

		// Token: 0x04000F6F RID: 3951
		private UpdateRowSource _updatedRowSource = UpdateRowSource.Both;

		// Token: 0x04000F70 RID: 3952
		private bool _designTimeInvisible;

		// Token: 0x04000F71 RID: 3953
		private bool _isPrepared;

		// Token: 0x04000F72 RID: 3954
		private OdbcConnection _connection;

		// Token: 0x04000F73 RID: 3955
		private OdbcTransaction _transaction;

		// Token: 0x04000F74 RID: 3956
		private WeakReference weakDataReaderReference;

		// Token: 0x04000F75 RID: 3957
		private CMDWrapper _cmdWrapper;

		// Token: 0x04000F76 RID: 3958
		private OdbcParameterCollection _parameterCollection;

		// Token: 0x04000F77 RID: 3959
		private ConnectionState cmdState;
	}
}
