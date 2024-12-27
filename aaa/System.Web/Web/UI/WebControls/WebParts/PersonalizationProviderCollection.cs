using System;
using System.Configuration.Provider;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006E1 RID: 1761
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PersonalizationProviderCollection : ProviderCollection
	{
		// Token: 0x17001651 RID: 5713
		public PersonalizationProvider this[string name]
		{
			get
			{
				return (PersonalizationProvider)base[name];
			}
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x0015D05C File Offset: 0x0015C05C
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (!(provider is PersonalizationProvider))
			{
				throw new ArgumentException(SR.GetString("Provider_must_implement_the_interface", new object[]
				{
					provider.GetType().FullName,
					"PersonalizationProvider"
				}));
			}
			base.Add(provider);
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x0015D0B4 File Offset: 0x0015C0B4
		public void CopyTo(PersonalizationProvider[] array, int index)
		{
			base.CopyTo(array, index);
		}
	}
}
