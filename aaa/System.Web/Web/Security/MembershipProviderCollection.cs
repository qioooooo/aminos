using System;
using System.Configuration.Provider;

namespace System.Web.Security
{
	// Token: 0x02000342 RID: 834
	public sealed class MembershipProviderCollection : ProviderCollection
	{
		// Token: 0x0600289E RID: 10398 RVA: 0x000B26C4 File Offset: 0x000B16C4
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (!(provider is MembershipProvider))
			{
				throw new ArgumentException(SR.GetString("Provider_must_implement_type", new object[] { typeof(MembershipProvider).ToString() }), "provider");
			}
			base.Add(provider);
		}

		// Token: 0x17000898 RID: 2200
		public MembershipProvider this[string name]
		{
			get
			{
				return (MembershipProvider)base[name];
			}
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x000B272B File Offset: 0x000B172B
		public void CopyTo(MembershipProvider[] array, int index)
		{
			base.CopyTo(array, index);
		}
	}
}
