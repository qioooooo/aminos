using System;

namespace System.Data.Common
{
	// Token: 0x0200015D RID: 349
	public class RowUpdatingEventArgs : EventArgs
	{
		// Token: 0x060015DA RID: 5594 RVA: 0x0022D038 File Offset: 0x0022C438
		public RowUpdatingEventArgs(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			ADP.CheckArgumentNull(dataRow, "dataRow");
			ADP.CheckArgumentNull(tableMapping, "tableMapping");
			switch (statementType)
			{
			case StatementType.Select:
			case StatementType.Insert:
			case StatementType.Update:
			case StatementType.Delete:
				this._dataRow = dataRow;
				this._command = command;
				this._statementType = statementType;
				this._tableMapping = tableMapping;
				return;
			case StatementType.Batch:
				throw ADP.NotSupportedStatementType(statementType, "RowUpdatingEventArgs");
			default:
				throw ADP.InvalidStatementType(statementType);
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x0022D0B0 File Offset: 0x0022C4B0
		// (set) Token: 0x060015DC RID: 5596 RVA: 0x0022D0C4 File Offset: 0x0022C4C4
		protected virtual IDbCommand BaseCommand
		{
			get
			{
				return this._command;
			}
			set
			{
				this._command = value;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060015DD RID: 5597 RVA: 0x0022D0D8 File Offset: 0x0022C4D8
		// (set) Token: 0x060015DE RID: 5598 RVA: 0x0022D0EC File Offset: 0x0022C4EC
		public IDbCommand Command
		{
			get
			{
				return this.BaseCommand;
			}
			set
			{
				this.BaseCommand = value;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x060015DF RID: 5599 RVA: 0x0022D100 File Offset: 0x0022C500
		// (set) Token: 0x060015E0 RID: 5600 RVA: 0x0022D114 File Offset: 0x0022C514
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

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x060015E1 RID: 5601 RVA: 0x0022D128 File Offset: 0x0022C528
		public DataRow Row
		{
			get
			{
				return this._dataRow;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x0022D13C File Offset: 0x0022C53C
		public StatementType StatementType
		{
			get
			{
				return this._statementType;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x0022D150 File Offset: 0x0022C550
		// (set) Token: 0x060015E4 RID: 5604 RVA: 0x0022D164 File Offset: 0x0022C564
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

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060015E5 RID: 5605 RVA: 0x0022D19C File Offset: 0x0022C59C
		public DataTableMapping TableMapping
		{
			get
			{
				return this._tableMapping;
			}
		}

		// Token: 0x04000CBD RID: 3261
		private IDbCommand _command;

		// Token: 0x04000CBE RID: 3262
		private StatementType _statementType;

		// Token: 0x04000CBF RID: 3263
		private DataTableMapping _tableMapping;

		// Token: 0x04000CC0 RID: 3264
		private Exception _errors;

		// Token: 0x04000CC1 RID: 3265
		private DataRow _dataRow;

		// Token: 0x04000CC2 RID: 3266
		private UpdateStatus _status;
	}
}
