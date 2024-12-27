using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Drawing
{
	// Token: 0x0200007A RID: 122
	public sealed class ColorTranslator
	{
		// Token: 0x06000784 RID: 1924 RVA: 0x0001C10D File Offset: 0x0001B10D
		private ColorTranslator()
		{
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001C115 File Offset: 0x0001B115
		public static int ToWin32(Color c)
		{
			return (int)c.R | ((int)c.G << 8) | ((int)c.B << 16);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0001C134 File Offset: 0x0001B134
		public static int ToOle(Color c)
		{
			if (c.IsKnownColor)
			{
				KnownColor knownColor = c.ToKnownColor();
				switch (knownColor)
				{
				case KnownColor.ActiveBorder:
					return -2147483638;
				case KnownColor.ActiveCaption:
					return -2147483646;
				case KnownColor.ActiveCaptionText:
					return -2147483639;
				case KnownColor.AppWorkspace:
					return -2147483636;
				case KnownColor.Control:
					return -2147483633;
				case KnownColor.ControlDark:
					return -2147483632;
				case KnownColor.ControlDarkDark:
					return -2147483627;
				case KnownColor.ControlLight:
					return -2147483626;
				case KnownColor.ControlLightLight:
					return -2147483628;
				case KnownColor.ControlText:
					return -2147483630;
				case KnownColor.Desktop:
					return -2147483647;
				case KnownColor.GrayText:
					return -2147483631;
				case KnownColor.Highlight:
					return -2147483635;
				case KnownColor.HighlightText:
					return -2147483634;
				case KnownColor.HotTrack:
					return -2147483635;
				case KnownColor.InactiveBorder:
					return -2147483637;
				case KnownColor.InactiveCaption:
					return -2147483645;
				case KnownColor.InactiveCaptionText:
					return -2147483629;
				case KnownColor.Info:
					return -2147483624;
				case KnownColor.InfoText:
					return -2147483625;
				case KnownColor.Menu:
					return -2147483644;
				case KnownColor.MenuText:
					return -2147483641;
				case KnownColor.ScrollBar:
					return int.MinValue;
				case KnownColor.Window:
					return -2147483643;
				case KnownColor.WindowFrame:
					return -2147483642;
				case KnownColor.WindowText:
					return -2147483640;
				default:
					switch (knownColor)
					{
					case KnownColor.ButtonFace:
						return -2147483633;
					case KnownColor.ButtonHighlight:
						return -2147483628;
					case KnownColor.ButtonShadow:
						return -2147483632;
					case KnownColor.GradientActiveCaption:
						return -2147483621;
					case KnownColor.GradientInactiveCaption:
						return -2147483620;
					case KnownColor.MenuBar:
						return -2147483618;
					case KnownColor.MenuHighlight:
						return -2147483619;
					}
					break;
				}
			}
			return ColorTranslator.ToWin32(c);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0001C2C0 File Offset: 0x0001B2C0
		public static Color FromOle(int oleColor)
		{
			if ((int)((long)oleColor & (long)((ulong)(-16777216))) == -2147483648 && (oleColor & 16777215) <= 24)
			{
				switch (oleColor)
				{
				case -2147483648:
					return Color.FromKnownColor(KnownColor.ScrollBar);
				case -2147483647:
					return Color.FromKnownColor(KnownColor.Desktop);
				case -2147483646:
					return Color.FromKnownColor(KnownColor.ActiveCaption);
				case -2147483645:
					return Color.FromKnownColor(KnownColor.InactiveCaption);
				case -2147483644:
					return Color.FromKnownColor(KnownColor.Menu);
				case -2147483643:
					return Color.FromKnownColor(KnownColor.Window);
				case -2147483642:
					return Color.FromKnownColor(KnownColor.WindowFrame);
				case -2147483641:
					return Color.FromKnownColor(KnownColor.MenuText);
				case -2147483640:
					return Color.FromKnownColor(KnownColor.WindowText);
				case -2147483639:
					return Color.FromKnownColor(KnownColor.ActiveCaptionText);
				case -2147483638:
					return Color.FromKnownColor(KnownColor.ActiveBorder);
				case -2147483637:
					return Color.FromKnownColor(KnownColor.InactiveBorder);
				case -2147483636:
					return Color.FromKnownColor(KnownColor.AppWorkspace);
				case -2147483635:
					return Color.FromKnownColor(KnownColor.Highlight);
				case -2147483634:
					return Color.FromKnownColor(KnownColor.HighlightText);
				case -2147483633:
					return Color.FromKnownColor(KnownColor.Control);
				case -2147483632:
					return Color.FromKnownColor(KnownColor.ControlDark);
				case -2147483631:
					return Color.FromKnownColor(KnownColor.GrayText);
				case -2147483630:
					return Color.FromKnownColor(KnownColor.ControlText);
				case -2147483629:
					return Color.FromKnownColor(KnownColor.InactiveCaptionText);
				case -2147483628:
					return Color.FromKnownColor(KnownColor.ControlLightLight);
				case -2147483627:
					return Color.FromKnownColor(KnownColor.ControlDarkDark);
				case -2147483626:
					return Color.FromKnownColor(KnownColor.ControlLight);
				case -2147483625:
					return Color.FromKnownColor(KnownColor.InfoText);
				case -2147483624:
					return Color.FromKnownColor(KnownColor.Info);
				case -2147483621:
					return Color.FromKnownColor(KnownColor.GradientActiveCaption);
				case -2147483620:
					return Color.FromKnownColor(KnownColor.GradientInactiveCaption);
				case -2147483619:
					return Color.FromKnownColor(KnownColor.MenuHighlight);
				case -2147483618:
					return Color.FromKnownColor(KnownColor.MenuBar);
				}
			}
			return KnownColorTable.ArgbToKnownColor(Color.FromArgb((int)((byte)(oleColor & 255)), (int)((byte)((oleColor >> 8) & 255)), (int)((byte)((oleColor >> 16) & 255))).ToArgb());
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0001C499 File Offset: 0x0001B499
		public static Color FromWin32(int win32Color)
		{
			return ColorTranslator.FromOle(win32Color);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0001C4A4 File Offset: 0x0001B4A4
		public static Color FromHtml(string htmlColor)
		{
			Color color = Color.Empty;
			if (htmlColor == null || htmlColor.Length == 0)
			{
				return color;
			}
			if (htmlColor[0] == '#' && (htmlColor.Length == 7 || htmlColor.Length == 4))
			{
				if (htmlColor.Length == 7)
				{
					color = Color.FromArgb(Convert.ToInt32(htmlColor.Substring(1, 2), 16), Convert.ToInt32(htmlColor.Substring(3, 2), 16), Convert.ToInt32(htmlColor.Substring(5, 2), 16));
				}
				else
				{
					string text = char.ToString(htmlColor[1]);
					string text2 = char.ToString(htmlColor[2]);
					string text3 = char.ToString(htmlColor[3]);
					color = Color.FromArgb(Convert.ToInt32(text + text, 16), Convert.ToInt32(text2 + text2, 16), Convert.ToInt32(text3 + text3, 16));
				}
			}
			if (color.IsEmpty && string.Equals(htmlColor, "LightGrey", StringComparison.OrdinalIgnoreCase))
			{
				color = Color.LightGray;
			}
			if (color.IsEmpty)
			{
				if (ColorTranslator.htmlSysColorTable == null)
				{
					ColorTranslator.InitializeHtmlSysColorTable();
				}
				object obj = ColorTranslator.htmlSysColorTable[htmlColor.ToLower(CultureInfo.InvariantCulture)];
				if (obj != null)
				{
					color = (Color)obj;
				}
			}
			if (color.IsEmpty)
			{
				color = (Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(htmlColor);
			}
			return color;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001C5F8 File Offset: 0x0001B5F8
		public static string ToHtml(Color c)
		{
			string text = string.Empty;
			if (c.IsEmpty)
			{
				return text;
			}
			if (c.IsSystemColor)
			{
				KnownColor knownColor = c.ToKnownColor();
				switch (knownColor)
				{
				case KnownColor.ActiveBorder:
					return "activeborder";
				case KnownColor.ActiveCaption:
					break;
				case KnownColor.ActiveCaptionText:
					return "captiontext";
				case KnownColor.AppWorkspace:
					return "appworkspace";
				case KnownColor.Control:
					return "buttonface";
				case KnownColor.ControlDark:
					return "buttonshadow";
				case KnownColor.ControlDarkDark:
					return "threeddarkshadow";
				case KnownColor.ControlLight:
					return "buttonface";
				case KnownColor.ControlLightLight:
					return "buttonhighlight";
				case KnownColor.ControlText:
					return "buttontext";
				case KnownColor.Desktop:
					return "background";
				case KnownColor.GrayText:
					return "graytext";
				case KnownColor.Highlight:
				case KnownColor.HotTrack:
					return "highlight";
				case KnownColor.HighlightText:
					goto IL_012F;
				case KnownColor.InactiveBorder:
					return "inactiveborder";
				case KnownColor.InactiveCaption:
					goto IL_0145;
				case KnownColor.InactiveCaptionText:
					return "inactivecaptiontext";
				case KnownColor.Info:
					return "infobackground";
				case KnownColor.InfoText:
					return "infotext";
				case KnownColor.Menu:
					goto IL_0171;
				case KnownColor.MenuText:
					return "menutext";
				case KnownColor.ScrollBar:
					return "scrollbar";
				case KnownColor.Window:
					return "window";
				case KnownColor.WindowFrame:
					return "windowframe";
				case KnownColor.WindowText:
					return "windowtext";
				default:
					switch (knownColor)
					{
					case KnownColor.GradientActiveCaption:
						break;
					case KnownColor.GradientInactiveCaption:
						goto IL_0145;
					case KnownColor.MenuBar:
						goto IL_0171;
					case KnownColor.MenuHighlight:
						goto IL_012F;
					default:
						return text;
					}
					break;
				}
				return "activecaption";
				IL_012F:
				return "highlighttext";
				IL_0145:
				return "inactivecaption";
				IL_0171:
				text = "menu";
			}
			else if (c.IsNamedColor)
			{
				if (c == Color.LightGray)
				{
					text = "LightGrey";
				}
				else
				{
					text = c.Name;
				}
			}
			else
			{
				text = "#" + c.R.ToString("X2", null) + c.G.ToString("X2", null) + c.B.ToString("X2", null);
			}
			return text;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001C83C File Offset: 0x0001B83C
		private static void InitializeHtmlSysColorTable()
		{
			ColorTranslator.htmlSysColorTable = new Hashtable(26);
			ColorTranslator.htmlSysColorTable["activeborder"] = Color.FromKnownColor(KnownColor.ActiveBorder);
			ColorTranslator.htmlSysColorTable["activecaption"] = Color.FromKnownColor(KnownColor.ActiveCaption);
			ColorTranslator.htmlSysColorTable["appworkspace"] = Color.FromKnownColor(KnownColor.AppWorkspace);
			ColorTranslator.htmlSysColorTable["background"] = Color.FromKnownColor(KnownColor.Desktop);
			ColorTranslator.htmlSysColorTable["buttonface"] = Color.FromKnownColor(KnownColor.Control);
			ColorTranslator.htmlSysColorTable["buttonhighlight"] = Color.FromKnownColor(KnownColor.ControlLightLight);
			ColorTranslator.htmlSysColorTable["buttonshadow"] = Color.FromKnownColor(KnownColor.ControlDark);
			ColorTranslator.htmlSysColorTable["buttontext"] = Color.FromKnownColor(KnownColor.ControlText);
			ColorTranslator.htmlSysColorTable["captiontext"] = Color.FromKnownColor(KnownColor.ActiveCaptionText);
			ColorTranslator.htmlSysColorTable["graytext"] = Color.FromKnownColor(KnownColor.GrayText);
			ColorTranslator.htmlSysColorTable["highlight"] = Color.FromKnownColor(KnownColor.Highlight);
			ColorTranslator.htmlSysColorTable["highlighttext"] = Color.FromKnownColor(KnownColor.HighlightText);
			ColorTranslator.htmlSysColorTable["inactiveborder"] = Color.FromKnownColor(KnownColor.InactiveBorder);
			ColorTranslator.htmlSysColorTable["inactivecaption"] = Color.FromKnownColor(KnownColor.InactiveCaption);
			ColorTranslator.htmlSysColorTable["inactivecaptiontext"] = Color.FromKnownColor(KnownColor.InactiveCaptionText);
			ColorTranslator.htmlSysColorTable["infobackground"] = Color.FromKnownColor(KnownColor.Info);
			ColorTranslator.htmlSysColorTable["infotext"] = Color.FromKnownColor(KnownColor.InfoText);
			ColorTranslator.htmlSysColorTable["menu"] = Color.FromKnownColor(KnownColor.Menu);
			ColorTranslator.htmlSysColorTable["menutext"] = Color.FromKnownColor(KnownColor.MenuText);
			ColorTranslator.htmlSysColorTable["scrollbar"] = Color.FromKnownColor(KnownColor.ScrollBar);
			ColorTranslator.htmlSysColorTable["threeddarkshadow"] = Color.FromKnownColor(KnownColor.ControlDarkDark);
			ColorTranslator.htmlSysColorTable["threedface"] = Color.FromKnownColor(KnownColor.Control);
			ColorTranslator.htmlSysColorTable["threedhighlight"] = Color.FromKnownColor(KnownColor.ControlLight);
			ColorTranslator.htmlSysColorTable["threedlightshadow"] = Color.FromKnownColor(KnownColor.ControlLightLight);
			ColorTranslator.htmlSysColorTable["window"] = Color.FromKnownColor(KnownColor.Window);
			ColorTranslator.htmlSysColorTable["windowframe"] = Color.FromKnownColor(KnownColor.WindowFrame);
			ColorTranslator.htmlSysColorTable["windowtext"] = Color.FromKnownColor(KnownColor.WindowText);
		}

		// Token: 0x040004DB RID: 1243
		private const int Win32RedShift = 0;

		// Token: 0x040004DC RID: 1244
		private const int Win32GreenShift = 8;

		// Token: 0x040004DD RID: 1245
		private const int Win32BlueShift = 16;

		// Token: 0x040004DE RID: 1246
		private static Hashtable htmlSysColorTable;
	}
}
