using System;

namespace System.Data.Common
{
	// Token: 0x0200015C RID: 348
	public class RowUpdatedEventArgs : EventArgs
	{
		// Token: 0x060015CA RID: 5578 RVA: 0x0022CE3C File Offset: 0x0022C23C
		public RowUpdatedEventArgs(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			switch (statementType)
			{
			case StatementType.Select:
			case StatementType.Insert:
			case StatementType.Update:
			case StatementType.Delete:
			case StatementType.Batch:
				this._dataRow = dataRow;
				this._command = command;
				this._statementType = statementType;
				this._tableMapping = tableMapping;
				return;
			default:
				throw ADP.InvalidStatementType(statementType);
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060015CB RID: 5579 RVA: 0x0022CE90 File Offset: 0x0022C290
		public IDbCommand Command
		{
			get
			{
				return this._command;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x0022CEA4 File Offset: 0x0022C2A4
		// (set) Token: 0x060015CD RID: 5581 RVA: 0x0022CEB8 File Offset: 0x0022C2B8
		public Exception Errors
		{
			get
			{
				return this._errors;
			}
			set
			{
				this._errors = value;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x0022CECC File Offset: 0x0022C2CC
		public int RecordsAffected
		{
			get
			{
				return this._recordsAffected;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x0022CEE0 File Offset: 0x0022C2E0
		public DataRow Row
		{
			get
			{
				return this._dataRow;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x0022CEF4 File Offset: 0x0022C2F4
		internal DataRow[] Rows
		{
			get
			{
				return this._dataRows;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060015D1 RID: 5585 RVA: 0x0022CF08 File Offset: 0x0022C308
		public int RowCount
		{
			get
			{
				DataRow[] dataRows = this._dataRows;
				if (dataRows != null)
				{
					return dataRows.Length;
				}
				if (this._dataRow == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x0022CF30 File Offset: 0x0022C330
		public StatementType StatementType
		{
			get
			{
				return this._statementType;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x0022CF44 File Offset: 0x0022C344
		// (set) Token: 0x060015D4 RID: 5588 RVA: 0x0022CF58 File Offset: 0x0022C358
		public UpdateStatus Status
		{
			get
			{
				return this._status;
			}
			set
			{
				switch (value)
				{
				case UpdateStatus.Continue:
				case UpdateStatus.ErrorsOccurred:
				case UpdateStatus.SkipCurrentRow:
				case UpdateStatus.SkipAllRemainingRows:
					this._status = value;
					return;
				default:
					throw ADP.InvalidUpdateStatus(value);
				}
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060015D5 RID: 5589 RVA: 0x0022CF90 File Offset: 0x0022C390
		public DataTableMapping TableMapping
		{
			get
			{
				return this._tableMapping;
			}
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x0022CFA4 File Offset: 0x0022C3A4
		internal void AdapterInit(DataRow[] dataRows)
		{
			this._statementType = StatementType.Batch;
			this._dataRows = dataRows;
			if (dataRows != null && 1 == dataRows.Length)
			{
				this._dataRow = dataRows[0];
			}
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x0022CFD4 File Offset: 0x0022C3D4
		internal void AdapterInit(int recordsAffected)
		{
			this._recordsAffected = recordsAffected;
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0022CFE8 File Offset: 0x0022C3E8
		public void CopyToRows(DataRow[] array)
		{
			this.CopyToRows(array, 0);
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x0022D000 File Offset: 0x0022C400
		public void CopyToRows(DataRow[] array, int arrayIndex)
		{
			DataRow[] dataRows = this._dataRows;
			if (dataRows != null)
			{
				dataRows.CopyTo(array, arrayIndex);
				return;
			}
			if (array == null)
			{
				throw ADP.ArgumentNull("array");
			}
			array[arrayIndex] = this.Row;
		}

		// Token: 0x04000CB5 RID: 3253
		private IDbCommand _command;

		// Token: 0x04000CB6 RID: 3254
		private StatementType _statementType;

		// Token: 0x04000CB7 RID: 3255
		private DataTableMapping _tableMapping;

		// Token: 0x04000CB8 RID: 3256
		private Exception _errors;

		// Token: 0x04000CB9 RID: 3257
		private DataRow _dataRow;

		// Token: 0x04000CBA RID: 3258
		private DataRow[] _dataRows;

		// Token: 0x04000CBB RID: 3259
		private UpdateStatus _status;

		// Token: 0x04000CBC RID: 3260
		private int _recordsAffected;
	}
}
