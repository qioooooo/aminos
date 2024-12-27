using System;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000D3 RID: 211
	public sealed class PathData
	{
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x00025732 File Offset: 0x00024732
		// (set) Token: 0x06000C7D RID: 3197 RVA: 0x0002573A File Offset: 0x0002473A
		public PointF[] Points
		{
			get
			{
				return this.points;
			}
			set
			{
				this.points = value;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x00025743 File Offset: 0x00024743
		// (set) Token: 0x06000C7F RID: 3199 RVA: 0x0002574B File Offset: 0x0002474B
		public byte[] Types
		{
			get
			{
				return this.types;
			}
			set
			{
				this.types = value;
			}
		}

		// Token: 0x04000AC6 RID: 2758
		private PointF[] points;

		// Token: 0x04000AC7 RID: 2759
		private byte[] types;
	}
}
