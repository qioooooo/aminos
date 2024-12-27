using System;
using System.Configuration;

namespace System.Runtime.Remoting.Configuration
{
	// Token: 0x0200006B RID: 107
	internal static class AppSettings
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0001016A File Offset: 0x0000F16A
		internal static bool AllowTransparentProxyMessage
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings.allowTransparentProxyMessageValue;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00010176 File Offset: 0x0000F176
		internal static bool LateHttpHeaderParsing
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings.lateHttpHeaderParsingValue;
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00010184 File Offset: 0x0000F184
		private static void EnsureSettingsLoaded()
		{
			if (!AppSettings.settingsInitialized)
			{
				lock (AppSettings.appSettingsLock)
				{
					if (!AppSettings.settingsInitialized)
					{
						try
						{
							AppSettingsReader appSettingsReader = new AppSettingsReader();
							object obj2 = null;
							if (AppSettings.TryGetValue(appSettingsReader, AppSettings.AllowTransparentProxyMessageKeyName, typeof(bool), out obj2))
							{
								AppSettings.allowTransparentProxyMessageValue = (bool)obj2;
							}
							else
							{
								AppSettings.allowTransparentProxyMessageValue = AppSettings.AllowTransparentProxyMessageDefaultValue;
							}
							if (AppSettings.TryGetValue(appSettingsReader, AppSettings.LateHttpHeaderParsingKeyName, typeof(bool), out obj2))
							{
								AppSettings.lateHttpHeaderParsingValue = (bool)obj2;
							}
							else
							{
								AppSettings.lateHttpHeaderParsingValue = AppSettings.LateHttpHeaderParsingDefaultValue;
							}
						}
						catch
						{
						}
						finally
						{
							AppSettings.settingsInitialized = true;
						}
					}
				}
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0001025C File Offset: 0x0000F25C
		private static bool TryGetValue(AppSettingsReader appSettingsReader, string key, Type type, out object value)
		{
			bool flag;
			try
			{
				value = appSettingsReader.GetValue(key, type);
				flag = true;
			}
			catch
			{
				value = null;
				flag = false;
			}
			return flag;
		}

		// Token: 0x04000273 RID: 627
		internal static readonly string AllowTransparentProxyMessageKeyName = "microsoft:Remoting:AllowTransparentProxyMessage";

		// Token: 0x04000274 RID: 628
		internal static readonly bool AllowTransparentProxyMessageDefaultValue = false;

		// Token: 0x04000275 RID: 629
		internal static readonly string AllowTransparentProxyMessageFwLink = "http://go.microsoft.com/fwlink/?LinkId=390633";

		// Token: 0x04000276 RID: 630
		internal static readonly string LateHttpHeaderParsingKeyName = "microsoft:Remoting:LateHttpHeaderParsing";

		// Token: 0x04000277 RID: 631
		internal static readonly bool LateHttpHeaderParsingDefaultValue = false;

		// Token: 0x04000278 RID: 632
		private static bool allowTransparentProxyMessageValue = AppSettings.AllowTransparentProxyMessageDefaultValue;

		// Token: 0x04000279 RID: 633
		private static bool lateHttpHeaderParsingValue = AppSettings.LateHttpHeaderParsingDefaultValue;

		// Token: 0x0400027A RID: 634
		private static volatile bool settingsInitialized = false;

		// Token: 0x0400027B RID: 635
		private static object appSettingsLock = new object();
	}
}
