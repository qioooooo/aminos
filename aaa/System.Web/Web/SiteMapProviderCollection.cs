using System;
using System.Configuration.Provider;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x020000C9 RID: 201
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SiteMapProviderCollection : ProviderCollection
	{
		// Token: 0x06000905 RID: 2309 RVA: 0x00028D6C File Offset: 0x00027D6C
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (!(provider is SiteMapProvider))
			{
				throw new ArgumentException(SR.GetString("Provider_must_implement_the_interface", new object[]
				{
					provider.GetType().Name,
					typeof(SiteMapProvider).Name
				}), "provider");
			}
			this.Add((SiteMapProvider)provider);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00028DD8 File Offset: 0x00027DD8
		public void Add(SiteMapProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			base.Add(provider);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00028DF0 File Offset: 0x00027DF0
		public void AddArray(SiteMapProvider[] providerArray)
		{
			if (providerArray == null)
			{
				throw new ArgumentNullException("providerArray");
			}
			foreach (SiteMapProvider siteMapProvider in providerArray)
			{
				if (this[siteMapProvider.Name] != null)
				{
					throw new ArgumentException(SR.GetString("SiteMapProvider_Multiple_Providers_With_Identical_Name", new object[] { siteMapProvider.Name }));
				}
				this.Add(siteMapProvider);
			}
		}

		// Token: 0x170002D8 RID: 728
		public SiteMapProvider this[string name]
		{
			get
			{
				return (SiteMapProvider)base[name];
			}
		}
	}
}
