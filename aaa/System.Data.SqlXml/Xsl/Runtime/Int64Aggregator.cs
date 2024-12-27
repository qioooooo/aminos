using System;
using System.ComponentModel;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200009F RID: 159
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct Int64Aggregator
	{
		// Token: 0x06000783 RID: 1923 RVA: 0x00026DFF File Offset: 0x00025DFF
		public void Create()
		{
			this.cnt = 0;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00026E08 File Offset: 0x00025E08
		public void Sum(long value)
		{
			if (this.cnt == 0)
			{
				this.result = value;
				this.cnt = 1;
				return;
			}
			this.result += value;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00026E2F File Offset: 0x00025E2F
		public void Average(long value)
		{
			if (this.cnt == 0)
			{
				this.result = value;
			}
			else
			{
				this.result += value;
			}
			this.cnt++;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00026E5E File Offset: 0x00025E5E
		public void Minimum(long value)
		{
			if (this.cnt == 0 || value < this.result)
			{
				this.result = value;
			}
			this.cnt = 1;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00026E7F File Offset: 0x00025E7F
		public void Maximum(long value)
		{
			if (this.cnt == 0 || value > this.result)
			{
				this.result = value;
			}
			this.cnt = 1;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x00026EA0 File Offset: 0x00025EA0
		public long SumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x00026EA8 File Offset: 0x00025EA8
		public long AverageResult
		{
			get
			{
				return this.result / (long)this.cnt;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x00026EB8 File Offset: 0x00025EB8
		public long MinimumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x00026EC0 File Offset: 0x00025EC0
		public long MaximumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x00026EC8 File Offset: 0x00025EC8
		public bool IsEmpty
		{
			get
			{
				return this.cnt == 0;
			}
		}

		// Token: 0x0400051C RID: 1308
		private long result;

		// Token: 0x0400051D RID: 1309
		private int cnt;
	}
}
