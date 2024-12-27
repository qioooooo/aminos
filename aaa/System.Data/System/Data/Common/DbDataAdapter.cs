using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.ProviderBase;

namespace System.Data.Common
{
	// Token: 0x02000132 RID: 306
	public abstract class DbDataAdapter : DataAdapter, IDbDataAdapter, IDataAdapter, ICloneable
	{
		// Token: 0x0600141B RID: 5147 RVA: 0x002254DC File Offset: 0x002248DC
		protected DbDataAdapter()
		{
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x002254F0 File Offset: 0x002248F0
		protected DbDataAdapter(DbDataAdapter adapter)
			: base(adapter)
		{
			this.CloneFrom(adapter);
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x0022550C File Offset: 0x0022490C
		private IDbDataAdapter _IDbDataAdapter
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x0022551C File Offset: 0x0022491C
		// (set) Token: 0x0600141F RID: 5151 RVA: 0x0022553C File Offset: 0x0022493C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DbCommand DeleteCommand
		{
			get
			{
				return (DbCommand)this._IDbDataAdapter.DeleteCommand;
			}
			set
			{
				this._IDbDataAdapter.DeleteCommand = value;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x00225558 File Offset: 0x00224958
		// (set) Token: 0x06001421 RID: 5153 RVA: 0x0022556C File Offset: 0x0022496C
		IDbCommand IDbDataAdapter.DeleteCommand
		{
			get
			{
				return this._deleteCommand;
			}
			set
			{
				this._deleteCommand = value;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x00225580 File Offset: 0x00224980
		// (set) Token: 0x06001423 RID: 5155 RVA: 0x00225598 File Offset: 0x00224998
		protected internal CommandBehavior FillCommandBehavior
		{
			get
			{
				return this._fillCommandBehavior | CommandBehavior.SequentialAccess;
			}
			set
			{
				this._fillCommandBehavior = value | CommandBehavior.SequentialAccess;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x002255B0 File Offset: 0x002249B0
		// (set) Token: 0x06001425 RID: 5157 RVA: 0x002255D0 File Offset: 0x002249D0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DbCommand InsertCommand
		{
			get
			{
				return (DbCommand)this._IDbDataAdapter.InsertCommand;
			}
			set
			{
				this._IDbDataAdapter.InsertCommand = value;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x002255EC File Offset: 0x002249EC
		// (set) Token: 0x06001427 RID: 5159 RVA: 0x00225600 File Offset: 0x00224A00
		IDbCommand IDbDataAdapter.InsertCommand
		{
			get
			{
				return this._insertCommand;
			}
			set
			{
				this._insertCommand = value;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x00225614 File Offset: 0x00224A14
		// (set) Token: 0x06001429 RID: 5161 RVA: 0x00225634 File Offset: 0x00224A34
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DbCommand SelectCommand
		{
			get
			{
				return (DbCommand)this._IDbDataAdapter.SelectCommand;
			}
			set
			{
				this._IDbDataAdapter.SelectCommand = value;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x00225650 File Offset: 0x00224A50
		// (set) Token: 0x0600142B RID: 5163 RVA: 0x00225664 File Offset: 0x00224A64
		IDbCommand IDbDataAdapter.SelectCommand
		{
			get
			{
				return this._selectCommand;
			}
			set
			{
				this._selectCommand = value;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x00225678 File Offset: 0x00224A78
		// (set) Token: 0x0600142D RID: 5165 RVA: 0x00225688 File Offset: 0x00224A88
		[DefaultValue(1)]
		[ResDescription("DbDataAdapter_UpdateBatchSize")]
		[ResCategory("DataCategory_Update")]
		public virtual int UpdateBatchSize
		{
			get
			{
				return 1;
			}
			set
			{
				if (1 != value)
				{
					throw ADP.NotSupported();
				}
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x002256A0 File Offset: 0x00224AA0
		// (set) Token: 0x0600142F RID: 5167 RVA: 0x002256C0 File Offset: 0x00224AC0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DbCommand UpdateCommand
		{
			get
			{
				return (DbCommand)this._IDbDataAdapter.UpdateCommand;
			}
			set
			{
				this._IDbDataAdapter.UpdateCommand = value;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x002256DC File Offset: 0x00224ADC
		// (set) Token: 0x06001431 RID: 5169 RVA: 0x002256F0 File Offset: 0x00224AF0
		IDbCommand IDbDataAdapter.UpdateCommand
		{
			get
			{
				return this._updateCommand;
			}
			set
			{
				this._updateCommand = value;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x00225704 File Offset: 0x00224B04
		private MissingMappingAction UpdateMappingAction
		{
			get
			{
				if (MissingMappingAction.Passthrough == base.MissingMappingAction)
				{
					return MissingMappingAction.Passthrough;
				}
				return MissingMappingAction.Error;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x00225720 File Offset: 0x00224B20
		private MissingSchemaAction UpdateSchemaAction
		{
			get
			{
				MissingSchemaAction missingSchemaAction = base.MissingSchemaAction;
				if (MissingSchemaAction.Add == missingSchemaAction || MissingSchemaAction.AddWithKey == missingSchemaAction)
				{
					return MissingSchemaAction.Ignore;
				}
				return MissingSchemaAction.Error;
			}
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x00225740 File Offset: 0x00224B40
		protected virtual int AddToBatch(IDbCommand command)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x00225754 File Offset: 0x00224B54
		protected virtual void ClearBatch()
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x00225768 File Offset: 0x00224B68
		object ICloneable.Clone()
		{
			DbDataAdapter dbDataAdapter = (DbDataAdapter)this.CloneInternals();
			dbDataAdapter.CloneFrom(this);
			return dbDataAdapter;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0022578C File Offset: 0x00224B8C
		private void CloneFrom(DbDataAdapter from)
		{
			IDbDataAdapter idbDataAdapter = from._IDbDataAdapter;
			this._IDbDataAdapter.SelectCommand = this.CloneCommand(idbDataAdapter.SelectCommand);
			this._IDbDataAdapter.InsertCommand = this.CloneCommand(idbDataAdapter.InsertCommand);
			this._IDbDataAdapter.UpdateCommand = this.CloneCommand(idbDataAdapter.UpdateCommand);
			this._IDbDataAdapter.DeleteCommand = this.CloneCommand(idbDataAdapter.DeleteCommand);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x002257FC File Offset: 0x00224BFC
		private IDbCommand CloneCommand(IDbCommand command)
		{
			return (IDbCommand)((command is ICloneable) ? ((ICloneable)command).Clone() : null);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x00225824 File Offset: 0x00224C24
		protected virtual RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new RowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0022583C File Offset: 0x00224C3C
		protected virtual RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new RowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x00225854 File Offset: 0x00224C54
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((IDbDataAdapter)this).SelectCommand = null;
				((IDbDataAdapter)this).InsertCommand = null;
				((IDbDataAdapter)this).UpdateCommand = null;
				((IDbDataAdapter)this).DeleteCommand = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0022588C File Offset: 0x00224C8C
		protected virtual int ExecuteBatch()
		{
			throw ADP.NotSupported();
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x002258A0 File Offset: 0x00224CA0
		public DataTable FillSchema(DataTable dataTable, SchemaType schemaType)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.FillSchema|API> %d#, dataTable, schemaType=%d{ds.SchemaType}\n", base.ObjectID, (int)schemaType);
			DataTable dataTable2;
			try
			{
				IDbCommand selectCommand = this._IDbDataAdapter.SelectCommand;
				CommandBehavior fillCommandBehavior = this.FillCommandBehavior;
				dataTable2 = this.FillSchema(dataTable, schemaType, selectCommand, fillCommandBehavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable2;
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x00225908 File Offset: 0x00224D08
		public override DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.FillSchema|API> %d#, dataSet, schemaType=%d{ds.SchemaType}\n", base.ObjectID, (int)schemaType);
			DataTable[] array;
			try
			{
				IDbCommand selectCommand = this._IDbDataAdapter.SelectCommand;
				if (base.DesignMode && (selectCommand == null || selectCommand.Connection == null || ADP.IsEmpty(selectCommand.CommandText)))
				{
					array = new DataTable[0];
				}
				else
				{
					CommandBehavior fillCommandBehavior = this.FillCommandBehavior;
					array = this.FillSchema(dataSet, schemaType, selectCommand, "Table", fillCommandBehavior);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return array;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0022599C File Offset: 0x00224D9C
		public DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType, string srcTable)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.FillSchema|API> %d#, dataSet, schemaType=%d{ds.SchemaType}, srcTable=%ls%\n", base.ObjectID, (int)schemaType, srcTable);
			DataTable[] array;
			try
			{
				IDbCommand selectCommand = this._IDbDataAdapter.SelectCommand;
				CommandBehavior fillCommandBehavior = this.FillCommandBehavior;
				array = this.FillSchema(dataSet, schemaType, selectCommand, srcTable, fillCommandBehavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return array;
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x00225A04 File Offset: 0x00224E04
		protected virtual DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType, IDbCommand command, string srcTable, CommandBehavior behavior)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.FillSchema|API> %d#, dataSet, schemaType, command, srcTable, behavior=%d{ds.CommandBehavior}\n", base.ObjectID, (int)behavior);
			DataTable[] array;
			try
			{
				if (dataSet == null)
				{
					throw ADP.ArgumentNull("dataSet");
				}
				if (SchemaType.Source != schemaType && SchemaType.Mapped != schemaType)
				{
					throw ADP.InvalidSchemaType(schemaType);
				}
				if (ADP.IsEmpty(srcTable))
				{
					throw ADP.FillSchemaRequiresSourceTableName("srcTable");
				}
				if (command == null)
				{
					throw ADP.MissingSelectCommand("FillSchema");
				}
				array = (DataTable[])this.FillSchemaInternal(dataSet, null, schemaType, command, srcTable, behavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return array;
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x00225AA0 File Offset: 0x00224EA0
		protected virtual DataTable FillSchema(DataTable dataTable, SchemaType schemaType, IDbCommand command, CommandBehavior behavior)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.FillSchema|API> %d#, dataTable, schemaType, command, behavior=%d{ds.CommandBehavior}\n", base.ObjectID, (int)behavior);
			DataTable dataTable2;
			try
			{
				if (dataTable == null)
				{
					throw ADP.ArgumentNull("dataTable");
				}
				if (SchemaType.Source != schemaType && SchemaType.Mapped != schemaType)
				{
					throw ADP.InvalidSchemaType(schemaType);
				}
				if (command == null)
				{
					throw ADP.MissingSelectCommand("FillSchema");
				}
				string text = dataTable.TableName;
				int num = base.IndexOfDataSetTable(text);
				if (-1 != num)
				{
					text = base.TableMappings[num].SourceTable;
				}
				dataTable2 = (DataTable)this.FillSchemaInternal(null, dataTable, schemaType, command, text, behavior | CommandBehavior.SingleResult);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return dataTable2;
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00225B50 File Offset: 0x00224F50
		private object FillSchemaInternal(DataSet dataset, DataTable datatable, SchemaType schemaType, IDbCommand command, string srcTable, CommandBehavior behavior)
		{
			object obj = null;
			bool flag = null == command.Connection;
			try
			{
				IDbConnection connection = DbDataAdapter.GetConnection3(this, command, "FillSchema");
				ConnectionState connectionState = ConnectionState.Open;
				try
				{
					DbDataAdapter.QuietOpen(connection, out connectionState);
					using (IDataReader dataReader = command.ExecuteReader(behavior | CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
					{
						if (datatable != null)
						{
							obj = this.FillSchema(datatable, schemaType, dataReader);
						}
						else
						{
							obj = this.FillSchema(dataset, schemaType, srcTable, dataReader);
						}
					}
				}
				finally
				{
					DbDataAdapter.QuietClose(connection, connectionState);
				}
			}
			finally
			{
				if (flag)
				{
					command.Transaction = null;
					command.Connection = null;
				}
			}
			return obj;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00225C28 File Offset: 0x00225028
		public override int Fill(DataSet dataSet)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Fill|API> %d#, dataSet\n", base.ObjectID);
			int num;
			try
			{
				IDbCommand selectCommand = this._IDbDataAdapter.SelectCommand;
				CommandBehavior fillCommandBehavior = this.FillCommandBehavior;
				num = this.Fill(dataSet, 0, 0, "Table", selectCommand, fillCommandBehavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00225C94 File Offset: 0x00225094
		public int Fill(DataSet dataSet, string srcTable)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Fill|API> %d#, dataSet, srcTable='%ls'\n", base.ObjectID, srcTable);
			int num;
			try
			{
				IDbCommand selectCommand = this._IDbDataAdapter.SelectCommand;
				CommandBehavior fillCommandBehavior = this.FillCommandBehavior;
				num = this.Fill(dataSet, 0, 0, srcTable, selectCommand, fillCommandBehavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00225CFC File Offset: 0x002250FC
		public int Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Fill|API> %d#, dataSet, startRecord=%d, maxRecords=%d, srcTable='%ls'\n", base.ObjectID, startRecord, maxRecords, srcTable);
			int num;
			try
			{
				IDbCommand selectCommand = this._IDbDataAdapter.SelectCommand;
				CommandBehavior fillCommandBehavior = this.FillCommandBehavior;
				num = this.Fill(dataSet, startRecord, maxRecords, srcTable, selectCommand, fillCommandBehavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00225D68 File Offset: 0x00225168
		protected virtual int Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable, IDbCommand command, CommandBehavior behavior)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Fill|API> %d#, dataSet, startRecord, maxRecords, srcTable, command, behavior=%d{ds.CommandBehavior}\n", base.ObjectID, (int)behavior);
			int num;
			try
			{
				if (dataSet == null)
				{
					throw ADP.FillRequires("dataSet");
				}
				if (startRecord < 0)
				{
					throw ADP.InvalidStartRecord("startRecord", startRecord);
				}
				if (maxRecords < 0)
				{
					throw ADP.InvalidMaxRecords("maxRecords", maxRecords);
				}
				if (ADP.IsEmpty(srcTable))
				{
					throw ADP.FillRequiresSourceTableName("srcTable");
				}
				if (command == null)
				{
					throw ADP.MissingSelectCommand("Fill");
				}
				num = this.FillInternal(dataSet, null, startRecord, maxRecords, srcTable, command, behavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00225E14 File Offset: 0x00225214
		public int Fill(DataTable dataTable)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Fill|API> %d#, dataTable\n", base.ObjectID);
			int num;
			try
			{
				DataTable[] array = new DataTable[] { dataTable };
				IDbCommand selectCommand = this._IDbDataAdapter.SelectCommand;
				CommandBehavior fillCommandBehavior = this.FillCommandBehavior;
				num = this.Fill(array, 0, 0, selectCommand, fillCommandBehavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00225E88 File Offset: 0x00225288
		public int Fill(int startRecord, int maxRecords, params DataTable[] dataTables)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Fill|API> %d#, startRecord=%d, maxRecords=%d, dataTable[]\n", base.ObjectID, startRecord, maxRecords);
			int num;
			try
			{
				IDbCommand selectCommand = this._IDbDataAdapter.SelectCommand;
				CommandBehavior fillCommandBehavior = this.FillCommandBehavior;
				num = this.Fill(dataTables, startRecord, maxRecords, selectCommand, fillCommandBehavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x00225EF0 File Offset: 0x002252F0
		protected virtual int Fill(DataTable dataTable, IDbCommand command, CommandBehavior behavior)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Fill|API> dataTable, command, behavior=%d{ds.CommandBehavior}%d#\n", base.ObjectID, (int)behavior);
			int num;
			try
			{
				DataTable[] array = new DataTable[] { dataTable };
				num = this.Fill(array, 0, 0, command, behavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00225F50 File Offset: 0x00225350
		protected virtual int Fill(DataTable[] dataTables, int startRecord, int maxRecords, IDbCommand command, CommandBehavior behavior)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Fill|API> %d#, dataTables[], startRecord, maxRecords, command, behavior=%d{ds.CommandBehavior}\n", base.ObjectID, (int)behavior);
			int num;
			try
			{
				if (dataTables == null || dataTables.Length == 0 || dataTables[0] == null)
				{
					throw ADP.FillRequires("dataTable");
				}
				if (startRecord < 0)
				{
					throw ADP.InvalidStartRecord("startRecord", startRecord);
				}
				if (maxRecords < 0)
				{
					throw ADP.InvalidMaxRecords("maxRecords", maxRecords);
				}
				if (1 < dataTables.Length && (startRecord != 0 || maxRecords != 0))
				{
					throw ADP.OnlyOneTableForStartRecordOrMaxRecords();
				}
				if (command == null)
				{
					throw ADP.MissingSelectCommand("Fill");
				}
				if (1 == dataTables.Length)
				{
					behavior |= CommandBehavior.SingleResult;
				}
				num = this.FillInternal(null, dataTables, startRecord, maxRecords, null, command, behavior);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num;
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00226010 File Offset: 0x00225410
		private int FillInternal(DataSet dataset, DataTable[] datatables, int startRecord, int maxRecords, string srcTable, IDbCommand command, CommandBehavior behavior)
		{
			int num = 0;
			bool flag = null == command.Connection;
			try
			{
				IDbConnection connection = DbDataAdapter.GetConnection3(this, command, "Fill");
				ConnectionState connectionState = ConnectionState.Open;
				if (MissingSchemaAction.AddWithKey == base.MissingSchemaAction)
				{
					behavior |= CommandBehavior.KeyInfo;
				}
				try
				{
					DbDataAdapter.QuietOpen(connection, out connectionState);
					behavior |= CommandBehavior.SequentialAccess;
					IDataReader dataReader = null;
					try
					{
						dataReader = command.ExecuteReader(behavior);
						if (datatables != null)
						{
							num = this.Fill(datatables, dataReader, startRecord, maxRecords);
						}
						else
						{
							num = this.Fill(dataset, srcTable, dataReader, startRecord, maxRecords);
						}
					}
					finally
					{
						if (dataReader != null)
						{
							dataReader.Dispose();
						}
					}
				}
				finally
				{
					DbDataAdapter.QuietClose(connection, connectionState);
				}
			}
			finally
			{
				if (flag)
				{
					command.Transaction = null;
					command.Connection = null;
				}
			}
			return num;
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00226100 File Offset: 0x00225500
		protected virtual IDataParameter GetBatchedParameter(int commandIdentifier, int parameterIndex)
		{
			throw ADP.NotSupported();
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00226114 File Offset: 0x00225514
		protected virtual bool GetBatchedRecordsAffected(int commandIdentifier, out int recordsAffected, out Exception error)
		{
			recordsAffected = 1;
			error = null;
			return true;
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00226128 File Offset: 0x00225528
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public override IDataParameter[] GetFillParameters()
		{
			IDataParameter[] array = null;
			IDbCommand selectCommand = this._IDbDataAdapter.SelectCommand;
			if (selectCommand != null)
			{
				IDataParameterCollection parameters = selectCommand.Parameters;
				if (parameters != null)
				{
					array = new IDataParameter[parameters.Count];
					parameters.CopyTo(array, 0);
				}
			}
			if (array == null)
			{
				array = new IDataParameter[0];
			}
			return array;
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x00226170 File Offset: 0x00225570
		internal DataTableMapping GetTableMapping(DataTable dataTable)
		{
			DataTableMapping dataTableMapping = null;
			int num = base.IndexOfDataSetTable(dataTable.TableName);
			if (-1 != num)
			{
				dataTableMapping = base.TableMappings[num];
			}
			if (dataTableMapping == null)
			{
				if (MissingMappingAction.Error == base.MissingMappingAction)
				{
					throw ADP.MissingTableMappingDestination(dataTable.TableName);
				}
				dataTableMapping = new DataTableMapping(dataTable.TableName, dataTable.TableName);
			}
			return dataTableMapping;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x002261C8 File Offset: 0x002255C8
		protected virtual void InitializeBatching()
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x002261DC File Offset: 0x002255DC
		protected virtual void OnRowUpdated(RowUpdatedEventArgs value)
		{
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x002261EC File Offset: 0x002255EC
		protected virtual void OnRowUpdating(RowUpdatingEventArgs value)
		{
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x002261FC File Offset: 0x002255FC
		private void ParameterInput(IDataParameterCollection parameters, StatementType typeIndex, DataRow row, DataTableMapping mappings)
		{
			MissingMappingAction updateMappingAction = this.UpdateMappingAction;
			MissingSchemaAction updateSchemaAction = this.UpdateSchemaAction;
			foreach (object obj in parameters)
			{
				IDataParameter dataParameter = (IDataParameter)obj;
				if (dataParameter != null && (ParameterDirection.Input & dataParameter.Direction) != (ParameterDirection)0)
				{
					string sourceColumn = dataParameter.SourceColumn;
					if (!ADP.IsEmpty(sourceColumn))
					{
						DataColumn dataColumn = mappings.GetDataColumn(sourceColumn, null, row.Table, updateMappingAction, updateSchemaAction);
						if (dataColumn != null)
						{
							DataRowVersion parameterSourceVersion = DbDataAdapter.GetParameterSourceVersion(typeIndex, dataParameter);
							dataParameter.Value = row[dataColumn, parameterSourceVersion];
						}
						else
						{
							dataParameter.Value = null;
						}
						DbParameter dbParameter = dataParameter as DbParameter;
						if (dbParameter != null && dbParameter.SourceColumnNullMapping)
						{
							dataParameter.Value = (ADP.IsNull(dataParameter.Value) ? DbDataAdapter.ParameterValueNullValue : DbDataAdapter.ParameterValueNonNullValue);
						}
					}
				}
			}
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x002262FC File Offset: 0x002256FC
		private void ParameterOutput(IDataParameter parameter, DataRow row, DataTableMapping mappings, MissingMappingAction missingMapping, MissingSchemaAction missingSchema)
		{
			if ((ParameterDirection.Output & parameter.Direction) != (ParameterDirection)0)
			{
				object value = parameter.Value;
				if (value != null)
				{
					string sourceColumn = parameter.SourceColumn;
					if (!ADP.IsEmpty(sourceColumn))
					{
						DataColumn dataColumn = mappings.GetDataColumn(sourceColumn, null, row.Table, missingMapping, missingSchema);
						if (dataColumn != null)
						{
							if (dataColumn.ReadOnly)
							{
								try
								{
									dataColumn.ReadOnly = false;
									row[dataColumn] = value;
									return;
								}
								finally
								{
									dataColumn.ReadOnly = true;
								}
							}
							row[dataColumn] = value;
						}
					}
				}
			}
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x00226388 File Offset: 0x00225788
		private void ParameterOutput(IDataParameterCollection parameters, DataRow row, DataTableMapping mappings)
		{
			MissingMappingAction updateMappingAction = this.UpdateMappingAction;
			MissingSchemaAction updateSchemaAction = this.UpdateSchemaAction;
			foreach (object obj in parameters)
			{
				IDataParameter dataParameter = (IDataParameter)obj;
				if (dataParameter != null)
				{
					this.ParameterOutput(dataParameter, row, mappings, updateMappingAction, updateSchemaAction);
				}
			}
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x00226400 File Offset: 0x00225800
		protected virtual void TerminateBatching()
		{
			throw ADP.NotSupported();
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x00226414 File Offset: 0x00225814
		public override int Update(DataSet dataSet)
		{
			return this.Update(dataSet, "Table");
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x00226430 File Offset: 0x00225830
		public int Update(DataRow[] dataRows)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Update|API> %d#, dataRows[]\n", base.ObjectID);
			int num2;
			try
			{
				if (dataRows == null)
				{
					throw ADP.ArgumentNull("dataRows");
				}
				int num = 0;
				if (dataRows != null || dataRows.Length != 0)
				{
					DataTable dataTable = null;
					for (int i = 0; i < dataRows.Length; i++)
					{
						if (dataRows[i] != null && dataTable != dataRows[i].Table)
						{
							if (dataTable != null)
							{
								throw ADP.UpdateMismatchRowTable(i);
							}
							dataTable = dataRows[i].Table;
						}
					}
					if (dataTable != null)
					{
						DataTableMapping tableMapping = this.GetTableMapping(dataTable);
						num = this.Update(dataRows, tableMapping);
					}
				}
				num2 = num;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num2;
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x002264DC File Offset: 0x002258DC
		public int Update(DataTable dataTable)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Update|API> %d#, dataTable", base.ObjectID);
			int num2;
			try
			{
				if (dataTable == null)
				{
					throw ADP.UpdateRequiresDataTable("dataTable");
				}
				DataTableMapping dataTableMapping = null;
				int num = base.IndexOfDataSetTable(dataTable.TableName);
				if (-1 != num)
				{
					dataTableMapping = base.TableMappings[num];
				}
				if (dataTableMapping == null)
				{
					if (MissingMappingAction.Error == base.MissingMappingAction)
					{
						throw ADP.MissingTableMappingDestination(dataTable.TableName);
					}
					dataTableMapping = new DataTableMapping("Table", dataTable.TableName);
				}
				num2 = this.UpdateFromDataTable(dataTable, dataTableMapping);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num2;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00226584 File Offset: 0x00225984
		public int Update(DataSet dataSet, string srcTable)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Update|API> %d#, dataSet, srcTable='%ls'", base.ObjectID, srcTable);
			int num2;
			try
			{
				if (dataSet == null)
				{
					throw ADP.UpdateRequiresNonNullDataSet("dataSet");
				}
				if (ADP.IsEmpty(srcTable))
				{
					throw ADP.UpdateRequiresSourceTableName("srcTable");
				}
				int num = 0;
				MissingMappingAction updateMappingAction = this.UpdateMappingAction;
				DataTableMapping tableMappingBySchemaAction = base.GetTableMappingBySchemaAction(srcTable, srcTable, this.UpdateMappingAction);
				MissingSchemaAction updateSchemaAction = this.UpdateSchemaAction;
				DataTable dataTableBySchemaAction = tableMappingBySchemaAction.GetDataTableBySchemaAction(dataSet, updateSchemaAction);
				if (dataTableBySchemaAction != null)
				{
					num = this.UpdateFromDataTable(dataTableBySchemaAction, tableMappingBySchemaAction);
				}
				else if (!base.HasTableMappings() || -1 == base.TableMappings.IndexOf(tableMappingBySchemaAction))
				{
					throw ADP.UpdateRequiresSourceTable(srcTable);
				}
				num2 = num;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num2;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00226644 File Offset: 0x00225A44
		protected virtual int Update(DataRow[] dataRows, DataTableMapping tableMapping)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<comm.DbDataAdapter.Update|API> %d#, dataRows[], tableMapping", base.ObjectID);
			int num4;
			try
			{
				int num = 0;
				IDbConnection[] array = new IDbConnection[5];
				ConnectionState[] array2 = new ConnectionState[5];
				bool flag = false;
				IDbCommand selectCommand = this._IDbDataAdapter.SelectCommand;
				if (selectCommand != null)
				{
					array[0] = selectCommand.Connection;
					if (array[0] != null)
					{
						array2[0] = array[0].State;
						flag = true;
					}
				}
				int num2 = Math.Min(this.UpdateBatchSize, dataRows.Length);
				if (num2 < 1)
				{
					num2 = dataRows.Length;
				}
				DbDataAdapter.BatchCommandInfo[] array3 = new DbDataAdapter.BatchCommandInfo[num2];
				DataRow[] array4 = new DataRow[num2];
				int num3 = 0;
				try
				{
					try
					{
						if (1 != num2)
						{
							this.InitializeBatching();
						}
						StatementType statementType = StatementType.Select;
						IDbCommand dbCommand = null;
						foreach (DataRow dataRow in dataRows)
						{
							if (dataRow != null)
							{
								bool flag2 = false;
								DataRowState rowState = dataRow.RowState;
								switch (rowState)
								{
								case DataRowState.Detached:
								case DataRowState.Unchanged:
									goto IL_052F;
								case DataRowState.Detached | DataRowState.Unchanged:
									goto IL_0115;
								case DataRowState.Added:
									statementType = StatementType.Insert;
									dbCommand = this._IDbDataAdapter.InsertCommand;
									break;
								default:
									if (rowState != DataRowState.Deleted)
									{
										if (rowState != DataRowState.Modified)
										{
											goto IL_0115;
										}
										statementType = StatementType.Update;
										dbCommand = this._IDbDataAdapter.UpdateCommand;
									}
									else
									{
										statementType = StatementType.Delete;
										dbCommand = this._IDbDataAdapter.DeleteCommand;
									}
									break;
								}
								RowUpdatingEventArgs rowUpdatingEventArgs = this.CreateRowUpdatingEvent(dataRow, dbCommand, statementType, tableMapping);
								try
								{
									dataRow.RowError = null;
									if (dbCommand != null)
									{
										this.ParameterInput(dbCommand.Parameters, statementType, dataRow, tableMapping);
									}
								}
								catch (Exception ex)
								{
									if (!ADP.IsCatchableExceptionType(ex))
									{
										throw;
									}
									ADP.TraceExceptionForCapture(ex);
									rowUpdatingEventArgs.Errors = ex;
									rowUpdatingEventArgs.Status = UpdateStatus.ErrorsOccurred;
								}
								this.OnRowUpdating(rowUpdatingEventArgs);
								IDbCommand command = rowUpdatingEventArgs.Command;
								flag2 = dbCommand != command;
								dbCommand = command;
								UpdateStatus status = rowUpdatingEventArgs.Status;
								if (status != UpdateStatus.Continue)
								{
									if (UpdateStatus.ErrorsOccurred == status)
									{
										this.UpdatingRowStatusErrors(rowUpdatingEventArgs, dataRow);
										goto IL_052F;
									}
									if (UpdateStatus.SkipCurrentRow == status)
									{
										if (DataRowState.Unchanged == dataRow.RowState)
										{
											num++;
											goto IL_052F;
										}
										goto IL_052F;
									}
									else
									{
										if (UpdateStatus.SkipAllRemainingRows != status)
										{
											throw ADP.InvalidUpdateStatus(status);
										}
										if (DataRowState.Unchanged == dataRow.RowState)
										{
											num++;
											break;
										}
										break;
									}
								}
								else
								{
									rowUpdatingEventArgs = null;
									RowUpdatedEventArgs rowUpdatedEventArgs = null;
									if (1 == num2)
									{
										if (dbCommand != null)
										{
											array3[0].CommandIdentifier = 0;
											array3[0].ParameterCount = dbCommand.Parameters.Count;
											array3[0].StatementType = statementType;
											array3[0].UpdatedRowSource = dbCommand.UpdatedRowSource;
										}
										array3[0].Row = dataRow;
										array4[0] = dataRow;
										num3 = 1;
									}
									else
									{
										Exception ex2 = null;
										try
										{
											if (dbCommand != null)
											{
												if ((UpdateRowSource.FirstReturnedRecord & dbCommand.UpdatedRowSource) == UpdateRowSource.None)
												{
													array3[num3].CommandIdentifier = this.AddToBatch(dbCommand);
													array3[num3].ParameterCount = dbCommand.Parameters.Count;
													array3[num3].Row = dataRow;
													array3[num3].StatementType = statementType;
													array3[num3].UpdatedRowSource = dbCommand.UpdatedRowSource;
													array4[num3] = dataRow;
													num3++;
													if (num3 < num2)
													{
														goto IL_052F;
													}
												}
												else
												{
													ex2 = ADP.ResultsNotAllowedDuringBatch();
												}
											}
											else
											{
												ex2 = ADP.UpdateRequiresCommand(statementType, flag2);
											}
										}
										catch (Exception ex3)
										{
											if (!ADP.IsCatchableExceptionType(ex3))
											{
												throw;
											}
											ADP.TraceExceptionForCapture(ex3);
											ex2 = ex3;
										}
										if (ex2 != null)
										{
											rowUpdatedEventArgs = this.CreateRowUpdatedEvent(dataRow, dbCommand, StatementType.Batch, tableMapping);
											rowUpdatedEventArgs.Errors = ex2;
											rowUpdatedEventArgs.Status = UpdateStatus.ErrorsOccurred;
											this.OnRowUpdated(rowUpdatedEventArgs);
											if (ex2 != rowUpdatedEventArgs.Errors)
											{
												for (int j = 0; j < array3.Length; j++)
												{
													array3[j].Errors = null;
												}
											}
											num += this.UpdatedRowStatus(rowUpdatedEventArgs, array3, num3);
											if (UpdateStatus.SkipAllRemainingRows == rowUpdatedEventArgs.Status)
											{
												break;
											}
											goto IL_052F;
										}
									}
									rowUpdatedEventArgs = this.CreateRowUpdatedEvent(dataRow, dbCommand, statementType, tableMapping);
									try
									{
										if (1 != num2)
										{
											IDbConnection connection = DbDataAdapter.GetConnection1(this);
											ConnectionState connectionState = this.UpdateConnectionOpen(connection, StatementType.Batch, array, array2, flag);
											rowUpdatedEventArgs.AdapterInit(array4);
											if (ConnectionState.Open == connectionState)
											{
												this.UpdateBatchExecute(array3, num3, rowUpdatedEventArgs);
											}
											else
											{
												rowUpdatedEventArgs.Errors = ADP.UpdateOpenConnectionRequired(StatementType.Batch, false, connectionState);
												rowUpdatedEventArgs.Status = UpdateStatus.ErrorsOccurred;
											}
										}
										else if (dbCommand != null)
										{
											IDbConnection connection2 = DbDataAdapter.GetConnection4(this, dbCommand, statementType, flag2);
											ConnectionState connectionState2 = this.UpdateConnectionOpen(connection2, statementType, array, array2, flag);
											if (ConnectionState.Open == connectionState2)
											{
												this.UpdateRowExecute(rowUpdatedEventArgs, dbCommand, statementType);
												array3[0].RecordsAffected = new int?(rowUpdatedEventArgs.RecordsAffected);
												array3[0].Errors = null;
											}
											else
											{
												rowUpdatedEventArgs.Errors = ADP.UpdateOpenConnectionRequired(statementType, flag2, connectionState2);
												rowUpdatedEventArgs.Status = UpdateStatus.ErrorsOccurred;
											}
										}
										else
										{
											rowUpdatedEventArgs.Errors = ADP.UpdateRequiresCommand(statementType, flag2);
											rowUpdatedEventArgs.Status = UpdateStatus.ErrorsOccurred;
										}
									}
									catch (Exception ex4)
									{
										if (!ADP.IsCatchableExceptionType(ex4))
										{
											throw;
										}
										ADP.TraceExceptionForCapture(ex4);
										rowUpdatedEventArgs.Errors = ex4;
										rowUpdatedEventArgs.Status = UpdateStatus.ErrorsOccurred;
									}
									bool flag3 = UpdateStatus.ErrorsOccurred == rowUpdatedEventArgs.Status;
									Exception errors = rowUpdatedEventArgs.Errors;
									this.OnRowUpdated(rowUpdatedEventArgs);
									if (errors != rowUpdatedEventArgs.Errors)
									{
										for (int k = 0; k < array3.Length; k++)
										{
											array3[k].Errors = null;
										}
									}
									num += this.UpdatedRowStatus(rowUpdatedEventArgs, array3, num3);
									if (UpdateStatus.SkipAllRemainingRows != rowUpdatedEventArgs.Status)
									{
										if (1 != num2)
										{
											this.ClearBatch();
											num3 = 0;
										}
										for (int l = 0; l < array3.Length; l++)
										{
											array3[l] = default(DbDataAdapter.BatchCommandInfo);
										}
										num3 = 0;
										goto IL_052F;
									}
									if (flag3 && 1 != num2)
									{
										this.ClearBatch();
										num3 = 0;
										break;
									}
									break;
								}
								IL_0115:
								throw ADP.InvalidDataRowState(dataRow.RowState);
							}
							IL_052F:;
						}
						if (1 != num2 && 0 < num3)
						{
							RowUpdatedEventArgs rowUpdatedEventArgs2 = this.CreateRowUpdatedEvent(null, dbCommand, statementType, tableMapping);
							try
							{
								IDbConnection connection3 = DbDataAdapter.GetConnection1(this);
								ConnectionState connectionState3 = this.UpdateConnectionOpen(connection3, StatementType.Batch, array, array2, flag);
								DataRow[] array5 = array4;
								if (num3 < array4.Length)
								{
									array5 = new DataRow[num3];
									Array.Copy(array4, array5, num3);
								}
								rowUpdatedEventArgs2.AdapterInit(array5);
								if (ConnectionState.Open == connectionState3)
								{
									this.UpdateBatchExecute(array3, num3, rowUpdatedEventArgs2);
								}
								else
								{
									rowUpdatedEventArgs2.Errors = ADP.UpdateOpenConnectionRequired(StatementType.Batch, false, connectionState3);
									rowUpdatedEventArgs2.Status = UpdateStatus.ErrorsOccurred;
								}
							}
							catch (Exception ex5)
							{
								if (!ADP.IsCatchableExceptionType(ex5))
								{
									throw;
								}
								ADP.TraceExceptionForCapture(ex5);
								rowUpdatedEventArgs2.Errors = ex5;
								rowUpdatedEventArgs2.Status = UpdateStatus.ErrorsOccurred;
							}
							Exception errors2 = rowUpdatedEventArgs2.Errors;
							this.OnRowUpdated(rowUpdatedEventArgs2);
							if (errors2 != rowUpdatedEventArgs2.Errors)
							{
								for (int m = 0; m < array3.Length; m++)
								{
									array3[m].Errors = null;
								}
							}
							num += this.UpdatedRowStatus(rowUpdatedEventArgs2, array3, num3);
						}
					}
					finally
					{
						if (1 != num2)
						{
							this.TerminateBatching();
						}
					}
				}
				finally
				{
					for (int n = 0; n < array.Length; n++)
					{
						DbDataAdapter.QuietClose(array[n], array2[n]);
					}
				}
				num4 = num;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return num4;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00226D78 File Offset: 0x00226178
		private void UpdateBatchExecute(DbDataAdapter.BatchCommandInfo[] batchCommands, int commandCount, RowUpdatedEventArgs rowUpdatedEvent)
		{
			try
			{
				int num = this.ExecuteBatch();
				rowUpdatedEvent.AdapterInit(num);
			}
			catch (DbException ex)
			{
				ADP.TraceExceptionForCapture(ex);
				rowUpdatedEvent.Errors = ex;
				rowUpdatedEvent.Status = UpdateStatus.ErrorsOccurred;
			}
			MissingMappingAction updateMappingAction = this.UpdateMappingAction;
			MissingSchemaAction updateSchemaAction = this.UpdateSchemaAction;
			int num2 = 0;
			bool flag = false;
			List<DataRow> list = null;
			for (int i = 0; i < commandCount; i++)
			{
				DbDataAdapter.BatchCommandInfo batchCommandInfo = batchCommands[i];
				StatementType statementType = batchCommandInfo.StatementType;
				int num3;
				if (this.GetBatchedRecordsAffected(batchCommandInfo.CommandIdentifier, out num3, out batchCommands[i].Errors))
				{
					batchCommands[i].RecordsAffected = new int?(num3);
				}
				if (batchCommands[i].Errors == null && batchCommands[i].RecordsAffected != null)
				{
					if (StatementType.Update == statementType || StatementType.Delete == statementType)
					{
						num2++;
						if (num3 == 0)
						{
							if (list == null)
							{
								list = new List<DataRow>();
							}
							batchCommands[i].Errors = ADP.UpdateConcurrencyViolation(batchCommands[i].StatementType, 0, 1, new DataRow[] { rowUpdatedEvent.Rows[i] });
							flag = true;
							list.Add(rowUpdatedEvent.Rows[i]);
						}
					}
					if ((StatementType.Insert == statementType || StatementType.Update == statementType) && (UpdateRowSource.OutputParameters & batchCommandInfo.UpdatedRowSource) != UpdateRowSource.None && num3 != 0)
					{
						if (StatementType.Insert == statementType)
						{
							rowUpdatedEvent.Rows[i].AcceptChanges();
						}
						for (int j = 0; j < batchCommandInfo.ParameterCount; j++)
						{
							IDataParameter batchedParameter = this.GetBatchedParameter(batchCommandInfo.CommandIdentifier, j);
							this.ParameterOutput(batchedParameter, batchCommandInfo.Row, rowUpdatedEvent.TableMapping, updateMappingAction, updateSchemaAction);
						}
					}
				}
			}
			if (rowUpdatedEvent.Errors == null && rowUpdatedEvent.Status == UpdateStatus.Continue && 0 < num2 && (rowUpdatedEvent.RecordsAffected == 0 || flag))
			{
				DataRow[] array = ((list != null) ? list.ToArray() : rowUpdatedEvent.Rows);
				rowUpdatedEvent.Errors = ADP.UpdateConcurrencyViolation(StatementType.Batch, commandCount - array.Length, commandCount, array);
				rowUpdatedEvent.Status = UpdateStatus.ErrorsOccurred;
			}
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x00226F78 File Offset: 0x00226378
		private ConnectionState UpdateConnectionOpen(IDbConnection connection, StatementType statementType, IDbConnection[] connections, ConnectionState[] connectionStates, bool useSelectConnectionState)
		{
			if (connection != connections[(int)statementType])
			{
				DbDataAdapter.QuietClose(connections[(int)statementType], connectionStates[(int)statementType]);
				connections[(int)statementType] = connection;
				connectionStates[(int)statementType] = ConnectionState.Closed;
				DbDataAdapter.QuietOpen(connection, out connectionStates[(int)statementType]);
				if (useSelectConnectionState && connections[0] == connection)
				{
					connectionStates[(int)statementType] = connections[0].State;
				}
			}
			return connection.State;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x00226FCC File Offset: 0x002263CC
		private int UpdateFromDataTable(DataTable dataTable, DataTableMapping tableMapping)
		{
			int num = 0;
			DataRow[] array = ADP.SelectAdapterRows(dataTable, false);
			if (array != null && 0 < array.Length)
			{
				num = this.Update(array, tableMapping);
			}
			return num;
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x00226FF8 File Offset: 0x002263F8
		private void UpdateRowExecute(RowUpdatedEventArgs rowUpdatedEvent, IDbCommand dataCommand, StatementType cmdIndex)
		{
			bool flag = true;
			UpdateRowSource updatedRowSource = dataCommand.UpdatedRowSource;
			if (StatementType.Delete == cmdIndex || (UpdateRowSource.FirstReturnedRecord & updatedRowSource) == UpdateRowSource.None)
			{
				int num = dataCommand.ExecuteNonQuery();
				rowUpdatedEvent.AdapterInit(num);
			}
			else if (StatementType.Insert == cmdIndex || StatementType.Update == cmdIndex)
			{
				using (IDataReader dataReader = dataCommand.ExecuteReader(CommandBehavior.SequentialAccess))
				{
					DataReaderContainer dataReaderContainer = DataReaderContainer.Create(dataReader, this.ReturnProviderSpecificTypes);
					try
					{
						bool flag2 = false;
						while (0 >= dataReaderContainer.FieldCount)
						{
							if (!dataReader.NextResult())
							{
								IL_0065:
								if (flag2 && dataReader.RecordsAffected != 0)
								{
									SchemaMapping schemaMapping = new SchemaMapping(this, null, rowUpdatedEvent.Row.Table, dataReaderContainer, false, SchemaType.Mapped, rowUpdatedEvent.TableMapping.SourceTable, true, null, null);
									if (schemaMapping.DataTable != null && schemaMapping.DataValues != null && dataReader.Read())
									{
										if (StatementType.Insert == cmdIndex && flag)
										{
											rowUpdatedEvent.Row.AcceptChanges();
											flag = false;
										}
										schemaMapping.ApplyToDataRow(rowUpdatedEvent.Row);
									}
								}
								goto IL_00E7;
							}
						}
						flag2 = true;
						goto IL_0065;
					}
					finally
					{
						dataReader.Close();
						int recordsAffected = dataReader.RecordsAffected;
						rowUpdatedEvent.AdapterInit(recordsAffected);
					}
					IL_00E7:;
				}
			}
			if ((StatementType.Insert == cmdIndex || StatementType.Update == cmdIndex) && (UpdateRowSource.OutputParameters & updatedRowSource) != UpdateRowSource.None && rowUpdatedEvent.RecordsAffected != 0)
			{
				if (StatementType.Insert == cmdIndex && flag)
				{
					rowUpdatedEvent.Row.AcceptChanges();
				}
				this.ParameterOutput(dataCommand.Parameters, rowUpdatedEvent.Row, rowUpdatedEvent.TableMapping);
			}
			UpdateStatus status = rowUpdatedEvent.Status;
			if (status != UpdateStatus.Continue)
			{
				return;
			}
			switch (cmdIndex)
			{
			case StatementType.Update:
			case StatementType.Delete:
				if (rowUpdatedEvent.RecordsAffected == 0)
				{
					rowUpdatedEvent.Errors = ADP.UpdateConcurrencyViolation(cmdIndex, rowUpdatedEvent.RecordsAffected, 1, new DataRow[] { rowUpdatedEvent.Row });
					rowUpdatedEvent.Status = UpdateStatus.ErrorsOccurred;
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x002271C4 File Offset: 0x002265C4
		private int UpdatedRowStatus(RowUpdatedEventArgs rowUpdatedEvent, DbDataAdapter.BatchCommandInfo[] batchCommands, int commandCount)
		{
			int num;
			switch (rowUpdatedEvent.Status)
			{
			case UpdateStatus.Continue:
				num = this.UpdatedRowStatusContinue(rowUpdatedEvent, batchCommands, commandCount);
				break;
			case UpdateStatus.ErrorsOccurred:
				num = this.UpdatedRowStatusErrors(rowUpdatedEvent, batchCommands, commandCount);
				break;
			case UpdateStatus.SkipCurrentRow:
			case UpdateStatus.SkipAllRemainingRows:
				num = this.UpdatedRowStatusSkip(batchCommands, commandCount);
				break;
			default:
				throw ADP.InvalidUpdateStatus(rowUpdatedEvent.Status);
			}
			return num;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x00227224 File Offset: 0x00226624
		private int UpdatedRowStatusContinue(RowUpdatedEventArgs rowUpdatedEvent, DbDataAdapter.BatchCommandInfo[] batchCommands, int commandCount)
		{
			int num = 0;
			bool acceptChangesDuringUpdate = base.AcceptChangesDuringUpdate;
			for (int i = 0; i < commandCount; i++)
			{
				DataRow row = batchCommands[i].Row;
				if (batchCommands[i].Errors == null && batchCommands[i].RecordsAffected != null && batchCommands[i].RecordsAffected.Value != 0)
				{
					if (acceptChangesDuringUpdate && ((DataRowState.Added | DataRowState.Deleted | DataRowState.Modified) & row.RowState) != (DataRowState)0)
					{
						row.AcceptChanges();
					}
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x002272A0 File Offset: 0x002266A0
		private int UpdatedRowStatusErrors(RowUpdatedEventArgs rowUpdatedEvent, DbDataAdapter.BatchCommandInfo[] batchCommands, int commandCount)
		{
			Exception ex = rowUpdatedEvent.Errors;
			if (ex == null)
			{
				ex = ADP.RowUpdatedErrors();
				rowUpdatedEvent.Errors = ex;
			}
			int num = 0;
			bool flag = false;
			string message = ex.Message;
			for (int i = 0; i < commandCount; i++)
			{
				DataRow row = batchCommands[i].Row;
				if (batchCommands[i].Errors != null)
				{
					string text = batchCommands[i].Errors.Message;
					if (string.IsNullOrEmpty(text))
					{
						text = message;
					}
					DataRow dataRow = row;
					dataRow.RowError += text;
					flag = true;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < commandCount; j++)
				{
					DataRow row2 = batchCommands[j].Row;
					DataRow dataRow2 = row2;
					dataRow2.RowError += message;
				}
			}
			else
			{
				num = this.UpdatedRowStatusContinue(rowUpdatedEvent, batchCommands, commandCount);
			}
			if (!base.ContinueUpdateOnError)
			{
				throw ex;
			}
			return num;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00227378 File Offset: 0x00226778
		private int UpdatedRowStatusSkip(DbDataAdapter.BatchCommandInfo[] batchCommands, int commandCount)
		{
			int num = 0;
			for (int i = 0; i < commandCount; i++)
			{
				DataRow row = batchCommands[i].Row;
				if (((DataRowState.Detached | DataRowState.Unchanged) & row.RowState) != (DataRowState)0)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x002273B0 File Offset: 0x002267B0
		private void UpdatingRowStatusErrors(RowUpdatingEventArgs rowUpdatedEvent, DataRow dataRow)
		{
			Exception ex = rowUpdatedEvent.Errors;
			if (ex == null)
			{
				ex = ADP.RowUpdatingErrors();
				rowUpdatedEvent.Errors = ex;
			}
			string message = ex.Message;
			dataRow.RowError += message;
			if (!base.ContinueUpdateOnError)
			{
				throw ex;
			}
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x002273F8 File Offset: 0x002267F8
		private static IDbConnection GetConnection1(DbDataAdapter adapter)
		{
			IDbCommand dbCommand = adapter._IDbDataAdapter.SelectCommand;
			if (dbCommand == null)
			{
				dbCommand = adapter._IDbDataAdapter.InsertCommand;
				if (dbCommand == null)
				{
					dbCommand = adapter._IDbDataAdapter.UpdateCommand;
					if (dbCommand == null)
					{
						dbCommand = adapter._IDbDataAdapter.DeleteCommand;
					}
				}
			}
			IDbConnection dbConnection = null;
			if (dbCommand != null)
			{
				dbConnection = dbCommand.Connection;
			}
			if (dbConnection == null)
			{
				throw ADP.UpdateConnectionRequired(StatementType.Batch, false);
			}
			return dbConnection;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00227458 File Offset: 0x00226858
		private static IDbConnection GetConnection3(DbDataAdapter adapter, IDbCommand command, string method)
		{
			IDbConnection connection = command.Connection;
			if (connection == null)
			{
				throw ADP.ConnectionRequired_Res(method);
			}
			return connection;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x00227478 File Offset: 0x00226878
		private static IDbConnection GetConnection4(DbDataAdapter adapter, IDbCommand command, StatementType statementType, bool isCommandFromRowUpdating)
		{
			IDbConnection connection = command.Connection;
			if (connection == null)
			{
				throw ADP.UpdateConnectionRequired(statementType, isCommandFromRowUpdating);
			}
			return connection;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00227498 File Offset: 0x00226898
		private static DataRowVersion GetParameterSourceVersion(StatementType statementType, IDataParameter parameter)
		{
			switch (statementType)
			{
			case StatementType.Select:
			case StatementType.Batch:
				throw ADP.UnwantedStatementType(statementType);
			case StatementType.Insert:
				return DataRowVersion.Current;
			case StatementType.Update:
				return parameter.SourceVersion;
			case StatementType.Delete:
				return DataRowVersion.Original;
			default:
				throw ADP.InvalidStatementType(statementType);
			}
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x002274E4 File Offset: 0x002268E4
		private static void QuietClose(IDbConnection connection, ConnectionState originalState)
		{
			if (connection != null && originalState == ConnectionState.Closed)
			{
				connection.Close();
			}
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00227500 File Offset: 0x00226900
		private static void QuietOpen(IDbConnection connection, out ConnectionState originalState)
		{
			originalState = connection.State;
			if (originalState == ConnectionState.Closed)
			{
				connection.Open();
			}
		}

		// Token: 0x04000C2D RID: 3117
		public const string DefaultSourceTableName = "Table";

		// Token: 0x04000C2E RID: 3118
		internal static readonly object ParameterValueNonNullValue = 0;

		// Token: 0x04000C2F RID: 3119
		internal static readonly object ParameterValueNullValue = 1;

		// Token: 0x04000C30 RID: 3120
		private IDbCommand _deleteCommand;

		// Token: 0x04000C31 RID: 3121
		private IDbCommand _insertCommand;

		// Token: 0x04000C32 RID: 3122
		private IDbCommand _selectCommand;

		// Token: 0x04000C33 RID: 3123
		private IDbCommand _updateCommand;

		// Token: 0x04000C34 RID: 3124
		private CommandBehavior _fillCommandBehavior;

		// Token: 0x02000133 RID: 307
		private struct BatchCommandInfo
		{
			// Token: 0x04000C35 RID: 3125
			internal int CommandIdentifier;

			// Token: 0x04000C36 RID: 3126
			internal int ParameterCount;

			// Token: 0x04000C37 RID: 3127
			internal DataRow Row;

			// Token: 0x04000C38 RID: 3128
			internal StatementType StatementType;

			// Token: 0x04000C39 RID: 3129
			internal UpdateRowSource UpdatedRowSource;

			// Token: 0x04000C3A RID: 3130
			internal int? RecordsAffected;

			// Token: 0x04000C3B RID: 3131
			internal Exception Errors;
		}
	}
}
