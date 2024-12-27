using System;

namespace System.Windows.Forms
{
	// Token: 0x020005AB RID: 1451
	public class OSFeature : FeatureSupport
	{
		// Token: 0x06004B2A RID: 19242 RVA: 0x00110617 File Offset: 0x0010F617
		protected OSFeature()
		{
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06004B2B RID: 19243 RVA: 0x0011061F File Offset: 0x0010F61F
		public static OSFeature Feature
		{
			get
			{
				if (OSFeature.feature == null)
				{
					OSFeature.feature = new OSFeature();
				}
				return OSFeature.feature;
			}
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x00110638 File Offset: 0x0010F638
		public override Version GetVersionPresent(object feature)
		{
			Version version = null;
			if (feature == OSFeature.LayeredWindows)
			{
				if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.CompareTo(new Version(5, 0, 0, 0)) >= 0)
				{
					version = new Version(0, 0, 0, 0);
				}
			}
			else if (feature == OSFeature.Themes)
			{
				if (!OSFeature.themeSupportTested)
				{
					try
					{
						SafeNativeMethods.IsAppThemed();
						OSFeature.themeSupport = true;
					}
					catch
					{
						OSFeature.themeSupport = false;
					}
					OSFeature.themeSupportTested = true;
				}
				if (OSFeature.themeSupport)
				{
					version = new Version(0, 0, 0, 0);
				}
			}
			return version;
		}

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06004B2D RID: 19245 RVA: 0x001106D4 File Offset: 0x0010F6D4
		internal bool OnXp
		{
			get
			{
				bool flag = false;
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					flag = Environment.OSVersion.Version.CompareTo(new Version(5, 1, 0, 0)) >= 0;
				}
				return flag;
			}
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06004B2E RID: 19246 RVA: 0x00110710 File Offset: 0x0010F710
		internal bool OnWin2k
		{
			get
			{
				bool flag = false;
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					flag = Environment.OSVersion.Version.CompareTo(new Version(5, 0, 0, 0)) >= 0;
				}
				return flag;
			}
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x0011074C File Offset: 0x0010F74C
		public static bool IsPresent(SystemParameter enumVal)
		{
			switch (enumVal)
			{
			case SystemParameter.DropShadow:
				return OSFeature.Feature.OnXp;
			case SystemParameter.FlatMenu:
				return OSFeature.Feature.OnXp;
			case SystemParameter.FontSmoothingContrastMetric:
				return OSFeature.Feature.OnXp;
			case SystemParameter.FontSmoothingTypeMetric:
				return OSFeature.Feature.OnXp;
			case SystemParameter.MenuFadeEnabled:
				return OSFeature.Feature.OnWin2k;
			case SystemParameter.SelectionFade:
				return OSFeature.Feature.OnWin2k;
			case SystemParameter.ToolTipAnimationMetric:
				return OSFeature.Feature.OnWin2k;
			case SystemParameter.UIEffects:
				return OSFeature.Feature.OnWin2k;
			case SystemParameter.CaretWidthMetric:
				return OSFeature.Feature.OnWin2k;
			case SystemParameter.VerticalFocusThicknessMetric:
				return OSFeature.Feature.OnXp;
			case SystemParameter.HorizontalFocusThicknessMetric:
				return OSFeature.Feature.OnXp;
			default:
				return false;
			}
		}

		// Token: 0x040030EF RID: 12527
		public static readonly object LayeredWindows = new object();

		// Token: 0x040030F0 RID: 12528
		public static readonly object Themes = new object();

		// Token: 0x040030F1 RID: 12529
		private static OSFeature feature = null;

		// Token: 0x040030F2 RID: 12530
		private static bool themeSupportTested = false;

		// Token: 0x040030F3 RID: 12531
		private static bool themeSupport = false;
	}
}
