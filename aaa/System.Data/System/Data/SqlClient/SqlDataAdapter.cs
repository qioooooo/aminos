using System;
using System.ComponentModel;
using System.Data.Common;

namespace System.Data.SqlClient
{
	// Token: 0x020002DD RID: 733
	[ToolboxItem("Microsoft.VSDesigner.Data.VS.SqlDataAdapterToolboxItem, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("RowUpdated")]
	[Designer("Microsoft.VSDesigner.Data.VS.SqlDataAdapterDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class SqlDataAdapter : DbDataAdapter, IDbDataAdapter, IDataAdapter, ICloneable
	{
		// Token: 0x060025AE RID: 9646 RVA: 0x0027D304 File Offset: 0x0027C704
		public SqlDataAdapter()
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x0027D324 File Offset: 0x0027C724
		public SqlDataAdapter(SqlCommand selectCommand)
			: this()
		{
			this.SelectCommand = selectCommand;
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x0027D340 File Offset: 0x0027C740
		public SqlDataAdapter(string selectCommandText, string selectConnectionString)
			: this()
		{
			SqlConnection sqlConnection = new SqlConnection(selectConnectionString);
			this.SelectCommand = new SqlCommand(selectCommandText, sqlConnection);
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x0027D368 File Offset: 0x0027C768
		public SqlDataAdapter(string selectCommandText, SqlConnection selectConnection)
			: this()
		{
			this.SelectCommand = new SqlCommand(selectCommandText, selectConnection);
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x0027D388 File Offset: 0x0027C788
		private SqlDataAdapter(SqlDataAdapter from)
			: base(from)
		{
			GC.SuppressFinalize(this);
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x0027D3AC File Offset: 0x0027C7AC
		// (set) Token: 0x060025B4 RID: 9652 RVA: 0x0027D3C0 File Offset: 0x0027C7C0
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbDataAdapter_DeleteCommand")]
		[DefaultValue(null)]
		public new SqlCommand DeleteCommand
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

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x0027D3D4 File Offset: 0x0027C7D4
		// (set) Token: 0x060025B6 RID: 9654 RVA: 0x0027D3E8 File Offset: 0x0027C7E8
		IDbCommand IDbDataAdapter.DeleteCommand
		{
			get
			{
				return this._deleteCommand;
			}
			set
			{
				this._deleteCommand = (SqlCommand)value;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x0027D404 File Offset: 0x0027C804
		// (set) Token: 0x060025B8 RID: 9656 RVA: 0x0027D418 File Offset: 0x0027C818
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbDataAdapter_InsertCommand")]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue(null)]
		public new SqlCommand InsertCommand
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

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x0027D42C File Offset: 0x0027C82C
		// (set) Token: 0x060025BA RID: 9658 RVA: 0x0027D440 File Offset: 0x0027C840
		IDbCommand IDbDataAdapter.InsertCommand
		{
			get
			{
				return this._insertCommand;
			}
			set
			{
				this._insertCommand = (SqlCommand)value;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x060025BB RID: 9659 RVA: 0x0027D45C File Offset: 0x0027C85C
		// (set) Token: 0x060025BC RID: 9660 RVA: 0x0027D470 File Offset: 0x0027C870
		[DefaultValue(null)]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResCategory("DataCategory_Fill")]
		[ResDescription("DbDataAdapter_SelectCommand")]
		public new SqlCommand SelectCommand
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

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x0027D484 File Offset: 0x0027C884
		// (set) Token: 0x060025BE RID: 9662 RVA: 0x0027D498 File Offset: 0x0027C898
		IDbCommand IDbDataAdapter.SelectCommand
		{
			get
			{
				return this._selectCommand;
			}
			set
			{
				this._selectCommand = (SqlCommand)value;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x0027D4B4 File Offset: 0x0027C8B4
		// (set) Token: 0x060025C0 RID: 9664 RVA: 0x0027D4C8 File Offset: 0x0027C8C8
		public override int UpdateBatchSize
		{
			get
			{
				return this._updateBatchSize;
			}
			set
			{
				if (0 > value)
				{
					throw ADP.ArgumentOutOfRange("UpdateBatchSize");
				}
				this._updateBatchSize = value;
				Bid.Trace("<sc.SqlDataAdapter.set_UpdateBatchSize|API> %d#, %d\n", base.ObjectID, value);
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060025C1 RID: 9665 RVA: 0x0027D4FC File Offset: 0x0027C8FC
		// (set) Token: 0x060025C2 RID: 9666 RVA: 0x0027D510 File Offset: 0x0027C910
		[ResDescription("DbDataAdapter_UpdateCommand")]
		[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue(null)]
		[ResCategory("DataCategory_Update")]
		public new SqlCommand UpdateCommand
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

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060025C3 RID: 9667 RVA: 0x0027D524 File Offset: 0x0027C924
		// (set) Token: 0x060025C4 RID: 9668 RVA: 0x0027D538 File Offset: 0x0027C938
		IDbCommand IDbDataAdapter.UpdateCommand
		{
			get
			{
				return this._updateCommand;
			}
			set
			{
				this._updateCommand = (SqlCommand)value;
			}
		}

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x060025C5 RID: 9669 RVA: 0x0027D554 File Offset: 0x0027C954
		// (remove) Token: 0x060025C6 RID: 9670 RVA: 0x0027D574 File Offset: 0x0027C974
		[ResDescription("DbDataAdapter_RowUpdated")]
		[ResCategory("DataCategory_Update")]
		public event SqlRowUpdatedEventHandler RowUpdated
		{
			add
			{
				base.Events.AddHandler(SqlDataAdapter.EventRowUpdated, value);
			}
			remove
			{
				base.Events.RemoveHandler(SqlDataAdapter.EventRowUpdated, value);
			}
		}

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060025C7 RID: 9671 RVA: 0x0027D594 File Offset: 0x0027C994
		// (remove) Token: 0x060025C8 RID: 9672 RVA: 0x0027D5F8 File Offset: 0x0027C9F8
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbDataAdapter_RowUpdating")]
		public event SqlRowUpdatingEventHandler RowUpdating
		{
			add
			{
				SqlRowUpdatingEventHandler sqlRowUpdatingEventHandler = (SqlRowUpdatingEventHandler)base.Events[SqlDataAdapter.EventRowUpdating];
				if (sqlRowUpdatingEventHandler != null && value.Target is DbCommandBuilder)
				{
					SqlRowUpdatingEventHandler sqlRowUpdatingEventHandler2 = (SqlRowUpdatingEventHandler)ADP.FindBuilder(sqlRowUpdatingEventHandler);
					if (sqlRowUpdatingEventHandler2 != null)
					{
						base.Events.RemoveHandler(SqlDataAdapter.EventRowUpdating, sqlRowUpdatingEventHandler2);
					}
				}
				base.Events.AddHandler(SqlDataAdapter.EventRowUpdating, value);
			}
			remove
			{
				base.Events.RemoveHandler(SqlDataAdapter.EventRowUpdating, value);
			}
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x0027D618 File Offset: 0x0027CA18
		protected override int AddToBatch(IDbCommand command)
		{
			int commandCount = this._commandSet.CommandCount;
			this._commandSet.Append((SqlCommand)command);
			return commandCount;
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x0027D644 File Offset: 0x0027CA44
		protected override void ClearBatch()
		{
			this._commandSet.Clear();
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x0027D65C File Offset: 0x0027CA5C
		object ICloneable.Clone()
		{
			return new SqlDataAdapter(this);
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x0027D670 File Offset: 0x0027CA70
		protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new SqlRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x0027D688 File Offset: 0x0027CA88
		protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new SqlRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x0027D6A0 File Offset: 0x0027CAA0
		protected override int ExecuteBatch()
		{
			return this._commandSet.ExecuteNonQuery();
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x0027D6B8 File Offset: 0x0027CAB8
		protected override IDataParameter GetBatchedParameter(int commandIdentifier, int parameterIndex)
		{
			return this._commandSet.GetParameter(commandIdentifier, parameterIndex);
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x0027D6D4 File Offset: 0x0027CAD4
		protected override bool GetBatchedRecordsAffected(int commandIdentifier, out int recordsAffected, out Exception error)
		{
			return this._commandSet.GetBatchedAffected(commandIdentifier, out recordsAffected, out error);
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x0027D6F0 File Offset: 0x0027CAF0
		protected override void InitializeBatching()
		{
			Bid.Trace("<sc.SqlDataAdapter.InitializeBatching|API> %d#\n", base.ObjectID);
			this._commandSet = new SqlCommandSet();
			SqlCommand sqlCommand = this.SelectCommand;
			if (sqlCommand == null)
			{
				sqlCommand = this.InsertCommand;
				if (sqlCommand == null)
				{
					sqlCommand = this.UpdateCommand;
					if (sqlCommand == null)
					{
						sqlCommand = this.DeleteCommand;
					}
				}
			}
			if (sqlCommand != null)
			{
				this._commandSet.Connection = sqlCommand.Connection;
				this._commandSet.Transaction = sqlCommand.Transaction;
				this._commandSet.CommandTimeout = sqlCommand.CommandTimeout;
			}
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x0027D774 File Offset: 0x0027CB74
		protected override void OnRowUpdated(RowUpdatedEventArgs value)
		{
			SqlRowUpdatedEventHandler sqlRowUpdatedEventHandler = (SqlRowUpdatedEventHandler)base.Events[SqlDataAdapter.EventRowUpdated];
			if (sqlRowUpdatedEventHandler != null && value is SqlRowUpdatedEventArgs)
			{
				sqlRowUpdatedEventHandler(this, (SqlRowUpdatedEventArgs)value);
			}
			base.OnRowUpdated(value);
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x0027D7B8 File Offset: 0x0027CBB8
		protected override void OnRowUpdating(RowUpdatingEventArgs value)
		{
			SqlRowUpdatingEventHandler sqlRowUpdatingEventHandler = (SqlRowUpdatingEventHandler)base.Events[SqlDataAdapter.EventRowUpdating];
			if (sqlRowUpdatingEventHandler != null && value is SqlRowUpdatingEventArgs)
			{
				sqlRowUpdatingEventHandler(this, (SqlRowUpdatingEventArgs)value);
			}
			base.OnRowUpdating(value);
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x0027D7FC File Offset: 0x0027CBFC
		protected override void TerminateBatching()
		{
			if (this._commandSet != null)
			{
				this._commandSet.Dispose();
				this._commandSet = null;
			}
		}

		// Token: 0x040017FC RID: 6140
		private static readonly object EventRowUpdated = new object();

		// Token: 0x040017FD RID: 6141
		private static readonly object EventRowUpdating = new object();

		// Token: 0x040017FE RID: 6142
		private SqlCommand _deleteCommand;

		// Token: 0x040017FF RID: 6143
		private SqlCommand _insertCommand;

		// Token: 0x04001800 RID: 6144
		private SqlCommand _selectCommand;

		// Token: 0x04001801 RID: 6145
		private SqlCommand _updateCommand;

		// Token: 0x04001802 RID: 6146
		private SqlCommandSet _commandSet;

		// Token: 0x04001803 RID: 6147
		private int _updateBatchSize = 1;
	}
}
