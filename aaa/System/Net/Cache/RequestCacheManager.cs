using System;
using System.Net.Configuration;

namespace System.Net.Cache
{
	// Token: 0x02000569 RID: 1385
	internal sealed class RequestCacheManager
	{
		// Token: 0x06002A87 RID: 10887 RVA: 0x000B4CE0 File Offset: 0x000B3CE0
		private RequestCacheManager()
		{
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000B4CE8 File Offset: 0x000B3CE8
		internal static RequestCacheBinding GetBinding(string internedScheme)
		{
			if (internedScheme == null)
			{
				throw new ArgumentNullException("uriScheme");
			}
			if (RequestCacheManager.s_CacheConfigSettings == null)
			{
				RequestCacheManager.LoadConfigSettings();
			}
			if (RequestCacheManager.s_CacheConfigSettings.DisableAllCaching)
			{
				return RequestCacheManager.s_BypassCacheBinding;
			}
			if (internedScheme.Length == 0)
			{
				return RequestCacheManager.s_DefaultGlobalBinding;
			}
			if (internedScheme == Uri.UriSchemeHttp || internedScheme == Uri.UriSchemeHttps)
			{
				return RequestCacheManager.s_DefaultHttpBinding;
			}
			if (internedScheme == Uri.UriSchemeFtp)
			{
				return RequestCacheManager.s_DefaultFtpBinding;
			}
			return RequestCacheManager.s_BypassCacheBinding;
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002A89 RID: 10889 RVA: 0x000B4D58 File Offset: 0x000B3D58
		internal static bool IsCachingEnabled
		{
			get
			{
				if (RequestCacheManager.s_CacheConfigSettings == null)
				{
					RequestCacheManager.LoadConfigSettings();
				}
				return !RequestCacheManager.s_CacheConfigSettings.DisableAllCaching;
			}
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000B4D74 File Offset: 0x000B3D74
		internal static void SetBinding(string uriScheme, RequestCacheBinding binding)
		{
			if (uriScheme == null)
			{
				throw new ArgumentNullException("uriScheme");
			}
			if (RequestCacheManager.s_CacheConfigSettings == null)
			{
				RequestCacheManager.LoadConfigSettings();
			}
			if (RequestCacheManager.s_CacheConfigSettings.DisableAllCaching)
			{
				return;
			}
			if (uriScheme.Length == 0)
			{
				RequestCacheManager.s_DefaultGlobalBinding = binding;
				return;
			}
			if (uriScheme == Uri.UriSchemeHttp || uriScheme == Uri.UriSchemeHttps)
			{
				RequestCacheManager.s_DefaultHttpBinding = binding;
				return;
			}
			if (uriScheme == Uri.UriSchemeFtp)
			{
				RequestCacheManager.s_DefaultFtpBinding = binding;
			}
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000B4DEC File Offset: 0x000B3DEC
		private static void LoadConfigSettings()
		{
			lock (RequestCacheManager.s_BypassCacheBinding)
			{
				if (RequestCacheManager.s_CacheConfigSettings == null)
				{
					RequestCachingSectionInternal section = RequestCachingSectionInternal.GetSection();
					RequestCacheManager.s_DefaultGlobalBinding = new RequestCacheBinding(section.DefaultCache, section.DefaultHttpValidator, section.DefaultCachePolicy);
					RequestCacheManager.s_DefaultHttpBinding = new RequestCacheBinding(section.DefaultCache, section.DefaultHttpValidator, section.DefaultHttpCachePolicy);
					RequestCacheManager.s_DefaultFtpBinding = new RequestCacheBinding(section.DefaultCache, section.DefaultFtpValidator, section.DefaultFtpCachePolicy);
					RequestCacheManager.s_CacheConfigSettings = section;
				}
			}
		}

		// Token: 0x04002904 RID: 10500
		private static RequestCachingSectionInternal s_CacheConfigSettings;

		// Token: 0x04002905 RID: 10501
		private static readonly RequestCacheBinding s_BypassCacheBinding = new RequestCacheBinding(null, null, new RequestCachePolicy(RequestCacheLevel.BypassCache));

		// Token: 0x04002906 RID: 10502
		private static RequestCacheBinding s_DefaultGlobalBinding;

		// Token: 0x04002907 RID: 10503
		private static RequestCacheBinding s_DefaultHttpBinding;

		// Token: 0x04002908 RID: 10504
		private static RequestCacheBinding s_DefaultFtpBinding;
	}
}
