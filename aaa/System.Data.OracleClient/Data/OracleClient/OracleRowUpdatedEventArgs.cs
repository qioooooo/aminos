using System;
using System.Data.Common;

namespace System.Data.OracleClient
{
	// Token: 0x02000076 RID: 118
	public sealed class OracleRowUpdatedEventArgs : RowUpdatedEventArgs
	{
		// Token: 0x0600067E RID: 1662 RVA: 0x0006E8F8 File Offset: 0x0006DCF8
		public OracleRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
			: base(row, command, statementType, tableMapping)
		{
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0006E910 File Offset: 0x0006DD10
		public new OracleCommand Command
		{
			get
			{
				return (OracleCommand)base.Command;
			}
		}
	}
}
