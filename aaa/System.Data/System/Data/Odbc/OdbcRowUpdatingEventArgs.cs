using System;
using System.Data.Common;

namespace System.Data.Odbc
{
	// Token: 0x02000203 RID: 515
	public sealed class OdbcRowUpdatingEventArgs : RowUpdatingEventArgs
	{
		// Token: 0x06001C87 RID: 7303 RVA: 0x0024D590 File Offset: 0x0024C990
		public OdbcRowUpdatingEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
			: base(row, command, statementType, tableMapping)
		{
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001C88 RID: 7304 RVA: 0x0024D5A8 File Offset: 0x0024C9A8
		// (set) Token: 0x06001C89 RID: 7305 RVA: 0x0024D5C0 File Offset: 0x0024C9C0
		public new OdbcCommand Command
		{
			get
			{
				return base.Command as OdbcCommand;
			}
			set
			{
				base.Command = value;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001C8A RID: 7306 RVA: 0x0024D5D4 File Offset: 0x0024C9D4
		// (set) Token: 0x06001C8B RID: 7307 RVA: 0x0024D5E8 File Offset: 0x0024C9E8
		protected override IDbCommand BaseCommand
		{
			get
			{
				return base.BaseCommand;
			}
			set
			{
				base.BaseCommand = value as OdbcCommand;
			}
		}
	}
}
