using System;
using System.Configuration.Internal;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x02000034 RID: 52
	internal static class ConfigurationManagerHelperFactory
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0001030C File Offset: 0x0000F30C
		internal static IConfigurationManagerHelper Instance
		{
			get
			{
				if (ConfigurationManagerHelperFactory.s_instance == null)
				{
					ConfigurationManagerHelperFactory.s_instance = ConfigurationManagerHelperFactory.CreateConfigurationManagerHelper();
				}
				return ConfigurationManagerHelperFactory.s_instance;
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00010324 File Offset: 0x0000F324
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.MemberAccess)]
		private static IConfigurationManagerHelper CreateConfigurationManagerHelper()
		{
			return TypeUtil.CreateInstance<IConfigurationManagerHelper>("System.Configuration.Internal.ConfigurationManagerHelper, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
		}

		// Token: 0x04000274 RID: 628
		private const string ConfigurationManagerHelperTypeString = "System.Configuration.Internal.ConfigurationManagerHelper, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		// Token: 0x04000275 RID: 629
		private static IConfigurationManagerHelper s_instance;
	}
}
