using System;

namespace System.Windows.Forms.Internal
{
	// Token: 0x0200001F RID: 31
	internal static class MeasurementDCInfo
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00003194 File Offset: 0x00002194
		internal static bool IsMeasurementDC(DeviceContext dc)
		{
			WindowsGraphics currentMeasurementGraphics = WindowsGraphicsCacheManager.GetCurrentMeasurementGraphics();
			return currentMeasurementGraphics != null && currentMeasurementGraphics.DeviceContext != null && currentMeasurementGraphics.DeviceContext.Hdc == dc.Hdc;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000031CA File Offset: 0x000021CA
		// (set) Token: 0x0600005F RID: 95 RVA: 0x000031DF File Offset: 0x000021DF
		internal static WindowsFont LastUsedFont
		{
			get
			{
				if (MeasurementDCInfo.cachedMeasurementDCInfo != null)
				{
					return MeasurementDCInfo.cachedMeasurementDCInfo.LastUsedFont;
				}
				return null;
			}
			set
			{
				if (MeasurementDCInfo.cachedMeasurementDCInfo == null)
				{
					MeasurementDCInfo.cachedMeasurementDCInfo = new MeasurementDCInfo.CachedInfo();
				}
				MeasurementDCInfo.cachedMeasurementDCInfo.UpdateFont(value);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003200 File Offset: 0x00002200
		internal static IntNativeMethods.DRAWTEXTPARAMS GetTextMargins(WindowsGraphics wg, WindowsFont font)
		{
			MeasurementDCInfo.CachedInfo cachedInfo = MeasurementDCInfo.cachedMeasurementDCInfo;
			if (cachedInfo != null && cachedInfo.LeftTextMargin > 0 && cachedInfo.RightTextMargin > 0 && font == cachedInfo.LastUsedFont)
			{
				return new IntNativeMethods.DRAWTEXTPARAMS(cachedInfo.LeftTextMargin, cachedInfo.RightTextMargin);
			}
			if (cachedInfo == null)
			{
				cachedInfo = new MeasurementDCInfo.CachedInfo();
				MeasurementDCInfo.cachedMeasurementDCInfo = cachedInfo;
			}
			IntNativeMethods.DRAWTEXTPARAMS textMargins = wg.GetTextMargins(font);
			cachedInfo.LeftTextMargin = textMargins.iLeftMargin;
			cachedInfo.RightTextMargin = textMargins.iRightMargin;
			return new IntNativeMethods.DRAWTEXTPARAMS(cachedInfo.LeftTextMargin, cachedInfo.RightTextMargin);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003284 File Offset: 0x00002284
		internal static void ResetIfIsMeasurementDC(IntPtr hdc)
		{
			WindowsGraphics currentMeasurementGraphics = WindowsGraphicsCacheManager.GetCurrentMeasurementGraphics();
			if (currentMeasurementGraphics != null && currentMeasurementGraphics.DeviceContext != null && currentMeasurementGraphics.DeviceContext.Hdc == hdc)
			{
				MeasurementDCInfo.CachedInfo cachedInfo = MeasurementDCInfo.cachedMeasurementDCInfo;
				if (cachedInfo != null)
				{
					cachedInfo.UpdateFont(null);
				}
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000032C8 File Offset: 0x000022C8
		internal static void Reset()
		{
			MeasurementDCInfo.CachedInfo cachedInfo = MeasurementDCInfo.cachedMeasurementDCInfo;
			if (cachedInfo != null)
			{
				cachedInfo.UpdateFont(null);
			}
		}

		// Token: 0x04000A94 RID: 2708
		[ThreadStatic]
		private static MeasurementDCInfo.CachedInfo cachedMeasurementDCInfo;

		// Token: 0x02000020 RID: 32
		private sealed class CachedInfo
		{
			// Token: 0x06000063 RID: 99 RVA: 0x000032E5 File Offset: 0x000022E5
			internal void UpdateFont(WindowsFont font)
			{
				if (this.LastUsedFont != font)
				{
					this.LastUsedFont = font;
					this.LeftTextMargin = -1;
					this.RightTextMargin = -1;
				}
			}

			// Token: 0x04000A95 RID: 2709
			public WindowsFont LastUsedFont;

			// Token: 0x04000A96 RID: 2710
			public int LeftTextMargin;

			// Token: 0x04000A97 RID: 2711
			public int RightTextMargin;
		}
	}
}
