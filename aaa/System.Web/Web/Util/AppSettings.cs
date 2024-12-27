using System;
using System.Collections.Specialized;

namespace System.Web.Util
{
	// Token: 0x02000751 RID: 1873
	internal static class AppSettings
	{
		// Token: 0x06005ADD RID: 23261 RVA: 0x0016EE54 File Offset: 0x0016DE54
		private static void EnsureSettingsLoaded()
		{
			if (!AppSettings._settingsInitialized)
			{
				lock (AppSettings._appSettingsLock)
				{
					if (!AppSettings._settingsInitialized)
					{
						NameValueCollection nameValueCollection = null;
						try
						{
							CachedPathData applicationPathData = CachedPathData.GetApplicationPathData();
							if (applicationPathData != null && applicationPathData.ConfigRecord != null)
							{
								nameValueCollection = applicationPathData.ConfigRecord.GetSection("appSettings") as NameValueCollection;
							}
						}
						finally
						{
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:IgnoreFormActionAttribute"], out AppSettings._ignoreFormActionAttribute))
							{
								AppSettings._ignoreFormActionAttribute = false;
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:UseHostHeaderForRequestUrl"], out AppSettings._useHostHeaderForRequestUrl))
							{
								AppSettings._useHostHeaderForRequestUrl = false;
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:AllowAnonymousImpersonation"], out AppSettings._allowAnonymousImpersonation))
							{
								AppSettings._allowAnonymousImpersonation = false;
							}
							if (nameValueCollection == null || !TimeSpan.TryParse(nameValueCollection["aspnet:SqlSessionState:RetryInterval"], out AppSettings._sqlSessionRetryInterval))
							{
								AppSettings._sqlSessionRetryInterval = TimeSpan.Zero;
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:UseLegacyEncryption"], out AppSettings._useLegacyEncryption))
							{
								AppSettings._useLegacyEncryption = false;
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:UseStrictParserRegex"], out AppSettings._useStrictParserRegex))
							{
								AppSettings._useStrictParserRegex = false;
							}
							if (nameValueCollection != null)
							{
								AppSettings._formsAuthReturnUrlVar = nameValueCollection["aspnet:FormsAuthReturnUrlVar"];
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:AllowRelaxedRelativeUrl"], out AppSettings._allowRelaxedRelativeUrl))
							{
								AppSettings._allowRelaxedRelativeUrl = false;
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:RestrictXmlControls"], out AppSettings._restrictXmlControls))
							{
								AppSettings._restrictXmlControls = false;
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:UseLegacyFormsAuthenticationTicketCompatibility"], out AppSettings._useLegacyFormsAuthenticationTicketCompatibility))
							{
								AppSettings._useLegacyFormsAuthenticationTicketCompatibility = false;
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:AllowRelaxedHttpUserName"], out AppSettings._allowRelaxedHttpUserName))
							{
								AppSettings._allowRelaxedHttpUserName = false;
							}
							if (nameValueCollection == null || !int.TryParse(nameValueCollection["aspnet:MaxHttpCollectionKeys"], out AppSettings._maxHttpCollectionKeys) || AppSettings._maxHttpCollectionKeys < 0)
							{
								AppSettings._maxHttpCollectionKeys = 1000;
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:UseLegacyBrowserCaps"], out AppSettings._useLegacyBrowserCaps))
							{
								AppSettings._useLegacyBrowserCaps = false;
							}
							AppSettings._allowInsecureDeserialization = AppSettings.GetNullableBooleanValue(nameValueCollection, "aspnet:AllowInsecureDeserialization");
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:AlwaysIgnoreViewStateValidationErrors"], out AppSettings._alwaysIgnoreViewStateValidationErrors))
							{
								AppSettings._alwaysIgnoreViewStateValidationErrors = false;
							}
							if (nameValueCollection == null || !int.TryParse(nameValueCollection["aspnet:RequestQueueLimitPerSession"], out AppSettings._requestQueueLimitPerSession) || AppSettings._requestQueueLimitPerSession < 0)
							{
								AppSettings._requestQueueLimitPerSession = int.MaxValue;
							}
							if (nameValueCollection != null)
							{
								AppSettings._controlBuilderInterceptor = nameValueCollection["aspnet:20ControlBuilderInterceptor"];
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:VerifyVirtualPathFromDiskCache"], out AppSettings._verifyVirtualPathFromDiskCache))
							{
								AppSettings._verifyVirtualPathFromDiskCache = true;
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:CheckMemoryBytes"], out AppSettings._checkMemoryBytes))
							{
								AppSettings._checkMemoryBytes = true;
							}
							if (nameValueCollection == null || !bool.TryParse(nameValueCollection["aspnet:RestoreAggressiveCookielessPathRemoval"], out AppSettings._restoreAggressiveCookielessPathRemoval))
							{
								AppSettings._restoreAggressiveCookielessPathRemoval = false;
							}
							AppSettings._settingsInitialized = true;
						}
					}
				}
			}
		}

		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x06005ADE RID: 23262 RVA: 0x0016F16C File Offset: 0x0016E16C
		internal static bool IgnoreFormActionAttribute
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._ignoreFormActionAttribute;
			}
		}

		// Token: 0x1700178E RID: 6030
		// (get) Token: 0x06005ADF RID: 23263 RVA: 0x0016F178 File Offset: 0x0016E178
		internal static bool UseHostHeaderForRequestUrl
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._useHostHeaderForRequestUrl;
			}
		}

		// Token: 0x1700178F RID: 6031
		// (get) Token: 0x06005AE0 RID: 23264 RVA: 0x0016F184 File Offset: 0x0016E184
		internal static bool AllowAnonymousImpersonation
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._allowAnonymousImpersonation;
			}
		}

		// Token: 0x17001790 RID: 6032
		// (get) Token: 0x06005AE1 RID: 23265 RVA: 0x0016F190 File Offset: 0x0016E190
		internal static TimeSpan SqlSessionRetryInterval
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._sqlSessionRetryInterval;
			}
		}

		// Token: 0x17001791 RID: 6033
		// (get) Token: 0x06005AE2 RID: 23266 RVA: 0x0016F19C File Offset: 0x0016E19C
		internal static bool UseLegacyEncryption
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._useLegacyEncryption;
			}
		}

		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x06005AE3 RID: 23267 RVA: 0x0016F1A8 File Offset: 0x0016E1A8
		internal static bool UseStrictParserRegex
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._useStrictParserRegex;
			}
		}

