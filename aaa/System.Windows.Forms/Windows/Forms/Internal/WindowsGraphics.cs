using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
	// Token: 0x0200002A RID: 42
	internal sealed class WindowsGraphics : MarshalByRefObject, IDeviceContext, IDisposable
	{
		// Token: 0x0600010B RID: 267 RVA: 0x000044FB File Offset: 0x000034FB
		public WindowsGraphics(DeviceContext dc)
		{
			this.dc = dc;
			this.dc.SaveHdc();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004518 File Offset: 0x00003518
		public static WindowsGraphics CreateMeasurementWindowsGraphics()
		{
			DeviceContext deviceContext = DeviceContext.FromCompatibleDC(IntPtr.Zero);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00004540 File Offset: 0x00003540
		public static WindowsGraphics FromHwnd(IntPtr hWnd)
		{
			DeviceContext deviceContext = DeviceContext.FromHwnd(hWnd);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004564 File Offset: 0x00003564
		public static WindowsGraphics FromHdc(IntPtr hDc)
		{
			DeviceContext deviceContext = DeviceContext.FromHdc(hDc);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004588 File Offset: 0x00003588
		public static WindowsGraphics FromGraphics(Graphics g)
		{
			ApplyGraphicsProperties applyGraphicsProperties = ApplyGraphicsProperties.All;
			return WindowsGraphics.FromGraphics(g, applyGraphicsProperties);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000045A0 File Offset: 0x000035A0
		public static WindowsGraphics FromGraphics(Graphics g, ApplyGraphicsProperties properties)
		{
			WindowsRegion windowsRegion = null;
			float[] array = null;
			Region region = null;
			Matrix matrix = null;
			if ((properties & ApplyGraphicsProperties.TranslateTransform) != ApplyGraphicsProperties.None || (properties & ApplyGraphicsProperties.Clipping) != ApplyGraphicsProperties.None)
			{
				object[] array2 = g.GetContextInfo() as object[];
				if (array2 != null && array2.Length == 2)
				{
					region = array2[0] as Region;
					matrix = array2[1] as Matrix;
				}
				if (matrix != null)
				{
					if ((properties & ApplyGraphicsProperties.TranslateTransform) != ApplyGraphicsProperties.None)
					{
						array = matrix.Elements;
					}
					matrix.Dispose();
				}
				if (region != null)
				{
					if ((properties & ApplyGraphicsProperties.Clipping) != ApplyGraphicsProperties.None && !region.IsInfinite(g))
					{
						windowsRegion = WindowsRegion.FromRegion(region, g);
					}
					region.Dispose();
				}
			}
			WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(g.GetHdc());
			windowsGraphics.graphics = g;
			if (windowsRegion != null)
			{
				using (windowsRegion)
				{
					windowsGraphics.DeviceContext.IntersectClip(windowsRegion);
				}
			}
			if (array != null)
			{
				windowsGraphics.DeviceContext.TranslateTransform((int)array[4], (int)array[5]);
			}
			return windowsGraphics;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004680 File Offset: 0x00003680
		~WindowsGraphics()
		{
			this.Dispose(false);
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000046B0 File Offset: 0x000036B0
		public DeviceContext DeviceContext
		{
			get
			{
				return this.dc;
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000046B8 File Offset: 0x000036B8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000046C8 File Offset: 0x000036C8
		internal void Dispose(bool disposing)
		{
			if (this.dc != null)
			{
				try
				{
					this.dc.RestoreHdc();
					if (this.disposeDc)
					{
						this.dc.Dispose(disposing);
					}
					if (this.graphics != null)
					{
						this.graphics.ReleaseHdcInternal(this.dc.Hdc);
						this.graphics = null;
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.dc = null;
				}
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004758 File Offset: 0x00003758
		public IntPtr GetHdc()
		{
			return this.dc.Hdc;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004765 File Offset: 0x00003765
		public void ReleaseHdc()
		{
			this.dc.Dispose();
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00004772 File Offset: 0x00003772
		// (set) Token: 0x06000118 RID: 280 RVA: 0x0000477A File Offset: 0x0000377A
		public TextPaddingOptions TextPadding
		{
			get
			{
				return this.paddingFlags;
			}
			set
			{
				if (this.paddingFlags != value)
				{
					this.paddingFlags = value;
				}
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000478C File Offset: 0x0000378C
		public void DrawPie(WindowsPen pen, Rectangle bounds, float startAngle, float sweepAngle)
		{
			HandleRef handleRef = new HandleRef(this.dc, this.dc.Hdc);
			if (pen != null)
			{
				IntUnsafeNativeMethods.SelectObject(handleRef, new HandleRef(pen, pen.HPen));
			}
			int num = Math.Min(bounds.Width, bounds.Height);
			Point point = new Point(bounds.X + num / 2, bounds.Y + num / 2);
			int num2 = num / 2;
			IntUnsafeNativeMethods.BeginPath(handleRef);
			IntUnsafeNativeMethods.MoveToEx(handleRef, point.X, point.Y, null);
			IntUnsafeNativeMethods.AngleArc(handleRef, point.X, point.Y, num2, startAngle, sweepAngle);
			IntUnsafeNativeMethods.LineTo(handleRef, point.X, point.Y);
			IntUnsafeNativeMethods.EndPath(handleRef);
			IntUnsafeNativeMethods.StrokePath(handleRef);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00004858 File Offset: 0x00003858
		private void DrawEllipse(WindowsPen pen, WindowsBrush brush, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
		{
			HandleRef handleRef = new HandleRef(this.dc, this.dc.Hdc);
			if (pen != null)
			{
				IntUnsafeNativeMethods.SelectObject(handleRef, new HandleRef(pen, pen.HPen));
			}
			if (brush != null)
			{
				IntUnsafeNativeMethods.SelectObject(handleRef, new HandleRef(brush, brush.HBrush));
			}
			IntUnsafeNativeMethods.Ellipse(handleRef, nLeftRect, nTopRect, nRightRect, nBottomRect);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000048B7 File Offset: 0x000038B7
		public void DrawAndFillEllipse(WindowsPen pen, WindowsBrush brush, Rectangle bounds)
		{
			this.DrawEllipse(pen, brush, bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000048DD File Offset: 0x000038DD
		public void DrawText(string text, WindowsFont font, Point pt, Color foreColor)
		{
			this.DrawText(text, font, pt, foreColor, Color.Empty, IntTextFormatFlags.Default);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000048F0 File Offset: 0x000038F0
		public void DrawText(string text, WindowsFont font, Point pt, Color foreColor, Color backColor)
		{
			this.DrawText(text, font, pt, foreColor, backColor, IntTextFormatFlags.Default);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004900 File Offset: 0x00003900
		public void DrawText(string text, WindowsFont font, Point pt, Color foreColor, IntTextFormatFlags flags)
		{
			this.DrawText(text, font, pt, foreColor, Color.Empty, flags);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00004914 File Offset: 0x00003914
		public void DrawText(string text, WindowsFont font, Point pt, Color foreColor, Color backColor, IntTextFormatFlags flags)
		{
			Rectangle rectangle = new Rectangle(pt.X, pt.Y, int.MaxValue, int.MaxValue);
			this.DrawText(text, font, rectangle, foreColor, backColor, flags);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000494F File Offset: 0x0000394F
		public void DrawText(string text, WindowsFont font, Rectangle bounds, Color foreColor)
		{
			this.DrawText(text, font, bounds, foreColor, Color.Empty);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00004961 File Offset: 0x00003961
		public void DrawText(string text, WindowsFont font, Rectangle bounds, Color foreColor, Color backColor)
		{
			this.DrawText(text, font, bounds, foreColor, backColor, IntTextFormatFlags.HorizontalCenter | IntTextFormatFlags.VerticalCenter);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004971 File Offset: 0x00003971
		public void DrawText(string text, WindowsFont font, Rectangle bounds, Color color, IntTextFormatFlags flags)
		{
			this.DrawText(text, font, bounds, color, Color.Empty, flags);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00004988 File Offset: 0x00003988
		public void DrawText(string text, WindowsFont font, Rectangle bounds, Color foreColor, Color backColor, IntTextFormatFlags flags)
		{
			if (string.IsNullOrEmpty(text) || foreColor == Color.Transparent)
			{
				return;
			}
			HandleRef handleRef = new HandleRef(this.dc, this.dc.Hdc);
			if (this.dc.TextAlignment != DeviceContextTextAlignment.Top)
			{
				this.dc.SetTextAlignment(DeviceContextTextAlignment.Top);
			}
			if (!foreColor.IsEmpty && foreColor != this.dc.TextColor)
			{
				this.dc.SetTextColor(foreColor);
			}
			if (font != null)
			{
				this.dc.SelectFont(font);
			}
			DeviceContextBackgroundMode deviceContextBackgroundMode = ((backColor.IsEmpty || backColor == Color.Transparent) ? DeviceContextBackgroundMode.Transparent : DeviceContextBackgroundMode.Opaque);
			if (this.dc.BackgroundMode != deviceContextBackgroundMode)
			{
				this.dc.SetBackgroundMode(deviceContextBackgroundMode);
			}
			if (deviceContextBackgroundMode != DeviceContextBackgroundMode.Transparent && backColor != this.dc.BackgroundColor)
			{
				this.dc.SetBackgroundColor(backColor);
			}
			IntNativeMethods.DRAWTEXTPARAMS textMargins = this.GetTextMargins(font);
			bounds = WindowsGraphics.AdjustForVerticalAlignment(handleRef, text, bounds, flags, textMargins);
			if (bounds.Width == WindowsGraphics.MaxSize.Width)
			{
				bounds.Width -= bounds.X;
			}
			if (bounds.Height == WindowsGraphics.MaxSize.Height)
			{
				bounds.Height -= bounds.Y;
			}
			IntNativeMethods.RECT rect = new IntNativeMethods.RECT(bounds);
			IntUnsafeNativeMethods.DrawTextEx(handleRef, text, ref rect, (int)flags, textMargins);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004B00 File Offset: 0x00003B00
		public Color GetNearestColor(Color color)
		{
			HandleRef handleRef = new HandleRef(null, this.dc.Hdc);
			int nearestColor = IntUnsafeNativeMethods.GetNearestColor(handleRef, ColorTranslator.ToWin32(color));
			return ColorTranslator.FromWin32(nearestColor);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00004B34 File Offset: 0x00003B34
		public float GetOverhangPadding(WindowsFont font)
		{
			WindowsFont windowsFont = font;
			if (windowsFont == null)
			{
				windowsFont = this.dc.Font;
			}
			float num = (float)windowsFont.Height / 6f;
			if (windowsFont != font)
			{
				windowsFont.Dispose();
			}
			return num;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00004B6C File Offset: 0x00003B6C
		public IntNativeMethods.DRAWTEXTPARAMS GetTextMargins(WindowsFont font)
		{
			int num = 0;
			int num2 = 0;
			switch (this.TextPadding)
			{
			case TextPaddingOptions.GlyphOverhangPadding:
			{
				float num3 = this.GetOverhangPadding(font);
				num = (int)Math.Ceiling((double)num3);
				num2 = (int)Math.Ceiling((double)(num3 * 1.5f));
				break;
			}
			case TextPaddingOptions.LeftAndRightPadding:
			{
				float num3 = this.GetOverhangPadding(font);
				num = (int)Math.Ceiling((double)(2f * num3));
				num2 = (int)Math.Ceiling((double)(num3 * 2.5f));
				break;
			}
			}
			return new IntNativeMethods.DRAWTEXTPARAMS(num, num2);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00004BF0 File Offset: 0x00003BF0
		public Size GetTextExtent(string text, WindowsFont font)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
			HandleRef handleRef = new HandleRef(null, this.dc.Hdc);
			if (font != null)
			{
				this.dc.SelectFont(font);
			}
			IntUnsafeNativeMethods.GetTextExtentPoint32(handleRef, text, size);
			if (font != null && !MeasurementDCInfo.IsMeasurementDC(this.dc))
			{
				this.dc.ResetFont();
			}
			return new Size(size.cx, size.cy);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004C69 File Offset: 0x00003C69
		public Size MeasureText(string text, WindowsFont font)
		{
			return this.MeasureText(text, font, WindowsGraphics.MaxSize, IntTextFormatFlags.Default);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00004C79 File Offset: 0x00003C79
		public Size MeasureText(string text, WindowsFont font, Size proposedSize)
		{
			return this.MeasureText(text, font, proposedSize, IntTextFormatFlags.Default);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004C88 File Offset: 0x00003C88
		public Size MeasureText(string text, WindowsFont font, Size proposedSize, IntTextFormatFlags flags)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			IntNativeMethods.DRAWTEXTPARAMS drawtextparams = null;
			if (MeasurementDCInfo.IsMeasurementDC(this.DeviceContext))
			{
				drawtextparams = MeasurementDCInfo.GetTextMargins(this, font);
			}
			if (drawtextparams == null)
			{
				drawtextparams = this.GetTextMargins(font);
			}
			int num = 1 + drawtextparams.iLeftMargin + drawtextparams.iRightMargin;
			if (proposedSize.Width <= num)
			{
				proposedSize.Width = num;
			}
			if (proposedSize.Height <= 0)
			{
				proposedSize.Height = 1;
			}
			IntNativeMethods.RECT rect = IntNativeMethods.RECT.FromXYWH(0, 0, proposedSize.Width, proposedSize.Height);
			HandleRef handleRef = new HandleRef(null, this.dc.Hdc);
			if (font != null)
			{
				this.dc.SelectFont(font);
			}
			if (proposedSize.Height >= WindowsGraphics.MaxSize.Height && (flags & IntTextFormatFlags.SingleLine) != IntTextFormatFlags.Default)
			{
				flags &= ~(IntTextFormatFlags.Bottom | IntTextFormatFlags.VerticalCenter);
			}
			if (proposedSize.Width == WindowsGraphics.MaxSize.Width)
			{
				flags &= ~IntTextFormatFlags.WordBreak;
			}
			flags |= IntTextFormatFlags.CalculateRectangle;
			IntUnsafeNativeMethods.DrawTextEx(handleRef, text, ref rect, (int)flags, drawtextparams);
			return rect.Size;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00004D94 File Offset: 0x00003D94
		public static Rectangle AdjustForVerticalAlignment(HandleRef hdc, string text, Rectangle bounds, IntTextFormatFlags flags, IntNativeMethods.DRAWTEXTPARAMS dtparams)
		{
			if (((flags & IntTextFormatFlags.Bottom) == IntTextFormatFlags.Default && (flags & IntTextFormatFlags.VerticalCenter) == IntTextFormatFlags.Default) || (flags & IntTextFormatFlags.SingleLine) != IntTextFormatFlags.Default || (flags & IntTextFormatFlags.CalculateRectangle) != IntTextFormatFlags.Default)
			{
				return bounds;
			}
			IntNativeMethods.RECT rect = new IntNativeMethods.RECT(bounds);
			flags |= IntTextFormatFlags.CalculateRectangle;
			int num = IntUnsafeNativeMethods.DrawTextEx(hdc, text, ref rect, (int)flags, dtparams);
			if (num > bounds.Height)
			{
				return bounds;
			}
			Rectangle rectangle = bounds;
			if ((flags & IntTextFormatFlags.VerticalCenter) != IntTextFormatFlags.Default)
			{
				rectangle.Y = rectangle.Top + rectangle.Height / 2 - num / 2;
			}
			else
			{
				rectangle.Y = rectangle.Bottom - num;
			}
			return rectangle;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00004E29 File Offset: 0x00003E29
		public void DrawRectangle(WindowsPen pen, Rectangle rect)
		{
			this.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00004E50 File Offset: 0x00003E50
		public void DrawRectangle(WindowsPen pen, int x, int y, int width, int height)
		{
			HandleRef handleRef = new HandleRef(this.dc, this.dc.Hdc);
			if (pen != null)
			{
				this.dc.SelectObject(pen.HPen, GdiObjectType.Pen);
			}
			DeviceContextBinaryRasterOperationFlags deviceContextBinaryRasterOperationFlags = this.dc.BinaryRasterOperation;
			if (deviceContextBinaryRasterOperationFlags != DeviceContextBinaryRasterOperationFlags.CopyPen)
			{
				deviceContextBinaryRasterOperationFlags = this.dc.SetRasterOperation(DeviceContextBinaryRasterOperationFlags.CopyPen);
			}
			IntUnsafeNativeMethods.SelectObject(handleRef, new HandleRef(null, IntUnsafeNativeMethods.GetStockObject(5)));
			IntUnsafeNativeMethods.Rectangle(handleRef, x, y, x + width, y + height);
			if (deviceContextBinaryRasterOperationFlags != DeviceContextBinaryRasterOperationFlags.CopyPen)
			{
				this.dc.SetRasterOperation(deviceContextBinaryRasterOperationFlags);
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004EE0 File Offset: 0x00003EE0
		public void FillRectangle(WindowsBrush brush, Rectangle rect)
		{
			this.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00004F08 File Offset: 0x00003F08
		public void FillRectangle(WindowsBrush brush, int x, int y, int width, int height)
		{
			HandleRef handleRef = new HandleRef(this.dc, this.dc.Hdc);
			IntPtr hbrush = brush.HBrush;
			IntNativeMethods.RECT rect = new IntNativeMethods.RECT(x, y, x + width, y + height);
			IntUnsafeNativeMethods.FillRect(handleRef, ref rect, new HandleRef(brush, hbrush));
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004F5A File Offset: 0x00003F5A
		public void DrawLine(WindowsPen pen, Point p1, Point p2)
		{
			this.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00004F80 File Offset: 0x00003F80
		public void DrawLine(WindowsPen pen, int x1, int y1, int x2, int y2)
		{
			HandleRef handleRef = new HandleRef(this.dc, this.dc.Hdc);
			DeviceContextBinaryRasterOperationFlags deviceContextBinaryRasterOperationFlags = this.dc.BinaryRasterOperation;
			DeviceContextBackgroundMode deviceContextBackgroundMode = this.dc.BackgroundMode;
			if (deviceContextBinaryRasterOperationFlags != DeviceContextBinaryRasterOperationFlags.CopyPen)
			{
				deviceContextBinaryRasterOperationFlags = this.dc.SetRasterOperation(DeviceContextBinaryRasterOperationFlags.CopyPen);
			}
			if (deviceContextBackgroundMode != DeviceContextBackgroundMode.Transparent)
			{
				deviceContextBackgroundMode = this.dc.SetBackgroundMode(DeviceContextBackgroundMode.Transparent);
			}
			if (pen != null)
			{
				this.dc.SelectObject(pen.HPen, GdiObjectType.Pen);
			}
			IntNativeMethods.POINT point = new IntNativeMethods.POINT();
			IntUnsafeNativeMethods.MoveToEx(handleRef, x1, y1, point);
			IntUnsafeNativeMethods.LineTo(handleRef, x2, y2);
			if (deviceContextBackgroundMode != DeviceContextBackgroundMode.Transparent)
			{
				this.dc.SetBackgroundMode(deviceContextBackgroundMode);
			}
			if (deviceContextBinaryRasterOperationFlags != DeviceContextBinaryRasterOperationFlags.CopyPen)
			{
				this.dc.SetRasterOperation(deviceContextBinaryRasterOperationFlags);
			}
			IntUnsafeNativeMethods.MoveToEx(handleRef, point.x, point.y, null);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000504C File Offset: 0x0000404C
		public IntNativeMethods.TEXTMETRIC GetTextMetrics()
		{
			IntNativeMethods.TEXTMETRIC textmetric = default(IntNativeMethods.TEXTMETRIC);
			HandleRef handleRef = new HandleRef(this.dc, this.dc.Hdc);
			DeviceContextMapMode deviceContextMapMode = this.dc.MapMode;
			bool flag = deviceContextMapMode != DeviceContextMapMode.Text;
			if (flag)
			{
				this.dc.SaveHdc();
			}
			try
			{
				if (flag)
				{
					deviceContextMapMode = this.dc.SetMapMode(DeviceContextMapMode.Text);
				}
				IntUnsafeNativeMethods.GetTextMetrics(handleRef, ref textmetric);
			}
			finally
			{
				if (flag)
				{
					this.dc.RestoreHdc();
				}
			}
			return textmetric;
		}

		// Token: 0x04000AD4 RID: 2772
		public const int GdiUnsupportedFlagMask = -16777216;

		// Token: 0x04000AD5 RID: 2773
		private const float ItalicPaddingFactor = 0.5f;

		// Token: 0x04000AD6 RID: 2774
		private DeviceContext dc;

		// Token: 0x04000AD7 RID: 2775
		private bool disposeDc;

		// Token: 0x04000AD8 RID: 2776
		private Graphics graphics;

		// Token: 0x04000AD9 RID: 2777
		public static readonly Size MaxSize = new Size(int.MaxValue, int.MaxValue);

		// Token: 0x04000ADA RID: 2778
		private TextPaddingOptions paddingFlags;
	}
}
