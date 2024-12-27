using System;

namespace System.Data
{
	// Token: 0x020000CF RID: 207
	internal struct Range
	{
		// Token: 0x06000CDE RID: 3294 RVA: 0x001FC338 File Offset: 0x001FB738
		public Range(int min, int max)
		{
			if (min > max)
			{
				throw ExceptionBuilder.RangeArgument(min, max);
			}
			this.min = min;
			this.max = max;
			this.isNotNull = true;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x001FC368 File Offset: 0x001FB768
		public int Count
		{
			get
			{
				if (this.IsNull)
				{
					return 0;
				}
				return this.max - this.min + 1;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x001FC390 File Offset: 0x001FB790
		public bool IsNull
		{
			get
			{
				return !this.isNotNull;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x001FC3A8 File Offset: 0x001FB7A8
		public int Max
		{
			get
			{
				this.CheckNull();
				return this.max;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x001FC3C4 File Offset: 0x001FB7C4
		public int Min
		{
			get
			{
				this.CheckNull();
				return this.min;
			}
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x001FC3E0 File Offset: 0x001FB7E0
		internal void CheckNull()
		{
			if (this.IsNull)
			{
				throw ExceptionBuilder.NullRange();
			}
		}

		// Token: 0x040008C1 RID: 2241
		private int min;

		// Token: 0x040008C2 RID: 2242
		private int max;

		// Token: 0x040008C3 RID: 2243
		private bool isNotNull;
	}
}
