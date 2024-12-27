using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000209 RID: 521
	public class PaintEventArgs : EventArgs, IDisposable
	{
		// Token: 0x060017CD RID: 6093 RVA: 0x00027FCB File Offset: 0x00026FCB
		public PaintEventArgs(Graphics graphics, Rectangle clipRect)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			this.graphics = graphics;
			this.clipRect = clipRect;
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00028005 File Offset: 0x00027005
		internal PaintEventArgs(IntPtr dc, Rectangle clipRect)
		{
			this.dc = dc;
			this.clipRect = clipRect;
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00028034 File Offset: 0x00027034
		~PaintEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00028064 File Offset: 0x00027064
		public Rectangle ClipRectangle
		{
			get
			{
				return this.clipRect;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0002806C File Offset: 0x0002706C
		internal IntPtr HDC
		{
			get
			{
				if (this.graphics == null)
				{
					return this.dc;
				}
				return IntPtr.Zero;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x00028084 File Offset: 0x00027084
		public Graphics Graphics
		{
			get
			{
				if (this.graphics == null && this.dc != IntPtr.Zero)
				{
					this.oldPal = Control.SetUpPalette(this.dc, false, false);
					this.graphics = Graphics.FromHdcInternal(this.dc);
					this.graphics.PageUnit = GraphicsUnit.Pixel;
					this.savedGraphicsState = this.graphics.Save();
				}
				return this.graphics;
			}
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000280F2 File Offset: 0x000270F2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00028104 File Offset: 0x00027104
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.graphics != null && this.dc != IntPtr.Zero)
			{
				this.graphics.Dispose();
			}
			if (this.oldPal != IntPtr.Zero && this.dc != IntPtr.Zero)
			{
				SafeNativeMethods.SelectPalette(new HandleRef(this, this.dc), new HandleRef(this, this.oldPal), 0);
				this.oldPal = IntPtr.Zero;
			}
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00028187 File Offset: 0x00027187
		internal void ResetGraphics()
		{
			if (this.graphics != null && this.savedGraphicsState != null)
			{
				this.graphics.Restore(this.savedGraphicsState);
				this.savedGraphicsState = null;
			}
		}

		// Token: 0x040011CB RID: 4555
		private Graphics graphics;

		// Token: 0x040011CC RID: 4556
		private GraphicsState savedGraphicsState;

		// Token: 0x040011CD RID: 4557
		private readonly IntPtr dc = IntPtr.Zero;

		// Token: 0x040011CE RID: 4558
		private IntPtr oldPal = IntPtr.Zero;

		// Token: 0x040011CF RID: 4559
		private readonly Rectangle clipRect;
	}
}
