using System;
using System.Security.Permissions;
using System.Web.Configuration;

namespace System.Web
{
	// Token: 0x020000C8 RID: 200
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public static class SiteMap
	{
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x00028BDA File Offset: 0x00027BDA
		public static SiteMapNode CurrentNode
		{
			get
			{
				return SiteMap.Provider.CurrentNode;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00028BE8 File Offset: 0x00027BE8
		public static bool Enabled
		{
			get
			{
				if (!SiteMap._configEnabledEvaluated)
				{
					SiteMapSection siteMap = RuntimeConfig.GetAppConfig().SiteMap;
					SiteMap._enabled = siteMap != null && siteMap.Enabled;
					SiteMap._configEnabledEvaluated = true;
				}
				return SiteMap._enabled;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x00028C23 File Offset: 0x00027C23
		public static SiteMapProvider Provider
		{
			get
			{
				SiteMap.Initialize();
				return SiteMap._provider;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00028C2F File Offset: 0x00027C2F
		public static SiteMapProviderCollection Providers
		{
			get
			{
				SiteMap.Initialize();
				return SiteMap._providers;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x00028C3C File Offset: 0x00027C3C
		public static SiteMapNode RootNode
		{
			get
			{
				SiteMapProvider rootProvider = SiteMap.Provider.RootProvider;
				SiteMapNode rootNode = rootProvider.RootNode;
				if (rootNode == null)
				{
					string name = rootProvider.Name;
					throw new InvalidOperationException(SR.GetString("SiteMapProvider_Invalid_RootNode", new object[] { name }));
				}
				return rootNode;
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000901 RID: 2305 RVA: 0x00028C82 File Offset: 0x00027C82
		// (remove) Token: 0x06000902 RID: 2306 RVA: 0x00028C8F File Offset: 0x00027C8F
		public static event SiteMapResolveEventHandler SiteMapResolve
		{
			add
			{
				SiteMap.Provider.SiteMapResolve += value;
			}
			remove
			{
				SiteMap.Provider.SiteMapResolve -= value;
			}
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00028C9C File Offset: 0x00027C9C
		private static void Initialize()
		{
			if (SiteMap._providers != null)
			{
				return;
			}
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
			lock (SiteMap._lockObject)
			{
				if (SiteMap._providers == null)
				{
					SiteMapSection siteMap = RuntimeConfig.GetAppConfig().SiteMap;
					if (siteMap == null)
					{
						SiteMap._providers = new SiteMapProviderCollection();
					}
					else
					{
						if (!siteMap.Enabled)
						{
							throw new InvalidOperationException(SR.GetString("SiteMap_feature_disabled", new object[] { "system.web/siteMap" }));
						}
						siteMap.ValidateDefaultProvider();
						SiteMap._providers = siteMap.ProvidersInternal;
						SiteMap._provider = SiteMap._providers[siteMap.DefaultProvider];
						SiteMap._providers.SetReadOnly();
					}
				}
			}
		}

		// Token: 0x0400122C RID: 4652
		internal const string SectionName = "system.web/siteMap";

		// Token: 0x0400122D RID: 4653
		private static SiteMapProviderCollection _providers;

		// Token: 0x0400122E RID: 4654
		private static SiteMapProvider _provider;

		// Token: 0x0400122F RID: 4655
		private static object _lockObject = new object();

		// Token: 0x04001230 RID: 4656
		private static bool _configEnabledEvaluated;

		// Token: 0x04001231 RID: 4657
		private static bool _enabled;
	}
}
