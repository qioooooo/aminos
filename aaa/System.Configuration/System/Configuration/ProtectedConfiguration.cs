using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x02000087 RID: 135
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public static class ProtectedConfiguration
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x000194DC File Offset: 0x000184DC
		public static ProtectedConfigurationProviderCollection Providers
		{
			get
			{
				ProtectedConfigurationSection protectedConfigurationSection = PrivilegedConfigurationManager.GetSection("configProtectedData") as ProtectedConfigurationSection;
				if (protectedConfigurationSection == null)
				{
					return new ProtectedConfigurationProviderCollection();
				}
				return protectedConfigurationSection.GetAllProviders();
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x00019508 File Offset: 0x00018508
		public static string DefaultProvider
		{
			get
			{
				ProtectedConfigurationSection protectedConfigurationSection = PrivilegedConfigurationManager.GetSection("configProtectedData") as ProtectedConfigurationSection;
				if (protectedConfigurationSection != null)
				{
					return protectedConfigurationSection.DefaultProvider;
				}
				return "";
			}
		}

		// Token: 0x04000369 RID: 873
		public const string RsaProviderName = "RsaProtectedConfigurationProvider";

		// Token: 0x0400036A RID: 874
		public const string DataProtectionProviderName = "DataProtectionConfigurationProvider";

		// Token: 0x0400036B RID: 875
		public const string ProtectedDataSectionName = "configProtectedData";
	}
}
