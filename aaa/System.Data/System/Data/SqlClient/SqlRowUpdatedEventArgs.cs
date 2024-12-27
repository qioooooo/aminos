using System;
using System.Data.Common;

namespace System.Data.SqlClient
{
	// Token: 0x02000309 RID: 777
	public sealed class SqlRowUpdatedEventArgs : RowUpdatedEventArgs
	{
		// Token: 0x060028C3 RID: 10435 RVA: 0x00290B94 File Offset: 0x0028FF94
		public SqlRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
			: base(row, command, statementType, tableMapping)
		{
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060028C4 RID: 10436 RVA: 0x00290BAC File Offset: 0x0028FFAC
		public new SqlCommand Command
		{
			get
			{
				return (SqlCommand)base.Command;
			}
		}
	}
}
