using System;
using System.Drawing.Drawing2D;

namespace System.Drawing
{
	// Token: 0x02000043 RID: 67
	internal class GraphicsContext : IDisposable
	{
		// Token: 0x0600032E RID: 814 RVA: 0x0000C333 File Offset: 0x0000B333
		private GraphicsContext()
		{
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000C33C File Offset: 0x0000B33C
		public GraphicsContext(Graphics g)
		{
			Matrix transform = g.Transform;
			if (!transform.IsIdentity)
			{
				float[] elements = transform.Elements;
				this.transformOffset.X = elements[4];
				this.transformOffset.Y = elements[5];
			}
			transform.Dispose();
			Region clip = g.Clip;
			if (clip.IsInfinite(g))
			{
				clip.Dispose();
				return;
			}
			this.clipRegion = clip;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000C3A5 File Offset: 0x0000B3A5
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000C3B4 File Offset: 0x0000B3B4
		public void Dispose(bool disposing)
		{
			if (this.nextContext != null)
			{
				this.nextContext.Dispose();
				this.nextContext = null;
			}
			if (this.clipRegion != null)
			{
				this.clipRegion.Dispose();
				this.clipRegion = null;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000C3EA File Offset: 0x0000B3EA
		// (set) Token: 0x06000333 RID: 819 RVA: 0x0000C3F2 File Offset: 0x0000B3F2
		public int State
		{
			get
			{
				return this.contextState;
			}
			set
			{
				this.contextState = value;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000C3FB File Offset: 0x0000B3FB
		public PointF TransformOffset
		{
			get
			{
				return this.transformOffset;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000C403 File Offset: 0x0000B403
		public Region Clip
		{
			get
			{
				return this.clipRegion;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000C40B File Offset: 0x0000B40B
		// (set) Token: 0x06000337 RID: 823 RVA: 0x0000C413 File Offset: 0x0000B413
		public GraphicsContext Next
		{
			get
			{
				return this.nextContext;
			}
			set
			{
				this.nextContext = value;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000C41C File Offset: 0x0000B41C
		// (set) Token: 0x06000339 RID: 825 RVA: 0x0000C424 File Offset: 0x0000B424
		public GraphicsContext Previous
		{
			get
			{
				return this.prevContext;
			}
			set
			{
				this.prevContext = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000C42D File Offset: 0x0000B42D
		// (set) Token: 0x0600033B RID: 827 RVA: 0x0000C435 File Offset: 0x0000B435
		public bool IsCumulative
		{
			get
			{
				return this.isCumulative;
			}
			set
			{
				this.isCumulative = value;
			}
		}

		// Token: 0x040002A5 RID: 677
		private int contextState;

		// Token: 0x040002A6 RID: 678
		private PointF transformOffset;

		// Token: 0x040002A7 RID: 679
		private Region clipRegion;

		// Token: 0x040002A8 RID: 680
		private GraphicsContext nextContext;

		// Token: 0x040002A9 RID: 681
		private GraphicsContext prevContext;

		// Token: 0x040002AA RID: 682
		private bool isCumulative;
	}
}
