using System;

namespace System.Data
{
	// Token: 0x020000C6 RID: 198
	public class MergeFailedEventArgs : EventArgs
	{
		// Token: 0x06000CC1 RID: 3265 RVA: 0x001FAF74 File Offset: 0x001FA374
		public MergeFailedEventArgs(DataTable table, string conflict)
		{
			this.table = table;
			this.conflict = conflict;
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x001FAF98 File Offset: 0x001FA398
		public DataTable Table
		{
			get
			{
				return this.table;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x001FAFAC File Offset: 0x001FA3AC
		public string Conflict
		{
			get
			{
				return this.conflict;
			}
		}

		// Token: 0x040008AB RID: 2219
		private DataTable table;

		// Token: 0x040008AC RID: 2220
		private string conflict;
	}
}
