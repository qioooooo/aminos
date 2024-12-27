using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x020000F1 RID: 241
	[StructLayout(LayoutKind.Sequential)]
	public sealed class WmfPlaceableFileHeader
	{
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00028017 File Offset: 0x00027017
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x0002801F File Offset: 0x0002701F
		public int Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00028028 File Offset: 0x00027028
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x00028030 File Offset: 0x00027030
		public short Hmf
		{
			get
			{
				return this.hmf;
			}
			set
			{
				this.hmf = value;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00028039 File Offset: 0x00027039
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x00028041 File Offset: 0x00027041
		public short BboxLeft
		{
			get
			{
				return this.bboxLeft;
			}
			set
			{
				this.bboxLeft = value;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x0002804A File Offset: 0x0002704A
		// (set) Token: 0x06000D71 RID: 3441 RVA: 0x00028052 File Offset: 0x00027052
		public short BboxTop
		{
			get
			{
				return this.bboxTop;
			}
			set
			{
				this.bboxTop = value;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0002805B File Offset: 0x0002705B
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x00028063 File Offset: 0x00027063
		public short BboxRight
		{
			get
			{
				return this.bboxRight;
			}
			set
			{
				this.bboxRight = value;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x0002806C File Offset: 0x0002706C
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x00028074 File Offset: 0x00027074
		public short BboxBottom
		{
			get
			{
				return this.bboxBottom;
			}
			set
			{
				this.bboxBottom = value;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x0002807D File Offset: 0x0002707D
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x00028085 File Offset: 0x00027085
		public short Inch
		{
			get
			{
				return this.inch;
			}
			set
			{
				this.inch = value;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x0002808E File Offset: 0x0002708E
		// (set) Token: 0x06000D79 RID: 3449 RVA: 0x00028096 File Offset: 0x00027096
		public int Reserved
		{
			get
			{
				return this.reserved;
			}
			set
			{
				this.reserved = value;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x0002809F File Offset: 0x0002709F
		// (set) Token: 0x06000D7B RID: 3451 RVA: 0x000280A7 File Offset: 0x000270A7
		public short Checksum
		{
			get
			{
				return this.checksum;
			}
			set
			{
				this.checksum = value;
			}
		}

		// Token: 0x04000B57 RID: 2903
		private int key = -1698247209;

		// Token: 0x04000B58 RID: 2904
		private short hmf;

		// Token: 0x04000B59 RID: 2905
		private short bboxLeft;

		// Token: 0x04000B5A RID: 2906
		private short bboxTop;

		// Token: 0x04000B5B RID: 2907
		private short bboxRight;

		// Token: 0x04000B5C RID: 2908
		private short bboxBottom;

		// Token: 0x04000B5D RID: 2909
		private short inch;

		// Token: 0x04000B5E RID: 2910
		private int reserved;

		// Token: 0x04000B5F RID: 2911
		private short checksum;
	}
}
