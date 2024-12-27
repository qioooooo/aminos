using System;
using System.ComponentModel;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000A0 RID: 160
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct DecimalAggregator
	{
		// Token: 0x0600078D RID: 1933 RVA: 0x00026ED3 File Offset: 0x00025ED3
		public void Create()
		{
			this.cnt = 0;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00026EDC File Offset: 0x00025EDC
		public void Sum(decimal value)
		{
			if (this.cnt == 0)
			{
				this.result = value;
				this.cnt = 1;
				return;
			}
			this.result += value;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00026F07 File Offset: 0x00025F07
		public void Average(decimal value)
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

		// Token: 0x06000790 RID: 1936 RVA: 0x00026F3A File Offset: 0x00025F3A
		public void Minimum(decimal value)
		{
			if (this.cnt == 0 || value < this.result)
			{
				this.result = value;
			}
			this.cnt = 1;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00026F60 File Offset: 0x00025F60
		public void Maximum(decimal value)
		{
			if (this.cnt == 0 || value > this.result)
			{
				this.result = value;
			}
			this.cnt = 1;
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x00026F86 File Offset: 0x00025F86
		public decimal SumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00026F8E File Offset: 0x00025F8E
		public decimal AverageResult
		{
			get
			{
				return this.result / this.cnt;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00026FA6 File Offset: 0x00025FA6
		public decimal MinimumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x00026FAE File Offset: 0x00025FAE
		public decimal MaximumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00026FB6 File Offset: 0x00025FB6
		public bool IsEmpty
		{
			get
			{
				return this.cnt == 0;
			}
		}

		// Token: 0x0400051E RID: 1310
		private decimal result;

		// Token: 0x0400051F RID: 1311
		private int cnt;
	}
}
