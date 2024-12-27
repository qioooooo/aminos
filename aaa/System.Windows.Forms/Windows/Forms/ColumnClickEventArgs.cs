using System;

namespace System.Windows.Forms
{
	// Token: 0x0200027C RID: 636
	public class ColumnClickEventArgs : EventArgs
	{
		// Token: 0x06002241 RID: 8769 RVA: 0x0004AB87 File Offset: 0x00049B87
		public ColumnClickEventArgs(int column)
		{
			this.column = column;
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06002242 RID: 8770 RVA: 0x0004AB96 File Offset: 0x00049B96
		public int Column
		{
			get
			{
				return this.column;
			}
		}

		// Token: 0x04001503 RID: 5379
		private readonly int column;
	}
}
