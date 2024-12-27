using System;

namespace System.Data
{
	// Token: 0x020000D3 RID: 211
	public sealed class StatementCompletedEventArgs : EventArgs
	{
		// Token: 0x06000CF8 RID: 3320 RVA: 0x001FC97C File Offset: 0x001FBD7C
		public StatementCompletedEventArgs(int recordCount)
		{
			this._recordCount = recordCount;
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x001FC998 File Offset: 0x001FBD98
		public int RecordCount
		{
			get
			{
				return this._recordCount;
			}
		}

		// Token: 0x040008E0 RID: 2272
		private readonly int _recordCount;
	}
}
