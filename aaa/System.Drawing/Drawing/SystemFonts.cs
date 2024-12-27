using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing
{
	// Token: 0x02000065 RID: 101
	public sealed class SystemFonts
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x0001A006 File Offset: 0x00019006
		private SystemFonts()
		{
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001A010 File Offset: 0x00019010
		public static Font CaptionFont
		{
			get
			{
				Font font = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfCaptionFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(nonclientmetrics.lfCaptionFont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("CaptionFont");
				return font;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0001A0B0 File Offset: 0x000190B0
		public static Font SmallCaptionFont
		{
			get
			{
				Font font = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfSmCaptionFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(nonclientmetrics.lfSmCaptionFont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("SmallCaptionFont");
				return font;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x0001A150 File Offset: 0x00019150
		public static Font MenuFont
		{
			get
			{
				Font font = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfMenuFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(nonclientmetrics.lfMenuFont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("MenuFont");
				return font;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001A1F0 File Offset: 0x000191F0
		public static Font StatusFont
		{
			get
			{
				Font font = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfStatusFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(nonclientmetrics.lfStatusFont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("StatusFont");
				return font;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0001A290 File Offset: 0x00019290
		public static Font MessageBoxFont
		{
			get
			{
				Font font = null;
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.lfMessageFont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(nonclientmetrics.lfMessageFont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("MessageBoxFont");
				return font;
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001A330 File Offset: 0x00019330
		private static bool IsCriticalFontException(Exception ex)
		{
			return !(ex is ExternalException) && !(ex is ArgumentException) && !(ex is OutOfMemoryException) && !(ex is InvalidOperationException) && !(ex is NotImplementedException) && !(ex is FileNotFoundException);
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x0001A368 File Offset: 0x00019368
		public static Font IconTitleFont
		{
			get
			{
				Font font = null;
				SafeNativeMethods.LOGFONT logfont = new SafeNativeMethods.LOGFONT();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(31, Marshal.SizeOf(logfont), logfont, 0);
				if (flag && logfont != null)
				{
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						font = Font.FromLogFont(logfont);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (font == null)
					{
						font = SystemFonts.DefaultFont;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						font = SystemFonts.FontInPoints(font);
					}
				}
				font.SetSystemFontName("IconTitleFont");
				return font;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001A400 File Offset: 0x00019400
		public static Font DefaultFont
		{
			get
			{
				Font font = null;
				bool flag = false;
				if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major <= 4 && (UnsafeNativeMethods.GetSystemDefaultLCID() & 1023) == 17)
				{
					try
					{
						font = new Font("MS UI Gothic", 9f);
					}
					catch (Exception ex)
					{
						if (SystemFonts.IsCriticalFontException(ex))
						{
							throw;
						}
					}
				}
				if (font == null)
				{
					flag = (UnsafeNativeMethods.GetSystemDefaultLCID() & 1023) == 1;
				}
				if (flag)
				{
					try
					{
						font = new Font("Tahoma", 8f);
					}
					catch (Exception ex2)
					{
						if (SystemFonts.IsCriticalFontException(ex2))
						{
							throw;
						}
					}
				}
				if (font == null)
				{
					IntPtr stockObject = UnsafeNativeMethods.GetStockObject(17);
					try
					{
						Font font2 = null;
						IntSecurity.ObjectFromWin32Handle.Assert();
						try
						{
							font2 = Font.FromHfont(stockObject);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						try
						{
							font = SystemFonts.FontInPoints(font2);
						}
						finally
						{
							font2.Dispose();
						}
					}
					catch (ArgumentException)
					{
					}
				}
				if (font == null)
				{
					try
					{
						font = new Font("Tahoma", 8f);
					}
					catch (ArgumentException)
					{
					}
				}
				if (font == null)
				{
					font = new Font(FontFamily.GenericSansSerif, 8f);
				}
				if (font.Unit != GraphicsUnit.Point)
				{
					font = SystemFonts.FontInPoints(font);
				}
				font.SetSystemFontName("DefaultFont");
				return font;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0001A568 File Offset: 0x00019568
		public static Font DialogFont
		{
			get
			{
				Font font = null;
				if ((UnsafeNativeMethods.GetSystemDefaultLCID() & 1023) == 17)
				{
					font = SystemFonts.DefaultFont;
				}
				else if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
				{
					font = SystemFonts.DefaultFont;
				}
				else
				{
					try
					{
						font = new Font("Tahoma", 8f);
					}
					catch (ArgumentException)
					{
					}
				}
				if (font == null)
				{
					font = SystemFonts.DefaultFont;
				}
				else if (font.Unit != GraphicsUnit.Point)
				{
					font = SystemFonts.FontInPoints(font);
				}
				font.SetSystemFontName("DialogFont");
				return font;
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001A5F0 File Offset: 0x000195F0
		private static Font FontInPoints(Font font)
		{
			return new Font(font.FontFamily, font.SizeInPoints, font.Style, GraphicsUnit.Point, font.GdiCharSet, font.GdiVerticalFont);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001A618 File Offset: 0x00019618
		public static Font GetFontByName(string systemFontName)
		{
			if ("CaptionFont".Equals(systemFontName))
			{
				return SystemFonts.CaptionFont;
			}
			if ("DefaultFont".Equals(systemFontName))
			{
				return SystemFonts.DefaultFont;
			}
			if ("DialogFont".Equals(systemFontName))
			{
				return SystemFonts.DialogFont;
			}
			if ("IconTitleFont".Equals(systemFontName))
			{
				return SystemFonts.IconTitleFont;
			}
			if ("MenuFont".Equals(systemFontName))
			{
				return SystemFonts.MenuFont;
			}
			if ("MessageBoxFont".Equals(systemFontName))
			{
				return SystemFonts.MessageBoxFont;
			}
			if ("SmallCaptionFont".Equals(systemFontName))
			{
				return SystemFonts.SmallCaptionFont;
			}
			if ("StatusFont".Equals(systemFontName))
			{
				return SystemFonts.StatusFont;
			}
			return null;
		}

		// Token: 0x04000480 RID: 1152
		private static readonly object SystemFontsKey = new object();
	}
}
