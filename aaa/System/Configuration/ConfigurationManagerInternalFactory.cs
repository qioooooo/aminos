using System;
using System.Configuration.Internal;

namespace System.Configuration
{
	// Token: 0x020006EC RID: 1772
	internal static class ConfigurationManagerInternalFactory
	{
		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x060036C5 RID: 14021 RVA: 0x000E991F File Offset: 0x000E891F
		internal static IConfigurationManagerInternal Instance
		{
			get
			{
				if (ConfigurationManagerInternalFactory.s_instance == null)
				{
					ConfigurationManagerInternalFactory.s_instance = (IConfigurationManagerInternal)TypeUtil.CreateInstanceWithReflectionPermission("System.Configuration.Internal.ConfigurationManagerInternal, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				}
				return ConfigurationManagerInternalFactory.s_instance;
			}
		}

		// Token: 0x04003196 RID: 12694
		private const string ConfigurationManagerInternalTypeString = "System.Configuration.Internal.ConfigurationManagerInternal, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		// Token: 0x04003197 RID: 12695
		private static IConfigurationManagerInternal s_instance;
	}
}
