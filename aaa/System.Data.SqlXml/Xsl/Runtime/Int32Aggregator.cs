using System;
using System.ComponentModel;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200009E RID: 158
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct Int32Aggregator
	{
		// Token: 0x06000779 RID: 1913 RVA: 0x00026D2C File Offset: 0x00025D2C
		public void Create()
		{
			this.cnt = 0;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00026D35 File Offset: 0x00025D35
		public void Sum(int value)
		{
			if (this.cnt == 0)
			{
				this.result = value;
				this.cnt = 1;
				return;
			}
			this.result += value;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00026D5C File Offset: 0x00025D5C
		public void Average(int value)
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

		// Token: 0x0600077C RID: 1916 RVA: 0x00026D8B File Offset: 0x00025D8B
		public void Minimum(int value)
		{
			if (this.cnt == 0 || value < this.result)
			{
				this.result = value;
			}
			this.cnt = 1;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00026DAC File Offset: 0x00025DAC
		public void Maximum(int value)
		{
			if (this.cnt == 0 || value > this.result)
			{
				this.result = value;
			}
			this.cnt = 1;
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x00026DCD File Offset: 0x00025DCD
		public int SumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00026DD5 File Offset: 0x00025DD5
		public int AverageResult
		{
			get
			{
				return this.result / this.cnt;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00026DE4 File Offset: 0x00025DE4
		public int MinimumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x00026DEC File Offset: 0x00025DEC
		public int MaximumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x00026DF4 File Offset: 0x00025DF4
		public bool IsEmpty
		{
			get
			{
				return this.cnt == 0;
			}
		}

		// Token: 0x0400051A RID: 1306
		private int result;

		// Token: 0x0400051B RID: 1307
		private int cnt;
	}
}
