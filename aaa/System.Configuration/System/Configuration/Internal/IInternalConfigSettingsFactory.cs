using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Internal
{
	// Token: 0x020000BE RID: 190
	[ComVisible(false)]
	public interface IInternalConfigSettingsFactory
	{
		// Token: 0x06000719 RID: 1817
		void SetConfigurationSystem(IInternalConfigSystem internalConfigSystem, bool initComplete);

		// Token: 0x0600071A RID: 1818
		void CompleteInit();
	}
}
