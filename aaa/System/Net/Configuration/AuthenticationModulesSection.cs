using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000643 RID: 1603
	public sealed class AuthenticationModulesSection : ConfigurationSection
	{
		// Token: 0x060031A4 RID: 12708 RVA: 0x000D4666 File Offset: 0x000D3666
		public AuthenticationModulesSection()
		{
			this.properties.Add(this.authenticationModules);
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x000D46A4 File Offset: 0x000D36A4
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.UnmanagedPermission.Demand();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", new object[] { "authenticationModules" }), ex);
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x060031A6 RID: 12710 RVA: 0x000D4700 File Offset: 0x000D3700
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public AuthenticationModuleElementCollection AuthenticationModules
		{
			get
			{
				return (AuthenticationModuleElementCollection)base[this.authenticationModules];
			}
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x000D4714 File Offset: 0x000D3714
		protected override void InitializeDefault()
		{
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(NegotiateClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(KerberosClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(NtlmClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(DigestClient).AssemblyQualifiedName));
			this.AuthenticationModules.Add(new AuthenticationModuleElement(typeof(BasicClient).AssemblyQualifiedName));
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x060031A8 RID: 12712 RVA: 0x000D47BC File Offset: 0x000D37BC
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002E94 RID: 11924
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002E95 RID: 11925
		private readonly ConfigurationProperty authenticationModules = new ConfigurationProperty(null, typeof(AuthenticationModuleElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
