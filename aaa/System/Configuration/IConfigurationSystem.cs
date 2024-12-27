using System;
using System.Runtime.InteropServices;

namespace System.Configuration
{
	// Token: 0x020006F9 RID: 1785
	[ComVisible(false)]
	public interface IConfigurationSystem
	{
		// Token: 0x0600370B RID: 14091
		object GetConfig(string configKey);

		// Token: 0x0600370C RID: 14092
		void Init();
	}
}
