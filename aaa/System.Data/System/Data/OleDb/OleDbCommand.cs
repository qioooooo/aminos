using System;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.Data.OleDb
{
	// Token: 0x02000212 RID: 530
	[DefaultEvent("RecordsAffected")]
	[Designer("Microsoft.VSDesigner.Data.VS.OleDbCommandDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxItem(true)]
	public sealed class OleDbCommand : DbCommand, ICloneable, IDbCommand, IDisposable
	{
		// Token: 0x06001DC4 RID: 7620 RVA: 0x00254080 File Offset: 0x00253480
		public OleDbCommand()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x002540B8 File Offset: 0x002534B8
		public OleDbCommand(string cmdText)
			: this()
		{
			this.CommandText = cmdText;
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x002540D4 File Offset: 0x002534D4
		public OleDbCommand(string cmdText, OleDbConnection connection)
			: this()
		{
			this.CommandText = cmdText;
			this.Connection = connection;
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x002540F8 File Offset: 0x002534F8
		public OleDbCommand(string cmdText, OleDbConnection connection, OleDbTransaction transaction)
			: this()
		{
			this.CommandText = cmdText;
			this.Connection = connection;
			this.Transaction = transaction;
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00254120 File Offset: 0x00253520
		private OleDbCommand(OleDbCommand from)
			: this()
		{
			this.CommandText = from.CommandText;
			this.CommandTimeout = from.CommandTimeout;
			this.CommandType = from.CommandType;
			this.Connection = from.Connection;
			this.DesignTimeVisible = from.DesignTimeVisible;
			this.UpdatedRowSource = from.UpdatedRowSource;
			this.Transaction = from.Transaction;
			OleDbParameterCollection parameters = this.Parameters;
			foreach (object obj in from.Parameters)
			{
				parameters.Add((obj is ICloneable) ? (obj as ICloneable).Clone() : obj);
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001DC9 RID: 7625 RVA: 0x002541F8 File Offset: 0x002535F8
		// (set) Token: 0x06001DCA RID: 7626 RVA: 0x0025420C File Offset: 0x0025360C
		private Bindings ParameterBindings
		{
			get
			{
				return this._dbBindings;
			}
			set
			{
				Bindings dbBindings = this._dbBindings;
				this._dbBindings = value;
				if (dbBindings != null && value != dbBindings)
				{
					dbBindings.Dispose();
				}
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001DCB RID: 7627 RVA: 0x00254234 File Offset: 0x00253634
		// (set) Token: 0x06001DCC RID: 7628 RVA: 0x00254254 File Offset: 0x00253654
		[ResDescription("DbCommand_CommandText")]
		[Editor("Microsoft.VSDesigner.Data.ADO.Design.OleDbCommandTextEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.All)]
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
					Bid.Trace("<oledb.OleDbCommand.set_CommandText|API> %d#, '", this.ObjectID);
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

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001DCD RID: 7629 RVA: 0x002542A4 File Offset: 0x002536A4
		// (set) Token: 0x06001DCE RID: 7630 RVA: 0x002542B8 File Offset: 0x002536B8
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
				Bid.Trace("<oledb.OleDbCommand.set_CommandTimeout|API> %d#, %d\n", this.ObjectID, value);
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

		// Token: 0x06001DCF RID: 7631 RVA: 0x002542F8 File Offset: 0x002536F8
		public void ResetCommandTimeout()
		{
			if (30 != this._commandTimeout)
			{
				this.PropertyChanging();
				this._commandTimeout = 30;
			}
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x00254320 File Offset: 0x00253720
		private bool ShouldSerializeCommandTimeout()
		{
			return 30 != this._commandTimeout;
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001DD1 RID: 7633 RVA: 0x0025433C File Offset: 0x0025373C
		// (set) Token: 0x06001DD2 RID: 7634 RVA: 0x00254358 File Offset: 0x00253758
		[ResDescription("DbCommand_CommandType")]
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(CommandType.Text)]
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
				if (value == CommandType.Text || value == CommandType.StoredProcedure || value == CommandType.TableDirect)
				{
					this.PropertyChanging();
					this._commandType = value;
					return;
				}
				throw ADP.InvalidCommandType(value);
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x0025438C File Offset: 0x0025378C
		// (set) Token: 0x06001DD4 RID: 7636 RVA: 0x002543A0 File Offset: 0x002537A0
		[Editor("Microsoft.VSDesigner.Data.Design.DbConnectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResDescription("DbCommand_Connection")]
		[ResCategory("DataCategory_Data")]
		[DefaultValue(null)]
		public new OleDbConnection Connection
		{
			get
			{
				return this._connection;
			}
			set
			{
				OleDbConnection connection = this._connection;
				if (value != connection)
				{
					this.PropertyChanging();
					this.ResetConnection();
					this._connection = value;
					Bid.Trace("<oledb.OleDbCommand.set_Connection|API> %d#\n", this.ObjectID);
					if (value != null)
					{
						this._transaction = OleDbTransaction.TransactionUpdate(this._transaction);
					}
				}
			}
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x002543F0 File Offset: 0x002537F0
		private void ResetConnection()
		{
			OleDbConnection connection = this._connection;
			if (connection != null)
			{
				this.PropertyChanging();
				this.CloseInternal();
				if (this._trackingForClose)
				{
					connection.RemoveWeakReference(this);
					this._trackingForClose = false;
				}
			}
			this._connection = null;
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x00254430 File Offset: 0x00253830
		// (set) Token: 0x06001DD7 RID: 7639 RVA: 0x00254444 File Offset: 0x00253844
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
			set
			{
				this.Connection = (OleDbConnection)value;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x00254460 File Offset: 0x00253860
		protected override DbParameterCollection DbParameterCollection
		{
			get
			{
				return this.Parameters;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001DD9 RID: 7641 RVA: 0x00254474 File Offset: 0x00253874
		// (set) Token: 0x06001DDA RID: 7642 RVA: 0x00254488 File Offset: 0x00253888
		protected override DbTransaction DbTransaction
		{
			get
			{
				return this.Transaction;
			}
			set
			{
				this.Transaction = (OleDbTransaction)value;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001DDB RID: 7643 RVA: 0x002544A4 File Offset: 0x002538A4
		// (set) Token: 0x06001DDC RID: 7644 RVA: 0x002544BC File Offset: 0x002538BC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignOnly(true)]
		[Browsable(false)]
		[DefaultValue(true)]
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

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001DDD RID: 7645 RVA: 0x002544DC File Offset: 0x002538DC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbCommand_Parameters")]
		public new OleDbParameterCollection Parameters
		{
			get
			{
				OleDbParameterCollection oleDbParameterCollection = this._parameters;
				if (oleDbParameterCollection == null)
				{
					oleDbParameterCollection = new OleDbParameterCollection();
					this._parameters = oleDbParameterCollection;
				}
				return oleDbParameterCollection;
			}
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x00254504 File Offset: 0x00253904
		private bool HasParameters()
		{
			OleDbParameterCollection parameters = this._parameters;
			return parameters != null && 0 < parameters.Count;
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x00254528 File Offset: 0x00253928
		// (set) Token: 0x06001DE0 RID: 7648 RVA: 0x00254558 File Offset: 0x00253958
		[ResDescription("DbCommand_Transaction")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new OleDbTransaction Transaction
		{
			get
			{
				OleDbTransaction oleDbTransaction = this._transaction;
				while (oleDbTransaction != null && oleDbTransaction.Connection == null)
				{
					oleDbTransaction = oleDbTransaction.Parent;
					this._transaction = oleDbTransaction;
				}
				return oleDbTransaction;
			}
			set
			{
				this._transaction = value;
				Bid.Trace("<oledb.OleDbCommand.set_Transaction|API> %d#\n", this.ObjectID);
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x0025457C File Offset: 0x0025397C
		// (set) Token: 0x06001DE2 RID: 7650 RVA: 0x00254590 File Offset: 0x00253990
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

		// Token: 0x06001DE3 RID: 7651 RVA: 0x002545C8 File Offset: 0x002539C8
		private UnsafeNativeMethods.IAccessor IAccessor()
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|command> %d#, IAccessor\n", this.ObjectID);
			return (UnsafeNativeMethods.IAccessor)this._icommandText;
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x002545F0 File Offset: 0x002539F0
		internal UnsafeNativeMethods.ICommandProperties ICommandProperties()
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|command> %d#, ICommandProperties\n", this.ObjectID);
			return (UnsafeNativeMethods.ICommandProperties)this._icommandText;
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x00254618 File Offset: 0x00253A18
		private UnsafeNativeMethods.ICommandPrepare ICommandPrepare()
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|command> %d#, ICommandPrepare\n", this.ObjectID);
			return this._icommandText as UnsafeNativeMethods.ICommandPrepare;
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x00254640 File Offset: 0x00253A40
		private UnsafeNativeMethods.ICommandWithParameters ICommandWithParameters()
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|command> %d#, ICommandWithParameters\n", this.ObjectID);
			UnsafeNativeMethods.ICommandWithParameters commandWithParameters = this._icommandText as UnsafeNativeMethods.ICommandWithParameters;
			if (commandWithParameters == null)
			{
				throw ODB.NoProviderSupportForParameters(this._connection.Provider, null);
			}
			return commandWithParameters;
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x00254680 File Offset: 0x00253A80
		private void CreateAccessor()
		{
			UnsafeNativeMethods.ICommandWithParameters commandWithParameters = this.ICommandWithParameters();
			OleDbParameterCollection parameters = this._parameters;
			OleDbParameter[] array = new OleDbParameter[parameters.Count];
			parameters.CopyTo(array, 0);
			Bindings bindings = new Bindings(array, parameters.ChangeID);
			for (int i = 0; i < array.Length; i++)
			{
				bindings.ForceRebind |= array[i].BindParameter(i, bindings);
			}
			bindings.AllocateForAccessor(null, 0, 0);
			this.ApplyParameterBindings(commandWithParameters, bindings.BindInfo);
			UnsafeNativeMethods.IAccessor accessor = this.IAccessor();
			OleDbHResult oleDbHResult = bindings.CreateAccessor(accessor, 4);
			if (oleDbHResult < OleDbHResult.S_OK)
			{
				this.ProcessResults(oleDbHResult);
			}
			this._dbBindings = bindings;
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x00254724 File Offset: 0x00253B24
		private void ApplyParameterBindings(UnsafeNativeMethods.ICommandWithParameters commandWithParameters, tagDBPARAMBINDINFO[] bindInfo)
		{
			IntPtr[] array = new IntPtr[bindInfo.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (IntPtr)(i + 1);
			}
			Bid.Trace("<oledb.ICommandWithParameters.SetParameterInfo|API|OLEDB> %d#\n", this.ObjectID);
			OleDbHResult oleDbHResult = commandWithParameters.SetParameterInfo((IntPtr)bindInfo.Length, array, bindInfo);
			Bid.Trace("<oledb.ICommandWithParameters.SetParameterInfo|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
			if (oleDbHResult < OleDbHResult.S_OK)
			{
				this.ProcessResults(oleDbHResult);
			}
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x00254794 File Offset: 0x00253B94
		public override void Cancel()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbCommand.Cancel|API> %d#\n", this.ObjectID);
			try
			{
				this._changeID++;
				UnsafeNativeMethods.ICommandText icommandText = this._icommandText;
				if (icommandText != null)
				{
					OleDbHResult oleDbHResult = OleDbHResult.S_OK;
					lock (icommandText)
					{
						if (icommandText == this._icommandText)
						{
							Bid.Trace("<oledb.ICommandText.Cancel|API|OLEDB> %d#\n", this.ObjectID);
							oleDbHResult = icommandText.Cancel();
							Bid.Trace("<oledb.ICommandText.Cancel|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
						}
					}
					if (OleDbHResult.DB_E_CANTCANCEL != oleDbHResult)
					{
						this.canceling = true;
					}
					this.ProcessResultsNoReset(oleDbHResult);
				}
				else
				{
					this.canceling = true;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x00254868 File Offset: 0x00253C68
		public OleDbCommand Clone()
		{
			OleDbCommand oleDbCommand = new OleDbCommand(this);
			Bid.Trace("<oledb.OleDbCommand.Clone|API> %d#, clone=%d#\n", this.ObjectID, oleDbCommand.ObjectID);
			return oleDbCommand;
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x00254894 File Offset: 0x00253C94
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x002548A8 File Offset: 0x00253CA8
		internal void CloseCommandFromConnection(bool canceling)
		{
			this.canceling = canceling;
			this.CloseInternal();
			this._trackingForClose = false;
			this._transaction = null;
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x002548D0 File Offset: 0x00253CD0
		internal void CloseInternal()
		{
			this.CloseInternalParameters();
			this.CloseInternalCommand();
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x002548EC File Offset: 0x00253CEC
		internal void CloseFromDataReader(Bindings bindings)
		{
			if (bindings != null)
			{
				if (this.canceling)
				{
					bindings.Dispose();
				}
				else
				{
					bindings.ApplyOutputParameters();
					this.ParameterBindings = bindings;
				}
			}
			this._hasDataReader = false;
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x00254920 File Offset: 0x00253D20
		private void CloseInternalCommand()
		{
			this._changeID++;
			this.commandBehavior = CommandBehavior.Default;
			this._isPrepared = false;
			UnsafeNativeMethods.ICommandText commandText = Interlocked.Exchange<UnsafeNativeMethods.ICommandText>(ref this._icommandText, null);
			if (commandText != null)
			{
				lock (commandText)
				{
					Marshal.ReleaseComObject(commandText);
				}
			}
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x00254990 File Offset: 0x00253D90
		private void CloseInternalParameters()
		{
			Bindings dbBindings = this._dbBindings;
			this._dbBindings = null;
			if (dbBindings != null)
			{
				dbBindings.Dispose();
			}
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x002549B4 File Offset: 0x00253DB4
		public new OleDbParameter CreateParameter()
		{
			return new OleDbParameter();
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x002549C8 File Offset: 0x00253DC8
		protected override DbParameter CreateDbParameter()
		{
			return this.CreateParameter();
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x002549DC File Offset: 0x00253DDC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._changeID++;
				this.ResetConnection();
				this._transaction = null;
				this._parameters = null;
				this.CommandText = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x00254A1C File Offset: 0x00253E1C
		public new OleDbDataReader ExecuteReader()
		{
			return this.ExecuteReader(CommandBehavior.Default);
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x00254A30 File Offset: 0x00253E30
		IDataReader IDbCommand.ExecuteReader()
		{
			return this.ExecuteReader(CommandBehavior.Default);
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x00254A44 File Offset: 0x00253E44
		public new OleDbDataReader ExecuteReader(CommandBehavior behavior)
		{
			OleDbConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbCommand.ExecuteReader|API> %d#, behavior=%d{ds.CommandBehavior}\n", this.ObjectID, (int)behavior);
			OleDbDataReader oleDbDataReader;
			try
			{
				this._executeQuery = true;
				oleDbDataReader = this.ExecuteReaderInternal(behavior, "ExecuteReader");
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return oleDbDataReader;
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x00254AAC File Offset: 0x00253EAC
		IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)
		{
			return this.ExecuteReader(behavior);
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x00254AC0 File Offset: 0x00253EC0
		protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
		{
			return this.ExecuteReader(behavior);
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x00254AD4 File Offset: 0x00253ED4
		private OleDbDataReader ExecuteReaderInternal(CommandBehavior behavior, string method)
		{
			OleDbDataReader oleDbDataReader = null;
			OleDbException ex = null;
			int num = 0;
			try
			{
				this.ValidateConnectionAndTransaction(method);
				if ((CommandBehavior.SingleRow & behavior) != CommandBehavior.Default)
				{
					behavior |= CommandBehavior.SingleResult;
				}
				CommandType commandType = this.CommandType;
				object obj;
				int num2;
				switch (commandType)
				{
				case (CommandType)0:
				case CommandType.Text:
				case CommandType.StoredProcedure:
					num2 = this.ExecuteCommand(behavior, out obj);
					goto IL_006A;
				case (CommandType)2:
				case (CommandType)3:
					break;
				default:
					if (commandType == CommandType.TableDirect)
					{
						num2 = this.ExecuteTableDirect(behavior, out obj);
						goto IL_006A;
					}
					break;
				}
				throw ADP.InvalidCommandType(this.CommandType);
				IL_006A:
				if (this._executeQuery)
				{
					try
					{
						oleDbDataReader = new OleDbDataReader(this._connection, this, 0, this.commandBehavior);
						switch (num2)
						{
						case 0:
							oleDbDataReader.InitializeIMultipleResults(obj);
							oleDbDataReader.NextResult();
							break;
						case 1:
							oleDbDataReader.InitializeIRowset(obj, ChapterHandle.DB_NULL_HCHAPTER, this._recordsAffected);
							oleDbDataReader.BuildMetaInfo();
							oleDbDataReader.HasRowsRead();
							break;
						case 2:
							oleDbDataReader.InitializeIRow(obj, this._recordsAffected);
							oleDbDataReader.BuildMetaInfo();
							break;
						case 3:
							if (!this._isPrepared)
							{
								this.PrepareCommandText(2);
							}
							OleDbDataReader.GenerateSchemaTable(oleDbDataReader, this._icommandText, behavior);
							break;
						}
						obj = null;
						this._hasDataReader = true;
						this._connection.AddWeakReference(oleDbDataReader, 2);
						num = 1;
						goto IL_0199;
					}
					finally
					{
						if (1 != num)
						{
							this.canceling = true;
							if (oleDbDataReader != null)
							{
								((IDisposable)oleDbDataReader).Dispose();
								oleDbDataReader = null;
							}
						}
					}
				}
				try
				{
					if (num2 == 0)
					{
						UnsafeNativeMethods.IMultipleResults multipleResults = (UnsafeNativeMethods.IMultipleResults)obj;
						ex = OleDbDataReader.NextResults(multipleResults, this._connection, this, out this._recordsAffected);
					}
				}
				finally
				{
					try
					{
						if (obj != null)
						{
							Marshal.ReleaseComObject(obj);
							obj = null;
						}
						this.CloseFromDataReader(this.ParameterBindings);
					}
					catch (Exception ex2)
					{
						if (!ADP.IsCatchableExceptionType(ex2))
						{
							throw;
						}
						if (ex == null)
						{
							throw;
						}
						ex = new OleDbException(ex, ex2);
					}
				}
				IL_0199:;
			}
			finally
			{
				try
				{
					if (oleDbDataReader == null && 1 != num)
					{
						this.ParameterCleanup();
					}
				}
				catch (Exception ex3)
				{
					if (!ADP.IsCatchableExceptionType(ex3))
					{
						throw;
					}
					if (ex == null)
					{
						throw;
					}
					ex = new OleDbException(ex, ex3);
				}
				if (ex != null)
				{
					throw ex;
				}
			}
			return oleDbDataReader;
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x00254D30 File Offset: 0x00254130
		private int ExecuteCommand(CommandBehavior behavior, out object executeResult)
		{
			if (!this.InitializeCommand(behavior, false))
			{
				return this.ExecuteTableDirect(behavior, out executeResult);
			}
			if ((CommandBehavior.SchemaOnly & this.commandBehavior) != CommandBehavior.Default)
			{
				executeResult = null;
				return 3;
			}
			return this.ExecuteCommandText(out executeResult);
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x00254D68 File Offset: 0x00254168
		private int ExecuteCommandText(out object executeResult)
		{
			tagDBPARAMS tagDBPARAMS = null;
			RowBinding rowBinding = null;
			Bindings parameterBindings = this.ParameterBindings;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			int num;
			try
			{
				if (parameterBindings != null)
				{
					rowBinding = parameterBindings.RowBinding();
					rowBinding.DangerousAddRef(ref flag);
					parameterBindings.ApplyInputParameters();
					tagDBPARAMS = new tagDBPARAMS();
					tagDBPARAMS.pData = rowBinding.DangerousGetDataPtr();
					tagDBPARAMS.cParamSets = 1;
					tagDBPARAMS.hAccessor = rowBinding.DangerousGetAccessorHandle();
				}
				if ((CommandBehavior.SingleResult & this.commandBehavior) == CommandBehavior.Default && this._connection.SupportMultipleResults())
				{
					num = this.ExecuteCommandTextForMultpleResults(tagDBPARAMS, out executeResult);
				}
				else if ((CommandBehavior.SingleRow & this.commandBehavior) == CommandBehavior.Default || !this._executeQuery)
				{
					num = this.ExecuteCommandTextForSingleResult(tagDBPARAMS, out executeResult);
				}
				else
				{
					num = this.ExecuteCommandTextForSingleRow(tagDBPARAMS, out executeResult);
				}
			}
			finally
			{
				if (flag)
				{
					rowBinding.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x00254E38 File Offset: 0x00254238
		private int ExecuteCommandTextForMultpleResults(tagDBPARAMS dbParams, out object executeResult)
		{
			Bid.Trace("<oledb.ICommandText.Execute|API|OLEDB> %d#, IID_IMultipleResults\n", this.ObjectID);
			OleDbHResult oleDbHResult = this._icommandText.Execute(ADP.PtrZero, ref ODB.IID_IMultipleResults, dbParams, out this._recordsAffected, out executeResult);
			Bid.Trace("<oledb.ICommandText.Execute|API|OLEDB|RET> %08X{HRESULT}, RecordsAffected=%Id\n", oleDbHResult, this._recordsAffected);
			if (OleDbHResult.E_NOINTERFACE != oleDbHResult)
			{
				this.ExecuteCommandTextErrorHandling(oleDbHResult);
				return 0;
			}
			SafeNativeMethods.Wrapper.ClearErrorInfo();
			return this.ExecuteCommandTextForSingleResult(dbParams, out executeResult);
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x00254EA4 File Offset: 0x002542A4
		private int ExecuteCommandTextForSingleResult(tagDBPARAMS dbParams, out object executeResult)
		{
			OleDbHResult oleDbHResult;
			if (this._executeQuery)
			{
				Bid.Trace("<oledb.ICommandText.Execute|API|OLEDB> %d#, IID_IRowset\n", this.ObjectID);
				oleDbHResult = this._icommandText.Execute(ADP.PtrZero, ref ODB.IID_IRowset, dbParams, out this._recordsAffected, out executeResult);
				Bid.Trace("<oledb.ICommandText.Execute|API|OLEDB|RET> %08X{HRESULT}, RecordsAffected=%Id\n", oleDbHResult, this._recordsAffected);
			}
			else
			{
				Bid.Trace("<oledb.ICommandText.Execute|API|OLEDB> %d#, IID_NULL\n", this.ObjectID);
				oleDbHResult = this._icommandText.Execute(ADP.PtrZero, ref ODB.IID_NULL, dbParams, out this._recordsAffected, out executeResult);
				Bid.Trace("<oledb.ICommandText.Execute|API|OLEDB|RET> %08X{HRESULT}, RecordsAffected=%Id\n", oleDbHResult, this._recordsAffected);
			}
			this.ExecuteCommandTextErrorHandling(oleDbHResult);
			return 1;
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x00254F44 File Offset: 0x00254344
		private int ExecuteCommandTextForSingleRow(tagDBPARAMS dbParams, out object executeResult)
		{
			if (this._connection.SupportIRow(this))
			{
				Bid.Trace("<oledb.ICommandText.Execute|API|OLEDB> %d#, IID_IRow\n", this.ObjectID);
				OleDbHResult oleDbHResult = this._icommandText.Execute(ADP.PtrZero, ref ODB.IID_IRow, dbParams, out this._recordsAffected, out executeResult);
				Bid.Trace("<oledb.ICommandText.Execute|API|OLEDB|RET> %08X{HRESULT}, RecordsAffected=%Id\n", oleDbHResult, this._recordsAffected);
				if (OleDbHResult.DB_E_NOTFOUND == oleDbHResult)
				{
					SafeNativeMethods.Wrapper.ClearErrorInfo();
					return 2;
				}
				if (OleDbHResult.E_NOINTERFACE != oleDbHResult)
				{
					this.ExecuteCommandTextErrorHandling(oleDbHResult);
					return 2;
				}
			}
			SafeNativeMethods.Wrapper.ClearErrorInfo();
			return this.ExecuteCommandTextForSingleResult(dbParams, out executeResult);
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x00254FCC File Offset: 0x002543CC
		private void ExecuteCommandTextErrorHandling(OleDbHResult hr)
		{
			Exception ex = OleDbConnection.ProcessResults(hr, this._connection, this);
			if (ex != null)
			{
				ex = this.ExecuteCommandTextSpecialErrorHandling(hr, ex);
				throw ex;
			}
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x00254FF8 File Offset: 0x002543F8
		private Exception ExecuteCommandTextSpecialErrorHandling(OleDbHResult hr, Exception e)
		{
			if ((OleDbHResult.DB_E_ERRORSOCCURRED == hr || OleDbHResult.DB_E_BADBINDINFO == hr) && this._dbBindings != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.ParameterBindings.ParameterStatus(stringBuilder);
				e = ODB.CommandParameterStatus(stringBuilder.ToString(), e);
			}
			return e;
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x00255040 File Offset: 0x00254440
		public override int ExecuteNonQuery()
		{
			OleDbConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbCommand.ExecuteNonQuery|API> %d#\n", this.ObjectID);
			int num;
			try
			{
				this._executeQuery = false;
				this.ExecuteReaderInternal(CommandBehavior.Default, "ExecuteNonQuery");
				num = ADP.IntPtrToInt32(this._recordsAffected);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x002550B0 File Offset: 0x002544B0
		public override object ExecuteScalar()
		{
			OleDbConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbCommand.ExecuteScalar|API> %d#\n", this.ObjectID);
			object obj2;
			try
			{
				object obj = null;
				this._executeQuery = true;
				using (OleDbDataReader oleDbDataReader = this.ExecuteReaderInternal(CommandBehavior.Default, "ExecuteScalar"))
				{
					if (oleDbDataReader.Read() && 0 < oleDbDataReader.FieldCount)
					{
						obj = oleDbDataReader.GetValue(0);
					}
				}
				obj2 = obj;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return obj2;
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x00255158 File Offset: 0x00254558
		private int ExecuteTableDirect(CommandBehavior behavior, out object executeResult)
		{
			this.commandBehavior = behavior;
			executeResult = null;
			OleDbHResult oleDbHResult = OleDbHResult.S_OK;
			StringMemHandle stringMemHandle = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				stringMemHandle = new StringMemHandle(this.ExpandCommandText());
				stringMemHandle.DangerousAddRef(ref flag);
				if (flag)
				{
					tagDBID tagDBID = new tagDBID();
					tagDBID.uGuid = Guid.Empty;
					tagDBID.eKind = 2;
					tagDBID.ulPropid = stringMemHandle.DangerousGetHandle();
					using (IOpenRowsetWrapper openRowsetWrapper = this._connection.IOpenRowset())
					{
						using (DBPropSet dbpropSet = this.CommandPropertySets())
						{
							if (dbpropSet != null)
							{
								Bid.Trace("<oledb.IOpenRowset.OpenRowset|API|OLEDB> %d#, IID_IRowset\n", this.ObjectID);
								bool flag2 = false;
								RuntimeHelpers.PrepareConstrainedRegions();
								try
								{
									dbpropSet.DangerousAddRef(ref flag2);
									oleDbHResult = openRowsetWrapper.Value.OpenRowset(ADP.PtrZero, tagDBID, ADP.PtrZero, ref ODB.IID_IRowset, dbpropSet.PropertySetCount, dbpropSet.DangerousGetHandle(), out executeResult);
								}
								finally
								{
									if (flag2)
									{
										dbpropSet.DangerousRelease();
									}
								}
								Bid.Trace("<oledb.IOpenRowset.OpenRowset|API|OLEDB|RET> %08X{HRESULT}", oleDbHResult);
								if (OleDbHResult.DB_E_ERRORSOCCURRED == oleDbHResult)
								{
									Bid.Trace("<oledb.IOpenRowset.OpenRowset|API|OLEDB> %d#, IID_IRowset\n", this.ObjectID);
									oleDbHResult = openRowsetWrapper.Value.OpenRowset(ADP.PtrZero, tagDBID, ADP.PtrZero, ref ODB.IID_IRowset, 0, IntPtr.Zero, out executeResult);
									Bid.Trace("<oledb.IOpenRowset.OpenRowset|API|OLEDB|RET> %08X{HRESULT}", oleDbHResult);
								}
							}
							else
							{
								Bid.Trace("<oledb.IOpenRowset.OpenRowset|API|OLEDB> %d#, IID_IRowset\n", this.ObjectID);
								oleDbHResult = openRowsetWrapper.Value.OpenRowset(ADP.PtrZero, tagDBID, ADP.PtrZero, ref ODB.IID_IRowset, 0, IntPtr.Zero, out executeResult);
								Bid.Trace("<oledb.IOpenRowset.OpenRowset|API|OLEDB|RET> %08X{HRESULT}", oleDbHResult);
							}
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					stringMemHandle.DangerousRelease();
				}
			}
			this.ProcessResults(oleDbHResult);
			this._recordsAffected = ADP.RecordsUnaffected;
			return 1;
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x00255364 File Offset: 0x00254764
		private string ExpandCommandText()
		{
			string commandText = this.CommandText;
			if (ADP.IsEmpty(commandText))
			{
				return ADP.StrEmpty;
			}
			CommandType commandType = this.CommandType;
			CommandType commandType2 = commandType;
			if (commandType2 == CommandType.Text)
			{
				return commandText;
			}
			if (commandType2 == CommandType.StoredProcedure)
			{
				return this.ExpandStoredProcedureToText(commandText);
			}
			if (commandType2 != CommandType.TableDirect)
			{
				throw ADP.InvalidCommandType(commandType);
			}
			return commandText;
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x002553B4 File Offset: 0x002547B4
		private string ExpandOdbcMaximumToText(string sproctext, int parameterCount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (0 < parameterCount && ParameterDirection.ReturnValue == this.Parameters[0].Direction)
			{
				parameterCount--;
				stringBuilder.Append("{ ? = CALL ");
			}
			else
			{
				stringBuilder.Append("{ CALL ");
			}
			stringBuilder.Append(sproctext);
			switch (parameterCount)
			{
			case 0:
				stringBuilder.Append(" }");
				break;
			case 1:
				stringBuilder.Append("( ? ) }");
				break;
			default:
			{
				stringBuilder.Append("( ?, ?");
				for (int i = 2; i < parameterCount; i++)
				{
					stringBuilder.Append(", ?");
				}
				stringBuilder.Append(" ) }");
				break;
			}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x0025546C File Offset: 0x0025486C
		private string ExpandOdbcMinimumToText(string sproctext, int parameterCount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("exec ");
			stringBuilder.Append(sproctext);
			if (0 < parameterCount)
			{
				stringBuilder.Append(" ?");
				for (int i = 1; i < parameterCount; i++)
				{
					stringBuilder.Append(", ?");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x002554C4 File Offset: 0x002548C4
		private string ExpandStoredProcedureToText(string sproctext)
		{
			int num = ((this._parameters != null) ? this._parameters.Count : 0);
			if ((1 & this._connection.SqlSupport()) == 0)
			{
				return this.ExpandOdbcMinimumToText(sproctext, num);
			}
			return this.ExpandOdbcMaximumToText(sproctext, num);
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x00255508 File Offset: 0x00254908
		private void ParameterCleanup()
		{
			Bindings parameterBindings = this.ParameterBindings;
			if (parameterBindings != null)
			{
				parameterBindings.CleanupBindings();
			}
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x00255528 File Offset: 0x00254928
		private bool InitializeCommand(CommandBehavior behavior, bool throwifnotsupported)
		{
			int num = this._changeID;
			if ((CommandBehavior.KeyInfo & (this.commandBehavior ^ behavior)) != CommandBehavior.Default || this._lastChangeID != num)
			{
				this.CloseInternalParameters();
				this.CloseInternalCommand();
			}
			this.commandBehavior = behavior;
			num = this._changeID;
			if (!this.PropertiesOnCommand(false))
			{
				return false;
			}
			if (this._dbBindings != null && this._dbBindings.AreParameterBindingsInvalid(this._parameters))
			{
				this.CloseInternalParameters();
			}
			if (this._dbBindings == null && this.HasParameters())
			{
				this.CreateAccessor();
			}
			if (this._lastChangeID != num)
			{
				string text = this.ExpandCommandText();
				if (Bid.TraceOn)
				{
					Bid.Trace("<oledb.ICommandText.SetCommandText|API|OLEDB> %d#, DBGUID_DEFAULT, CommandText='", this.ObjectID);
					Bid.PutStr(text);
					Bid.Trace("'\n");
				}
				OleDbHResult oleDbHResult = this._icommandText.SetCommandText(ref ODB.DBGUID_DEFAULT, text);
				Bid.Trace("<oledb.ICommandText.SetCommandText|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
				if (oleDbHResult < OleDbHResult.S_OK)
				{
					this.ProcessResults(oleDbHResult);
				}
			}
			this._lastChangeID = num;
			return true;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x00255614 File Offset: 0x00254A14
		private void PropertyChanging()
		{
			this._changeID++;
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x00255630 File Offset: 0x00254A30
		public override void Prepare()
		{
			OleDbConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<oledb.OleDbCommand.Prepare|API> %d#\n", this.ObjectID);
			try
			{
				if (CommandType.TableDirect != this.CommandType)
				{
					this.ValidateConnectionAndTransaction("Prepare");
					this._isPrepared = false;
					if (CommandType.TableDirect != this.CommandType)
					{
						this.InitializeCommand(CommandBehavior.Default, true);
						this.PrepareCommandText(1);
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x002556BC File Offset: 0x00254ABC
		private void PrepareCommandText(int expectedExecutionCount)
		{
			OleDbParameterCollection parameters = this._parameters;
			if (parameters != null)
			{
				foreach (object obj in parameters)
				{
					OleDbParameter oleDbParameter = (OleDbParameter)obj;
					if (oleDbParameter.IsParameterComputed())
					{
						oleDbParameter.Prepare(this);
					}
				}
			}
			UnsafeNativeMethods.ICommandPrepare commandPrepare = this.ICommandPrepare();
			if (commandPrepare != null)
			{
				Bid.Trace("<oledb.ICommandPrepare.Prepare|API|OLEDB> %d#, expectedExecutionCount=%d\n", this.ObjectID, expectedExecutionCount);
				OleDbHResult oleDbHResult = commandPrepare.Prepare(expectedExecutionCount);
				Bid.Trace("<oledb.ICommandPrepare.Prepare|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
				this.ProcessResults(oleDbHResult);
			}
			this._isPrepared = true;
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x00255770 File Offset: 0x00254B70
		private void ProcessResults(OleDbHResult hr)
		{
			Exception ex = OleDbConnection.ProcessResults(hr, this._connection, this);
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x00255790 File Offset: 0x00254B90
		private void ProcessResultsNoReset(OleDbHResult hr)
		{
			Exception ex = OleDbConnection.ProcessResults(hr, null, this);
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x002557AC File Offset: 0x00254BAC
		internal object GetPropertyValue(Guid propertySet, int propertyID)
		{
			if (this._icommandText == null)
			{
				return OleDbPropertyStatus.NotSupported;
			}
			UnsafeNativeMethods.ICommandProperties commandProperties = this.ICommandProperties();
			tagDBPROP[] propertySet2;
			using (PropertyIDSet propertyIDSet = new PropertyIDSet(propertySet, propertyID))
			{
				OleDbHResult oleDbHResult;
				using (DBPropSet dbpropSet = new DBPropSet(commandProperties, propertyIDSet, out oleDbHResult))
				{
					if (oleDbHResult < OleDbHResult.S_OK)
					{
						SafeNativeMethods.Wrapper.ClearErrorInfo();
					}
					propertySet2 = dbpropSet.GetPropertySet(0, out propertySet);
				}
			}
			if (propertySet2[0].dwStatus == OleDbPropertyStatus.Ok)
			{
				return propertySet2[0].vValue;
			}
			return propertySet2[0].dwStatus;
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x00255864 File Offset: 0x00254C64
		private bool PropertiesOnCommand(bool throwNotSupported)
		{
			if (this._icommandText != null)
			{
				return true;
			}
			OleDbConnection connection = this._connection;
			if (connection == null)
			{
				connection.CheckStateOpen("Properties");
			}
			if (!this._trackingForClose)
			{
				this._trackingForClose = true;
				connection.AddWeakReference(this, 1);
			}
			this._icommandText = connection.ICommandText();
			if (this._icommandText != null)
			{
				using (DBPropSet dbpropSet = this.CommandPropertySets())
				{
					if (dbpropSet != null)
					{
						UnsafeNativeMethods.ICommandProperties commandProperties = this.ICommandProperties();
						Bid.Trace("<oledb.ICommandProperties.SetProperties|API|OLEDB> %d#\n", this.ObjectID);
						OleDbHResult oleDbHResult = commandProperties.SetProperties(dbpropSet.PropertySetCount, dbpropSet);
						Bid.Trace("<oledb.ICommandProperties.SetProperties|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
						if (oleDbHResult < OleDbHResult.S_OK)
						{
							SafeNativeMethods.Wrapper.ClearErrorInfo();
						}
					}
				}
				return true;
			}
			if (throwNotSupported || this.HasParameters())
			{
				throw ODB.CommandTextNotSupported(connection.Provider, null);
			}
			return false;
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x00255944 File Offset: 0x00254D44
		private DBPropSet CommandPropertySets()
		{
			DBPropSet dbpropSet = null;
			bool flag = CommandBehavior.Default != (CommandBehavior.KeyInfo & this.commandBehavior);
			int num = (this._executeQuery ? (flag ? 4 : 2) : 1);
			if (0 < num)
			{
				dbpropSet = new DBPropSet(1);
				tagDBPROP[] array = new tagDBPROP[num];
				array[0] = new tagDBPROP(34, false, this.CommandTimeout);
				if (this._executeQuery)
				{
					array[1] = new tagDBPROP(231, false, 2);
					if (flag)
					{
						array[2] = new tagDBPROP(238, false, flag);
						array[3] = new tagDBPROP(123, false, true);
					}
				}
				dbpropSet.SetPropertySet(0, OleDbPropertySetGuid.Rowset, array);
			}
			return dbpropSet;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x002559F4 File Offset: 0x00254DF4
		internal Bindings TakeBindingOwnerShip()
		{
			Bindings dbBindings = this._dbBindings;
			this._dbBindings = null;
			return dbBindings;
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x00255A10 File Offset: 0x00254E10
		private void ValidateConnection(string method)
		{
			if (this._connection == null)
			{
				throw ADP.ConnectionRequired(method);
			}
			this._connection.CheckStateOpen(method);
			if (this._hasDataReader)
			{
				if (this._connection.HasLiveReader(this))
				{
					throw ADP.OpenReaderExists();
				}
				this._hasDataReader = false;
			}
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x00255A5C File Offset: 0x00254E5C
		private void ValidateConnectionAndTransaction(string method)
		{
			this.ValidateConnection(method);
			this._transaction = this._connection.ValidateTransaction(this.Transaction, method);
			this.canceling = false;
		}

		// Token: 0x04001247 RID: 4679
		private string _commandText;

		// Token: 0x04001248 RID: 4680
		private CommandType _commandType;

		// Token: 0x04001249 RID: 4681
		private int _commandTimeout = 30;

		// Token: 0x0400124A RID: 4682
		private UpdateRowSource _updatedRowSource = UpdateRowSource.Both;

		// Token: 0x0400124B RID: 4683
		private bool _designTimeInvisible;

		// Token: 0x0400124C RID: 4684
		private OleDbConnection _connection;

		// Token: 0x0400124D RID: 4685
		private OleDbTransaction _transaction;

		// Token: 0x0400124E RID: 4686
		private static int _objectTypeCount;

		// Token: 0x0400124F RID: 4687
		internal readonly int ObjectID = Interlocked.Increment(ref OleDbCommand._objectTypeCount);

		// Token: 0x04001250 RID: 4688
		private OleDbParameterCollection _parameters;

		// Token: 0x04001251 RID: 4689
		private UnsafeNativeMethods.ICommandText _icommandText;

		// Token: 0x04001252 RID: 4690
		private CommandBehavior commandBehavior;

		// Token: 0x04001253 RID: 4691
		private Bindings _dbBindings;

		// Token: 0x04001254 RID: 4692
		internal bool canceling;

		// Token: 0x04001255 RID: 4693
		private bool _isPrepared;

		// Token: 0x04001256 RID: 4694
		private bool _executeQuery;

		// Token: 0x04001257 RID: 4695
		private bool _trackingForClose;

		// Token: 0x04001258 RID: 4696
		private bool _hasDataReader;

		// Token: 0x04001259 RID: 4697
		private IntPtr _recordsAffected;

		// Token: 0x0400125A RID: 4698
		private int _changeID;

		// Token: 0x0400125B RID: 4699
		private int _lastChangeID;
	}
}
