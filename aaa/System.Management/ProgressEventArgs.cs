using System;

namespace System.Management
{
	// Token: 0x02000013 RID: 19
	public class ProgressEventArgs : ManagementEventArgs
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00006189 File Offset: 0x00005189
		internal ProgressEventArgs(object context, int upperBound, int current, string message)
			: base(context)
		{
			this.upperBound = upperBound;
			this.current = current;
			this.message = message;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000061A8 File Offset: 0x000051A8
		public int UpperBound
		{
			get
			{
				return this.upperBound;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000061B0 File Offset: 0x000051B0
		public int Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AE RID: 174 RVA: 0x000061B8 File Offset: 0x000051B8
		public string Message
		{
			get
			{
				if (this.message == null)
				{
					return string.Empty;
				}
				return this.message;
			}
		}

		// Token: 0x04000084 RID: 132
		private int upperBound;

		// Token: 0x04000085 RID: 133
		private int current;

		// Token: 0x04000086 RID: 134
		private string message;
	}
}
