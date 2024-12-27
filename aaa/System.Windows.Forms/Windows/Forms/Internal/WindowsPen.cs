using System;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Internal
{
	// Token: 0x0200002B RID: 43
	internal sealed class WindowsPen : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x06000134 RID: 308 RVA: 0x000050EE File Offset: 0x000040EE
		public WindowsPen(DeviceContext dc)
			: this(dc, WindowsPenStyle.Solid, 1, Color.Black)
		{
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000050FE File Offset: 0x000040FE
		public WindowsPen(DeviceContext dc, Color color)
			: this(dc, WindowsPenStyle.Solid, 1, color)
		{
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000510A File Offset: 0x0000410A
		public WindowsPen(DeviceContext dc, WindowsBrush windowsBrush)
			: this(dc, WindowsPenStyle.Solid, 1, windowsBrush)
		{
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005116 File Offset: 0x00004116
		public WindowsPen(DeviceContext dc, WindowsPenStyle style, int width, Color color)
		{
			this.style = style;
			this.width = width;
			this.color = color;
			this.dc = dc;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000513B File Offset: 0x0000413B
		public WindowsPen(DeviceContext dc, WindowsPenStyle style, int width, WindowsBrush windowsBrush)
		{
			this.style = style;
			this.wndBrush = (WindowsBrush)windowsBrush.Clone();
			this.width = width;
			this.color = windowsBrush.Color;
			this.dc = dc;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005178 File Offset: 0x00004178
		private void CreatePen()
		{
			if (this.width > 1)
			{
				this.style |= WindowsPenStyle.Geometric;
			}
			if (this.wndBrush == null)
			{
				this.nativeHandle = IntSafeNativeMethods.CreatePen((int)this.style, this.width, ColorTranslator.ToWin32(this.color));
				return;
			}
			IntNativeMethods.LOGBRUSH logbrush = new IntNativeMethods.LOGBRUSH();
			logbrush.lbColor = ColorTranslator.ToWin32(this.wndBrush.Color);
			logbrush.lbStyle = 0;
			logbrush.lbHatch = 0;
			this.nativeHandle = IntSafeNativeMethods.ExtCreatePen((int)this.style, this.width, logbrush, 0, null);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005210 File Offset: 0x00004210
		public object Clone()
		{
			if (this.wndBrush == null)
			{
				return new WindowsPen(this.dc, this.style, this.width, this.color);
			}
			return new WindowsPen(this.dc, this.style, this.width, (WindowsBrush)this.wndBrush.Clone());
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000526C File Offset: 0x0000426C
		~WindowsPen()
		{
			this.Dispose(false);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000529C File Offset: 0x0000429C
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000052A8 File Offset: 0x000042A8
		private void Dispose(bool disposing)
		{
			if (this.nativeHandle != IntPtr.Zero && this.dc != null)
			{
				this.dc.DeleteObject(this.nativeHandle, GdiObjectType.Pen);
				this.nativeHandle = IntPtr.Zero;
			}
			if (this.wndBrush != null)
			{
				this.wndBrush.Dispose();
				this.wndBrush = null;
			}
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000530F File Offset: 0x0000430F
		public IntPtr HPen
		{
			get
			{
				if (this.nativeHandle == IntPtr.Zero)
				{
					this.CreatePen();
				}
				return this.nativeHandle;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005330 File Offset: 0x00004330
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}: Style={1}, Color={2}, Width={3}, Brush={4}", new object[]
			{
				base.GetType().Name,
				this.style,
				this.color,
				this.width,
				(this.wndBrush != null) ? this.wndBrush.ToString() : "null"
			});
		}

		// Token: 0x04000ADB RID: 2779
		private const int dashStyleMask = 15;

		// Token: 0x04000ADC RID: 2780
		private const int endCapMask = 3840;

		// Token: 0x04000ADD RID: 2781
		private const int joinMask = 61440;

		// Token: 0x04000ADE RID: 2782
		private const int cosmeticPenWidth = 1;

		// Token: 0x04000ADF RID: 2783
		private IntPtr nativeHandle;

		// Token: 0x04000AE0 RID: 2784
		private DeviceContext dc;

		// Token: 0x04000AE1 RID: 2785
		private WindowsBrush wndBrush;

		// Token: 0x04000AE2 RID: 2786
		private WindowsPenStyle style;

		// Token: 0x04000AE3 RID: 2787
		private Color color;

		// Token: 0x04000AE4 RID: 2788
		private int width;
	}
}
