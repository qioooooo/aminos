using System;
using System.Collections.Generic;
using System.Drawing;

namespace System.Windows.Forms.Internal
{
	// Token: 0x02000029 RID: 41
	internal class WindowsGraphicsCacheManager
	{
		// Token: 0x06000105 RID: 261 RVA: 0x00004344 File Offset: 0x00003344
		private WindowsGraphicsCacheManager()
		{
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000434C File Offset: 0x0000334C
		private static List<KeyValuePair<Font, WindowsFont>> WindowsFontCache
		{
			get
			{
				if (WindowsGraphicsCacheManager.windowsFontCache == null)
				{
					WindowsGraphicsCacheManager.currentIndex = -1;
					WindowsGraphicsCacheManager.windowsFontCache = new List<KeyValuePair<Font, WindowsFont>>(10);
				}
				return WindowsGraphicsCacheManager.windowsFontCache;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000436C File Offset: 0x0000336C
		public static WindowsGraphics MeasurementGraphics
		{
			get
			{
				if (WindowsGraphicsCacheManager.measurementGraphics == null || WindowsGraphicsCacheManager.measurementGraphics.DeviceContext == null)
				{
					WindowsGraphicsCacheManager.measurementGraphics = WindowsGraphics.CreateMeasurementWindowsGraphics();
				}
				return WindowsGraphicsCacheManager.measurementGraphics;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004390 File Offset: 0x00003390
		internal static WindowsGraphics GetCurrentMeasurementGraphics()
		{
			return WindowsGraphicsCacheManager.measurementGraphics;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004397 File Offset: 0x00003397
		public static WindowsFont GetWindowsFont(Font font)
		{
			return WindowsGraphicsCacheManager.GetWindowsFont(font, WindowsFontQuality.Default);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000043A0 File Offset: 0x000033A0
		public static WindowsFont GetWindowsFont(Font font, WindowsFontQuality fontQuality)
		{
			if (font == null)
			{
				return null;
			}
			int i = 0;
			int num = WindowsGraphicsCacheManager.currentIndex;
			while (i < WindowsGraphicsCacheManager.WindowsFontCache.Count)
			{
				if (WindowsGraphicsCacheManager.WindowsFontCache[num].Key.Equals(font))
				{
					WindowsFont value = WindowsGraphicsCacheManager.WindowsFontCache[num].Value;
					if (value.Quality == fontQuality)
					{
						return value;
					}
				}
				num--;
				i++;
				if (num < 0)
				{
					num = 9;
				}
			}
			WindowsFont windowsFont = WindowsFont.FromFont(font, fontQuality);
			KeyValuePair<Font, WindowsFont> keyValuePair = new KeyValuePair<Font, WindowsFont>(font, windowsFont);
			WindowsGraphicsCacheManager.currentIndex++;
			if (WindowsGraphicsCacheManager.currentIndex == 10)
			{
				WindowsGraphicsCacheManager.currentIndex = 0;
			}
			if (WindowsGraphicsCacheManager.WindowsFontCache.Count == 10)
			{
				WindowsFont windowsFont2 = null;
				bool flag = false;
				int num2 = WindowsGraphicsCacheManager.currentIndex;
				int num3 = num2 + 1;
				while (!flag)
				{
					if (num3 >= 10)
					{
						num3 = 0;
					}
					if (num3 == num2)
					{
						flag = true;
					}
					windowsFont2 = WindowsGraphicsCacheManager.WindowsFontCache[num3].Value;
					if (!DeviceContexts.IsFontInUse(windowsFont2))
					{
						WindowsGraphicsCacheManager.currentIndex = num3;
						break;
					}
					num3++;
					windowsFont2 = null;
				}
				if (windowsFont2 != null)
				{
					WindowsGraphicsCacheManager.WindowsFontCache[WindowsGraphicsCacheManager.currentIndex] = keyValuePair;
					windowsFont.OwnedByCacheManager = true;
					windowsFont2.OwnedByCacheManager = false;
					windowsFont2.Dispose();
				}
				else
				{
					windowsFont.OwnedByCacheManager = false;
				}
			}
			else
			{
				windowsFont.OwnedByCacheManager = true;
				WindowsGraphicsCacheManager.WindowsFontCache.Add(keyValuePair);
			}
			return windowsFont;
		}

		// Token: 0x04000AD0 RID: 2768
		private const int CacheSize = 10;

		// Token: 0x04000AD1 RID: 2769
		[ThreadStatic]
		private static WindowsGraphics measurementGraphics;

		// Token: 0x04000AD2 RID: 2770
		[ThreadStatic]
		private static int currentIndex;

		// Token: 0x04000AD3 RID: 2771
		[ThreadStatic]
		private static List<KeyValuePair<Font, WindowsFont>> windowsFontCache;
	}
}
