using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x02000036 RID: 54
	public sealed class BufferedGraphics : IDisposable
	{
		// Token: 0x06000234 RID: 564 RVA: 0x000095CF File Offset: 0x000085CF
		internal BufferedGraphics(Graphics bufferedGraphicsSurface, BufferedGraphicsContext context, Graphics targetGraphics, IntPtr targetDC, Point targetLoc, Size virtualSize)
		{
			this.context = context;
			this.bufferedGraphicsSurface = bufferedGraphicsSurface;
			this.targetDC = targetDC;
			this.targetGraphics = targetGraphics;
			this.targetLoc = targetLoc;
			this.virtualSize = virtualSize;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00009604 File Offset: 0x00008604
		~BufferedGraphics()
		{
			this.Dispose(false);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00009634 File Offset: 0x00008634
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00009640 File Offset: 0x00008640
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.context != null)
				{
					this.context.ReleaseBuffer(this);
					if (this.DisposeContext)
					{
						this.context.Dispose();
						this.context = null;
					}
				}
				if (this.bufferedGraphicsSurface != null)
				{
					this.bufferedGraphicsSurface.Dispose();
					this.bufferedGraphicsSurface = null;
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00009698 File Offset: 0x00008698
		// (set) Token: 0x06000239 RID: 569 RVA: 0x000096A0 File Offset: 0x000086A0
		internal bool DisposeContext
		{
			get
			{
				return this.disposeContext;
			}
			set
			{
				this.disposeContext = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600023A RID: 570 RVA: 0x000096A9 File Offset: 0x000086A9
		public Graphics Graphics
		{
			get
			{
				return this.bufferedGraphicsSurface;
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000096B1 File Offset: 0x000086B1
		public void Render()
		{
			if (this.targetGraphics != null)
			{
				this.Render(this.targetGraphics);
				return;
			}
			this.RenderInternal(new HandleRef(this.Graphics, this.targetDC), this);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000096E0 File Offset: 0x000086E0
		public void Render(Graphics target)
		{
			if (target != null)
			{
				IntPtr hdc = target.GetHdc();
				try
				{
					this.RenderInternal(new HandleRef(target, hdc), this);
				}
				finally
				{
					target.ReleaseHdcInternal(hdc);
				}
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00009720 File Offset: 0x00008720
		public void Render(IntPtr targetDC)
		{
			IntSecurity.UnmanagedCode.Demand();
			this.RenderInternal(new HandleRef(null, targetDC), this);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000973C File Offset: 0x0000873C
		private void RenderInternal(HandleRef refTargetDC, BufferedGraphics buffer)
		{
			IntPtr hdc = buffer.Graphics.GetHdc();
			try
			{
				SafeNativeMethods.BitBlt(refTargetDC, this.targetLoc.X, this.targetLoc.Y, this.virtualSize.Width, this.virtualSize.Height, new HandleRef(buffer.Graphics, hdc), 0, 0, BufferedGraphics.rop);
			}
			finally
			{
				buffer.Graphics.ReleaseHdcInternal(hdc);
			}
		}

		// Token: 0x04000253 RID: 595
		private Graphics bufferedGraphicsSurface;

		// Token: 0x04000254 RID: 596
		private Graphics targetGraphics;

		// Token: 0x04000255 RID: 597
		private BufferedGraphicsContext context;

		// Token: 0x04000256 RID: 598
		private IntPtr targetDC;

		// Token: 0x04000257 RID: 599
		private Point targetLoc;

		// Token: 0x04000258 RID: 600
		private Size virtualSize;

		// Token: 0x04000259 RID: 601
		private bool disposeContext;

		// Token: 0x0400025A RID: 602
		private static int rop = 13369376;
	}
}
