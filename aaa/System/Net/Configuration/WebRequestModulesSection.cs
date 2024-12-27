using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200066F RID: 1647
	public sealed class WebRequestModulesSection : ConfigurationSection
	{
		// Token: 0x060032E6 RID: 13030 RVA: 0x000D77CD File Offset: 0x000D67CD
		public WebRequestModulesSection()
		{
			this.properties.Add(this.webRequestModules);
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x000D780C File Offset: 0x000D680C
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", new object[] { "webRequestModules" }), ex);
			}
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x000D7868 File Offset: 0x000D6868
		protected override void InitializeDefault()
		{
			this.WebRequestModules.Add(new WebRequestModuleElement("https:", typeof(HttpRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("http:", typeof(HttpRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("file:", typeof(FileWebRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("ftp:", typeof(FtpWebRequestCreator)));
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x060032E9 RID: 13033 RVA: 0x000D78F1 File Offset: 0x000D68F1
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x060032EA RID: 13034 RVA: 0x000D78F9 File Offset: 0x000D68F9
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public WebRequestModuleElementCollection WebRequestModules
		{
			get
			{
				return (WebRequestModuleElementCollection)base[this.webRequestModules];
			}
		}

		// Token: 0x04002F6F RID: 12143
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F70 RID: 12144
		private readonly ConfigurationProperty webRequestModules = new ConfigurationProperty(null, typeof(WebRequestModuleElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
