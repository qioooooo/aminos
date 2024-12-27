using System;
using System.Data.Common;

namespace System.Data.OleDb
{
	// Token: 0x0200023A RID: 570
	public sealed class OleDbRowUpdatedEventArgs : RowUpdatedEventArgs
	{
		// Token: 0x06002057 RID: 8279 RVA: 0x00262028 File Offset: 0x00261428
		public OleDbRowUpdatedEventArgs(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
			: base(dataRow, command, statementType, tableMapping)
		{
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06002058 RID: 8280 RVA: 0x00262040 File Offset: 0x00261440
		public new OleDbCommand Command
		{
			get
			{
				return (OleDbCommand)base.Command;
			}
		}
	}
}
