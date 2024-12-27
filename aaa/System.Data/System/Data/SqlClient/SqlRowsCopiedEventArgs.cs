using System;

namespace System.Data.SqlClient
{
	// Token: 0x020002A5 RID: 677
	public class SqlRowsCopiedEventArgs : EventArgs
	{
		// Token: 0x060022BA RID: 8890 RVA: 0x0026E0A4 File Offset: 0x0026D4A4
		public SqlRowsCopiedEventArgs(long rowsCopied)
		{
			this._rowsCopied = rowsCopied;
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060022BB RID: 8891 RVA: 0x0026E0C0 File Offset: 0x0026D4C0
		// (set) Token: 0x060022BC RID: 8892 RVA: 0x0026E0D4 File Offset: 0x0026D4D4
		public bool Abort
		{
			get
			{
				return this._abort;
			}
			set
			{
				this._abort = value;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x0026E0E8 File Offset: 0x0026D4E8
		public long RowsCopied
		{
			get
			{
				return this._rowsCopied;
			}
		}

		// Token: 0x04001675 RID: 5749
		private bool _abort;

		// Token: 0x04001676 RID: 5750
		private long _rowsCopied;
	}
}
