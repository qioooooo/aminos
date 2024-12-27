using System;
using System.Data.Common;

namespace System.Data.Odbc
{
	// Token: 0x02000204 RID: 516
	public sealed class OdbcRowUpdatedEventArgs : RowUpdatedEventArgs
	{
		// Token: 0x06001C8C RID: 7308 RVA: 0x0024D604 File Offset: 0x0024CA04
		public OdbcRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
			: base(row, command, statementType, tableMapping)
		{
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001C8D RID: 7309 RVA: 0x0024D61C File Offset: 0x0024CA1C
		public new OdbcCommand Command
		{
			get
			{
				return (OdbcCommand)base.Command;
			}
		}
	}
}
