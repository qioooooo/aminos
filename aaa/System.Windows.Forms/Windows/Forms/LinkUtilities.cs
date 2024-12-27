using System;
using System.Drawing;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x02000478 RID: 1144
	internal class LinkUtilities
	{
		// Token: 0x06004342 RID: 17218 RVA: 0x000F09AC File Offset: 0x000EF9AC
		private static Color GetIEColor(string name)
		{
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			Color color;
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Settings");
				if (registryKey != null)
				{
					string text = (string)registryKey.GetValue(name);
					if (text != null)
					{
						string[] array = text.Split(new char[] { ',' });
						int[] array2 = new int[3];
						int num = Math.Min(array2.Length, array.Length);
						for (int i = 0; i < num; i++)
						{
							int.TryParse(array[i], out array2[i]);
						}
						return Color.FromArgb(array2[0], array2[1], array2[2]);
					}
				}
				if (string.Equals(name, "Anchor Color", StringComparison.OrdinalIgnoreCase))
				{
					color = Color.Blue;
				}
				else if (string.Equals(name, "Anchor Color Visited", StringComparison.OrdinalIgnoreCase))
				{
					color = Color.Purple;
				}
				else if (string.Equals(name, "Anchor Color Hover", StringComparison.OrdinalIgnoreCase))
				{
					color = Color.Red;
				}
				else
				{
					color = Color.Red;
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return color;
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06004343 RID: 17219 RVA: 0x000F0AB0 File Offset: 0x000EFAB0
		public static Color IELinkColor
		{
			get
			{
				if (LinkUtilities.ielinkColor.IsEmpty)
				{
					LinkUtilities.ielinkColor = LinkUtilities.GetIEColor("Anchor Color");
				}
				return LinkUtilities.ielinkColor;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06004344 RID: 17220 RVA: 0x000F0AD2 File Offset: 0x000EFAD2
		public static Color IEActiveLinkColor
		{
			get
			{
				if (LinkUtilities.ieactiveLinkColor.IsEmpty)
				{
					LinkUtilities.ieactiveLinkColor = LinkUtilities.GetIEColor("Anchor Color Hover");
				}
				return LinkUtilities.ieactiveLinkColor;
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06004345 RID: 17221 RVA: 0x000F0AF4 File Offset: 0x000EFAF4
		public static Color IEVisitedLinkColor
		{
			get
			{
				if (LinkUtilities.ievisitedLinkColor.IsEmpty)
				{
					LinkUtilities.ievisitedLinkColor = LinkUtilities.GetIEColor("Anchor Color Visited");
				}
				return LinkUtilities.ievisitedLinkColor;
			}
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x000F0B18 File Offset: 0x000EFB18
		public static LinkBehavior GetIELinkBehavior()
		{
			new RegistryPermission(PermissionState.Unrestricted).Assert();
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main");
				if (registryKey != null)
				{
					string text = (string)registryKey.GetValue("Anchor Underline");
					if (text != null && string.Compare(text, "no", true, CultureInfo.InvariantCulture) == 0)
					{
						return LinkBehavior.NeverUnderline;
					}
					if (text != null && string.Compare(text, "hover", true, CultureInfo.InvariantCulture) == 0)
					{
						return LinkBehavior.HoverUnderline;
					}
					return LinkBehavior.AlwaysUnderline;
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return LinkBehavior.AlwaysUnderline;
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x000F0BA8 File Offset: 0x000EFBA8
		public static void EnsureLinkFonts(Font baseFont, LinkBehavior link, ref Font linkFont, ref Font hoverLinkFont)
		{
			if (linkFont != null && hoverLinkFont != null)
			{
				return;
			}
			bool flag = true;
			bool flag2 = true;
			if (link == LinkBehavior.SystemDefault)
			{
				link = LinkUtilities.GetIELinkBehavior();
			}
			switch (link)
			{
			case LinkBehavior.AlwaysUnderline:
				flag = true;
				flag2 = true;
				break;
			case LinkBehavior.HoverUnderline:
				flag = false;
				flag2 = true;
				break;
			case LinkBehavior.NeverUnderline:
				flag = false;
				flag2 = false;
				break;
			}
			if (flag2 == flag)
			{
				FontStyle fontStyle = baseFont.Style;
				if (flag2)
				{
					fontStyle |= FontStyle.Underline;
				}
				else
				{
					fontStyle &= ~FontStyle.Underline;
				}
				hoverLinkFont = new Font(baseFont, fontStyle);
				linkFont = hoverLinkFont;
				return;
			}
			FontStyle fontStyle2 = baseFont.Style;
			if (flag2)
			{
				fontStyle2 |= FontStyle.Underline;
			}
			else
			{
				fontStyle2 &= ~FontStyle.Underline;
			}
			hoverLinkFont = new Font(baseFont, fontStyle2);
			FontStyle fontStyle3 = baseFont.Style;
			if (flag)
			{
				fontStyle3 |= FontStyle.Underline;
			}
			else
			{
				fontStyle3 &= ~FontStyle.Underline;
			}
			linkFont = new Font(baseFont, fontStyle3);
		}

		// Token: 0x040020D0 RID: 8400
		private const string IESettingsRegPath = "Software\\Microsoft\\Internet Explorer\\Settings";

		// Token: 0x040020D1 RID: 8401
		public const string IEMainRegPath = "Software\\Microsoft\\Internet Explorer\\Main";

		// Token: 0x040020D2 RID: 8402
		private const string IEAnchorColor = "Anchor Color";

		// Token: 0x040020D3 RID: 8403
		private const string IEAnchorColorVisited = "Anchor Color Visited";

		// Token: 0x040020D4 RID: 8404
		private const string IEAnchorColorHover = "Anchor Color Hover";

		// Token: 0x040020D5 RID: 8405
		private static Color ielinkColor = Color.Empty;

		// Token: 0x040020D6 RID: 8406
		private static Color ieactiveLinkColor = Color.Empty;

		// Token: 0x040020D7 RID: 8407
		private static Color ievisitedLinkColor = Color.Empty;
	}
}
