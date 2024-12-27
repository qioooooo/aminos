using System;
using System.Data.Common;

namespace System.Data.SqlClient
{
	// Token: 0x0200030B RID: 779
	public sealed class SqlRowUpdatingEventArgs : RowUpdatingEventArgs
	{
		// Token: 0x060028C9 RID: 10441 RVA: 0x00290BC4 File Offset: 0x0028FFC4
		public SqlRowUpdatingEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
			: base(row, command, statementType, tableMapping)
		{
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x060028CA RID: 10442 RVA: 0x00290BDC File Offset: 0x0028FFDC
		// (set) Token: 0x060028CB RID: 10443 RVA: 0x00290BF4 File Offset: 0x0028FFF4
		public new SqlCommand Command
		{
			get
			{
				return base.Command as SqlCommand;
			}
			set
			{
				base.Command = value;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x060028CC RID: 10444 RVA: 0x00290C08 File Offset: 0x00290008
		// (set) Token: 0x060028CD RID: 10445 RVA: 0x00290C1C File Offset: 0x0029001C
		protected override IDbCommand BaseCommand
		{
			get
			{
				return base.BaseCommand;
			}
			set
			{
				base.BaseCommand = value as SqlCommand;
			}
		}
	}
}