		// Token: 0x17001793 RID: 6035
		// (get) Token: 0x06005AE4 RID: 23268 RVA: 0x0016F1B4 File Offset: 0x0016E1B4
		internal static string FormsAuthReturnUrlVar
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._formsAuthReturnUrlVar;
			}
		}

		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x06005AE5 RID: 23269 RVA: 0x0016F1C0 File Offset: 0x0016E1C0
		internal static bool AllowRelaxedRelativeUrl
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._allowRelaxedRelativeUrl;
			}
		}

		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x06005AE6 RID: 23270 RVA: 0x0016F1CC File Offset: 0x0016E1CC
		internal static bool RestrictXmlControls
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._restrictXmlControls;
			}
		}

		// Token: 0x17001796 RID: 6038
		// (get) Token: 0x06005AE7 RID: 23271 RVA: 0x0016F1D8 File Offset: 0x0016E1D8
		internal static bool UseLegacyFormsAuthenticationTicketCompatibility
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._useLegacyFormsAuthenticationTicketCompatibility;
			}
		}

		// Token: 0x17001797 RID: 6039
		// (get) Token: 0x06005AE8 RID: 23272 RVA: 0x0016F1E4 File Offset: 0x0016E1E4
		internal static bool AllowRelaxedHttpUserName
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._allowRelaxedHttpUserName;
			}
		}

		// Token: 0x17001798 RID: 6040
		// (get) Token: 0x06005AE9 RID: 23273 RVA: 0x0016F1F0 File Offset: 0x0016E1F0
		internal static int MaxHttpCollectionKeys
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._maxHttpCollectionKeys;
			}
		}

		// Token: 0x17001799 RID: 6041
		// (get) Token: 0x06005AEA RID: 23274 RVA: 0x0016F1FC File Offset: 0x0016E1FC
		internal static bool UseLegacyBrowserCaps
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._useLegacyBrowserCaps;
			}
		}

		// Token: 0x1700179A RID: 6042
		// (get) Token: 0x06005AEB RID: 23275 RVA: 0x0016F208 File Offset: 0x0016E208
		internal static bool? AllowInsecureDeserialization
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._allowInsecureDeserialization;
			}
		}

		// Token: 0x1700179B RID: 6043
		// (get) Token: 0x06005AEC RID: 23276 RVA: 0x0016F214 File Offset: 0x0016E214
		internal static bool AlwaysIgnoreViewStateValidationErrors
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._alwaysIgnoreViewStateValidationErrors;
			}
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x0016F220 File Offset: 0x0016E220
		private static bool? GetNullableBooleanValue(NameValueCollection settings, string key)
		{
			bool flag;
			if (settings == null || !bool.TryParse(settings[key], out flag))
			{
				return null;
			}
			return new bool?(flag);
		}

		// Token: 0x1700179C RID: 6044
		// (get) Token: 0x06005AEE RID: 23278 RVA: 0x0016F250 File Offset: 0x0016E250
		internal static int RequestQueueLimitPerSession
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._requestQueueLimitPerSession;
			}
		}

		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x06005AEF RID: 23279 RVA: 0x0016F25C File Offset: 0x0016E25C
		internal static string ControlBuilderInterceptor
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._controlBuilderInterceptor;
			}
		}

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x06005AF0 RID: 23280 RVA: 0x0016F268 File Offset: 0x0016E268
		internal static bool VerifyVirtualPathFromDiskCache
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._verifyVirtualPathFromDiskCache;
			}
		}

		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x06005AF1 RID: 23281 RVA: 0x0016F274 File Offset: 0x0016E274
		internal static bool CheckMemoryBytes
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._checkMemoryBytes;
			}
		}

		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x06005AF2 RID: 23282 RVA: 0x0016F280 File Offset: 0x0016E280
		internal static bool RestoreAggressiveCookielessPathRemoval
		{
			get
			{
				AppSettings.EnsureSettingsLoaded();
				return AppSettings._restoreAggressiveCookielessPathRemoval;
			}
		}

		// Token: 0x040030E5 RID: 12517
		private const int DefaultMaxHttpCollectionKeys = 1000;

		// Token: 0x040030E6 RID: 12518
		internal const int UnlimitedRequestsPerSession = 2147483647;

		// Token: 0x040030E7 RID: 12519
		private static volatile bool _settingsInitialized = false;

		// Token: 0x040030E8 RID: 12520
		private static object _appSettingsLock = new object();

		// Token: 0x040030E9 RID: 12521
		private static bool _ignoreFormActionAttribute;

		// Token: 0x040030EA RID: 12522
		private static bool _useHostHeaderForRequestUrl;

		// Token: 0x040030EB RID: 12523
		private static bool _allowAnonymousImpersonation;

		// Token: 0x040030EC RID: 12524
		private static TimeSpan _sqlSessionRetryInterval = TimeSpan.Zero;

		// Token: 0x040030ED RID: 12525
		private static bool _useLegacyEncryption;

		// Token: 0x040030EE RID: 12526
		private static bool _useStrictParserRegex;

		// Token: 0x040030EF RID: 12527
		private static string _formsAuthReturnUrlVar;

		// Token: 0x040030F0 RID: 12528
		private static bool _allowRelaxedRelativeUrl;

		// Token: 0x040030F1 RID: 12529
		private static bool _restrictXmlControls;

		// Token: 0x040030F2 RID: 12530
		private static bool _useLegacyFormsAuthenticationTicketCompatibility;

		// Token: 0x040030F3 RID: 12531
		private static bool _allowRelaxedHttpUserName;

		// Token: 0x040030F4 RID: 12532
		private static int _maxHttpCollectionKeys = 1000;

		// Token: 0x040030F5 RID: 12533
		private static bool _useLegacyBrowserCaps;

		// Token: 0x040030F6 RID: 12534
		private static bool? _allowInsecureDeserialization;

		// Token: 0x040030F7 RID: 12535
		private static bool _alwaysIgnoreViewStateValidationErrors;

		// Token: 0x040030F8 RID: 12536
		private static int _requestQueueLimitPerSession;

		// Token: 0x040030F9 RID: 12537
		private static string _controlBuilderInterceptor;

		// Token: 0x040030FA RID: 12538
		private static bool _verifyVirtualPathFromDiskCache;

		// Token: 0x040030FB RID: 12539
		private static bool _checkMemoryBytes;

		// Token: 0x040030FC RID: 12540
		private static bool _restoreAggressiveCookielessPathRemoval;
	}
}
