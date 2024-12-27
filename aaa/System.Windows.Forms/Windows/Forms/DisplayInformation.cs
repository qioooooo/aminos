using System;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020003B4 RID: 948
	internal class DisplayInformation
	{
		// Token: 0x060039C7 RID: 14791 RVA: 0x000D3031 File Offset: 0x000D2031
		static DisplayInformation()
		{
			SystemEvents.UserPreferenceChanging += DisplayInformation.UserPreferenceChanging;
			SystemEvents.DisplaySettingsChanging += DisplayInformation.DisplaySettingsChanging;
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x060039C8 RID: 14792 RVA: 0x000D3055 File Offset: 0x000D2055
		public static short BitsPerPixel
		{
			get
			{
				if (DisplayInformation.bitsPerPixel == 0)
				{
					DisplayInformation.bitsPerPixel = (short)Screen.PrimaryScreen.BitsPerPixel;
				}
				return DisplayInformation.bitsPerPixel;
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x060039C9 RID: 14793 RVA: 0x000D3073 File Offset: 0x000D2073
		public static bool LowResolution
		{
			get
			{
				if (DisplayInformation.lowResSettingValid && !DisplayInformation.lowRes)
				{
					return DisplayInformation.lowRes;
				}
				DisplayInformation.lowRes = DisplayInformation.BitsPerPixel <= 8;
				DisplayInformation.lowResSettingValid = true;
				return DisplayInformation.lowRes;
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x000D30A4 File Offset: 0x000D20A4
		public static bool HighContrast
		{
			get
			{
				if (DisplayInformation.highContrastSettingValid)
				{
					return DisplayInformation.highContrast;
				}
				DisplayInformation.highContrast = SystemInformation.HighContrast;
				DisplayInformation.highContrastSettingValid = true;
				return DisplayInformation.highContrast;
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x060039CB RID: 14795 RVA: 0x000D30C8 File Offset: 0x000D20C8
		public static bool IsDropShadowEnabled
		{
			get
			{
				if (DisplayInformation.dropShadowSettingValid)
				{
					return DisplayInformation.dropShadowEnabled;
				}
				DisplayInformation.dropShadowEnabled = SystemInformation.IsDropShadowEnabled;
				DisplayInformation.dropShadowSettingValid = true;
				return DisplayInformation.dropShadowEnabled;
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x060039CC RID: 14796 RVA: 0x000D30EC File Offset: 0x000D20EC
		public static bool TerminalServer
		{
			get
			{
				if (DisplayInformation.terminalSettingValid)
				{
					return DisplayInformation.isTerminalServerSession;
				}
				DisplayInformation.isTerminalServerSession = SystemInformation.TerminalServerSession;
				DisplayInformation.terminalSettingValid = true;
				return DisplayInformation.isTerminalServerSession;
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x060039CD RID: 14797 RVA: 0x000D3110 File Offset: 0x000D2110
		public static bool MenuAccessKeysUnderlined
		{
			get
			{
				if (DisplayInformation.menuAccessKeysUnderlinedValid)
				{
					return DisplayInformation.menuAccessKeysUnderlined;
				}
				DisplayInformation.menuAccessKeysUnderlined = SystemInformation.MenuAccessKeysUnderlined;
				DisplayInformation.menuAccessKeysUnderlinedValid = true;
				return DisplayInformation.menuAccessKeysUnderlined;
			}
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x000D3134 File Offset: 0x000D2134
		private static void DisplaySettingsChanging(object obj, EventArgs ea)
		{
			DisplayInformation.highContrastSettingValid = false;
			DisplayInformation.lowResSettingValid = false;
			DisplayInformation.terminalSettingValid = false;
			DisplayInformation.dropShadowSettingValid = false;
			DisplayInformation.menuAccessKeysUnderlinedValid = false;
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x000D3154 File Offset: 0x000D2154
		private static void UserPreferenceChanging(object obj, UserPreferenceChangingEventArgs e)
		{
			DisplayInformation.highContrastSettingValid = false;
			DisplayInformation.lowResSettingValid = false;
			DisplayInformation.terminalSettingValid = false;
			DisplayInformation.dropShadowSettingValid = false;
			DisplayInformation.bitsPerPixel = 0;
			if (e.Category == UserPreferenceCategory.General)
			{
				DisplayInformation.menuAccessKeysUnderlinedValid = false;
			}
		}

		// Token: 0x04001CE0 RID: 7392
		private static bool highContrast;

		// Token: 0x04001CE1 RID: 7393
		private static bool lowRes;

		// Token: 0x04001CE2 RID: 7394
		private static bool isTerminalServerSession;

		// Token: 0x04001CE3 RID: 7395
		private static bool highContrastSettingValid;

		// Token: 0x04001CE4 RID: 7396
		private static bool lowResSettingValid;

		// Token: 0x04001CE5 RID: 7397
		private static bool terminalSettingValid;

		// Token: 0x04001CE6 RID: 7398
		private static short bitsPerPixel;

		// Token: 0x04001CE7 RID: 7399
		private static bool dropShadowSettingValid;

		// Token: 0x04001CE8 RID: 7400
		private static bool dropShadowEnabled;

		// Token: 0x04001CE9 RID: 7401
		private static bool menuAccessKeysUnderlinedValid;

		// Token: 0x04001CEA RID: 7402
		private static bool menuAccessKeysUnderlined;
	}
}
