using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.Data.OracleClient
{
	// Token: 0x0200004E RID: 78
	[ToolboxItem(true)]
	[Designer("Microsoft.VSDesigner.Data.VS.OracleCommandDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("RecordsAffected")]
	public sealed class OracleCommand : DbCommand, ICloneable
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0005E06C File Offset: 0x0005D46C
		public OracleCommand()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0005E09C File Offset: 0x0005D49C
		public OracleCommand(string commandText)
			: this()
		{
			this.CommandText = commandText;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0005E0B8 File Offset: 0x0005D4B8
		public OracleCommand(string commandText, OracleConnection connection)
			: this()
		{
			this.CommandText = commandText;
			this.Connection = connection;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0005E0DC File Offset: 0x0005D4DC
		public OracleCommand(string commandText, OracleConnection connection, OracleTransaction tx)
			: this()
		{
			this.CommandText = commandText;
			this.Connection = connection;
			this.Transaction = tx;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0005E104 File Offset: 0x0005D504
		private OracleCommand(OracleCommand command)
			: this()
		{
			this.CommandText = command.CommandText;
			this.CommandType = command.CommandType;
			this.Connection = command.Connection;
			this.DesignTimeVisible = command.DesignTimeVisible;
			this.UpdatedRowSource = command.UpdatedRowSource;
			this.Transaction = command.Transaction;
			if (command._parameterCollection != null && 0 < command._parameterCollection.Count)
			{
				OracleParameterCollection parameters = this.Parameters;
				foreach (object obj in command.Parameters)
				{
					ICloneable cloneable = (ICloneable)obj;
					parameters.Add(cloneable.Clone());
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0005E1DC File Offset: 0x0005D5DC
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0005E1FC File Offset: 0x0005D5FC
		[Editor("Microsoft.VSDesigner.Data.Oracle.Design.OracleCommandTextEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("OracleCategory_Data")]
		[DefaultValue("")]
		[ResDescription("DbCommand_CommandText")]
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
					Bid.Trace("<ora.OracleCommand.set_CommandText|API> %d#, '", this.ObjectID);
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0005E24C File Offset: 0x0005D64C
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x0005E25C File Offset: 0x0005D65C
		[ResCategory("OracleCategory_Data")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[ResDescription("DbCommand_CommandTimeout")]
		public override int CommandTimeout
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0005E26C File Offset: 0x0005D66C
		public void ResetCommandTimeout()
		{
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0005E27C File Offset: 0x0005D67C
		private bool ShouldSerializeCommandTimeout()
		{
			return false;
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0005E28C File Offset: 0x0005D68C
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x0005E2A8 File Offset: 0x0005D6A8
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(CommandType.Text)]
		[ResDescription("DbCommand_CommandType")]
		[ResCategory("OracleCategory_Data")]
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
				throw ADP.NoOptimizedDirectTableAccess();
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0005E2F0 File Offset: 0x0005D6F0
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x0005E304 File Offset: 0x0005D704
		[ResCategory("OracleCategory_Behavior")]
		[ResDescription("DbCommand_Connection")]
		[DefaultValue(null)]
		[Editor("Microsoft.VSDesigner.Data.Design.DbConnectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public new OracleConnection Connection
		{
			get
			{
				return this._connection;
			}
			set
			{
				if (this._connection != value)
				{
					this.PropertyChanging();
					this._connection = value;
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0005E328 File Offset: 0x0005D728
		private bool ConnectionIsClosed
		{
			get
			{
				OracleConnection connection = this.Connection;
				return connection == null || ConnectionState.Closed == connection.State;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0005E34C File Offset: 0x0005D74C
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0005E360 File Offset: 0x0005D760
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
			set
			{
				this.Connection = (OracleConnection)value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0005E37C File Offset: 0x0005D77C
		protected override DbParameterCollection DbParameterCollection
		{
			get
			{
				return this.Parameters;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0005E390 File Offset: 0x0005D790
		// (set) Token: 0x060002BF RID: 703 RVA: 0x0005E3A4 File Offset: 0x0005D7A4
		protected override DbTransaction DbTransaction
		{
			get
			{
				return this.Transaction;
			}
			set
			{
				this.Transaction = (OracleTransaction)value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0005E3C0 File Offset: 0x0005D7C0
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x0005E3D8 File Offset: 0x0005D7D8
		[Browsable(false)]
		[DesignOnly(true)]
		[DefaultValue(true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0005E3F8 File Offset: 0x0005D7F8
		private OciEnvironmentHandle EnvironmentHandle
		{
			get
			{
				return this._connection.EnvironmentHandle;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0005E410 File Offset: 0x0005D810
		private OciErrorHandle ErrorHandle
		{
			get
			{
				return this._connection.ErrorHandle;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0005E428 File Offset: 0x0005D828
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0005E43C File Offset: 0x0005D83C
		[ResCategory("OracleCategory_Data")]
		[ResDescription("DbCommand_Parameters")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public new OracleParameterCollection Parameters
		{
			get
			{
				if (this._parameterCollection == null)
				{
					this._parameterCollection = new OracleParameterCollection();
				}
				return this._parameterCollection;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0005E464 File Offset: 0x0005D864
		internal string StatementText
		{
			get
			{
				string text = null;
				string commandText = this.CommandText;
				if (ADP.IsEmpty(commandText))
				{
					throw ADP.NoCommandText();
				}
				CommandType commandType = this.CommandType;
				if (commandType != CommandType.Text)
				{
					if (commandType == CommandType.StoredProcedure)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append("begin ");
						int count = this.Parameters.Count;
						int num = 0;
						for (int i = 0; i < count; i++)
						{
							OracleParameter oracleParameter = this.Parameters[i];
							if (ADP.IsDirection(oracleParameter, ParameterDirection.ReturnValue))
							{
								stringBuilder.Append(":");
								stringBuilder.Append(oracleParameter.ParameterName);
								stringBuilder.Append(" := ");
							}
						}
						stringBuilder.Append(commandText);
						string text2 = "(";
						for (int j = 0; j < count; j++)
						{
							OracleParameter oracleParameter2 = this.Parameters[j];
							if (!ADP.IsDirection(oracleParameter2, ParameterDirection.ReturnValue) && (ADP.IsDirection(oracleParameter2, ParameterDirection.Output) || oracleParameter2.Value != null) && (oracleParameter2.Value != null || ADP.IsDirection(oracleParameter2, ParameterDirection.Output)))
							{
								stringBuilder.Append(text2);
								text2 = ", ";
								num++;
								stringBuilder.Append(oracleParameter2.ParameterName);
								stringBuilder.Append("=>:");
								stringBuilder.Append(oracleParameter2.ParameterName);
							}
						}
						if (num != 0)
						{
							stringBuilder.Append("); end;");
						}
						else
						{
							stringBuilder.Append("; end;");
						}
						text = stringBuilder.ToString();
					}
				}
				else
				{
					text = commandText;
				}
				return text;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0005E5D4 File Offset: 0x0005D9D4
		private OciServiceContextHandle ServiceContextHandle
		{
			get
			{
				return this._connection.ServiceContextHandle;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0005E5EC File Offset: 0x0005D9EC
		internal OCI.STMT StatementType
		{
			get
			{
				return this._statementType;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0005E600 File Offset: 0x0005DA00
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0005E630 File Offset: 0x0005DA30
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[ResDescription("DbCommand_Transaction")]
		[Browsable(false)]
		public new OracleTransaction Transaction
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
				this._transaction = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0005E644 File Offset: 0x0005DA44
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0005E658 File Offset: 0x0005DA58
		[DefaultValue(UpdateRowSource.Both)]
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbCommand_UpdatedRowSource")]
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

		// Token: 0x060002CD RID: 717 RVA: 0x0005E690 File Offset: 0x0005DA90
		public override void Cancel()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleCommand.Cancel|API> %d#\n", this.ObjectID);
			try
			{
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0005E6D8 File Offset: 0x0005DAD8
		public object Clone()
		{
			OracleCommand oracleCommand = new OracleCommand(this);
			Bid.Trace("<ora.OracleCommand.Clone|API> %d#, clone=%d#\n", this.ObjectID, oracleCommand.ObjectID);
			return oracleCommand;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0005E704 File Offset: 0x0005DB04
		public new OracleParameter CreateParameter()
		{
			return new OracleParameter();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0005E718 File Offset: 0x0005DB18
		protected override DbParameter CreateDbParameter()
		{
			return this.CreateParameter();
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0005E72C File Offset: 0x0005DB2C
		internal string Execute(OciStatementHandle statementHandle, CommandBehavior behavior, out ArrayList resultParameterOrdinals)
		{
			OciRowidDescriptor ociRowidDescriptor;
			return this.Execute(statementHandle, behavior, false, out ociRowidDescriptor, out resultParameterOrdinals);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0005E748 File Offset: 0x0005DB48
		internal string Execute(OciStatementHandle statementHandle, CommandBehavior behavior, bool needRowid, out OciRowidDescriptor rowidDescriptor, out ArrayList resultParameterOrdinals)
		{
			if (this.ConnectionIsClosed)
			{
				throw ADP.ClosedConnectionError();
			}
			if (this._transaction == null && this.Connection.Transaction != null)
			{
				throw ADP.TransactionRequired();
			}
			if (this._transaction != null && this._transaction.Connection != null && this.Connection != this._transaction.Connection)
			{
				throw ADP.TransactionConnectionMismatch();
			}
			rowidDescriptor = null;
			this.Connection.RollbackDeadTransaction();
			NativeBuffer nativeBuffer = null;
			bool flag = false;
			bool[] array = null;
			SafeHandle[] array2 = null;
			OracleParameterBinding[] array3 = null;
			string text = null;
			resultParameterOrdinals = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				int num;
				if (this._preparedStatementHandle != statementHandle)
				{
					text = this.StatementText;
					num = TracedNativeMethods.OCIStmtPrepare(statementHandle, this.ErrorHandle, text, OCI.SYNTAX.OCI_NTV_SYNTAX, OCI.MODE.OCI_DEFAULT, this.Connection);
					if (num != 0)
					{
						this.Connection.CheckError(this.ErrorHandle, num);
					}
				}
				short num2;
				statementHandle.GetAttribute(OCI.ATTR.OCI_ATTR_STMT_TYPE, out num2, this.ErrorHandle);
				this._statementType = (OCI.STMT)num2;
				int num3;
				if (OCI.STMT.OCI_STMT_SELECT != this._statementType)
				{
					num3 = 1;
				}
				else
				{
					num3 = 0;
					if (CommandBehavior.SingleRow != behavior)
					{
						statementHandle.SetAttribute(OCI.ATTR.OCI_ATTR_PREFETCH_ROWS, 0, this.ErrorHandle);
						statementHandle.SetAttribute(OCI.ATTR.OCI_ATTR_PREFETCH_MEMORY, 0, this.ErrorHandle);
					}
				}
				OCI.MODE mode = OCI.MODE.OCI_DEFAULT;
				if (num3 == 0)
				{
					if (OracleCommand.IsBehavior(behavior, CommandBehavior.SchemaOnly))
					{
						mode |= OCI.MODE.OCI_SHARED;
					}
				}
				else if (this._connection.TransactionState == TransactionState.AutoCommit)
				{
					mode |= OCI.MODE.OCI_COMMIT_ON_SUCCESS;
				}
				else if (TransactionState.GlobalStarted != this._connection.TransactionState)
				{
					this._connection.TransactionState = TransactionState.LocalStarted;
				}
				if ((mode & OCI.MODE.OCI_SHARED) == OCI.MODE.OCI_DEFAULT && this._parameterCollection != null && this._parameterCollection.Count > 0)
				{
					int num4 = 0;
					int count = this._parameterCollection.Count;
					array = new bool[count];
					array2 = new SafeHandle[count];
					array3 = new OracleParameterBinding[count];
					for (int i = 0; i < count; i++)
					{
						array3[i] = new OracleParameterBinding(this, this._parameterCollection[i]);
						array3[i].PrepareForBind(this._connection, ref num4);
						if (OracleType.Cursor == this._parameterCollection[i].OracleType || 0 < this._parameterCollection[i].CommandSetResult)
						{
							if (resultParameterOrdinals == null)
							{
								resultParameterOrdinals = new ArrayList();
							}
							resultParameterOrdinals.Add(i);
						}
					}
					nativeBuffer = new NativeBuffer_ParameterBuffer(num4);
					nativeBuffer.DangerousAddRef(ref flag);
					for (int j = 0; j < count; j++)
					{
						array3[j].Bind(statementHandle, nativeBuffer, this._connection, ref array[j], ref array2[j]);
					}
				}
				num = TracedNativeMethods.OCIStmtExecute(this.ServiceContextHandle, statementHandle, this.ErrorHandle, num3, mode);
				if (num != 0)
				{
					this.Connection.CheckError(this.ErrorHandle, num);
				}
				if (array3 != null)
				{
					int num5 = array3.Length;
					for (int k = 0; k < num5; k++)
					{
						array3[k].PostExecute(nativeBuffer, this._connection);
						array3[k].Dispose();
						array3[k] = null;
					}
					array3 = null;
				}
				if (needRowid && (mode & OCI.MODE.OCI_SHARED) == OCI.MODE.OCI_DEFAULT)
				{
					switch (this._statementType)
					{
					case OCI.STMT.OCI_STMT_UPDATE:
					case OCI.STMT.OCI_STMT_DELETE:
					case OCI.STMT.OCI_STMT_INSERT:
						rowidDescriptor = statementHandle.GetRowid(this.EnvironmentHandle, this.ErrorHandle);
						break;
					default:
						rowidDescriptor = null;
						break;
					}
				}
			}
			finally
			{
				if (flag)
				{
					nativeBuffer.DangerousRelease();
				}
				if (nativeBuffer != null)
				{
					nativeBuffer.Dispose();
					nativeBuffer = null;
				}
				if (array3 != null)
				{
					int num6 = array3.Length;
					for (int l = 0; l < num6; l++)
					{
						if (array3[l] != null)
						{
							array3[l].Dispose();
							array3[l] = null;
						}
					}
					array3 = null;
				}
				if (array != null && array2 != null)
				{
					int num7 = array.Length;
					for (int m = 0; m < num7; m++)
					{
						if (array[m])
						{
							array2[m].DangerousRelease();
						}
					}
				}
			}
			return text;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0005EAF4 File Offset: 0x0005DEF4
		protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
		{
			return this.ExecuteReader(behavior);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0005EB08 File Offset: 0x0005DF08
		public override int ExecuteNonQuery()
		{
			OracleConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleCommand.ExecuteNonQuery|API> %d#\n", this.ObjectID);
			int num2;
			try
			{
				OciRowidDescriptor ociRowidDescriptor = null;
				int num = this.ExecuteNonQueryInternal(false, out ociRowidDescriptor);
				OciHandle.SafeDispose(ref ociRowidDescriptor);
				num2 = num;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num2;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0005EB70 File Offset: 0x0005DF70
		private int ExecuteNonQueryInternal(bool needRowid, out OciRowidDescriptor rowidDescriptor)
		{
			OciStatementHandle ociStatementHandle = null;
			int num = -1;
			try
			{
				try
				{
					ArrayList arrayList = new ArrayList();
					ociStatementHandle = this.GetStatementHandle();
					this.Execute(ociStatementHandle, CommandBehavior.Default, needRowid, out rowidDescriptor, out arrayList);
					if (arrayList != null)
					{
						num = 0;
						using (IEnumerator enumerator = arrayList.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								int num2 = (int)obj;
								OracleParameter oracleParameter = this._parameterCollection[num2];
								if (OracleType.Cursor != oracleParameter.OracleType)
								{
									num += (int)oracleParameter.Value;
								}
							}
							goto IL_0098;
						}
					}
					if (OCI.STMT.OCI_STMT_SELECT != this._statementType)
					{
						ociStatementHandle.GetAttribute(OCI.ATTR.OCI_ATTR_ROW_COUNT, out num, this.ErrorHandle);
					}
					IL_0098:;
				}
				finally
				{
					if (ociStatementHandle != null)
					{
						this.ReleaseStatementHandle(ociStatementHandle);
					}
				}
			}
			catch
			{
				throw;
			}
			return num;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0005EC78 File Offset: 0x0005E078
		public int ExecuteOracleNonQuery(out OracleString rowid)
		{
			OracleConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleCommand.ExecuteOracleNonQuery|API> %d#\n", this.ObjectID);
			int num2;
			try
			{
				OciRowidDescriptor ociRowidDescriptor = null;
				int num = this.ExecuteNonQueryInternal(true, out ociRowidDescriptor);
				rowid = OracleCommand.GetPersistedRowid(this.Connection, ociRowidDescriptor);
				OciHandle.SafeDispose(ref ociRowidDescriptor);
				num2 = num;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num2;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0005ECF0 File Offset: 0x0005E0F0
		public object ExecuteOracleScalar()
		{
			OracleConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleCommand.ExecuteOracleScalar|API> %d#", this.ObjectID);
			object obj2;
			try
			{
				OciRowidDescriptor ociRowidDescriptor = null;
				object obj = this.ExecuteScalarInternal(false, false, out ociRowidDescriptor);
				OciHandle.SafeDispose(ref ociRowidDescriptor);
				obj2 = obj;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return obj2;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0005ED58 File Offset: 0x0005E158
		public new OracleDataReader ExecuteReader()
		{
			return this.ExecuteReader(CommandBehavior.Default);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0005ED6C File Offset: 0x0005E16C
		public new OracleDataReader ExecuteReader(CommandBehavior behavior)
		{
			OracleConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleCommand.ExecuteReader|API> %d#, behavior=%d{ds.CommandBehavior}\n", this.ObjectID, (int)behavior);
			OracleDataReader oracleDataReader2;
			try
			{
				OciStatementHandle ociStatementHandle = null;
				OracleDataReader oracleDataReader = null;
				ArrayList arrayList = null;
				try
				{
					ociStatementHandle = this.GetStatementHandle();
					string text = this.Execute(ociStatementHandle, behavior, out arrayList);
					if (ociStatementHandle == this._preparedStatementHandle)
					{
						this._preparedStatementHandle = null;
					}
					if (arrayList == null)
					{
						oracleDataReader = new OracleDataReader(this, ociStatementHandle, text, behavior);
					}
					else
					{
						oracleDataReader = new OracleDataReader(this, arrayList, text, behavior);
					}
				}
				finally
				{
					if (ociStatementHandle != null && (oracleDataReader == null || arrayList != null))
					{
						this.ReleaseStatementHandle(ociStatementHandle);
					}
				}
				oracleDataReader2 = oracleDataReader;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return oracleDataReader2;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0005EE30 File Offset: 0x0005E230
		public override object ExecuteScalar()
		{
			OracleConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleCommand.ExecuteScalar|API> %d#\n", this.ObjectID);
			object obj2;
			try
			{
				OciRowidDescriptor ociRowidDescriptor;
				object obj = this.ExecuteScalarInternal(true, false, out ociRowidDescriptor);
				OciHandle.SafeDispose(ref ociRowidDescriptor);
				obj2 = obj;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return obj2;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0005EE94 File Offset: 0x0005E294
		private object ExecuteScalarInternal(bool needCLStype, bool needRowid, out OciRowidDescriptor rowidDescriptor)
		{
			OciStatementHandle ociStatementHandle = null;
			object obj = null;
			try
			{
				ociStatementHandle = this.GetStatementHandle();
				ArrayList arrayList = new ArrayList();
				this.Execute(ociStatementHandle, CommandBehavior.Default, needRowid, out rowidDescriptor, out arrayList);
				if (OCI.STMT.OCI_STMT_SELECT == this._statementType)
				{
					OracleColumn oracleColumn = new OracleColumn(ociStatementHandle, 0, this.ErrorHandle, this._connection);
					int num = 0;
					bool flag = false;
					bool flag2 = false;
					SafeHandle safeHandle = null;
					oracleColumn.Describe(ref num, this._connection, this.ErrorHandle);
					NativeBuffer_RowBuffer nativeBuffer_RowBuffer = new NativeBuffer_RowBuffer(num, 1);
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						nativeBuffer_RowBuffer.DangerousAddRef(ref flag);
						oracleColumn.Bind(ociStatementHandle, nativeBuffer_RowBuffer, this.ErrorHandle, 0);
						oracleColumn.Rebind(this._connection, ref flag2, ref safeHandle);
						int num2 = TracedNativeMethods.OCIStmtFetch(ociStatementHandle, this.ErrorHandle, 1, OCI.FETCH.OCI_FETCH_NEXT, OCI.MODE.OCI_DEFAULT);
						if (100 != num2)
						{
							if (num2 != 0)
							{
								this.Connection.CheckError(this.ErrorHandle, num2);
							}
							if (needCLStype)
							{
								obj = oracleColumn.GetValue(nativeBuffer_RowBuffer);
							}
							else
							{
								obj = oracleColumn.GetOracleValue(nativeBuffer_RowBuffer);
							}
						}
					}
					finally
					{
						if (flag2)
						{
							safeHandle.DangerousRelease();
						}
						if (flag)
						{
							nativeBuffer_RowBuffer.DangerousRelease();
						}
					}
					GC.KeepAlive(oracleColumn);
				}
			}
			finally
			{
				if (ociStatementHandle != null)
				{
					this.ReleaseStatementHandle(ociStatementHandle);
				}
			}
			return obj;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0005EFDC File Offset: 0x0005E3DC
		internal static OracleString GetPersistedRowid(OracleConnection connection, OciRowidDescriptor rowidHandle)
		{
			OracleString oracleString = OracleString.Null;
			if (rowidHandle != null)
			{
				OciErrorHandle errorHandle = connection.ErrorHandle;
				NativeBuffer scratchBuffer = connection.GetScratchBuffer(3970);
				bool flag = false;
				bool flag2 = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					scratchBuffer.DangerousAddRef(ref flag);
					if (OCI.ClientVersionAtLeastOracle9i)
					{
						int length = scratchBuffer.Length;
						int num = TracedNativeMethods.OCIRowidToChar(rowidHandle, scratchBuffer, ref length, errorHandle);
						if (num != 0)
						{
							connection.CheckError(errorHandle, num);
						}
						string text = scratchBuffer.PtrToStringAnsi(0, length);
						oracleString = new OracleString(text);
					}
					else
					{
						rowidHandle.DangerousAddRef(ref flag2);
						OciServiceContextHandle serviceContextHandle = connection.ServiceContextHandle;
						OciStatementHandle ociStatementHandle = new OciStatementHandle(serviceContextHandle);
						string text2 = "begin :rowid := :rdesc; end;";
						int num2 = 0;
						int num3 = 4;
						int num4 = 8;
						int num5 = 12;
						int num6 = 16;
						int num7 = 20;
						try
						{
							int num = TracedNativeMethods.OCIStmtPrepare(ociStatementHandle, errorHandle, text2, OCI.SYNTAX.OCI_NTV_SYNTAX, OCI.MODE.OCI_DEFAULT, connection);
							if (num != 0)
							{
								connection.CheckError(errorHandle, num);
							}
							scratchBuffer.WriteIntPtr(num4, rowidHandle.DangerousGetHandle());
							scratchBuffer.WriteInt32(num2, 0);
							scratchBuffer.WriteInt32(num3, 4);
							scratchBuffer.WriteInt32(num5, 0);
							scratchBuffer.WriteInt32(num6, 3950);
							IntPtr intPtr;
							num = TracedNativeMethods.OCIBindByName(ociStatementHandle, out intPtr, errorHandle, "rowid", 5, scratchBuffer.DangerousGetDataPtr(num7), 3950, OCI.DATATYPE.VARCHAR2, scratchBuffer.DangerousGetDataPtr(num5), scratchBuffer.DangerousGetDataPtr(num6), OCI.MODE.OCI_DEFAULT);
							if (num != 0)
							{
								connection.CheckError(errorHandle, num);
							}
							IntPtr intPtr2;
							num = TracedNativeMethods.OCIBindByName(ociStatementHandle, out intPtr2, errorHandle, "rdesc", 5, scratchBuffer.DangerousGetDataPtr(num4), 4, OCI.DATATYPE.ROWID_DESC, scratchBuffer.DangerousGetDataPtr(num2), scratchBuffer.DangerousGetDataPtr(num3), OCI.MODE.OCI_DEFAULT);
							if (num != 0)
							{
								connection.CheckError(errorHandle, num);
							}
							num = TracedNativeMethods.OCIStmtExecute(serviceContextHandle, ociStatementHandle, errorHandle, 1, OCI.MODE.OCI_DEFAULT);
							if (num != 0)
							{
								connection.CheckError(errorHandle, num);
							}
							if (scratchBuffer.ReadInt16(num5) != -1)
							{
								oracleString = new OracleString(scratchBuffer, num7, num6, MetaType.GetMetaTypeForType(OracleType.RowId), connection, false, true);
								GC.KeepAlive(rowidHandle);
							}
						}
						finally
						{
							OciHandle.SafeDispose(ref ociStatementHandle);
						}
					}
				}
				finally
				{
					if (flag2)
					{
						rowidHandle.DangerousRelease();
					}
					if (flag)
					{
						scratchBuffer.DangerousRelease();
					}
				}
			}
			return oracleString;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0005F1E4 File Offset: 0x0005E5E4
		private OciStatementHandle GetStatementHandle()
		{
			if (this.ConnectionIsClosed)
			{
				throw ADP.ClosedConnectionError();
			}
			if (this._preparedStatementHandle != null)
			{
				if (this._connection.CloseCount == this._preparedAtCloseCount)
				{
					return this._preparedStatementHandle;
				}
				this._preparedStatementHandle.Dispose();
				this._preparedStatementHandle = null;
			}
			return new OciStatementHandle(this.ServiceContextHandle);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0005F240 File Offset: 0x0005E640
		internal static bool IsBehavior(CommandBehavior value, CommandBehavior condition)
		{
			return condition == (condition & value);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0005F254 File Offset: 0x0005E654
		public override void Prepare()
		{
			OracleConnection.ExecutePermission.Demand();
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ora.OracleCommand.Prepare|API> %d#\n", this.ObjectID);
			try
			{
				if (this.ConnectionIsClosed)
				{
					throw ADP.ClosedConnectionError();
				}
				if (CommandType.Text == this.CommandType)
				{
					OciStatementHandle statementHandle = this.GetStatementHandle();
					int closeCount = this._connection.CloseCount;
					string statementText = this.StatementText;
					int num = TracedNativeMethods.OCIStmtPrepare(statementHandle, this.ErrorHandle, statementText, OCI.SYNTAX.OCI_NTV_SYNTAX, OCI.MODE.OCI_DEFAULT, this.Connection);
					if (num != 0)
					{
						this.Connection.CheckError(this.ErrorHandle, num);
					}
					short num2;
					statementHandle.GetAttribute(OCI.ATTR.OCI_ATTR_STMT_TYPE, out num2, this.ErrorHandle);
					this._statementType = (OCI.STMT)num2;
					if (OCI.STMT.OCI_STMT_SELECT == this._statementType)
					{
						num = TracedNativeMethods.OCIStmtExecute(this._connection.ServiceContextHandle, statementHandle, this.ErrorHandle, 0, OCI.MODE.OCI_SHARED);
						if (num != 0)
						{
							this.Connection.CheckError(this.ErrorHandle, num);
						}
					}
					if (statementHandle != this._preparedStatementHandle)
					{
						OciHandle.SafeDispose(ref this._preparedStatementHandle);
					}
					this._preparedStatementHandle = statementHandle;
					this._preparedAtCloseCount = closeCount;
				}
				else if (this._preparedStatementHandle != null)
				{
					OciHandle.SafeDispose(ref this._preparedStatementHandle);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0005F38C File Offset: 0x0005E78C
		private void PropertyChanging()
		{
			if (this._preparedStatementHandle != null)
			{
				this._preparedStatementHandle.Dispose();
				this._preparedStatementHandle = null;
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0005F3B4 File Offset: 0x0005E7B4
		private void ReleaseStatementHandle(OciStatementHandle statementHandle)
		{
			if (this.Connection.State != ConnectionState.Closed && this._preparedStatementHandle != statementHandle)
			{
				OciHandle.SafeDispose(ref statementHandle);
			}
		}

		// Token: 0x0400034F RID: 847
		private static int _objectTypeCount;

		// Token: 0x04000350 RID: 848
		internal readonly int _objectID = Interlocked.Increment(ref OracleCommand._objectTypeCount);

		// Token: 0x04000351 RID: 849
		private string _commandText;

		// Token: 0x04000352 RID: 850
		private CommandType _commandType;

		// Token: 0x04000353 RID: 851
		private UpdateRowSource _updatedRowSource = UpdateRowSource.Both;

		// Token: 0x04000354 RID: 852
		private bool _designTimeInvisible;

		// Token: 0x04000355 RID: 853
		private OracleConnection _connection;

		// Token: 0x04000356 RID: 854
		private OciStatementHandle _preparedStatementHandle;

		// Token: 0x04000357 RID: 855
		private int _preparedAtCloseCount;

		// Token: 0x04000358 RID: 856
		private OracleParameterCollection _parameterCollection;

		// Token: 0x04000359 RID: 857
		private OCI.STMT _statementType;

		// Token: 0x0400035A RID: 858
		private OracleTransaction _transaction;
	}
}
