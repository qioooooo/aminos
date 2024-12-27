using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001DD RID: 477
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class FormsAuthenticationCredentials : ConfigurationElement
	{
		// Token: 0x06001AA8 RID: 6824 RVA: 0x0007BF34 File Offset: 0x0007AF34
		static FormsAuthenticationCredentials()
		{
			FormsAuthenticationCredentials._properties.Add(FormsAuthenticationCredentials._propUsers);
			FormsAuthenticationCredentials._properties.Add(FormsAuthenticationCredentials._propPasswordFormat);
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001AAA RID: 6826 RVA: 0x0007BFA8 File Offset: 0x0007AFA8
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return FormsAuthenticationCredentials._properties;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001AAB RID: 6827 RVA: 0x0007BFAF File Offset: 0x0007AFAF
		[ConfigurationProperty("", IsDefaultCollection = true, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
		public FormsAuthenticationUserCollection Users
		{
			get
			{
				return (FormsAuthenticationUserCollection)base[FormsAuthenticationCredentials._propUsers];
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001AAC RID: 6828 RVA: 0x0007BFC1 File Offset: 0x0007AFC1
		// (set) Token: 0x06001AAD RID: 6829 RVA: 0x0007BFD3 File Offset: 0x0007AFD3
		[ConfigurationProperty("passwordFormat", DefaultValue = FormsAuthPasswordFormat.SHA1)]
		public FormsAuthPasswordFormat PasswordFormat
		{
			get
			{
				return (FormsAuthPasswordFormat)base[FormsAuthenticationCredentials._propPasswordFormat];
			}
			set
			{
				base[FormsAuthenticationCredentials._propPasswordFormat] = value;
			}
		}

		// Token: 0x040017F0 RID: 6128
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040017F1 RID: 6129
		private static readonly ConfigurationProperty _propUsers = new ConfigurationProperty(null, typeof(FormsAuthenticationUserCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x040017F2 RID: 6130
		private static readonly ConfigurationProperty _propPasswordFormat = new ConfigurationProperty("passwordFormat", typeof(FormsAuthPasswordFormat), FormsAuthPasswordFormat.SHA1, ConfigurationPropertyOptions.None);
	}
}
