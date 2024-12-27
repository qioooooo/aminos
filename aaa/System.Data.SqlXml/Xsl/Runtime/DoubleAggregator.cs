using System;
using System.ComponentModel;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000A1 RID: 161
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct DoubleAggregator
	{
		// Token: 0x06000797 RID: 1943 RVA: 0x00026FC1 File Offset: 0x00025FC1
		public void Create()
		{
			this.cnt = 0;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00026FCA File Offset: 0x00025FCA
		public void Sum(double value)
		{
			if (this.cnt == 0)
			{
				this.result = value;
				this.cnt = 1;
				return;
			}
			this.result += value;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00026FF1 File Offset: 0x00025FF1
		public void Average(double value)
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

		// Token: 0x0600079A RID: 1946 RVA: 0x00027020 File Offset: 0x00026020
		public void Minimum(double value)
		{
			if (this.cnt == 0 || value < this.result || double.IsNaN(value))
			{
				this.result = value;
			}
			this.cnt = 1;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00027049 File Offset: 0x00026049
		public void Maximum(double value)
		{
			if (this.cnt == 0 || value > this.result || double.IsNaN(value))
			{
				this.result = value;
			}
			this.cnt = 1;
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x00027072 File Offset: 0x00026072
		public double SumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x0002707A File Offset: 0x0002607A
		public double AverageResult
		{
			get
			{
				return this.result / (double)this.cnt;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0002708A File Offset: 0x0002608A
		public double MinimumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x00027092 File Offset: 0x00026092
		public double MaximumResult
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0002709A File Offset: 0x0002609A
		public bool IsEmpty
		{
			get
			{
				return this.cnt == 0;
			}
		}

		// Token: 0x04000520 RID: 1312
		private double result;

		// Token: 0x04000521 RID: 1313
		private int cnt;
	}
}
