using System;
using System.Drawing;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms
{
	// Token: 0x020008C8 RID: 2248
	public sealed class TextRenderer
	{
		// Token: 0x06006D2B RID: 27947 RVA: 0x00190A29 File Offset: 0x0018FA29
		private TextRenderer()
		{
		}

		// Token: 0x06006D2C RID: 27948 RVA: 0x00190A34 File Offset: 0x0018FA34
		public static void DrawText(IDeviceContext dc, string text, Font font, Point pt, Color foreColor)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
					{
						windowsGraphics.DrawText(text, windowsFont, pt, foreColor);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
		}

		// Token: 0x06006D2D RID: 27949 RVA: 0x00190AC8 File Offset: 0x0018FAC8
		public static void DrawText(IDeviceContext dc, string text, Font font, Point pt, Color foreColor, Color backColor)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
					{
						windowsGraphics.DrawText(text, windowsFont, pt, foreColor, backColor);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
		}

		// Token: 0x06006D2E RID: 27950 RVA: 0x00190B5C File Offset: 0x0018FB5C
		public static void DrawText(IDeviceContext dc, string text, Font font, Point pt, Color foreColor, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, flags))
			{
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
				{
					windowsGraphicsWrapper.WindowsGraphics.DrawText(text, windowsFont, pt, foreColor, TextRenderer.GetIntTextFormatFlags(flags));
				}
			}
		}

		// Token: 0x06006D2F RID: 27951 RVA: 0x00190BE0 File Offset: 0x0018FBE0
		public static void DrawText(IDeviceContext dc, string text, Font font, Point pt, Color foreColor, Color backColor, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, flags))
			{
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
				{
					windowsGraphicsWrapper.WindowsGraphics.DrawText(text, windowsFont, pt, foreColor, backColor, TextRenderer.GetIntTextFormatFlags(flags));
				}
			}
		}

		// Token: 0x06006D30 RID: 27952 RVA: 0x00190C68 File Offset: 0x0018FC68
		public static void DrawText(IDeviceContext dc, string text, Font font, Rectangle bounds, Color foreColor)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
					{
						windowsGraphics.DrawText(text, windowsFont, bounds, foreColor);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
		}

		// Token: 0x06006D31 RID: 27953 RVA: 0x00190CFC File Offset: 0x0018FCFC
		public static void DrawText(IDeviceContext dc, string text, Font font, Rectangle bounds, Color foreColor, Color backColor)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
					{
						windowsGraphics.DrawText(text, windowsFont, bounds, foreColor, backColor);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
		}

		// Token: 0x06006D32 RID: 27954 RVA: 0x00190D90 File Offset: 0x0018FD90
		public static void DrawText(IDeviceContext dc, string text, Font font, Rectangle bounds, Color foreColor, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, flags))
			{
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
				{
					windowsGraphicsWrapper.WindowsGraphics.DrawText(text, windowsFont, bounds, foreColor, TextRenderer.GetIntTextFormatFlags(flags));
				}
			}
		}

		// Token: 0x06006D33 RID: 27955 RVA: 0x00190E14 File Offset: 0x0018FE14
		public static void DrawText(IDeviceContext dc, string text, Font font, Rectangle bounds, Color foreColor, Color backColor, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, flags))
			{
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
				{
					windowsGraphicsWrapper.WindowsGraphics.DrawText(text, windowsFont, bounds, foreColor, backColor, TextRenderer.GetIntTextFormatFlags(flags));
				}
			}
		}

		// Token: 0x06006D34 RID: 27956 RVA: 0x00190E9C File Offset: 0x0018FE9C
		private static IntTextFormatFlags GetIntTextFormatFlags(TextFormatFlags flags)
		{
			if (((ulong)flags & 18446744073692774400UL) == 0UL)
			{
				return (IntTextFormatFlags)flags;
			}
			return (IntTextFormatFlags)(flags & (TextFormatFlags)16777215);
		}

		// Token: 0x06006D35 RID: 27957 RVA: 0x00190EC4 File Offset: 0x0018FEC4
		public static Size MeasureText(string text, Font font)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			Size size;
			using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font))
			{
				size = WindowsGraphicsCacheManager.MeasurementGraphics.MeasureText(text, windowsFont);
			}
			return size;
		}

		// Token: 0x06006D36 RID: 27958 RVA: 0x00190F10 File Offset: 0x0018FF10
		public static Size MeasureText(string text, Font font, Size proposedSize)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			Size size;
			using (WindowsGraphicsCacheManager.GetWindowsFont(font))
			{
				size = WindowsGraphicsCacheManager.MeasurementGraphics.MeasureText(text, WindowsGraphicsCacheManager.GetWindowsFont(font), proposedSize);
			}
			return size;
		}

		// Token: 0x06006D37 RID: 27959 RVA: 0x00190F64 File Offset: 0x0018FF64
		public static Size MeasureText(string text, Font font, Size proposedSize, TextFormatFlags flags)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			Size size;
			using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font))
			{
				size = WindowsGraphicsCacheManager.MeasurementGraphics.MeasureText(text, windowsFont, proposedSize, TextRenderer.GetIntTextFormatFlags(flags));
			}
			return size;
		}

		// Token: 0x06006D38 RID: 27960 RVA: 0x00190FB8 File Offset: 0x0018FFB8
		public static Size MeasureText(IDeviceContext dc, string text, Font font)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			Size size;
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
					{
						size = windowsGraphics.MeasureText(text, windowsFont);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
			return size;
		}

		// Token: 0x06006D39 RID: 27961 RVA: 0x00191054 File Offset: 0x00190054
		public static Size MeasureText(IDeviceContext dc, string text, Font font, Size proposedSize)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			Size size;
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
					{
						size = windowsGraphics.MeasureText(text, windowsFont, proposedSize);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
			return size;
		}

		// Token: 0x06006D3A RID: 27962 RVA: 0x001910F4 File Offset: 0x001900F4
		public static Size MeasureText(IDeviceContext dc, string text, Font font, Size proposedSize, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			WindowsFontQuality windowsFontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			Size size;
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, flags))
			{
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, windowsFontQuality))
				{
					size = windowsGraphicsWrapper.WindowsGraphics.MeasureText(text, windowsFont, proposedSize, TextRenderer.GetIntTextFormatFlags(flags));
				}
			}
			return size;
		}

		// Token: 0x06006D3B RID: 27963 RVA: 0x00191184 File Offset: 0x00190184
		internal static Color DisabledTextColor(Color backColor)
		{
			Color color = SystemColors.ControlDark;
			if (ControlPaint.IsDarker(backColor, SystemColors.Control))
			{
				color = ControlPaint.Dark(backColor);
			}
			return color;
		}
	}
}
