using System;

namespace System.Diagnostics
{
	// Token: 0x0200077C RID: 1916
	internal class ProcessThreadTimes
	{
		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06003B2A RID: 15146 RVA: 0x000FBD83 File Offset: 0x000FAD83
		public DateTime StartTime
		{
			get
			{
				return DateTime.FromFileTime(this.create);
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06003B2B RID: 15147 RVA: 0x000FBD90 File Offset: 0x000FAD90
		public DateTime ExitTime
		{
			get
			{
				return DateTime.FromFileTime(this.exit);
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06003B2C RID: 15148 RVA: 0x000FBD9D File Offset: 0x000FAD9D
		public TimeSpan PrivilegedProcessorTime
		{
			get
			{
				return new TimeSpan(this.kernel);
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06003B2D RID: 15149 RVA: 0x000FBDAA File Offset: 0x000FADAA
		public TimeSpan UserProcessorTime
		{
			get
			{
				return new TimeSpan(this.user);
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06003B2E RID: 15150 RVA: 0x000FBDB7 File Offset: 0x000FADB7
		public TimeSpan TotalProcessorTime
		{
			get
			{
				return new TimeSpan(this.user + this.kernel);
			}
		}

		// Token: 0x040033CD RID: 13261
		internal long create;

		// Token: 0x040033CE RID: 13262
		internal long exit;

		// Token: 0x040033CF RID: 13263
		internal long kernel;

		// Token: 0x040033D0 RID: 13264
		internal long user;
	}
}
