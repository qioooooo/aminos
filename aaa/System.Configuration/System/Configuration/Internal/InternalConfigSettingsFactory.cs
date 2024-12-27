using System;

namespace System.Configuration.Internal
{
	// Token: 0x020000C4 RID: 196
	internal sealed class InternalConfigSettingsFactory : IInternalConfigSettingsFactory
	{
		// Token: 0x0600076F RID: 1903 RVA: 0x00020273 File Offset: 0x0001F273
		private InternalConfigSettingsFactory()
		{
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0002027B File Offset: 0x0001F27B
		void IInternalConfigSettingsFactory.SetConfigurationSystem(IInternalConfigSystem configSystem, bool initComplete)
		{
			ConfigurationManager.SetConfigurationSystem(configSystem, initComplete);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00020284 File Offset: 0x0001F284
		void IInternalConfigSettingsFactory.CompleteInit()
		{
			ConfigurationManager.CompleteConfigInit();
		}
	}
}
