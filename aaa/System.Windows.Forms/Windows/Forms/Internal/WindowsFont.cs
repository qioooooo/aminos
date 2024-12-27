using System;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
	// Token: 0x02000027 RID: 39
	internal sealed class WindowsFont : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x00003CC0 File Offset: 0x00002CC0
		private void CreateFont()
		{
			this.hFont = IntUnsafeNativeMethods.CreateFontIndirect(this.logFont);
			if (this.hFont == IntPtr.Zero)
			{
				this.logFont.lfFaceName = "Microsoft Sans Serif";
				this.logFont.lfOutPrecision = 7;
				this.hFont = IntUnsafeNativeMethods.CreateFontIndirect(this.logFont);
			}
			IntUnsafeNativeMethods.GetObject(new HandleRef(this, this.hFont), this.logFont);
			this.ownHandle = true;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00003D3C File Offset: 0x00002D3C
		public WindowsFont(string faceName)
			: this(faceName, 8.25f, FontStyle.Regular, 1, WindowsFontQuality.Default)
		{
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00003D4D File Offset: 0x00002D4D
		public WindowsFont(string faceName, float size)
			: this(faceName, size, FontStyle.Regular, 1, WindowsFontQuality.Default)
		{
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00003D5A File Offset: 0x00002D5A
		public WindowsFont(string faceName, float size, FontStyle style)
			: this(faceName, size, style, 1, WindowsFontQuality.Default)
		{
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00003D68 File Offset: 0x00002D68
		public WindowsFont(string faceName, float size, FontStyle style, byte charSet, WindowsFontQuality fontQuality)
		{
			this.fontSize = -1f;
			base..ctor();
			this.logFont = new IntNativeMethods.LOGFONT();
			int num = (int)Math.Ceiling((double)((float)WindowsGraphicsCacheManager.MeasurementGraphics.DeviceContext.DpiY * size / 72f));
			this.logFont.lfHeight = -num;
			this.logFont.lfFaceName = ((faceName != null) ? faceName : "Microsoft Sans Serif");
			this.logFont.lfCharSet = charSet;
			this.logFont.lfOutPrecision = 4;
			this.logFont.lfQuality = (byte)fontQuality;
			this.logFont.lfWeight = (((style & FontStyle.Bold) == FontStyle.Bold) ? 700 : 400);
			this.logFont.lfItalic = (((style & FontStyle.Italic) == FontStyle.Italic) ? 1 : 0);
			this.logFont.lfUnderline = (((style & FontStyle.Underline) == FontStyle.Underline) ? 1 : 0);
			this.logFont.lfStrikeOut = (((style & FontStyle.Strikeout) == FontStyle.Strikeout) ? 1 : 0);
			this.style = style;
			this.CreateFont();
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00003E64 File Offset: 0x00002E64
		private WindowsFont(IntNativeMethods.LOGFONT lf, bool createHandle)
		{
			this.fontSize = -1f;
			base..ctor();
			this.logFont = lf;
			if (this.logFont.lfFaceName == null)
			{
				this.logFont.lfFaceName = "Microsoft Sans Serif";
			}
			this.style = FontStyle.Regular;
			if (lf.lfWeight == 700)
			{
				this.style |= FontStyle.Bold;
			}
			if (lf.lfItalic == 1)
			{
				this.style |= FontStyle.Italic;
			}
			if (lf.lfUnderline == 1)
			{
				this.style |= FontStyle.Underline;
			}
			if (lf.lfStrikeOut == 1)
			{
				this.style |= FontStyle.Strikeout;
			}
			if (createHandle)
			{
				this.CreateFont();
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00003F16 File Offset: 0x00002F16
		public static WindowsFont FromFont(Font font)
		{
			return WindowsFont.FromFont(font, WindowsFontQuality.Default);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00003F20 File Offset: 0x00002F20
		public static WindowsFont FromFont(Font font, WindowsFontQuality fontQuality)
		{
			string text = font.FontFamily.Name;
			if (text != null && text.Length > 1 && text[0] == '@')
			{
				text = text.Substring(1);
			}
			return new WindowsFont(text, font.SizeInPoints, font.Style, font.GdiCharSet, fontQuality);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00003F74 File Offset: 0x00002F74
		public static WindowsFont FromHdc(IntPtr hdc)
		{
			IntPtr currentObject = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(null, hdc), 6);
			return WindowsFont.FromHfont(currentObject);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00003F95 File Offset: 0x00002F95
		public static WindowsFont FromHfont(IntPtr hFont)
		{
			return WindowsFont.FromHfont(hFont, false);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00003FA0 File Offset: 0x00002FA0
		public static WindowsFont FromHfont(IntPtr hFont, bool takeOwnership)
		{
			IntNativeMethods.LOGFONT logfont = new IntNativeMethods.LOGFONT();
			IntUnsafeNativeMethods.GetObject(new HandleRef(null, hFont), logfont);
			return new WindowsFont(logfont, false)
			{
				hFont = hFont,
				ownHandle = takeOwnership
			};
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00003FD8 File Offset: 0x00002FD8
		~WindowsFont()
		{
			this.Dispose(false);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004008 File Offset: 0x00003008
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004014 File Offset: 0x00003014
		internal void Dispose(bool disposing)
		{
			bool flag = false;
			if (this.ownHandle && (!this.ownedByCacheManager || !disposing) && (this.everOwnedByCacheManager || !disposing || !DeviceContexts.IsFontInUse(this)))
			{
				IntUnsafeNativeMethods.DeleteObject(new HandleRef(this, this.hFont));
				this.hFont = IntPtr.Zero;
				this.ownHandle = false;
				flag = true;
			}
			if (disposing && (flag || !this.ownHandle))
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004084 File Offset: 0x00003084
		public override bool Equals(object font)
		{
			WindowsFont windowsFont = font as WindowsFont;
			return windowsFont != null && (windowsFont == this || (this.Name == windowsFont.Name && this.LogFontHeight == windowsFont.LogFontHeight && this.Style == windowsFont.Style && this.CharSet == windowsFont.CharSet && this.Quality == windowsFont.Quality));
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000040F0 File Offset: 0x000030F0
		public override int GetHashCode()
		{
			return (int)((((int)this.Style << 13) | (this.Style >> 19)) ^ (FontStyle)(((int)this.CharSet << 26) | (int)((uint)this.CharSet >> 6)) ^ (FontStyle)(((uint)this.Size << 7) | ((uint)this.Size >> 25)));
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000412D File Offset: 0x0000312D
		public object Clone()
		{
			return new WindowsFont(this.logFont, true);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000413C File Offset: 0x0000313C
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "[{0}: Name={1}, Size={2} points, Height={3} pixels, Sytle={4}]", new object[]
			{
				base.GetType().Name,
				this.logFont.lfFaceName,
				this.Size,
				this.Height,
				this.Style
			});
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000041A6 File Offset: 0x000031A6
		public IntPtr Hfont
		{
			get
			{
				return this.hFont;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000041AE File Offset: 0x000031AE
		public bool Italic
		{
			get
			{
				return this.logFont.lfItalic == 1;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000041BE File Offset: 0x000031BE
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000041C6 File Offset: 0x000031C6
		public bool OwnedByCacheManager
		{
			get
			{
				return this.ownedByCacheManager;
			}
			set
			{
				if (value)
				{
					this.everOwnedByCacheManager = true;
				}
				this.ownedByCacheManager = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000041D9 File Offset: 0x000031D9
		public WindowsFontQuality Quality
		{
			get
			{
				return (WindowsFontQuality)this.logFont.lfQuality;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000041E6 File Offset: 0x000031E6
		public FontStyle Style
		{
			get
			{
				return this.style;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000FE RID: 254 RVA: 0x000041F0 File Offset: 0x000031F0
		public int Height
		{
			get
			{
				if (this.lineSpacing == 0)
				{
					WindowsGraphics measurementGraphics = WindowsGraphicsCacheManager.MeasurementGraphics;
					measurementGraphics.DeviceContext.SelectFont(this);
					this.lineSpacing = measurementGraphics.GetTextMetrics().tmHeight;
				}
				return this.lineSpacing;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00004232 File Offset: 0x00003232
		public byte CharSet
		{
			get
			{
				return this.logFont.lfCharSet;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000423F File Offset: 0x0000323F
		public int LogFontHeight
		{
			get
			{
				return this.logFont.lfHeight;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000424C File Offset: 0x0000324C
		public string Name
		{
			get
			{
				return this.logFont.lfFaceName;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000425C File Offset: 0x0000325C
		public float Size
		{
			get
			{
				if (this.fontSize < 0f)
				{
					WindowsGraphics measurementGraphics = WindowsGraphicsCacheManager.MeasurementGraphics;
					measurementGraphics.DeviceContext.SelectFont(this);
					IntNativeMethods.TEXTMETRIC textMetrics = measurementGraphics.GetTextMetrics();
					int num = ((this.logFont.lfHeight > 0) ? textMetrics.tmHeight : (textMetrics.tmHeight - textMetrics.tmInternalLeading));
					this.fontSize = (float)num * 72f / (float)measurementGraphics.DeviceContext.DpiY;
				}
				return this.fontSize;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000042D8 File Offset: 0x000032D8
		public static WindowsFontQuality WindowsFontQualityFromTextRenderingHint(Graphics g)
		{
			if (g == null)
			{
				return WindowsFontQuality.Default;
			}
			switch (g.TextRenderingHint)
			{
			case TextRenderingHint.SingleBitPerPixelGridFit:
				return WindowsFontQuality.Proof;
			case TextRenderingHint.SingleBitPerPixel:
				return WindowsFontQuality.Draft;
			case TextRenderingHint.AntiAliasGridFit:
				return WindowsFontQuality.AntiAliased;
			case TextRenderingHint.AntiAlias:
				return WindowsFontQuality.AntiAliased;
			case TextRenderingHint.ClearTypeGridFit:
				if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)
				{
					return WindowsFontQuality.ClearTypeNatural;
				}
				return WindowsFontQuality.ClearType;
			}
			return WindowsFontQuality.Default;
		}

		// Token: 0x04000ABC RID: 2748
		private const int LogFontNameOffset = 28;

		// Token: 0x04000ABD RID: 2749
		private const string defaultFaceName = "Microsoft Sans Serif";

		// Token: 0x04000ABE RID: 2750
		private const float defaultFontSize = 8.25f;

		// Token: 0x04000ABF RID: 2751
		private const int defaultFontHeight = 13;

		// Token: 0x04000AC0 RID: 2752
		private IntPtr hFont;

		// Token: 0x04000AC1 RID: 2753
		private float fontSize;

		// Token: 0x04000AC2 RID: 2754
		private int lineSpacing;

		// Token: 0x04000AC3 RID: 2755
		private bool ownHandle;

		// Token: 0x04000AC4 RID: 2756
		private bool ownedByCacheManager;

		// Token: 0x04000AC5 RID: 2757
		private bool everOwnedByCacheManager;

		// Token: 0x04000AC6 RID: 2758
		private IntNativeMethods.LOGFONT logFont;

		// Token: 0x04000AC7 RID: 2759
		private FontStyle style;
	}
}
