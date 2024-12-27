using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Security;

namespace System.Web.Configuration
{
	// Token: 0x020001FE RID: 510
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpModulesSection : ConfigurationSection
	{
		// Token: 0x06001BC9 RID: 7113 RVA: 0x0007FF12 File Offset: 0x0007EF12
		static HttpModulesSection()
		{
			HttpModulesSection._properties.Add(HttpModulesSection._propHttpModules);
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001BCB RID: 7115 RVA: 0x0007FF4C File Offset: 0x0007EF4C
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpModulesSection._properties;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001BCC RID: 7116 RVA: 0x0007FF53 File Offset: 0x0007EF53
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public HttpModuleActionCollection Modules
		{
			get
			{
				return (HttpModuleActionCollection)base[HttpModulesSection._propHttpModules];
			}
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x0007FF68 File Offset: 0x0007EF68
		internal HttpModuleCollection CreateModules()
		{
			HttpModuleCollection httpModuleCollection = new HttpModuleCollection();
			foreach (object obj in this.Modules)
			{
				HttpModuleAction httpModuleAction = (HttpModuleAction)obj;
				httpModuleCollection.AddModule(httpModuleAction.Entry.ModuleName, httpModuleAction.Entry.Create());
			}
			httpModuleCollection.AddModule("DefaultAuthentication", new DefaultAuthenticationModule());
			return httpModuleCollection;
		}

		// Token: 0x04001883 RID: 6275
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001884 RID: 6276
		private static readonly ConfigurationProperty _propHttpModules = new ConfigurationProperty(null, typeof(HttpModuleActionCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
