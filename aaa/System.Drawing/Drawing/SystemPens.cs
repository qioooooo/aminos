using System;

namespace System.Drawing
{
	// Token: 0x02000067 RID: 103
	public sealed class SystemPens
	{
		// Token: 0x06000692 RID: 1682 RVA: 0x0001A858 File Offset: 0x00019858
		private SystemPens()
		{
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0001A860 File Offset: 0x00019860
		public static Pen ActiveBorder
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ActiveBorder);
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x0001A86C File Offset: 0x0001986C
		public static Pen ActiveCaption
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ActiveCaption);
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0001A878 File Offset: 0x00019878
		public static Pen ActiveCaptionText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ActiveCaptionText);
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0001A884 File Offset: 0x00019884
		public static Pen AppWorkspace
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.AppWorkspace);
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0001A890 File Offset: 0x00019890
		public static Pen ButtonFace
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ButtonFace);
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0001A89C File Offset: 0x0001989C
		public static Pen ButtonHighlight
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ButtonHighlight);
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001A8A8 File Offset: 0x000198A8
		public static Pen ButtonShadow
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ButtonShadow);
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001A8B4 File Offset: 0x000198B4
		public static Pen Control
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Control);
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0001A8C0 File Offset: 0x000198C0
		public static Pen ControlText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ControlText);
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x0001A8CC File Offset: 0x000198CC
		public static Pen ControlDark
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ControlDark);
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001A8D8 File Offset: 0x000198D8
		public static Pen ControlDarkDark
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ControlDarkDark);
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x0001A8E4 File Offset: 0x000198E4
		public static Pen ControlLight
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ControlLight);
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001A8F0 File Offset: 0x000198F0
		public static Pen ControlLightLight
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ControlLightLight);
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0001A8FC File Offset: 0x000198FC
		public static Pen Desktop
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Desktop);
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001A908 File Offset: 0x00019908
		public static Pen GradientActiveCaption
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.GradientActiveCaption);
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0001A914 File Offset: 0x00019914
		public static Pen GradientInactiveCaption
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.GradientInactiveCaption);
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0001A920 File Offset: 0x00019920
		public static Pen GrayText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.GrayText);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0001A92C File Offset: 0x0001992C
		public static Pen Highlight
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Highlight);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0001A938 File Offset: 0x00019938
		public static Pen HighlightText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.HighlightText);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x0001A944 File Offset: 0x00019944
		public static Pen HotTrack
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.HotTrack);
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0001A950 File Offset: 0x00019950
		public static Pen InactiveBorder
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.InactiveBorder);
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x0001A95C File Offset: 0x0001995C
		public static Pen InactiveCaption
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.InactiveCaption);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0001A968 File Offset: 0x00019968
		public static Pen InactiveCaptionText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.InactiveCaptionText);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x0001A974 File Offset: 0x00019974
		public static Pen Info
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Info);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0001A980 File Offset: 0x00019980
		public static Pen InfoText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.InfoText);
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0001A98C File Offset: 0x0001998C
		public static Pen Menu
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Menu);
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001A998 File Offset: 0x00019998
		public static Pen MenuBar
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.MenuBar);
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0001A9A4 File Offset: 0x000199A4
		public static Pen MenuHighlight
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.MenuHighlight);
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0001A9B0 File Offset: 0x000199B0
		public static Pen MenuText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.MenuText);
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x0001A9BC File Offset: 0x000199BC
		public static Pen ScrollBar
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ScrollBar);
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001A9C8 File Offset: 0x000199C8
		public static Pen Window
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Window);
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x0001A9D4 File Offset: 0x000199D4
		public static Pen WindowFrame
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.WindowFrame);
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0001A9E0 File Offset: 0x000199E0
		public static Pen WindowText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.WindowText);
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001A9EC File Offset: 0x000199EC
		public static Pen FromSystemColor(Color c)
		{
			if (!c.IsSystemColor)
			{
				throw new ArgumentException(SR.GetString("ColorNotSystemColor", new object[] { c.ToString() }));
			}
			Pen[] array = (Pen[])SafeNativeMethods.Gdip.ThreadData[SystemPens.SystemPensKey];
			if (array == null)
			{
				array = new Pen[33];
				SafeNativeMethods.Gdip.ThreadData[SystemPens.SystemPensKey] = array;
			}
			int num = (int)c.ToKnownColor();
			if (num > 167)
			{
				num -= 141;
			}
			num--;
			if (array[num] == null)
			{
				array[num] = new Pen(c, true);
			}
			return array[num];
		}

		// Token: 0x0400048B RID: 1163
		private static readonly object SystemPensKey = new object();
	}
}
