using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200066A RID: 1642
	public sealed class WebProxyScriptElement : ConfigurationElement
	{
		// Token: 0x060032C3 RID: 12995 RVA: 0x000D740C File Offset: 0x000D640C
		public WebProxyScriptElement()
		{
			this.properties.Add(this.downloadTimeout);
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x000D7480 File Offset: 0x000D6480
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
				throw new ConfigurationErrorsException(SR.GetString("net_config_element_permission", new object[] { "webProxyScript" }), ex);
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x060032C5 RID: 12997 RVA: 0x000D74DC File Offset: 0x000D64DC
		// (set) Token: 0x060032C6 RID: 12998 RVA: 0x000D74EF File Offset: 0x000D64EF
		[ConfigurationProperty("downloadTimeout", DefaultValue = "00:01:00")]
		public TimeSpan DownloadTimeout
		{
			get
			{
				return (TimeSpan)base[this.downloadTimeout];
			}
			set
			{
				base[this.downloadTimeout] = value;
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x060032C7 RID: 12999 RVA: 0x000D7503 File Offset: 0x000D6503
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F68 RID: 12136
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F69 RID: 12137
		private readonly ConfigurationProperty downloadTimeout = new ConfigurationProperty("downloadTimeout", typeof(TimeSpan), TimeSpan.FromMinutes(1.0), null, new TimeSpanValidator(new TimeSpan(0, 0, 0), TimeSpan.MaxValue, false), ConfigurationPropertyOptions.None);
	}
}
