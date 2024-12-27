using System;

namespace System.Drawing.Drawing2D
{
	// Token: 0x0200006F RID: 111
	public sealed class Blend
	{
		// Token: 0x06000735 RID: 1845 RVA: 0x0001BA55 File Offset: 0x0001AA55
		public Blend()
		{
			this.factors = new float[1];
			this.positions = new float[1];
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001BA75 File Offset: 0x0001AA75
		public Blend(int count)
		{
			this.factors = new float[count];
			this.positions = new float[count];
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0001BA95 File Offset: 0x0001AA95
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x0001BA9D File Offset: 0x0001AA9D
		public float[] Factors
		{
			get
			{
				return this.factors;
			}
			set
			{
				this.factors = value;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0001BAA6 File Offset: 0x0001AAA6
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x0001BAAE File Offset: 0x0001AAAE
		public float[] Positions
		{
			get
			{
				return this.positions;
			}
			set
			{
				this.positions = value;
			}
		}

		// Token: 0x0400049C RID: 1180
		private float[] factors;

		// Token: 0x0400049D RID: 1181
		private float[] positions;
	}
}
