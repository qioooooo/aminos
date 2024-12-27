using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x02000312 RID: 786
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileProviderCollection : SettingsProviderCollection
	{
		// Token: 0x060026AA RID: 9898 RVA: 0x000A58BC File Offset: 0x000A48BC
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (!(provider is ProfileProvider))
			{
				throw new ArgumentException(SR.GetString("Provider_must_implement_type", new object[] { typeof(ProfileProvider).ToString() }), "provider");
			}
			base.Add(provider);
		}

		// Token: 0x17000811 RID: 2065
		public ProfileProvider this[string name]
		{
			get
			{
				return (ProfileProvider)base[name];
			}
		}
	}
}
