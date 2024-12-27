using System;
using System.Configuration;

namespace System.Web.Configuration
{
	// Token: 0x020001C9 RID: 457
	internal class ConfigsHelper
	{
		// Token: 0x060019FE RID: 6654 RVA: 0x0007AA04 File Offset: 0x00079A04
		internal static void GetRegistryStringAttribute(ref string val, ConfigurationElement config, string propName)
		{
			if (!HandlerBase.CheckAndReadRegistryValue(ref val, false))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_registry_config"), config.ElementInformation.Properties[propName].Source, config.ElementInformation.Properties[propName].LineNumber);
			}
		}
	}
}
