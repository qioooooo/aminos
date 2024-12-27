using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001A1 RID: 417
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AuthenticationSection : ConfigurationSection
	{
		// Token: 0x0600116A RID: 4458 RVA: 0x0004DB0C File Offset: 0x0004CB0C
		static AuthenticationSection()
		{
			AuthenticationSection._properties.Add(AuthenticationSection._propForms);
			AuthenticationSection._properties.Add(AuthenticationSection._propPassport);
			AuthenticationSection._properties.Add(AuthenticationSection._propMode);
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x0004DBAE File Offset: 0x0004CBAE
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AuthenticationSection._properties;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x0600116D RID: 4461 RVA: 0x0004DBB5 File Offset: 0x0004CBB5
		[ConfigurationProperty("forms")]
		public FormsAuthenticationConfiguration Forms
		{
			get
			{
				return (FormsAuthenticationConfiguration)base[AuthenticationSection._propForms];
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x0004DBC7 File Offset: 0x0004CBC7
		[ConfigurationProperty("passport")]
		public PassportAuthentication Passport
		{
			get
			{
				return (PassportAuthentication)base[AuthenticationSection._propPassport];
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x0600116F RID: 4463 RVA: 0x0004DBD9 File Offset: 0x0004CBD9
		// (set) Token: 0x06001170 RID: 4464 RVA: 0x0004DC06 File Offset: 0x0004CC06
		[ConfigurationProperty("mode", DefaultValue = AuthenticationMode.Windows)]
		public AuthenticationMode Mode
		{
			get
			{
				if (!this.authenticationModeCached)
				{
					this.authenticationModeCache = (AuthenticationMode)base[AuthenticationSection._propMode];
					this.authenticationModeCached = true;
				}
				return this.authenticationModeCache;
			}
			set
			{
				base[AuthenticationSection._propMode] = value;
				this.authenticationModeCache = value;
			}
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0004DC20 File Offset: 0x0004CC20
		protected override void Reset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
			this.authenticationModeCached = false;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x0004DC30 File Offset: 0x0004CC30
		internal void ValidateAuthenticationMode()
		{
			if (this.Mode == AuthenticationMode.Passport && UnsafeNativeMethods.PassportVersion() < 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Passport_not_installed"));
			}
		}

		// Token: 0x040016AD RID: 5805
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040016AE RID: 5806
		private static readonly ConfigurationProperty _propForms = new ConfigurationProperty("forms", typeof(FormsAuthenticationConfiguration), null, ConfigurationPropertyOptions.None);

		// Token: 0x040016AF RID: 5807
		private static readonly ConfigurationProperty _propPassport = new ConfigurationProperty("passport", typeof(PassportAuthentication), null, ConfigurationPropertyOptions.None);

		// Token: 0x040016B0 RID: 5808
		private static readonly ConfigurationProperty _propMode = new ConfigurationProperty("mode", typeof(AuthenticationMode), AuthenticationMode.Windows, ConfigurationPropertyOptions.None);

		// Token: 0x040016B1 RID: 5809
		private bool authenticationModeCached;

		// Token: 0x040016B2 RID: 5810
		private AuthenticationMode authenticationModeCache;
	}
}
