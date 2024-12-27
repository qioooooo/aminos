using System;
using System.Net.Configuration;

namespace System.Configuration.Internal
{
	// Token: 0x0200071E RID: 1822
	internal sealed class ConfigurationManagerHelper : IConfigurationManagerHelper
	{
		// Token: 0x060037AC RID: 14252 RVA: 0x000EBE3E File Offset: 0x000EAE3E
		private ConfigurationManagerHelper()
		{
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x000EBE46 File Offset: 0x000EAE46
		void IConfigurationManagerHelper.EnsureNetConfigLoaded()
		{
			SettingsSection.EnsureConfigLoaded();
		}
	}
}
