using System;
using System.Collections.Specialized;

namespace System.Configuration
{
	// Token: 0x020006ED RID: 1773
	public sealed class ConfigurationSettings
	{
		// Token: 0x060036C6 RID: 14022 RVA: 0x000E9941 File Offset: 0x000E8941
		private ConfigurationSettings()
		{
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x060036C7 RID: 14023 RVA: 0x000E9949 File Offset: 0x000E8949
		[Obsolete("This method is obsolete, it has been replaced by System.Configuration!System.Configuration.ConfigurationManager.AppSettings")]
		public static NameValueCollection AppSettings
		{
			get
			{
				return ConfigurationManager.AppSettings;
			}
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x000E9950 File Offset: 0x000E8950
		[Obsolete("This method is obsolete, it has been replaced by System.Configuration!System.Configuration.ConfigurationManager.GetSection")]
		public static object GetConfig(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}
	}
}
