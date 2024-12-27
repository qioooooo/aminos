using System;
using System.Configuration.Provider;

namespace System.Web.Security
{
	// Token: 0x02000354 RID: 852
	public sealed class RoleProviderCollection : ProviderCollection
	{
		// Token: 0x0600296D RID: 10605 RVA: 0x000B5F60 File Offset: 0x000B4F60
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (!(provider is RoleProvider))
			{
				throw new ArgumentException(SR.GetString("Provider_must_implement_type", new object[] { typeof(RoleProvider).ToString() }), "provider");
			}
			base.Add(provider);
		}

		// Token: 0x170008D2 RID: 2258
		public RoleProvider this[string name]
		{
			get
			{
				return (RoleProvider)base[name];
			}
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x000B5FC7 File Offset: 0x000B4FC7
		public void CopyTo(RoleProvider[] array, int index)
		{
			base.CopyTo(array, index);
		}
	}
}
