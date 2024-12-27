using System;
using System.Data.Common;

namespace System.Data.OleDb
{
	// Token: 0x0200023C RID: 572
	public sealed class OleDbRowUpdatingEventArgs : RowUpdatingEventArgs
	{
		// Token: 0x0600205D RID: 8285 RVA: 0x00262058 File Offset: 0x00261458
		public OleDbRowUpdatingEventArgs(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
			: base(dataRow, command, statementType, tableMapping)
		{
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x00262070 File Offset: 0x00261470
		// (set) Token: 0x0600205F RID: 8287 RVA: 0x00262088 File Offset: 0x00261488
		public new OleDbCommand Command
		{
			get
			{
				return base.Command as OleDbCommand;
			}
			set
			{
				base.Command = value;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06002060 RID: 8288 RVA: 0x0026209C File Offset: 0x0026149C
		// (set) Token: 0x06002061 RID: 8289 RVA: 0x002620B0 File Offset: 0x002614B0
		protected override IDbCommand BaseCommand
		{
			get
			{
				return base.BaseCommand;
			}
			set
			{
				base.BaseCommand = value as OleDbCommand;
			}
		}
	}
}
